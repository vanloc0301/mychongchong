namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP;
    using SkyMap.Net.XMPP.Collections;
    using SkyMap.Net.XMPP.protocol.client;
    using SkyMap.Net.XMPP.protocol.x.muc;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmGroupChat : Form
    {
        private Button cmdChangeSubject;
        private Button cmdSend;
        private IContainer components = null;
        private ColumnHeader headerAffiliation;
        private ColumnHeader headerNickname;
        private ColumnHeader headerRole;
        private ColumnHeader headerStatus;
        private ImageList ilsRoster;
        private Label label1;
        private ListView lvwRoster;
        private string m_Nickname;
        private Jid m_RoomJid;
        private XmppClientConnection m_XmppCon;
        private RichTextBox rtfChat;
        private RichTextBox rtfSend;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private StatusBar statusBar1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txtSubject;

        public frmGroupChat(XmppClientConnection xmppCon, Jid roomJid, string Nickname)
        {
            this.InitializeComponent();
            this.m_RoomJid = roomJid;
            this.m_XmppCon = xmppCon;
            this.m_Nickname = Nickname;
            Util.GroupChatForms.Add(roomJid.get_Bare().ToLower(), this);
            this.m_XmppCon.get_MesagageGrabber().Add(roomJid, new BareJidComparer(), new MessageCB(this, (IntPtr) this.MessageCallback), null);
            this.m_XmppCon.get_PresenceGrabber().Add(roomJid, new BareJidComparer(), new PresenceCB(this, (IntPtr) this.PresenceCallback), null);
        }

        private void cmdChangeSubject_Click(object sender, EventArgs e)
        {
            Message message = new Message();
            message.set_Type(2);
            message.set_To(this.m_RoomJid);
            message.set_Subject(this.txtSubject.Text);
            this.m_XmppCon.Send(message);
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            if (this.rtfSend.Text.Length > 0)
            {
                Message message = new Message();
                message.set_Type(2);
                message.set_To(this.m_RoomJid);
                message.set_Body(this.rtfSend.Text);
                this.m_XmppCon.Send(message);
                this.rtfSend.Text = "";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
            Util.GroupChatForms.Remove(this.m_RoomJid.get_Bare().ToLower());
            this.m_XmppCon.get_MesagageGrabber().Remove(this.m_RoomJid);
            this.m_XmppCon.get_PresenceGrabber().Remove(this.m_RoomJid);
        }

        private ListViewItem FindListViewItem(Jid jid)
        {
            foreach (ListViewItem item in this.lvwRoster.Items)
            {
                if (jid.ToString().ToLower() == item.Tag.ToString().ToLower())
                {
                    return item;
                }
            }
            return null;
        }

        private void frmGroupChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.m_RoomJid != null)
            {
                Presence presence = new Presence();
                presence.set_To(this.m_RoomJid);
                presence.set_Type(4);
                this.m_XmppCon.Send(presence);
            }
        }

        private void frmGroupChat_Load(object sender, EventArgs e)
        {
            if (this.m_RoomJid != null)
            {
                Presence presence = new Presence();
                Jid jid = new Jid(this.m_RoomJid.ToString());
                jid.set_Resource(this.m_Nickname);
                presence.set_To(jid);
                this.m_XmppCon.Send(presence);
            }
        }

        private void IncomingMessage(Message msg)
        {
            if (msg.get_Type() != 0)
            {
                if (msg.get_Subject() != null)
                {
                    this.txtSubject.Text = msg.get_Subject();
                    this.rtfChat.SelectionColor = Color.DarkGreen;
                    this.rtfChat.AppendText(msg.get_From().get_Resource() + " changed subject: ");
                    this.rtfChat.SelectionColor = Color.Black;
                    this.rtfChat.AppendText(msg.get_Subject());
                    this.rtfChat.AppendText("\r\n");
                }
                else if (msg.get_Body() != null)
                {
                    this.rtfChat.SelectionColor = Color.Red;
                    this.rtfChat.AppendText(msg.get_From().get_Resource() + " said: ");
                    this.rtfChat.SelectionColor = Color.Black;
                    this.rtfChat.AppendText(msg.get_Body());
                    this.rtfChat.AppendText("\r\n");
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmGroupChat));
            this.statusBar1 = new StatusBar();
            this.splitContainer1 = new SplitContainer();
            this.lvwRoster = new ListView();
            this.headerNickname = new ColumnHeader();
            this.headerStatus = new ColumnHeader();
            this.ilsRoster = new ImageList(this.components);
            this.splitContainer2 = new SplitContainer();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.rtfChat = new RichTextBox();
            this.label1 = new Label();
            this.cmdChangeSubject = new Button();
            this.txtSubject = new TextBox();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.rtfSend = new RichTextBox();
            this.cmdSend = new Button();
            this.headerRole = new ColumnHeader();
            this.headerAffiliation = new ColumnHeader();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.statusBar1.Location = new Point(0, 330);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new Size(0x26f, 0x18);
            this.statusBar1.TabIndex = 6;
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.lvwRoster);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new Size(0x26f, 330);
            this.splitContainer1.SplitterDistance = 0xbc;
            this.splitContainer1.TabIndex = 7;
            this.lvwRoster.Columns.AddRange(new ColumnHeader[] { this.headerNickname, this.headerStatus, this.headerRole, this.headerAffiliation });
            this.lvwRoster.Dock = DockStyle.Fill;
            this.lvwRoster.Location = new Point(0, 0);
            this.lvwRoster.Name = "lvwRoster";
            this.lvwRoster.Size = new Size(0xbc, 330);
            this.lvwRoster.SmallImageList = this.ilsRoster;
            this.lvwRoster.TabIndex = 0;
            this.lvwRoster.UseCompatibleStateImageBehavior = false;
            this.lvwRoster.View = View.Details;
            this.headerNickname.Text = "Nickname";
            this.headerNickname.Width = 0x52;
            this.headerStatus.Text = "Status";
            this.headerStatus.Width = 0x49;
            this.ilsRoster.ImageStream = (ImageListStreamer) manager.GetObject("ilsRoster.ImageStream");
            this.ilsRoster.TransparentColor = Color.Transparent;
            this.ilsRoster.Images.SetKeyName(0, "");
            this.ilsRoster.Images.SetKeyName(1, "");
            this.ilsRoster.Images.SetKeyName(2, "");
            this.ilsRoster.Images.SetKeyName(3, "");
            this.ilsRoster.Images.SetKeyName(4, "");
            this.ilsRoster.Images.SetKeyName(5, "");
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = Orientation.Horizontal;
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new Size(0x1af, 330);
            this.splitContainer2.SplitterDistance = 0xd7;
            this.splitContainer2.TabIndex = 0;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 302f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 71f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel2.Controls.Add(this.rtfChat, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmdChangeSubject, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSubject, 1, 0);
            this.tableLayoutPanel2.Dock = DockStyle.Fill;
            this.tableLayoutPanel2.Location = new Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Size = new Size(0x1af, 0xd7);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.SetColumnSpan(this.rtfChat, 3);
            this.rtfChat.Dock = DockStyle.Fill;
            this.rtfChat.Location = new Point(3, 0x21);
            this.rtfChat.Name = "rtfChat";
            this.rtfChat.Size = new Size(0x1a9, 0xb3);
            this.rtfChat.TabIndex = 3;
            this.rtfChat.Text = "";
            this.label1.AutoSize = true;
            this.label1.Dock = DockStyle.Fill;
            this.label1.Location = new Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x34, 30);
            this.label1.TabIndex = 4;
            this.label1.Text = "Subject:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.cmdChangeSubject.Dock = DockStyle.Fill;
            this.cmdChangeSubject.Location = new Point(0x16b, 3);
            this.cmdChangeSubject.Name = "cmdChangeSubject";
            this.cmdChangeSubject.Size = new Size(0x41, 0x18);
            this.cmdChangeSubject.TabIndex = 6;
            this.cmdChangeSubject.Text = "change";
            this.cmdChangeSubject.UseVisualStyleBackColor = true;
            this.cmdChangeSubject.Click += new EventHandler(this.cmdChangeSubject_Click);
            this.txtSubject.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtSubject.Location = new Point(0x3d, 3);
            this.txtSubject.Multiline = true;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new Size(0x128, 0x18);
            this.txtSubject.TabIndex = 5;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.rtfSend, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmdSend, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            this.tableLayoutPanel1.Size = new Size(0x1af, 0x6f);
            this.tableLayoutPanel1.TabIndex = 0;
            this.rtfSend.Dock = DockStyle.Fill;
            this.rtfSend.Location = new Point(3, 3);
            this.rtfSend.Name = "rtfSend";
            this.rtfSend.Size = new Size(0x1a9, 0x4b);
            this.rtfSend.TabIndex = 0;
            this.rtfSend.Text = "";
            this.cmdSend.Anchor = AnchorStyles.Right;
            this.cmdSend.Location = new Point(0x15f, 0x54);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new Size(0x4d, 0x18);
            this.cmdSend.TabIndex = 1;
            this.cmdSend.Text = "&Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new EventHandler(this.cmdSend_Click);
            this.headerRole.Text = "Role";
            this.headerAffiliation.Text = "Affiliation";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x26f, 0x162);
            base.Controls.Add(this.splitContainer1);
            base.Controls.Add(this.statusBar1);
            base.Name = "frmGroupChat";
            this.Text = "frmGroupChat";
            base.FormClosed += new FormClosedEventHandler(this.frmGroupChat_FormClosed);
            base.Load += new EventHandler(this.frmGroupChat_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void MessageCallback(object sender, Message msg, object data)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new MessageCB(this, (IntPtr) this.MessageCallback), new object[] { sender, msg, data });
            }
            else if (msg.get_Type() == 2)
            {
                this.IncomingMessage(msg);
            }
        }

        private void PresenceCallback(object sender, Presence pres, object data)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new PresenceCB(this, (IntPtr) this.PresenceCallback), new object[] { sender, pres, data });
            }
            else
            {
                int rosterImageIndex;
                User user;
                ListViewItem item = this.FindListViewItem(pres.get_From());
                if (item != null)
                {
                    if (pres.get_Type() == 4)
                    {
                        item.Remove();
                    }
                    else
                    {
                        rosterImageIndex = Util.GetRosterImageIndex(pres);
                        item.ImageIndex = rosterImageIndex;
                        item.SubItems[1].Text = (pres.get_Status() == null) ? "" : pres.get_Status();
                        user = pres.SelectSingleElement(typeof(User)) as User;
                        if (user != null)
                        {
                            item.SubItems[2].Text = user.get_Item().get_Affiliation().ToString();
                            item.SubItems[3].Text = user.get_Item().get_Role().ToString();
                        }
                    }
                }
                else
                {
                    rosterImageIndex = Util.GetRosterImageIndex(pres);
                    ListViewItem item2 = new ListViewItem(pres.get_From().get_Resource());
                    item2.Tag = pres.get_From().ToString();
                    item2.SubItems.Add((pres.get_Status() == null) ? "" : pres.get_Status());
                    user = pres.SelectSingleElement(typeof(User)) as User;
                    if (user != null)
                    {
                        item2.SubItems.Add(user.get_Item().get_Affiliation().ToString());
                        item2.SubItems.Add(user.get_Item().get_Role().ToString());
                    }
                    item2.ImageIndex = rosterImageIndex;
                    this.lvwRoster.Items.Add(item2);
                }
            }
        }
    }
}

