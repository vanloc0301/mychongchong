namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class DynamicList : UserControl, VerticalScrollContainer.IScrollable
    {
        private List<Control> allowedControls;
        private readonly CollectionWithEvents<DynamicListColumn> columns;
        public static readonly Color DefaultBackColor = Color.White;
        private bool inRecalculateColumnWidths;
        private bool inRecalculateNeedsRedraw;
        private bool inUpdate;
        private DynamicListItem itemAtMousePosition;
        private int lineMarginY;
        public const int MaxColumnCount = 0x3e8;
        private bool oldVisible;
        private List<Control> removedControls;
        private DynamicListRow rowAtMousePosition;
        private readonly CollectionWithEvents<DynamicListRow> rows;
        private int scrollOffset;

        public DynamicList()
            : this(new CollectionWithEvents<DynamicListColumn>(), new CollectionWithEvents<DynamicListRow>())
        {
        }

        public DynamicList(CollectionWithEvents<DynamicListColumn> columns, CollectionWithEvents<DynamicListRow> rows)
        {
            this.oldVisible = false;
            this.allowedControls = new List<Control>();
            this.removedControls = new List<Control>();
            this.scrollOffset = 0;
            this.lineMarginY = 0;
            this.inUpdate = true;
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            this.columns = columns;
            this.rows = rows;
            foreach (DynamicListColumn column in columns)
            {
                this.OnColumnAdded(null, new CollectionItemEventArgs<DynamicListColumn>(column));
            }
            foreach (DynamicListRow row in rows)
            {
                this.OnRowAdded(null, new CollectionItemEventArgs<DynamicListRow>(row));
            }
            columns.Added += new EventHandler<CollectionItemEventArgs<DynamicListColumn>>(this.OnColumnAdded);
            columns.Removed += new EventHandler<CollectionItemEventArgs<DynamicListColumn>>(this.OnColumnRemoved);
            rows.Added += new EventHandler<CollectionItemEventArgs<DynamicListRow>>(this.OnRowAdded);
            rows.Removed += new EventHandler<CollectionItemEventArgs<DynamicListRow>>(this.OnRowRemoved);
            this.BackColor = DefaultBackColor;
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Selectable, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.inUpdate = false;
            this.RecalculateColumnWidths();
        }

        public void BeginUpdate()
        {
            this.inUpdate = true;
        }

        private void ColumnMinimumWidthChanged(object sender, EventArgs e)
        {
            this.RecalculateColumnWidths();
        }

        private void ColumnWidthChanged(object sender, EventArgs e)
        {
            if (this.inRecalculateColumnWidths)
            {
                this.inRecalculateNeedsRedraw = true;
            }
            else
            {
                this.Redraw();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.columns.Added -= new EventHandler<CollectionItemEventArgs<DynamicListColumn>>(this.OnColumnAdded);
                this.columns.Removed -= new EventHandler<CollectionItemEventArgs<DynamicListColumn>>(this.OnColumnRemoved);
                this.rows.Added -= new EventHandler<CollectionItemEventArgs<DynamicListRow>>(this.OnRowAdded);
                this.rows.Removed -= new EventHandler<CollectionItemEventArgs<DynamicListRow>>(this.OnRowRemoved);
                foreach (DynamicListColumn column in this.columns)
                {
                    this.OnColumnRemoved(null, new CollectionItemEventArgs<DynamicListColumn>(column));
                }
                foreach (DynamicListRow row in this.rows)
                {
                    this.OnRowRemoved(null, new CollectionItemEventArgs<DynamicListRow>(row));
                }
            }
        }

        public void EndUpdate()
        {
            this.inUpdate = false;
            this.RecalculateColumnWidths();
        }

        public int GetColumnIndexFromPoint(int xPos)
        {
            int num = 0;
            int num2 = 0;
            foreach (DynamicListColumn column in this.Columns)
            {
                if (xPos < num2)
                {
                    break;
                }
                if (xPos <= (num2 + column.Width))
                {
                    return num;
                }
                num2 += column.Width + 1;
                num++;
            }
            return -1;
        }

        public DynamicListItem GetItemFromPoint(Point position)
        {
            DynamicListRow rowFromPoint = this.GetRowFromPoint(position.Y);
            if (rowFromPoint == null)
            {
                return null;
            }
            int columnIndexFromPoint = this.GetColumnIndexFromPoint(position.X);
            if (columnIndexFromPoint < 0)
            {
                return null;
            }
            return rowFromPoint[columnIndexFromPoint];
        }

        public Point GetPositionFromRow(DynamicListRow row)
        {
            int y = -this.scrollOffset;
            foreach (DynamicListRow row2 in this.Rows)
            {
                if (row2 == row)
                {
                    return new Point(0, y);
                }
                y += row2.Height + this.lineMarginY;
            }
            throw new ArgumentException("The row in not in this list!");
        }

        public int GetRequiredWidth(Graphics graphics)
        {
            int num = 0;
            for (int i = 0; i < this.columns.Count; i++)
            {
                if (this.columns[i].AutoSize)
                {
                    int minimumWidth = 0x10;
                    foreach (DynamicListRow row in this.Rows)
                    {
                        row[i].MeasureMinimumWidth(graphics, ref minimumWidth);
                    }
                    num += minimumWidth;
                }
                else
                {
                    num += this.columns[i].Width;
                }
                num++;
            }
            return num;
        }

        public DynamicListRow GetRowFromPoint(int yPos)
        {
            int num = -this.scrollOffset;
            foreach (DynamicListRow row in this.Rows)
            {
                if (yPos < num)
                {
                    break;
                }
                if (yPos <= (num + row.Height))
                {
                    return row;
                }
                num += row.Height + this.lineMarginY;
            }
            return null;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            DynamicListItem itemFromPoint = this.GetItemFromPoint(base.PointToClient(Control.MousePosition));
            if (itemFromPoint != null)
            {
                itemFromPoint.PerformClick(this);
            }
        }

        private void OnColumnAdded(object sender, CollectionItemEventArgs<DynamicListColumn> e)
        {
            e.Item.MinimumWidthChanged += new EventHandler(this.ColumnMinimumWidthChanged);
            e.Item.WidthChanged += new EventHandler(this.ColumnWidthChanged);
            this.RecalculateColumnWidths();
        }

        private void OnColumnRemoved(object sender, CollectionItemEventArgs<DynamicListColumn> e)
        {
            e.Item.MinimumWidthChanged -= new EventHandler(this.ColumnMinimumWidthChanged);
            e.Item.WidthChanged -= new EventHandler(this.ColumnWidthChanged);
            this.RecalculateColumnWidths();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            DynamicListItem itemFromPoint = this.GetItemFromPoint(base.PointToClient(Control.MousePosition));
            if (itemFromPoint != null)
            {
                itemFromPoint.PerformDoubleClick(this);
            }
        }

        protected virtual void OnLeaveItem(DynamicListItem item)
        {
            this.itemAtMousePosition.OnMouseLeave(this);
            this.Cursor = Cursors.Default;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DynamicListItem itemFromPoint = this.GetItemFromPoint(e.Location);
            if (itemFromPoint != null)
            {
                itemFromPoint.OnMouseDown(new DynamicListMouseEventArgs(this, e));
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            DynamicListItem itemFromPoint = this.GetItemFromPoint(base.PointToClient(Control.MousePosition));
            if (itemFromPoint != null)
            {
                itemFromPoint.OnMouseHover(this);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.rowAtMousePosition = null;
            if (this.itemAtMousePosition != null)
            {
                this.OnLeaveItem(this.itemAtMousePosition);
                this.itemAtMousePosition = null;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            DynamicListRow rowFromPoint = this.GetRowFromPoint(e.Y);
            if (this.rowAtMousePosition != rowFromPoint)
            {
                this.rowAtMousePosition = rowFromPoint;
                base.Invalidate();
            }
            if (rowFromPoint != null)
            {
                int columnIndexFromPoint = this.GetColumnIndexFromPoint(e.X);
                if (columnIndexFromPoint >= 0)
                {
                    DynamicListItem item = rowFromPoint[columnIndexFromPoint];
                    if (this.itemAtMousePosition != item)
                    {
                        if (this.itemAtMousePosition != null)
                        {
                            this.OnLeaveItem(this.itemAtMousePosition);
                        }
                        base.ResetMouseEventArgs();
                        this.itemAtMousePosition = item;
                        if (item != null)
                        {
                            if (item.Cursor != null)
                            {
                                this.Cursor = item.Cursor;
                            }
                            item.OnMouseEnter(this);
                        }
                    }
                    if (item != null)
                    {
                        item.OnMouseMove(new DynamicListMouseEventArgs(this, e));
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            DynamicListItem itemFromPoint = this.GetItemFromPoint(e.Location);
            if (itemFromPoint != null)
            {
                itemFromPoint.OnMouseUp(new DynamicListMouseEventArgs(this, e));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DynamicListColumn current;
            DynamicListItem item;
            int num4;
            bool isActivated;
            Graphics graphics = e.Graphics;
            this.allowedControls.Clear();
            int num = -1;
            using (IEnumerator<DynamicListColumn> enumerator = this.Columns.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    num++;
                    if (current.AutoSize)
                    {
                        int minimumWidth = 0x10;
                        foreach (DynamicListRow row in this.Rows)
                        {
                            item = row[num];
                            item.MeasureMinimumWidth(e.Graphics, ref minimumWidth);
                        }
                        current.MinimumWidth = minimumWidth;
                    }
                }
            }
            int newIndex = 0;
            int y = -this.scrollOffset;
            Size clientSize = base.ClientSize;
            foreach (DynamicListRow row in this.Rows)
            {
                if (((y + row.Height) > 0) && (y < clientSize.Height))
                {
                    num4 = 0;
                    for (num = 0; num < this.columns.Count; num++)
                    {
                        current = this.columns[num];
                        Rectangle rectangle = new Rectangle(num4, y, current.Width, row.Height);
                        if (num == (this.columns.Count - 1))
                        {
                            rectangle.Width = (clientSize.Width - 1) - rectangle.Left;
                        }
                        item = row[num];
                        Control control = item.Control;
                        if (control != null)
                        {
                            this.allowedControls.Add(control);
                            if (rectangle != control.Bounds)
                            {
                                control.Bounds = rectangle;
                            }
                            if (!base.Controls.Contains(control))
                            {
                                base.Controls.Add(control);
                                base.Controls.SetChildIndex(control, newIndex);
                            }
                            newIndex++;
                        }
                        else
                        {
                            item.PaintTo(e.Graphics, rectangle, this, current, item == this.itemAtMousePosition);
                        }
                        num4 += current.Width + 1;
                    }
                }
                y += row.Height + this.lineMarginY;
            }
            num4 = 0;
            Form form = base.FindForm();
            if (form is IActivatable)
            {
                isActivated = (form as IActivatable).IsActivated;
            }
            else
            {
                isActivated = this.Focused;
            }
            for (num = 0; num < (this.columns.Count - 1); num++)
            {
                Color columnSeperatorColor;
                current = this.columns[num];
                num4 += current.Width + 1;
                if (isActivated)
                {
                    columnSeperatorColor = current.ColumnSeperatorColor;
                    if (columnSeperatorColor.IsEmpty)
                    {
                        columnSeperatorColor = current.ColumnSeperatorColorInactive;
                    }
                }
                else
                {
                    columnSeperatorColor = current.ColumnSeperatorColorInactive;
                    if (columnSeperatorColor.IsEmpty)
                    {
                        columnSeperatorColor = current.ColumnSeperatorColor;
                    }
                }
                if (columnSeperatorColor.IsEmpty)
                {
                    columnSeperatorColor = this.BackColor;
                }
                using (Pen pen = new Pen(columnSeperatorColor))
                {
                    e.Graphics.DrawLine(pen, num4 - 1, 1, num4 - 1, Math.Min(clientSize.Height, y) - 2);
                }
            }
            this.removedControls.Clear();
            foreach (Control control in base.Controls)
            {
                if (!this.allowedControls.Contains(control))
                {
                    this.removedControls.Add(control);
                }
            }
            foreach (Control control in this.removedControls)
            {
                base.Controls.Remove(control);
            }
            this.allowedControls.Clear();
            this.removedControls.Clear();
            base.OnPaint(e);
        }

        private void OnRowAdded(object sender, CollectionItemEventArgs<DynamicListRow> e)
        {
            e.Item.HeightChanged += new EventHandler(this.RowHeightChanged);
            e.Item.ItemChanged += new EventHandler(this.RowItemChanged);
            if (base.Visible && (base.Parent != null))
            {
                e.Item.NotifyListVisibilityChange(this, true);
            }
        }

        private void OnRowRemoved(object sender, CollectionItemEventArgs<DynamicListRow> e)
        {
            e.Item.HeightChanged -= new EventHandler(this.RowHeightChanged);
            e.Item.ItemChanged -= new EventHandler(this.RowItemChanged);
            if (base.Visible)
            {
                e.Item.NotifyListVisibilityChange(this, false);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.RecalculateColumnWidths();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            bool visible = base.Visible && (base.Parent != null);
            if (visible != this.oldVisible)
            {
                this.oldVisible = visible;
                foreach (DynamicListRow row in this.Rows)
                {
                    row.NotifyListVisibilityChange(this, visible);
                }
            }
        }

        private void RecalculateColumnWidths()
        {
            if (!this.inUpdate && !this.inRecalculateColumnWidths)
            {
                this.inRecalculateColumnWidths = true;
                this.inRecalculateNeedsRedraw = false;
                try
                {
                    int width = base.ClientSize.Width;
                    int num2 = 0;
                    foreach (DynamicListColumn column in this.columns)
                    {
                        if (column.AllowGrow)
                        {
                            num2 += column.MinimumWidth;
                        }
                        else
                        {
                            width -= column.Width;
                        }
                        width--;
                    }
                    foreach (DynamicListColumn column in this.columns)
                    {
                        if (column.AllowGrow)
                        {
                            column.Width = Math.Max(2, (column.MinimumWidth * width) / num2);
                        }
                    }
                }
                finally
                {
                    this.inRecalculateColumnWidths = false;
                }
                if (this.inRecalculateNeedsRedraw)
                {
                    this.Redraw();
                }
            }
        }

        private void Redraw()
        {
            if (!this.inUpdate)
            {
                base.Invalidate();
            }
        }

        private void RowHeightChanged(object sender, EventArgs e)
        {
            this.Redraw();
        }

        private void RowItemChanged(object sender, EventArgs e)
        {
            this.Redraw();
        }

        event MouseEventHandler handler1;

        event MouseEventHandler VerticalScrollContainer.IScrollable.MouseWheel
        {
            add
            {
                handler1 += value;
            }
            remove
            {
                handler1 -= value;
            }
        }


        int VerticalScrollContainer.IScrollable.Height
        {
            get
            {
                return base.Height;
            }
        }



        public CollectionWithEvents<DynamicListColumn> Columns
        {
            [DebuggerStepThrough]
            get
            {
                return this.columns;
            }
        }

        public bool IsActivated
        {
            get
            {
                Form form = base.FindForm();
                if (form is IActivatable)
                {
                    return (form as IActivatable).IsActivated;
                }
                return this.Focused;
            }
        }

        public DynamicListItem ItemAtMousePosition
        {
            get
            {
                return this.itemAtMousePosition;
            }
        }

        public int LineMarginY
        {
            get
            {
                return this.lineMarginY;
            }
            set
            {
                if (this.lineMarginY != value)
                {
                    this.lineMarginY = value;
                    this.Redraw();
                }
            }
        }

        public DynamicListRow RowAtMousePosition
        {
            get
            {
                return this.rowAtMousePosition;
            }
        }

        [Browsable(false)]
        public CollectionWithEvents<DynamicListRow> Rows
        {
            [DebuggerStepThrough]
            get
            {
                return this.rows;
            }
        }

        public int ScrollOffset
        {
            get
            {
                return this.scrollOffset;
            }
            set
            {
                if (this.scrollOffset != value)
                {
                    this.scrollOffset = value;
                    this.Redraw();
                }
            }
        }

        int VerticalScrollContainer.IScrollable.ScrollHeightY
        {
            get
            {
                return this.TotalRowHeight;
            }
        }

        int VerticalScrollContainer.IScrollable.ScrollOffsetY
        {
            get
            {
                return this.ScrollOffset;
            }
            set
            {
                this.ScrollOffset = value;
            }
        }

        public int TotalRowHeight
        {
            get
            {
                int num = 0;
                foreach (DynamicListRow row in this.Rows)
                {
                    num += row.Height + this.lineMarginY;
                }
                return num;
            }
        }
    }
}

