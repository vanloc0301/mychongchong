namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class ExtTreeView : TreeView
    {
        private bool canClearSelection = true;
        private List<ExtTreeNode> cutNodes = new List<ExtTreeNode>();
        private Dictionary<string, int> imageIndexTable = new Dictionary<string, int>();
        private bool inRefresh;
        private bool isSorted = true;
        private int mouseClickNum;
        private IComparer<TreeNode> nodeSorter = new ExtTreeViewComparer();

        public ExtTreeView()
        {
            base.DrawMode = TreeViewDrawMode.OwnerDrawText;
            base.HideSelection = false;
            this.AllowDrop = true;
            ImageList list = new ImageList();
            list.ImageSize = new Size(0x10, 0x10);
            list.ColorDepth = ColorDepth.Depth32Bit;
            base.ImageList = list;
            this.Font = new Font("宋体", 11.2f);
        }

        public void Clear()
        {
            if (!base.IsDisposed)
            {
                TreeNode[] dest = new TreeNode[base.Nodes.Count];
                base.Nodes.CopyTo(dest, 0);
                base.Nodes.Clear();
                foreach (TreeNode node in dest)
                {
                    if (node is IDisposable)
                    {
                        ((IDisposable) node).Dispose();
                    }
                }
            }
        }

        public void ClearCutNodes()
        {
            foreach (ExtTreeNode node in this.CutNodes)
            {
                node.DoPerformCut = false;
            }
            this.CutNodes.Clear();
        }

        public int GetImageIndexForImage(string image, bool performCutBitmap)
        {
            string key = performCutBitmap ? (image + "_ghost") : image;
            if (!this.imageIndexTable.ContainsKey(key))
            {
                try
                {
                    base.ImageList.Images.Add(performCutBitmap ? IconService.GetGhostBitmap(image) : IconService.GetBitmap(image));
                    this.imageIndexTable[key] = base.ImageList.Images.Count - 1;
                    return (base.ImageList.Images.Count - 1);
                }
                catch
                {
                    LoggingService.WarnFormatted("没有找到图标:{0}", new object[] { image });
                    return 0;
                }
            }
            return this.imageIndexTable[key];
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);
            ExtTreeNode node = e.Node as ExtTreeNode;
            if (node != null)
            {
                node.CheckedChanged();
            }
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
            if (this.inRefresh)
            {
                this.inRefresh = false;
                base.EndUpdate();
            }
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            base.OnAfterLabelEdit(e);
            base.LabelEdit = false;
            e.CancelEdit = true;
            ExtTreeNode node = e.Node as ExtTreeNode;
            if ((node != null) && (e.Label != null))
            {
                node.AfterLabelEdit(e.Label);
            }
            this.SortParentNodes(e.Node);
        }

        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            if (this.mouseClickNum == 2)
            {
                this.mouseClickNum = 0;
                e.Cancel = true;
            }
            else
            {
                base.OnBeforeCollapse(e);
                if (e.Node is ExtTreeNode)
                {
                    ((ExtTreeNode) e.Node).Collapsing();
                }
            }
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            if (this.mouseClickNum == 2)
            {
                this.mouseClickNum = 0;
                e.Cancel = true;
            }
            else
            {
                base.OnBeforeExpand(e);
                if (e.Node != null)
                {
                    try
                    {
                        if (e.Node is ExtTreeNode)
                        {
                            if (!((ExtTreeNode) e.Node).IsInitialized && !this.inRefresh)
                            {
                                this.inRefresh = true;
                                base.BeginUpdate();
                            }
                            ((ExtTreeNode) e.Node).Expanding();
                        }
                        if (this.inRefresh)
                        {
                            this.SortNodes(e.Node.Nodes, false);
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageService.ShowError(exception);
                    }
                    if ((e.Node.Nodes.Count == 0) && this.inRefresh)
                    {
                        this.inRefresh = false;
                        base.EndUpdate();
                    }
                }
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);
            ExtTreeNode node = e.Node as ExtTreeNode;
            if (node != null)
            {
                node.ContextMenuStrip = MenuService.CreateContextMenu(e.Node, node.ContextmenuAddinTreePath);
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            Point pt = base.PointToClient(new Point(e.X, e.Y));
            ExtTreeNode nodeAt = base.GetNodeAt(pt) as ExtTreeNode;
            if (nodeAt != null)
            {
                nodeAt.DoDragDrop(e.Data, e.Effect);
                this.SortParentNodes(nodeAt);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            e.Effect = DragDropEffects.Move | DragDropEffects.Copy;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            Point pt = base.PointToClient(new Point(e.X, e.Y));
            ExtTreeNode nodeAt = base.GetNodeAt(pt) as ExtTreeNode;
            if (nodeAt != null)
            {
                DragDropEffects none = DragDropEffects.None;
                if ((e.KeyState & 8) > 0)
                {
                    none = DragDropEffects.Copy;
                }
                else
                {
                    none = DragDropEffects.Move;
                }
                e.Effect = nodeAt.GetDragDropEffect(e.Data, none);
                if (e.Effect != DragDropEffects.None)
                {
                    base.SelectedNode = nodeAt;
                }
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (!this.inRefresh)
            {
                ExtTreeNode node = e.Node as ExtTreeNode;
                if (!((node == null) || node.DrawDefault))
                {
                    node.Draw(e);
                    e.DrawDefault = false;
                }
                else if ((e.State & (TreeNodeStates.Focused | TreeNodeStates.Selected)) == TreeNodeStates.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                    e.Graphics.DrawString(e.Node.Text, this.Font, SystemBrushes.ControlText, (PointF) e.Bounds.Location);
                    e.DrawDefault = false;
                }
                else
                {
                    e.DrawDefault = true;
                }
            }
            else
            {
                e.DrawDefault = false;
            }
            base.OnDrawNode(e);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            ExtTreeNode item = e.Item as ExtTreeNode;
            if (item != null)
            {
                DataObject dragDropDataObject = item.DragDropDataObject;
                if (dragDropDataObject != null)
                {
                    base.DoDragDrop(dragDropDataObject, DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll);
                    this.SortParentNodes(item);
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == '\r')
            {
                ExtTreeNode selectedNode = base.SelectedNode as ExtTreeNode;
                if (selectedNode != null)
                {
                    selectedNode.ActivateItem();
                }
                e.Handled = true;
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            ExtTreeNode nodeAt = base.GetNodeAt(e.Location) as ExtTreeNode;
            if (nodeAt != null)
            {
                nodeAt.ActivateItem();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.mouseClickNum = e.Clicks;
            base.OnMouseDown(e);
            TreeNode nodeAt = base.GetNodeAt(e.X, e.Y);
            if (nodeAt != null)
            {
                if (base.SelectedNode != nodeAt)
                {
                    base.SelectedNode = nodeAt;
                }
            }
            else if (this.canClearSelection)
            {
                base.SelectedNode = null;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mouseClickNum = 0;
            base.OnMouseUp(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                this.StartLabelEdit(base.SelectedNode as ExtTreeNode);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void Sort()
        {
            this.SortNodes(base.Nodes, true);
        }

        public void SortNodes(TreeNodeCollection nodes, bool recursive)
        {
            if (this.isSorted)
            {
                TreeNode[] dest = new TreeNode[nodes.Count];
                nodes.CopyTo(dest, 0);
                Array.Sort<TreeNode>(dest, this.nodeSorter);
                nodes.Clear();
                nodes.AddRange(dest);
                if (recursive)
                {
                    foreach (TreeNode node in dest)
                    {
                        this.SortNodes(node.Nodes, true);
                    }
                }
            }
        }

        private void SortParentNodes(TreeNode treeNode)
        {
            TreeNode parent = treeNode.Parent;
            this.SortNodes((parent == null) ? base.Nodes : parent.Nodes, false);
        }

        public void StartLabelEdit(ExtTreeNode node)
        {
            if ((node != null) && node.CanLabelEdit)
            {
                node.EnsureVisible();
                base.SelectedNode = node;
                base.LabelEdit = true;
                node.BeforeLabelEdit();
                node.BeginEdit();
            }
        }

        public bool CanClearSelection
        {
            get
            {
                return this.canClearSelection;
            }
            set
            {
                this.canClearSelection = value;
            }
        }

        public List<ExtTreeNode> CutNodes
        {
            get
            {
                return this.cutNodes;
            }
        }

        public bool IsSorted
        {
            get
            {
                return this.isSorted;
            }
            set
            {
                this.isSorted = value;
            }
        }

        public IComparer<TreeNode> NodeSorter
        {
            get
            {
                return this.nodeSorter;
            }
            set
            {
                this.nodeSorter = value;
            }
        }

        [Obsolete("Use IsSorted instead!")]
        public bool Sorted
        {
            get
            {
                return base.Sorted;
            }
            set
            {
                base.Sorted = value;
            }
        }

        [Obsolete("Use NodeSorter instead!")]
        public IComparer TreeViewNodeSorter
        {
            get
            {
                return base.TreeViewNodeSorter;
            }
            set
            {
                base.TreeViewNodeSorter = value;
            }
        }
    }
}

