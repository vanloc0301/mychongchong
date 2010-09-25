namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Engine.Impl;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using SkyMap.Net.Workflow.XPDL;

    [Dao]
    public sealed class WorkflowClient : ContextBoundObject
    {
        [Transaction("SkyMap.Net.Workflow")]
        public void Abort(WorkItem workItem)
        {
            WfFactory.GetWfProcess(workItem.ProinstId).Abort();
            WfFactory.GetAbnormalAudit(workItem).Create(WfAbnormalType.ABROTED);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Abort(IDictionary<string, WorkItem> workItems)
        {
            foreach (WorkItem item in workItems.Values)
            {
                this.Abort(item);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void AbortDeleted(IDictionary<string, WorkItem> workItems)
        {
            this.ReleaseAbnormal(workItems, WfReleaseType.AbortDeleted);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void AbortDeleted(WorkItem workItem)
        {
            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>(1);
            workItems.Add(workItem.AbnormalAuditId, workItem);
            this.AbortDeleted(workItems);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Accept(IDictionary<string, WorkItem> workItems)
        {
            string[] workItemIds = this.GetWorkItemIds(workItems, "AssignId");
            IList<WfAssigninst> assigns = this.Dao.Query<WfAssigninst>("GetNotAcceptAssignment", new string[] { "assignIds" }, new object[] { workItemIds });
            this.Accept(assigns);
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void Accept(IList<WfAssigninst> assigns)
        {
            foreach (WfAssigninst assigninst in assigns)
            {
                WfFactory.GetWfAssignment(assigninst).Accept();
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void CallBack(WfResinst resInst)
        {
            WfFactory.GetWfResource(resInst).CallBack();
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void CallBack(IDictionary<string, WorkItem> workItems)
        {
            string[] workItemIds = this.GetWorkItemIds(workItems, "ActinstId");
            IList<WfResinst> list = this.Dao.Query<WfResinst>("GetWfResOfNotAssigned", new string[] { "resIds" }, new object[] { workItemIds });
            WorkItem workItem = null;
            List<string> list2 = new List<string>();
            foreach (WfResinst resinst in list)
            {
                if (!list2.Contains(resinst.FromActinstId))
                {
                    this.CallBack(resinst);
                    list2.Add(resinst.FromActinstId);
                }
                workItem = workItems[resinst.Id];
                WfFactory.GetAbnormalAudit(workItem).Create(WfAbnormalType.CALL_BACK);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void CancelAccept(IDictionary<string, WorkItem> workItems)
        {
            string[] workItemIds = this.GetWorkItemIds(workItems, "AssignId");
            IList<WfAssigninst> list = this.Dao.Query<WfAssigninst>("GetAcceptAssignment", new string[] { "assignIds" }, new object[] { workItemIds });
            WorkItem workItem = null;
            foreach (WfAssigninst assigninst in list)
            {
                this.CancelAccept(assigninst);
                workItem = workItems[assigninst.Id];
                WfFactory.GetAbnormalAudit(workItem).Create(WfAbnormalType.CANCEL_ACCEPTED);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void CancelAccept(WfAssigninst assignInst)
        {
            WfFactory.GetWfAssignment(assignInst).CancelAccept();
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void CancelDelegate()
        {
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            IList<WfAbnormalAuditInst> list = this.Dao.Query<WfAbnormalAuditInst>("GetMyDelegates", new object[] { smIdentity.UserId });
            string s = string.Empty;
            foreach (WfAbnormalAuditInst inst in list)
            {
                s = inst.AssignId;
                if (!StringHelper.IsNull(s))
                {
                    IList<AssignStatusType> list2 = this.Dao.Query<AssignStatusType>("GetAssignStatus", new object[] { s });
                    if (list2.Count > 0)
                    {
                        AssignStatusType type = list2[0];
                        if (type != AssignStatusType.Completed)
                        {
                            WfFactory.GetAbnormalAudit(inst).Release(WfReleaseType.Resumed);
                        }
                    }
                }
                else
                {
                    WfFactory.GetAbnormalAudit(inst).Release(WfReleaseType.Resumed);
                }
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void CancelMonitor(IDictionary<string, WorkItem> workItems)
        {
            this.SetLowPriority(workItems, 1, WfAbnormalType.MONITOR);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void CancelPress(IDictionary<string, WorkItem> workItems)
        {
            this.SetLowPriority(workItems, 2, WfAbnormalType.PRESS);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void ComleteDelete(IDictionary<string, WorkItem> workItems)
        {
            Dictionary<DAODataSet, string> deletes = new Dictionary<DAODataSet, string>();
            foreach (WorkItem item in workItems.Values)
            {
                if (!StringHelper.IsNull(item.ProinstId))
                {
                    Proinst proinst = this.Dao.Load(typeof(Proinst), item.ProinstId) as Proinst;
                    if (proinst == null)
                    {
                        continue;
                    }
                    string templetDataSetId = proinst.TempletDataSetId;
                    DAODataSet key = null;
                    if (!StringHelper.IsNull(templetDataSetId))
                    {
                        key = QueryHelper.Get<DAODataSet>("DAODataSet_" + templetDataSetId, templetDataSetId);
                    }
                    if ((StringHelper.IsNull(templetDataSetId) || (key == null)) && !StringHelper.IsNull(QueryHelper.Get<Prodef>("Prodef_" + proinst.ProdefId, proinst.ProdefId).DaoDataSetId))
                    {
                        key = key = QueryHelper.Get<DAODataSet>("DAODataSet_" + templetDataSetId, templetDataSetId);
                    }
                    if (key != null)
                    {
                        if (!deletes.ContainsKey(key))
                        {
                            deletes.Add(key, "'" + proinst.ProjectId + "'");
                        }
                        else
                        {
                            Dictionary<DAODataSet, string> dictionary2;
                            DAODataSet set2;
                            (dictionary2 = deletes)[set2 = key] = dictionary2[set2] + ",'" + proinst.ProjectId + "'";
                        }
                    }
                    this.Dao.Put(proinst, DAOType.DELETE);
                }
            }
            if (deletes.Count > 0)
            {
                DataHelper.Delete(deletes);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void CompleteWorkItem(WorkItem workItem)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("获取将设置完成的任务：{0}...", new object[] { workItem.AssignId });
            }
            WfAssigninst assignInst = this.Dao.Load(typeof(WfAssigninst), workItem.AssignId) as WfAssigninst;
            this.CompleteWorkItem(assignInst);
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void CompleteWorkItem(WfAssigninst assignInst)
        {
            IWfAssignment wfAssignment = WfFactory.GetWfAssignment(assignInst);
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("检查任务状态是不是在办状态...", new object[0]);
            }
            if (wfAssignment.Status != AssignStatusType.Accepted)
            {
                throw new AlreadyCompleteException("任务已不在在办状态，可能已被别人转出或被执行其它操作;");
            }
            wfAssignment.Complete();
        }

        [Transaction("SkyMap.Net.Workflow")]
        public string CreateWfProcess(string prodefId)
        {
            return this.CreateWfProcess(prodefId, 1)[0];
        }

        [Transaction("SkyMap.Net.Workflow")]
        public string[] CreateWfProcess(string prodefId, int num)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将创建业务定义ID为：{0}的新业务实例{1}个", new object[] { prodefId, num });
            }
            IWfProcessMgr wfProcessMgr = WfFactory.GetWfProcessMgr(QueryHelper.Get<Prodef>("Prodef_" + prodefId, prodefId));
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            if (smIdentity == null)
            {
                throw new WfException("Security exception:Cannot get identity");
            }
            string userId = smIdentity.UserId;
            string userName = smIdentity.UserName;
            string[] strArray = new string[num];
            string[] strArray2 = new string[num];
            string[] strArray3 = new string[] { "PROINST_ID" };
            List<string[]> list = new List<string[]>(num);
            for (int i = 0; i < num; i++)
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("开始新建第一个业务...", new object[0]);
                }
                IWfRequester wfRequester = WfFactory.GetWfRequester(userId, userName);
                IWfProcess process = wfProcessMgr.CreateProcess(wfRequester);
                process.Start();
                Proinst instanceObject = process.GetInstanceObject() as Proinst;
                ProinstWorkMem mem = new ProinstWorkMem();
                mem.Id = instanceObject.Id;
                this.Dao.Put(mem, DAOType.SAVE);
                strArray2[i] = process.ProjectID;
                strArray[i] = process.Key;
                list.Add(new string[] { process.Key });
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("业务创建完毕，将提交事物...", new object[0]);
            }
            return strArray;
        }

        [Transaction("SkyMap.Net.Workflow")]
        public string[] CreateWfProcessAndAccept(string prodefId, int num)
        {
            CallContext.SetData("CreateAndAccept", true);
            string[] strArray = this.CreateWfProcess(prodefId, num);
            CallContext.FreeNamedDataSlot("CreateAndAccept");
            return strArray;
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Delegate(IDictionary<string, WorkItem> workItems)
        {
            IList<WfAssigninst> myAcceptedAssigns = null;
            if ((workItems != null) && (workItems.Count > 0))
            {
                string[] workItemIds = this.GetWorkItemIds(workItems, "AssignId");
                myAcceptedAssigns = this.Dao.Query<WfAssigninst>("GetAcceptAssignmentAndNotDelegated", new string[] { "assignIds" }, new object[] { workItemIds });
            }
            else
            {
                if (this.IsMyAllDelegated())
                {
                    return;
                }
                myAcceptedAssigns = this.GetMyAcceptedAssigns();
                WfFactory.GetAbnormalAudit().Create(WfAbnormalType.ALL_DELEGATED);
            }
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            foreach (WfAssigninst assigninst in myAcceptedAssigns)
            {
                if (assigninst.StaffId != smIdentity.UserId)
                {
                    throw new CannotRepeatDelegateException();
                }
                WfFactory.GetWfAssignment(assigninst).SetDelegate();
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        private IList<IWfAbnormalAudit> GetAbnormalAudits(IDictionary<string, WorkItem> workItems)
        {
            List<IWfAbnormalAudit> list = new List<IWfAbnormalAudit>(workItems.Count);
            string[] workItemIds = this.GetWorkItemIds(workItems, "AbnormalAuditId");
            IList<WfAbnormalAuditInst> list2 = this.Dao.Query<WfAbnormalAuditInst>("GetAbnormalAuditInstOfNotRelease", new string[] { "AbnormalIds" }, new object[] { workItemIds });
            foreach (WfAbnormalAuditInst inst in list2)
            {
                list.Add(WfFactory.GetAbnormalAudit(inst));
            }
            return list;
        }

        [NoDao]
        public DataTable GetHolidays()
        {
            return QueryHelper.ExecuteSqlQuery("Default", "WF_HOLIDAYS", "GetHolidays");
        }

        [Transaction("SkyMap.Net.Workflow")]
        private IList<WfAssigninst> GetMyAcceptedAssigns()
        {
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            return this.Dao.Query<WfAssigninst>("GetMyWorkItems", new object[] { smIdentity.UserId });
        }

        public Proinst GetProinst(string processKey)
        {
            try
            {        

            }
            catch (Exception exception)
            {
                throw new WfException("Query object have error", exception);
            }
            Proinst proinst = this.Dao.Load<Proinst>(processKey);
            int count = proinst.WfAbnormalAudits.Count;
            foreach (Actinst actinst in proinst.Actinsts)
            {
                foreach (WfAssigninst assigninst in actinst.Assigns)
                {
                    count = assigninst.WfAbnormalAudits.Count;
                }
            }
            return proinst;           
        }

        [NoDao]
        public DateTime GetServerTime()
        {
            DataTable table = QueryHelper.ExecuteSqlQuery("SkyMap.Net.Core", string.Empty, "GetServerTime");
            if (table.Rows.Count > 0)
            {
                return (DateTime) table.Rows[0][0];
            }
            return DateTimeHelper.Null;
        }

        private string[] GetWorkItemIds(IDictionary<string, WorkItem> workItems, string idName)
        {
            PropertyInfo property = typeof(WorkItem).GetProperty(idName);
            if (property == null)
            {
                throw new WfException("Cannot know the property of workitem : " + idName);
            }
            List<string> list = new List<string>();
            foreach (WorkItem item in workItems.Values)
            {
                list.Add((string) property.GetValue(item, null));
            }
            return list.ToArray();
        }

        [NoDao]
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }

        [NoDao]
        public bool IsMyAllDelegated()
        {
            IList<string> list;
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            try
            {
                list = QueryHelper.List<string>(typeof(Proinst).Namespace, string.Empty, "GetMyAllDelegated", new object[] { smIdentity.UserId });
            }
            catch (Exception exception)
            {
                throw new WfException("Get delegate error:\r" + exception.Message, exception);
            }
            return (list.Count > 0);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Monitor(IDictionary<string, WorkItem> workItems)
        {
            this.SetHighPriority(workItems, 1, WfAbnormalType.MONITOR);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Press(IDictionary<string, WorkItem> workItems)
        {
            this.SetHighPriority(workItems, 2, WfAbnormalType.PRESS);
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void ReleaseAbnormal(IDictionary<string, WorkItem> workItems, WfReleaseType type)
        {
            IList<IWfAbnormalAudit> abnormalAudits = this.GetAbnormalAudits(workItems);
            foreach (IWfAbnormalAudit audit in abnormalAudits)
            {
                if (!audit.Released)
                {
                    if (type == WfReleaseType.Resumed)
                    {
                        WfFactory.GetWfProcess(audit.ProcessKey).Resume();
                    }
                    audit.Release(type);
                }
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Resume(IDictionary<string, WorkItem> workItems)
        {
            this.ReleaseAbnormal(workItems, WfReleaseType.Resumed);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Resume(WorkItem workItem)
        {
            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>(1);
            workItems.Add(workItem.AbnormalAuditId, workItem);
            this.Resume(workItems);
        }

        [NoDao, Transaction("SkyMap.Net.Workflow")]
        public void SaveAsProinst(string[] processKeys, string url)
        {
            UnitOfWork work = new UnitOfWork(typeof(Proinst), url);
            List<WfRequesterinst> list = new List<WfRequesterinst>();
            foreach (string str in processKeys)
            {
                Proinst proinst = this.Dao.Load<Proinst>(str);
                if (!((proinst.Requester == null) || list.Contains(proinst.Requester)))
                {
                    list.Add(proinst.Requester);
                }
                work.RegisterNew(proinst);
                if (proinst.ProinstWorkMem != null)
                {
                    work.RegisterNew(proinst.ProinstWorkMem);
                }
                foreach (Actinst actinst in proinst.Actinsts)
                {
                    work.RegisterNew(actinst);
                    if (actinst.WfResinst != null)
                    {
                        work.RegisterNew(actinst.WfResinst);
                    }
                    foreach (WfAssigninst assigninst in actinst.Assigns)
                    {
                        work.RegisterNew(assigninst);
                    }
                    foreach (WfRouteInst inst in actinst.RouteFromMe)
                    {
                        work.RegisterNew(inst);
                    }
                }
                foreach (WfAbnormalAuditInst inst2 in proinst.WfAbnormalAudits)
                {
                    work.RegisterNew(inst2);
                }
                foreach (WfStateEventAuditInst inst3 in proinst.StateEvents)
                {
                    work.RegisterNew(inst3);
                }
                foreach (WfAdjunct adjunct in proinst.WfAdjuncts)
                {
                    work.RegisterNew(adjunct);
                    WfAdjunctFile file = this.Dao.Load<WfAdjunctFile>(adjunct.Id);
                    if (file != null)
                    {
                        work.RegisterDirty(file);
                    }
                }
                foreach (WfProinstMater mater in proinst.Maters)
                {
                    work.RegisterNew(mater);
                }
                foreach (WfStaffNotion notion in proinst.StaffNotions)
                {
                    work.RegisterNew(notion);
                }
            }
            work.Commit();
            foreach (WfRequesterinst requesterinst in list)
            {
                try
                {
                    work.RegisterNew(requesterinst);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void SendBack(IDictionary<string, WorkItem> workItems)
        {
            IWfAssignment wfAssignment = WfFactory.GetWfAssignment(WfUtil.GetAbnormalContextData().ReceiveAssignId);
            string actdefId = wfAssignment.Activity.ActdefId;
            foreach (WorkItem item in workItems.Values)
            {
                this.SendBack(item, wfAssignment, actdefId);
                wfAssignment = null;
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void SendBack(WorkItem workItem, IWfAssignment sendBackAssign, string sendBackActdefId)
        {
            if (sendBackAssign == null)
            {
                IList<WfAssigninst> list = this.Dao.Query<WfAssigninst>("GetSendBackToAssign", new object[] { workItem.ProinstId, sendBackActdefId });
                if (list.Count == 0)
                {
                    throw new CannotSendBackException("Cannot find the send to assignment for:" + workItem.AssignId + "," + workItem.ProinstName);
                }
                if (list.Count > 1)
                {
                    throw new CannotSendBackException("Cannot batch send back the assignment:" + workItem.AssignId);
                }
                sendBackAssign = WfFactory.GetWfAssignment(list[0]);
            }
            IWfAssignment wfAssignment = WfFactory.GetWfAssignment(workItem.AssignId);
            if ((wfAssignment.AbnormalStatus == WfAbnormalType.NO_ABNORMAL) || (wfAssignment.AbnormalStatus == WfAbnormalType.SEND_BACKING))
            {
                wfAssignment.SendBack(sendBackAssign);
                WfFactory.GetAbnormalAudit(workItem).Create(WfAbnormalType.SEND_BACKED);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void SetHighPriority(IDictionary<string, WorkItem> workItems, short priority, WfAbnormalType abnormalType)
        {
            foreach (WorkItem item in workItems.Values)
            {
                if (!StringHelper.IsNull(item.ProinstId))
                {
                    Proinst proinst = this.Dao.Load(typeof(Proinst), item.ProinstId) as Proinst;
                    if (proinst.Status == WfStatusType.WF_RUNNING)
                    {
                        if ((proinst.Priority & priority) == priority)
                        {
                            break;
                        }
                        proinst.Priority = Convert.ToInt16((int) (proinst.Priority | priority));
                        this.Dao.Put(proinst, DAOType.UPDATE);
                        WfAbnormalAudit audit = new WfAbnormalAudit();
                        audit.CreateAbnormalAudit(proinst);
                        audit.Create(abnormalType);
                    }
                }
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        private void SetLowPriority(IDictionary<string, WorkItem> workItems, short priority, WfAbnormalType abnormalType)
        {
            foreach (WorkItem item in workItems.Values)
            {
                if (!StringHelper.IsNull(item.ProinstId))
                {
                    SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                    IList<WfAbnormalAuditInst> list = this.Dao.Query<WfAbnormalAuditInst>("GetProinstNotReleaseAblInst", new object[] { item.ProinstId, abnormalType, smIdentity.UserId });
                    if (list.Count == 1)
                    {
                        Proinst proinst = this.Dao.Load(typeof(Proinst), item.ProinstId) as Proinst;
                        proinst.Priority = Convert.ToInt16((int) (proinst.Priority & priority));
                        this.Dao.Put(proinst, DAOType.UPDATE);
                        WfAbnormalAuditInst inst = list[0];
                        inst.ReleaseStaffId = smIdentity.UserId;
                        inst.ReleaseStaffName = smIdentity.UserName;
                        inst.ReleaseTime = new DateTime?(DateTimeHelper.GetNow());
                        inst.ReleaseType = WfReleaseType.Resumed;
                        this.Dao.Put(inst, DAOType.UPDATE);
                    }
                }
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Suspend(IDictionary<string, WorkItem> workItems)
        {
            foreach (WorkItem item in workItems.Values)
            {
                this.Suspend(item);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Suspend(WorkItem workItem)
        {
            WfFactory.GetWfProcess(workItem.ProinstId).Suspend();
            WfFactory.GetAbnormalAudit(workItem).Create(WfAbnormalType.SUSPENDED);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Terminate(IDictionary<string, WorkItem> workItems)
        {
            foreach (WorkItem item in workItems.Values)
            {
                this.Terminate(item);
            }
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void Terminate(WorkItem workItem)
        {
            WfFactory.GetWfProcess(workItem.ProinstId).Terminate();
            WfFactory.GetAbnormalAudit(workItem).Create(WfAbnormalType.TERMINATED);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void TerminateCompleted(IDictionary<string, WorkItem> workItems)
        {
            this.ReleaseAbnormal(workItems, WfReleaseType.TerminateCompleted);
        }

        [Transaction("SkyMap.Net.Workflow")]
        public void TerminateCompleted(WorkItem workItem)
        {
            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>();
            workItems.Add(workItem.AbnormalAuditId, workItem);
            this.TerminateCompleted(workItems);
        }

        private IDA0 Dao
        {
            get
            {
                return DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            }
        }
    }
}

