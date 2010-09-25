namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public sealed class DynamicListItem
    {
        private bool allowLabelEdit;
        private Brush backgroundBrush;
        private Brush backgroundBrushInactive;
        private System.Windows.Forms.Control control;
        private System.Windows.Forms.Cursor cursor;
        private static StringFormat defaultTextFormat;
        private System.Drawing.Font font = System.Windows.Forms.Control.DefaultFont;
        private Brush highlightBrush;
        private DynamicListRow row;
        private string text = string.Empty;
        private Brush textBrush = SystemBrushes.ControlText;
        private StringFormat textFormat = DefaultTextFormat;

        public event EventHandler<DynamicListEventArgs> BeginLabelEdit;

        public event EventHandler<DynamicListEventArgs> CanceledLabelEdit;

        public event EventHandler<DynamicListEventArgs> Click;

        public event EventHandler<DynamicListEventArgs> DoubleClick;

        public event EventHandler<DynamicListEventArgs> FinishLabelEdit;

        public event EventHandler<MeasureWidthEventArgs> MeasureWidth;

        public event EventHandler<DynamicListMouseEventArgs> MouseDown;

        public event EventHandler<DynamicListEventArgs> MouseEnter;

        public event EventHandler<DynamicListEventArgs> MouseHover;

        public event EventHandler<DynamicListEventArgs> MouseLeave;

        public event EventHandler<DynamicListMouseEventArgs> MouseMove;

        public event EventHandler<DynamicListMouseEventArgs> MouseUp;

        public event EventHandler<ItemPaintEventArgs> Paint;

        internal DynamicListItem(DynamicListRow row)
        {
            this.row = row;
        }

        public void AssignControlUntilFocusChange(System.Windows.Forms.Control control)
        {
            MethodInvoker method = delegate {
                if (!control.Focus())
                {
                    control.Focus();
                }
                control.LostFocus += delegate {
                    this.Control = null;
                    control.Dispose();
                };
            };
            control.HandleCreated += delegate {
                control.BeginInvoke(method);
            };
            this.Control = control;
        }

        private Brush GetBrush(bool isActive, Brush activeBrush, Brush inactiveBrush)
        {
            return (isActive ? (activeBrush ?? inactiveBrush) : (inactiveBrush ?? activeBrush));
        }

        private void HandleLabelEditClick(DynamicList list)
        {
            if (this.allowLabelEdit)
            {
                TextBox txt = new TextBox();
                txt.Text = this.Text;
                this.AssignControlUntilFocusChange(txt);
                if (this.BeginLabelEdit != null)
                {
                    this.BeginLabelEdit(this, new DynamicListEventArgs(list));
                }
                bool escape = false;
                txt.KeyDown += delegate (object sender2, KeyEventArgs e2) {
                    if ((e2.KeyData == Keys.Return) || (e2.KeyData == Keys.Escape))
                    {
                        e2.Handled = true;
                        if (e2.KeyData == Keys.Escape)
                        {
                            if (this.CanceledLabelEdit != null)
                            {
                                this.CanceledLabelEdit(this, new DynamicListEventArgs(list));
                            }
                            escape = true;
                        }
                        this.Control = null;
                        txt.Dispose();
                    }
                };
                txt.LostFocus += delegate {
                    if (!escape)
                    {
                        this.Text = txt.Text;
                        if (this.FinishLabelEdit != null)
                        {
                            this.FinishLabelEdit(this, new DynamicListEventArgs(list));
                        }
                    }
                };
            }
        }

        internal void MeasureMinimumWidth(Graphics graphics, ref int minimumWidth)
        {
            if (this.MeasureWidth != null)
            {
                MeasureWidthEventArgs e = new MeasureWidthEventArgs(graphics);
                this.MeasureWidth(this, e);
                minimumWidth = Math.Max(minimumWidth, e.ItemWidth);
            }
            if (this.text.Length > 0)
            {
                int num = 2 + ((int) graphics.MeasureString(this.text, this.font, new PointF(0f, 0f), this.textFormat).Width);
                minimumWidth = Math.Max(minimumWidth, num);
            }
        }

        internal void OnMouseDown(DynamicListMouseEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        internal void OnMouseEnter(DynamicList list)
        {
            if (this.MouseEnter != null)
            {
                this.MouseEnter(this, new DynamicListEventArgs(list));
            }
            if (this.highlightBrush != null)
            {
                this.RaiseItemChanged();
            }
        }

        internal void OnMouseHover(DynamicList list)
        {
            if (this.MouseHover != null)
            {
                this.MouseHover(this, new DynamicListEventArgs(list));
            }
        }

        internal void OnMouseLeave(DynamicList list)
        {
            if (this.MouseLeave != null)
            {
                this.MouseLeave(this, new DynamicListEventArgs(list));
            }
            if (this.highlightBrush != null)
            {
                this.RaiseItemChanged();
            }
        }

        internal void OnMouseMove(DynamicListMouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }
        }

        internal void OnMouseUp(DynamicListMouseEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        internal void PaintTo(Graphics g, Rectangle rectangle, DynamicList list, DynamicListColumn column, bool isMouseEntered)
        {
            Rectangle rect = rectangle;
            rect.Width++;
            if ((this.highlightBrush != null) && isMouseEntered)
            {
                g.FillRectangle(this.highlightBrush, rect);
            }
            else
            {
                bool isActivated = list.IsActivated;
                Brush rowHighlightBrush = this.GetBrush(isActivated, this.backgroundBrush, this.backgroundBrushInactive);
                if (rowHighlightBrush == null)
                {
                    rowHighlightBrush = this.GetBrush(isActivated, column.BackgroundBrush, column.BackgroundBrushInactive);
                    if ((isActivated && (list.RowAtMousePosition == this.row)) && (column.RowHighlightBrush != null))
                    {
                        rowHighlightBrush = column.RowHighlightBrush;
                    }
                }
                g.FillRectangle(rowHighlightBrush, rect);
            }
            if (this.Paint != null)
            {
                this.Paint(this, new ItemPaintEventArgs(g, rectangle, rect, list, column, this, isMouseEntered));
            }
            if (this.text.Length > 0)
            {
                g.DrawString(this.text, this.font, this.textBrush, rectangle, this.textFormat);
            }
        }

        public void PerformClick(DynamicList list)
        {
            if (this.Click != null)
            {
                this.Click(this, new DynamicListEventArgs(list));
            }
            this.HandleLabelEditClick(list);
        }

        public void PerformDoubleClick(DynamicList list)
        {
            if (this.DoubleClick != null)
            {
                this.DoubleClick(this, new DynamicListEventArgs(list));
            }
        }

        public void RaiseItemChanged()
        {
            this.row.RaiseItemChanged(this);
        }

        public bool AllowLabelEdit
        {
            get
            {
                return this.allowLabelEdit;
            }
            set
            {
                this.allowLabelEdit = value;
            }
        }

        public Brush BackgroundBrush
        {
            get
            {
                return this.backgroundBrush;
            }
            set
            {
                if (this.backgroundBrush != value)
                {
                    this.backgroundBrush = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public Brush BackgroundBrushInactive
        {
            get
            {
                return this.backgroundBrushInactive;
            }
            set
            {
                if (this.backgroundBrushInactive != value)
                {
                    this.backgroundBrushInactive = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public System.Windows.Forms.Control Control
        {
            get
            {
                return this.control;
            }
            set
            {
                if (this.control != value)
                {
                    this.control = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return this.cursor;
            }
            set
            {
                this.cursor = value;
            }
        }

        public static StringFormat DefaultTextFormat
        {
            get
            {
                if (defaultTextFormat == null)
                {
                    defaultTextFormat = (StringFormat) StringFormat.GenericDefault.Clone();
                    defaultTextFormat.FormatFlags |= StringFormatFlags.NoWrap;
                }
                return defaultTextFormat;
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this.font != value)
                {
                    this.font = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public Brush HighlightBrush
        {
            get
            {
                return this.highlightBrush;
            }
            set
            {
                if (this.highlightBrush != value)
                {
                    this.highlightBrush = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public DynamicListRow Row
        {
            get
            {
                return this.row;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Use string.Empty instead of null!");
                }
                if (this.text != value)
                {
                    this.text = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public Brush TextBrush
        {
            get
            {
                return this.textBrush;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this.textBrush != value)
                {
                    this.textBrush = value;
                    this.RaiseItemChanged();
                }
            }
        }

        public StringFormat TextFormat
        {
            get
            {
                return this.textFormat;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this.textFormat != value)
                {
                    this.textFormat = value;
                    this.RaiseItemChanged();
                }
            }
        }
    }
}

