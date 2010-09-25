namespace SkyMap.Net.Gui.Dialogs
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class CommonAboutDialog : SmForm
    {
        private Label label1;
        private PictureBox pictureBox1;
        private SimpleButton simpleButton1;

        public CommonAboutDialog()
        {
            Stream stream;
            this.InitializeComponent();
            base.Icon = ResourceService.GetIcon("Dialog.About.Icon");
            this.Text = ResourceService.GetString("Dialog.About.Text");
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "SplashScreen.png");
            Bitmap bitmap = null;
            if (File.Exists(path))
            {
                using (stream = File.OpenRead(path))
                {
                    bitmap = new Bitmap(stream);
                }
            }
            else
            {
                using (stream = Assembly.GetEntryAssembly().GetManifestResourceStream("Resources.SplashScreen.png"))
                {
                    bitmap = new Bitmap(stream);
                }
            }
            base.Width = bitmap.Width + 1;
            base.Height = (bitmap.Height + this.label1.Height) + 50;
            this.pictureBox1.Image = bitmap;
            base.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new PictureBox();
            this.label1 = new Label();
            this.simpleButton1 = new SimpleButton();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox1.Dock = DockStyle.Top;
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x1e2, 0x108);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.Location = new Point(8, 280);
            this.label1.Name = "label1";
            this.label1.Size = new Size(400, 0x38);
            this.label1.TabIndex = 1;
            this.label1.Text = "警告：本计算机程序受版权法与国际条约保护。如未经授权复制或传播本软件（或其中任何部分），都将受到严厉的刑事与民事制裁，并将在法律许可的最大限度内受到起诉。";
            this.simpleButton1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.simpleButton1.Location = new Point(0x1a8, 0x138);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new Size(0x38, 0x17);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new EventHandler(this.simpleButton1_Click);
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x1e2, 0x15f);
            base.Controls.Add(this.simpleButton1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.pictureBox1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CommonAboutDialog";
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

