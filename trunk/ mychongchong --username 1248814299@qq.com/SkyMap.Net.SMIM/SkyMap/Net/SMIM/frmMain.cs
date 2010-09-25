namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.Core;
    using SkyMap.Net.XMPP;
    using SkyMap.Net.XMPP.net;
    using SkyMap.Net.XMPP.protocol.client;
    using SkyMap.Net.XMPP.protocol.iq.agent;
    using SkyMap.Net.XMPP.protocol.iq.browse;
    using SkyMap.Net.XMPP.protocol.iq.roster;
    using SkyMap.Net.XMPP.protocol.iq.version;
    using SkyMap.Net.XMPP.protocol.x.data;
    using SkyMap.Net.XMPP.ui.roster;
    using SkyMap.Net.XMPP.Xml.Dom;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net.Security;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    public class frmMain : Form
    {
        private bool canClose = false;
        private ToolStripMenuItem chatToolStripMenuItem;
        private IContainer components;
        private ContextMenuStrip contextMenuStripRoster;
        private NotifyIcon niTray;
        private RosterControl rosterControl;
        private StatusBar statusBar1;
        private ToolStripMenuItem vcardToolStripMenuItem;
        private XmppClientConnection XmppCon;

        public frmMain()
        {
            this.InitializeComponent();
            base.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - base.Width, Screen.PrimaryScreen.WorkingArea.Height - base.Height);
            this.XmppCon = new XmppClientConnection();
            this.XmppCon.set_SocketConnectionType(0);
            this.XmppCon.add_OnReadXml(new XmlHandler(this, (IntPtr) this.XmppCon_OnReadXml));
            this.XmppCon.add_OnWriteXml(new XmlHandler(this, (IntPtr) this.XmppCon_OnWriteXml));
            this.XmppCon.add_OnRosterStart(new ObjectHandler(this, (IntPtr) this.XmppCon_OnRosterStart));
            this.XmppCon.add_OnRosterEnd(new ObjectHandler(this, (IntPtr) this.XmppCon_OnRosterEnd));
            this.XmppCon.add_OnRosterItem(new XmppClientConnection.RosterHandler(this, (IntPtr) this.XmppCon_OnRosterItem));
            this.XmppCon.add_OnAgentStart(new ObjectHandler(this, (IntPtr) this.XmppCon_OnAgentStart));
            this.XmppCon.add_OnAgentEnd(new ObjectHandler(this, (IntPtr) this.XmppCon_OnAgentEnd));
            this.XmppCon.add_OnAgentItem(new XmppClientConnection.AgentHandler(this, (IntPtr) this.XmppCon_OnAgentItem));
            this.XmppCon.add_OnLogin(new ObjectHandler(this, (IntPtr) this.XmppCon_OnLogin));
            this.XmppCon.add_OnClose(new ObjectHandler(this, (IntPtr) this.XmppCon_OnClose));
            this.XmppCon.add_OnError(new ErrorHandler(this, (IntPtr) this.XmppCon_OnError));
            this.XmppCon.add_OnPresence(new XmppClientConnection.PresenceHandler(this, (IntPtr) this.XmppCon_OnPresence));
            this.XmppCon.add_OnMessage(new XmppClientConnection.MessageHandler(this, (IntPtr) this.XmppCon_OnMessage));
            this.XmppCon.add_OnIq(new StreamHandler(this, (IntPtr) this.XmppCon_OnIq));
            this.XmppCon.add_OnAuthError(new OnXmppErrorHandler(this, (IntPtr) this.XmppCon_OnAuthError));
            this.XmppCon.add_OnReadSocketData(new BaseSocket.OnSocketDataHandler(this, (IntPtr) this.ClientSocket_OnReceive));
            this.XmppCon.add_OnWriteSocketData(new BaseSocket.OnSocketDataHandler(this, (IntPtr) this.ClientSocket_OnSend));
            this.XmppCon.get_ClientSocket().add_OnValidateCertificate(new RemoteCertificateValidationCallback(this.ClientSocket_OnValidateCertificate));
            this.XmppCon.add_OnXmppConnectionStateChanged(new XmppConnection.XmppConnectionStateHandler(this, (IntPtr) this.XmppCon_OnXmppConnectionStateChanged));
            this.XmppCon.add_OnSaslStart(new SaslEventHandler(this, (IntPtr) this.XmppCon_OnSaslStart));
        }

        private void chatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterNode node = this.rosterControl.SelectedItem();
            if ((node != null) && !Util.ChatForms.ContainsKey(node.get_RosterItem().get_Jid().ToString()))
            {
                new frmChat(node.get_RosterItem().get_Jid(), this.XmppCon, node.Text).Show();
            }
        }

        private void ClientSocket_OnReceive(object sender, byte[] data, int count)
        {
        }

        private void ClientSocket_OnSend(object sender, byte[] data, int count)
        {
        }

        private bool ClientSocket_OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.canClose)
            {
                e.Cancel = true;
                base.Hide();
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
                base.Hide();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmMain));
            this.statusBar1 = new StatusBar();
            this.contextMenuStripRoster = new ContextMenuStrip(this.components);
            this.chatToolStripMenuItem = new ToolStripMenuItem();
            this.vcardToolStripMenuItem = new ToolStripMenuItem();
            this.niTray = new NotifyIcon(this.components);
            this.rosterControl = new RosterControl();
            this.contextMenuStripRoster.SuspendLayout();
            base.SuspendLayout();
            this.statusBar1.Location = new Point(0, 0x17f);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new Size(0x120, 0x19);
            this.statusBar1.TabIndex = 1;
            this.statusBar1.Text = "离线";
            this.contextMenuStripRoster.Items.AddRange(new ToolStripItem[] { this.chatToolStripMenuItem, this.vcardToolStripMenuItem });
            this.contextMenuStripRoster.Name = "contextMenuStripRoster";
            this.contextMenuStripRoster.Size = new Size(0x5f, 0x30);
            this.chatToolStripMenuItem.Name = "chatToolStripMenuItem";
            this.chatToolStripMenuItem.Size = new Size(0x5e, 0x16);
            this.chatToolStripMenuItem.Text = "聊天";
            this.chatToolStripMenuItem.Click += new EventHandler(this.chatToolStripMenuItem_Click);
            this.vcardToolStripMenuItem.Name = "vcardToolStripMenuItem";
            this.vcardToolStripMenuItem.Size = new Size(0x5e, 0x16);
            this.vcardToolStripMenuItem.Text = "名片";
            this.vcardToolStripMenuItem.Click += new EventHandler(this.vcardToolStripMenuItem_Click);
            this.niTray.BalloonTipIcon = ToolTipIcon.Info;
            this.niTray.BalloonTipTitle = "SMIM";
            this.niTray.Icon = (Icon) manager.GetObject("niTray.Icon");
            this.niTray.Text = "SMIM";
            this.niTray.Visible = true;
            this.niTray.MouseDoubleClick += new MouseEventHandler(this.niTray_MouseDoubleClick);
            this.rosterControl.set_ColorGroup(SystemColors.Highlight);
            this.rosterControl.set_ColorResource(SystemColors.ControlText);
            this.rosterControl.set_ColorRoot(SystemColors.Highlight);
            this.rosterControl.set_ColorRoster(SystemColors.ControlText);
            this.rosterControl.set_DefaultGroupName("ungrouped");
            this.rosterControl.Dock = DockStyle.Fill;
            this.rosterControl.set_HideEmptyGroups(true);
            this.rosterControl.Location = new Point(0, 0);
            this.rosterControl.Name = "rosterControl";
            this.rosterControl.Size = new Size(0x120, 0x17f);
            this.rosterControl.TabIndex = 12;
            this.rosterControl.add_SelectionChanged(new EventHandler(this.rosterControl_SelectionChanged));
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x120, 0x198);
            base.Controls.Add(this.rosterControl);
            base.Controls.Add(this.statusBar1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "frmMain";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "SMIM";
            base.TopMost = true;
            base.Resize += new EventHandler(this.frmMain_Resize);
            base.FormClosing += new FormClosingEventHandler(this.frmMain_FormClosing);
            this.contextMenuStripRoster.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void Login(string server, string user, string password, string nickname, int port, bool useSSL)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new OnLoginDelegate(this.Login), new object[] { server, user, password, port, useSSL });
            }
            else
            {
                try
                {
                    if (this.XmppCon.get_XmppConnectionState() != 0)
                    {
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.Info("要先关闭先前的登录连接!");
                        }
                        this.XmppCon.SocketDisconnect();
                    }
                }
                catch
                {
                }
                this.Text = "SMIM－" + nickname;
                this.niTray.BalloonTipText = nickname;
                this.XmppCon.set_Server(server);
                this.XmppCon.set_Username(user);
                this.XmppCon.set_Password(password);
                this.XmppCon.set_Resource("SMIM");
                this.XmppCon.set_Priority(10);
                this.XmppCon.set_Port(port);
                this.XmppCon.set_UseSSL(useSSL);
                this.XmppCon.set_AutoResolveConnectServer(true);
                this.XmppCon.set_ConnectServer(null);
                this.XmppCon.set_SocketConnectionType(0);
                this.XmppCon.set_UseStartTLS(true);
                this.XmppCon.set_RegisterAccount(false);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("准备好要打开连接了...");
                }
                this.XmppCon.Open();
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("登录到了服务器...");
                }
            }
        }

        private void niTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
            }
            if (!base.Visible)
            {
                base.Show();
            }
            base.Activate();
        }

        private void OnBrowseIQ(object sender, IQ iq, object data)
        {
            Element element = iq.SelectSingleElement(typeof(Service));
            if (element != null)
            {
                string[] namespaces = (element as Service).GetNamespaces();
            }
        }

        private void rosterControl_SelectionChanged(object sender, EventArgs e)
        {
            RosterNode node = this.rosterControl.SelectedItem();
            if (node != null)
            {
                if (node.get_NodeType() == 2)
                {
                    this.rosterControl.ContextMenuStrip = this.contextMenuStripRoster;
                }
                else if (node.get_NodeType() == 1)
                {
                    this.rosterControl.ContextMenuStrip = null;
                }
                else if (node.get_NodeType() == 0)
                {
                    this.rosterControl.ContextMenuStrip = null;
                }
                else if (node.get_NodeType() == 3)
                {
                    this.rosterControl.ContextMenuStrip = null;
                }
            }
        }

        private void vcardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterNode node = this.rosterControl.SelectedItem();
            if (node != null)
            {
                new frmVcard(node.get_RosterItem().get_Jid(), this.XmppCon).Show();
            }
        }

        private void XmppCon_OnAgentEnd(object sender)
        {
        }

        private void XmppCon_OnAgentItem(object sender, Agent agent)
        {
        }

        private void XmppCon_OnAgentStart(object sender)
        {
        }

        private void XmppCon_OnAuthError(object sender, Element e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new OnXmppErrorHandler(this, (IntPtr) this.XmppCon_OnAuthError), new object[] { sender, e });
            }
            else
            {
                if (this.XmppCon.get_XmppConnectionState() != 0)
                {
                    this.XmppCon.Close();
                }
                MessageBox.Show("Authentication Error!\r\nWrong password or username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void XmppCon_OnClose(object sender)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new ObjectHandler(this, (IntPtr) this.XmppCon_OnClose), new object[] { sender });
            }
            else
            {
                Console.WriteLine("OnClose");
                this.rosterControl.Clear();
            }
        }

        private void XmppCon_OnError(object sender, Exception ex)
        {
        }

        private void XmppCon_OnIq(object sender, Node e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new StreamHandler(this, (IntPtr) this.XmppCon_OnIq), new object[] { sender, e });
            }
            else
            {
                IQ iq = e as IQ;
                Element element = iq.get_Query();
                if ((element != null) && (element.GetType() == typeof(Version)))
                {
                    Version version = element as Version;
                    if (iq.get_Type() == 0)
                    {
                        iq.SwitchDirection();
                        iq.set_Type(2);
                        version.set_Name("SMIM");
                        version.set_Ver("0.5");
                        version.set_Os(Environment.OSVersion.ToString());
                        this.XmppCon.Send(iq);
                    }
                }
            }
        }

        private void XmppCon_OnLogin(object sender)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new ObjectHandler(this, (IntPtr) this.XmppCon_OnLogin), new object[] { sender });
            }
        }

        private void XmppCon_OnMessage(object sender, Message msg)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new OnMessageDelegate(this.XmppCon_OnMessage), new object[] { sender, msg });
            }
            else if ((msg.get_Type() != 2) && (msg.get_Type() != 0))
            {
                Element element = msg.SelectSingleElement(typeof(Data));
                if (element != null)
                {
                    Data data = element as Data;
                    if (data.get_Type() == 0)
                    {
                        frmXData data2 = new frmXData(data);
                        data2.Text = "xData Form from " + msg.get_From().ToString();
                        data2.Show();
                    }
                }
                else if (!Util.ChatForms.ContainsKey(msg.get_From().get_Bare()))
                {
                    RosterNode rosterItem = this.rosterControl.GetRosterItem(msg.get_From());
                    string nickname = msg.get_From().get_Bare();
                    if (rosterItem != null)
                    {
                        nickname = rosterItem.Text;
                    }
                    frmChat chat = new frmChat(msg.get_From(), this.XmppCon, nickname);
                    chat.CreateControl();
                    chat.Show();
                    chat.IncomingMessage(msg);
                }
            }
        }

        private void XmppCon_OnPresence(object sender, Presence pres)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new OnPresenceDelegate(this.XmppCon_OnPresence), new object[] { sender, pres });
            }
            else if (pres.get_Type() == 0)
            {
                new frmSubscribe(this.XmppCon, pres.get_From()).Show();
            }
            else if (((pres.get_Type() != 1) && (pres.get_Type() != 2)) && (pres.get_Type() != 3))
            {
                try
                {
                    this.rosterControl.SetPresence(pres);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        private void XmppCon_OnReadXml(object sender, string xml)
        {
        }

        private void XmppCon_OnRosterEnd(object sender)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new ObjectHandler(this, (IntPtr) this.XmppCon_OnRosterEnd), new object[] { this });
            }
            else
            {
                this.rosterControl.EndUpdate();
                this.rosterControl.ExpandAll();
                if ((this.XmppCon != null) && this.XmppCon.get_Authenticated())
                {
                    this.XmppCon.set_Show(-1);
                    this.XmppCon.SendMyPresence();
                }
            }
        }

        private void XmppCon_OnRosterItem(object sender, RosterItem item)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new XmppClientConnection.RosterHandler(this, (IntPtr) this.XmppCon_OnRosterItem), new object[] { this, item });
            }
            else if (item.get_Subscription() != 4)
            {
                this.rosterControl.AddRosterItem(item);
            }
            else
            {
                this.rosterControl.RemoveRosterItem(item);
            }
        }

        private void XmppCon_OnRosterStart(object sender)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new ObjectHandler(this, (IntPtr) this.XmppCon_OnRosterStart), new object[] { this });
            }
            else
            {
                this.rosterControl.BeginUpdate();
            }
        }

        private void XmppCon_OnSaslStart(object sender, SaslEventArgs args)
        {
        }

        private void XmppCon_OnWriteXml(object sender, string xml)
        {
        }

        private void XmppCon_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            Console.WriteLine("OnXmppConnectionStateChanged: " + state.ToString());
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new OnXmppConnectionStateChangedDelegate(this.XmppCon_OnXmppConnectionStateChanged), new object[] { sender, state });
            }
            else
            {
                XmppConnectionState state2 = state;
                if (state2 != 0)
                {
                    if (state2 == 4)
                    {
                        this.statusBar1.Text = "在线";
                    }
                }
                else
                {
                    this.statusBar1.Text = "离线";
                }
            }
        }

        public bool CanClose
        {
            get
            {
                return this.canClose;
            }
            set
            {
                this.canClose = value;
            }
        }

        private delegate void OnLoginDelegate(string server, string user, string password, string nickname, int port, bool useSSL);

        private delegate void OnMessageDelegate(object sender, Message msg);

        private delegate void OnPresenceDelegate(object sender, Presence pres);

        private delegate void OnXmppConnectionStateChangedDelegate(object sender, XmppConnectionState state);
    }
}

