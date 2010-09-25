namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class GradientHeaderPanel : Label
    {
        public GradientHeaderPanel()
        {
            base.ResizeRedraw = true;
            this.Text = string.Empty;
        }

        public GradientHeaderPanel(int fontSize) : this()
        {
            this.Font = ResourceService.LoadFont("Tahoma", fontSize);
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            base.OnPaintBackground(pe);
            Graphics graphics = pe.Graphics;
            using (Brush brush = new LinearGradientBrush(new Point(0, 0), new Point(base.Width, base.Height), SystemColors.Window, SystemColors.Control))
            {
                graphics.FillRectangle(brush, new Rectangle(0, 0, base.Width, base.Height));
            }
        }
    }
}

