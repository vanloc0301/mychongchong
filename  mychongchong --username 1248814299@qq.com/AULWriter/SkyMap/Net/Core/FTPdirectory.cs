namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public class FTPdirectory : List<FTPfileInfo>
    {
        private const char slash = '/';

        public FTPdirectory()
        {
        }

        public FTPdirectory(string dir, string path)
        {
            foreach (string str in dir.Replace("\n", "").Split(new char[] { Convert.ToChar('\r') }))
            {
                if ((str != "") && !str.StartsWith("total"))
                {
                    base.Add(new FTPfileInfo(str, path));
                }
            }
        }

        public bool FileExists(string filename)
        {
            foreach (FTPfileInfo info in this)
            {
                if (info.Filename == filename)
                {
                    return true;
                }
            }
            return false;
        }

        public FTPdirectory GetDirectories()
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.Directory, "");
        }

        private FTPdirectory GetFileOrDir(FTPfileInfo.DirectoryEntryTypes type, string ext)
        {
            FTPdirectory pdirectory = new FTPdirectory();
            foreach (FTPfileInfo info in this)
            {
                if (info.FileType == type)
                {
                    if (ext == "")
                    {
                        pdirectory.Add(info);
                    }
                    else if (ext == info.Extension)
                    {
                        pdirectory.Add(info);
                    }
                }
            }
            return pdirectory;
        }

        public FTPdirectory GetFiles(string ext)
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.File, ext);
        }

        public static string GetParentDirectory(string dir)
        {
            string str = dir.TrimEnd(new char[] { '/' });
            int num = str.LastIndexOf('/');
            if (num <= 0)
            {
                throw new ApplicationException("No parent for root");
            }
            return str.Substring(0, num - 1);
        }
    }
}

