namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;

    public class TipOfTheDayView : UserControl
    {
        private int curtip = 0;
        private string didyouknowtext = ResourceService.GetString("Dialog.TipOfTheDay.DidYouKnowText");
        private Rectangle drawrect;
        private Bitmap icon = null;
        private readonly int ICON_DISTANCE = 0x10;
        private Font textfont = ResourceService.LoadFont("Times new Roman", 12);
        private string[] tips;
        private Font titlefont = ResourceService.LoadFont("Times new Roman", 15, FontStyle.Bold);

        public TipOfTheDayView(XmlElement el)
        {
            this.icon = ResourceService.GetBitmap("Icons.TipOfTheDayIcon");
            XmlNodeList childNodes = el.ChildNodes;
            this.tips = new string[childNodes.Count];
            for (int i = 0; i < childNodes.Count; i++)
            {
                this.tips[i] = SkyMap.Net.Core.StringParser.Parse(childNodes[i].InnerText);
            }
            this.curtip = new Random().Next() % childNodes.Count;
        }

        public void NextTip()
        {
            this.curtip = (this.curtip + 1) % this.tips.Length;
            base.Invalidate(this.drawrect);
            base.Update();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graphics = pe.Graphics;
            graphics.FillRectangle(Brushes.Gray, 0, 0, this.icon.Width + this.ICON_DISTANCE, base.Height);
            graphics.FillRectangle(Brushes.White, this.icon.Width + this.ICON_DISTANCE, 0, (base.Width - this.icon.Width) - this.ICON_DISTANCE, base.Height);
            graphics.DrawImage(this.icon, 2 + (this.ICON_DISTANCE / 2), 4);
            graphics.DrawString(this.didyouknowtext, this.titlefont, Brushes.Black, (float) ((this.icon.Width + this.ICON_DISTANCE) + 4), 8f);
            graphics.DrawLine(Pens.Black, new Point(this.icon.Width + this.ICON_DISTANCE, (8 + this.titlefont.Height) + 2), new Point(base.Width, (8 + this.titlefont.Height) + 2));
            this.drawrect = new Rectangle(this.icon.Width + this.ICON_DISTANCE, (8 + this.titlefont.Height) + 6, (base.Width - this.icon.Width) - this.ICON_DISTANCE, base.Height - ((8 + this.titlefont.Height) + 6));
            graphics.DrawString(this.tips[this.curtip], this.textfont, Brushes.Black, this.drawrect);
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
        }
    }
}

