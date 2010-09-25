namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Internal.ExternalTool;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    public class ToolMenuBuilder : ISubmenuBuilder
    {
        public ToolStripItem[] BuildSubmenu(Codon codon, object owner)
        {
            MenuCommand[] commandArray = new MenuCommand[ToolLoader.Tool.Count];
            for (int i = 0; i < ToolLoader.Tool.Count; i++)
            {
                MenuCommand command = new MenuCommand(ToolLoader.Tool[i].ToString(), new EventHandler(this.ToolEvt));
                command.Description = "Start tool " + string.Join(string.Empty, ToolLoader.Tool[i].ToString().Split(new char[] { '&' }));
                commandArray[i] = command;
            }
            return commandArray;
        }

        private void ProcessExitEvent(object sender, EventArgs e)
        {
            Process process = (Process) sender;
            string str = process.StandardOutput.ReadToEnd();
        }

        private void ToolEvt(object sender, EventArgs e)
        {
            MenuCommand command = (MenuCommand) sender;
            for (int i = 0; i < ToolLoader.Tool.Count; i++)
            {
                if (command.Text == ToolLoader.Tool[i].ToString())
                {
                    SkyMap.Net.Internal.ExternalTool.ExternalTool tool = ToolLoader.Tool[i];
                    IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
                    string path = (activeWorkbenchWindow == null) ? null : activeWorkbenchWindow.ViewContent.FileName;
                    SkyMap.Net.Core.StringParser.Properties["ItemPath"] = (path == null) ? string.Empty : path;
                    SkyMap.Net.Core.StringParser.Properties["ItemDir"] = (path == null) ? string.Empty : Path.GetDirectoryName(path);
                    SkyMap.Net.Core.StringParser.Properties["ItemFileName"] = (path == null) ? string.Empty : Path.GetFileName(path);
                    SkyMap.Net.Core.StringParser.Properties["ItemExt"] = (path == null) ? string.Empty : Path.GetExtension(path);
                    SkyMap.Net.Core.StringParser.Properties["StartupPath"] = Application.StartupPath;
                    string fileName = SkyMap.Net.Core.StringParser.Parse(tool.Command);
                    string arguments = SkyMap.Net.Core.StringParser.Parse(tool.Arguments);
                    if (tool.PromptForArguments)
                    {
                    }
                    try
                    {
                        ProcessStartInfo info;
                        if (((arguments == null) || (arguments.Length == 0)) || (arguments.Trim(new char[] { '"', ' ' }).Length == 0))
                        {
                            info = new ProcessStartInfo(fileName);
                        }
                        else
                        {
                            info = new ProcessStartInfo(fileName, arguments);
                        }
                        info.WorkingDirectory = SkyMap.Net.Core.StringParser.Parse(tool.InitialDirectory);
                        if (tool.UseOutputPad)
                        {
                            info.UseShellExecute = false;
                            info.RedirectStandardOutput = true;
                        }
                        Process process = new Process();
                        process.EnableRaisingEvents = true;
                        process.StartInfo = info;
                        if (tool.UseOutputPad)
                        {
                            process.Exited += new EventHandler(this.ProcessExitEvent);
                        }
                        process.Start();
                    }
                    catch (Exception exception)
                    {
                        MessageService.ShowError("${res:XML.MainMenu.ToolMenu.ExternalTools.ExecutionFailed} '" + fileName + " " + arguments + "'\n" + exception.Message);
                    }
                    break;
                }
            }
        }
    }
}

