namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Workflow.FlowChartCtl;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ProinstFlowInfo : UserControl
    {
        private IContainer components = null;
        private ImageList img32;
        private GraphDoc mGraphDoc;
        private ProinstDoc mProinstDoc;
        private PictureBox pic;

        public ProinstFlowInfo()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitGraphDoc()
        {
            this.pic = new PictureBox();
            base.Controls.Add(this.pic);
            this.pic.BackColor = Color.White;
            this.pic.Location = new Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new Size(0x4b0, 0x4b0);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.Paint += new PaintEventHandler(this.pic_Paint);
            this.mGraphDoc = new GraphDoc(this.pic);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ProinstFlowInfo));
            this.img32 = new ImageList(this.components);
            base.SuspendLayout();
            this.img32.ImageStream = (ImageListStreamer) manager.GetObject("img32.ImageStream");
            this.img32.TransparentColor = Color.White;
            this.img32.Images.SetKeyName(0, "");
            this.img32.Images.SetKeyName(1, "");
            this.img32.Images.SetKeyName(2, "");
            this.img32.Images.SetKeyName(3, "");
            this.img32.Images.SetKeyName(4, "");
            this.img32.Images.SetKeyName(5, "");
            this.img32.Images.SetKeyName(6, "");
            this.img32.Images.SetKeyName(7, "");
            this.img32.Images.SetKeyName(8, "");
            this.img32.Images.SetKeyName(9, "");
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "ProinstFlowInfo";
            base.Padding = new Padding(1);
            base.Size = new Size(0x17a, 0x121);
            base.ResumeLayout(false);
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            this.mGraphDoc.Draw(e.Graphics);
        }

        public void ShowMe(Prodef prodef, Proinst proinst)
        {
            if (prodef != null)
            {
                if (this.mGraphDoc != null)
                {
                    this.mGraphDoc.Reset();
                }
                else
                {
                    this.InitGraphDoc();
                }
                if (this.mProinstDoc == null)
                {
                    this.mProinstDoc = new ProinstDoc();
                }
                this.mProinstDoc.Load(prodef, proinst, this.mGraphDoc, this.img32);
            }
        }
    }
}

