namespace SkyMap.Net.Gui
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using SkyMap.Net.DAO;

    public abstract class ObjectNode : ExtTreeNode, IDisposable
    {
        protected bool autoClearNodes = true;
        public const string Loading = "Loading";
        private Image overlay;
        private string toolbarAddinTreePath = null;

        public static  event TreeViewEventHandler AfterNodeInitialize;

        public ObjectNode()
        {
            this.InitChildNode();
        }

        public abstract ObjectNode AddChild();
        public void AddNodes<T, U>(IList<T> objs) where T: DomainObject where U: AbstractDomainObjectNode<T>, new()
        {
            foreach (T local in objs)
            {
                this.AddSingleNode<T, U>(local);
            }
        }

        public void AddNodes<T, U>(List<T> objs) where T: DomainObject where U: AbstractDomainObjectNode<T>, new()
        {
            foreach (T local in objs)
            {
                this.AddSingleNode<T, U>(local);
            }
        }

        public void AddNodes<T, U>(IList objs) where T: DomainObject where U: AbstractDomainObjectNode<T>, new()
        {
            foreach (object obj2 in objs)
            {
                this.AddSingleNode<T, U>((T) obj2);
            }
        }

        protected ObjectNode AddObjectNode(System.Type type)
        {
            ObjectNode node = (ObjectNode) Activator.CreateInstance(type);
            node.AddTo(this);
            return node;
        }

        public U AddSingleNode<T, U>(T obj) where T: DomainObject where U: AbstractDomainObjectNode<T>, new()
        {
            U local = Activator.CreateInstance<U>();
            local.DomainObject = obj;
            local.AddTo(this);
            return local;
        }

        public override void Expanding()
        {
            if (!base.isInitialized)
            {
                base.isInitialized = true;
                if (this.autoClearNodes)
                {
                    base.Nodes.Clear();
                }
                this.Initialize();
                base.UpdateVisibility();
            }
        }

        private void InitChildNode()
        {
            if (this.EnableAddChild)
            {
                base.Nodes.Add(new TreeNode("Loading"));
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (AfterNodeInitialize != null)
            {
                AfterNodeInitialize(null, new TreeViewEventArgs(this));
            }
        }

        public U InsertNode<T, U>(T obj, int index) where T: DomainObject where U: AbstractDomainObjectNode<T>, new()
        {
            U local = Activator.CreateInstance<U>();
            local.DomainObject = obj;
            local.InsertTo(this, index);
            return local;
        }

        public virtual void PropertyChanged()
        {
        }

        public virtual void SaveAs()
        {
            MessageHelper.ShowInfo("你选择的节点或其连接的对象没有实现SaveAs,不能进行该操作");
        }

        public virtual void Select()
        {
        }

        public virtual bool EnableAddChild
        {
            get
            {
                return true;
            }
        }

        public override bool EnableDelete
        {
            get
            {
                return false;
            }
        }

        public Image Overlay
        {
            get
            {
                return this.overlay;
            }
            set
            {
                if (this.overlay != value)
                {
                    this.overlay = value;
                    if ((base.TreeView != null) && base.IsVisible)
                    {
                        Rectangle bounds = base.Bounds;
                        bounds.Width += bounds.X;
                        bounds.X = 0;
                        base.TreeView.Invalidate(bounds);
                    }
                }
            }
        }

        public virtual string ToolbarAddinTreePath
        {
            get
            {
                return this.toolbarAddinTreePath;
            }
            set
            {
                this.toolbarAddinTreePath = value;
            }
        }
    }
}

