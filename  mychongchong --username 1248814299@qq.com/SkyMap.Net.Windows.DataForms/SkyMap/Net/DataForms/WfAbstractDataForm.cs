namespace SkyMap.Net.DataForms
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraTab;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.WebCamLibrary;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class WfAbstractDataForm : AbstractDataForm, IWfDataForm, IDataForm
    {
        private IContainer components = null;
        protected XtraTabControl tblData;
        protected IWfView wfView;

        public WfAbstractDataForm()
        {
            this.InitializeComponent();
        }

        private Uri BuildPhotoHtml(string strName)
        {
            string[] strArray = new string[0];
            if (!string.IsNullOrEmpty(strName))
            {
                strArray = strName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("<html>\r\n<head>\r\n<meta http-equiv='refresh' content='15'> \r\n    <title>ImageView</title>\r\n    <style type='text/css'>\r\n        img\r\n        {\r\n            width: 100%;\r\n            border-width:0pt;\r\n        }\r\n    </style>\r\n</head>\r\n<body leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'>");
            FtpSetting cameraInstance = FtpSetting.CameraInstance;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(strArray[i]))
                {
                    builder.AppendFormat("<img src='http://{0}/{1}/{2}/{3}.jpg' alt='{3}常未拍照'/><br/><div style='font-size:9pt;color: rgb(255, 51, 153);'>如果已拍照但未看到照片或未看到最新照片，请等待，照片下载与重新获取需要时间</div>", new object[] { cameraInstance.Host, cameraInstance.Path, this.wfView.WorkItem.ProjectId, strArray[i] });
                }
            }
            builder.AppendFormat("</body></html>", new object[0]);
            string path = Path.GetTempFileName() + ".htm";
            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
            return new Uri(path);
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
            this.tblData = new XtraTabControl();
            this.tblData.BeginInit();
            base.SuspendLayout();
            this.tblData.Location = new Point(8, 8);
            this.tblData.Name = "tblData";
            this.tblData.TabIndex = 0;
            this.tblData.Text = "xtraTabControl1";
            base.Controls.Add(this.tblData);
            base.Name = "WfAbstractDataForm";
            base.Size = new Size(0x178, 320);
            this.tblData.EndInit();
            base.ResumeLayout(false);
        }

        protected virtual void Qm(BaseEdit qmEdit, BaseEdit rqEdit)
        {
            qmEdit.EditValue = base.DataFormConntroller.GetParamValue("StaffName", string.Empty);
            rqEdit.EditValue = DateTimeHelper.GetNow();
            qmEdit.DoValidate();
            rqEdit.DoValidate();
            base.OnChanged(this, null);
        }

        protected void ResizeMe()
        {
            try
            {
                if (base.Controls.Contains(this.tblData))
                {
                    TableLayoutPanel panel = null;
                    for (int i = 0; i < this.tblData.TabPages.Count; i++)
                    {
                        XtraTabPage page = this.tblData.TabPages[i];
                        if (page.HasChildren && (page.Controls[0] is TableLayoutPanel))
                        {
                            this.tblData.Dock = DockStyle.None;
                            TableLayoutPanel panel2 = page.Controls[0] as TableLayoutPanel;
                            panel2.Dock = DockStyle.None;
                            panel2.AutoSize = true;
                            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                            for (int j = 0; j < panel2.RowCount; j++)
                            {
                                panel2.RowStyles[j].SizeType = SizeType.AutoSize;
                            }
                            if ((panel == null) || (panel2.Height > panel.Height))
                            {
                                panel = panel2;
                            }
                        }
                    }
                    if (panel != null)
                    {
                        this.tblData.ClientSize = new Size(this.tblData.ClientSize.Width, (this.tblData.ClientSize.Height - panel.Parent.Height) + panel.Height);
                        base.ClientSize = this.tblData.Size;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        protected override bool SelfBind(Control control, DataTable dataSource, string memberName, string[] valueCollection)
        {
            EventHandler handler = null;
            if (control.Name.StartsWith("imgwb_"))
            {
                if ((valueCollection.Length == 1) && (control is WebBrowser))
                {
                    Control control2 = this.FindControl(valueCollection[0]);
                    if (control2 != null)
                    {
                        WebBrowser browser = control as WebBrowser;
                        browser.Tag = control2.Text;
                        browser.Navigate(this.BuildPhotoHtml(control2.Text));
                        control2.Tag = browser;
                        if (handler == null)
                        {
                            handler = delegate (object sender, EventArgs e) {
                                Control control1 = sender as Control;
                                if (control1.Tag is WebBrowser)
                                {
                                    WebBrowser tag = control1.Tag as WebBrowser;
                                    if (!control1.Text.Equals(tag.Tag))
                                    {
                                        tag.Tag = control1.Text;
                                        tag.Navigate(this.BuildPhotoHtml(control1.Text));
                                    }
                                }
                            };
                        }
                        control2.TextChanged += handler;
                    }
                    else
                    {
                        LoggingService.WarnFormatted("不能找到显示照片对应的名称控件：{0} ", new object[] { valueCollection[0] });
                    }
                }
                else
                {
                    LoggingService.WarnFormatted("拍照按钮：{0}的值列表配置的错误它有且只能有1个参数或者控件类型错误，它只能是WebBrowser控件", new object[] { control.Name });
                }
                return true;
            }
            if (control is IEditView)
            {
                ((IEditView) control).LoadData(this.wfView.CurrentUnitOfWork, this.wfView.WorkItem.ProinstId, this.wfView.WorkItem.ProdefId, this.wfView.WorkItem.AssignId, this.ProjectSubTypes);
                return true;
            }
            return base.SelfBind(control, dataSource, memberName, valueCollection);
        }

        protected override void SelfButtonClick(object sender, EventArgs e)
        {
            Control control = sender as Control;
            string[] tag = (string[]) control.Tag;
            if (tag != null)
            {
                if (control.Name.StartsWith("qm_"))
                {
                    if (tag.Length == 3)
                    {
                        Control control2 = this.FindControl(tag[0]);
                        Control control3 = this.FindControl(tag[1]);
                        if ((control2 != null) && (control3 != null))
                        {
                            base.SetControlBindValue(control2, base.DataFormConntroller.GetParamValue("StaffName", string.Empty));
                            base.SetControlBindValue(control3, DateTimeHelper.GetNow());
                            if ((tag[2] == "1") && (this.wfView != null))
                            {
                                try
                                {
                                    this.wfView.Save();
                                }
                                catch (Exception exception)
                                {
                                    MessageHelper.ShowError("数据保存不成功!\r\n请关闭本窗口，重新打开业务后使用【载入最近未保存成功数据】尝试再次保存本次编辑的数据", exception);
                                    return;
                                }
                                this.wfView.Pass();
                            }
                        }
                        else
                        {
                            LoggingService.WarnFormatted("签名按钮：{0}的找不对对应的姓名控件：{0} 或日期控件：{1} ", new object[] { tag[0], tag[1] });
                        }
                    }
                    else
                    {
                        LoggingService.WarnFormatted("签名按钮：{0}的值列表配置有问题...", new object[] { control.Name });
                    }
                    return;
                }
                if (control.Name.StartsWith("tp_"))
                {
                    if (tag.Length == 1)
                    {
                        ToolBarCommand command = (ToolBarCommand) AddInTree.BuildItem(tag[0], this.wfView);
                        if (command != null)
                        {
                            command.AutoClick();
                        }
                        else
                        {
                            LoggingService.WarnFormatted("不能根据配置的路径：{0} 创建拍照ToolbarCommand", new object[] { tag[0] });
                        }
                    }
                    else
                    {
                        LoggingService.WarnFormatted("拍照按钮：{0}的值列表配置的错误：它有且只能有1个参数", new object[] { control.Name });
                    }
                    return;
                }
                if (control.Name.StartsWith("print_"))
                {
                    control.Tag = tag;
                    if (tag.Length == 1)
                    {
                        TempletPrint templetPrint = QueryHelper.Get<TempletPrint>("TempletPrint_" + tag[0], tag[0]);
                        if (templetPrint != null)
                        {
                            if (this.wfView != null)
                            {
                                this.wfView.Save();
                            }
                            else
                            {
                                this.Save();
                            }
                            this.Print(templetPrint, false);
                        }
                    }
                    else
                    {
                        LoggingService.WarnFormatted("打印按钮：{0}的值列表配置的报表ID错误：它有且只能有一个...", new object[] { control.Name });
                    }
                    return;
                }
            }
            base.SelfButtonClick(sender, e);
        }

        protected virtual void SetControlDisable(Control ctr)
        {
            System.Type type = ctr.GetType();
            if (ctr.HasChildren && SkyMap.Net.DataForms.SupportClass.IsNeedFindChild(type))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("将控件 {0}-{1} ({2})设置为不可编辑", new object[] { ctr.Name, ctr.Text, type.FullName });
                }
                if (ctr is ToolStrip)
                {
                    ((ToolStrip) ctr).Enabled = false;
                    return;
                }
                if (!SkyMap.Net.DataForms.SupportClass.ContainDataBindPropertyKey(type) || string.IsNullOrEmpty(SkyMap.Net.DataForms.SupportClass.GetPropertyName(type)))
                {
                    foreach (Control control in ctr.Controls)
                    {
                        this.SetControlDisable(control);
                    }
                    return;
                }
            }
            LoggingService.DebugFormatted("将设置控件：{0}-{1}({2})不能编辑", new object[] { ctr.Name, ctr.Text, type.FullName });
            if (ctr.GetType() == typeof(SmGridControl))
            {
                SmGridControl control2 = ctr as SmGridControl;
                control2.UseEmbeddedNavigator = false;
                if (control2.MainView != null)
                {
                    (control2.MainView as ColumnView).OptionsBehavior.Editable = false;
                }
                (ctr as SmGridControl).FocusedViewChanged += new ViewFocusEventHandler(this.WfAbstractDataForm_FocusedViewChanged);
            }
            else if (ctr is BaseEdit)
            {
                ((BaseEdit) ctr).Properties.ReadOnly = true;
                ((BaseEdit) ctr).BackColor = Color.FromArgb(0xf4, 0xf4, 0xf4);
            }
            else if (ctr is TextBox)
            {
                ((TextBox) ctr).ReadOnly = true;
                ((TextBox) ctr).BackColor = Color.FromArgb(0xf4, 0xf4, 0xf4);
            }
            else if (!(ctr is Label))
            {
                ctr.Enabled = false;
                try
                {
                    ctr.BackColor = Color.FromArgb(0xf4, 0xf4, 0xf4);
                    ctr.ForeColor = Color.Black;
                }
                catch
                {
                }
            }
        }

        public override void SetFormPermission(FormPermission formPermission, DAODataForm ddf, bool saveEnable)
        {
            base.SetFormPermission(formPermission, ddf, saveEnable);
            try
            {
                if (base.Controls.Contains(this.tblData))
                {
                    this.tblData.SelectedTabPageIndex = formPermission.PageIndex;
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            if (saveEnable)
            {
                Control control;
                if (!StringHelper.IsNull(formPermission.UnableFrame))
                {
                    string[] strArray = StringHelper.Split(formPermission.UnableFrame);
                    foreach (string str in strArray)
                    {
                        control = this.FindControl(str);
                        if (control != null)
                        {
                            this.SetControlDisable(control);
                        }
                    }
                }
                if (!StringHelper.IsNull(formPermission.InVisibleFrame))
                {
                    string[] strArray2 = StringHelper.Split(formPermission.InVisibleFrame, ',');
                    foreach (string str in strArray2)
                    {
                        control = this.FindControl(str);
                        if (control != null)
                        {
                            if (control.GetType().FullName == "DevExpress.XtraTab.XtraTabPage")
                            {
                                ((XtraTabPage) control).PageVisible = false;
                            }
                            else
                            {
                                control.Visible = false;
                            }
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("set {0} invisible", new object[] { control.Name });
                            }
                        }
                    }
                    this.ResizeMe();
                }
            }
            else if (base.Controls.Contains(this.tblData))
            {
                foreach (XtraTabPage page in this.tblData.TabPages)
                {
                    foreach (Control control2 in page.Controls)
                    {
                        control2.Enabled = false;
                    }
                }
            }
        }

        private void WfAbstractDataForm_FocusedViewChanged(object sender, ViewFocusEventArgs e)
        {
            if ((e.View != null) && (e.View is ColumnView))
            {
                ((ColumnView) e.View).OptionsBehavior.Editable = false;
            }
        }

        public IWfView WfView
        {
            set
            {
                this.wfView = value;
            }
        }
    }
}

