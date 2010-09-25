namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Util;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.ZSTax;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;

    public class BillNode : ExtTreeNode
    {
        public string AddressPrefix;
        public string FormName;
        public string Logo;
        private static string printExe = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "") + Path.DirectorySeparatorChar + "ProPriTax.exe");
        public string ReportOID;
        public string SQL;
        public string TaxTypeOID;

        public override void ActivateItem()
        {
            this.MouseClick();
        }

        protected virtual DataSet GetReportDataSource(TempletPrint templetPrint, string projectId)
        {
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("{ProjectId}", projectId);
            return templetPrint.GetReportDataSource(true, vals);
        }

        private void LoadOldBill(string project_id, DataTable dt)
        {
            XmlDocument document = new XmlDocument();
            XmlElement newChild = document.CreateElement("project");
            newChild.SetAttribute("caption", base.Text);
            newChild.SetAttribute("logo", this.Logo);
            newChild.SetAttribute("SQL", this.SQL);
            newChild.SetAttribute("formname", this.FormName);
            newChild.SetAttribute("staffname", StaffEditCommand.StaffName);
            newChild.SetAttribute("deptname", DeptEditCommand.DeptName);
            newChild.SetAttribute("intx", IntXEditCommand.X.ToString());
            newChild.SetAttribute("inty", IntYEditCommand.Y.ToString());
            newChild.SetAttribute("addressprefix", this.AddressPrefix);
            newChild.SetAttribute("id", project_id);
            document.AppendChild(newChild);
            if ((dt != null) && (dt.Rows.Count == 1))
            {
                DataRow row = dt.Rows[0];
                foreach (DataColumn column in row.Table.Columns)
                {
                    XmlElement element2 = document.CreateElement("field");
                    element2.SetAttribute("name", column.ColumnName);
                    element2.SetAttribute("value", row[column].ToString());
                    newChild.AppendChild(element2);
                }
            }
            string tempFileName = Path.GetTempFileName();
            document.Save(tempFileName);
            if (File.Exists(printExe))
            {
                Process.Start(printExe, tempFileName);
            }
        }

        public void MouseClick()
        {
            DataTable dt = null;
            string str = string.Empty;
            if ((BoxService.CurrentBox != null) && (BoxService.CurrentBox is IWfBox))
            {
                IWfBox currentBox = BoxService.CurrentBox as IWfBox;
                try
                {
                    DataRow[] selectedRows = currentBox.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        DataRow row = selectedRows[0];
                        if (row.Table.Columns.Contains("project_id"))
                        {
                            str = row["project_id"].ToString();
                            if (string.IsNullOrEmpty(this.TaxTypeOID))
                            {
                                if (string.IsNullOrEmpty(this.ReportOID))
                                {
                                    string sql = string.Format(this.SQL, str);
                                    if (LoggingService.IsDebugEnabled)
                                    {
                                        LoggingService.DebugFormatted("Will Execute SQL:\r\n{0}", new object[] { sql });
                                    }
                                    dt = QueryHelper.ExecuteSql("SkyMap.Net.GTOA", string.Empty, sql);
                                }
                                else
                                {
                                    this.ShowTempletPrint(str);
                                    return;
                                }
                            }
                            if (PropertyService.Get<bool>("IsCancelSelectedItemAfterPrint", true))
                            {
                                currentBox.CancelTopSelectedRow();
                            }
                        }
                    }
                }
                catch (NotSelectException)
                {
                }
            }
            if (string.IsNullOrEmpty(this.TaxTypeOID))
            {
                this.LoadOldBill(str, dt);
            }
            else
            {
                TaxList list = PrintBillDialog.ShowBillDialog(str, this.TaxTypeOID, base.Text);
                if ((list != null) && LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("票据：{0} 状态：{1}", new object[] { list.Description, list.Status });
                }
            }
        }

        protected void PrintRDLC(TempletPrint templetPrint, string projectId)
        {
            WaitDialogHelper.Show();
            try
            {
                if ((templetPrint.Data != null) && (templetPrint.Data.Length > 0))
                {
                    DataSet reportDataSource = new DataSet();
                    if (!string.IsNullOrEmpty(templetPrint.Sql))
                    {
                        reportDataSource = this.GetReportDataSource(templetPrint, projectId);
                    }
                    using (MemoryStream stream = new MemoryStream(templetPrint.Data))
                    {
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        foreach (DataTable table in reportDataSource.Tables)
                        {
                            dictionary.Add(table.TableName, table);
                        }
                        if (templetPrint.PrintPreview)
                        {
                            PrintHelper.ShowRDLC(templetPrint.Name, stream, dictionary, null);
                        }
                        else
                        {
                            string text = PrintHelper.PrintRDLC(null, null, stream, dictionary);
                            if (text != null)
                            {
                                MessageHelper.ShowInfo(text);
                            }
                        }
                    }
                }
            }
            finally
            {
                WaitDialogHelper.Close();
            }
        }

        private void ShowTempletPrint(string projectId)
        {
            TempletPrint templetPrint = QueryHelper.Get<TempletPrint>("TempletPrint_" + this.ReportOID, this.ReportOID);
            if (templetPrint == null)
            {
                MessageHelper.ShowInfo("没有找到指定的报表模板:{0}", this.ReportOID);
            }
            else
            {
                LoggingService.DebugFormatted("将输出打印报表：{0},类型：{1}", new object[] { templetPrint.Name, templetPrint.Type });
                DataSet reportDataSource = this.GetReportDataSource(templetPrint, projectId);
                switch (templetPrint.Type)
                {
                    case ".doc":
                        PrintHelper.PrintWord(templetPrint.Data, reportDataSource, true, null, null, !templetPrint.PrintPreview);
                        return;

                    case ".xslt":
                        PrintHelper.PrintXLST(reportDataSource, templetPrint.Data);
                        return;

                    case ".rdlc":
                        PrintHelper.PrintOrShowRDLC(templetPrint.Name, templetPrint.PrintPreview, templetPrint.Data, reportDataSource, null, null, null);
                        return;
                }
                if (!PrintHelper.PrintByReportVistor(templetPrint.Type, reportDataSource, templetPrint.Data, null))
                {
                    throw new NotImplementedException("没有实现的报表打印方式:" + templetPrint.Type);
                }
            }
        }
    }
}

