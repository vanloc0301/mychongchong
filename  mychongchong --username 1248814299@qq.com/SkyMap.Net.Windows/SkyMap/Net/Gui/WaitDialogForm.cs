namespace SkyMap.Net.Gui
{
    using DevExpress.LookAndFeel;
    using DevExpress.Skins;
    using DevExpress.Utils.Drawing;
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WaitDialogForm : XtraForm
    {
        private Font boldFont;
        private string caption;
        private Font font;
        private PictureBox pic;
        private string title;

        public WaitDialogForm() : this("")
        {
        }

        public WaitDialogForm(string caption) : this(caption, "")
        {
        }

        public WaitDialogForm(string caption, Size size) : this(caption, "", size, null)
        {
        }

        public WaitDialogForm(string caption, string title) : this(caption, title, new Size(260, 50), null)
        {
        }

        public WaitDialogForm(string caption, string title, Size size) : this(caption, title, size, null)
        {
        }

        public WaitDialogForm(string caption, string title, Size size, Form parent)
        {
            this.caption = "";
            this.title = "";
            this.boldFont = new Font("Arial", 9f, FontStyle.Bold);
            this.font = new Font("Arial", 9f, FontStyle.Italic);
            this.caption = caption;
            this.title = (title == "") ? "运行中,请稍等..." : title;
            this.pic = new PictureBox();
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.ControlBox = false;
            base.ClientSize = size;
            if (parent == null)
            {
                base.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                base.StartPosition = FormStartPosition.Manual;
                base.Left = parent.Left + ((parent.Width - base.Width) / 2);
                base.Top = parent.Top + ((parent.Height - base.Height) / 2);
            }
            base.ShowInTaskbar = false;
            base.Paint += new PaintEventHandler(this.WaitDialogPaint);
            this.pic.Size = new Size(0x10, 0x10);
            this.pic.Location = new Point(8, (base.ClientSize.Height / 2) - 0x10);
            this.pic.Image = ResourceService.GetBitmap("wait.gif");
            base.Controls.Add(this.pic);
            base.Show();
            this.Refresh();
        }

        public string GetCaption()
        {
            return this.Caption;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.pic.Image = null;
            this.boldFont = null;
            this.font = null;
            base.OnClosing(e);
        }

        public void SetCaption(string newCaption)
        {
            this.Caption = newCaption;
        }

        private void WaitDialogPaint(object sender, PaintEventArgs e)
        {
            Rectangle clipRectangle = e.ClipRectangle;
            clipRectangle.Inflate(-1, -1);
            GraphicsCache cache = new GraphicsCache(e);
            using (StringFormat format = new StringFormat())
            {
                Brush solidBrush = cache.GetSolidBrush(DevExpress.LookAndFeel.LookAndFeelHelper.GetSystemColor(base.LookAndFeel, SystemColors.WindowText));
                format.Alignment = format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;
                if (base.LookAndFeel.ActiveLookAndFeel.ActiveStyle == ActiveLookAndFeelStyle.Skin)
                {
                    ObjectPainter.DrawObject(cache, new SkinTextBorderPainter(base.LookAndFeel), new BorderObjectInfoArgs(null, clipRectangle, null));
                }
                else
                {
                    ControlPaint.DrawBorder3D(e.Graphics, clipRectangle, Border3DStyle.RaisedInner);
                }
                clipRectangle.X += 30;
                clipRectangle.Width -= 30;
                clipRectangle.Height /= 3;
                clipRectangle.Y += clipRectangle.Height / 2;
                e.Graphics.DrawString(this.title, this.boldFont, solidBrush, clipRectangle, format);
                clipRectangle.Y += clipRectangle.Height;
                e.Graphics.DrawString(this.caption, this.font, solidBrush, clipRectangle, format);
                cache.Dispose();
            }
        }

        public override bool AllowFormSkin
        {
            get
            {
                return false;
            }
        }

        public string Caption
        {
            get
            {
                return this.caption;
            }
            set
            {
                this.caption = value;
                this.Refresh();
            }
        }
    }
}

