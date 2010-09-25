namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Tools.DataForms;
    using System;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;
    using WordDataSetTemplateEditor;

    public class WordTempletEditCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            TempletPrintNode owner = (TempletPrintNode) this.Owner;
            if (owner != null)
            {
                TempletPrint domainObject = owner.DomainObject;
                if (domainObject.Type == ".wzx")
                {
                    DataSet reportDataSource = domainObject.GetReportDataSource(true, null);
                    string path = null;
                    if ((domainObject.Data != null) && (domainObject.Data.Length > 0))
                    {
                        byte[] data = domainObject.Data;
                        try
                        {
                            data = ZipHelper.GZDecompress(domainObject.Data);
                        }
                        catch
                        {
                        }
                        path = Path.GetTempFileName();
                        File.WriteAllBytes(path, data);
                    }
                    frmWordControl control = new frmWordControl(reportDataSource, path);
                    if (control.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                    {
                        domainObject.Data = ZipHelper.GZCompress(File.ReadAllBytes(control.TemplateFileName));
                        PropertyGridEditPanel.UnitOfWork.RegisterDirty(domainObject);
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("只能编辑类型为：.wzx的报表模板!");
                }
            }
        }
    }
}

