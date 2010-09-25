namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;

    public class FileList : ListView
    {
        private FileSystemWatcher watcher;

        public FileList()
        {
            ResourceManager manager = new ResourceManager("ProjectComponentResources", base.GetType().Module.Assembly);
            base.Columns.Add(ResourceService.GetString("CompilerResultView.FileText"), 100, HorizontalAlignment.Left);
            base.Columns.Add(ResourceService.GetString("MainWindow.Windows.FileScout.Size"), -2, HorizontalAlignment.Right);
            base.Columns.Add(ResourceService.GetString("MainWindow.Windows.FileScout.LastModified"), -2, HorizontalAlignment.Left);
            try
            {
                this.watcher = new FileSystemWatcher();
            }
            catch
            {
            }
            if (this.watcher != null)
            {
                this.watcher.NotifyFilter = NotifyFilters.FileName;
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Renamed += new RenamedEventHandler(this.fileRenamed);
                this.watcher.Deleted += new FileSystemEventHandler(this.fileDeleted);
                this.watcher.Created += new FileSystemEventHandler(this.fileCreated);
                this.watcher.Changed += new FileSystemEventHandler(this.fileChanged);
            }
            base.HideSelection = false;
            base.GridLines = true;
            base.LabelEdit = true;
            base.SmallImageList = IconManager.List;
            base.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            base.View = View.Details;
            base.Alignment = ListViewAlignment.Left;
        }

        private void deleteFiles(object sender, EventArgs e)
        {
            string fullName = "";
            foreach (FileListItem item in base.SelectedItems)
            {
                fullName = item.FullName;
                break;
            }
            if (MessageService.AskQuestion(SkyMap.Net.Core.StringParser.Parse("${res:ProjectComponent.ContextMenu.Delete.Question}", new string[,] { { "FileName", fullName } }), "${Global.Delete}"))
            {
                foreach (FileListItem item in base.SelectedItems)
                {
                    try
                    {
                        File.Delete(item.FullName);
                    }
                    catch (Exception exception)
                    {
                        MessageService.ShowError(exception, "Couldn't delete file '" + Path.GetFileName(item.FullName) + "'");
                        break;
                    }
                }
            }
        }

        private void fileChanged(object sender, FileSystemEventArgs e)
        {
            MethodInvoker method = delegate {
                foreach (FileListItem item in this.Items)
                {
                    if (item.FullName.Equals(e.FullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        FileInfo info = new FileInfo(e.FullPath);
                        try
                        {
                            item.SubItems[1].Text = Math.Round((double) (((double) info.Length) / 1024.0)).ToString() + " KB";
                            item.SubItems[2].Text = info.LastWriteTime.ToString();
                        }
                        catch (IOException)
                        {
                        }
                        break;
                    }
                }
            };
            WorkbenchSingleton.SafeThreadAsyncCall(method, new object[0]);
        }

        private void fileCreated(object sender, FileSystemEventArgs e)
        {
            MethodInvoker method = delegate {
                FileInfo info = new FileInfo(e.FullPath);
                ListViewItem item = this.Items.Add(new FileListItem(e.FullPath));
                try
                {
                    item.SubItems.Add(Math.Round((double) (((double) info.Length) / 1024.0)).ToString() + " KB");
                    item.SubItems.Add(info.LastWriteTime.ToString());
                }
                catch (IOException)
                {
                }
            };
            WorkbenchSingleton.SafeThreadAsyncCall(method, new object[0]);
        }

        private void fileDeleted(object sender, FileSystemEventArgs e)
        {
            MethodInvoker method = delegate {
                foreach (FileListItem item in this.Items)
                {
                    if (item.FullName.Equals(e.FullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        this.Items.Remove(item);
                        break;
                    }
                }
            };
            WorkbenchSingleton.SafeThreadAsyncCall(method, new object[0]);
        }

        private void fileRenamed(object sender, RenamedEventArgs e)
        {
            MethodInvoker method = delegate {
                foreach (FileListItem item in this.Items)
                {
                    if (item.FullName.Equals(e.OldFullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        item.FullName = e.FullPath;
                        item.Text = e.Name;
                        break;
                    }
                }
            };
            WorkbenchSingleton.SafeThreadAsyncCall(method, new object[0]);
        }

        protected override void OnAfterLabelEdit(LabelEditEventArgs e)
        {
            if (!((e.Label != null) && FileService.CheckFileName(e.Label)))
            {
                e.CancelEdit = true;
            }
            else
            {
                string fullName = ((FileListItem) base.Items[e.Item]).FullName;
                string newName = Path.Combine(Path.GetDirectoryName(fullName), e.Label);
                if (FileService.RenameFile(fullName, newName, false))
                {
                    ((FileListItem) base.Items[e.Item]).FullName = newName;
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ListViewItem itemAt = base.GetItemAt(base.PointToScreen(new Point(e.X, e.Y)).X, base.PointToScreen(new Point(e.X, e.Y)).Y);
            if ((e.Button == MouseButtons.Right) && (base.SelectedItems.Count > 0))
            {
            }
        }

        private void renameFile(object sender, EventArgs e)
        {
            if (base.SelectedItems.Count == 1)
            {
                base.SelectedItems[0].BeginEdit();
            }
        }

        public void ShowFilesInPath(string path)
        {
            base.Items.Clear();
            try
            {
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    this.watcher.Path = path;
                    this.watcher.EnableRaisingEvents = true;
                    foreach (string str in files)
                    {
                        FileInfo info = new FileInfo(str);
                        ListViewItem item = base.Items.Add(new FileListItem(str));
                        item.SubItems.Add(Math.Round((double) (((double) info.Length) / 1024.0)).ToString() + " KB");
                        item.SubItems.Add(info.LastWriteTime.ToString());
                    }
                    base.EndUpdate();
                }
            }
            catch (Exception)
            {
            }
        }

        public class FileListItem : ListViewItem
        {
            private string fullname;

            public FileListItem(string fullname) : base(Path.GetFileName(fullname))
            {
                this.fullname = fullname;
                base.ImageIndex = IconManager.GetIndexForFile(fullname);
            }

            public string FullName
            {
                get
                {
                    return this.fullname;
                }
                set
                {
                    this.fullname = value;
                }
            }
        }
    }
}

