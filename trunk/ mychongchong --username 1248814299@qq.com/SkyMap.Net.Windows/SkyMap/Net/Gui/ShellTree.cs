namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class ShellTree : TreeView
    {
        public ShellTree()
        {
            TreeNode node4;
            base.Sorted = true;
            TreeNode node = base.Nodes.Add("Desktop");
            node.ImageIndex = 6;
            node.SelectedImageIndex = 6;
            node.Tag = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            TreeNode node2 = node.Nodes.Add(ResourceService.GetString("MainWindow.Windows.FileScout.MyDocuments"));
            node2.ImageIndex = 7;
            node2.SelectedImageIndex = 7;
            try
            {
                node2.Tag = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            catch (Exception)
            {
                node2.Tag = @"C:\";
            }
            node2.Nodes.Add("");
            TreeNode node3 = node.Nodes.Add(ResourceService.GetString("MainWindow.Windows.FileScout.MyComputer"));
            node3.ImageIndex = 8;
            node3.SelectedImageIndex = 8;
            try
            {
                node3.Tag = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            catch (Exception)
            {
                node3.Tag = @"C:\";
            }
            foreach (string str in Environment.GetLogicalDrives())
            {
                DriveObject obj2 = new DriveObject(str);
                node4 = new TreeNode(obj2.ToString());
                node4.Nodes.Add(new TreeNode(""));
                node4.Tag = str.Substring(0, str.Length - 1);
                node3.Nodes.Add(node4);
                switch (DriveObject.GetDriveType(str))
                {
                    case SkyMap.Net.Gui.DriveType.Removeable:
                        node4.ImageIndex = node4.SelectedImageIndex = 2;
                        break;

                    case SkyMap.Net.Gui.DriveType.Fixed:
                        node4.ImageIndex = node4.SelectedImageIndex = 3;
                        break;

                    case SkyMap.Net.Gui.DriveType.Remote:
                        node4.ImageIndex = node4.SelectedImageIndex = 5;
                        break;

                    case SkyMap.Net.Gui.DriveType.Cdrom:
                        node4.ImageIndex = node4.SelectedImageIndex = 4;
                        break;

                    default:
                        node4.ImageIndex = node4.SelectedImageIndex = 3;
                        break;
                }
            }
            foreach (string str2 in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
            {
                node4 = node.Nodes.Add(Path.GetFileName(str2));
                node4.Tag = str2;
                node4.ImageIndex = node4.SelectedImageIndex = 0;
                node4.Nodes.Add(new TreeNode(""));
            }
            node.Expand();
            node3.Expand();
            this.InitializeComponent();
        }

        private int getNodeLevel(TreeNode node)
        {
            TreeNode parent = node;
            int num = 0;
            while (true)
            {
                parent = parent.Parent;
                if (parent == null)
                {
                    return num;
                }
                num++;
            }
        }

        private void InitializeComponent()
        {
            base.BeforeSelect += new TreeViewCancelEventHandler(this.SetClosedIcon);
            base.AfterSelect += new TreeViewEventHandler(this.SetOpenedIcon);
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if ((e.Node.Parent != null) && (e.Node.Parent.Parent != null))
                {
                    this.PopulateSubDirectory(e.Node, 2);
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.PopulateSubDirectory(e.Node, 1);
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception, "Device error");
                e.Cancel = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void PopulateShellTree(string path)
        {
            string[] strArray = path.Split(new char[] { Path.DirectorySeparatorChar });
            TreeNodeCollection nodes = base.Nodes;
            foreach (string str in strArray)
            {
                foreach (TreeNode node in nodes)
                {
                    if (((string) node.Tag).Equals(str, StringComparison.OrdinalIgnoreCase))
                    {
                        base.SelectedNode = node;
                        this.PopulateSubDirectory(node, 2);
                        node.Expand();
                        nodes = node.Nodes;
                        break;
                    }
                }
            }
        }

        private void PopulateSubDirectory(TreeNode curNode, int depth)
        {
            if (--depth >= 0)
            {
                if ((curNode.Nodes.Count == 1) && curNode.Nodes[0].Text.Equals(""))
                {
                    string[] directories = null;
                    curNode.Nodes.Clear();
                    try
                    {
                        directories = Directory.GetDirectories(curNode.Tag.ToString() + Path.DirectorySeparatorChar);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    foreach (string str in directories)
                    {
                        try
                        {
                            string fileName = Path.GetFileName(str);
                            if ((File.GetAttributes(str) & FileAttributes.Hidden) == 0)
                            {
                                TreeNode node = curNode.Nodes.Add(fileName);
                                node.Tag = curNode.Tag.ToString() + Path.DirectorySeparatorChar + fileName;
                                node.ImageIndex = node.SelectedImageIndex = 0;
                                node.Nodes.Add("");
                                this.PopulateSubDirectory(node, depth);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    foreach (TreeNode node in curNode.Nodes)
                    {
                        this.PopulateSubDirectory(node, depth);
                    }
                }
            }
        }

        private void SetClosedIcon(object sender, TreeViewCancelEventArgs e)
        {
            if ((base.SelectedNode != null) && (this.getNodeLevel(base.SelectedNode) > 2))
            {
                base.SelectedNode.ImageIndex = base.SelectedNode.SelectedImageIndex = 0;
            }
        }

        private void SetOpenedIcon(object sender, TreeViewEventArgs e)
        {
            if ((this.getNodeLevel(e.Node) > 2) && ((e.Node.Parent != null) && (e.Node.Parent.Parent != null)))
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
            }
        }

        public string NodePath
        {
            get
            {
                return (string) base.SelectedNode.Tag;
            }
            set
            {
                this.PopulateShellTree(value);
            }
        }
    }
}

