namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP;
    using SkyMap.Net.XMPP.Collections;
    using SkyMap.Net.XMPP.protocol.client;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmChat : Form
    {
        private XmppClientConnection _connection;
        private string _nickname;
        private Button cmdSend;
        private Container components;
        private SkyMap.Net.XMPP.Jid m_Jid;
        private PictureBox pictureBox1;
        private RichTextBox rtfChat;
        private RichTextBox rtfSend;
        private Splitter splitter1;
        private StatusBar statusBar1;

        public frmChat(SkyMap.Net.XMPP.Jid jid, XmppClientConnection con, string nickname)
        {
            this.components = null;
            this.m_Jid = jid;
            this._connection = con;
            this._nickname = nickname;
            this.InitializeComponent();
            this.Text = "与：" + nickname + "聊天中...";
            Util.ChatForms.Add(this.m_Jid.get_Bare().ToLower(), this);
            con.get_MesagageGrabber().Add(jid, new BareJidComparer(), new MessageCB(this, (IntPtr) this.MessageCallback), null);
        }

        public frmChat(SkyMap.Net.XMPP.Jid jid, XmppClientConnection con, string nickname, bool privateChat)
        {
            this.components = null;
            this.m_Jid = jid;
            this._connection = con;
            this._nickname = nickname;
            this.InitializeComponent();
            this.Text = "与：" + nickname + "聊天中...";
            Util.ChatForms.Add(this.m_Jid.get_Bare().ToLower(), this);
            if (privateChat)
            {
                con.get_MesagageGrabber().Add(jid, new BareJidComparer(), new MessageCB(this, (IntPtr) this.MessageCallback), null);
            }
            else
            {
                con.get_MesagageGrabber().Add(jid, new FullJidComparer(), new MessageCB(this, (IntPtr) this.MessageCallback), null);
            }
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            Message msg = new Message();
            msg.set_Type(1);
            msg.set_To(this.m_Jid);
            msg.set_Body(this.rtfSend.Text);
            this._connection.Send(msg);
            this.OutgoingMessage(msg);
            this.rtfSend.Text = "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
            Util.ChatForms.Remove(this.m_Jid.get_Bare().ToLower());
            this._connection.get_MesagageGrabber().Remove(this.m_Jid);
            this._connection = null;
        }

        public void IncomingMessage(Message msg)
        {
            this.rtfChat.SelectionColor = Color.Red;
            this.rtfChat.AppendText("<" + this._nickname + "> : ");
            this.rtfChat.SelectionColor = Color.Black;
            this.rtfChat.AppendText(msg.get_Body());
            this.rtfChat.AppendText("\r\n");
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmChat));
            this.statusBar1 = new StatusBar();
            this.pictureBox1 = new PictureBox();
            this.cmdSend = new Button();
            this.rtfSend = new RichTextBox();
            this.splitter1 = new Splitter();
            this.rtfChat = new RichTextBox();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.statusBar1.Location = new Point(0, 0xd4);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new Size(0x1a8, 0x1a);
            this.statusBar1.TabIndex = 5;
            this.pictureBox1.Dock = DockStyle.Bottom;
            this.pictureBox1.Location = new Point(0, 0xae);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x1a8, 0x26);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.cmdSend.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cmdSend.FlatStyle = FlatStyle.System;
            this.cmdSend.Location = new Point(0x148, 180);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new Size(0x56, 0x1a);
            this.cmdSend.TabIndex = 7;
            this.cmdSend.Text = "发送(&S)";
            this.cmdSend.Click += new EventHandler(this.cmdSend_Click);
            this.rtfSend.Dock = DockStyle.Bottom;
            this.rtfSend.Location = new Point(0, 0x7a);
            this.rtfSend.Name = "rtfSend";
            this.rtfSend.Size = new Size(0x1a8, 0x34);
            this.rtfSend.TabIndex = 8;
            this.rtfSend.Text = "";
            this.splitter1.Dock = DockStyle.Bottom;
            this.splitter1.Location = new Point(0, 0x71);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(0x1a8, 9);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            this.rtfChat.Dock = DockStyle.Fill;
            this.rtfChat.Location = new Point(0, 0);
            this.rtfChat.Name = "rtfChat";
            this.rtfChat.Size = new Size(0x1a8, 0x71);
            this.rtfChat.TabIndex = 10;
            this.rtfChat.Text = "";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x1a8, 0xee);
            base.Controls.Add(this.rtfChat);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.rtfSend);
            base.Controls.Add(this.cmdSend);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.statusBar1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmChat";
            this.Text = "frmChat";
            base.TopMost = true;
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void MessageCallback(object sender, Message msg, object data)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new MessageCB(this, (IntPtr) this.MessageCallback), new object[] { sender, msg, data });
            }
            else if (msg.get_Body() != null)
            {
                this.IncomingMessage(msg);
            }
        }

        private void OutgoingMessage(Message msg)
        {
            this.rtfChat.SelectionColor = Color.Blue;
            this.rtfChat.AppendText("<我> : ");
            this.rtfChat.SelectionColor = Color.Black;
            this.rtfChat.AppendText(msg.get_Body());
            this.rtfChat.AppendText("\r\n");
        }

        public SkyMap.Net.XMPP.Jid Jid
        {
            get
            {
                return this.m_Jid;
            }
            set
            {
                this.m_Jid = value;
            }
        }

        private delegate void OnMessageDelegate(Message msg);
    }
}

