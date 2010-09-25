namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSubscribe : Form
    {
        private XmppClientConnection _connection;
        private Jid _from;
        private Button cmdApprove;
        private Button cmdRefuse;
        private Container components = null;
        private Label label1;
        private Label lblFrom;

        public frmSubscribe(XmppClientConnection con, Jid jid)
        {
            this.InitializeComponent();
            this._connection = con;
            this._from = jid;
            this.lblFrom.Text = jid.ToString();
        }

        private void cmdApprove_Click(object sender, EventArgs e)
        {
            new PresenceManager(this._connection).ApproveSubscriptionRequest(this._from);
            base.Close();
        }

        private void cmdRefuse_Click(object sender, EventArgs e)
        {
            new PresenceManager(this._connection).RefuseSubscriptionRequest(this._from);
            base.Close();
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
            this.cmdApprove = new Button();
            this.cmdRefuse = new Button();
            this.label1 = new Label();
            this.lblFrom = new Label();
            base.SuspendLayout();
            this.cmdApprove.FlatStyle = FlatStyle.System;
            this.cmdApprove.Location = new Point(0xa8, 0x38);
            this.cmdApprove.Name = "cmdApprove";
            this.cmdApprove.Size = new Size(0x48, 0x18);
            this.cmdApprove.TabIndex = 0;
            this.cmdApprove.Text = "Approve";
            this.cmdApprove.Click += new EventHandler(this.cmdApprove_Click);
            this.cmdRefuse.FlatStyle = FlatStyle.System;
            this.cmdRefuse.Location = new Point(0x38, 0x38);
            this.cmdRefuse.Name = "cmdRefuse";
            this.cmdRefuse.Size = new Size(0x48, 0x18);
            this.cmdRefuse.TabIndex = 1;
            this.cmdRefuse.Text = "Refuse";
            this.cmdRefuse.Click += new EventHandler(this.cmdRefuse_Click);
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            this.lblFrom.Location = new Point(0x38, 0x10);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new Size(0xe0, 0x20);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "jid";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x124, 0x58);
            base.Controls.Add(this.lblFrom);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cmdRefuse);
            base.Controls.Add(this.cmdApprove);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmSubscribe";
            this.Text = "Subscription Request";
            base.ResumeLayout(false);
        }
    }
}

