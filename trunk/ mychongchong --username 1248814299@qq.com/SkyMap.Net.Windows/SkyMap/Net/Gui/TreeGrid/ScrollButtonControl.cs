namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ScrollButtonControl : Control
    {
        private ScrollButton arrow = ScrollButton.Down;
        private bool cursorEntered = false;
        private bool drawSeparatorLine = true;
        private Color highlightColor = SystemColors.Highlight;

        public ScrollButtonControl()
        {
            this.BackColor = DynamicListColumn.DefaultBackColor;
            base.TabStop = false;
            base.SetStyle(ControlStyles.Selectable, false);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.cursorEntered = true;
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.cursorEntered = false;
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Point[] pointArray;
            Color grayText;
            Point[] pointArray2;
            base.OnPaint(e);
            int height = base.ClientSize.Height;
            int num2 = height - 4;
            int width = base.ClientSize.Width;
            int x = (width - num2) / 2;
            int num5 = (width + num2) / 2;
            switch (this.arrow)
            {
                case ScrollButton.Up:
                    pointArray2 = new Point[] { new Point(x, 2 + num2), new Point(num5, 2 + num2), new Point(width / 2, 2) };
                    pointArray = pointArray2;
                    if (this.drawSeparatorLine)
                    {
                        e.Graphics.DrawLine(SystemPens.GrayText, 0, height - 1, width, height - 1);
                    }
                    break;

                case ScrollButton.Down:
                    pointArray2 = new Point[] { new Point(x, 2), new Point(num5, 2), new Point(width / 2, 2 + num2) };
                    pointArray = pointArray2;
                    if (this.drawSeparatorLine)
                    {
                        e.Graphics.DrawLine(SystemPens.GrayText, 0, 0, width, 0);
                    }
                    break;

                default:
                    return;
            }
            if (base.Enabled)
            {
                grayText = this.cursorEntered ? this.HighlightColor : this.ForeColor;
            }
            else
            {
                grayText = SystemColors.GrayText;
            }
            using (Brush brush = new SolidBrush(grayText))
            {
                e.Graphics.FillPolygon(brush, pointArray);
            }
        }

        public ScrollButton Arrow
        {
            get
            {
                return this.arrow;
            }
            set
            {
                this.arrow = value;
                base.Invalidate();
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(14, 14);
            }
        }

        public bool DrawSeparatorLine
        {
            get
            {
                return this.drawSeparatorLine;
            }
            set
            {
                this.drawSeparatorLine = value;
                base.Invalidate();
            }
        }

        public Color HighlightColor
        {
            get
            {
                return this.highlightColor;
            }
            set
            {
                this.highlightColor = value;
                base.Invalidate();
            }
        }
    }
}

