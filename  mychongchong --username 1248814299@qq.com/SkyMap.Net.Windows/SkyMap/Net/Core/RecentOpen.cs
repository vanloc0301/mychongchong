namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class RecentOpen
    {
        private List<string> lastfile;
        private List<string> lastproject;
        private int MAX_LENGTH;

        public event EventHandler RecentFileChanged;

        public event EventHandler RecentProjectChanged;

        public RecentOpen()
        {
            this.MAX_LENGTH = 10;
            this.lastfile = new List<string>();
            this.lastproject = new List<string>();
        }

        public RecentOpen(Properties p)
        {
            this.MAX_LENGTH = 10;
            this.lastfile = new List<string>();
            this.lastproject = new List<string>();
            if (p.Contains("Files"))
            {
                string[] strArray = p["Files"].Split(new char[] { ',' });
                foreach (string str in strArray)
                {
                    if (File.Exists(str))
                    {
                        this.lastfile.Add(str);
                    }
                }
            }
            if (p.Contains("Projects"))
            {
                string[] strArray2 = p["Projects"].Split(new char[] { ',' });
                foreach (string str in strArray2)
                {
                    if (File.Exists(str))
                    {
                        this.lastproject.Add(str);
                    }
                }
            }
        }

        public void AddLastFile(string name)
        {
            for (int i = 0; i < this.lastfile.Count; i++)
            {
                if (this.lastfile[i].ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    this.lastfile.RemoveAt(i);
                }
            }
            while (this.lastfile.Count >= this.MAX_LENGTH)
            {
                this.lastfile.RemoveAt(this.lastfile.Count - 1);
            }
            if (this.lastfile.Count > 0)
            {
                this.lastfile.Insert(0, name);
            }
            else
            {
                this.lastfile.Add(name);
            }
            this.OnRecentFileChange();
        }

        public void AddLastProject(string name)
        {
            for (int i = 0; i < this.lastproject.Count; i++)
            {
                if (this.lastproject[i].ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    this.lastproject.RemoveAt(i);
                }
            }
            while (this.lastproject.Count >= this.MAX_LENGTH)
            {
                this.lastproject.RemoveAt(this.lastproject.Count - 1);
            }
            if (this.lastproject.Count > 0)
            {
                this.lastproject.Insert(0, name);
            }
            else
            {
                this.lastproject.Add(name);
            }
            this.OnRecentProjectChange();
        }

        public void ClearRecentFiles()
        {
            this.lastfile.Clear();
            this.OnRecentFileChange();
        }

        public void ClearRecentProjects()
        {
            this.lastproject.Clear();
            this.OnRecentProjectChange();
        }

        public void FileRemoved(object sender, FileEventArgs e)
        {
            for (int i = 0; i < this.lastfile.Count; i++)
            {
                string str = this.lastfile[i].ToString();
                if (e.FileName == str)
                {
                    this.lastfile.RemoveAt(i);
                    this.OnRecentFileChange();
                    break;
                }
            }
        }

        public void FileRenamed(object sender, FileRenameEventArgs e)
        {
            for (int i = 0; i < this.lastfile.Count; i++)
            {
                string str = this.lastfile[i].ToString();
                if (e.SourceFile == str)
                {
                    this.lastfile.RemoveAt(i);
                    this.lastfile.Insert(i, e.TargetFile);
                    this.OnRecentFileChange();
                    break;
                }
            }
        }

        public static RecentOpen FromXmlElement(Properties properties)
        {
            return new RecentOpen(properties);
        }

        private void OnRecentFileChange()
        {
            if (this.RecentFileChanged != null)
            {
                this.RecentFileChanged(this, null);
            }
        }

        private void OnRecentProjectChange()
        {
            if (this.RecentProjectChanged != null)
            {
                this.RecentProjectChanged(this, null);
            }
        }

        public Properties ToProperties()
        {
            Properties properties = new Properties();
            properties["Files"] = string.Join(",", this.lastfile.ToArray());
            properties["Projects"] = string.Join(",", this.lastproject.ToArray());
            return properties;
        }

        public List<string> RecentFile
        {
            get
            {
                return this.lastfile;
            }
        }

        public List<string> RecentProject
        {
            get
            {
                return this.lastproject;
            }
        }
    }
}

