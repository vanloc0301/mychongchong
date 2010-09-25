namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Config;
    using SkyMap.Net.Workflow.Client.Dialog;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Client.View;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.ComponentModel;

    public sealed class BoxHelper
    {
        private static IDictionary<string, IWfBox> boxs = new Dictionary<string, IWfBox>();
        public const string fldSel = "sel";

        public static void AddDataTable(IWfBox box, DataTable table)
        {
            table.TableName = box.BoxName;
            AddTable(ref table);
            DataView defaultView = table.DefaultView;
            WfViewHelper.DataViewChanged(defaultView, null);
            defaultView.ListChanged += new ListChangedEventHandler(WfViewHelper.DataViewChanged);
        }

        public static void AddSelColumn(DataTable rs)
        {
            if (!rs.Columns.Contains("sel"))
            {
                DataColumn column = rs.Columns.Add("sel", typeof(bool));
                column.DefaultValue = false;
                column.ReadOnly = false;
            }
        }

        private static void AddTable(ref DataTable table)
        {
            SetRsReadOnly(table);
            AddSelColumn(table);
        }

        private static void BoxDisposed(object sender, EventArgs e)
        {
            boxs.Remove((sender as IBox).BoxName);
        }

        public static void CancelDelegate(IBox box)
        {
            WorkflowService.WfcInstance.CancelDelegate();
            WorkflowService.IsMyAllDelegated = false;
            box.RefreshData();
        }

        private static WfAbnormalType CheckCondition(WorkItem workItem)
        {
            string actinstId = workItem.ActinstId;
            string prodefId = workItem.ProdefId;
            string actdefId = workItem.ActdefId;
            Prodef prodef = WorkflowService.Prodefs[prodefId];
            if (prodef != null)
            {
                Actdef actdef = prodef.Actdefs[actdefId];
                if ((actdef != null) && (actdef.Froms.Count > 0))
                {
                    Transition transition = actdef.Froms[0];
                    SkyMap.Net.Workflow.XPDL.Application application = null;
                    string typeName = string.Empty;
                    IWfConditionApplication application2 = null;
                    System.Type type = null;
                    foreach (SkyMap.Net.Workflow.XPDL.Condition condition in transition.Conditions)
                    {
                        if (condition.Type.Equals("APPLICATION"))
                        {
                            application = condition.Application;
                            if (application != null)
                            {
                                typeName = application.TypeClass + "," + application.Assembly;
                            }
                            type = System.Type.GetType(typeName);
                            if (type != null)
                            {
                                application2 = Activator.CreateInstance(type) as IWfConditionApplication;
                                if (application.ExecutionType)
                                {
                                    int num = application2.IsOk(actinstId);
                                    try
                                    {
                                        return (WfAbnormalType) Enum.Parse(typeof(WfAbnormalType), num.ToString());
                                    }
                                    catch (ArgumentException exception)
                                    {
                                        throw new WfClientException("Cannot know the return value of check condition : " + num.ToString(), exception);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return WfAbnormalType.NO_ABNORMAL;
        }

        public static void CreateProcess(IWfBox box, Prodef prodef, int num)
        {
            string id = prodef.Id;
            WorkflowService.WfcInstance.CreateWfProcess(id, num);
        }

        public static void CreateProcessAndAccept(IWfBox box, Prodef prodef, int num)
        {
            string id = prodef.Id;
            WorkflowService.WfcInstance.CreateWfProcessAndAccept(id, num);
        }

        public static void Delegate(IWfBox box, IWfDialog wfDlg)
        {
            IDictionary<string, WorkItem> workItems = null;
            WorkItem firstSelectedWorkItem = null;
            try
            {
                DataRowView[] selectedRows = GetSelectedRows(box.DataSource as DataView);
                workItems = GetWorkItems(box, selectedRows);
                firstSelectedWorkItem = GetFirstSelectedWorkItem(box);
            }
            catch (NotSelectException)
            {
            }
            wfDlg.WorkItem = firstSelectedWorkItem;
            if (wfDlg.ShowDialog() == DialogResult.OK)
            {
                WfLogicalAbnormalContextData contextData = new WfLogicalAbnormalContextData();
                wfDlg.SetContextData(contextData);
                WfUtil.SetAbnormalContextData(contextData);
                WorkflowService.WfcInstance.Delegate(workItems);
                box.RefreshData();
                WfUtil.FreeAbnormalContextData();
                if (firstSelectedWorkItem == null)
                {
                    WorkflowService.IsMyAllDelegated = true;
                }
            }
        }

        public static void DelegateEvent(IWfBox box, WfClientAPIHandler handle)
        {
            DelegateEvent(box, handle, null);
        }

        public static void DelegateEvent(IWfBox box, WfClientAPIHandler handle, string[] batchField)
        {
            DataRowView[] selectedRows = GetSelectedRows(box.DataSource as DataView, batchField);
            IDictionary<string, WorkItem> workItems = GetWorkItems(box, selectedRows);
            DelegateEvent(selectedRows, workItems, handle);
        }

        public static void DelegateEvent(IWfBox box, DataRowView row, WfClientAPIHandler handle)
        {
            WorkItem workItem = GetWorkItem(row, box);
            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>(1);
            workItems.Add(workItem.Id, workItem);
            handle(workItems);
            if (row.Row.RowState != DataRowState.Deleted)
            {
                row.Delete();
            }
        }

        private static void DelegateEvent(DataRowView[] rows, IDictionary<string, WorkItem> workItems, WfClientAPIHandler handle)
        {
            handle(workItems);
            DeleteRows(rows);
        }

        public static void DelegateEvent(IWfBox box, IWfDialog wfDlg, WfClientAPIHandler handle)
        {
            DelegateEvent(box, wfDlg, handle, null);
        }

        public static void DelegateEvent(IWfBox box, IWfDialog wfDlg, WfClientAPIHandler handle, string[] batchField)
        {
            try
            {
                DataRowView[] selectedRows = GetSelectedRows(box.DataSource as DataView, batchField);
                IDictionary<string, WorkItem> workItems = GetWorkItems(box, selectedRows);
                IEnumerator<KeyValuePair<string, WorkItem>> enumerator = workItems.GetEnumerator();
                enumerator.MoveNext();
                KeyValuePair<string, WorkItem> current = enumerator.Current;
                wfDlg.WorkItem = current.Value;
                if (wfDlg.ShowDialog() == DialogResult.OK)
                {
                    WfLogicalAbnormalContextData contextData = new WfLogicalAbnormalContextData();
                    wfDlg.SetContextData(contextData);
                    WfUtil.SetAbnormalContextData(contextData);
                    DelegateEvent(selectedRows, workItems, handle);
                    WfUtil.FreeAbnormalContextData();
                }
            }
            finally
            {
                wfDlg.Close();
            }
        }

        private static void DeleteRows(DataRowView[] rows)
        {
            int length = rows.Length;
            DataRowView view = null;
            for (int i = length - 1; i > -1; i--)
            {
                view = rows[i];
                if (view == null)
                {
                    return;
                }
                if (view.Row.RowState != DataRowState.Deleted)
                {
                    view.Delete();
                }
            }
            if (length >= 1)
            {
                rows[0].DataView.Table.AcceptChanges();
            }
        }

        public static IBox GetBox(string boxName)
        {
            IBox box2;
            try
            {
                IBox box;
                CBoxConfig boxConfig = CControlConfig.GetBoxConfig(boxName);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("开始创建工作箱：{0}", new object[] { boxConfig.Name });
                }
                switch (boxConfig.Class)
                {
                    case "WfBox":
                    case "SkyMap.Net.Workflow.Client.Box.WfBox":
                    case "SkyMap.Net.Workflow.Client.Box.Abort":
                    case "SkyMap.Net.Workflow.Client.Box.Back":
                    case "SkyMap.Net.Workflow.Client.Box.Bulletin":
                    case "SkyMap.Net.Workflow.Client.Box.Fj":
                    case "SkyMap.Net.Workflow.Client.Box.Sj":
                    case "SkyMap.Net.Workflow.Client.Box.Suspend":
                    case "SkyMap.Net.Workflow.Client.Box.Terminate":
                    case "SkyMap.Net.Workflow.Client.Box.Zb":
                    case "SkyMap.Net.Workflow.Client.Box.WfExternalBox":
                        box = new WfBox();
                        if (!string.IsNullOrEmpty(boxConfig.OpenViewCommand))
                        {
                            goto Label_0235;
                        }
                        switch (boxConfig.Class)
                        {
                            case "SkyMap.Net.Workflow.Client.Box.WfExternalBox":
                                goto Label_0235;

                            case "SkyMap.Net.Workflow.Client.Box.Bulletin":
                                goto Label_0200;

                            case "SkyMap.Net.Workflow.Client.Box.Sj":
                                goto Label_020E;

                            case "SkyMap.Net.Workflow.Client.Box.Fj":
                                goto Label_021C;
                            case "SkyMap.Net.Workflow.Client.Box.Zb": // lhm add 20100720
                                goto Label_0200_hm;
                            case "SkyMap.Net.Workflow.Client.Box.Back":
                                goto Label_0200_hm;

                        }
                        goto Label_0226;

                    default:
                        goto Label_027B;
                }
            Label_0200_hm:
                boxConfig.OpenViewCommand = "SkyMap.Net.Workflow.Client.Commands.OpenZbViewForFocusCommand,SkyMap.Net.Windows.Workflow";
                goto Label_0235;
            Label_0200:
                boxConfig.OpenViewCommand = "SkyMap.Net.Workflow.Client.Commands.OpenViewForQueryFocusCommand,SkyMap.Net.Windows.Workflow";
                goto Label_0235;
            Label_020E:
                boxConfig.OpenViewCommand = "SkyMap.Net.Workflow.Client.Commands.AcceptAndOpenFocusCommand,SkyMap.Net.Windows.Workflow";
                goto Label_0235;
            Label_021C:
                boxConfig.OpenViewCommand = null;
                goto Label_0235;
            Label_0226:
                boxConfig.OpenViewCommand = "SkyMap.Net.Workflow.Client.Commands.OpenViewForFocusCommand,SkyMap.Net.Windows.Workflow";
            Label_0235:
                if (string.IsNullOrEmpty(boxConfig.ToolbarPath))
                {
                    boxConfig.ToolbarPath = string.Format("/Workflow/{0}/ToolBar", boxConfig.Class.Substring(boxConfig.Class.LastIndexOf('.') + 1));
                }
                goto Label_031C;
            Label_027B:
                box = Assembly.GetExecutingAssembly().CreateInstance(boxConfig.Class, true) as IBox;
                if (box == null)
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.InfoFormatted("没有在本程序集中找到类型：{0}", new object[] { boxConfig.Class });
                    }
                    box = Activator.CreateInstance(System.Type.GetType(boxConfig.Class)) as IBox;
                }
                if (string.IsNullOrEmpty(boxConfig.ToolbarPath))
                {
                    boxConfig.ToolbarPath = string.Format("/Workflow/{0}/ToolBar", box.GetType().Name);
                }
            Label_031C:
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("完成创建工作箱：{0}", new object[] { boxConfig.Name });
                }
                box.BoxName = boxName;
                box.Init(boxConfig);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("开始更新工作箱：{0} 的数据", new object[] { box.BoxName });
                }
                box.RefreshData();
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("完成工作箱：{0} 数据载入", new object[] { box.BoxName });
                }
                box2 = box;
            }
            catch (CoreException exception)
            {
                throw exception;
            }
            catch (Exception exception2)
            {
                LoggingService.Error(exception2.Message + exception2.StackTrace);
                throw new WfClientException("Cannot get the box :" + boxName, exception2);
            }
            return box2;
        }

        private static string GetColumnValue(string name, DataRowView dvrow)
        {
            if (dvrow.Row.Table.Columns.Contains(name))
            {
                return dvrow[name].ToString().Trim();
            }
            return string.Empty;
        }

        public static DataView GetData(IWfBox box)
        {
            string[] queryParameters = GetQueryParameters(box.QueryParameters);
            return GetData(box, queryParameters);
        }

        public static DataView GetData(IWfBox box, string[] vals)
        {
            return GetData(box, box.DAONameSpace, box.QueryName, vals);
        }

        public static DataView GetData(IWfBox box, string nameSpace, string queryName, string[] vals)
        {
            DataTable rs = null;
            if (string.IsNullOrEmpty(nameSpace))
            {
                rs = WorkflowService.GetRs(box.QueryName, vals);
            }
            else
            {
                rs = WorkflowService.GetRs(nameSpace, queryName, vals);
            }
            rs.AcceptChanges();
            AddDataTable(box, rs);
            return rs.DefaultView;
        }

        public static int GetDataCount(CBoxConfig boxConfig)
        {
            int num = -1;
            string[] queryParameters = GetQueryParameters(boxConfig.QueryParameters);
            try
            {
                num = Convert.ToInt32(QueryHelper.ExecuteScalar("SkyMap.Net.Workflow", boxConfig.QueryCountName, queryParameters));
            }
            catch (Exception exception)
            {
                LoggingService.ErrorFormatted("获取‘{0}’数量时发生错误:{1}\r\n{2}", new object[] { boxConfig.Name, exception.Message, exception.StackTrace });
            }
            return num;
        }

        public static DataRowView GetDataRowView(IWfBox box, int index)
        {
            DataView dataSource = box.DataSource as DataView;
            if ((index > -1) && (index < dataSource.Count))
            {
                return GetDataRowView(dataSource, index);
            }
            return null;
        }

        public static DataRowView GetDataRowView(DataView dv, int index)
        {
            return dv[index];
        }

        public static int GetFirstSelectedIndex(IWfBox box)
        {
            DataView dataSource = (DataView) box.DataSource;
            DataRowView view2 = null;
            for (int i = 0; i < dataSource.Count; i++)
            {
                view2 = dataSource[i];
                if (view2["sel"].ToString().Equals("True"))
                {
                    return i;
                }
            }
            throw new NotSelectException();
        }

        private static WorkItem GetFirstSelectedWorkItem(IWfBox box)
        {
            int firstSelectedIndex = GetFirstSelectedIndex(box);
            return GetWorkItem(GetDataRowView(box, firstSelectedIndex), box);
        }

        public static DataRowView[] GetNavigationDataRowViews(DataRowView drv, int index, out int[] indexs)
        {
            DataRowView[] viewArray = new DataRowView[4];
            DataView dataView = drv.DataView;
            indexs = new int[] { -1, -1, -1, -1 };
            if (drv.Row.RowState == DataRowState.Deleted)
            {
                if (dataView.Count > 0)
                {
                    viewArray[0] = dataView[0];
                    viewArray[3] = dataView[dataView.Count - 1];
                    indexs[0] = 0;
                    indexs[3] = dataView.Count - 1;
                    if (index >= 1)
                    {
                        viewArray[1] = dataView[index - 1];
                        indexs[1] = index - 1;
                    }
                    if (index < dataView.Count)
                    {
                        viewArray[2] = dataView[index];
                        indexs[2] = index;
                    }
                }
                return viewArray;
            }
            if (index > 0)
            {
                viewArray[0] = dataView[0];
                viewArray[1] = dataView[index - 1];
                indexs[0] = 0;
                indexs[1] = index - 1;
            }
            if (index < (dataView.Count - 1))
            {
                viewArray[2] = dataView[index + 1];
                viewArray[3] = dataView[dataView.Count - 1];
                indexs[2] = index + 1;
                indexs[3] = dataView.Count - 1;
            }
            return viewArray;
        }

        private static string[] GetQueryParameters(string[] queryParameters)
        {
            if (queryParameters == null)
            {
                return null;
            }
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            SmIdentity identity = smPrincipal.Identity as SmIdentity;
            CStaff staff = null;
            string[] strArray = new string[queryParameters.Length];
            for (int i = 0; i < queryParameters.Length; i++)
            {
                string str = queryParameters[i].ToUpper();
                if (str != null)
                {
                    if (!(str == "STAFF"))
                    {
                        if (str == "DEPTS")
                        {
                            goto Label_0094;
                        }
                        if (str == "ROLES")
                        {
                            goto Label_00A5;
                        }
                        if (str == "USERNAME")
                        {
                            goto Label_00B6;
                        }
                    }
                    else
                    {
                        strArray[i] = identity.UserId;
                    }
                }
                continue;
            Label_0094:
                strArray[i] = StringHelper.LinkForQuery(smPrincipal.DeptIds);
                continue;
            Label_00A5:
                strArray[i] = StringHelper.LinkForQuery(smPrincipal.RoleIds);
                continue;
            Label_00B6:
                if (staff == null)
                {
                    staff = OGMService.GetStaff(identity.UserId);
                }
                strArray[i] = staff.UserName;
            }
            return strArray;
        }

        public static int GetSelectedCount(DataView datasource)
        {
            if (datasource == null)
            {
                throw new WfClientException("Datasource is null");
            }
            int num = 0;
            foreach (DataRowView view in datasource)
            {
                if (view["sel"].Equals(true))
                {
                    num++;
                }
            }
            return num;
        }

        public static DataRowView[] GetSelectedRows(DataView datasource)
        {
            if (datasource == null)
            {
                throw new WfClientException("Datasource is null");
            }
            List<DataRowView> list = new List<DataRowView>();
            for (int i = 0; i < datasource.Count; i++)
            {
                DataRowView item = datasource[i];
                if (item["sel"].Equals(true))
                {
                    list.Add(item);
                }
            }
            if (list.Count == 0)
            {
                throw new NotSelectException();
            }
            return list.ToArray();
        }

        public static DataRowView[] GetSelectedRows(DataView datasource, string[] batchField)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("获取选中的业务(需要判别是否可以进行批处理)...");
            }
            DataRowView[] selectedRows = GetSelectedRows(datasource);
            if ((batchField != null) && (batchField.Length != 0))
            {
                object[] objArray = new object[batchField.Length];
                int index = 0;
                for (index = 0; index < batchField.Length; index++)
                {
                    objArray[index] = selectedRows[0][batchField[index]];
                }
                for (index = 1; index < selectedRows.Length; index++)
                {
                    for (int i = 0; i < batchField.Length; i++)
                    {
                        if (!selectedRows[index][batchField[i]].Equals(objArray[i]))
                        {
                            throw new CannotBatchExecuteException();
                        }
                    }
                }
            }
            return selectedRows;
        }

        public static WorkItem GetWorkItem(DataRowView row, IWfBox box)
        {
            WorkItem item = new WorkItem();
            item.ActdefId = GetColumnValue("ACTDEF_ID", row);
            item.ProinstId = GetColumnValue("PROINST_ID", row);
            item.ProinstName = GetColumnValue("PROINST_NAME", row);
            item.ActinstId = GetColumnValue("ACTINST_ID", row);
            item.ActinstName = GetColumnValue("ACTINST_NAME", row);
            item.AssignId = GetColumnValue("ASSIGN_ID", row);
            item.AbnormalAuditId = GetColumnValue("ABNORMALAUDIT_ID", row);
            item.ProjectId = GetColumnValue("PROJECT_ID", row);
            item.ProdefId = GetColumnValue("PRODEF_ID", row);
            if (box != null)
            {
                item.Id = GetColumnValue(box.IdField, row);
            }
            return item;
        }

        private static IDictionary<string, WorkItem> GetWorkItems(IWfBox box, DataRowView[] rows)
        {
            IDictionary<string, WorkItem> dictionary = new Dictionary<string, WorkItem>();
            foreach (DataRowView view in rows)
            {
                WorkItem workItem = GetWorkItem(view, box);
                dictionary.Add(workItem.Id, workItem);
            }
            return dictionary;
        }

        public static bool IsSelected(IWfBox box)
        {
            DataView dataSource = (DataView) box.DataSource;
            DataRowView view2 = null;
            for (int i = 0; i < dataSource.Count; i++)
            {
                view2 = dataSource[i];
                if (view2["sel"].ToString().Equals("True"))
                {
                    return true;
                }
            }
            return false;
        }

        public static void PassToNext(IWfBox box, IWfDialog wfDlg)
        {
            DataRowView[] selectedRows = GetSelectedRows(box.DataSource as DataView);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("选中了{0}件可转出的业务", new object[] { selectedRows.Length });
            }
            PassToNext(GetWorkItems(box, selectedRows), selectedRows, wfDlg);
        }

        private static void PassToNext(IDictionary<string, WorkItem> workItems, DataRowView[] rows, IWfDialog wfDlg)
        {
            try
            {
                LoggingService.InfoFormatted("开始进行转出操作...", new object[0]);
                Dictionary<string, WfLogicalPassContextData> dictionary = new Dictionary<string, WfLogicalPassContextData>();
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                foreach (WorkItem item in workItems.Values)
                {
                    switch (CheckCondition(item))
                    {
                        case WfAbnormalType.SUSPENDED:
                            WorkflowService.WfcInstance.Suspend(item);
                            goto Label_017A;

                        case WfAbnormalType.TERMINATED:
                            WorkflowService.WfcInstance.Terminate(item);
                            goto Label_017A;

                        case WfAbnormalType.NO_ABNORMAL:
                            WfLogicalPassContextData data;
                            if (!dictionary.ContainsKey(item.ActdefId))
                            {
                                data = new WfLogicalPassContextData();
                                wfDlg.WorkItem = item;
                                switch (wfDlg.ShowDialog())
                                {
                                    case DialogResult.OK:
                                        wfDlg.SetContextData(data);
                                        break;

                                    case DialogResult.Cancel:
                                        return;
                                }
                                data.FromStaffId = smIdentity.UserId;
                                data.FromStaffName = smIdentity.UserName;
                            }
                            else
                            {
                                data = dictionary[item.ActdefId];
                            }
                            data.FromActivityKey = item.ActinstId;
                            data.FromActivityName = item.ActinstName;
                            data.FromAssignId = item.Id;
                            WfUtil.SetPassContextData(data);
                            WorkflowService.WfcInstance.CompleteWorkItem(item);
                            goto Label_017A;

                        case WfAbnormalType.ABROTED:
                            WorkflowService.WfcInstance.Abort(item);
                            goto Label_017A;
                    }
                    throw new WfClientException("Not implement the abnormal type operation of condition pass to next");
                Label_017A:;
                }
                DeleteRows(rows);
                LoggingService.InfoFormatted("转出操作完成...", new object[0]);
            }
            catch (CancelExecuteException)
            {
            }
            finally
            {
                wfDlg.Close();
                WfUtil.FreePassContextData();
            }
        }

        public static void PassToNext(WorkItem workItem, DataRowView row, IWfDialog wfDlg)
        {
            IDictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>();
            workItems.Add(workItem.Id, workItem);
            PassToNext(workItems, new DataRowView[] { row }, wfDlg);
        }

        public static void SelectRow(object dataSource, int i)
        {
            if (dataSource is DataView)
            {
                DataView view = dataSource as DataView;
                if (view.Table.Columns.Contains("sel") && ((i > -1) && (i < view.Count)))
                {
                    view[i]["sel"] = true;
                }
            }
        }

        public static void SetRsReadOnly(DataTable rs)
        {
            foreach (DataColumn column in rs.Columns)
            {
                if (string.Compare(column.ColumnName, "sel", true) != 0)
                {
                    column.ReadOnly = true;
                }
            }
        }
    }
}

