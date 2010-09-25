namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public class FTPclient
    {
        private string _currentDirectory;
        private string _hostname;
        private string _lastDirectory;
        private string _password;
        private string _username;

        private sealed class c__DisplayClass1
        {
            // Fields
            public string[] dirArray;
        }

        private sealed class c__DisplayClass4
        {
            // Fields
            public FTPclient.c__DisplayClass1 CS_8__locals2;
            public int i;

            // Methods
            public bool Eb__0(FTPfileInfo f)
            {
                return ((f.FileType == FTPfileInfo.DirectoryEntryTypes.Directory) && (f.Filename == this.CS_8__locals2.dirArray[this.i]));
            }
        }

        public FTPclient()
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
        }

        public FTPclient(string Hostname)
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._hostname = Hostname;
        }

        public FTPclient(string Hostname, string Username, string Password)
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._hostname = Hostname;
            this._username = Username;
            this._password = Password;
        }

        private string AdjustDir(string path)
        {
            return ((path.StartsWith("/") ? "" : "/").ToString() + path);
        }

        public bool Download(FTPfileInfo file, FileInfo localFI, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFI, PermitOverwrite);
        }

        public bool Download(FTPfileInfo file, string localFilename, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFilename, PermitOverwrite);
        }

        public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite)
        {
            string str;
            if (targetFI.Exists && !PermitOverwrite)
            {
                throw new ApplicationException("Target file already exists");
            }
            if (sourceFilename.Trim() == "")
            {
                throw new ApplicationException("File not specified");
            }
            if (sourceFilename.Contains("/"))
            {
                str = this.AdjustDir(sourceFilename);
            }
            else
            {
                str = this.CurrentDirectory + sourceFilename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest request = this.GetRequest(uRI);
            request.Method = "RETR";
            request.UseBinary = true;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    FileStream stream2 = targetFI.OpenWrite();
                    try
                    {
                        byte[] buffer = new byte[0x800];
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, buffer.Length);
                            stream2.Write(buffer, 0, count);
                        }
                        while (count != 0);
                        stream.Close();
                        stream2.Flush();
                        stream2.Close();
                    }
                    catch (Exception)
                    {
                        stream2.Close();
                        targetFI.Delete();
                        throw;
                    }
                    finally
                    {
                        if (stream2 != null)
                        {
                            stream2.Dispose();
                        }
                    }
                    stream.Close();
                }
                response.Close();
            }
            return true;
        }

        public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite)
        {
            FileInfo targetFI = new FileInfo(localFilename);
            return this.Download(sourceFilename, targetFI, PermitOverwrite);
        }

        private void EnsureDirtory(string ftpfile)
        {
            if (!this.FtpFileExists(ftpfile))
            {
                c__DisplayClass1 class3 = new c__DisplayClass1();
                string[] dirArray = ftpfile.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                class3.dirArray = dirArray;
                string dirpath = string.Empty;
                FTPdirectory pdirectory = this.ListDirectoryDetail(string.Empty);
                Predicate<FTPfileInfo> match = null;
                c__DisplayClass1 CS_8__locals2 = class3;
                for (int i = 0; i < (dirArray.Length - 1); i++)
                {
                    dirpath = dirpath + "/" + dirArray[i].Trim();
                    if (pdirectory != null)
                    {
                        if (match == null)
                        {
                            match = delegate(FTPfileInfo f)
                            {
                                return (f.FileType == FTPfileInfo.DirectoryEntryTypes.Directory) && (f.Filename == CS_8__locals2.dirArray[i]);
                            };
                        }
                        if (pdirectory.Exists(match))
                        {
                            goto Label_00AC;
                        }
                    }
                    this.FtpCreateDirectory(dirpath);
                    if (pdirectory != null)
                    {
                        pdirectory = null;
                    }
                    goto Label_00B4;
                Label_00AC:
                    pdirectory = this.ListDirectoryDetail(dirpath);
                Label_00B4: ;
                }
            }
        }

        public bool FtpCreateDirectory(string dirpath)
        {
            string uRI = this.Hostname + this.AdjustDir(dirpath);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "MKD";
            try
            {
                this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpDelete(string filename)
        {
            string uRI = this.Hostname + this.GetFullPath(filename);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "DELE";
            try
            {
                this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpDeleteDirectory(string dirpath)
        {
            string uRI = this.Hostname + this.AdjustDir(dirpath);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "RMD";
            try
            {
                this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpFileExists(string filename)
        {
            bool flag;
            try
            {
                this.GetFileSize(filename);
                flag = true;
            }
            catch (Exception exception)
            {
                if (!(exception is WebException))
                {
                    throw;
                }
                if (!exception.Message.Contains("550"))
                {
                    throw;
                }
                return false;
            }
            return flag;
        }

        public bool FtpRename(string sourceFilename, string newName)
        {
            string fullPath = this.GetFullPath(sourceFilename);
            if (!this.FtpFileExists(fullPath))
            {
                throw new FileNotFoundException("File " + fullPath + " not found");
            }
            string filename = this.GetFullPath(newName);
            if (filename == fullPath)
            {
                throw new ApplicationException("Source and target are the same");
            }
            if (this.FtpFileExists(filename))
            {
                throw new ApplicationException("Target file " + filename + " already exists");
            }
            string uRI = this.Hostname + fullPath;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "RENAME";
            ftp.RenameTo = filename.Substring(filename.LastIndexOfAny(new char[] { '\\', '/' }) + 1);
            try
            {
                this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private ICredentials GetCredentials()
        {
            return new NetworkCredential(this.Username, this.Password);
        }

        private string GetDirectory(string directory)
        {
            string str;
            if (directory == "")
            {
                str = this.Hostname + this.CurrentDirectory;
                this._lastDirectory = this.CurrentDirectory;
                return str;
            }
            if (!directory.StartsWith("/"))
            {
                throw new ApplicationException("Directory should start with /");
            }
            str = this.Hostname + directory;
            this._lastDirectory = directory;
            return str;
        }

        public long GetFileSize(string filename)
        {
            string str;
            if (filename.Contains("/"))
            {
                str = this.AdjustDir(filename);
            }
            else
            {
                str = this.CurrentDirectory + filename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "SIZE";
            this.GetStringResponse(ftp);
            return this.GetSize(ftp);
        }

        private string GetFullPath(string file)
        {
            if (file.Contains("/"))
            {
                return this.AdjustDir(file);
            }
            return (this.CurrentDirectory + file);
        }

        private FtpWebRequest GetRequest(string URI)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(URI);
            request.Credentials = this.GetCredentials();
            request.UsePassive = true;
            request.KeepAlive = false;
            return request;
        }

        private long GetSize(FtpWebRequest ftp)
        {
            long contentLength;
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                contentLength = response.ContentLength;
                response.Close();
            }
            return contentLength;
        }

        private string GetStringResponse(FtpWebRequest ftp)
        {
            string str = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long contentLength = response.ContentLength;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                    {
                        str = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
                response.Close();
            }
            return str;
        }

        public List<string> ListDirectory(string directory)
        {
            FtpWebRequest ftp = this.GetRequest(this.GetDirectory(directory));
            ftp.Method = "NLST";
            string str = this.GetStringResponse(ftp).Replace("\r\n", "\r").TrimEnd(new char[] { '\r' });
            List<string> list = new List<string>();
            list.AddRange(str.Split(new char[] { '\r' }));
            return list;
        }

        public FTPdirectory ListDirectoryDetail(string directory)
        {
            FtpWebRequest ftp = this.GetRequest(this.GetDirectory(directory));
            ftp.Method = "LIST";
            return new FTPdirectory(this.GetStringResponse(ftp).Replace("\r\n", "\r").TrimEnd(new char[] { '\r' }), this._lastDirectory);
        }

        public bool Upload(FileInfo fi, string targetFilename)
        {
            string str;
            if (targetFilename.Trim() == "")
            {
                str = this.CurrentDirectory + fi.Name;
            }
            else if (targetFilename.Contains("/"))
            {
                str = this.AdjustDir(targetFilename);
            }
            else
            {
                str = this.CurrentDirectory + targetFilename;
            }
            this.EnsureDirtory(str);
            string uRI = this.Hostname + str;
            FtpWebRequest request = this.GetRequest(uRI);
            request.Method = "STOR";
            request.UseBinary = true;
            request.ContentLength = fi.Length;
            byte[] buffer = new byte[0x800];
            using (FileStream stream = fi.OpenRead())
            {
                try
                {
                    using (Stream stream2 = request.GetRequestStream())
                    {
                        int num;
                        do
                        {
                            num = stream.Read(buffer, 0, 0x800);
                            stream2.Write(buffer, 0, num);
                        }
                        while (num >= 0x800);
                        stream2.Close();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    stream.Close();
                }
            }
            request = null;
            return true;
        }

        public bool Upload(string localFilename, string targetFilename)
        {
            if (!System.IO.File.Exists(localFilename))
            {
                throw new ApplicationException("File " + localFilename + " not found");
            }
            FileInfo fi = new FileInfo(localFilename);
            return this.Upload(fi, targetFilename);
        }

        public string CurrentDirectory
        {
            get
            {
                return (this._currentDirectory + (this._currentDirectory.EndsWith("/") ? "" : "/").ToString());
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw new ApplicationException("Directory should start with /");
                }
                this._currentDirectory = value;
            }
        }

        public string Hostname
        {
            get
            {
                if (this._hostname.StartsWith("ftp://"))
                {
                    return this._hostname;
                }
                return ("ftp://" + this._hostname);
            }
            set
            {
                this._hostname = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public string Username
        {
            get
            {
                if (!(this._username == ""))
                {
                    return this._username;
                }
                return "anonymous";
            }
            set
            {
                this._username = value;
            }
        }
    }
}

