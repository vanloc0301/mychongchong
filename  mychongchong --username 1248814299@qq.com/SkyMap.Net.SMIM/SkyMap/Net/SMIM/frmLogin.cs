namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP;
    using SkyMap.Net.XMPP.Xml.Dom;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class frmLogin : Form
    {
        private XmppClientConnection _connection;
        private CheckBox chkRegister;
        private CheckBox chkSSL;
        private Button cmdCancel;
        private Button cmdLogin;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private NumericUpDown numPriority;
        private TextBox txtJid;
        private TextBox txtPassword;
        private TextBox txtPort;
        private TextBox txtResource;

        public frmLogin(XmppClientConnection con)
        {
            this.InitializeComponent();
            this.LoadSettings();
            base.DialogResult = DialogResult.Cancel;
            this._connection = con;
        }

        private void chkSSL_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPort.Text = "5223";
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void cmdLogin_Click(object sender, EventArgs e)
        {
            Jid jid = new Jid(this.txtJid.Text);
            this._connection.set_Server(jid.get_Server());
            this._connection.set_Username(jid.get_User());
            this._connection.set_Password(this.txtPassword.Text);
            this._connection.set_Resource(this.txtResource.Text);
            this._connection.set_Priority((int) this.numPriority.Value);
            this._connection.set_Port(int.Parse(this.txtPort.Text));
            this._connection.set_UseSSL(this.chkSSL.Checked);
            this._connection.set_AutoResolveConnectServer(true);
            this._connection.set_ConnectServer(null);
            this._connection.set_SocketConnectionType(0);
            this._connection.set_UseStartTLS(true);
            if (this.chkRegister.Checked)
            {
                this._connection.set_RegisterAccount(true);
            }
            else
            {
                this._connection.set_RegisterAccount(false);
            }
            base.DialogResult = DialogResult.OK;
            this.SaveSettings();
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
            this.label1 = new Label();
            this.txtJid = new TextBox();
            this.cmdLogin = new Button();
            this.cmdCancel = new Button();
            this.label2 = new Label();
            this.txtPassword = new TextBox();
            this.label3 = new Label();
            this.numPriority = new NumericUpDown();
            this.label4 = new Label();
            this.label5 = new Label();
            this.txtResource = new TextBox();
            this.label6 = new Label();
            this.txtPort = new TextBox();
            this.chkSSL = new CheckBox();
            this.chkRegister = new CheckBox();
            this.numPriority.BeginInit();
            base.SuspendLayout();
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Jabber ID:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.txtJid.Location = new Point(80, 8);
            this.txtJid.Name = "txtJid";
            this.txtJid.Size = new Size(0xa8, 20);
            this.txtJid.TabIndex = 0;
            this.cmdLogin.FlatStyle = FlatStyle.System;
            this.cmdLogin.Location = new Point(0x8b, 0xd5);
            this.cmdLogin.Name = "cmdLogin";
            this.cmdLogin.Size = new Size(0x58, 0x18);
            this.cmdLogin.TabIndex = 6;
            this.cmdLogin.Text = "Login";
            this.cmdLogin.Click += new EventHandler(this.cmdLogin_Click);
            this.cmdCancel.FlatStyle = FlatStyle.System;
            this.cmdCancel.Location = new Point(0x20, 0xd5);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(0x58, 0x18);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
            this.label2.Location = new Point(80, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xb0, 0x10);
            this.label2.TabIndex = 4;
            this.label2.Text = "user@server.org";
            this.txtPassword.Location = new Point(80, 0x38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(0xa8, 20);
            this.txtPassword.TabIndex = 1;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(8, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x10);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.numPriority.Location = new Point(80, 0x58);
            this.numPriority.Name = "numPriority";
            this.numPriority.Size = new Size(40, 20);
            this.numPriority.TabIndex = 2;
            int[] bits = new int[4];
            bits[0] = 10;
            this.numPriority.Value = new decimal(bits);
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(8, 0x58);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x38, 0x10);
            this.label4.TabIndex = 8;
            this.label4.Text = "Priority:";
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(8, 120);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x40, 0x10);
            this.label5.TabIndex = 9;
            this.label5.Text = "Resource:";
            this.txtResource.Location = new Point(80, 120);
            this.txtResource.Name = "txtResource";
            this.txtResource.Size = new Size(0xa8, 20);
            this.txtResource.TabIndex = 4;
            this.txtResource.Text = "MiniClient";
            this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label6.Location = new Point(0x88, 0x58);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x20, 0x10);
            this.label6.TabIndex = 10;
            this.label6.Text = "Port:";
            this.txtPort.Location = new Point(0xb0, 0x58);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new Size(0x48, 20);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "5222";
            this.chkSSL.FlatStyle = FlatStyle.System;
            this.chkSSL.Location = new Point(80, 0x98);
            this.chkSSL.Name = "chkSSL";
            this.chkSSL.Size = new Size(160, 0x10);
            this.chkSSL.TabIndex = 5;
            this.chkSSL.Text = "use SSL (old style SSL)";
            this.chkSSL.CheckedChanged += new EventHandler(this.chkSSL_CheckedChanged);
            this.chkRegister.FlatStyle = FlatStyle.System;
            this.chkRegister.Location = new Point(80, 0xae);
            this.chkRegister.Name = "chkRegister";
            this.chkRegister.Size = new Size(160, 0x10);
            this.chkRegister.TabIndex = 11;
            this.chkRegister.Text = "register new Account";
            base.AcceptButton = this.cmdLogin;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x102, 0xf9);
            base.Controls.Add(this.chkRegister);
            base.Controls.Add(this.txtPort);
            base.Controls.Add(this.txtResource);
            base.Controls.Add(this.txtPassword);
            base.Controls.Add(this.txtJid);
            base.Controls.Add(this.chkSSL);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.numPriority);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cmdCancel);
            base.Controls.Add(this.cmdLogin);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "frmLogin";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Login form";
            this.numPriority.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadSettings()
        {
            if (File.Exists(this.SettingsFilename))
            {
                Document document = new Document();
                document.LoadFile(this.SettingsFilename);
                Element element = document.get_RootElement().SelectSingleElement("Login");
                this.txtJid.Text = element.GetTag("Jid");
                this.txtPassword.Text = element.GetTag("Password");
                this.txtResource.Text = element.GetTag("Resource");
                this.numPriority.Value = element.GetTagInt("Priority");
                this.chkSSL.Checked = element.GetTagBool("Ssl");
            }
        }

        private void SaveSettings()
        {
            Document document = new Document();
            Element element = new Element("Settings");
            Element element2 = new Element("Login");
            element2.get_ChildNodes().Add(new Element("Jid", this.txtJid.Text));
            element2.get_ChildNodes().Add(new Element("Password", this.txtPassword.Text));
            element2.get_ChildNodes().Add(new Element("Resource", this.txtResource.Text));
            element2.get_ChildNodes().Add(new Element("Priority", this.numPriority.Value.ToString()));
            element2.get_ChildNodes().Add(new Element("Port", this.txtPort.Text));
            element2.get_ChildNodes().Add(new Element("Ssl", this.chkSSL.Checked));
            document.get_ChildNodes().Add(element);
            element.get_ChildNodes().Add(element2);
            document.Save(this.SettingsFilename);
        }

        private string SettingsFilename
        {
            get
            {
                return (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings.xml");
            }
        }
    }
}

