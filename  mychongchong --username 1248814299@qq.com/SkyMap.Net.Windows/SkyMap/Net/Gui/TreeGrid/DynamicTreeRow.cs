namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DynamicTreeRow : DynamicListRow
    {
        private bool blockClickEvent;
        private Color childBorderColor = DefaultBorderColor;
        private CollectionWithEvents<DynamicListColumn> childColumns = new CollectionWithEvents<DynamicListColumn>();
        private CollectionWithEvents<DynamicListRow> childRows = new CollectionWithEvents<DynamicListRow>();
        public static readonly Color DefaultBorderColor = Color.FromArgb(0xc3, 0xc0, 0xaf);
        public static readonly Color DefaultExpandedRowColor = Color.FromArgb(0xeb, 0xe5, 0xd1);
        private List<DynamicList> expandedIn;
        private Color expandedRowColor = DefaultExpandedRowColor;
        private static bool isOpeningChild;
        private static readonly Color LightPlusBorder = Color.FromArgb(0xb0, 0xc2, 0xdd);
        private DynamicListItem plus;
        private static readonly Color PlusBorder = Color.FromArgb(120, 0x98, 0xb5);
        private bool showPlus;

        public event EventHandler<DynamicListEventArgs> Collapsed;

        public event EventHandler<DynamicListEventArgs> Expanded;

        public event EventHandler<DynamicListEventArgs> Expanding;

        public event EventHandler ShowPlusChanged;

        public DynamicTreeRow()
        {
            this.plus = base[0];
            this.ShowPlus = true;
        }

        protected override DynamicListItem CreateItem()
        {
            DynamicListItem item = base.CreateItem();
            item.Paint += delegate (object sender, ItemPaintEventArgs e) {
                if ((((e.Item != this.plus) && (this.expandedIn != null)) && !this.expandedRowColor.IsEmpty) && this.expandedIn.Contains(e.List))
                {
                    using (Brush brush = new SolidBrush(this.expandedRowColor))
                    {
                        e.Graphics.FillRectangle(brush, e.FillRectangle);
                    }
                }
            };
            return item;
        }

        public static void DrawPlusSign(Graphics graphics, Rectangle r, bool drawMinus)
        {
            Brush brush;
            using (brush = new LinearGradientBrush(r, Color.White, DynamicListColumn.DefaultRowHighlightBackColor, 66f))
            {
                graphics.FillRectangle(brush, r);
            }
            using (Pen pen = new Pen(PlusBorder))
            {
                graphics.DrawRectangle(pen, r);
            }
            using (brush = new SolidBrush(LightPlusBorder))
            {
                graphics.FillRectangle(brush, new Rectangle(r.X, r.Y, 1, 1));
                graphics.FillRectangle(brush, new Rectangle(r.Right, r.Y, 1, 1));
                graphics.FillRectangle(brush, new Rectangle(r.X, r.Bottom, 1, 1));
                graphics.FillRectangle(brush, new Rectangle(r.Right, r.Bottom, 1, 1));
            }
            graphics.DrawLine(Pens.Black, (int) (r.Left + 2), (int) (r.Top + (r.Height / 2)), (int) (r.Right - 2), (int) (r.Top + (r.Height / 2)));
            if (!drawMinus)
            {
                graphics.DrawLine(Pens.Black, (int) (r.Left + (r.Width / 2)), (int) (r.Top + 2), (int) (r.Left + (r.Width / 2)), (int) (r.Bottom - 2));
            }
        }

        protected virtual void OnCollapsed(DynamicListEventArgs e)
        {
            if (this.Collapsed != null)
            {
                this.Collapsed(this, e);
            }
        }

        protected virtual void OnExpanded(DynamicListEventArgs e)
        {
            if (this.Expanded != null)
            {
                this.Expanded(this, e);
            }
        }

        protected virtual void OnExpanding(DynamicListEventArgs e)
        {
            if (this.Expanding != null)
            {
                this.Expanding(this, e);
            }
        }

        protected virtual void OnPlusClick(object sender, DynamicListEventArgs e)
        {
            if (this.blockClickEvent)
            {
                this.blockClickEvent = false;
            }
            else
            {
                int num5;
                this.OnExpanding(e);
                ChildForm frm = new ChildForm();
                frm.Closed += delegate {
                    this.blockClickEvent = true;
                    if (this.expandedIn != null)
                    {
                        this.expandedIn.Remove(e.List);
                    }
                    this.OnCollapsed(e);
                    this.plus.RaiseItemChanged();
                    Timer timer = new Timer {
                        Interval = 0x55
                    };
                    timer.Tick += delegate (object sender2, EventArgs e2) {
                        ((Timer) sender2).Stop();
                        ((Timer) sender2).Dispose();
                        this.blockClickEvent = false;
                    };
                    timer.Start();
                };
                Point point = e.List.PointToScreen(e.List.GetPositionFromRow(this));
                point.Offset(e.List.Columns[0].Width, base.Height);
                frm.StartPosition = FormStartPosition.Manual;
                frm.BackColor = this.childBorderColor;
                frm.Location = point;
                frm.ShowInTaskbar = false;
                frm.Owner = e.List.FindForm();
                VerticalScrollContainer container = new VerticalScrollContainer();
                container.Dock = DockStyle.Fill;
                DynamicList list = new DynamicList(this.childColumns, this.childRows);
                list.Dock = DockStyle.Fill;
                list.KeyDown += delegate (object sender2, KeyEventArgs e2) {
                    if (e2.KeyData == Keys.Escape)
                    {
                        frm.Close();
                        e.List.FindForm().Focus();
                    }
                };
                container.Controls.Add(list);
                frm.Controls.Add(container);
                int num = Screen.FromPoint(point).WorkingArea.Bottom - point.Y;
                num -= frm.Size.Height - frm.ClientSize.Height;
                int num2 = list.TotalRowHeight + 4;
                int height = Math.Min(num2, num);
                if (height < num2)
                {
                    int num4 = Math.Min(100, num2 - height);
                    height += num4;
                    frm.Top -= num4;
                }
                using (Graphics graphics = list.CreateGraphics())
                {
                    num5 = 8 + list.GetRequiredWidth(graphics);
                }
                int num6 = Screen.FromPoint(point).WorkingArea.Right - point.X;
                if (num5 > num6)
                {
                    int num7 = Math.Min(100, num5 - num6);
                    num5 = num6 + num7;
                    frm.Left -= num7;
                }
                frm.ClientSize = new Size(num5, height);
                frm.MinimumSize = new Size(100, Math.Min(50, height));
                isOpeningChild = true;
                frm.Show();
                isOpeningChild = false;
                list.Focus();
                if (this.expandedIn != null)
                {
                    this.expandedIn.Add(e.List);
                }
                this.OnExpanded(e);
                this.plus.RaiseItemChanged();
            }
        }

        protected virtual void OnPlusPaint(object sender, ItemPaintEventArgs e)
        {
            Rectangle clipRectangle = e.ClipRectangle;
            clipRectangle.Inflate(-4, -4);
            DrawPlusSign(e.Graphics, clipRectangle, (this.expandedIn != null) && this.expandedIn.Contains(e.List));
        }

        public virtual void OnShowPlusChanged(EventArgs e)
        {
            if (this.ShowPlusChanged != null)
            {
                this.ShowPlusChanged(this, e);
            }
        }

        public Color ChildBorderColor
        {
            get
            {
                return this.childBorderColor;
            }
            set
            {
                this.childBorderColor = value;
            }
        }

        public CollectionWithEvents<DynamicListColumn> ChildColumns
        {
            get
            {
                return this.childColumns;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.childColumns = value;
            }
        }

        public CollectionWithEvents<DynamicListRow> ChildRows
        {
            get
            {
                return this.childRows;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.childRows = value;
            }
        }

        public Color ExpandedRowColor
        {
            get
            {
                return this.expandedRowColor;
            }
            set
            {
                this.expandedRowColor = value;
            }
        }

        public bool ShowMinusWhileExpanded
        {
            get
            {
                return (this.expandedIn != null);
            }
            set
            {
                if (this.ShowMinusWhileExpanded != value)
                {
                    this.expandedIn = value ? new List<DynamicList>() : null;
                }
            }
        }

        public bool ShowPlus
        {
            get
            {
                return this.showPlus;
            }
            set
            {
                if (this.showPlus != value)
                {
                    this.showPlus = value;
                    this.plus.HighlightBrush = this.showPlus ? Brushes.AliceBlue : null;
                    this.plus.Cursor = this.showPlus ? Cursors.Hand : null;
                    if (this.showPlus)
                    {
                        this.plus.Click += new EventHandler<DynamicListEventArgs>(this.OnPlusClick);
                        this.plus.Paint += new EventHandler<ItemPaintEventArgs>(this.OnPlusPaint);
                    }
                    else
                    {
                        this.plus.Click -= new EventHandler<DynamicListEventArgs>(this.OnPlusClick);
                        this.plus.Paint -= new EventHandler<ItemPaintEventArgs>(this.OnPlusPaint);
                    }
                    this.OnShowPlusChanged(EventArgs.Empty);
                }
            }
        }

        public class ChildForm : Form, IActivatable
        {
            private bool allowResizing = true;
            private bool isActivated = true;
            private bool showWindowWithoutActivation;

            public ChildForm()
            {
                base.FormBorderStyle = FormBorderStyle.None;
                base.DockPadding.All = 2;
                this.BackColor = DynamicTreeRow.DefaultBorderColor;
            }

            private void CloseOnDeactivate()
            {
                DynamicTreeRow.ChildForm owner = base.Owner as DynamicTreeRow.ChildForm;
                if (owner != null)
                {
                    if (owner.isActivated)
                    {
                        base.Close();
                    }
                    else
                    {
                        owner.CloseOnDeactivate();
                    }
                }
                else
                {
                    base.Close();
                }
            }

            private void HitTest(ref Message m)
            {
                if (this.allowResizing)
                {
                    int num = m.LParam.ToInt32();
                    int num2 = num & 0xffff;
                    int num3 = num >> 0x10;
                    Rectangle bounds = base.Bounds;
                    bool flag = (num2 == bounds.Left) || ((num2 + 1) == bounds.Left);
                    bool flag2 = (num3 == bounds.Top) || ((num3 + 1) == bounds.Top);
                    bool flag3 = (num2 == (bounds.Right - 1)) || (num2 == (bounds.Right - 2));
                    bool flag4 = (num3 == (bounds.Bottom - 1)) || (num3 == (bounds.Bottom - 2));
                    if (flag)
                    {
                        if (flag2)
                        {
                            m.Result = new IntPtr(13);
                        }
                        else if (flag4)
                        {
                            m.Result = new IntPtr(0x10);
                        }
                        else
                        {
                            m.Result = new IntPtr(10);
                        }
                    }
                    else if (flag3)
                    {
                        if (flag2)
                        {
                            m.Result = new IntPtr(14);
                        }
                        else if (flag4)
                        {
                            m.Result = new IntPtr(0x11);
                        }
                        else
                        {
                            m.Result = new IntPtr(11);
                        }
                    }
                    else if (flag2)
                    {
                        m.Result = new IntPtr(12);
                    }
                    else if (flag4)
                    {
                        m.Result = new IntPtr(15);
                    }
                }
            }

            protected override void OnActivated(EventArgs e)
            {
                this.isActivated = true;
                base.OnActivated(e);
                this.Refresh();
            }

            protected override void OnDeactivate(EventArgs e)
            {
                this.isActivated = false;
                base.OnDeactivate(e);
                if (DynamicTreeRow.isOpeningChild)
                {
                    this.Refresh();
                }
                else
                {
                    base.BeginInvoke(new MethodInvoker(this.CloseOnDeactivate));
                }
            }

            event EventHandler hander1;
            event EventHandler hander2;

             event EventHandler IActivatable.Activated
            {
                add
                {
                    this.hander1 += value;
                }
                remove
                {
                    this.hander1 -= value;
                }
            }


            event EventHandler IActivatable.Deactivate
            {
                add
                {
                    this.hander2 += value;
                }
                remove
                {
                    this.hander2 -= value;
                }
             
            }



            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == 0x84)
                {
                    this.HitTest(ref m);
                }
            }

            public bool AllowResizing
            {
                get
                {
                    return this.allowResizing;
                }
                set
                {
                    if (this.allowResizing != value)
                    {
                        this.allowResizing = value;
                        base.DockPadding.All = value ? 2 : 1;
                    }
                }
            }

            protected override System.Windows.Forms.CreateParams CreateParams
            {
                get
                {
                    return base.CreateParams;
                }
            }

            public bool IsActivated
            {
                get
                {
                    return this.isActivated;
                }
            }

            public bool ShowWindowWithoutActivation
            {
                get
                {
                    return this.showWindowWithoutActivation;
                }
                set
                {
                    this.showWindowWithoutActivation = value;
                }
            }

            protected override bool ShowWithoutActivation
            {
                get
                {
                    return this.showWindowWithoutActivation;
                }
            }

            private enum MousePositionCodes
            {
                HTBORDER = 0x12,
                HTBOTTOM = 15,
                HTBOTTOMLEFT = 0x10,
                HTBOTTOMRIGHT = 0x11,
                HTCAPTION = 2,
                HTCLIENT = 1,
                HTCLOSE = 20,
                HTERROR = -2,
                HTGROWBOX = 4,
                HTHELP = 0x15,
                HTHSCROLL = 6,
                HTLEFT = 10,
                HTMAXBUTTON = 9,
                HTMENU = 5,
                HTMINBUTTON = 8,
                HTNOWHERE = 0,
                HTOBJECT = 0x13,
                HTREDUCE = 8,
                HTRIGHT = 11,
                HTSIZE = 4,
                HTSIZEFIRST = 10,
                HTSIZELAST = 0x11,
                HTSYSMENU = 3,
                HTTOP = 12,
                HTTOPLEFT = 13,
                HTTOPRIGHT = 14,
                HTTRANSPARENT = -1,
                HTVSCROLL = 7,
                HTZOOM = 9
            }
        }
    }
}

