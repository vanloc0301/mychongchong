namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public class WorkbenchTreePanel : UserControl
    {
        private WorkbenchTreeControl definitionObjectBrowserControl = new WorkbenchTreeControl();
        private ToolStripItem[] standardItems;
        private ToolStrip toolStrip;

        public WorkbenchTreePanel()
        {
            this.definitionObjectBrowserControl.Dock = DockStyle.Fill;
            base.Controls.Add(this.definitionObjectBrowserControl);
            this.toolStrip = ToolbarService.CreateToolStrip(this, "/Workbench/Pads/WorkbenchTree/ToolBar/Standard");
            if (this.toolStrip != null)
            {
                this.toolStrip.ShowItemToolTips = true;
                this.toolStrip.Dock = DockStyle.Top;
                this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
                this.toolStrip.Stretch = true;
                this.standardItems = new ToolStripItem[this.toolStrip.Items.Count];
                this.toolStrip.Items.CopyTo(this.standardItems, 0);
                base.Controls.Add(this.toolStrip);
                this.definitionObjectBrowserControl.TreeView.AfterSelect += new TreeViewEventHandler(this.TreeViewAfterSelect);
            }
        }

        public void Clear()
        {
            this.definitionObjectBrowserControl.Clear();
            this.UpdateToolStrip(null);
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateToolStrip((ObjectNode) e.Node);
        }

        private void UpdateToolStrip(ObjectNode node)
        {
            this.toolStrip.Items.Clear();
            this.toolStrip.Items.AddRange(this.standardItems);
            ToolbarService.UpdateToolbar(this.toolStrip);
            if ((node != null) && (node.ToolbarAddinTreePath != null))
            {
                this.toolStrip.Items.Add(new ToolStripSeparator());
                this.toolStrip.Items.AddRange((ToolStripItem[]) AddInTree.BuildItems(node.ToolbarAddinTreePath, node, false).ToArray(typeof(ToolStripItem)));
            }
        }

        public WorkbenchTreeControl DefinitionObjectBrowserControl
        {
            get
            {
                return this.definitionObjectBrowserControl;
            }
        }

        public ObjectNode SelectedNode
        {
            get
            {
                return this.definitionObjectBrowserControl.SelectedNode;
            }
        }
    }
}

