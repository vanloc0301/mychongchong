namespace SkyMap.Net.DataForms
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Security;
    using SkyMap.Net.Util;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Windows.Forms;

    [ComVisible(true), PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    public class AbstractDataForm : SmUserControl, IDataForm
    {
        private string appearancexmlFile = FileUtility.Combine(new string[] { PropertyService.ConfigDirectory, "GridView.Appearances.xml" });
        private IContainer components;
        protected IDataEngine dataEngine;
        protected DataFormController dataFormController;
        private Dictionary<string, Control> dcs;
        private Dictionary<string, GridControl> grids = new Dictionary<string, GridControl>();
        private bool isBindData = true;
        protected Font resetFont = new Font("Tahoma", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        private DevExpress.XtraEditors.StyleController styleController;
        protected ToolTipController toolTipController;
        private Dictionary<string, WebBrowser> wbs = new Dictionary<string, WebBrowser>();

        public event EventHandler Changed;

        public event DataFormCallBackEventHandler DataFormCallBack;

        public AbstractDataForm()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected virtual void AddCustomReportSqls(Dictionary<string, string> dtsqls)
        {
        }

        protected virtual void AddDataControls(Control container)
        {
            if (this.dcs == null)
            {
                this.dcs = new Dictionary<string, Control>();
            }
            if (this.IsSelfBindControl(container))
            {
                if (this.IsResetFont)
                {
                    this.FontReset(container, false);
                }
                if (!this.IsExistSameNameControl(container))
                {
                    this.dcs.Add(container.Name, container);
                }
            }
            else if (container is IEditView)
            {
                if (this.IsResetFont)
                {
                    this.FontReset(container, true);
                }
                if (!this.IsExistSameNameControl(container))
                {
                    this.dcs.Add(container.Name, container);
                }
            }
            else
            {
                if (this.IsResetFont)
                {
                    this.FontReset(container, false);
                }
                System.Type type = container.GetType();
                if (SkyMap.Net.DataForms.SupportClass.ContainDataBindPropertyKey(type))
                {
                    if (this.dcs == null)
                    {
                        this.dcs = new Dictionary<string, Control>();
                    }
                    if (!this.IsExistSameNameControl(container))
                    {
                        this.dcs.Add(container.Name, container);
                    }
                    if (SkyMap.Net.DataForms.SupportClass.GetPropertyName(type).Trim().Length > 0)
                    {
                        return;
                    }
                }
                if (container.HasChildren && SkyMap.Net.DataForms.SupportClass.IsNeedFindChild(type))
                {
                    if (!(!(container.Parent is TableLayoutPanel) || this.IsExistSameNameControl(container)))
                    {
                        this.dcs.Add(container.Name, container);
                    }
                    foreach (Control control in container.Controls)
                    {
                        this.AddDataControls(control);
                    }
                }
            }
        }

        public void AddParams(string key, object val)
        {
            this.DataFormConntroller.AddParams(key, val);
        }

        protected virtual void AfterBindData()
        {
            this.OnBindingContextChanged(EventArgs.Empty);
        }

        protected virtual void AfterSave()
        {
        }

        private void BackgroundDisplayTooltip(object state)
        {
            if (state is ToolTipControllerShowEventArgs)
            {
                ToolTipControllerShowEventArgs args = state as ToolTipControllerShowEventArgs;
                try
                {
                    Control selectedControl = args.SelectedControl;
                    if (selectedControl.DataBindings.Count > 0)
                    {
                        Binding binding = selectedControl.DataBindings[0];
                        if ((binding.BindingManagerBase != null) && (binding.BindingManagerBase.Current != null))
                        {
                            DataTable dataSource = binding.DataSource as DataTable;
                            string tableName = dataSource.TableName;
                            string bindingField = binding.BindingMemberInfo.BindingField;
                            string keyValue = (binding.BindingManagerBase.Current as DataRowView)[dataSource.PrimaryKey[0].ColumnName].ToString();
                            args.IconType = ToolTipIconType.Information;
                            args.ToolTip = this.DataFormConntroller.GetTraceHistory(tableName, bindingField, keyValue);
                            args.Rounded = true;
                            args.ShowBeak = true;
                            args.Show = args.ToolTip.Length > 0;
                        }
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    args.ToolTip = string.Empty;
                }
            }
        }

        protected virtual void BeforeBindData()
        {
        }

        protected virtual void BeforeEndEdit()
        {
        }

        protected virtual void BeforeSave()
        {
        }

        protected void BeginBindData()
        {
            if (base.IsDisposed)
            {
                LoggingService.Warn("数据表单已关闭，不能进行绑定操作的");
            }
            else
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.Debug("执行数据绑定前任务...");
                }
                this.BeforeBindData();
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.Debug("要绑定数据了...");
                }
                this.DataFormConntroller.BindData(this);
                this.OnBindingContextChanged(null);
                this.AfterBindData();
                this.isBindData = true;
            }
        }

        public void BindDataToControl(string controlName, DataTable dataSource, string memberName, string[] valueCollection)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将尝试绑定控件：{0},表：{1}，列：{2}", new object[] { controlName, (dataSource != null) ? dataSource.TableName : string.Empty, memberName });
            }
            Control control = this.FindControl(controlName);
            if ((control != null) && !this.SelfBind(control, dataSource, memberName, valueCollection))
            {
                if (dataSource == null)
                {
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("控件:{0}绑定设置可能有问题，数据表设置为空", new object[] { control.Name });
                    }
                }
                else if (!control.GetType().Equals(typeof(WebBrowser)))
                {
                    control.MouseUp -= new MouseEventHandler(this.DataControlMouseUp);
                    control.KeyUp -= new KeyEventHandler(this.DataControlKeyUp);
                    string propertyName = SkyMap.Net.DataForms.SupportClass.GetPropertyName(control.GetType());
                    if ((memberName != null) && (memberName.Length > 0))
                    {
                        if (valueCollection != null)
                        {
                            if (control is ComboBoxEdit)
                            {
                                ComboBoxEdit edit = control as ComboBoxEdit;
                                edit.Properties.Items.Clear();
                                edit.Properties.Items.AddRange(valueCollection);
                            }
                            else if (control is CheckedComboBoxEdit)
                            {
                                CheckedComboBoxEdit edit2 = control as CheckedComboBoxEdit;
                                edit2.Properties.Items.Clear();
                                foreach (string str2 in valueCollection)
                                {
                                    edit2.Properties.Items.Add(str2, false);
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(propertyName))
                        {
                            LoggingService.WarnFormatted("没有设定控件:{0} 类型:{1} 的数据据绑定属性", new object[] { control.Name, control.GetType().FullName });
                            return;
                        }
                        if (!string.IsNullOrEmpty(memberName) && dataSource.Columns.Contains(memberName))
                        {
                            control.DataBindings.Clear();
                            control.DataBindings.Add(propertyName, dataSource, memberName).Parse += new ConvertEventHandler(this.OnChanged);
                            if (control is BaseEdit)
                            {
                                (control as BaseEdit).ToolTipController = this.toolTipController;
                                (control as BaseEdit).ToolTip = "test";
                            }
                            else
                            {
                                this.toolTipController.SetToolTip(control, "test");
                            }
                        }
                        else
                        {
                            LoggingService.WarnFormatted("在表：{0}中没有找到控件：{1} 绑定所需要的列：{2}", new object[] { dataSource.TableName, control.Name, memberName });
                        }
                    }
                    else
                    {
                        PropertyInfo bindPropertyInfo = SkyMap.Net.DataForms.SupportClass.GetBindPropertyInfo(control.GetType(), propertyName);
                        if (bindPropertyInfo != null)
                        {
                            DataView defaultView = dataSource.DefaultView;
                            if (bindPropertyInfo.PropertyType.IsAssignableFrom(typeof(DataView)))
                            {
                                bindPropertyInfo.SetValue(control, defaultView, null);
                                defaultView.ListChanged += new ListChangedEventHandler(this.DataViewListChanged);
                                if (!this.grids.ContainsKey(control.Name) && (control is GridControl))
                                {
                                    GridControl control2 = (GridControl) control;
                                    this.grids.Add(control.Name, control2);
                                    if ((dataSource.Rows.Count > 0) && (control2.MainView is GridView))
                                    {
                                        ((GridView) control2.MainView).BestFitColumns();
                                    }
                                }
                            }
                            else
                            {
                                LoggingService.WarnFormatted("控件:{0}的属性：{1},不能绑定表：{2}", new object[] { control.Name, propertyName, dataSource.TableName });
                            }
                        }
                    }
                    control.MouseUp += new MouseEventHandler(this.DataControlMouseUp);
                    control.KeyUp += new KeyEventHandler(this.DataControlKeyUp);
                    control.TextChanged += new EventHandler(this.DataControlTextChanged);
                }
                else
                {
                    WebBrowser browser = (WebBrowser) control;
                    browser.AllowWebBrowserDrop = false;
                    browser.IsWebBrowserContextMenuEnabled = false;
                    browser.ObjectForScripting = this;
                    if (!browser.Name.StartsWith("html"))
                    {
                        throw new ApplicationException(string.Format("绑定的WebBrowser必须是命名必须是以html开头,当前命名:{0}不正确", browser.Name));
                    }
                    string uriString = FileUtility.Combine(new string[] { PropertyService.DataDirectory, "Resources", "DataForms", base.Name, browser.Name.Substring(4) + ".htm" });
                    browser.Url = new Uri(uriString);
                    LoggingService.InfoFormatted("将载入HTML文件{0}并绑定数据值...", new object[] { uriString });
                    browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.WB_DocumentCopmpleted);
                    if (!this.wbs.ContainsKey(browser.Name))
                    {
                        this.wbs.Add(browser.Name, browser);
                    }
                }
            }
        }

        private void CopyUserInfo(string[] dknames, HtmlDocument doc, string newvalue)
        {
            foreach (string str in dknames)
            {
                HtmlElement elementById = doc.GetElementById(str);
                if (elementById != null)
                {
                    string str2 = string.IsNullOrEmpty(elementById.GetAttribute("value")) ? newvalue : (elementById.GetAttribute("value") + @"\" + newvalue);
                    this.SetTableRowValue(elementById.GetAttribute("dbtable"), elementById.GetAttribute("dbcolumn"), 0, str2, new GetTableRowValueString(this.GetHtmlElementValue), elementById);
                    this.SetElseWebBrowseHtmlElementValue(doc, elementById.Id, str2);
                }
                else
                {
                    LoggingService.WarnFormatted("找不到控件：{0}", new object[] { str });
                }
            }
        }

        protected virtual void CustomReportViewDialog(ReportViewDialog rptDlg)
        {
        }

        private void DataControlKeyUp(object sender, KeyEventArgs e)
        {
            this.OnChanged(sender, null);
        }

        private void DataControlMouseUp(object sender, MouseEventArgs e)
        {
            if (SkyMap.Net.DataForms.SupportClass.GetPropertyName(sender.GetType()) != null)
            {
                if (((Control) sender).DataBindings[SkyMap.Net.DataForms.SupportClass.GetPropertyName(sender.GetType())] != null)
                {
                    ((Control) sender).DataBindings[SkyMap.Net.DataForms.SupportClass.GetPropertyName(sender.GetType())].BindingManagerBase.EndCurrentEdit();
                }
                if (sender is BaseEdit)
                {
                    BaseEdit edit = sender as BaseEdit;
                    if (edit.IsModified)
                    {
                        edit.DoValidate();
                    }
                }
                else
                {
                    this.OnChanged(sender, null);
                }
            }
        }

        protected void DataControlTextChanged(object sender, EventArgs e)
        {
        }

        protected void DataViewListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(this, e);
            }
        }

        protected virtual void DbHtmlElementLostFocus(object sender, HtmlElementEventArgs e)
        {
            LoggingService.InfoFormatted("事件类型:{0},源:{1},目的:{2}", new object[] { e.EventType, (e.FromElement == null) ? "无" : e.FromElement.Id, (e.ToElement == null) ? "无" : e.ToElement.Id });
            HtmlElement he = sender as HtmlElement;
            if (he != null)
            {
                if (((e.EventType == "focusout") || (e.EventType == "blur")) || (he.TagName.ToLower() == "select"))
                {
                    he.GetAttribute("");
                    string attribute = he.GetAttribute("dbtable");
                    string dbcolumn = he.GetAttribute("dbcolumn");
                    string str3 = this.TestHmtlEelementValue(he, attribute, dbcolumn, 0);
                    this.SetTableRowValue(attribute, dbcolumn, 0, str3, new GetTableRowValueString(this.GetHtmlElementValue), he);
                    WebBrowserExtensions.SetHtmlElementValue(he, str3);
                    this.SetElseWebBrowseHtmlElementValue(he.Document, he.Id, str3);
                }
                this.OnChanged(this, null);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("数据已被修改...！");
                }
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

        protected void EditValueValidating(object sender, CancelEventArgs e)
        {
            try
            {
                if (sender is BaseEdit)
                {
                    BaseEdit edit = sender as BaseEdit;
                    edit.Validating -= new CancelEventHandler(this.EditValueValidating);
                    if (edit.EditValue.ToString().Length == 0)
                    {
                        edit.EditValue = null;
                    }
                    edit.Validating += new CancelEventHandler(this.EditValueValidating);
                }
            }
            catch
            {
            }
        }

        public virtual bool ExistFailSavedFile()
        {
            return File.Exists(this.DataFormConntroller.GetFailSavedFile());
        }

        public virtual PrintSet FilterTempletPrints(PrintSet printSet)
        {
            return printSet;
        }

        protected virtual Control FindControl(string name)
        {
            this.LoadDataControls();
            if (this.dcs.ContainsKey(name))
            {
                return this.dcs[name];
            }
            return null;
        }

        protected virtual void FontReset(Control ctl, bool resetChildren)
        {
            if ((ctl is Label) || (ctl is SimpleButton))
            {
                ctl.Font = this.resetFont;
            }
            else if (ctl is BaseEdit)
            {
                (ctl as BaseControl).StyleController = this.StyleController;
            }
            else if ((ctl is SmGridControl) && File.Exists(this.appearancexmlFile))
            {
                SmGridControl control = ctl as SmGridControl;
                control.MainView.Appearance.RestoreLayoutFromXml(this.appearancexmlFile);
            }
            else if (resetChildren && ctl.HasChildren)
            {
                foreach (Control control2 in ctl.Controls)
                {
                    this.FontReset(control2, true);
                }
            }
        }

        protected virtual string FormatColumnValue(object value, System.Type type, string name)
        {
            if (Convert.IsDBNull(value))
            {
                return "";
            }
            if (type == typeof(DateTime))
            {
                DateTime time = (DateTime) value;
                return time.ToString("yyyy年MM月dd日");
            }
            return value.ToString();
        }

        protected object GetControlBindValue(Control control)
        {
            try
            {
                Binding binding = control.DataBindings[0];
                if (binding != null)
                {
                    return (binding.BindingManagerBase.Current as DataRowView)[binding.BindingMemberInfo.BindingField];
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            LoggingService.WarnFormatted("获取不了控件：{0} 数据绑定值", new object[] { control.Name });
            return null;
        }

        private Dictionary<string, DataControl> GetDataControlDict()
        {
            string key = "DICT_DAODataForm_DataControls" + this.dataFormController.DAODataForm.Id;
            Dictionary<string, DataControl> dictionary = (Dictionary<string, DataControl>) DAOCacheService.Get(key);
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, DataControl>();
                foreach (DataControl control in this.dataFormController.DAODataForm.DataControls)
                {
                    string str2 = control.Name.ToLower();
                    if (!dictionary.ContainsKey(str2))
                    {
                        dictionary.Add(str2, control);
                    }
                    else
                    {
                        LoggingService.WarnFormatted("存在同名控件配置:{0}", new object[] { str2 });
                    }
                }
                DAOCacheService.Put(key, dictionary);
            }
            return dictionary;
        }

        protected string GetHtmlElementValue(object sender, System.Type type, object rowcolumvalue)
        {
            string str = string.Empty;
            if (type == typeof(DateTime))
            {
                str = Convert.IsDBNull(rowcolumvalue) ? string.Empty : Convert.ToDateTime(rowcolumvalue).ToString("yyyy-MM-dd");
            }
            else
            {
                str = Convert.IsDBNull(rowcolumvalue) ? string.Empty : rowcolumvalue.ToString();
            }
            HtmlElement element = sender as HtmlElement;
            if ((element != null) && (element.TagName.ToLower() == "select"))
            {
                foreach (HtmlElement element2 in element.Children)
                {
                    if (element2.TagName.ToLower() == "option")
                    {
                        string attribute = element2.GetAttribute("value");
                        if (attribute == str)
                        {
                            element2.SetAttribute("selected", "true");
                            return str;
                        }
                        if (((((type.Equals(typeof(double)) || type.Equals(typeof(decimal))) || (type.Equals(typeof(float)) || type.Equals(typeof(float)))) || type.Equals(typeof(int))) || type.Equals(typeof(long))) && ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(attribute)) && double.Parse(str).Equals(double.Parse(attribute))))
                        {
                            element2.SetAttribute("selected", "true");
                            return attribute;
                        }
                    }
                }
            }
            return str;
        }

        protected virtual DataSet GetReportDataSource(TempletPrint templetPrint)
        {
            if (!string.IsNullOrEmpty(templetPrint.Sql))
            {
                return templetPrint.GetReportDataSource(true, this.DataFormConntroller.SqlParams);
            }
            return this.dataFormController.DataSource;
        }

        protected object GetTableColumnValue(string dbtable, string dbcolumn, int index)
        {
            DataSet dataSource = this.DataFormConntroller.DataSource;
            if (!dataSource.Tables.Contains(dbtable))
            {
                LoggingService.WarnFormatted("Table:{0},Column:{1} 关联的表没有找到...", new object[] { dbtable, dbcolumn });
                return string.Empty;
            }
            DataTable table = dataSource.Tables[dbtable];
            if (!table.Columns.Contains(dbcolumn))
            {
                LoggingService.WarnFormatted("Table:{0},Column:{1} 关联的表没有找到相应的列...", new object[] { dbtable, dbcolumn });
                return string.Empty;
            }
            return dataSource.Tables[dbtable].Rows[index][dbcolumn];
        }

        protected bool GetTableColumnValueFromHtmlDocument(HtmlDocument doc, string dbtable, string dbcolumn, ref object result)
        {
            foreach (string str in WebBrowserExtensions.tags)
            {
                HtmlElementCollection elementsByTagName = doc.GetElementsByTagName(str);
                foreach (HtmlElement element in elementsByTagName)
                {
                    string attribute = element.GetAttribute("dbtable");
                    string str3 = element.GetAttribute("dbcolumn");
                    if (((!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(dbcolumn)) && (attribute == dbtable)) && (str3 == dbcolumn))
                    {
                        result = element.GetAttribute("value");
                        return true;
                    }
                }
            }
            return false;
        }

        private void HtmlDKButtonClick(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement) sender;
            if (element != null)
            {
                string[] strArray = element.GetAttribute("dk").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length > 0)
                {
                    string[] dknames = strArray[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] strArray3 = null;
                    if (strArray.Length > 1)
                    {
                        strArray3 = strArray[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    if (dknames.Length > 0)
                    {
                        HtmlDocument doc = element.Document;
                        if (doc.GetElementById(dknames[0]) != null)
                        {
                            try
                            {
                                clsUserInfo info = clsTermb.ReadInfo();
                                this.CopyUserInfo(dknames, doc, info.m_strPeopleName);
                                if ((strArray3 != null) && (strArray3.Length > 0))
                                {
                                    this.CopyUserInfo(strArray3, doc, info.m_strPeopleIDCode);
                                    this.OnChanged(this, null);
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageHelper.ShowInfo(exception.Message);
                            }
                        }
                        else
                        {
                            LoggingService.WarnFormatted("找不到控件：{0}", new object[] { dknames[0] });
                        }
                    }
                }
            }
        }

        protected void HtmlHJButtonClick(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement) sender;
            if (element != null)
            {
                string attribute = element.GetAttribute("hj");
                if (string.IsNullOrEmpty(attribute))
                {
                    throw new ApplicationException(string.Format("找不到按钮{0}对应的税费合计设置,请检查hj属性是否设置正确...", element.Id));
                }
                WebBrowserExtensions.Hj(element.Document, attribute);
                this.OnChanged(this, null);
            }
        }

        protected void HtmlHQButtonClick(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement) sender;
            if (element != null)
            {
                if (string.IsNullOrEmpty(element.GetAttribute("hq")))
                {
                    throw new ApplicationException(string.Format("找不到按钮{0}对应的获取数据设置,请检查hq属性是否设置正确...", element.Id));
                }
                HtmlDocument doc = element.Document;
                foreach (WebBrowser browser in this.wbs.Values)
                {
                    if (browser.Document != doc)
                    {
                        WebBrowserExtensions.AttachValueToDataSource(this.DataFormConntroller.DataSource, browser.Document);
                    }
                }
                WebBrowserExtensions.GetData(doc, this.DataFormConntroller.DataSource, this.InvokeResults);
                this.OnChanged(this, null);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("获取数据修改！");
                }
            }
        }

        private void HtmlInheritDataButtonClick(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement) sender;
            if (element != null)
            {
                string[] strArray = element.GetAttribute("jc").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if ((strArray.Length <= 0) || ((strArray.Length % 2) != 0))
                {
                    throw new ApplicationException(string.Format("ID:{0} 的继承数据的配置:{1}有问题", element.Id, element.GetAttribute("jc")));
                }
                int index = 0;
                do
                {
                    HtmlDocument doc = element.Document;
                    HtmlElement elementById = doc.GetElementById(strArray[index + 1]);
                    string attribute = doc.GetElementById(strArray[index]).GetAttribute("value");
                    this.SetTableRowValue(elementById.GetAttribute("dbtable"), elementById.GetAttribute("dbcolumn"), 0, attribute, new GetTableRowValueString(this.GetHtmlElementValue), elementById);
                    this.SetElseWebBrowseHtmlElementValue(doc, elementById.Id, attribute);
                    index += 2;
                }
                while (index < strArray.Length);
                this.OnChanged(this, null);
            }
        }

        protected void HtmlJSButtonClick(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement) sender;
            if (element != null)
            {
                if (string.IsNullOrEmpty(element.GetAttribute("js")))
                {
                    throw new ApplicationException(string.Format("找不到签名按钮{0}对应的税费合计设置,请检查hj属性是否设置正确...", element.Id));
                }
                WebBrowserExtensions.Calc(element.Document, this.DataFormConntroller.DataSource, this.InvokeResults);
                this.OnChanged(this, null);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("税费计算修改！");
                }
            }
        }

        protected void HtmlQMButtonClick(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = (HtmlElement) sender;
            if (element != null)
            {
                string[] strArray = element.GetAttribute("qm").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length == 2)
                {
                    HtmlDocument document = element.Document;
                    HtmlElement elementById = document.GetElementById(strArray[0].Trim());
                    HtmlElement he = document.GetElementById(strArray[1].Trim());
                    if ((elementById != null) && (he != null))
                    {
                        string userName = SecurityUtil.GetSmIdentity().UserName;
                        string str2 = DateTimeHelper.GetNow().ToString("yyyy-MM-dd");
                        WebBrowserExtensions.SetHtmlElementValue(elementById, userName);
                        WebBrowserExtensions.SetHtmlElementValue(he, str2);
                        this.OnChanged(this, null);
                        return;
                    }
                }
                throw new ApplicationException(string.Format("找不到签名按钮{0}对应的:{1}签名控件,请检查qm属性是否设置正确...", element.Id, element.GetAttribute("qm")));
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.toolTipController = new ToolTipController(this.components);
            base.SuspendLayout();
            this.toolTipController.BeforeShow += new ToolTipControllerBeforeShowEventHandler(this.toolTipController_BeforeShow);
            this.AutoScroll = true;
            base.Name = "AbstractDataForm";
            base.Size = new Size(0x1d0, 0x180);
            this.toolTipController.SetSuperTip(this, null);
            base.ResumeLayout(false);
        }

        private bool IsExistSameNameControl(Control ctl)
        {
            if (!this.dcs.ContainsKey(ctl.Name))
            {
                return false;
            }
            if (ctl.Parent != null)
            {
                LoggingService.WarnFormatted("存在同名控件：{0},父控件分别是：{0} - {2}", new object[] { ctl.Name, ctl.Parent.Name, this.dcs[ctl.Name].Parent.Name });
            }
            return true;
        }

        protected virtual bool IsSelfBindControl(Control container)
        {
            return ((((container.Name.StartsWith("qm_") || container.Name.StartsWith("print_")) || (container.Name.StartsWith("tp_") || container.Name.StartsWith("imgwb_"))) || container.Name.StartsWith("dk_")) || container.Name.StartsWith("jc_"));
        }

        protected virtual void LoadDataControls()
        {
            if (this.dcs == null)
            {
                foreach (Control control in base.Controls)
                {
                    this.AddDataControls(control);
                }
            }
        }

        public virtual bool LoadMe()
        {
            this.BeforeEndEdit();
            base.Enabled = false;
            if (base.IsHandleCreated)
            {
                base.Visible = false;
                try
                {
                    this.BeginBindData();
                    if (!base.Enabled)
                    {
                        base.Enabled = true;
                    }
                }
                finally
                {
                    base.Visible = true;
                }
            }
            else
            {
                this.isBindData = false;
            }
            return true;
        }

        internal bool LoadMeSync()
        {
            base.Enabled = false;
            this.BeforeBindData();
            this.DataFormConntroller.BindData(this);
            this.OnBindingContextChanged(null);
            this.AfterBindData();
            base.Enabled = true;
            return true;
        }

        public void NotifySystemMessage(string message)
        {
            SystemHintHelper.Show(message);
        }

        protected void OnChanged(object sender, ConvertEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(this, e);
            }
        }

        protected virtual void OnCreateNewDataRow(DataRow row)
        {
        }

        protected void OnDataFormCallBack(object sender, DataFormCallBackEventArgs e)
        {
            this.DataFormCallBack(sender, e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            Action action2 = null;
            base.OnHandleCreated(e);
            if (!this.isBindData)
            {
                if (action2 == null)
                {
                    action2 = delegate {
                        base.Visible = false;
                        WaitDialogHelper.Show();
                        try
                        {
                            this.BeginBindData();
                            if (!base.Enabled)
                            {
                                base.Enabled = true;
                            }
                        }
                        finally
                        {
                            base.Visible = true;
                            WaitDialogHelper.Close();
                        }
                    };
                }
                Action method = action2;
                base.BeginInvoke(method);
            }
        }

        public virtual void PostEditor()
        {
            if (base.Enabled)
            {
                this.BeforeEndEdit();
                foreach (DictionaryEntry entry in (IEnumerable) this.BindingContext)
                {
                    ((entry.Value as WeakReference).Target as BindingManagerBase).EndCurrentEdit();
                }
            }
            foreach (GridControl control in this.grids.Values)
            {
                if ((control.EmbeddedNavigator != null) && (control.EmbeddedNavigator.Buttons.EndEdit != null))
                {
                    control.EmbeddedNavigator.Buttons.DoClick(control.EmbeddedNavigator.Buttons.EndEdit);
                }
            }
        }

        public virtual void Print(TempletPrint templetPrint, bool reBuild)
        {
            string str;
            DataSet set;
            if (!this.PrintBySelf(templetPrint))
            {
                str = templetPrint.Type.ToLower();
                string str2 = str;
                if (str2 == null)
                {
                    goto Label_0083;
                }
                if (!(str2 == ".doc"))
                {
                    if (str2 == ".xslt")
                    {
                        this.PrintXSLT(templetPrint, reBuild);
                        return;
                    }
                    if (str2 == ".rdlc")
                    {
                        this.PrintRDLC(templetPrint, reBuild);
                        return;
                    }
                    if (str2 == ".wzx")
                    {
                        this.PrintWordEx(templetPrint, reBuild);
                        return;
                    }
                    goto Label_0083;
                }
                this.PrintWord(templetPrint, reBuild);
            }
            return;
        Label_0083:
            set = this.GetReportDataSource(templetPrint);
            if (!PrintHelper.PrintByReportVistor(templetPrint.Type, set, templetPrint.Data, this))
            {
                throw new NotImplementedException("没有实现的报表打印方式:" + str);
            }
        }

        protected virtual bool PrintBySelf(TempletPrint templetPrint)
        {
            return false;
        }

        protected virtual bool PrintBySelf(string name, ref string bmText)
        {
            return false;
        }

        protected void PrintRDLC(TempletPrint templetPrint, bool reBuild)
        {
            WaitDialogHelper.Show();
            try
            {
                if ((templetPrint.Data != null) && (templetPrint.Data.Length > 0))
                {
                    DataSet reportDataSource = this.GetReportDataSource(templetPrint);
                    PrintHelper.PrintOrShowRDLC(templetPrint.Name, templetPrint.PrintPreview, templetPrint.Data, reportDataSource, null, null, new Action<ReportViewDialog>(this.CustomReportViewDialog));
                }
            }
            finally
            {
                WaitDialogHelper.Close();
            }
        }

        protected virtual void PrintWord(TempletPrint templetPrint, bool reBuild)
        {
            LoggingService.DebugFormatted("将输出打印WORD报表：{0}", new object[] { templetPrint.Name });
            PrintHelper.PrintWord(templetPrint.Data, this.GetReportDataSource(templetPrint), true, new SkyMap.Net.Util.PrintBySelf(this.PrintBySelf), new SkyMap.Net.Util.FormatColumnValue(this.FormatColumnValue), !templetPrint.PrintPreview);
        }

        protected virtual void PrintWord(TempletPrint templetPrint, string targetFile, bool reBuild)
        {
            LoggingService.DebugFormatted("将输出打印WORD报表：{0}至：{1}", new object[] { templetPrint.Name, targetFile });
            PrintHelper.PrintWord(targetFile, templetPrint.Data, this.GetReportDataSource(templetPrint), true, new SkyMap.Net.Util.PrintBySelf(this.PrintBySelf), new SkyMap.Net.Util.FormatColumnValue(this.FormatColumnValue), !templetPrint.PrintPreview);
        }

        protected virtual void PrintWordEx(TempletPrint templetPrint, bool reBuild)
        {
            LoggingService.DebugFormatted("将输出打印新WORD报表：{0}", new object[] { templetPrint.Name });
            PrintHelper.PrintWordEx(ZipHelper.GZDecompress(templetPrint.Data), this.GetReportDataSource(templetPrint), true, new SkyMap.Net.Util.PrintBySelf(this.PrintBySelf), new SkyMap.Net.Util.FormatColumnValue(this.FormatColumnValue), !templetPrint.PrintPreview);
        }

        protected virtual void PrintWordEx(TempletPrint templetPrint, string targetFile, bool reBuild)
        {
            LoggingService.DebugFormatted("将输出打印新WORD报表：{0}至：{1}", new object[] { templetPrint.Name, targetFile });
            PrintHelper.PrintWordEx(targetFile, ZipHelper.GZCompress(templetPrint.Data), this.GetReportDataSource(templetPrint), true, new SkyMap.Net.Util.PrintBySelf(this.PrintBySelf), new SkyMap.Net.Util.FormatColumnValue(this.FormatColumnValue), !templetPrint.PrintPreview);
        }

        private void PrintXSLT(TempletPrint templetPrint, bool reBuild)
        {
            LoggingService.DebugFormatted("将输出XSLT报表：{0}", new object[] { templetPrint.Name });
            PrintHelper.PrintXLST(this.DataFormConntroller.DataSource, templetPrint.Data);
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (this.Changed != null)
            {
                this.Changed(this, new EventArgs());
            }
            return base.ProcessKeyPreview(ref m);
        }

        public virtual void RestoreFailSavedDataSet()
        {
            if (this.ExistFailSavedFile())
            {
                this.DataFormConntroller.RestoreUnsavedData();
                this.LoadMeSync();
                this.Changed(this, new EventArgs());
            }
        }

        public virtual bool Save()
        {
            foreach (WebBrowser browser in this.wbs.Values)
            {
                WebBrowserExtensions.AttachValueToDataSource(this.DataFormConntroller.DataSource, browser.Document);
            }
            this.PostEditor();
            this.BeforeSave();
            this.DataFormConntroller.Save();
            this.AfterSave();
            this.LoadMeSync();
            return true;
        }

        protected virtual bool SelfBind(Control control, DataTable dataSource, string memberName, string[] valueCollection)
        {
            if (this.IsSelfBindControl(control))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("控件自定义绑定：{0},值集合：{1}", new object[] { control.Name, (valueCollection != null) ? string.Join(",", valueCollection) : string.Empty });
                }
                control.Tag = valueCollection;
                control.Click -= new EventHandler(this.SelfButtonClick);
                control.Click += new EventHandler(this.SelfButtonClick);
                return true;
            }
            return false;
        }

        protected virtual void SelfButtonClick(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将运行自定义事件：{0}，参数：{1}", new object[] { control.Name, control.Tag });
            }
            string[] tag = (string[]) control.Tag;
            if (tag != null)
            {
                if (control.Name.StartsWith("print_"))
                {
                    control.Tag = tag;
                    if (tag.Length == 1)
                    {
                        TempletPrint templetPrint = QueryHelper.Get<TempletPrint>("TempletPrint_" + tag[0], tag[0]);
                        if (templetPrint != null)
                        {
                            this.Save();
                            this.Print(templetPrint, false);
                        }
                    }
                    else
                    {
                        LoggingService.WarnFormatted("打印按钮：{0}的值列表配置的报表ID错误：它有且只能有一个...", new object[] { control.Name });
                    }
                }
                else if (control.Name.StartsWith("dk_"))
                {
                    if (tag.Length == 2)
                    {
                        try
                        {
                            Control control2 = this.FindControl(tag[0]);
                            Control control3 = this.FindControl(tag[1]);
                            if ((control2 != null) && (control3 != null))
                            {
                                clsUserInfo info = clsTermb.ReadInfo();
                                object controlBindValue = this.GetControlBindValue(control2);
                                object obj3 = this.GetControlBindValue(control3);
                                LoggingService.DebugFormatted(@"将设置身份证姓名控件：{0}值：{1}\{2}", new object[] { tag[0], controlBindValue, info.m_strPeopleName });
                                this.SetControlBindValue(control2, string.Format("{0}{1}", ((controlBindValue != null) && (controlBindValue.ToString().Length > 0)) ? (controlBindValue.ToString() + @"\") : string.Empty, info.m_strPeopleName));
                                this.SetControlBindValue(control3, string.Format("{0}{1}", ((obj3 != null) && (obj3.ToString().Length > 0)) ? (obj3.ToString() + @"\") : string.Empty, info.m_strPeopleIDCode));
                                control2.Focus();
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageHelper.ShowInfo(exception.Message);
                        }
                    }
                    else
                    {
                        LoggingService.WarnFormatted("身份证读卡按钮：{0}的值列表配置错误：格式应是：姓名控件,身份证号控件...", new object[] { control.Name });
                    }
                }
                else if (control.Name.StartsWith("jc_"))
                {
                    if ((tag.Length <= 0) || ((tag.Length % 2) != 0))
                    {
                        throw new ApplicationException(string.Format("ID:{0} 的继承数据的配置:{1}有问题", control.Name, string.Join(",", tag)));
                    }
                    int index = 0;
                    do
                    {
                        Control control4 = this.FindControl(tag[index + 1]);
                        if (control4 != null)
                        {
                            Control control5 = this.FindControl(tag[index]);
                            if (control5 != null)
                            {
                                object obj4 = this.GetControlBindValue(control5);
                                this.SetControlBindValue(control4, obj4);
                            }
                        }
                        index += 2;
                    }
                    while (index < tag.Length);
                    this.OnChanged(this, null);
                }
            }
        }

        protected void SetControlBindValue(Control control, object value)
        {
            Binding binding = control.DataBindings[0];
            if (binding != null)
            {
                DataRowView current = binding.BindingManagerBase.Current as DataRowView;
                if (current != null)
                {
                    if (current.IsEdit)
                    {
                        current.BeginEdit();
                    }
                    (binding.BindingManagerBase.Current as DataRowView)[binding.BindingMemberInfo.BindingField] = value;
                    binding.ReadValue();
                }
            }
        }

        public virtual void SetElse()
        {
        }

        private void SetElseWebBrowseHtmlElementValue(HtmlDocument doc, string heId, object value)
        {
            try
            {
                LoggingService.Info("查找有没有其它需要变值的。。。");
                foreach (WebBrowser browser in this.wbs.Values)
                {
                    if (browser.Document != doc)
                    {
                        HtmlElement elementById = browser.Document.GetElementById(heId);
                        if (elementById != null)
                        {
                            LoggingService.InfoFormatted("在其它HTML表单中找到相同ID控件{0}:{1}，如果置不是：{2}将设为相同值", new object[] { browser.Name, heId, value });
                            if (elementById.GetAttribute("value") != value.ToString())
                            {
                                WebBrowserExtensions.SetHtmlElementValue(elementById, value.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        private void SetFieldValue(string heId, string tableName, string columnName, object value)
        {
            DataTable table = this.dataFormController.DataSource.Tables[tableName];
            DataRow row = table.Rows[0];
            if (!value.Equals(row[columnName]))
            {
                try
                {
                    row[columnName] = value;
                }
                catch
                {
                    try
                    {
                        row[columnName] = Convert.DBNull;
                        value = string.Empty;
                    }
                    catch
                    {
                    }
                }
                if ((heId != null) && (heId.Length > 0))
                {
                    foreach (WebBrowser browser in this.wbs.Values)
                    {
                        HtmlElement elementById = browser.Document.GetElementById(heId);
                        if (elementById != null)
                        {
                            WebBrowserExtensions.SetHtmlElementValue(elementById, value.ToString());
                        }
                    }
                }
            }
        }

        public virtual void SetFormPermission(FormPermission formPermission, DAODataForm ddf, bool saveEnable)
        {
            this.DataFormConntroller.SetFormPermission(formPermission, ddf, saveEnable);
        }

        public virtual void SetPropertys()
        {
            this.DataFormConntroller.InitDataFormParams();
        }

        protected bool SetTableRowValue(string dbtable, string dbcolumn, int index, object value, GetTableRowValueString getTableRowValueString, object control)
        {
            DataSet dataSource = this.DataFormConntroller.DataSource;
            if (!dataSource.Tables.Contains(dbtable))
            {
                LoggingService.WarnFormatted("Table:{0},Column:{1} 关联的表没有找到...", new object[] { dbtable, dbcolumn });
                return false;
            }
            DataTable table = dataSource.Tables[dbtable];
            if (!table.Columns.Contains(dbcolumn))
            {
                LoggingService.WarnFormatted("Table:{0},Column:{1} 关联的表没有找到相应的列...", new object[] { dbtable, dbcolumn });
                return false;
            }
            DataRow row = dataSource.Tables[dbtable].Rows[index];
            DataColumn column = this.dataFormController.DataSource.Tables[dbtable].Columns[dbcolumn];
            string str = getTableRowValueString(control, column.DataType, this.dataFormController.DataSource.Tables[dbtable].Rows[0][dbcolumn]);
            if (value.Equals(str))
            {
                return false;
            }
            this.dataFormController.DataSource.Tables[dbtable].Rows[index].BeginEdit();
            try
            {
                this.dataFormController.DataSource.Tables[dbtable].Rows[index][dbcolumn] = value;
                string str2 = getTableRowValueString(control, column.DataType, this.dataFormController.DataSource.Tables[dbtable].Rows[index][dbcolumn]);
                if (str == str2)
                {
                    return false;
                }
                if (this.dataFormController.DataSource.Tables[dbtable].Rows[index].RowState == DataRowState.Unchanged)
                {
                    this.dataFormController.DataSource.Tables[dbtable].Rows[index].SetModified();
                }
                LoggingService.InfoFormatted("数据有修改,原值:{0} 新值: {1}", new object[] { str, str2 });
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                if (((value.ToString().Trim().Length == 0) && column.AllowDBNull) && ((((column.DataType == typeof(decimal)) || (column.DataType == typeof(int))) || ((column.DataType == typeof(double)) || (column.DataType == typeof(float)))) || (column.DataType == typeof(DateTime))))
                {
                    this.dataFormController.DataSource.Tables[dbtable].Rows[index][dbcolumn] = Convert.DBNull;
                    value = string.Empty;
                }
                else
                {
                    MessageHelper.ShowInfo("{0}输入有错误：{1}", new string[] { dbcolumn, exception.Message });
                    if (control is Control)
                    {
                        (control as Control).Focus();
                    }
                    else if (control is HtmlElement)
                    {
                        (control as HtmlElement).Focus();
                    }
                    return false;
                }
            }
            finally
            {
                this.dataFormController.DataSource.Tables[dbtable].Rows[index].EndEdit();
            }
            return true;
        }

        protected string TestHmtlEelementValue(HtmlElement he, string dbtable, string dbcolumn, int index)
        {
            string attribute = he.GetAttribute("value");
            if (string.IsNullOrEmpty(attribute))
            {
                return attribute;
            }
            DataSet dataSource = this.DataFormConntroller.DataSource;
            if (!dataSource.Tables.Contains(dbtable))
            {
                LoggingService.WarnFormatted("Table:{0},Column:{1} 关联的表没有找到...", new object[] { dbtable, dbcolumn });
                return attribute;
            }
            DataTable table = dataSource.Tables[dbtable];
            if (!table.Columns.Contains(dbcolumn))
            {
                LoggingService.WarnFormatted("Table:{0},Column:{1} 关联的表没有找到相应的列...", new object[] { dbtable, dbcolumn });
                return attribute;
            }
            DataRow row = dataSource.Tables[dbtable].Rows[index];
            DataColumn column = this.dataFormController.DataSource.Tables[dbtable].Columns[dbcolumn];
            System.Type dataType = column.DataType;
            if ((((!dataType.Equals(typeof(int)) && !dataType.Equals(typeof(long))) && (!dataType.Equals(typeof(double)) && !dataType.Equals(typeof(decimal)))) && !dataType.Equals(typeof(float))) && !dataType.Equals(typeof(float)))
            {
                return attribute;
            }
            int decimals = 2;
            if (dataType.Equals(typeof(int)) || dataType.Equals(typeof(long)))
            {
                decimals = 0;
            }
            else
            {
                string str2 = he.GetAttribute("format");
                if (!string.IsNullOrEmpty(str2))
                {
                    int num2 = str2.LastIndexOf('.');
                    decimals = (num2 >= 0) ? ((str2.Length - num2) - 1) : 0;
                }
            }
            return MathHelper.Round(Convert.ToDouble(attribute), decimals).ToString();
        }

        private void toolTipController_BeforeShow(object sender, ToolTipControllerShowEventArgs e)
        {
            if (e.ToolTip == "test")
            {
                e.Show = false;
                this.BackgroundDisplayTooltip(e);
            }
            else if (e.ToolTip.Length > 0)
            {
                e.Show = true;
            }
        }

        private void WB_DocumentCopmpleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument document = (sender as WebBrowser).Document;
            foreach (string str in WebBrowserExtensions.tags)
            {
                HtmlElementCollection elementsByTagName = document.GetElementsByTagName(str);
                Dictionary<string, DataControl> dataControlDict = this.GetDataControlDict();
                DataSet dataSource = this.dataFormController.DataSource;
                foreach (HtmlElement element in elementsByTagName)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(element.GetAttribute("qm")))
                        {
                            element.Click += new HtmlElementEventHandler(this.HtmlQMButtonClick);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(element.GetAttribute("jc")))
                        {
                            element.Click += new HtmlElementEventHandler(this.HtmlInheritDataButtonClick);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(element.GetAttribute("hj")))
                        {
                            element.Click += new HtmlElementEventHandler(this.HtmlHJButtonClick);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(element.GetAttribute("hq")))
                        {
                            element.Click += new HtmlElementEventHandler(this.HtmlHQButtonClick);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(element.GetAttribute("js")))
                        {
                            element.Click += new HtmlElementEventHandler(this.HtmlJSButtonClick);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(element.GetAttribute("dk")))
                        {
                            element.Click += new HtmlElementEventHandler(this.HtmlDKButtonClick);
                            continue;
                        }
                        string attribute = element.GetAttribute("dbtable");
                        string name = element.GetAttribute("dbcolumn");
                        if (string.IsNullOrEmpty(attribute) || string.IsNullOrEmpty(name))
                        {
                            LoggingService.DebugFormatted("将在后台数据库定义中查找ID为:{0}的HTML控件", new object[] { element.Id });
                            if (dataControlDict.ContainsKey(element.Id.ToLower()))
                            {
                                DataControl control = dataControlDict[element.Id.ToLower()];
                                if (control.MapTable != null)
                                {
                                    attribute = control.MapTable.Name;
                                    name = control.MapColumn.Name;
                                    element.SetAttribute("dbtable", attribute);
                                    element.SetAttribute("dbcolumn", name);
                                    element.SetAttribute("title", control.Description);
                                    if (element.TagName.ToLower() == "select")
                                    {
                                        foreach (string str4 in control.ValueCollection)
                                        {
                                            HtmlElement newElement = document.CreateElement("option");
                                            newElement.SetAttribute("value", str4);
                                            newElement.InnerText = str4;
                                            element.AppendChild(newElement);
                                        }
                                    }
                                }
                                else
                                {
                                    LoggingService.WarnFormatted("ID:{0}没有设定关联表", new object[] { element.Id });
                                    continue;
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(attribute) || string.IsNullOrEmpty(name))
                        {
                            LoggingService.WarnFormatted("ID:{0},Table:{1},Column:{2} 没有设置表或列,请检查是不是真的不需要保存该控件的值...", new object[] { element.Id, attribute, name });
                            continue;
                        }
                        if (!dataSource.Tables.Contains(attribute))
                        {
                            LoggingService.WarnFormatted("ID:{0},Table:{1},Column:{2} 关联的表没有找到...", new object[] { element.Id, attribute, name });
                            continue;
                        }
                        DataTable table = dataSource.Tables[attribute];
                        if (!table.Columns.Contains(name))
                        {
                            LoggingService.WarnFormatted("ID:{0},Table:{1},Column:{2} 关联的表没有找到相应的列...", new object[] { element.Id, attribute, name });
                            continue;
                        }
                        DataColumn column = table.Columns[name];
                        DataRow row = table.Rows[0];
                        string str5 = this.GetHtmlElementValue(element, column.DataType, row[column]);
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("将设置ID:{0},DbTable:{1},DbColumn:{2}的值为:{3}", new object[] { element.Id, attribute, name, str5 });
                        }
                        if (!((element.GetAttribute("type") == "hidden") && string.IsNullOrEmpty(str5)))
                        {
                            WebBrowserExtensions.SetHtmlElementValue(element, str5);
                        }
                        if (element.GetAttribute("type") != "hidden")
                        {
                            element.MouseDown += new HtmlElementEventHandler(this.DbHtmlElementLostFocus);
                            element.KeyDown += new HtmlElementEventHandler(this.DbHtmlElementLostFocus);
                            if (element.TagName.ToLower() == "select")
                            {
                                element.LostFocus += new HtmlElementEventHandler(this.DbHtmlElementLostFocus);
                            }
                            else
                            {
                                element.LosingFocus += new HtmlElementEventHandler(this.DbHtmlElementLostFocus);
                            }
                            if (column.DataType == typeof(string))
                            {
                                element.SetAttribute("maxlength", column.MaxLength.ToString());
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error(exception);
                    }
                }
            }
        }

        public virtual IDictionary DataControls
        {
            get
            {
                this.LoadDataControls();
                return this.dcs;
            }
        }

        public DataFormController DataFormConntroller
        {
            get
            {
                if (this.dataFormController == null)
                {
                    this.dataFormController = DataFormController.Create();
                }
                return this.dataFormController;
            }
        }

        protected virtual Dictionary<string, InvokeResult> InvokeResults
        {
            get
            {
                return null;
            }
        }

        public virtual bool IsChanged
        {
            get
            {
                bool isChanged;
                try
                {
                    this.PostEditor();
                    foreach (WebBrowser browser in this.wbs.Values)
                    {
                        WebBrowserExtensions.AttachValueToDataSource(this.DataFormConntroller.DataSource, browser.Document);
                    }
                    isChanged = this.DataFormConntroller.IsChanged;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                return isChanged;
            }
        }

        protected virtual bool IsResetFont
        {
            get
            {
                return false;
            }
        }

        public virtual string[] ProjectSubTypes
        {
            get
            {
                return null;
            }
        }

        protected DevExpress.XtraEditors.StyleController StyleController
        {
            get
            {
                if (this.styleController == null)
                {
                    this.styleController = new DevExpress.XtraEditors.StyleController();
                    this.styleController.Appearance.Options.UseFont = true;
                    this.styleController.Appearance.Font = this.resetFont;
                    this.styleController.AppearanceDropDown.Font = this.resetFont;
                    this.styleController.AppearanceDropDown.Options.UseFont = true;
                    this.styleController.AppearanceDropDownHeader.Font = this.resetFont;
                    this.styleController.AppearanceDropDownHeader.Options.UseFont = true;
                    this.styleController.AppearanceReadOnly.Font = this.resetFont;
                    this.styleController.AppearanceReadOnly.Options.UseFont = true;
                }
                return this.styleController;
            }
        }
    }
}

