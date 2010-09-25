namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FileScout : UserControl, IPadContent, IDisposable
    {
        private FileList filelister = new FileList();
        private ShellTree filetree = new ShellTree();
        private Splitter splitter1 = new Splitter();

        public FileScout()
        {
            this.filetree.Dock = DockStyle.Top;
            this.filetree.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.filetree.Location = new Point(0, 0x16);
            this.filetree.Size = new Size(0xb8, 0x9d);
            this.filetree.TabIndex = 1;
            this.filetree.AfterSelect += new TreeViewEventHandler(this.DirectorySelected);
            ImageList list = new ImageList();
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.ClosedFolderBitmap"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.OpenFolderBitmap"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.FLOPPY"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.DRIVE"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.CDROM"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.NETWORK"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.Desktop"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.PersonalFiles"));
            list.Images.Add(ResourceService.GetBitmap("Icons.16x16.MyComputer"));
            this.filetree.ImageList = list;
            this.filelister.Dock = DockStyle.Fill;
            this.filelister.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.filelister.Location = new Point(0, 0xb8);
            this.filelister.Sorting = SortOrder.Ascending;
            this.filelister.Size = new Size(0xb8, 450);
            this.filelister.TabIndex = 3;
            this.splitter1.Dock = DockStyle.Top;
            this.splitter1.Location = new Point(0, 0xb3);
            this.splitter1.Size = new Size(0xb8, 5);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            this.splitter1.MinSize = 50;
            this.splitter1.MinExtra = 50;
            base.Controls.Add(this.filelister);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.filetree);
        }

        private void DirectorySelected(object sender, TreeViewEventArgs e)
        {
            this.filelister.ShowFilesInPath(this.filetree.NodePath + Path.DirectorySeparatorChar);
        }

        public void RedrawContent()
        {
        }

        public System.Windows.Forms.Control Control
        {
            get
            {
                return this;
            }
        }
    }
}

