namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class Print : AbstractMenuCommand
    {
        public override void Run()
        {
            IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
            if (activeWorkbenchWindow != null)
            {
                if (activeWorkbenchWindow.ViewContent is IPrintable)
                {
                    PrintDocument printDocument = ((IPrintable) activeWorkbenchWindow.ViewContent).PrintDocument;
                    if (printDocument != null)
                    {
                        using (PrintDialog dialog = new PrintDialog())
                        {
                            dialog.Document = printDocument;
                            dialog.AllowSomePages = true;
                            if (dialog.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                            {
                                printDocument.Print();
                            }
                        }
                    }
                    else
                    {
                        MessageService.ShowError("${res:SkyMap.Net.Commands.Print.CreatePrintDocumentError}");
                    }
                }
                else
                {
                    MessageService.ShowError("${res:SkyMap.Net.Commands.Print.CantPrintWindowContentError}");
                }
            }
        }
    }
}

