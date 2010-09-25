namespace SkyMap.Net.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    internal class SystemHintForm : AniForm
    {
        private IContainer components = null;
        private Label label1;
        private Label lblHintText;
        private PictureBox picClose;
        private PictureBox pictureBox1;

        public SystemHintForm()
        {
            this.InitializeComponent();
            base.StackMode = StackMode.Top;
            base.Placement = FormPlacement.Tray;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(SystemHintForm));
            this.picClose = new PictureBox();
            this.pictureBox1 = new PictureBox();
            this.label1 = new Label();
            this.lblHintText = new Label();
            base.SuspendLayout();
            this.picClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.picClose.Cursor = Cursors.Hand;
            this.picClose.Image = (Image) manager.GetObject("picClose.Image");
            this.picClose.Location = new Point(0xee, 3);
            this.picClose.Name = "picClose";
            this.picClose.Size = new Size(0x13, 0x11);
            this.picClose.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picClose.TabIndex = 3;
            this.picClose.TabStop = false;
            this.picClose.Click += new EventHandler(this.picClose_Click);
            this.pictureBox1.Cursor = Cursors.Hand;
            this.pictureBox1.Image = (Image) manager.GetObject("pictureBox1.Image");
            this.pictureBox1.Location = new Point(4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x13, 0x11);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label1.ForeColor = Color.Blue;
            this.label1.Location = new Point(0x18, 6);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x36, 0x11);
            this.label1.TabIndex = 5;
            this.label1.Text = "系统提示";
            this.lblHintText.BackColor = Color.Transparent;
            this.lblHintText.ForeColor = Color.Magenta;
            this.lblHintText.Location = new Point(0x18, 0x20);
            this.lblHintText.Name = "lblHintText";
            this.lblHintText.Size = new Size(0xd8, 0xb0);
            this.lblHintText.TabIndex = 6;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(260, 0xe8);
            base.Controls.Add(this.lblHintText);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.picClose);
            base.Delay = 0xbb8;
            base.Location = new Point(0, 650);
            base.Name = "SystemHintForm";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.Speed = 0x23;
            base.StartLocation = new Point(0, 650);
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "";
            base.ResumeLayout(false);
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            base.RequestClose();
        }

        public string HintText
        {
            get
            {
                return this.lblHintText.Text;
            }
            set
            {
                this.lblHintText.Text = value;
            }
        }
    }
}

