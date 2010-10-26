namespace SkyMap.Net.Gui.Dialogs
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;

    public class UserLogin : SmForm
    {
        private BackgroundWorker backgroundWorker;
        private SimpleButton btCancel;
        private SimpleButton btOk;
        private Container components = null;
        private MarqueeProgressBarControl marqueeProgressBarControl1;
        private OGMTreeView ogmTv;
        private PopupContainerControl pcContainer;
        private TextEdit txtPassword;
        private PictureBox pClose;
        private DevExpress.XtraEditors.PopupContainerEdit txtUser;

        public UserLogin()
        {
            this.InitializeComponent();
            this.InitMe();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        }


        protected void Save()
        {
            this.txtUser.Enabled = true;
        }
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.marqueeProgressBarControl1.Hide();

            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("异步操作完成");
            }
            if (e.Error != null)
            {
                LoggingService.Error("发生错误:", e.Error);
                if ((e.Error.InnerException != null) && (e.Error.InnerException is WebException))
                {
                    LoggingService.Error("内部错误是：", e.Error.InnerException);
                    MessageHelper.ShowInfo("网络连接有问题，有可能是：\r\n1.服务端没有运行；\r\n2.你所在的网络连接有问题!");
                }
                else
                {
                    MessageHelper.ShowError("异步操作错误发生...", e.Error);
                }
            }
            else
            {
                this.LoadOGM(this, null);
                if (this.txtUser.EditValue == null)
                {
                    this.txtUser.Text = "请选择...";
                }
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            LoggingService.DebugFormatted("使用用户：{0}登录系统...", new object[] { this.txtUser.Text });
            string name = string.Empty;
            CStaffDept editValue = this.txtUser.EditValue as CStaffDept;
            if (editValue == null)
            {
                name = "Dialog.UserLogin.Message.NoSelectUser";
            }
            else if (!OGMService.CheckPassword(editValue.Staff, (string)this.txtPassword.EditValue))
            {
                name = "Dialog.UserLogin.Message.PasswordError";
                this.txtPassword.SelectAll();
            }
            if (name != string.Empty)
            {
                MessageHelper.ShowInfo(ResourceService.GetString(name));
                base.DialogResult = DialogResult.None;
            }
            else
            {
                OGMService.SetSmPrincipal(editValue.Staff, editValue.Dept);
                DAOCacheService.Put("LastLoginStaff", editValue);
                PropertyService.Set<string>("LastLoginDeptId", editValue.Dept.Id);
                PropertyService.Set<string>("LastLoginStaffId", editValue.Staff.Id);
            }
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
            this.txtUser = new DevExpress.XtraEditors.PopupContainerEdit();
            this.pcContainer = new DevExpress.XtraEditors.PopupContainerControl();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.btOk = new DevExpress.XtraEditors.SimpleButton();
            this.btCancel = new DevExpress.XtraEditors.SimpleButton();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.pClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pClose)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(182, 95);
            this.txtUser.Name = "txtUser";
            this.txtUser.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtUser.Properties.PopupControl = this.pcContainer;
            this.txtUser.Size = new System.Drawing.Size(154, 21);
            this.txtUser.TabIndex = 3;
            this.txtUser.DoubleClick += new System.EventHandler(this.ogmTv_DoubleClick);
            // 
            // pcContainer
            // 
            this.pcContainer.Location = new System.Drawing.Point(12, 165);
            this.pcContainer.Name = "pcContainer";
            this.pcContainer.Size = new System.Drawing.Size(72, 24);
            this.pcContainer.TabIndex = 4;
            // 
            // txtPassword
            // 
            this.txtPassword.EditValue = "";
            this.txtPassword.Location = new System.Drawing.Point(183, 127);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(154, 21);
            this.txtPassword.TabIndex = 0;
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(183, 165);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "登录";
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(261, 165);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "取消";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.marqueeProgressBarControl1.EditValue = "";
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(0, 268);
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(466, 14);
            this.marqueeProgressBarControl1.TabIndex = 8;
            // 
            // pClose
            // 
            this.pClose.Image = global::Properties.Resources.关闭;
            this.pClose.InitialImage = null;
            this.pClose.Location = new System.Drawing.Point(431, 2);
            this.pClose.Name = "pClose";
            this.pClose.Size = new System.Drawing.Size(19, 17);
            this.pClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pClose.TabIndex = 9;
            this.pClose.TabStop = false;
            this.pClose.MouseLeave += new System.EventHandler(this.pClose_MouseLeave);
            this.pClose.Click += new System.EventHandler(this.pClose_Click);
            this.pClose.MouseHover += new System.EventHandler(this.pClose_MouseHover);
            // 
            // UserLogin
            // 
            this.AcceptButton = this.btOk;
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = global::Properties.Resources.评估管理软件登录;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(466, 282);
            this.Controls.Add(this.pClose);
            this.Controls.Add(this.marqueeProgressBarControl1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.pcContainer);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.Name = "UserLogin";
            this.Text = "UserLogin";
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitMe()
        {
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = ResourceService.GetString("Dialog.UserLogin.Text");
            base.Icon = ResourceService.GetIcon("Dialog.UserLogin.Icon");
            this.btOk.Text = ResourceService.GetString("Dialog.UserLogin.LoginButtonText");
            this.btCancel.Text = ResourceService.GetString("Global.CancelButtonText");
            this.txtPassword.Properties.PasswordChar = '*';
            this.btOk.Image = ResourceService.GetBitmap("Global.OkButtonImage");
            this.btCancel.Image = ResourceService.GetBitmap("Global.CancelButtonImage");
            this.btOk.DialogResult = DialogResult.OK;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.txtUser.EditValue = CacheService.Get("LastLoginStaff");
            this.ogmTv = new OGMTreeView();
            this.ogmTv.Dock = DockStyle.Fill;
            this.pcContainer.Size = this.ogmTv.Size;
            this.pcContainer.Controls.Add(this.ogmTv);
            this.ogmTv.HandleCreated += new EventHandler(this.ogmTv_HandleCreated);
            this.ogmTv.DoubleClick += new EventHandler(this.ogmTv_DoubleClick);
            if (this.txtUser.EditValue == null)
            {
                string str = PropertyService.Get<string>("LastLoginDeptId", string.Empty);
                string str2 = PropertyService.Get<string>("LastLoginStaffId", string.Empty);
                if (!(string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2)))
                {
                    this.txtUser.EditValue = OGMService.GetStaffDept(str, str2);
                }
                this.marqueeProgressBarControl1.Hide();
            }
            else
            {
                this.marqueeProgressBarControl1.Hide();
            }
            this.txtUser.Properties.QueryCloseUp += new CancelEventHandler(this.txtUser_QueryCloseUp);
            this.txtUser.Properties.QueryPopUp += new CancelEventHandler(this.txtUser_QueryPopUp);
            LoggingService.Debug("登录窗口初始化完成...");
        }

        private void LoadOGM(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new EventHandler(this.LoadOGM), new object[] { sender, e });
            }
            else
            {
                if (!(this.txtUser.EditValue is CStaffDept))
                {
                    string deptId = PropertyService.Get<string>("LastLoginDeptId", string.Empty);
                    string staffId = PropertyService.Get<string>("LastLoginStaffId", string.Empty);
                    this.txtUser.Properties.NullText = string.Empty;
                    if ((deptId != string.Empty) && (staffId != string.Empty))
                    {
                        CStaffDept dept = this.ogmTv.SelectStaffInDept(deptId, staffId) as CStaffDept;
                        if (dept != null)
                        {
                            this.txtUser.EditValue = dept;
                        }
                    }
                    else
                    {
                        this.txtUser.EditValue = null;
                    }
                }
                this.txtUser.Enabled = true;
                this.btOk.Enabled = true;
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("载入完成...");
                }
            }
        }

        private void LoadOGMTreeView()
        {
        }

        private void ogmTv_DoubleClick(object sender, EventArgs e)
        {
            if (this.ogmTv.QueryValue() is CStaffDept)
            {
                this.txtUser.ClosePopup();
            }
        }

        private void ogmTv_HandleCreated(object sender, EventArgs e)
        {
            this.marqueeProgressBarControl1.Show();
            this.btOk.Enabled = false;            
            this.backgroundWorker.DoWork += delegate(object sender1, DoWorkEventArgs e1)
            {
                this.ogmTv.InitOGMTree();
            };
            this.backgroundWorker.RunWorkerAsync();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate(object sender1, DoWorkEventArgs e1)
            {
                try
                {
                    LoggingService.InfoFormatted("开始异步载入人员部门缓存", new object[0]);
                    new SkyMap.Net.Security.DAOCache().Put();
                    LoggingService.InfoFormatted("异步载入人员部门缓存完成", new object[0]);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            };
            worker.RunWorkerAsync();
        }

        private void txtUser_QueryCloseUp(object sender, CancelEventArgs e)
        {
            object obj2 = this.ogmTv.QueryValue();
            string name = string.Empty;
            if ((obj2 == null) || (obj2 is CDept))
            {
                name = "Dialog.UserLogin.Message.MustSelectUser";
            }
            if (name != string.Empty)
            {
                MessageHelper.ShowInfo(ResourceService.GetString(name));
                e.Cancel = true;
            }
            else
            {
                CStaffDept dept = obj2 as CStaffDept;
                this.txtUser.Visible = false;
                this.txtUser.EditValue = dept;
                this.txtUser.Visible = true;
            }
        }

        private void txtUser_QueryPopUp(object sender, CancelEventArgs e)
        {
        }

        private void pClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pClose_MouseHover(object sender, EventArgs e)
        {
            this.pClose.Image = global::Properties.Resources.关闭1;
        }

        private void pClose_MouseLeave(object sender, EventArgs e)
        {
            this.pClose.Image = global::Properties.Resources.关闭;
        }
    }
}

