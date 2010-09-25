namespace SkyMap.Net.Util
{
    using Microsoft.Reporting.WinForms;
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;
    using System.IO;
    using System.Text;

    public class RDLCPrint : IDisposable
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;

        private PrintDocument CreatePrintDocument(PrinterSettings printerSetting, ref string msg)
        {
            LoggingService.Debug("创建PrintDocument...");
            PrintDocument document = new PrintDocument();
            if (printerSetting != null)
            {
                LoggingService.Debug("设置打印机置...");
                document.PrinterSettings = printerSetting;
            }
            LoggingService.Debug("检查打印机设置是否有效...");
            if (!document.PrinterSettings.IsValid)
            {
                msg = "系统没有安装打印机或者打印设置无效.";
            }
            return document;
        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream item = new FileStream(Path.GetTempFileName(), FileMode.Create);
            this.m_streams.Add(item);
            return item;
        }

        public void Dispose()
        {
            if (this.m_streams != null)
            {
                foreach (Stream stream in this.m_streams)
                {
                    stream.Close();
                }
                this.m_streams = null;
            }
        }

        public string Execute(PrintEventHandler end_print, string templetFile, Dictionary<string, object> reportDataSources)
        {
            return this.Execute(null, end_print, templetFile, reportDataSources);
        }

        public string Execute(PrinterSettings printerSetting, PrintEventHandler end_print, Stream stream, Dictionary<string, object> reportDataSources)
        {
            string msg = string.Empty;
            PrintDocument printDoc = this.CreatePrintDocument(printerSetting, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return msg;
            }
            LoggingService.Debug("检查打印模板文件是否存在...");
            if ((stream == null) || (stream.Length == 0L))
            {
                LoggingService.Warn("模板是空值或无内容");
                return "报表模板不存在!";
            }
            LoggingService.Debug("准备生成打印流...");
            LocalReport report = new LocalReport();
            report.LoadReportDefinition(stream);
            return this.InternalExecute(printDoc, report, end_print, reportDataSources);
        }

        public string Execute(PrinterSettings printerSetting, PrintEventHandler end_print, string templetFile, Dictionary<string, object> reportDataSources)
        {
            string msg = string.Empty;
            PrintDocument printDoc = this.CreatePrintDocument(printerSetting, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return msg;
            }
            LoggingService.Debug("检查打印模板文件是否存在...");
            if (!File.Exists(templetFile))
            {
                LoggingService.WarnFormatted("没有报表模板{0}", new object[] { templetFile });
                return "报表模板文件不存在!";
            }
            LoggingService.Debug("准备生成打印流...");
            LocalReport report = new LocalReport();
            report.ReportPath = templetFile;
            return this.InternalExecute(printDoc, report, end_print, reportDataSources);
        }

        private void Export(LocalReport report)
        {
            Warning[] warningArray;
            string deviceInfo = "<DeviceInfo>  <OutputFormat>EMF</OutputFormat>  <PageWidth>8.5in</PageWidth>  <PageHeight>11in</PageHeight>  <MarginTop>0.25in</MarginTop>  <MarginLeft>0.25in</MarginLeft>  <MarginRight>0.25in</MarginRight>  <MarginBottom>0.25in</MarginBottom></DeviceInfo>";
            this.m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, new CreateStreamCallback(this.CreateStream), out warningArray);
            foreach (Stream stream in this.m_streams)
            {
                stream.Position = 0L;
            }
        }

        private string InternalExecute(PrintDocument printDoc, LocalReport report, PrintEventHandler end_print, Dictionary<string, object> reportDataSources)
        {
            foreach (KeyValuePair<string, object> pair in reportDataSources)
            {
                report.DataSources.Add(new ReportDataSource(pair.Key, pair.Value));
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("添加报表数据源：{0}", new object[] { pair.Key });
                }
            }
            this.Export(report);
            this.m_currentPageIndex = 0;
            if ((this.m_streams == null) || (this.m_streams.Count == 0))
            {
                return "无法生成要打印的报表！";
            }
            LoggingService.Debug("生成了打印流...");
            printDoc.PrintPage += new PrintPageEventHandler(this.PrintPage);
            if (end_print != null)
            {
                printDoc.EndPrint += end_print;
            }
            try
            {
                LoggingService.Debug("开始打印了...");
                printDoc.Print();
                LoggingService.Debug("打印完成了...");
            }
            catch (Win32Exception exception)
            {
                LoggingService.Error(exception);
                return "连接不到打印机或打印机不能正常工作，请检查打印机！";
            }
            return string.Empty;
        }

        private void Print()
        {
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile image = new Metafile(this.m_streams[this.m_currentPageIndex]);
            ev.Graphics.DrawImage(image, ev.PageBounds);
            this.m_currentPageIndex++;
            ev.HasMorePages = this.m_currentPageIndex < this.m_streams.Count;
        }
    }
}

