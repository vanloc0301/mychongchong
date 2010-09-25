namespace SkyMap.Net.Core
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public static class FileUtility
    {
        private static string applicationRootPath = Environment.CurrentDirectory;
        private const string fileNameRegEx = "^([a-zA-Z]:)?[^:]+$";
        public static int MaxPathLength = 260;
        private static readonly char[] separators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar };

        public static  event FileNameEventHandler FileLoaded;

        public static  event FileNameEventHandler FileSaved;

        public static string Combine(params string[] paths)
        {
            if ((paths == null) || (paths.Length == 0))
            {
                return string.Empty;
            }
            string str = paths[0];
            for (int i = 1; i < paths.Length; i++)
            {
                str = Path.Combine(str, paths[i]);
            }
            return str;
        }

        public static void DeepCopy(string sourceDirectory, string destinationDirectory, bool overwrite)
        {
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }
            foreach (string str in Directory.GetFiles(sourceDirectory))
            {
                File.Copy(str, Path.Combine(destinationDirectory, Path.GetFileName(str)), overwrite);
            }
            foreach (string str2 in Directory.GetDirectories(sourceDirectory))
            {
                DeepCopy(str2, Path.Combine(destinationDirectory, Path.GetFileName(str2)), overwrite);
            }
        }

        public static string GetAbsolutePath(string baseDirectoryPath, string relPath)
        {
            return Path.GetFullPath(Path.Combine(baseDirectoryPath, relPath));
        }

        public static string[] GetAvailableRuntimeVersions()
        {
            string[] directories = Directory.GetDirectories(NETFrameworkInstallRoot);
            ArrayList list = new ArrayList();
            foreach (string str2 in directories)
            {
                string fileName = Path.GetFileName(str2);
                if (fileName.StartsWith("v"))
                {
                    list.Add(fileName);
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        public static string GetRelativePath(string baseDirectoryPath, string absPath)
        {
            if (IsUrl(absPath) || IsUrl(baseDirectoryPath))
            {
                return absPath;
            }
            try
            {
                baseDirectoryPath = Path.GetFullPath(baseDirectoryPath.TrimEnd(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }));
                absPath = Path.GetFullPath(absPath);
            }
            catch (Exception exception)
            {
                throw new ArgumentException("GetRelativePath error '" + baseDirectoryPath + "' -> '" + absPath + "'", exception);
            }
            string[] strArray = baseDirectoryPath.Split(separators);
            string[] strArray2 = absPath.Split(separators);
            int index = 0;
            while (index < Math.Min(strArray.Length, strArray2.Length))
            {
                if (!strArray[index].Equals(strArray2[index], StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                index++;
            }
            if (index == 0)
            {
                return absPath;
            }
            StringBuilder builder = new StringBuilder();
            if (index != strArray.Length)
            {
                for (int i = index; i < strArray.Length; i++)
                {
                    builder.Append("..");
                    builder.Append(Path.DirectorySeparatorChar);
                }
            }
            builder.Append(string.Join(Path.DirectorySeparatorChar.ToString(), strArray2, index, strArray2.Length - index));
            return builder.ToString();
        }

        public static bool IsBaseDirectory(string baseDirectory, string testDirectory)
        {
            try
            {
                baseDirectory = Path.GetFullPath(baseDirectory).ToUpperInvariant();
                testDirectory = Path.GetFullPath(testDirectory).ToUpperInvariant();
                baseDirectory = baseDirectory.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                testDirectory = testDirectory.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (baseDirectory[baseDirectory.Length - 1] != Path.DirectorySeparatorChar)
                {
                    baseDirectory = baseDirectory + Path.DirectorySeparatorChar;
                }
                if (testDirectory[testDirectory.Length - 1] != Path.DirectorySeparatorChar)
                {
                    testDirectory = testDirectory + Path.DirectorySeparatorChar;
                }
                return testDirectory.StartsWith(baseDirectory);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsDirectory(string filename)
        {
            if (!Directory.Exists(filename))
            {
                return false;
            }
            return ((File.GetAttributes(filename) & FileAttributes.Directory) != 0);
        }

        public static bool IsEqualFileName(string fileName1, string fileName2)
        {
            if ((fileName1.Length == 0) || (fileName2.Length == 0))
            {
                return false;
            }
            char ch = fileName1[fileName1.Length - 1];
            if ((ch == Path.DirectorySeparatorChar) || (ch == Path.AltDirectorySeparatorChar))
            {
                fileName1 = fileName1.Substring(0, fileName1.Length - 1);
            }
            ch = fileName2[fileName2.Length - 1];
            if ((ch == Path.DirectorySeparatorChar) || (ch == Path.AltDirectorySeparatorChar))
            {
                fileName2 = fileName2.Substring(0, fileName2.Length - 1);
            }
            try
            {
                if ((((fileName1.Length < 2) || (fileName1[1] != ':')) || (fileName1.IndexOf("/.") >= 0)) || (fileName1.IndexOf(@"\.") >= 0))
                {
                    fileName1 = Path.GetFullPath(fileName1);
                }
                if ((((fileName2.Length < 2) || (fileName2[1] != ':')) || (fileName2.IndexOf("/.") >= 0)) || (fileName2.IndexOf(@"\.") >= 0))
                {
                    fileName2 = Path.GetFullPath(fileName2);
                }
            }
            catch (Exception)
            {
            }
            return string.Equals(fileName1, fileName2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsUrl(string path)
        {
            return (path.IndexOf(':') >= 2);
        }

        public static bool IsValidDirectoryName(string name)
        {
            if (!IsValidFileName(name))
            {
                return false;
            }
            if (name.IndexOfAny(new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar }) >= 0)
            {
                return false;
            }
            if (name.Trim(new char[] { ' ' }).Length == 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidFileName(string fileName)
        {
            if (((fileName == null) || (fileName.Length == 0)) || (fileName.Length >= MaxPathLength))
            {
                return false;
            }
            if (fileName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }
            if ((fileName.IndexOf('?') >= 0) || (fileName.IndexOf('*') >= 0))
            {
                return false;
            }
            if (!Regex.IsMatch(fileName, "^([a-zA-Z]:)?[^:]+$"))
            {
                return false;
            }
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (fileNameWithoutExtension != null)
            {
                fileNameWithoutExtension = fileNameWithoutExtension.ToUpperInvariant();
            }
            if ((((fileNameWithoutExtension == "CON") || (fileNameWithoutExtension == "PRN")) || (fileNameWithoutExtension == "AUX")) || (fileNameWithoutExtension == "NUL"))
            {
                return false;
            }
            char c = (fileNameWithoutExtension.Length == 4) ? fileNameWithoutExtension[3] : '\0';
            return ((!fileNameWithoutExtension.StartsWith("COM") && !fileNameWithoutExtension.StartsWith("LPT")) || !char.IsDigit(c));
        }

        private static bool Match(string src, string pattern)
        {
            if (pattern[0] == '*')
            {
                int length = pattern.Length;
                int num2 = src.Length;
                while (--length > 0)
                {
                    if (pattern[length] == '*')
                    {
                        return MatchN(src, 0, pattern, 0);
                    }
                    if (num2-- == 0)
                    {
                        return false;
                    }
                    if ((pattern[length] != src[num2]) && (pattern[length] != '?'))
                    {
                        return false;
                    }
                }
                return true;
            }
            return MatchN(src, 0, pattern, 0);
        }

        public static bool MatchesPattern(string filename, string pattern)
        {
            filename = filename.ToUpper();
            pattern = pattern.ToUpper();
            string[] strArray = pattern.Split(new char[] { ';' });
            foreach (string str in strArray)
            {
                if (Match(filename, str))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool MatchN(string src, int srcidx, string pattern, int patidx)
        {
            int length = pattern.Length;
            int num2 = src.Length;
            while (true)
            {
                if (patidx == length)
                {
                    return (srcidx == num2);
                }
                char ch = pattern[patidx++];
                if (ch == '?')
                {
                    if (srcidx == src.Length)
                    {
                        return false;
                    }
                    srcidx++;
                }
                else if (ch != '*')
                {
                    if ((srcidx == src.Length) || (src[srcidx] != ch))
                    {
                        return false;
                    }
                    srcidx++;
                }
                else
                {
                    if (patidx == pattern.Length)
                    {
                        return true;
                    }
                    while (srcidx < num2)
                    {
                        if (MatchN(src, srcidx, pattern, patidx))
                        {
                            return true;
                        }
                        srcidx++;
                    }
                    return false;
                }
            }
        }

        public static FileOperationResult ObservedLoad(FileOperationDelegate loadFile, string fileName)
        {
            return ObservedLoad(loadFile, fileName, FileErrorPolicy.Inform);
        }

        public static FileOperationResult ObservedLoad(NamedFileOperationDelegate saveFileAs, string fileName)
        {
            return ObservedLoad(saveFileAs, fileName, FileErrorPolicy.Inform);
        }

        public static FileOperationResult ObservedLoad(FileOperationDelegate loadFile, string fileName, FileErrorPolicy policy)
        {
            return ObservedLoad(loadFile, fileName, ResourceService.GetString("FileUtilityService.CantLoadFileStandardText"), policy);
        }

        public static FileOperationResult ObservedLoad(NamedFileOperationDelegate saveFileAs, string fileName, FileErrorPolicy policy)
        {
            return ObservedLoad(saveFileAs, fileName, ResourceService.GetString("FileUtilityService.CantLoadFileStandardText"), policy);
        }

        public static FileOperationResult ObservedLoad(FileOperationDelegate loadFile, string fileName, string message, FileErrorPolicy policy)
        {
            try
            {
                loadFile();
                OnFileLoaded(new FileNameEventArgs(fileName));
                return FileOperationResult.OK;
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                switch (policy)
                {
                    case FileErrorPolicy.Inform:
                        using (SaveErrorInformDialog dialog = new SaveErrorInformDialog(fileName, message, "${res:FileUtilityService.ErrorWhileLoading}", exception))
                        {
                            dialog.ShowDialog();
                        }
                        goto Label_00CE;

                    case FileErrorPolicy.ProvideAlternative:
                        using (SaveErrorChooseDialog dialog2 = new SaveErrorChooseDialog(fileName, message, "${res:FileUtilityService.ErrorWhileLoading}", exception, false))
                        {
                            switch (dialog2.ShowDialog())
                            {
                                case DialogResult.OK:
                                case DialogResult.Cancel:
                                case DialogResult.Abort:
                                    goto Label_00CE;

                                case DialogResult.Retry:
                                    return ObservedLoad(loadFile, fileName, message, policy);

                                case DialogResult.Ignore:
                                    return FileOperationResult.Failed;
                            }
                        }
                        goto Label_00CE;
                }
            }
        Label_00CE:
            return FileOperationResult.Failed;
        }

        public static FileOperationResult ObservedLoad(NamedFileOperationDelegate saveFileAs, string fileName, string message, FileErrorPolicy policy)
        {
            return ObservedLoad(new FileOperationDelegate(new LoadWrapper(saveFileAs, fileName).Invoke), fileName, message, policy);
        }

        public static FileOperationResult ObservedSave(FileOperationDelegate saveFile, string fileName)
        {
            return ObservedSave(saveFile, fileName, FileErrorPolicy.Inform);
        }

        public static FileOperationResult ObservedSave(NamedFileOperationDelegate saveFileAs, string fileName)
        {
            return ObservedSave(saveFileAs, fileName, FileErrorPolicy.Inform);
        }

        public static FileOperationResult ObservedSave(FileOperationDelegate saveFile, string fileName, FileErrorPolicy policy)
        {
            return ObservedSave(saveFile, fileName, ResourceService.GetString("FileUtilityService.CantSaveFileStandardText"), policy);
        }

        public static FileOperationResult ObservedSave(NamedFileOperationDelegate saveFileAs, string fileName, FileErrorPolicy policy)
        {
            return ObservedSave(saveFileAs, fileName, ResourceService.GetString("Services.FileUtilityService.CantSaveFileStandardText"), policy);
        }

        public static FileOperationResult ObservedSave(FileOperationDelegate saveFile, string fileName, string message, FileErrorPolicy policy)
        {
            try
            {
                saveFile();
                OnFileSaved(new FileNameEventArgs(fileName));
                return FileOperationResult.OK;
            }
            catch (Exception exception)
            {
                switch (policy)
                {
                    case FileErrorPolicy.Inform:
                        using (SaveErrorInformDialog dialog = new SaveErrorInformDialog(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", exception))
                        {
                            dialog.ShowDialog();
                        }
                        goto Label_00C7;

                    case FileErrorPolicy.ProvideAlternative:
                        using (SaveErrorChooseDialog dialog2 = new SaveErrorChooseDialog(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", exception, false))
                        {
                            switch (dialog2.ShowDialog())
                            {
                                case DialogResult.OK:
                                case DialogResult.Cancel:
                                case DialogResult.Abort:
                                    goto Label_00C7;

                                case DialogResult.Retry:
                                    return ObservedSave(saveFile, fileName, message, policy);

                                case DialogResult.Ignore:
                                    return FileOperationResult.Failed;
                            }
                        }
                        goto Label_00C7;
                }
            }
        Label_00C7:
            return FileOperationResult.Failed;
        }

        public static FileOperationResult ObservedSave(NamedFileOperationDelegate saveFileAs, string fileName, string message, FileErrorPolicy policy)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                saveFileAs(fileName);
                OnFileSaved(new FileNameEventArgs(fileName));
                return FileOperationResult.OK;
            }
            catch (Exception exception)
            {
                switch (policy)
                {
                    case FileErrorPolicy.Inform:
                        using (SaveErrorInformDialog dialog = new SaveErrorInformDialog(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", exception))
                        {
                            dialog.ShowDialog();
                        }
                        goto Label_016F;

                    case FileErrorPolicy.ProvideAlternative:
                        goto Label_0084;

                    default:
                        goto Label_016F;
                }
            Label_0084:
                using (SaveErrorChooseDialog dialog2 = new SaveErrorChooseDialog(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", exception, true))
                {
                    switch (dialog2.ShowDialog())
                    {
                        case DialogResult.OK:
                        {
                            using (SaveFileDialog dialog3 = new SaveFileDialog())
                            {
                                dialog3.OverwritePrompt = true;
                                dialog3.AddExtension = true;
                                dialog3.CheckFileExists = false;
                                dialog3.CheckPathExists = true;
                                dialog3.Title = "Choose alternate file name";
                                dialog3.FileName = fileName;
                                if (dialog3.ShowDialog() == DialogResult.OK)
                                {
                                    return ObservedSave(saveFileAs, dialog3.FileName, message, policy);
                                }
                                goto Label_0084;
                            }
                        }
                        case DialogResult.Cancel:
                        case DialogResult.Abort:
                            goto Label_016F;

                        case DialogResult.Retry:
                            return ObservedSave(saveFileAs, fileName, message, policy);

                        case DialogResult.Ignore:
                            return FileOperationResult.Failed;
                    }
                    goto Label_016F;
                }
            }
        Label_016F:
            return FileOperationResult.Failed;
        }

        private static void OnFileLoaded(FileNameEventArgs e)
        {
            if (FileLoaded != null)
            {
                FileLoaded(null, e);
            }
        }

        private static void OnFileSaved(FileNameEventArgs e)
        {
            if (FileSaved != null)
            {
                FileSaved(null, e);
            }
        }

        public static string RenameBaseDirectory(string fileName, string oldDirectory, string newDirectory)
        {
            fileName = Path.GetFullPath(fileName);
            oldDirectory = Path.GetFullPath(oldDirectory.TrimEnd(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }));
            newDirectory = Path.GetFullPath(newDirectory.TrimEnd(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }));
            if (IsBaseDirectory(oldDirectory, fileName))
            {
                if (fileName.Length == oldDirectory.Length)
                {
                    return newDirectory;
                }
                return Path.Combine(newDirectory, fileName.Substring(oldDirectory.Length + 1));
            }
            return fileName;
        }

        public static List<string> SearchDirectory(string directory, string filemask)
        {
            return SearchDirectory(directory, filemask, true, false);
        }

        public static List<string> SearchDirectory(string directory, string filemask, bool searchSubdirectories)
        {
            return SearchDirectory(directory, filemask, searchSubdirectories, false);
        }

        public static List<string> SearchDirectory(string directory, string filemask, bool searchSubdirectories, bool ignoreHidden)
        {
            List<string> collection = new List<string>();
            SearchDirectory(directory, filemask, collection, searchSubdirectories, ignoreHidden);
            return collection;
        }

        private static void SearchDirectory(string directory, string filemask, List<string> collection, bool searchSubdirectories, bool ignoreHidden)
        {
            string[] files = Directory.GetFiles(directory, filemask);
            foreach (string str in files)
            {
                if (!ignoreHidden || ((File.GetAttributes(str) & FileAttributes.Hidden) != FileAttributes.Hidden))
                {
                    collection.Add(str);
                }
            }
            if (searchSubdirectories)
            {
                string[] directories = Directory.GetDirectories(directory);
                foreach (string str2 in directories)
                {
                    if (!ignoreHidden || ((File.GetAttributes(str2) & FileAttributes.Hidden) != FileAttributes.Hidden))
                    {
                        SearchDirectory(str2, filemask, collection, searchSubdirectories, ignoreHidden);
                    }
                }
            }
        }

        public static bool TestFileExists(string filename)
        {
            if (!File.Exists(filename))
            {
                MessageService.ShowWarning(SkyMap.Net.Core.StringParser.Parse("${res:Fileutility.CantFindFileError}", new string[,] { { "FILE", filename } }));
                return false;
            }
            return true;
        }

        public static string ApplicationRootPath
        {
            get
            {
                return applicationRootPath;
            }
            set
            {
                applicationRootPath = value;
            }
        }

        public static string NETFrameworkInstallRoot
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework"))
                {
                    object obj2 = key.GetValue("InstallRoot");
                    return ((obj2 == null) ? string.Empty : obj2.ToString());
                }
            }
        }

        public static string NetSdkInstallRoot
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework"))
                {
                    object obj2 = key.GetValue("sdkInstallRootv2.0");
                    return ((obj2 == null) ? string.Empty : obj2.ToString());
                }
            }
        }

        private class LoadWrapper
        {
            private string fileName;
            private NamedFileOperationDelegate loadFile;

            public LoadWrapper(NamedFileOperationDelegate loadFile, string fileName)
            {
                this.loadFile = loadFile;
                this.fileName = fileName;
            }

            public void Invoke()
            {
                this.loadFile(this.fileName);
            }
        }
    }
}

