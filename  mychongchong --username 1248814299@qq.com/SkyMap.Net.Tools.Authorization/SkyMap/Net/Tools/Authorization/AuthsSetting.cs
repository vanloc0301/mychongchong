namespace SkyMap.Net.Tools.Authorization
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AuthsSetting : SmUserControl
    {
        private IAuthSetting authSetting;
        private Button btCancel;
        private Button btOk;
        private ContextMenu cmTvAuthorizations;
        private Container components = null;
        private TreeNode currentNode;
        private UnitOfWork currentUnitOfWork;
        private Label label1;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private MenuItem miAdd;
        private MenuItem miDel;
        private TabPage pAuthSetting;
        private TabPage pAuthType;
        private Panel pClient;
        private Splitter splitter1;
        private TabControl tabAuthorization;
        private TreeView tvAuthorizations;
        private TextBox txtAuthGetClass;
        private TextBox txtAuthName;
        private TextBox txtAuthSetClass;
        private TextBox txtAuthTable;
        private TextBox txtDescription;
        private TextBox txtResourceIdField;
        private TextBox txtResourceName;
        private TextBox txtResourceNameField;
        private TextBox txtResourceTable;
        private TextBox txtTypeCode;

        public AuthsSetting()
        {
            this.InitializeComponent();
            SkyMap.Net.Tools.Authorization.TreeViewHelper.SetImageList(this.tvAuthorizations);
            this.InitTvAuthorizations(this.tvAuthorizations);
            this.SetButton(false);
            this.CreateCurrentUnitOfWork();
        }

        private TreeNode AddAuthNode(CAuthType authType, TreeNodeCollection nodes)
        {
            return SkyMap.Net.Tools.Authorization.TreeViewHelper.AddTreeNode(nodes, this.GetAuthTypeNodeText(authType), authType);
        }

        private void AuthDataChanged(object sender, EventArgs e)
        {
            this.currentUnitOfWork.RegisterDirty(this.currentNode.Tag as CAuthType);
        }

        private void btCancelClick(object sender, EventArgs e)
        {
            this.currentUnitOfWork.Clear();
            CAuthType currentAuthType = this.GetCurrentAuthType();
            if (currentAuthType != null)
            {
                QueryHelper.Refresh(currentAuthType);
                this.DisplayAuthorization(currentAuthType);
                if (this.tabAuthorization.SelectedIndex != 0)
                {
                    this.tabAuthorization.SelectedIndex = 0;
                }
            }
        }

        private void btOkClick(object sender, EventArgs e)
        {
            this.Save();
        }

        private bool CheckAuthType(CAuthType authType)
        {
            if ((((StringHelper.IsNull(authType.Name) || StringHelper.IsNull(authType.ResourceIdField)) || (StringHelper.IsNull(authType.ResourceNameField) || StringHelper.IsNull(authType.ResourceTable))) || StringHelper.IsNull(authType.AuthTable)) || StringHelper.IsNull(authType.AuthSetClass))
            {
                return false;
            }
            return true;
        }

        private void cmTvAuthorizations_Popup(object sender, EventArgs e)
        {
            this.miAdd.Enabled = this.tvAuthorizations.SelectedNode.Parent == null;
            this.miDel.Enabled = !this.miAdd.Enabled;
        }

        private void CreateCurrentUnitOfWork()
        {
            //this.currentUnitOfWork = new UnitOfWork(typeof(CAuthType).Namespace);
            this.currentUnitOfWork = new UnitOfWork(typeof(CAuthType));
            this.currentUnitOfWork.Changed += new EventHandler(this.CurrenUnitOfWorkChanged);
            this.currentUnitOfWork.Cleared += new EventHandler(this.CurrenUnitOfWorkCleared);
        }

        private void CurrenUnitOfWorkChanged(object sender, EventArgs e)
        {
            this.SetButton(true);
        }

        private void CurrenUnitOfWorkCleared(object sender, EventArgs e)
        {
            this.SetButton(false);
        }

        private void DisplayAuthorization(CAuthType authType)
        {
            BinderHelper.DoBinding(new TextBox[] { this.txtAuthName, this.txtTypeCode, this.txtResourceName, this.txtResourceTable, this.txtResourceIdField, this.txtResourceNameField, this.txtAuthTable, this.txtAuthSetClass, this.txtAuthGetClass, this.txtDescription }, authType, new string[] { "Name", "TypeCode", "ResourceName", "ResourceTable", "ResourceIdField", "ResourceNameField", "AuthTable", "AuthSetClass", "AuthGetClass", "Description" });
            this.BindingContext[authType].CurrentChanged += new EventHandler(this.AuthDataChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetAuthTypeNodeText(CAuthType authType)
        {
            if (authType == null)
            {
                throw new ArgumentNullException();
            }
            return authType.Name;
        }

        private CAuthType GetCurrentAuthType()
        {
            if ((this.currentNode != null) && (this.currentNode.Tag is CAuthType))
            {
                return (this.currentNode.Tag as CAuthType);
            }
            return null;
        }

        private CAuthType GetNewAuthType()
        {
            CAuthType type = new CAuthType();
            type.Name = "新的权限管理";
            this.currentUnitOfWork.RegisterNew(type);
            this.currentUnitOfWork.Commit();
            return type;
        }

        private void InitializeComponent()
        {
            this.tvAuthorizations = new TreeView();
            this.cmTvAuthorizations = new ContextMenu();
            this.miAdd = new MenuItem();
            this.miDel = new MenuItem();
            this.splitter1 = new Splitter();
            this.pClient = new Panel();
            this.btCancel = new Button();
            this.btOk = new Button();
            this.tabAuthorization = new TabControl();
            this.pAuthType = new TabPage();
            this.txtTypeCode = new TextBox();
            this.label10 = new Label();
            this.txtResourceNameField = new TextBox();
            this.label8 = new Label();
            this.txtResourceIdField = new TextBox();
            this.label9 = new Label();
            this.txtDescription = new TextBox();
            this.label7 = new Label();
            this.txtAuthGetClass = new TextBox();
            this.label6 = new Label();
            this.txtAuthSetClass = new TextBox();
            this.label5 = new Label();
            this.txtAuthTable = new TextBox();
            this.label4 = new Label();
            this.txtResourceTable = new TextBox();
            this.label3 = new Label();
            this.txtResourceName = new TextBox();
            this.label2 = new Label();
            this.txtAuthName = new TextBox();
            this.label1 = new Label();
            this.pAuthSetting = new TabPage();
            this.pClient.SuspendLayout();
            this.tabAuthorization.SuspendLayout();
            this.pAuthType.SuspendLayout();
            base.SuspendLayout();
            this.tvAuthorizations.ContextMenu = this.cmTvAuthorizations;
            this.tvAuthorizations.Dock = DockStyle.Left;
            this.tvAuthorizations.HideSelection = false;
            this.tvAuthorizations.Location = new Point(3, 3);
            this.tvAuthorizations.Name = "tvAuthorizations";
            this.tvAuthorizations.Size = new Size(0x79, 450);
            this.tvAuthorizations.TabIndex = 0;
            this.tvAuthorizations.AfterSelect += new TreeViewEventHandler(this.NodeSelected);
            this.cmTvAuthorizations.MenuItems.AddRange(new MenuItem[] { this.miAdd, this.miDel });
            this.cmTvAuthorizations.Popup += new EventHandler(this.cmTvAuthorizations_Popup);
            this.miAdd.Index = 0;
            this.miAdd.Text = "添加";
            this.miAdd.Click += new EventHandler(this.miAddClick);
            this.miDel.Index = 1;
            this.miDel.Text = "删除";
            this.miDel.Click += new EventHandler(this.miDel_Click);
            this.splitter1.Location = new Point(0x7c, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 450);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.pClient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pClient.Controls.Add(this.btCancel);
            this.pClient.Controls.Add(this.btOk);
            this.pClient.Controls.Add(this.tabAuthorization);
            this.pClient.Dock = DockStyle.Fill;
            this.pClient.Location = new Point(0x7f, 3);
            this.pClient.Name = "pClient";
            this.pClient.Padding = new Padding(20, 20, 20, 40);
            this.pClient.Size = new Size(0x1a6, 450);
            this.pClient.TabIndex = 2;
            this.btCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btCancel.Location = new Point(320, 0x1a0);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x4b, 0x17);
            this.btCancel.TabIndex = 0x1d;
            this.btCancel.Text = "取消";
            this.btCancel.Click += new EventHandler(this.btCancelClick);
            this.btOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btOk.Location = new Point(0xe0, 0x1a0);
            this.btOk.Name = "btOk";
            this.btOk.Size = new Size(0x4b, 0x17);
            this.btOk.TabIndex = 0x1c;
            this.btOk.Text = "保存";
            this.btOk.Click += new EventHandler(this.btOkClick);
            this.tabAuthorization.Controls.Add(this.pAuthType);
            this.tabAuthorization.Controls.Add(this.pAuthSetting);
            this.tabAuthorization.Dock = DockStyle.Fill;
            this.tabAuthorization.Location = new Point(20, 20);
            this.tabAuthorization.Name = "tabAuthorization";
            this.tabAuthorization.SelectedIndex = 0;
            this.tabAuthorization.Size = new Size(0x17a, 0x182);
            this.tabAuthorization.TabIndex = 0;
            this.tabAuthorization.SelectedIndexChanged += new EventHandler(this.tabAuthorization_SelectedIndexChanged);
            this.pAuthType.Controls.Add(this.txtTypeCode);
            this.pAuthType.Controls.Add(this.label10);
            this.pAuthType.Controls.Add(this.txtResourceNameField);
            this.pAuthType.Controls.Add(this.label8);
            this.pAuthType.Controls.Add(this.txtResourceIdField);
            this.pAuthType.Controls.Add(this.label9);
            this.pAuthType.Controls.Add(this.txtDescription);
            this.pAuthType.Controls.Add(this.label7);
            this.pAuthType.Controls.Add(this.txtAuthGetClass);
            this.pAuthType.Controls.Add(this.label6);
            this.pAuthType.Controls.Add(this.txtAuthSetClass);
            this.pAuthType.Controls.Add(this.label5);
            this.pAuthType.Controls.Add(this.txtAuthTable);
            this.pAuthType.Controls.Add(this.label4);
            this.pAuthType.Controls.Add(this.txtResourceTable);
            this.pAuthType.Controls.Add(this.label3);
            this.pAuthType.Controls.Add(this.txtResourceName);
            this.pAuthType.Controls.Add(this.label2);
            this.pAuthType.Controls.Add(this.txtAuthName);
            this.pAuthType.Controls.Add(this.label1);
            this.pAuthType.Location = new Point(4, 0x15);
            this.pAuthType.Name = "pAuthType";
            this.pAuthType.Size = new Size(370, 0x169);
            this.pAuthType.TabIndex = 0;
            this.pAuthType.Text = "基本属性";
            this.txtTypeCode.Location = new Point(110, 0x38);
            this.txtTypeCode.MaxLength = 50;
            this.txtTypeCode.Name = "txtTypeCode";
            this.txtTypeCode.Size = new Size(0x102, 0x15);
            this.txtTypeCode.TabIndex = 0x21;
            this.txtTypeCode.Text = "txtTypeCode";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x27, 0x3f);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x35, 12);
            this.label10.TabIndex = 0x20;
            this.label10.Text = "类型代码";
            this.txtResourceNameField.Location = new Point(110, 0xb8);
            this.txtResourceNameField.MaxLength = 50;
            this.txtResourceNameField.Name = "txtResourceNameField";
            this.txtResourceNameField.Size = new Size(0x102, 0x15);
            this.txtResourceNameField.TabIndex = 0x1f;
            this.txtResourceNameField.Text = "txtResourceNameField";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(14, 0xbb);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x4d, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "资源名称字段";
            this.txtResourceIdField.Location = new Point(110, 0x98);
            this.txtResourceIdField.MaxLength = 50;
            this.txtResourceIdField.Name = "txtResourceIdField";
            this.txtResourceIdField.Size = new Size(0x102, 0x15);
            this.txtResourceIdField.TabIndex = 0x1d;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x1b, 0x9c);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x41, 12);
            this.label9.TabIndex = 0x1c;
            this.label9.Text = "资源ID字段";
            this.txtDescription.Location = new Point(110, 0x138);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(0x102, 0x15);
            this.txtDescription.TabIndex = 0x1b;
            this.txtDescription.Text = "textBox6";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x40, 0x137);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x1d, 12);
            this.label7.TabIndex = 0x1a;
            this.label7.Text = "备注";
            this.txtAuthGetClass.Location = new Point(110, 280);
            this.txtAuthGetClass.MaxLength = 100;
            this.txtAuthGetClass.Name = "txtAuthGetClass";
            this.txtAuthGetClass.Size = new Size(0x102, 0x15);
            this.txtAuthGetClass.TabIndex = 0x17;
            this.txtAuthGetClass.Text = "textBox6";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x1b, 280);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x41, 12);
            this.label6.TabIndex = 0x16;
            this.label6.Text = "权限获取类";
            this.txtAuthSetClass.Location = new Point(110, 0xf8);
            this.txtAuthSetClass.MaxLength = 100;
            this.txtAuthSetClass.Name = "txtAuthSetClass";
            this.txtAuthSetClass.Size = new Size(0x102, 0x15);
            this.txtAuthSetClass.TabIndex = 0x15;
            this.txtAuthSetClass.Text = "textBox5";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x1b, 0xf9);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x41, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "权限设置类";
            this.txtAuthTable.Location = new Point(110, 0xd8);
            this.txtAuthTable.MaxLength = 50;
            this.txtAuthTable.Name = "txtAuthTable";
            this.txtAuthTable.Size = new Size(0x102, 0x15);
            this.txtAuthTable.TabIndex = 0x13;
            this.txtAuthTable.Text = "textBox4";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x27, 0xda);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x35, 12);
            this.label4.TabIndex = 0x12;
            this.label4.Text = "权限表名";
            this.txtResourceTable.Location = new Point(110, 120);
            this.txtResourceTable.MaxLength = 50;
            this.txtResourceTable.Name = "txtResourceTable";
            this.txtResourceTable.Size = new Size(0x102, 0x15);
            this.txtResourceTable.TabIndex = 0x11;
            this.txtResourceTable.Text = "textBox3";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x27, 0x7d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x35, 12);
            this.label3.TabIndex = 0x10;
            this.label3.Text = "资源表名";
            this.txtResourceName.Location = new Point(110, 0x58);
            this.txtResourceName.MaxLength = 50;
            this.txtResourceName.Name = "txtResourceName";
            this.txtResourceName.Size = new Size(0x102, 0x15);
            this.txtResourceName.TabIndex = 15;
            this.txtResourceName.Text = "textBox2";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x27, 0x5e);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "资源名称";
            this.txtAuthName.Location = new Point(110, 0x18);
            this.txtAuthName.MaxLength = 50;
            this.txtAuthName.Name = "txtAuthName";
            this.txtAuthName.Size = new Size(0x102, 0x15);
            this.txtAuthName.TabIndex = 13;
            this.txtAuthName.Text = "textBox1";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x27, 0x20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "权限名称";
            this.pAuthSetting.Location = new Point(4, 0x15);
            this.pAuthSetting.Name = "pAuthSetting";
            this.pAuthSetting.Size = new Size(370, 0x169);
            this.pAuthSetting.TabIndex = 1;
            this.pAuthSetting.Text = "权限设置";
            base.Controls.Add(this.pClient);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.tvAuthorizations);
            base.Name = "AuthsSetting";
            base.Padding = new Padding(3);
            base.Size = new Size(0x228, 0x1c8);
            this.pClient.ResumeLayout(false);
            this.tabAuthorization.ResumeLayout(false);
            this.pAuthType.ResumeLayout(false);
            this.pAuthType.PerformLayout();
            base.ResumeLayout(false);
        }

        private void InitTvAuthorizations(TreeView tree)
        {
            tree.Nodes.Clear();
            // IList list = QueryHelper.List(typeof(CAuthType));
            IList list = QueryHelper.List(typeof(CAuthType).Namespace,"","");
            TreeNode node = SkyMap.Net.Tools.Authorization.TreeViewHelper.AddTreeNode(this.tvAuthorizations.Nodes, "权限管理集合");
            foreach (CAuthType type in list)
            {
                this.AddAuthNode(type, node.Nodes);
            }
            node.Expand();
            tree.SelectedNode = node;
        }

        private void miAddClick(object sender, EventArgs e)
        {
            if (this.tvAuthorizations.SelectedNode.Parent == null)
            {
                CAuthType newAuthType = this.GetNewAuthType();
                TreeNode node = this.AddAuthNode(newAuthType, this.tvAuthorizations.SelectedNode.Nodes);
                this.tvAuthorizations.SelectedNode = node;
            }
        }

        private void miDel_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowOkCancelInfo("你确定要删除它吗？") == DialogResult.OK)
            {
                this.currentUnitOfWork.Clear();
                this.currentUnitOfWork.RegisterRemoved(this.currentNode.Tag as CAuthType);
                this.currentUnitOfWork.Commit();
                this.currentNode.Remove();
            }
        }

        private void NodeSelected(object sender, TreeViewEventArgs e)
        {
            if (this.IsDirty)
            {
                this.Save();
            }
            this.currentNode = e.Node;
            this.tabAuthorization.Visible = false;
            CAuthType currentAuthType = this.GetCurrentAuthType();
            if (currentAuthType != null)
            {
                this.DisplayAuthorization(currentAuthType);
                this.tabAuthorization.SelectedIndex = 0;
                this.tabAuthorization.Visible = true;
            }
        }

        private void PostEditor()
        {
            CAuthType currentAuthType = this.GetCurrentAuthType();
            if (currentAuthType != null)
            {
                this.BindingContext[currentAuthType].EndCurrentEdit();
            }
            if (this.authSetting != null)
            {
                this.authSetting.PostEditor();
            }
        }

        public void Save()
        {
            this.PostEditor();
            this.currentUnitOfWork.Commit();
        }

        private void SetButton(bool b)
        {
            this.btOk.Enabled = b;
            this.btCancel.Enabled = b;
        }

        private void tabAuthorization_SelectedIndexChanged(object sender, EventArgs e)
        {
            WaitDialogHelper.Show();
            try
            {
                if (this.tabAuthorization.SelectedTab == this.pAuthSetting)
                {
                    CAuthType tag = this.currentNode.Tag as CAuthType;
                    this.BindingContext[tag].EndCurrentEdit();
                    if (!this.CheckAuthType(tag))
                    {
                        throw new CoreException("权限类型的基本属性不完整，无法开启权限设置！");
                    }
                    System.Type type = System.Type.GetType(tag.AuthSetClass);
                    if (type == null)
                    {
                        throw new CoreException("权限设置类型设置错误，无法获取相应类型，请重新设置！");
                    }
                    if (type.GetInterface("IAuthSetting", true) == null)
                    {
                        throw new CoreException("权限设置类型没有实现’IAuthSetting‘接口，请重新设置！");
                    }
                    if ((this.authSetting != null) && !this.authSetting.GetType().Equals(type))
                    {
                        this.pAuthSetting.Controls.Clear();
                        (this.authSetting as Control).Dispose();
                        this.authSetting = null;
                    }
                    if (this.authSetting == null)
                    {
                        this.authSetting = Activator.CreateInstance(type) as IAuthSetting;
                        (this.authSetting as Control).Dock = DockStyle.Fill;
                        this.pAuthSetting.Controls.Add(this.authSetting as Control);
                    }
                    if (!((this.authSetting.AuthType != null) && this.authSetting.AuthType.Equals(tag)))
                    {
                        this.authSetting.AuthType = tag;
                        this.authSetting.CurrentUnitOfWork = this.currentUnitOfWork;
                    }
                }
            }
            catch (CoreException exception)
            {
                MessageHelper.ShowInfo(exception.Message);
                this.tabAuthorization.SelectedTab = this.pAuthType;
            }
            finally
            {
                WaitDialogHelper.Close();
            }
        }

        public bool IsDirty
        {
            get
            {
                return (this.tabAuthorization.Visible && ((this.GetCurrentAuthType() != null) && this.btOk.Enabled));
            }
        }
    }
}

