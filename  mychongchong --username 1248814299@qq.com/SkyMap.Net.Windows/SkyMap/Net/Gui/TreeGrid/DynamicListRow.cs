namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class DynamicListRow
    {
        private int height = 0x10;
        private DynamicListItem[] items = new DynamicListItem[10];

        public event EventHandler HeightChanged;

        public event EventHandler<DynamicListEventArgs> Hidden;

        public event EventHandler ItemChanged;

        public event EventHandler<DynamicListEventArgs> Shown;

        protected virtual DynamicListItem CreateItem()
        {
            return new DynamicListItem(this);
        }

        internal void NotifyListVisibilityChange(DynamicList list, bool visible)
        {
            if (visible)
            {
                this.OnShown(new DynamicListEventArgs(list));
            }
            else
            {
                this.OnHidden(new DynamicListEventArgs(list));
            }
        }

        protected virtual void OnHeightChanged(EventArgs e)
        {
            if (this.HeightChanged != null)
            {
                this.HeightChanged(this, e);
            }
        }

        protected virtual void OnHidden(DynamicListEventArgs e)
        {
            if (this.Hidden != null)
            {
                this.Hidden(this, e);
            }
        }

        protected virtual void OnItemChanged(EventArgs e)
        {
            if (this.ItemChanged != null)
            {
                this.ItemChanged(this, e);
            }
        }

        protected virtual void OnShown(DynamicListEventArgs e)
        {
            if (this.Shown != null)
            {
                this.Shown(this, e);
            }
        }

        internal void RaiseItemChanged(DynamicListItem item)
        {
            this.OnItemChanged(EventArgs.Empty);
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException("value", value, "value must be at least 2");
                }
                if (this.height != value)
                {
                    this.height = value;
                    this.OnHeightChanged(EventArgs.Empty);
                }
            }
        }

        public DynamicListItem this[int columnIndex]
        {
            [DebuggerStepThrough]
            get
            {
                if (columnIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("columnIndex", columnIndex, "columnIndex must be >= 0");
                }
                if (columnIndex > 0x3e8)
                {
                    throw new ArgumentOutOfRangeException("columnIndex", columnIndex, "columnIndex must be <= " + 0x3e8);
                }
                if (columnIndex >= this.items.Length)
                {
                    Array.Resize<DynamicListItem>(ref this.items, (columnIndex * 2) + 1);
                }
                DynamicListItem item = this.items[columnIndex];
                if (item == null)
                {
                    this.items[columnIndex] = item = this.CreateItem();
                }
                return item;
            }
        }
    }
}

