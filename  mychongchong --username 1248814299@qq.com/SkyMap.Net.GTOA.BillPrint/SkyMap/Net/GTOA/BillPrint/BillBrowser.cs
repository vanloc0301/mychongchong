namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;

    public class BillBrowser : AbstractPadContent
    {
        protected bool b = true;
        private ExtTreeView billBrowserTreeView = new ExtTreeView();
        private Panel contentPanel = new Panel();
        private static BillBrowser instance;

        public BillBrowser()
        {
            instance = this;
            this.LoadBills();
            this.billBrowserTreeView.Dock = DockStyle.Fill;
            ImageList list = new ImageList();
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.Add(IconService.GetBitmap("Icons.16x16.ClosedFolderBitmap"));
            list.Images.Add(IconService.GetBitmap("Icons.16x16.OpenFolderBitmap"));
            list.Images.Add(IconService.GetBitmap("Icons.16x16.SelectionArrow"));
            list.Images.Add(IconService.GetBitmap("Icons.16x16.SelectionArrow"));
            this.billBrowserTreeView.IsSorted = false;
            this.billBrowserTreeView.ImageList = list;
            this.contentPanel.Controls.Add(this.billBrowserTreeView);
            this.contentPanel.Controls.AddRange(ToolbarService.CreateToolbars(this, "/Workbench/Pads/BillBrowser"));
            this.billBrowserTreeView.ExpandAll();
            this.billBrowserTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.billBrowserTreeView_NodeMouseClick);
        }

        private void BeforeExpandNode(object sender, TreeViewCancelEventArgs e)
        {
            if (this.b)
            {
                this.b = false;
                this.billBrowserTreeView.BeginUpdate();
                TreeNode firstNode = e.Node.FirstNode;
                while (firstNode.Nodes.Count > 0)
                {
                    firstNode = firstNode.FirstNode;
                }
                this.billBrowserTreeView.CollapseAll();
                firstNode.EnsureVisible();
                firstNode.ImageIndex = 3;
                this.billBrowserTreeView.EndUpdate();
                this.b = true;
            }
        }

        private void BeforeSelectNode(object sender, TreeViewCancelEventArgs e)
        {
            this.ResetImageIndex(this.billBrowserTreeView.Nodes);
            if (this.b)
            {
                this.CollapseOrExpandNode(e.Node);
            }
        }

        private void billBrowserTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is BillNode)
            {
                ((BillNode) e.Node).MouseClick();
            }
        }

        private void CollapseOrExpandNode(TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                if (node.IsExpanded)
                {
                    node.Collapse();
                }
                else
                {
                    node.Expand();
                }
            }
        }

        private void LoadBillNodes(XmlNodeList xmlNodes, ExtTreeNode parentNode)
        {
            foreach (XmlNode node in xmlNodes)
            {
                if (!(node is XmlElement))
                {
                    continue;
                }
                XmlElement element = node as XmlElement;
                if ((element.ChildNodes.Count == 0) || (element.SelectNodes("Bill").Count == 0))
                {
                    BillNode node2 = new BillNode();
                    node2.ImageIndex = 2;
                    node2.SelectedImageIndex = 2;
                    foreach (XmlAttribute attribute in element.Attributes)
                    {
                        switch (attribute.Name)
                        {
                            case "name":
                            {
                                node2.Text = attribute.Value;
                                continue;
                            }
                            case "logo":
                            {
                                node2.Logo = attribute.Value;
                                continue;
                            }
                            case "formname":
                            {
                                node2.FormName = attribute.Value;
                                continue;
                            }
                            case "SQL":
                            {
                                node2.SQL = attribute.Value;
                                continue;
                            }
                            case "reportoid":
                            {
                                node2.ReportOID = attribute.Value;
                                continue;
                            }
                            case "addressprefix":
                            {
                                node2.AddressPrefix = attribute.Value;
                                continue;
                            }
                            case "taxtypeoid":
                            {
                                node2.TaxTypeOID = attribute.Value;
                                continue;
                            }
                        }
                    }
                    parentNode.Nodes.Add(node2);
                    continue;
                }
                ExtTreeNode node3 = new ExtTreeNode();
                node3.ImageIndex = 1;
                node3.SelectedImageIndex = 1;
                node3.Text = element.GetAttribute("name");
                parentNode.Nodes.Add(node3);
                this.LoadBillNodes(element.ChildNodes, node3);
            }
        }

        private void LoadBills()
        {
            string filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + Path.DirectorySeparatorChar + "Bills.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlElement documentElement = document.DocumentElement;
            if (documentElement != null)
            {
                ExtTreeNode parentNode = new ExtTreeNode();
                parentNode.ImageIndex = 1;
                parentNode.SelectedImageIndex = 1;
                parentNode.Text = documentElement.GetAttribute("name");
                this.LoadBillNodes(documentElement.ChildNodes, parentNode);
                this.billBrowserTreeView.Nodes.Add(parentNode);
                parentNode.Expand();
            }
        }

        private void ResetImageIndex(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    this.ResetImageIndex(node.Nodes);
                }
                else
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 3;
                }
            }
        }

        private void ShowClosedFolderIcon(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 0;
            }
        }

        private void ShowOpenFolderIcon(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
            }
        }

        private void TreeMouseDown(object sender, MouseEventArgs e)
        {
            TreeNode nodeAt = this.billBrowserTreeView.GetNodeAt(this.billBrowserTreeView.PointToClient(System.Windows.Forms.Control.MousePosition));
            if ((nodeAt != null) && (nodeAt.Nodes.Count == 0))
            {
                this.billBrowserTreeView.SelectedNode = nodeAt;
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.contentPanel;
            }
        }

        public static BillBrowser Instance
        {
            get
            {
                return instance;
            }
        }
    }
}

