namespace SkyMap.Net.Gui
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using SkyMap.Net.Core;

    public class ExtTreeNode : TreeNode, IDisposable, IClipboardHandler
    {
        private static System.Drawing.Font boldFont = ResourceService.LoadFont("Tahoma", 9, FontStyle.Bold);
        private static System.Drawing.Font boldMonospacedFont = ResourceService.LoadFont("Courier New", 10, FontStyle.Bold);
        protected bool canLabelEdit = false;
        private string contextmenuAddinTreePath = null;
        private bool doPerformCut = false;
        protected bool drawDefault = true;
        private static System.Drawing.Font font = ResourceService.LoadFont("Tahoma", 9);
        private string image = null;
        private TreeNode internalParent;
        protected List<ExtTreeNode> invisibleNodes = new List<ExtTreeNode>();
        private bool isDisposed = false;
        protected bool isInitialized = false;
        private static System.Drawing.Font italicFont = ResourceService.LoadFont("Tahoma", 9, FontStyle.Italic);
        private static System.Drawing.Font italicMonospacedFont = ResourceService.LoadFont("Courier New", 10, FontStyle.Italic);
        private static System.Drawing.Font monospacedFont = ResourceService.LoadFont("Courier New", 10);
        protected int sortOrder = 0;

        public virtual void ActivateItem()
        {
            base.Toggle();
        }

        public void AddTo(TreeNode node)
        {
            this.internalParent = node;
            this.AddTo(node.Nodes);
        }

        private void AddTo(TreeNodeCollection nodes)
        {
            nodes.Add(this);
            this.Refresh();
        }

        public void AddTo(TreeView view)
        {
            this.internalParent = null;
            this.AddTo(view.Nodes);
        }

        public virtual void AfterLabelEdit(string newName)
        {
            throw new NotImplementedException();
        }

        public virtual void BeforeLabelEdit()
        {
        }

        public virtual void CheckedChanged()
        {
        }

        public virtual void Collapsing()
        {
        }

        public virtual void Copy()
        {
            throw new NotImplementedException();
        }

        public virtual void Cut()
        {
            throw new NotImplementedException();
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            this.isDisposed = true;
            foreach (TreeNode node in base.Nodes)
            {
                if (node is IDisposable)
                {
                    ((ExtTreeNode)node).Dispose();
                }
            }
        }

        public virtual void DoDragDrop(IDataObject dataObject, DragDropEffects effect)
        {
            throw new NotImplementedException();
        }

        public void Draw(DrawTreeNodeEventArgs e)
        {
            this.DrawBackground(e);
            this.DrawForeground(e);
        }

        protected virtual void DrawBackground(DrawTreeNodeEventArgs e)
        {
            Graphics graphics = e.Graphics;
            int width = this.MeasureItemWidth(e);
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, width, e.Bounds.Height);
            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                graphics.FillRectangle(SystemBrushes.Highlight, rect);
            }
            else
            {
                graphics.FillRectangle(SystemBrushes.Window, rect);
            }
            if ((e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused)
            {
                rect.Width--;
                rect.Height--;
                graphics.DrawRectangle(SystemPens.HighlightText, rect);
            }
        }

        protected virtual void DrawForeground(DrawTreeNodeEventArgs e)
        {
        }

        protected void DrawText(Graphics g, string text, Brush brush, System.Drawing.Font font, ref float x, int y)
        {
            if (base.IsSelected)
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            g.DrawString(text, font, brush, new PointF(x, (float)y));
            SizeF ef = g.MeasureString(text, font);
            x += ef.Width;
        }

        public virtual void Expanding()
        {
            this.PerformInitialization();
        }

        public virtual DragDropEffects GetDragDropEffect(IDataObject dataObject, DragDropEffects proposedEffect)
        {
            return DragDropEffects.None;
        }

        protected Color GetTextColor(TreeNodeStates state, Color c)
        {
            if ((state & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                return SystemColors.HighlightText;
            }
            return c;
        }

        protected virtual void Initialize()
        {
        }

        public void InsertTo(TreeNode node, int index)
        {
            this.internalParent = node;
            this.InsertTo(node.Nodes, index);
        }

        private void InsertTo(TreeNodeCollection nodes, int index)
        {
            nodes.Insert(index, this);
            this.Refresh();
        }

        protected virtual int MeasureItemWidth(DrawTreeNodeEventArgs e)
        {
            return this.MeasureTextWidth(e.Graphics, base.Text, base.TreeView.Font);
        }

        protected int MeasureTextWidth(Graphics g, string text, System.Drawing.Font font)
        {
            return (int)g.MeasureString(text, font).Width;
        }

        public virtual void Paste()
        {
            throw new NotImplementedException();
        }

        public void PerformInitialization()
        {
            if (!this.isInitialized)
            {
                this.Initialize();
                this.isInitialized = true;
            }
        }

        public virtual void Refresh()
        {
            this.SetIcon(this.image);
            foreach (TreeNode node in base.Nodes)
            {
                if (node is ExtTreeNode)
                {
                    ((ExtTreeNode)node).Refresh();
                }
            }
        }

        public virtual void SelectAll()
        {
            throw new NotImplementedException();
        }

        public void SetIcon(string iconName)
        {
            if (iconName != null)
            {
                this.image = iconName;
                ExtTreeView treeView = base.TreeView as ExtTreeView;
                if (treeView != null)
                {
                    int imageIndexForImage = treeView.GetImageIndexForImage(iconName, this.DoPerformCut);
                    if (base.ImageIndex != imageIndexForImage)
                    {
                        base.ImageIndex = base.SelectedImageIndex = imageIndexForImage;
                    }
                }
            }
        }

        public virtual void UpdateVisibility()
        {
            int index = 0;
            while (index < this.invisibleNodes.Count)
            {
                if (this.invisibleNodes[index].Visible)
                {
                    this.invisibleNodes[index].AddTo(this);
                    this.invisibleNodes.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            foreach (TreeNode node in base.Nodes)
            {
                if (node is ExtTreeNode)
                {
                    ExtTreeNode item = (ExtTreeNode)node;
                    if (!item.Visible)
                    {
                        this.invisibleNodes.Add(item);
                    }
                }
            }
            foreach (TreeNode node in this.invisibleNodes)
            {
                base.Nodes.Remove(node);
            }
            foreach (TreeNode node in base.Nodes)
            {
                if (node is ExtTreeNode)
                {
                    ((ExtTreeNode)node).UpdateVisibility();
                }
            }
        }

        public IEnumerable<ExtTreeNode> AllNodes
        {
            get
            {
                Cgetallnodes d__ = new Cgetallnodes(-2);
                d__._a4this = this;
                return d__;
            }
        }

        public static System.Drawing.Font BoldFont
        {
            get
            {
                return boldFont;
            }
        }

        public static System.Drawing.Font BoldMonospacedFont
        {
            get
            {
                return boldMonospacedFont;
            }
        }

        public virtual bool CanLabelEdit
        {
            get
            {
                return this.canLabelEdit;
            }
        }

        public virtual string CompareString
        {
            get
            {
                return base.Text;
            }
        }

        public virtual string ContextmenuAddinTreePath
        {
            get
            {
                return this.contextmenuAddinTreePath;
            }
            set
            {
                this.contextmenuAddinTreePath = value;
            }
        }

        public virtual bool DoPerformCut
        {
            get
            {
                ExtTreeNode parent = this.Parent as ExtTreeNode;
                return ((parent == null) ? this.doPerformCut : (this.doPerformCut | parent.DoPerformCut));
            }
            set
            {
                this.doPerformCut = value;
                if (this.doPerformCut)
                {
                    ((ExtTreeView)base.TreeView).CutNodes.Add(this);
                }
                this.Refresh();
            }
        }

        public virtual DataObject DragDropDataObject
        {
            get
            {
                return null;
            }
        }

        public bool DrawDefault
        {
            get
            {
                return this.drawDefault;
            }
        }

        public virtual bool EnableCopy
        {
            get
            {
                return false;
            }
        }

        public virtual bool EnableCut
        {
            get
            {
                return false;
            }
        }

        public virtual bool EnableDelete
        {
            get
            {
                return false;
            }
        }

        public virtual bool EnablePaste
        {
            get
            {
                return false;
            }
        }

        public virtual bool EnableSelectAll
        {
            get
            {
                return false;
            }
        }

        public static System.Drawing.Font Font
        {
            get
            {
                return font;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }

        internal bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
        }

        public static System.Drawing.Font ItalicFont
        {
            get
            {
                return italicFont;
            }
        }

        public static System.Drawing.Font ItalicMonospacedFont
        {
            get
            {
                return italicMonospacedFont;
            }
        }

        public static System.Drawing.Font MonospacedFont
        {
            get
            {
                return monospacedFont;
            }
        }

        public TreeNode Parent
        {
            get
            {
                return this.internalParent;
            }
        }

        public virtual int SortOrder
        {
            get
            {
                return this.sortOrder;
            }
        }

        public virtual bool Visible
        {
            get
            {
                return true;
            }
        }

        //    [CompilerGenerated]
        private sealed class Cgetallnodes : IEnumerable<ExtTreeNode>, IEnumerable, IEnumerator<ExtTreeNode>, IEnumerator, IDisposable
        {
            private int _a1state;
            private ExtTreeNode _a2current;
            public ExtTreeNode _a4this;
            public IEnumerator _a7wrap3;
            public IDisposable _a7wrap4;
            public List<ExtTreeNode>.Enumerator _a7wrap6;
            private int l__initialThreadId;
            public ExtTreeNode n_a51;
            public ExtTreeNode n_a52;

            //      [DebuggerHidden]
            public Cgetallnodes(int _a1state)
            {
                this._a1state = _a1state;
                this.l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void m__Finally5()
            {
                this._a1state = -1;
                this._a7wrap4 = this._a7wrap3 as IDisposable;
                if (this._a7wrap4 != null)
                {
                    this._a7wrap4.Dispose();
                }
            }

            private void m__Finally7()
            {
                this._a1state = -1;
                this._a7wrap6.Dispose();
            }

            bool System.Collections.IEnumerator.MoveNext()
            {
                try
                {
                    switch (this._a1state)
                    {
                        case 0:
                            this._a1state = -1;
                            this._a7wrap3 = this._a4this.Nodes.GetEnumerator();
                            this._a1state = 1;
                            while (this._a7wrap3.MoveNext())
                            {
                                this.n_a51 = (ExtTreeNode)this._a7wrap3.Current;
                                this._a2current = this.n_a51;
                                this._a1state = 2;
                                return true;
                            }
                            aa:
                            this._a1state = 1;
                            this.m__Finally5();
                            this._a7wrap6 = this._a4this.invisibleNodes.GetEnumerator();
                            this._a1state = 3;
                            while (this._a7wrap6.MoveNext())
                            {
                                this.n_a52 = this._a7wrap6.Current;
                                this._a2current = this.n_a52;
                                this._a1state = 4;
                                return true;
                            }
                            bb:
                            this._a1state = 3;
                            this.m__Finally7();
                            break;

                        case 2:
                            goto aa;
                        // break;

                        case 4:
                            goto bb;
                        // break;
                    }
                    return false;
                }
                finally
                {
                    (this as System.IDisposable).Dispose();
                }
            }

            //    [DebuggerHidden]
            IEnumerator<ExtTreeNode> IEnumerable<ExtTreeNode>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.l__initialThreadId) && (this._a1state == -2))
                {
                    this._a1state = 0;
                    return this;
                }
                ExtTreeNode.Cgetallnodes d__ = new ExtTreeNode.Cgetallnodes(0);
                d__._a4this = this._a4this;
                return d__;
            }

            //    [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                // return IEnumerable<SkyMap.Net.Gui.ExtTreeNode>.GetEnumerator() as IEnumerator;
                return (this as System.Collections.Generic.IEnumerable<SkyMap.Net.Gui.ExtTreeNode>).GetEnumerator();
            }

            //   [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this._a1state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.m__Finally5();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.m__Finally7();
                        }
                        break;
                }
            }

            ExtTreeNode IEnumerator<ExtTreeNode>.Current
            {
                //[DebuggerHidden]
                get
                {
                    return this._a2current;
                }
            }

            object IEnumerator.Current
            {
                //[DebuggerHidden]
                get
                {
                    return this._a2current;
                }
            }
        }
    }
}

