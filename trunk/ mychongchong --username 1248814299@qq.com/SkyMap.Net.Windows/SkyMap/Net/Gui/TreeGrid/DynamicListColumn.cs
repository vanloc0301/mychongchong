namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class DynamicListColumn : ICloneable
    {
        private bool allowGrow = true;
        private bool autoSize = false;
        private Brush backgroundBrush = DefaultBackBrush;
        private Brush backgroundBrushInactive = DefaultInactiveBackBrush;
        private Color columnSeperatorColor = Color.Empty;
        private Color columnSeperatorColorInactive = Color.Empty;
        public static readonly Brush DefaultBackBrush = new SolidBrush(DefaultBackColor);
        public static readonly Color DefaultBackColor = Color.FromArgb(0xf7, 0xf5, 0xe9);
        public static readonly Brush DefaultInactiveBackBrush = new SolidBrush(DefaultInactiveBackColor);
        public static readonly Color DefaultInactiveBackColor = Color.FromArgb(0xf2, 240, 0xe4);
        public static readonly Color DefaultRowHighlightBackColor = Color.FromArgb(0xdd, 0xda, 0xcb);
        public static readonly Brush DefaultRowHighlightBrush = new SolidBrush(DefaultRowHighlightBackColor);
        public const int DefaultWidth = 0x10;
        private int minimumWidth = 0x10;
        private Brush rowHighlightBrush = DefaultRowHighlightBrush;
        private int width = 0x10;

        public event EventHandler MinimumWidthChanged;

        public event EventHandler WidthChanged;

        public virtual DynamicListColumn Clone()
        {
            return (DynamicListColumn) base.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public bool AllowGrow
        {
            get
            {
                return this.allowGrow;
            }
            set
            {
                this.allowGrow = value;
            }
        }

        public bool AutoSize
        {
            get
            {
                return this.autoSize;
            }
            set
            {
                this.autoSize = value;
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
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.backgroundBrush = value;
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
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.backgroundBrushInactive = value;
            }
        }

        public Color ColumnSeperatorColor
        {
            get
            {
                return this.columnSeperatorColor;
            }
            set
            {
                this.columnSeperatorColor = value;
            }
        }

        public Color ColumnSeperatorColorInactive
        {
            get
            {
                return this.columnSeperatorColorInactive;
            }
            set
            {
                this.columnSeperatorColorInactive = value;
            }
        }

        public int MinimumWidth
        {
            get
            {
                return this.minimumWidth;
            }
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException("value", value, "MinimumWidth must be at least 2");
                }
                if (this.minimumWidth != value)
                {
                    this.minimumWidth = value;
                    if (this.MinimumWidthChanged != null)
                    {
                        this.MinimumWidthChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public Brush RowHighlightBrush
        {
            get
            {
                return this.rowHighlightBrush;
            }
            set
            {
                this.rowHighlightBrush = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Width must be at least 2");
                }
                if (this.width != value)
                {
                    this.width = value;
                    if (this.WidthChanged != null)
                    {
                        this.WidthChanged(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}

