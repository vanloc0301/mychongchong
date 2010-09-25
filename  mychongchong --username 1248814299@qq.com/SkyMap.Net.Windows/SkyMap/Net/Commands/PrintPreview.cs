namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class PrintPreview : AbstractMenuCommand
    {
        public override void Run()
        {
            try
            {
                IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
                if ((activeWorkbenchWindow != null) && (activeWorkbenchWindow.ViewContent is IPrintable))
                {
                    using (PrintDocument document = ((IPrintable) activeWorkbenchWindow.ViewContent).PrintDocument)
                    {
                        if (document != null)
                        {
                            PrintPreviewDialog dialog = new PrintPreviewDialog();
                            dialog.Owner = (Form) WorkbenchSingleton.Workbench;
                            dialog.TopMost = true;
                            dialog.Document = document;
                            dialog.Show();
                        }
                        else
                        {
                            MessageService.ShowError("${res:SkyMap.Net.Commands.Print.CreatePrintDocumentError}");
                        }
                    }
                }
            }
            catch (InvalidPrinterException)
            {
            }
        }
    }
}

