namespace SkyMap.Net.Workflow.Client.Services
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Text;

    public static class WorkflowService
    {        
        public const string FLD_SEL = "sel";
        private static bool? isMyAllDelegated;
        private const string KEY_AllProdefs = "ALL_Prodefs";

        private sealed class c__DisplayClass5
{
    // Fields
    public List<string> unableFrames;
}

    private sealed class c__DisplayClass8
{
    // Fields
    public WorkflowService.c__DisplayClass5 CS_8__locals6;
    public int i;

    // Methods
    public bool Gb__4(string s)
    {
        return (s == this.CS_8__locals6.unableFrames[this.i]);
    }
}
private static void cctorb__0(object senderd, DoWorkEventArgs ed)
{
    WorkflowClient wfcInstance = WfcInstance;
    DateTime now = DateTimeHelper.GetNow();
    new SkyMap.Net.DAO.DAOCache().Put();
}

private static void cctorb__1(object senderw, RunWorkerCompletedEventArgs ea)
{
    if (LoggingService.IsInfoEnabled)
    {
        LoggingService.Info("异步读取工作流配置数据成功");
    }
    if (ea.Error != null)
    {
        LoggingService.Error("异步读取工作流配置数据失败：", ea.Error);
    }
}


        static WorkflowService()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate (object senderd, DoWorkEventArgs ed) {
                WorkflowClient wfcInstance = WfcInstance;
                DateTime now = DateTimeHelper.GetNow();
                new SkyMap.Net.Workflow.XPDL.DAOCache().Put();
            };
            worker.RunWorkerCompleted += delegate (object senderw, RunWorkerCompletedEventArgs ea) {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("异步读取工作流配置数据成功");
                }
                if (ea.Error != null)
                {
                    LoggingService.Error("异步读取工作流配置数据失败：", ea.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        public static void DelegateEvent(IWfBox box, WfClientAPIHandler handle)
        {
            DelegateEvent(box, handle, null);
        }

        public static void DelegateEvent(WorkItem workItem, WfClientAPIHandler handle)
        {
            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>(1);
            workItems.Add(workItem.Id, workItem);
            handle(workItems);
        }

        public static void DelegateEvent(Dictionary<string, WorkItem> sl, WfClientAPIHandler handle)
        {
            handle(sl);
        }

        public static void DelegateEvent(IWfBox box, WfClientAPIHandler handle, string[] batchField)
        {
            DataRow[] selectedRows = box.GetSelectedRows(batchField);
            if (selectedRows.Length > 0)
            {
                IDictionary<string, WorkItem> workItems = GetWorkItems(selectedRows, box.IdField);
                DelegateEvent(box, selectedRows, workItems, handle);
            }
        }

        private static void DelegateEvent(IWfBox box, DataRow row, WfClientAPIHandler handle, string idField)
        {
            WorkItem workItem = GetWorkItem(row, idField);
            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>(1);
            workItems.Add(workItem.Id, workItem);
            handle(workItems);
            box.DeleteRows(new DataRow[] { row });
        }

        private static void DelegateEvent(IWfBox box, DataRow[] rows, IDictionary<string, WorkItem> workItems, WfClientAPIHandler handle)
        {
            handle(workItems);
            box.DeleteRows(rows);
        }

        public static IList<WfAdjunct> GetAdjuncts(string proistId)
        {
            KeyValuePair<string, object[]>[] querys = new KeyValuePair<string, object[]>[] { new KeyValuePair<string, object[]>("GetAdjuncts", new object[] { proistId }) };
            return (QueryHelper.List<WfAdjunct, WfAdjunct, WfAdjunct, WfAdjunct>(typeof(WfAdjunct).Namespace, string.Empty, querys)[0] as IList<WfAdjunct>);
        }

        private static IDictionary<string, Prodef> GetAllProdefs()
        {
            LoggingService.InfoFormatted("将获取所有业务定义...", new object[0]);
            Dictionary<string, Prodef> dictionary = new Dictionary<string, Prodef>();
            IList<Prodef> list = QueryHelper.List<Prodef>("ALL_Prodef");
            foreach (Prodef prodef in list)
            {
                dictionary.Add(prodef.Id, prodef);
            }
            return dictionary;
        }

        public static DataTable GetBoxItems(IWfBox box)
        {
            string[] queryParameterValues = GetQueryParameterValues(box.QueryParameters);
            DataTable rs = GetRs(box.QueryName, queryParameterValues);
            rs.Columns.Add("sel", typeof(bool));
            return rs;
        }

        private static string GetColumnValue(string name, DataRow row)
        {
            if (row.Table.Columns.Contains(name))
            {
                return row[name].ToString().Trim();
            }
            return string.Empty;
        }

        public static DAODataForm GetDaoDataForm(string id)
        {
            OGMService.CheckLoginStatus();
            if (StringHelper.IsNull(id))
            {
                return null;
            }
            return QueryHelper.Get<DAODataForm>("DAODataForm_" + id, id);
        }

        public static DAODataSet GetDaoDataSet(string id)
        {
            if (StringHelper.IsNull(id))
            {
                return null;
            }
            return QueryHelper.Get<DAODataSet>("DAODataSet_" + id, id);
        }

        public static ParticipantFormPermission GetFormPermissionsOfCurrentStaff(DAODataForm form)
        {       
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            SmIdentity identity = smPrincipal.Identity as SmIdentity;
            bool flag = (identity.AdminLevel == AdminLevelType.Admin) || (identity.AdminLevel == AdminLevelType.AdminData);
            string userId = identity.UserId;
            string key = "new_2_formpermission_" + form.Id + "_" + userId;
            ParticipantFormPermission permission = DAOCacheService.Get(key) as ParticipantFormPermission;
            if ((permission == null) || (permission.DaoDataFormId != form.Id))
            {
                permission = new ParticipantFormPermission();
                List<ParticipantFormPermission> list = new List<ParticipantFormPermission>();
                if (form != null)
                {
                    List<string> tmplist = new List<string>(smPrincipal.Participants);
                    foreach (ParticipantFormPermission permission2 in ParticipantFormPermissions)
                    {
                        
                        if ((permission2.DaoDataFormId == form.Id) && (flag ||  tmplist.Contains(permission2.ParticipantId)))
                        {
                            list.Add(permission2);
                        }
                    }
                }
                ParticipantFormPermission permission3 = null;
                StringBuilder sb = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    permission3 = list[i];
                    permission.DaoDataFormId = form.Id;
                    if (i == 0)
                    {
                        if (!flag)
                        {
                            sb.Append(permission3.UnableFrame);
                            builder2.Append(permission3.InVisibleFrame);
                            permission.PageIndex = permission3.PageIndex;
                        }
                        permission.EnableDelete = permission3.EnableDelete;
                        permission.PrintSetId = permission3.PrintSetId;
                    }
                    else
                    {
                        if (!flag)
                        {
                            sb.Append(",").Append(permission3.UnableFrame);
                            builder2.Append(",").Append(permission3.InVisibleFrame);
                        }
                        if (!string.IsNullOrEmpty(permission3.PrintSetId))
                        {
                            permission.PrintSetId = permission.PrintSetId + "," + permission3.PrintSetId;
                        }
                        if (!(!permission3.EnableDelete || permission.EnableDelete))
                        {
                            permission.EnableDelete = true;
                        }
                    }
                }
                if (count == 1)
                {
                    permission.UnableFrame = sb.ToString();
                    permission.InVisibleFrame = builder2.ToString();
                }
                else if (count > 1)
                {
                    permission.UnableFrame = GetUnablePerms(sb, count);
                    permission.InVisibleFrame = GetUnablePerms(builder2, count);
                }
                if ((count == 0) && flag)
                {
                    permission.DaoDataFormId = form.Id;
                }
                DAOCacheService.Put(key, permission);
            }
            if (StringHelper.IsNull(permission.DaoDataFormId))
            {
                return null;
            }
            return permission;
        }

        private static IList<Transition> GetInteractionTrans(Actdef actdef)
        {
            IList<Transition> froms = new List<Transition>();
            foreach (Transition transition in actdef.Froms)
            {
                ActdefType type = transition.To.Type;
                if (type != ActdefType.INTERACTION)
                {
                    if (type == ActdefType.OR_BRANCH)
                    {
                        goto Label_0040;
                    }
                    goto Label_004E;
                }
                froms = actdef.Froms;
                goto Label_005C;
            Label_0040:
                froms = transition.To.Froms;
                goto Label_005C;
            Label_004E:
                froms = GetInteractionTrans(transition.To);
            Label_005C:
                if (froms.Count > 0)
                {
                    return froms;
                }
            }
            return froms;
        }

        public static IList<ProdefRow> GetMyFirstProdefs()
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            string userId = (smPrincipal.Identity as SmIdentity).UserId;
            string key = "MyFirst_ProdefRows" + userId;
            IList<ProdefRow> list = DAOCacheService.Get(key) as IList<ProdefRow>;
            if (list == null)
            {
                string[] roleIds = smPrincipal.RoleIds;
                string[] deptIds = smPrincipal.DeptIds;
                if (roleIds.Length == 0)
                {
                    roleIds = new string[] { "''" };
                }
                if (deptIds.Length == 0)
                {
                    deptIds = new string[] { "''" };
                }
                list = QueryHelper.List<ProdefRow>(typeof(Prodef).Namespace, key + "_DAO", "GetMyFirstProdefs", new string[] { "roles", "depts", "staff" }, new object[] { roleIds, deptIds, userId });
                DAOCacheService.Put(key, list);
            }
            return list;
        }

        public static IList<ProdefRow> GetMyProdefs(string type)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            string userId = (smPrincipal.Identity as SmIdentity).UserId;
            string key = "My_" + type + "_ProdefRows_" + userId;
            IList<ProdefRow> list = DAOCacheService.Get(key) as IList<ProdefRow>;
            if (list == null)
            {
                string[] roleIds = smPrincipal.RoleIds;
                string[] deptIds = smPrincipal.DeptIds;
                if (roleIds.Length == 0)
                {
                    roleIds = new string[] { "''" };
                }
                if (deptIds.Length == 0)
                {
                    deptIds = new string[] { "''" };
                }
                list = QueryHelper.List<ProdefRow>(typeof(Prodef).Namespace, key + "_DAO", "GetMyProdefs", new string[] { "roles", "depts", "staff", "type" }, new object[] { roleIds, deptIds, userId, type });
                DAOCacheService.Put(key, list);
            }
            return list;
        }

        public static PrintSet GetPrintSet(string id)
        {
            if (StringHelper.IsNull(id))
            {
                return null;
            }
            string key = "PrintSet_" + id;
            PrintSet set = DAOCacheService.Get(key) as PrintSet;
            if (set == null)
            {
                set = new PrintSet();
                set.Id = id;
                set.TempletPrints = new List<TempletPrint>();
                string[] strArray = id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str2 in strArray)
                {
                    PrintSet set2 = QueryHelper.Get<PrintSet>("PrintSet_" + str2, str2);
                    if (set2 != null)
                    {
                        foreach (TempletPrint print in set2.TempletPrints)
                        {
                            if (!set.TempletPrints.Contains(print))
                            {
                                set.TempletPrints.Add(print);
                            }
                        }
                    }
                }
                DAOCacheService.Put(key, set);
            }
            return set;
        }

        public static IList<WfProinstMater> GetProinstMaters(string proistId)
        {
            KeyValuePair<string, object[]>[] querys = new KeyValuePair<string, object[]>[] { new KeyValuePair<string, object[]>("GetProinstMaters", new object[] { proistId }) };
            return (QueryHelper.List<WfProinstMater, WfProinstMater, WfProinstMater, WfProinstMater>(typeof(WfProinstMater).Namespace, string.Empty, querys)[0] as IList<WfProinstMater>);
        }

        private static string[] GetQueryParameterValues(string[] queryParameters)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            SmIdentity identity = smPrincipal.Identity as SmIdentity;
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
                            goto Label_0066;
                        }
                        if (str == "ROLES")
                        {
                            goto Label_0076;
                        }
                    }
                    else
                    {
                        strArray[i] = identity.UserId;
                    }
                }
                continue;
            Label_0066:
                strArray[i] = StringHelper.LinkForQuery(smPrincipal.DeptIds);
                continue;
            Label_0076:
                strArray[i] = StringHelper.LinkForQuery(smPrincipal.RoleIds);
            }
            return strArray;
        }

        public static DataTable GetRs(string queryName, object[] vals)
        {
            DataTable table;
            try
            {
                table = QueryHelper.ExecuteSqlQuery(typeof(Prodef).Namespace, string.Empty, queryName, vals);
            }
            catch (Exception exception)
            {
                LoggingService.Error("查询数据发生错误:", exception);
                throw new WfException("Get Result of " + queryName + " have error", exception);
            }
            return table;
        }

        public static DataTable GetRs(string nameSpace, string queryName, object[] vals)
        {
            DataTable table;
            try
            {
                table = QueryHelper.ExecuteSqlQuery(nameSpace, string.Empty, queryName, vals);
            }
            catch (Exception exception)
            {
                LoggingService.Error("查询数据发生错误:", exception);
                throw new WfException("Get Result of " + queryName + " have error", exception);
            }
            return table;
        }

        public static IList<CStaff> GetStaffsOfParticipant(CParticipant part)
        {
            return OGMService.GetStaffs(part.ParticipantEntity.Type.ToUpper(), part.ParticipantEntity.IdValue);
        }

        public static IList<Actdef> GetToInteractionActdefs(Transition tran)
        {
            List<Actdef> list = new List<Actdef>(1);
            Actdef to = tran.To;
            if (to.Type == ActdefType.INTERACTION)
            {
                list.Add(to);
                return list;
            }
            foreach (Transition transition in to.Froms)
            {
                list.AddRange(GetToInteractionActdefs(transition));
            }
            return list;
        }

        public static IList<Transition> GetTrans(string prodefId, string actdefId)
        {
            Prodef prodef = Prodefs[prodefId];
            if (prodef == null)
            {
                throw new WfClientException("Cannot find the prodef :" + prodefId);
            }
            Actdef actdef = prodef.Actdefs[actdefId];
            if (!actdef.PassNeedInteraction)
            {
                return null;
            }
            return GetInteractionTrans(actdef);
        }

        private static string GetUnablePerms(StringBuilder sb, int count)
        {
            if (sb.Length > 0)
            {
                c__DisplayClass5 class3 = new c__DisplayClass5();
                List<string> unableFrames = new List<string>(StringHelper.Split(sb.ToString()));
                sb.Remove(0, sb.Length);
                class3.unableFrames = unableFrames;
                Predicate<string> match = null;
                c__DisplayClass5 CS_8__locals6 = class3;
                for (int i = unableFrames.Count - 1; i >= 0; i--)
                {
                    if (match == null)
                    {
                        match = delegate (string s) {
                            return s == CS_8__locals6.unableFrames[i];
                        };
                    }
                    if (unableFrames.FindAll(match).Count == count)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(unableFrames[i]);
                    }
                    unableFrames.Remove(unableFrames[i]);
                    i = unableFrames.Count - 1;
                }
            }
            return sb.ToString();
        }

        public static WorkItem GetWorkItem(DataRow row, string idField)
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
            if (idField != null)
            {
                item.Id = GetColumnValue(idField, row);
            }
            return item;
        }

        public static Dictionary<string, WorkItem> GetWorkItems(DataRow[] rows, string idField)
        {
            Dictionary<string, WorkItem> dictionary = new Dictionary<string, WorkItem>(rows.Length);
            foreach (DataRow row in rows)
            {
                WorkItem workItem = GetWorkItem(row, idField);
                dictionary.Add(workItem.Id, workItem);
            }
            return dictionary;
        }

        public static bool IsCanCreateProdef(string prodefId)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            string userId = (smPrincipal.Identity as SmIdentity).UserId;
            string key = string.Format("IsCanCreateProdef_{0}_{1}", userId, prodefId);
            IList<ProdefRow> list = DAOCacheService.Get(key) as IList<ProdefRow>;
            if (list == null)
            {
                string[] roleIds = smPrincipal.RoleIds;
                string[] deptIds = smPrincipal.DeptIds;
                if (roleIds.Length == 0)
                {
                    roleIds = new string[] { "''" };
                }
                if (deptIds.Length == 0)
                {
                    deptIds = new string[] { "''" };
                }
                list = QueryHelper.List<ProdefRow>(typeof(Prodef).Namespace, key, "IsCanCreateProdef", new string[] { "prodefId", "roles", "depts", "staff" }, new object[] { prodefId, roleIds, deptIds, userId });
                DAOCacheService.Put(key, list);
            }
            return (list.Count > 0);
        }

        public static bool IsMyAllDelegated
        {
            get
            {
                if (!isMyAllDelegated.HasValue)
                {
                    isMyAllDelegated = new bool?(WfcInstance.IsMyAllDelegated());
                }
                return isMyAllDelegated.Value;
            }
            set
            {
                isMyAllDelegated = new bool?(value);
            }
        }

        private static IList<ParticipantFormPermission> ParticipantFormPermissions
        {
            get
            {
                string key = "All_ParticipantFormPermission";
                IList<ParticipantFormPermission> list = DAOCacheService.Get(key) as IList<ParticipantFormPermission>;
                if (list == null)
                {
                    list = QueryHelper.List<ParticipantFormPermission>("ALL_ParticipantFormPermission");
                    DAOCacheService.Put(key, list);
                }
                return list;
            }
        }

        public static IDictionary<string, Prodef> Prodefs
        {
            get
            {
                IDictionary<string, Prodef> allProdefs = DAOCacheService.Get("ALL_Prodefs") as IDictionary<string, Prodef>;
                if (allProdefs == null)
                {
                    allProdefs = GetAllProdefs();
                    DAOCacheService.Put("ALL_Prodefs", allProdefs);
                }
                return allProdefs;
            }
        }

        public static WorkflowClient WfcInstance
        {
            get
            {
                return new WorkflowClient();
            }
        }
    }
}

