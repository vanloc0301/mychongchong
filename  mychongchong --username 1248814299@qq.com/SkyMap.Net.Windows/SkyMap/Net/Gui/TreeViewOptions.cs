namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.XmlForms;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class TreeViewOptions : BaseSharpDevelopForm
    {
        protected bool b = true;
        protected Font boldFont = null;
        protected List<IDialogPanel> OptionPanels = new List<IDialogPanel>();
        protected GradientHeaderPanel optionsPanelLabel;
        protected Font plainFont = null;
        protected SkyMap.Net.Core.Properties properties = null;

        public TreeViewOptions(SkyMap.Net.Core.Properties properties, AddInTreeNode node)
        {
            this.properties = properties;
            this.Text = SkyMap.Net.Core.StringParser.Parse("${res:Dialog.Options.TreeViewOptions.DialogName}");
            this.InitializeComponent();
            this.plainFont = new Font(((TreeView) base.ControlDictionary["optionsTreeView"]).Font, FontStyle.Regular);
            this.boldFont = new Font(((TreeView) base.ControlDictionary["optionsTreeView"]).Font, FontStyle.Bold);
            this.InitImageList();
            if (node != null)
            {
                this.AddNodes(properties, ((TreeView) base.ControlDictionary["optionsTreeView"]).Nodes, new List<IDialogPanelDescriptor>((IDialogPanelDescriptor[]) node.BuildChildItems(this).ToArray(typeof(IDialogPanelDescriptor))));
            }
        }

        protected void AcceptEvent(object sender, EventArgs e)
        {
            foreach (AbstractOptionPanel panel in this.OptionPanels)
            {
                if (!panel.ReceiveDialogMessage(DialogMessage.OK))
                {
                    return;
                }
            }
            base.DialogResult = DialogResult.OK;
        }

        protected void AddNodes(object customizer, TreeNodeCollection nodes, List<IDialogPanelDescriptor> dialogPanelDescriptors)
        {
            nodes.Clear();
            foreach (IDialogPanelDescriptor descriptor in dialogPanelDescriptors)
            {
                if (descriptor.DialogPanel != null)
                {
                    descriptor.DialogPanel.CustomizationObject = customizer;
                    descriptor.DialogPanel.Control.Dock = DockStyle.Fill;
                    this.OptionPanels.Add(descriptor.DialogPanel);
                }
                TreeNode node = new TreeNode(descriptor.Label);
                node.Tag = descriptor;
                node.NodeFont = this.plainFont;
                nodes.Add(node);
                if (descriptor.ChildDialogPanelDescriptors != null)
                {
                    this.AddNodes(customizer, node.Nodes, descriptor.ChildDialogPanelDescriptors);
                }
            }
        }

        protected void BeforeExpandNode(object sender, TreeViewCancelEventArgs e)
        {
            if (this.b)
            {
                this.b = false;
                ((TreeView) base.ControlDictionary["optionsTreeView"]).BeginUpdate();
                TreeNode firstNode = e.Node.FirstNode;
                while (firstNode.Nodes.Count > 0)
                {
                    firstNode = firstNode.FirstNode;
                }
                ((TreeView) base.ControlDictionary["optionsTreeView"]).CollapseAll();
                firstNode.EnsureVisible();
                firstNode.ImageIndex = 3;
                ((TreeView) base.ControlDictionary["optionsTreeView"]).EndUpdate();
                this.SetOptionPanelTo(firstNode);
                this.b = true;
            }
        }

        protected void BeforeSelectNode(object sender, TreeViewCancelEventArgs e)
        {
            this.ResetImageIndex(((TreeView) base.ControlDictionary["optionsTreeView"]).Nodes);
            if (this.b)
            {
                this.CollapseOrExpandNode(e.Node);
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

        protected void HandleClick(object sender, EventArgs e)
        {
            if ((((TreeView) base.ControlDictionary["optionsTreeView"]).GetNodeAt(((TreeView) base.ControlDictionary["optionsTreeView"]).PointToClient(Control.MousePosition)) == ((TreeView) base.ControlDictionary["optionsTreeView"]).SelectedNode) && this.b)
            {
                this.CollapseOrExpandNode(((TreeView) base.ControlDictionary["optionsTreeView"]).SelectedNode);
            }
        }

        protected void InitializeComponent()
        {
            base.Owner = (Form) WorkbenchSingleton.Workbench;
            base.SetupFromXmlStream(base.GetType().Assembly.GetManifestResourceStream("Resources.TreeViewOptionsDialog.xfrm"));
            this.optionsPanelLabel = new GradientHeaderPanel();
            this.optionsPanelLabel.Font = new Font("Tahoma", 14f, FontStyle.Bold, GraphicsUnit.Pixel, 0);
            this.optionsPanelLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.optionsPanelLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionsPanelLabel.Dock = DockStyle.Fill;
            base.ControlDictionary["headerPanel"].Controls.Add(this.optionsPanelLabel);
            base.Icon = null;
            base.ControlDictionary["okButton"].Click += new EventHandler(this.AcceptEvent);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).Click += new EventHandler(this.HandleClick);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).AfterSelect += new TreeViewEventHandler(this.SelectNode);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).BeforeSelect += new TreeViewCancelEventHandler(this.BeforeSelectNode);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).BeforeExpand += new TreeViewCancelEventHandler(this.BeforeExpandNode);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).BeforeExpand += new TreeViewCancelEventHandler(this.ShowOpenFolderIcon);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).BeforeCollapse += new TreeViewCancelEventHandler(this.ShowClosedFolderIcon);
            ((TreeView) base.ControlDictionary["optionsTreeView"]).MouseDown += new MouseEventHandler(this.TreeMouseDown);
        }

        protected void InitImageList()
        {
            ImageList list = new ImageList();
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.Add(IconService.GetBitmap("Icons.16x16.ClosedFolderBitmap"));
            list.Images.Add(IconService.GetBitmap("Icons.16x16.OpenFolderBitmap"));
            list.Images.Add(new Bitmap(1, 1));
            list.Images.Add(IconService.GetBitmap("Icons.16x16.SelectionArrow"));
            ((TreeView) base.ControlDictionary["optionsTreeView"]).ImageList = list;
        }

        protected void ResetImageIndex(TreeNodeCollection nodes)
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

        protected void SelectNode(object sender, TreeViewEventArgs e)
        {
            this.SetOptionPanelTo(((TreeView) base.ControlDictionary["optionsTreeView"]).SelectedNode);
        }

        protected void SetOptionPanelTo(TreeNode node)
        {
            IDialogPanelDescriptor tag = node.Tag as IDialogPanelDescriptor;
            if (((tag != null) && (tag.DialogPanel != null)) && (tag.DialogPanel.Control != null))
            {
                tag.DialogPanel.ReceiveDialogMessage(DialogMessage.Activated);
                base.ControlDictionary["optionControlPanel"].Controls.Clear();
                RightToLeftConverter.ConvertRecursive(tag.DialogPanel.Control);
                base.ControlDictionary["optionControlPanel"].Controls.Add(tag.DialogPanel.Control);
                this.optionsPanelLabel.Text = tag.Label;
            }
        }

        protected void ShowClosedFolderIcon(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 0;
            }
        }

        protected void ShowOpenFolderIcon(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
            }
        }

        private void TreeMouseDown(object sender, MouseEventArgs e)
        {
            TreeNode nodeAt = ((TreeView) base.ControlDictionary["optionsTreeView"]).GetNodeAt(((TreeView) base.ControlDictionary["optionsTreeView"]).PointToClient(Control.MousePosition));
            if ((nodeAt != null) && (nodeAt.Nodes.Count == 0))
            {
                ((TreeView) base.ControlDictionary["optionsTreeView"]).SelectedNode = nodeAt;
            }
        }

        public SkyMap.Net.Core.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }
    }
}

