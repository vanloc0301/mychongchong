namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.XtraExport;
    using DevExpress.XtraGrid.Export;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Diagnostics;

    public class ExportExcel2Command : ExportExcelCommand
    {
        private void ExportTo(SmGridView view, IExportProvider provider)
        {
            try
            {
                try
                {
                    view.Columns["sel"].VisibleIndex = -1;
                }
                catch
                {
                }
                BaseExportLink link = view.CreateExportLink(provider);
                (link as GridViewExportLink).ExpandAll = true;
                (link as GridViewExportLink).ExportDetails = true;
                link.ExportTo(true);
                view.Columns[0].VisibleIndex = 0;
            }
            finally
            {
            }
        }

        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if (owner != null)
            {
                string fileName = base.ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
                if (fileName != "")
                {
                    ExportHtmlProvider provider = new ExportHtmlProvider(fileName);
                    this.ExportTo(owner.view, provider);
                    Process.Start(fileName);
                }
            }
        }
    }
}

