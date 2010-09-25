namespace SkyMap.Net.Gui
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using SkyMap.Net.Core;

    public class CurrentPanelPanel : UserControl
    {
        private Font normalFont = ResourceService.LoadFont("SansSerif", 0x12, GraphicsUnit.World);
        private WizardDialog wizard;

        public CurrentPanelPanel(WizardDialog wizard)
        {
            this.wizard = wizard;
            base.Size = new Size(wizard.Width - 220, 30);
            base.ResizeRedraw = false;
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graphics = pe.Graphics;
            graphics.DrawString(this.wizard.WizardPanels[this.wizard.ActivePanelNumber].Label, this.normalFont, Brushes.Black, 10f, (float) (0x18 - this.normalFont.Height), StringFormat.GenericTypographic);
            graphics.DrawLine(Pens.Black, 10, 0x18, base.Width - 10, 0x18);
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            Graphics graphics = pe.Graphics;
            using (Brush brush = new LinearGradientBrush(new Point(0, 0), new Point(base.Width, base.Height), Color.White, SystemColors.Control))
            {
                graphics.FillRectangle(brush, new Rectangle(0, 0, base.Width, base.Height));
            }
        }
    }
}

