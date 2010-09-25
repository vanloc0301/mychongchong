namespace SkyMap.Net.DataForms
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(SmCommonDiction), "SkyMap.Net.DataForms.SmCommonDiction.png")]
    public class SmCommonDiction : SmUserControl
    {
        private IList<CommonDiction> _currentCDs;
        private SimpleButton btAdd;
        private IContainer components;
        private UnitOfWork currentUnitOfWork;
        private LabelControl labelControl1;
        private string path;
        private ComboBoxEdit pcEdit;
        private SimpleButton simpleButton1;
        private Control targetControl;

        public event SkyMap.Net.DataForms.CommonDictionValueChange CommonDictionValueChange;

        public SmCommonDiction()
        {
            this.InitializeComponent();
            this.pcEdit.Click += new EventHandler(this.pcEdit_Click);
            this.pcEdit.LostFocus += new EventHandler(this.pcEdit_SelectedIndexChanged);
        }

        private void Add(string name, string content)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "常用语标题不能为空");
            }
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content", "常用内容不能为空");
            }
            CommonDiction diction = new CommonDiction();
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            diction.Id = StringHelper.GetNewGuid();
            if (smIdentity.AdminLevel == AdminLevelType.NotAdmin)
            {
                diction.StaffId = smIdentity.UserId;
                diction.StaffName = smIdentity.UserName;
                diction.Path = this.Path;
            }
            diction.Name = name;
            diction.Description = content;
            this.CurrentUnitOfWork.RegisterNew(diction);
            this.currentUnitOfWork.Commit();
            this.pcEdit.Properties.Items.Add(diction);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if ((this.targetControl != null) && !string.IsNullOrEmpty(this.targetControl.Text))
            {
                InputBox box = new InputBox(string.Format("请输入常用语:{0}\r\n的易记性标题:", this.targetControl.Text), "请输入常用语标题", (this.targetControl.Text.Length > 10) ? this.targetControl.Text.Substring(0, 10) : this.targetControl.Text);
                if (box.ShowDialog(base.FindForm()) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(box.Result))
                    {
                        this.Add(box.Result, this.targetControl.Text);
                    }
                    else
                    {
                        MessageHelper.ShowInfo("常用语标题没有输入!");
                    }
                }
                box.Close();
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
            this.labelControl1 = new LabelControl();
            this.btAdd = new SimpleButton();
            this.pcEdit = new ComboBoxEdit();
            this.simpleButton1 = new SimpleButton();
            this.pcEdit.Properties.BeginInit();
            base.SuspendLayout();
            this.labelControl1.Appearance.ForeColor = Color.Black;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new Point(0x2a, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new Size(0x24, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "常用语";
            this.btAdd.Location = new Point(3, 2);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new Size(0x27, 0x17);
            this.btAdd.TabIndex = 2;
            this.btAdd.Text = "添加";
            this.btAdd.ToolTip = "将当前意见添加到常用语";
            this.btAdd.Click += new EventHandler(this.btAdd_Click);
            this.pcEdit.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pcEdit.Location = new Point(0x54, 3);
            this.pcEdit.Name = "pcEdit";
            this.pcEdit.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.pcEdit.Size = new Size(0x109, 0x15);
            this.pcEdit.TabIndex = 3;
            this.simpleButton1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.simpleButton1.Location = new Point(0x15c, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new Size(0x27, 0x17);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "删除";
            this.simpleButton1.ToolTip = "将当前意见添加到常用语";
            this.simpleButton1.Click += new EventHandler(this.simpleButton1_Click);
            base.Controls.Add(this.simpleButton1);
            base.Controls.Add(this.pcEdit);
            base.Controls.Add(this.btAdd);
            base.Controls.Add(this.labelControl1);
            base.Name = "SmCommonDiction";
            base.Size = new Size(390, 0x18);
            this.pcEdit.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void pcEdit_Click(object sender, EventArgs e)
        {
            if (this._currentCDs == null)
            {
                this.pcEdit.Visible = false;
                this.pcEdit.Properties.Items.AddRange(new List<CommonDiction>(this.CurrentCDs));
                this.pcEdit.Visible = true;
            }
        }

        private void pcEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonDiction selectedItem = this.pcEdit.SelectedItem as CommonDiction;
            if (selectedItem != null)
            {
                string description = selectedItem.Description;
                if (!((this.targetControl == null) || this.targetControl.Text.EndsWith(description)))
                {
                    this.targetControl.Text = this.targetControl.Text + description;
                }
                if (this.CommonDictionValueChange != null)
                {
                    this.CommonDictionValueChange(description);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            CommonDiction selectedItem = this.pcEdit.SelectedItem as CommonDiction;
            if ((selectedItem != null) && (MessageHelper.ShowOkCancelInfo("你真的要删除除常用语:{0}?", selectedItem.Name) == DialogResult.OK))
            {
                this.CurrentUnitOfWork.RegisterRemoved(selectedItem);
                this.currentUnitOfWork.Commit();
                this.pcEdit.Properties.Items.Remove(selectedItem);
                this.pcEdit.SelectedItem = null;
            }
        }

        private IList<CommonDiction> CurrentCDs
        {
            get
            {
                if (this._currentCDs == null)
                {
                    SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                    this._currentCDs = QueryHelper.List<CommonDiction>(typeof(CommonDiction).Namespace, string.Empty, "GetCommonDictionsByPathAndStaff", new string[] { this.Path, smIdentity.UserId });
                }
                return this._currentCDs;
            }
        }

        private UnitOfWork CurrentUnitOfWork
        {
            get
            {
                if (this.currentUnitOfWork == null)
                {
                    this.currentUnitOfWork = new UnitOfWork(typeof(CommonDiction));
                }
                return this.currentUnitOfWork;
            }
        }

        private string Path
        {
            get
            {
                if (string.IsNullOrEmpty(this.path))
                {
                    return "CommonPath";
                }
                return this.path;
            }
            set
            {
                if (this.path != value)
                {
                    this.path = value;
                }
            }
        }

        public Control TargetControl
        {
            set
            {
                this.targetControl = value;
            }
        }
    }
}

