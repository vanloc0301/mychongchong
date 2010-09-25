namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class FileService
    {
        private static SkyMap.Net.Core.RecentOpen recentOpen = null;

        public static  event EventHandler<FileEventArgs> FileRemoved;

        public static  event EventHandler<FileCancelEventArgs> FileRemoving;

        public static  event EventHandler<FileRenameEventArgs> FileRenamed;

        public static  event EventHandler<FileRenamingEventArgs> FileRenaming;

        public static  event EventHandler<FileEventArgs> FileReplaced;

        public static  event EventHandler<FileCancelEventArgs> FileReplacing;

        public static bool CheckDirectoryName(string name)
        {
            if (FileUtility.IsValidDirectoryName(name))
            {
                return true;
            }
            MessageService.ShowMessage(SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Commands.SaveFile.InvalidFileNameError}", new string[,] { { "FileName", name } }));
            return false;
        }

        public static bool CheckFileName(string fileName)
        {
            if (FileUtility.IsValidFileName(fileName))
            {
                return true;
            }
            MessageService.ShowMessage(SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Commands.SaveFile.InvalidFileNameError}", new string[,] { { "FileName", fileName } }));
            return false;
        }

        public static void FireFileReplaced(string fileName, bool isDirectory)
        {
            if (FileReplaced != null)
            {
                FileReplaced(null, new FileEventArgs(fileName, isDirectory));
            }
        }

        public static bool FireFileReplacing(string fileName, bool isDirectory)
        {
            FileCancelEventArgs e = new FileCancelEventArgs(fileName, isDirectory);
            if (FileReplacing != null)
            {
                FileReplacing(null, e);
            }
            return !e.Cancel;
        }

        public static IWorkbenchWindow GetOpenFile(string fileName)
        {
            if ((fileName != null) && (fileName.Length > 0))
            {
                foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    string str = content.IsUntitled ? content.UntitledName : content.FileName;
                    if ((str != null) && FileUtility.IsEqualFileName(fileName, str))
                    {
                        return content.WorkbenchWindow;
                    }
                }
            }
            return null;
        }

        public static IList<string> GetOpenFiles()
        {
            List<string> list = new List<string>();
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                string item = content.IsUntitled ? content.UntitledName : content.FileName;
                if (item != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public static bool IsOpen(string fileName)
        {
            return (GetOpenFile(fileName) != null);
        }

        public static IViewContent JumpToFilePosition(string fileName, int line, int column)
        {
            if ((fileName == null) || (fileName.Length == 0))
            {
                return null;
            }
            IWorkbenchWindow window = OpenFile(fileName);
            if (window == null)
            {
                return null;
            }
            IViewContent viewContent = window.ViewContent;
            if (viewContent is IPositionable)
            {
                window.SwitchView(0);
                ((IPositionable) viewContent).JumpTo(Math.Max(0, line), Math.Max(0, column));
            }
            return viewContent;
        }

        public static IWorkbenchWindow NewFile(string defaultName, string language, string content)
        {
            IDisplayBinding bindingPerLanguageName = DisplayBindingService.GetBindingPerLanguageName(language);
            if (bindingPerLanguageName == null)
            {
                throw new ApplicationException("Can't create display binding for language " + language);
            }
            IViewContent viewContent = bindingPerLanguageName.CreateContentForLanguage(language, content);
            if (viewContent == null)
            {
                LoggingService.Warn(string.Format("Created view content was null{3}DefaultName:{0}{3}Language:{1}{3}Content:{2}", new object[] { defaultName, language, content, Environment.NewLine }));
                return null;
            }
            viewContent.UntitledName = viewContent.GetHashCode() + "/" + defaultName;
            DisplayBindingService.AttachSubWindows(viewContent, false);
            WorkbenchSingleton.Workbench.ShowView(viewContent);
            return viewContent.WorkbenchWindow;
        }

        private static void OnFileRemoved(FileEventArgs e)
        {
            if (FileRemoved != null)
            {
                FileRemoved(null, e);
            }
        }

        private static void OnFileRemoving(FileCancelEventArgs e)
        {
            if (FileRemoving != null)
            {
                FileRemoving(null, e);
            }
        }

        private static void OnFileRenamed(FileRenameEventArgs e)
        {
            if (FileRenamed != null)
            {
                FileRenamed(null, e);
            }
        }

        private static void OnFileRenaming(FileRenamingEventArgs e)
        {
            if (FileRenaming != null)
            {
                FileRenaming(null, e);
            }
        }

        public static IWorkbenchWindow OpenFile(string fileName)
        {
            LoggingService.Info("Open file " + fileName);
            IWorkbenchWindow openFile = GetOpenFile(fileName);
            if (openFile != null)
            {
                openFile.SelectWindow();
                return openFile;
            }
            IDisplayBinding bindingPerFileName = DisplayBindingService.GetBindingPerFileName(fileName);
            if (bindingPerFileName == null)
            {
                throw new ApplicationException("Can't open " + fileName + ", no display codon found.");
            }
            if (FileUtility.ObservedLoad(new NamedFileOperationDelegate(new LoadFileWrapper(bindingPerFileName).Invoke), fileName) == FileOperationResult.OK)
            {
                RecentOpen.AddLastFile(fileName);
            }
            return GetOpenFile(fileName);
        }

        public static void RemoveFile(string fileName, bool isDirectory)
        {
            FileCancelEventArgs e = new FileCancelEventArgs(fileName, isDirectory);
            OnFileRemoving(e);
            if (!e.Cancel)
            {
                if (!e.OperationAlreadyDone)
                {
                    Exception exception;
                    if (isDirectory)
                    {
                        try
                        {
                            if (Directory.Exists(fileName))
                            {
                                Directory.Delete(fileName, true);
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            MessageService.ShowError(exception, "Can't remove directory " + fileName);
                        }
                    }
                    else
                    {
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            MessageService.ShowError(exception, "Can't remove file " + fileName);
                        }
                    }
                }
                OnFileRemoved(new FileEventArgs(fileName, isDirectory));
            }
        }

        public static bool RenameFile(string oldName, string newName, bool isDirectory)
        {
            if (FileUtility.IsEqualFileName(oldName, newName))
            {
                return false;
            }
            FileRenamingEventArgs e = new FileRenamingEventArgs(oldName, newName, isDirectory);
            OnFileRenaming(e);
            if (e.Cancel)
            {
                return false;
            }
            if (!e.OperationAlreadyDone)
            {
                try
                {
                    if (isDirectory && Directory.Exists(oldName))
                    {
                        if (Directory.Exists(newName))
                        {
                            MessageService.ShowMessage(SkyMap.Net.Core.StringParser.Parse("${res:Gui.ProjectBrowser.FileInUseError}"));
                            return false;
                        }
                        Directory.Move(oldName, newName);
                    }
                    else if (File.Exists(oldName))
                    {
                        if (File.Exists(newName))
                        {
                            MessageService.ShowMessage(SkyMap.Net.Core.StringParser.Parse("${res:Gui.ProjectBrowser.FileInUseError}"));
                            return false;
                        }
                        File.Move(oldName, newName);
                    }
                }
                catch (Exception exception)
                {
                    if (isDirectory)
                    {
                        MessageService.ShowError(exception, "Can't rename directory " + oldName);
                    }
                    else
                    {
                        MessageService.ShowError(exception, "Can't rename file " + oldName);
                    }
                    return false;
                }
            }
            OnFileRenamed(new FileRenameEventArgs(oldName, newName, isDirectory));
            return true;
        }

        public static void Unload()
        {
            PropertyService.Set<Properties>("RecentOpen", RecentOpen.ToProperties());
        }

        public static SkyMap.Net.Core.RecentOpen RecentOpen
        {
            get
            {
                if (recentOpen == null)
                {
                    recentOpen = SkyMap.Net.Core.RecentOpen.FromXmlElement(PropertyService.Get<Properties>("RecentOpen", new Properties()));
                }
                return recentOpen;
            }
        }

        private class LoadFileWrapper
        {
            private IDisplayBinding binding;

            public LoadFileWrapper(IDisplayBinding binding)
            {
                this.binding = binding;
            }

            public void Invoke(string fileName)
            {
                IViewContent viewContent = this.binding.CreateContentForFile(fileName);
                if (viewContent != null)
                {
                    DisplayBindingService.AttachSubWindows(viewContent, false);
                    WorkbenchSingleton.Workbench.ShowView(viewContent);
                }
            }
        }
    }
}

