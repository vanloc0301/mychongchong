namespace SkyMap.Net.Util
{
    using Aspose.Words;
    using Aspose.Words.Rendering;

    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing.Printing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml.Xsl;
    using Aspose.Words.Reporting;

    public static class PrintHelper
    {
        private static Aspose.Words.License license;

        private static void DocumentPrintOut(Microsoft.Office.Interop.Word.Application oWordApplic, Microsoft.Office.Interop.Word.Document doc)
        {
            object append = Missing.Value;
            object background = true;
            object wdPrintAllDocument = Microsoft.Office.Interop.Word.WdPrintOutRange.wdPrintAllDocument;
            object copies = 1;
            object wdPrintAllPages = Microsoft.Office.Interop.Word.WdPrintOutPages.wdPrintAllPages;
            object printToFile = false;
            object collate = false;
            object obj9 = append;
            object manualDuplexPrint = false;
            object printZoomColumn = 1;
            object printZoomRow = 1;
            doc.PrintOut(ref background, ref append, ref wdPrintAllDocument, ref append, ref append, ref append, ref append, ref copies, ref append, ref wdPrintAllPages, ref printToFile, ref collate, ref append, ref manualDuplexPrint, ref printZoomColumn, ref printZoomRow, ref append, ref append);
            object wdDoNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            object wdOriginalDocumentFormat = Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
            oWordApplic.Quit(ref wdDoNotSaveChanges, ref wdOriginalDocumentFormat, ref append);
            Marshal.ReleaseComObject(doc);
            Marshal.ReleaseComObject(oWordApplic);
            doc = null;
            oWordApplic = null;
        }

        private static void InternalWrodReportExecute(Aspose.Words.Document doc, DataSet dataSource, PrintBySelf printByself, FormatColumnValue formatColumnValue)
        {
            foreach (Aspose.Words.Bookmark bookmark in doc.Range.Bookmarks)
            {
            }
            doc.MailMerge.MergeField += delegate (object sender, MergeFieldEventArgs e) {
                string text = string.Empty;
                bool flag = false;
                try
                {
                    if (printByself != null)
                    {
                        flag = printByself(e.FieldName, ref text);
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
                if (!flag && ((formatColumnValue != null) && (e.FieldValue != null)))
                {
                    text = formatColumnValue(e.FieldValue, e.FieldValue.GetType(), e.FieldName);
                    flag = true;
                }
                if (flag)
                {
                    e.Text = text;
                }
            };
            doc.MailMerge.MergeImageField += delegate (object sender, MergeImageFieldEventArgs e) {
                if (e.FieldValue != null)
                {
                    MemoryStream stream = new MemoryStream((byte[]) e.FieldValue);
                    e.ImageStream = stream;
                }
            };
            List<string> list = new List<string>(doc.MailMerge.GetFieldNames());
            foreach (DataTable table in dataSource.Tables)
            {
                if (table.Rows.Count > 0)
                {
                    if (list.Contains(string.Format("TableStart:{0}", table.TableName)))
                    {
                        doc.MailMerge.ExecuteWithRegions(table);
                    }
                    else
                    {
                        doc.MailMerge.Execute(table);
                    }
                }
            }
        }

        public static bool PrintByReportVistor(string reportVistorTypeName, DataSet ds, byte[] reportData, object caller)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(reportVistorTypeName) && (reportVistorTypeName.IndexOf(",") > 0))
            {
                
                System.Type type = System.Type.GetType(reportVistorTypeName, false);
                if (type == null)
                {
                    return flag;
                }
                IReportVistor vistor = Activator.CreateInstance(type) as IReportVistor;
                if (vistor == null)
                {
                    return flag;
                }
                try
                {
                    vistor.Caller = caller;
                    vistor.Execute(reportData, ds);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    MessageHelper.ShowInfo(exception.Message);
                }
                finally
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static void PrintOrShowRDLC(string name, bool preview, byte[] reportData, DataSet ds, PrintEventHandler end_print, PrinterSettings printerSetting, Action<ReportViewDialog> action)
        {
            Dictionary<string, object> reportDataSource = new Dictionary<string, object>();
            foreach (DataTable table in ds.Tables)
            {
                reportDataSource.Add(table.TableName, table);
            }
            using (MemoryStream stream = new MemoryStream(reportData))
            {
                if (preview)
                {
                    ShowRDLC(name, stream, reportDataSource, action, end_print);
                }
                else
                {
                    PrintRDLC(printerSetting, end_print, stream, reportDataSource);
                }
            }
        }

        public static string PrintRDLC(PrintEventHandler end_print, string templetFile, Dictionary<string, object> reportDataSources)
        {
            return RDLCPrintInstance.Execute(null, end_print, templetFile, reportDataSources);
        }

        public static string PrintRDLC(PrinterSettings printerSetting, PrintEventHandler end_print, Stream stream, Dictionary<string, object> reportDataSources)
        {
            return RDLCPrintInstance.Execute(printerSetting, end_print, stream, reportDataSources);
        }

        public static string PrintRDLC(PrinterSettings printerSetting, PrintEventHandler end_print, string templetFile, Dictionary<string, object> reportDataSources)
        {
            return RDLCPrintInstance.Execute(printerSetting, end_print, templetFile, reportDataSources);
        }

        public static string PrintRDLC(PrinterSettings printerSetting, PrintEventHandler end_print, byte[] bytes, Dictionary<string, object> reportDataSources)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return PrintRDLC(printerSetting, end_print, stream, reportDataSources);
            }
        }

        public static void PrintWord(string file, bool preview, bool write)
        {
            DoWorkEventHandler handler = null;
            if (File.Exists(file))
            {
                BackgroundWorker worker = new BackgroundWorker();
                if (handler == null)
                {
                    handler = delegate (object sender, DoWorkEventArgs e) {
                        WaitDialogHelper.Show();
                        try
                        {
                            object fileName = file;
                            object readOnly = false;
                            object visible = true;
                            object confirmConversions = Missing.Value;
                            Microsoft.Office.Interop.Word.Application oWordApplic = new Microsoft.Office.Interop.Word.ApplicationClass();
                            Microsoft.Office.Interop.Word.Document doc = oWordApplic.Documents.Open(ref fileName, ref confirmConversions, ref readOnly, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref visible, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions);
                            ShowOrPrintDocumnet(preview, write, ref confirmConversions, ref oWordApplic, ref doc);
                        }
                        finally
                        {
                            WaitDialogHelper.Close();
                        }
                    };
                }
                worker.DoWork += handler;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PrintHelper.RunPrintWordWorkerCompleted);
                worker.RunWorkerAsync();
            }
        }

        public static void PrintWord(byte[] docBytes, DataSet dataset, bool preview, PrintBySelf printByself, FormatColumnValue formatColumnValue, bool write)
        {
            PrintWord(docBytes, dataset.Tables[0], preview, printByself, formatColumnValue, write);
        }

        public static void PrintWord(byte[] docBytes, DataTable dataSource, bool preview, PrintBySelf printByself, FormatColumnValue formatColumnValue, bool write)
        {
            PrintWord(Path.GetTempFileName(), docBytes, dataSource, preview, printByself, formatColumnValue, write);
        }

        public static void PrintWord(string targetFile, byte[] docBytes, DataSet dataSource, bool preview, PrintBySelf printByself, FormatColumnValue formatColumnValue, bool write)
        {
            PrintWord(targetFile, docBytes, dataSource.Tables[0], preview, printByself, formatColumnValue, write);
        }

        public static void PrintWord(string targetFile, byte[] docBytes, DataTable dataSource, bool preview, PrintBySelf printByself, FormatColumnValue formatColumnValue, bool write)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate (object sender, DoWorkEventArgs e) {
                WaitDialogHelper.Show();
                try
                {
                    object fileName = targetFile;
                    object readOnly = false;
                    object visible = true;
                    object confirmConversions = Missing.Value;
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("准备写临时WORD文件");
                    }
                    File.WriteAllBytes((string) fileName, docBytes);
                    Microsoft.Office.Interop.Word.Application oWordApplic = new Microsoft.Office.Interop.Word.ApplicationClass();
                    Microsoft.Office.Interop.Word.Document doc = oWordApplic.Documents.Open(ref fileName, ref confirmConversions, ref readOnly, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref visible, ref confirmConversions, ref confirmConversions, ref confirmConversions, ref confirmConversions);
                    Microsoft.Office.Interop.Word.Bookmarks bookmarks = doc.Bookmarks;
                    foreach (Microsoft.Office.Interop.Word.Bookmark bookmark in bookmarks)
                    {
                        string text = string.Empty;
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.InfoFormatted("Word标签:{0}", new object[] { bookmark.Name });
                        }
                        bool flag = false;
                        try
                        {
                            if (printByself != null)
                            {
                                flag = printByself(bookmark.Name, ref text);
                            }
                        }
                        catch (Exception exception)
                        {
                            LoggingService.Error(exception);
                        }
                        if (!flag && dataSource.Columns.Contains(bookmark.Name))
                        {
                            if (formatColumnValue != null)
                            {
                                text = formatColumnValue(dataSource.Rows[0][bookmark.Name], dataSource.Columns[bookmark.Name].DataType, bookmark.Name);
                            }
                            else
                            {
                                text = dataSource.Rows[0][bookmark.Name].ToString();
                            }
                            flag = true;
                        }
                        if (flag)
                        {
                            bookmark.Range.Text = text;
                        }
                    }
                    doc.Fields.Update();
                    ShowOrPrintDocumnet(preview, write, ref confirmConversions, ref oWordApplic, ref doc);
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            };
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PrintHelper.RunPrintWordWorkerCompleted);
            worker.RunWorkerAsync();
        }

        public static void PrintWordEx(byte[] docBytes, DataSet dataSource, bool preview, PrintBySelf printByself, FormatColumnValue formatColumnValue, bool write)
        {
            PrintWordEx(Path.GetTempFileName() + ".doc", docBytes, dataSource, preview, printByself, formatColumnValue, write);
        }

        public static void PrintWordEx(string targetFile, byte[] docBytes, DataSet dataSource, bool preview, PrintBySelf printByself, FormatColumnValue formatColumnValue, bool write)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate (object sender, DoWorkEventArgs e) {
                WaitDialogHelper.Show();
                try
                {
                    using (MemoryStream stream = new MemoryStream(docBytes))
                    {
                        if (license == null)
                        {
                            license = new Aspose.Words.License();
                            license.SetLicense("Aspose.lic");
                        }
                        Aspose.Words.Document doc = new Aspose.Words.Document(stream);
                        InternalWrodReportExecute(doc, dataSource, printByself, formatColumnValue);
                        if (!write)
                        {
                            doc.Protect(ProtectionType.ReadOnly, "sbdwlfty");
                        }
                        if (preview)
                        {
                            doc.Save(targetFile);
                            Process.Start(targetFile);
                        }
                        else
                        {
                            new AsposeWordsPrintDocument(doc).Print();
                        }
                    }
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            };
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PrintHelper.RunPrintWordWorkerCompleted);
            worker.RunWorkerAsync();
        }

        public static void PrintXLST(DataSet dataset, byte[] bytes)
        {
            string fileName = string.Format("{0}.xml", Path.GetTempFileName());
            dataset.WriteXml(fileName);
            PrintXLST(fileName, bytes);
        }

        public static void PrintXLST(string inputfile, byte[] bytes)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate (object sender, DoWorkEventArgs e) {
                WaitDialogHelper.Show();
                try
                {
                    if ((bytes == null) || (bytes.Length == 0))
                    {
                        throw new ArgumentNullException("bytes");
                    }
                    string path = string.Format("{0}.xslt", Path.GetTempFileName());
                    File.WriteAllBytes(path, bytes);
                    string outputfile = string.Format("{0}.doc", Path.GetTempFileName());
                    object tempFileName = Path.GetTempFileName();
                    XslTransform transform = new XslTransform();
                    transform.Load(path);
                    transform.Transform(inputfile, outputfile);
                    Process.Start(outputfile);
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            };
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PrintHelper.RunPrintWordWorkerCompleted);
            worker.RunWorkerAsync();
        }

        private static void RunPrintWordWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LoggingService.Error("输出报表时出错:", e.Error);
                MessageHelper.ShowInfo("报表输出错误:{0}", e.Error.Message);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("报表异步加载完成");
            }
        }

        private static void ShowOrPrintDocumnet(bool preview, bool write, ref object missing, ref Microsoft.Office.Interop.Word.Application oWordApplic, ref Microsoft.Office.Interop.Word.Document doc)
        {
            if (!write)
            {
                object password = "sbdwlfty";
                doc.Protect(Microsoft.Office.Interop.Word.WdProtectionType.wdAllowOnlyReading, ref missing, ref password, ref missing, ref missing);
            }
            oWordApplic.Visible = true;
            Marshal.ReleaseComObject(doc);
            Marshal.ReleaseComObject(oWordApplic);
            doc = null;
            oWordApplic = null;
        }

        public static void ShowRDLC(string name, Stream stream, Dictionary<string, object> reportDataSource, Action<ReportViewDialog> action)
        {
            ShowRDLC(name, stream, reportDataSource, action, null);
        }

        public static void ShowRDLC(string name, Stream stream, Dictionary<string, object> reportDataSource, Action<ReportViewDialog> action, PrintEventHandler printHandler)
        {
            using (ReportViewDialog dialog = new ReportViewDialog())
            {
                dialog.LoadReportDefinition(stream);
                dialog.Text = name;
                foreach (KeyValuePair<string, object> pair in reportDataSource)
                {
                    dialog.AddDataSource(pair.Key, pair.Value);
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("添加报表数据源：{0}", new object[] { pair.Key });
                    }
                }
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.Debug("执行自定义报表处理过程");
                }
                if (action != null)
                {
                    action(dialog);
                }
                dialog.WindowState = FormWindowState.Maximized;
                if (printHandler != null)
                {
                    dialog.Print += printHandler;
                }
                dialog.ShowDialog();
                try
                {
                    dialog.Close();
                }
                catch
                {
                }
            }
        }

        public static void ShowRDLC(string name, byte[] bytes, Dictionary<string, object> reportDataSource, Action<ReportViewDialog> action, PrintEventHandler printHandler)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                ShowRDLC(name, stream, reportDataSource, action, printHandler);
            }
        }

        private static RDLCPrint RDLCPrintInstance
        {
            get
            {
                return new RDLCPrint();
            }
        }
    }
}

