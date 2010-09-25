namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class WorkbenchTreeControl : UserControl, IHasPropertyViewContainer
    {
        private ObjectNode lastNode;
        private SkyMap.Net.Gui.PropertyViewContainer propertyViewContainer = new SkyMap.Net.Gui.PropertyViewContainer();
        private ExtTreeView treeView;

        public WorkbenchTreeControl()
        {
            this.InitializeComponent();
            this.treeView.CanClearSelection = false;
            this.treeView.IsSorted = false;
            this.treeView.BeforeSelect += new TreeViewCancelEventHandler(this.TreeViewBeforeSelect);
            this.treeView.DrawNode += new DrawTreeNodeEventHandler(this.TreeViewDrawNode);
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
            this.LoadNodes();
            PropertyView.PropertyValueChanged += new PropertyValueChangedEventHandler(this.DefinitionObjectPropertyValueChanged);
        }

        public void Clear()
        {
            this.treeView.Clear();
        }

        private void DefinitionObjectPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (this.lastNode != null)
            {
                this.lastNode.PropertyChanged();
            }
        }

        private void InitializeComponent()
        {
            this.treeView = new ExtTreeView();
            base.SuspendLayout();
            this.treeView.Dock = DockStyle.Fill;
            this.treeView.ImageIndex = -1;
            this.treeView.Location = new Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = -1;
            this.treeView.Size = new Size(0x124, 0x10a);
            this.treeView.TabIndex = 0;
            base.Controls.Add(this.treeView);
            base.Name = "ProjectBrowserControl";
            base.Size = new Size(0x124, 0x10a);
            base.ResumeLayout(false);
        }

        private void LoadNodes()
        {
            this.treeView.Nodes.Clear();
            List<TaskNode> topTaskNode = TaskNode.GetTopTaskNode();
            foreach (TaskNode node in topTaskNode)
            {
                try
                {
                    if (TaskTreeNode.IfAuthNode(node))
                    {
                        new TaskTreeNode(node).AddTo(this.treeView);
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            }
        }

        public void PadActivated()
        {
        }

        public void RefreshView()
        {
        }

        private void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            for (int i = WorkbenchSingleton.Workbench.ViewContentCollection.Count - 1; i > -1; i--)
            {
                IViewContent content = WorkbenchSingleton.Workbench.ViewContentCollection[i];
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("尝试关闭窗口 :" + content.TitleName);
                }
                content.WorkbenchWindow.CloseWindow(false);
            }
            this.LoadNodes();
        }

        private void TreeViewBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            LoggingService.InfoFormatted("新选中节点:{0}", new object[] { e.Node.Text });
            if (this.lastNode != e.Node)
            {
                if (((this.lastNode != null) && (PropertyView.Instance != null)) && (PropertyView.Instance.PropertyEditPanel != null))
                {
                    LoggingService.Info("检查数据是否已经修改...");
                    PropertyView.Instance.PropertyEditPanel.PostEditor();
                    if ((this.lastNode != null) && PropertyView.Instance.IsDirty)
                    {
                        if (MessageHelper.ShowYesNoInfo("数据已修改，是否保存？") == DialogResult.Yes)
                        {
                            try
                            {
                                PropertyView.Instance.Save();
                                this.lastNode.PropertyChanged();
                            }
                            catch (Exception exception)
                            {
                                LoggingService.ErrorFormatted("保存数据出错：{0}\r\n", new object[] { exception.Message, exception.StackTrace });
                                MessageHelper.ShowError("保存数据出错", exception);
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            PropertyView.Instance.CancelEdit();
                        }
                    }
                }
                ObjectNode node = e.Node as ObjectNode;
                if (node != null)
                {
                    this.lastNode = node;
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("will select object node...");
                    }
                    node.Select();
                }
            }
        }

        private void TreeViewDrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            ObjectNode node = e.Node as ObjectNode;
            if (node != null)
            {
                Image overlay = node.Overlay;
                if (overlay != null)
                {
                    e.Graphics.DrawImageUnscaled(overlay, e.Bounds.X - overlay.Width, e.Bounds.Bottom - overlay.Height);
                }
            }
        }

        public SkyMap.Net.Gui.PropertyViewContainer PropertyViewContainer
        {
            get
            {
                return this.propertyViewContainer;
            }
        }

        public ObjectNode SelectedNode
        {
            get
            {
                return (this.treeView.SelectedNode as ObjectNode);
            }
        }

        public ExtTreeView TreeView
        {
            get
            {
                return this.treeView;
            }
        }
    }
}

