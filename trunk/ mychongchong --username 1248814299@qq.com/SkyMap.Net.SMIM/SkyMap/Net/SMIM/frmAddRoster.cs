namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmAddRoster : Form
    {
        private XmppClientConnection _connection;
        private Button cmdAdd;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtJid;
        private TextBox txtNickname;

        public frmAddRoster(XmppClientConnection con)
        {
            this.InitializeComponent();
            this._connection = con;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            Jid jid = new Jid(this.txtJid.Text);
            if (this.txtNickname.Text.Length > 0)
            {
                this._connection.get_RosterManager().AddRosterItem(jid, this.txtNickname.Text);
            }
            else
            {
                this._connection.get_RosterManager().AddRosterItem(jid);
            }
            this._connection.get_PresenceManager().Subcribe(jid);
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
            this.txtNickname = new TextBox();
            this.txtJid = new TextBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.cmdAdd = new Button();
            base.SuspendLayout();
            this.txtNickname.Location = new Point(80, 0x38);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new Size(0xa8, 20);
            this.txtNickname.TabIndex = 9;
            this.txtNickname.Text = "";
            this.txtJid.Location = new Point(80, 8);
            this.txtJid.Name = "txtJid";
            this.txtJid.Size = new Size(0xa8, 20);
            this.txtJid.TabIndex = 8;
            this.txtJid.Text = "";
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(8, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x10);
            this.label3.TabIndex = 11;
            this.label3.Text = "Nickname:";
            this.label2.Location = new Point(80, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(160, 0x10);
            this.label2.TabIndex = 10;
            this.label2.Text = "user@server.org";
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 7;
            this.label1.Text = "Jabber ID:";
            this.cmdAdd.FlatStyle = FlatStyle.System;
            this.cmdAdd.Location = new Point(0x60, 0x58);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new Size(80, 0x18);
            this.cmdAdd.TabIndex = 12;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.Click += new EventHandler(this.cmdAdd_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x108, 0x76);
            base.Controls.Add(this.cmdAdd);
            base.Controls.Add(this.txtNickname);
            base.Controls.Add(this.txtJid);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmAddRoster";
            this.Text = "Add Contact";
            base.ResumeLayout(false);
        }
    }
}

