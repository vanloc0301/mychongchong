namespace SkyMap.Net.Gui
{
    using System;
    using System.Windows.Forms;

    public class WorkbenchTreePad : AbstractPadContent, IClipboardHandler, IHasPropertyViewContainer
    {
        private WorkbenchTreePanel definitionObjectBrowserPanel = new WorkbenchTreePanel();
        private static WorkbenchTreePad instance;

        public WorkbenchTreePad()
        {
            instance = this;
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindowChanged += new EventHandler(this.ActiveWindowChanged);
            this.ActiveWindowChanged(null, null);
        }

        private void ActiveWindowChanged(object sender, EventArgs e)
        {
            if (WorkbenchSingleton.Workbench.ActiveContent == this)
            {
                this.definitionObjectBrowserPanel.DefinitionObjectBrowserControl.PadActivated();
            }
        }

        public void Copy()
        {
            this.DefinitionObjectBrowserControl.TreeView.ClearCutNodes();
            ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
            if (selectedNode != null)
            {
                selectedNode.Copy();
            }
        }

        public void Cut()
        {
            this.DefinitionObjectBrowserControl.TreeView.ClearCutNodes();
            ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
            if (selectedNode != null)
            {
                selectedNode.Cut();
            }
        }

        public void Delete()
        {
            ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
            if (selectedNode != null)
            {
                selectedNode.Delete();
            }
            this.DefinitionObjectBrowserControl.TreeView.ClearCutNodes();
        }

        public void Paste()
        {
            ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
            if (selectedNode != null)
            {
                selectedNode.Paste();
            }
            this.DefinitionObjectBrowserControl.TreeView.ClearCutNodes();
        }

        public void SelectAll()
        {
            ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
            if (selectedNode != null)
            {
                selectedNode.SelectAll();
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.definitionObjectBrowserPanel;
            }
        }

        public WorkbenchTreeControl DefinitionObjectBrowserControl
        {
            get
            {
                return this.definitionObjectBrowserPanel.DefinitionObjectBrowserControl;
            }
        }

        public bool EnableCopy
        {
            get
            {
                ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
                return ((selectedNode != null) ? selectedNode.EnableCopy : false);
            }
        }

        public bool EnableCut
        {
            get
            {
                ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
                return ((selectedNode != null) ? selectedNode.EnableCut : false);
            }
        }

        public bool EnableDelete
        {
            get
            {
                ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
                return ((selectedNode != null) ? selectedNode.EnableDelete : false);
            }
        }

        public bool EnablePaste
        {
            get
            {
                ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
                return ((selectedNode != null) ? selectedNode.EnablePaste : false);
            }
        }

        public bool EnableSelectAll
        {
            get
            {
                ExtTreeNode selectedNode = this.DefinitionObjectBrowserControl.TreeView.SelectedNode as ExtTreeNode;
                return ((selectedNode != null) ? selectedNode.EnableSelectAll : false);
            }
        }

        public static WorkbenchTreePad Instance
        {
            get
            {
                if (instance == null)
                {
                    WorkbenchSingleton.Workbench.GetPad(typeof(WorkbenchTreePad)).CreatePad();
                }
                return instance;
            }
        }

        public SkyMap.Net.Gui.PropertyViewContainer PropertyViewContainer
        {
            get
            {
                return this.definitionObjectBrowserPanel.DefinitionObjectBrowserControl.PropertyViewContainer;
            }
        }

        public ObjectNode SelectedNode
        {
            get
            {
                return this.definitionObjectBrowserPanel.SelectedNode;
            }
        }
    }
}

