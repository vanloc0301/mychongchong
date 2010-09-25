namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP;
    using SkyMap.Net.XMPP.protocol.client;
    using SkyMap.Net.XMPP.protocol.iq.vcard;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmVcard : Form
    {
        private XmppClientConnection _connection;
        private Button cmdClose;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private string packetId;
        private PictureBox picPhoto;
        private TextBox txtBirthday;
        private TextBox txtDescription;
        private TextBox txtFullname;
        private TextBox txtNickname;

        public frmVcard(Jid jid, XmppClientConnection con)
        {
            this.InitializeComponent();
            this._connection = con;
            this.Text = "名片 : " + jid.get_Bare();
            VcardIq iq = new VcardIq(0, new Jid(jid.get_Bare()));
            this.packetId = iq.get_Id();
            con.get_IqGrabber().SendIq(iq, new IqCB(this, (IntPtr) this.VcardResult), null);
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
            if (this._connection.get_IqGrabber() != null)
            {
                this._connection.get_IqGrabber().Remove(this.packetId);
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.txtFullname = new TextBox();
            this.txtBirthday = new TextBox();
            this.txtDescription = new TextBox();
            this.txtNickname = new TextBox();
            this.label5 = new Label();
            this.picPhoto = new PictureBox();
            this.cmdClose = new Button();
            ((ISupportInitialize) this.picPhoto).BeginInit();
            base.SuspendLayout();
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x13, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x73, 0x1a);
            this.label1.TabIndex = 0;
            this.label1.Text = "全名:";
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x13, 0x5f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x7d, 0x11);
            this.label2.TabIndex = 1;
            this.label2.Text = "生日:";
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x13, 0x81);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x7d, 0x11);
            this.label3.TabIndex = 2;
            this.label3.Text = "描述:";
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x13, 0x34);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x73, 0x11);
            this.label4.TabIndex = 3;
            this.label4.Text = "昵称:";
            this.txtFullname.Location = new Point(0x9a, 0x11);
            this.txtFullname.Name = "txtFullname";
            this.txtFullname.ReadOnly = true;
            this.txtFullname.Size = new Size(0xd3, 0x15);
            this.txtFullname.TabIndex = 0;
            this.txtBirthday.Location = new Point(0x9a, 0x56);
            this.txtBirthday.Name = "txtBirthday";
            this.txtBirthday.ReadOnly = true;
            this.txtBirthday.Size = new Size(0xd3, 0x15);
            this.txtBirthday.TabIndex = 2;
            this.txtDescription.Location = new Point(0x9a, 0x79);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(0xd3, 0x45);
            this.txtDescription.TabIndex = 3;
            this.txtNickname.Location = new Point(0x9a, 0x34);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.ReadOnly = true;
            this.txtNickname.Size = new Size(0xd3, 0x15);
            this.txtNickname.TabIndex = 1;
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0x13, 0xcf);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x7d, 0x11);
            this.label5.TabIndex = 8;
            this.label5.Text = "相片:";
            this.picPhoto.Location = new Point(0x9a, 0xcf);
            this.picPhoto.Name = "picPhoto";
            this.picPhoto.Size = new Size(0xd3, 0x70);
            this.picPhoto.TabIndex = 9;
            this.picPhoto.TabStop = false;
            this.cmdClose.FlatStyle = FlatStyle.System;
            this.cmdClose.Location = new Point(0x90, 0x145);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new Size(0x73, 0x1a);
            this.cmdClose.TabIndex = 4;
            this.cmdClose.Text = "关闭";
            this.cmdClose.Click += new EventHandler(this.cmdClose_Click);
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x181, 0x160);
            base.Controls.Add(this.cmdClose);
            base.Controls.Add(this.picPhoto);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.txtNickname);
            base.Controls.Add(this.txtDescription);
            base.Controls.Add(this.txtBirthday);
            base.Controls.Add(this.txtFullname);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "frmVcard";
            this.Text = "frmVcard";
            ((ISupportInitialize) this.picPhoto).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void VcardResult(object sender, IQ iq, object data)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new IqCB(this, (IntPtr) this.VcardResult), new object[] { sender, iq, data });
            }
            else if (iq.get_Type() == 2)
            {
                Vcard vcard = iq.get_Vcard();
                if (vcard != null)
                {
                    this.txtFullname.Text = vcard.get_Fullname();
                    this.txtNickname.Text = vcard.get_Nickname();
                    this.txtBirthday.Text = vcard.get_Birthday().ToString();
                    this.txtDescription.Text = vcard.get_Description();
                    if (vcard.get_Photo() != null)
                    {
                        this.picPhoto.Image = vcard.get_Photo().get_Image();
                    }
                }
            }
        }
    }
}

