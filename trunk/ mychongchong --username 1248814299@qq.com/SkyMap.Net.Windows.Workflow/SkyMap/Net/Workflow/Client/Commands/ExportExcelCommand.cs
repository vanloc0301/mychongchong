namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.XtraGrid.Columns;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Windows.Forms;

    public class ExportExcelCommand : AbstractMenuCommand
    {
        private void OpenFile(string fileName)
        {
            if (MessageHelper.ShowYesNoInfo("立即查看该文档吗？") == DialogResult.Yes)
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    MessageHelper.ShowInfo("找不到合适的程序打开该文档。");
                }
            }
        }

        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if (owner != null)
            {
                string fileName = this.ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
                if (fileName != "")
                {
                    DataTable dt = new DataTable();
                    List<GridColumn> list = new List<GridColumn>();
                    foreach (GridColumn column in owner.view.Columns)
                    {
                        if (column.VisibleIndex > 0)
                        {
                            dt.Columns.Add(column.Caption);
                            list.Add(column);
                        }
                    }
                    for (int i = 0; i < owner.view.RowCount; i++)
                    {
                        DataRow row = dt.NewRow();
                        int num2 = 0;
                        foreach (GridColumn column in list)
                        {
                            row[num2] = owner.view.GetRowCellDisplayText(i, column);
                            num2++;
                        }
                        dt.Rows.Add(row);
                    }
                    ExportHelper.ExportToExcel(dt, fileName);
                    this.OpenFile(fileName);
                }
            }
        }

        protected string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            string str = DateTime.Now.ToLongDateString();
            int startIndex = str.LastIndexOf(".") + 1;
            if (startIndex > 0)
            {
                str = str.Substring(startIndex, str.Length - startIndex);
            }
            dialog.Title = "导出到..." + title;
            dialog.FileName = str;
            dialog.Filter = filter;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return "";
        }
    }
}

