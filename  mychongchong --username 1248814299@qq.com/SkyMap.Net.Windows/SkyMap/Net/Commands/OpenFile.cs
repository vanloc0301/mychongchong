namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class OpenFile : AbstractMenuCommand
    {
        public override void Run()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.AddExtension = true;
                string[] strArray = (string[]) AddInTree.GetTreeNode("/Workbench/FileFilter").BuildChildItems(this).ToArray(typeof(string));
                dialog.Filter = string.Join("|", strArray);
                if (true)
                {
                    IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
                    if (activeWorkbenchWindow != null)
                    {
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (strArray[i].IndexOf(Path.GetExtension((activeWorkbenchWindow.ViewContent.FileName == null) ? activeWorkbenchWindow.ViewContent.UntitledName : activeWorkbenchWindow.ViewContent.FileName)) >= 0)
                            {
                                dialog.FilterIndex = i + 1;
                                break;
                            }
                        }
                    }
                }
                dialog.Multiselect = true;
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                {
                    foreach (string str in dialog.FileNames)
                    {
                        FileService.OpenFile(str);
                    }
                }
            }
        }
    }
}

