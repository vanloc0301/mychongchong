namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP.protocol.x.data;
    using SkyMap.Net.XMPP.ui.xdata;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmXData : Form
    {
        private Container components = null;
        private XDataControl xDataControl1;

        public frmXData(Data data)
        {
            this.InitializeComponent();
            this.xDataControl1.Create(data);
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
            this.xDataControl1 = new XDataControl();
            base.SuspendLayout();
            this.xDataControl1.Dock = DockStyle.Fill;
            this.xDataControl1.Location = new Point(0, 0);
            this.xDataControl1.Name = "xDataControl1";
            this.xDataControl1.set_ShowLegend(true);
            this.xDataControl1.Size = new Size(0x120, 0xde);
            this.xDataControl1.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x120, 0xde);
            base.Controls.Add(this.xDataControl1);
            base.Name = "frmXData";
            this.Text = "frmXData";
            base.ResumeLayout(false);
        }
    }
}

