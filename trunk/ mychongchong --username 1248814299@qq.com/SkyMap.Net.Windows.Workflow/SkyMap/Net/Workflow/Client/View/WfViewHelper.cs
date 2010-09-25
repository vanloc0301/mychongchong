namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Windows.Forms;

    public sealed class WfViewHelper
    {
        private static Dictionary<string, Assembly> dfAssemblys = new Dictionary<string, Assembly>(3);
        private static Dictionary<string, System.Type> dfTypes = new Dictionary<string, System.Type>(3);
        private static Dictionary<string, IWfView> views = new Dictionary<string, IWfView>();

        static WfViewHelper()
        {
            SecurityUtil.CurrentPrincipalChanging += new CancelEventHandler(WfViewHelper.SecurityUtil_CurrentPrincipalChanging);
        }

        private static void AddParams(IDataForm dataForm, FormPermission formPerm)
        {
            dataForm.AddParams("PageIndex", formPerm.PageIndex);
        }

        private static void AddParams(IDataForm dataForm, WorkItem workItem)
        {
            dataForm.AddParams("ActdefId", workItem.ActdefId);
            dataForm.AddParams("ActinsId", workItem.ActinstId);
            dataForm.AddParams("ProinsId", workItem.ProinstId);
            dataForm.AddParams("ProjectId", workItem.ProjectId);
        }

        public static void DataViewChanged(object sender, ListChangedEventArgs e)
        {
            LoggingService.Info("DataView Chaned");
            if (((e == null) || (e.ListChangedType == ListChangedType.Reset)) || (e.ListChangedType == ListChangedType.ItemMoved))
            {
                foreach (IWfView view in views.Values)
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("set data row view");
                    }
                    view.NavigationDataRowViews = null;
                    view.NavigationIndexs = null;
                    (view as WfView).BarStatusUpdate();
                }
            }
        }

        public static IDataForm GetNewDataFormInstance(DAODataForm df)
        {
            System.Type type = null;
            lock (dfTypes)
            {
                string key = string.Format("{0},{1}", df.Name, df.AssemblyName);
                if (!dfTypes.ContainsKey(key))
                {
                    Assembly assembly = null;
                    lock (dfAssemblys)
                    {
                        if (!dfAssemblys.ContainsKey(df.AssemblyName))
                        {
                            try
                            {
                                if (LoggingService.IsDebugEnabled)
                                {
                                    LoggingService.DebugFormatted("载入程序集：{0}{1}.dll", new object[] { DataFormController.BaseDir, df.AssemblyName });
                                }
                                assembly = Assembly.LoadFrom(DataFormController.BaseDir + df.AssemblyName + ".dll");
                                if (LoggingService.IsDebugEnabled)
                                {
                                    LoggingService.Debug("成功载入程序集...");
                                }
                                dfAssemblys.Add(df.AssemblyName, assembly);
                            }
                            catch (FileNotFoundException)
                            {
                                throw new CoreException("表单程序集：" + df.AssemblyName + "没找到");
                            }
                            catch (System.Security.SecurityException)
                            {
                                throw new CoreException("权限不足不能访问指定的程序集：" + df.AssemblyName);
                            }
                        }
                        else
                        {
                            assembly = dfAssemblys[df.AssemblyName];
                        }
                    }
                    if (assembly != null)
                    {
                        type = assembly.GetType(df.Name, false);
                        if (type != null)
                        {
                            dfTypes.Add(key, type);
                        }
                    }
                }
                else
                {
                    type = dfTypes[key];
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将创建类型：{0}的实例", new object[] { df.Name });
            }
            IDataForm form = null;
            if (type != null)
            {
                form = Activator.CreateInstance(type) as IDataForm;
            }
            if (form == null)
            {
                throw new NullReferenceException(string.Format("不能创建表单类型：{0},{1}", df.Name, df.AssemblyName));
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("创建类型：{0}的实例完成", new object[] { df.Name });
            }
            return form;
        }

        public static void OpenView(IWfBox box, IWfView view, DataRowView drv, int index, bool canEdit)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Ready to open view");
            }
            WorkItem workItem = BoxHelper.GetWorkItem(drv, box);
            if (StringHelper.IsNull(workItem.ProinstId))
            {
                (view as Form).Close();
            }
            else
            {
                int[] numArray;
                if (StringHelper.IsNull(workItem.ProjectId) || StringHelper.IsNull(workItem.ProdefId))
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("获取的行中没有业务定义信息，需要根据业务ID从数据库中获取!");
                    }
                    Proinst proinst = QueryHelper.Get<Proinst>(workItem.ProinstId);
                    if (proinst == null)
                    {
                        return;
                    }
                    workItem.ProjectId = proinst.ProjectId;
                    workItem.ProdefId = proinst.ProdefId;
                    workItem.ProinstName = proinst.Name;
                }
                DataRowView[] navigationDataRowViews = BoxHelper.GetNavigationDataRowViews(drv, index, out numArray);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("打开的是业务列表中的第{0}个业务 : " + index.ToString());
                }
                OpenView(view, workItem, drv, navigationDataRowViews, numArray, canEdit);
            }
        }

        public static void OpenView(IWfView view, WorkItem workItem, DataRowView row, DataRowView[] navigationDataRowViews, int[] indexs, bool canEdit)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("检查是否已经打开该业务");
            }
            if (views.ContainsKey(workItem.ProinstId))
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("该业务已经打开，直接转到该业务！");
                }
                view.Close();
                Form form = views[workItem.ProinstId] as Form;
                if (form.WindowState == FormWindowState.Minimized)
                {
                    form.WindowState = FormWindowState.Normal;
                }
                form.Focus();
            }
            else
            {
                DAODataForm daoDataForm = null;
                Prodef prodef = WorkflowService.Prodefs[workItem.ProdefId];
                FormPermission formPerm = null;
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("检查是否是工作流在办业务");
                }
                if (!StringHelper.IsNull(workItem.ActdefId))
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("准备获取在办业务权限");
                    }
                    Actdef actdef = prodef.Actdefs[workItem.ActdefId];
                    if (actdef == null)
                    {
                        QueryHelper.Get<Actdef>("Actdef_" + workItem.ActdefId, workItem.ActdefId);
                    }
                    formPerm = actdef.ActdefFormPermission;
                    if (formPerm != null)
                    {
                        daoDataForm = WorkflowService.GetDaoDataForm(formPerm.DaoDataFormId);
                        if (daoDataForm == null)
                        {
                            MessageHelper.ShowInfo("找不到ID为:{0}的表单配置,\r\n请检查通用表单权限配置是否选择了正确的表单", formPerm.DaoDataFormId);
                            return;
                        }
                        ParticipantFormPermission formPermissionsOfCurrentStaff = WorkflowService.GetFormPermissionsOfCurrentStaff(daoDataForm);
                        if (formPermissionsOfCurrentStaff != null)
                        {
                            view.PrintSetId = formPerm.PrintSetId + "," + formPermissionsOfCurrentStaff.PrintSetId;
                        }
                        else
                        {
                            view.PrintSetId = formPerm.PrintSetId;
                        }
                    }
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("在办业务：获取了其在办权限");
                    }
                }
                else
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("不是在办业务就查看人员对些表单的权限设置");
                    }
                    if (prodef != null)
                    {
                        daoDataForm = WorkflowService.GetDaoDataForm(prodef.DaoDataFormId);
                        if (daoDataForm != null)
                        {
                            formPerm = WorkflowService.GetFormPermissionsOfCurrentStaff(daoDataForm);
                            if (formPerm == null)
                            {
                                LoggingService.Warn("当前操作用户没有对该表单的通用表单参与者权限的设置");
                                view.Close();
                                return;
                            }
                            view.PrintSetId = formPerm.PrintSetId;
                        }
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.Info("获取了对表单的操作权限");
                        }
                    }
                }
                IWfDataForm dataForm = null;
                if (daoDataForm != null)
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("准备初始化表单参数");
                    }
                    dataForm = view.DataForm;
                    if (!((dataForm != null) && dataForm.GetType().FullName.Equals(daoDataForm.Name)))
                    {
                        dataForm = GetNewDataFormInstance(daoDataForm) as IWfDataForm;
                        dataForm.WfView = view;
                        dataForm.SetElse();
                    }
                    AddParams(dataForm, formPerm);
                    AddParams(dataForm, workItem);
                    dataForm.SetPropertys();
                    dataForm.SetFormPermission(formPerm, daoDataForm, canEdit);
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("调用通用表单载入数据");
                    }
                    dataForm.LoadMe();
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("载入数据完成");
                    }
                }
                view.CanPass = false;
                if (((workItem != null) && !string.IsNullOrEmpty(workItem.AssignId)) && (QueryHelper.Get<WfAssigninst>(workItem.AssignId).Status == AssignStatusType.Accepted))
                {
                    view.CanPass = true;
                }
                view.ViewClosed += new EventHandler(WfViewHelper.ViewClosed);
                view.SetCurrentDataRowView(row);
                view.NavigationDataRowViews = navigationDataRowViews;
                view.NavigationIndexs = indexs;
                if (formPerm != null)
                {
                    LoggingService.InfoFormatted("是否具有删除权限：{0}", new object[] { formPerm.EnableDelete });
                    view.CanRemove = formPerm.EnableDelete;
                }
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("准备好要显示表单及其它相关视图");
                }
                view.Show(prodef.Style, dataForm, workItem);
                views.Add(workItem.ProinstId, view);
            }
        }

        public static void OpenViewForCreateAndAcceptProinst(IWfBox box, IWfView view, string proinstId, bool canEdit)
        {
            string sql = string.Format("select s.ASSIGN_ID,p.PROINST_ID,p.PROINST_NAME,\r\n\t\t\t\t\tp.PROJECT_ID,p.PRODEF_ID,\r\n\t\t\t\t\ta.ACTINST_ID,a.ACTINST_NAME,a.ACTDEF_ID\r\n\t\tfrom WF_PROINST p\r\n\t\tinner join WF_ACTINST a on a.PROINST_ID=p.PROINST_ID\r\n\t\tinner join WF_ASSIGN s on s.ACTINST_ID=a.ACTINST_ID\r\n\t\twhere p.proinst_id='{0}' and s.ASSIGN_STATUS=1", proinstId);
            DataTable table = QueryHelper.ExecuteSql(typeof(Proinst).Namespace, null, sql);
            if (table.Rows.Count == 1)
            {
                WorkItem workItem = BoxHelper.GetWorkItem(table.DefaultView[0], box);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("将打开业务：{0} 的数据表单 : ", new object[] { workItem.ProjectId });
                }
                OpenView(view, workItem, null, null, null, canEdit);
            }
        }

        private static void SecurityUtil_CurrentPrincipalChanging(object sender, CancelEventArgs e)
        {
            int count = views.Count;
            if (count > 0)
            {
                if (MessageHelper.ShowYesNoInfo("注销用户将关闭所有打开的数据表单，是否继续？") == DialogResult.Yes)
                {
                    WfView[] array = new WfView[count];
                    views.Values.CopyTo(array, 0);
                    DialogResult none = DialogResult.None;
                    for (int i = count - 1; i > -1; i--)
                    {
                        if ((none == DialogResult.None) && array[i].Changed)
                        {
                            none = MessageHelper.ShowYesNoCancelInfo("数据已经改变，是否保存？");
                        }
                        if (none == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                            break;
                        }
                        if (none == DialogResult.Yes)
                        {
                            array[i].Save();
                        }
                        else
                        {
                            array[i].ForceCloseWithoutSaveChanged = true;
                        }
                        array[i].Close();
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private static void ViewClosed(object sender, EventArgs e)
        {
            IWfView view = sender as IWfView;
            try
            {
                WorkItem workItem = view.WorkItem;
                if (workItem != null)
                {
                    views.Remove(workItem.ProinstId);
                }
            }
            catch
            {
            }
        }
    }
}

