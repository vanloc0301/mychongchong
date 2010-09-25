namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Evaluant;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Serializable]
    public class WfActivity : WfExecutionOjbect, IWfActivity, IWfExecutionObject
    {
        private Actinst actinst;
        private WfActivityAbstractImpl executor;
        private IWfProcess process;

        public WfActivity(Actinst actins) : base(actins)
        {
            this.actinst = actins;
            this.process = WfFactory.GetWfProcess(this.actinst.Proinst);
            this.executor = WfActivityAbstractImplFact.GetConcretImpl(this);
        }

        public WfActivity(Actdef actdef, IWfProcess process) : base(new Actinst(), actdef)
        {
            this.actinst = base.wfInstance as Actinst;
            this.process = process;
            this.actinst.Proinst = (Proinst) process.GetInstanceObject();
            this.actinst.ActdefId = actdef.Id;
            this.actinst.ActdefName = actdef.Name;
            this.actinst.DueTime = actdef.Limit;
            this.actinst.Name = actdef.Name;
            this.actinst.Type = actdef.Type;
            this.actinst.FromCount = 0;
            this.actinst.ToCount = 0;
            this.executor = WfActivityAbstractImplFact.GetConcretImpl(this);
        }

        public override void Abort()
        {
            if (base.WfState == WfStateType.Open)
            {
                base.Abort();
                this.SetAssignAbnormaled(WfAbnormalType.ABROTED);
            }
        }

        public void Activate()
        {
            this.executor.Activate();
        }

        public override void ChangeStatus(WfStatusType newstatus)
        {
            IWfStateEventAudit audit = new WfStateEventAudit(this, this.actinst.Status, newstatus);
            base.ChangeStatus(newstatus);
        }

        internal bool CheckCondition(SkyMap.Net.Workflow.XPDL.Condition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将验证类型：{0}的条件", new object[] { condition.Type });
            }
            string type = condition.Type;
            if (type != null)
            {
                WfLogicalPassContextData passContextData;
                DataTable table;
                int num;
                if ((type == "SQL") || (type == "AUTOROUTE"))
                {
                    string str;
                    return XMLSQLConditionsHelper.ParserAndEval(this.ReplaceSystemParams(condition.Xpression), out str);
                }
                if (type == "EXCLUDESTAFFSQL")
                {
                    if (!string.IsNullOrEmpty(condition.Xpression))
                    {
                        passContextData = WfUtil.GetPassContextData();
                        if (passContextData != null)
                        {
                            table = QueryHelper.ExecuteSql("Default", string.Empty, this.ReplaceSystemParams(condition.Xpression));
                            if ((table != null) && (table.Rows.Count > 0))
                            {
                                passContextData.ExcludeStaffId = string.Empty;
                                for (num = 0; num < table.Rows.Count; num++)
                                {
                                    passContextData.ExcludeStaffId = passContextData.ExcludeStaffId + table.Rows[num][0].ToString() + ",";
                                }
                                WfUtil.SetPassContextData(passContextData);
                            }
                            else
                            {
                                LoggingService.WarnFormatted("业务：{0}环节：{1}({2})不能从自动获取下一步要排除的经办人", new object[] { this.actinst.Proinst.ProjectId, this.actinst.Id, this.actinst.ActdefName });
                            }
                        }
                    }
                }
                else if ((type == "AUTOSTAFFSQL") && !string.IsNullOrEmpty(condition.Xpression))
                {
                    passContextData = WfUtil.GetPassContextData();
                    if ((passContextData != null) && ((passContextData.ToStaffIds == null) || (passContextData.ToStaffIds.Length == 0)))
                    {
                        table = QueryHelper.ExecuteSql("Default", string.Empty, this.ReplaceSystemParams(condition.Xpression));
                        if ((table != null) && (table.Rows.Count > 0))
                        {
                            int count = table.Rows.Count;
                            passContextData.ToStaffIds = new string[count];
                            passContextData.ToStaffNames = new string[count];
                            for (num = 0; num < count; num++)
                            {
                                passContextData.ToStaffIds[num] = table.Rows[num]["staff_id"].ToString();
                                passContextData.ToStaffNames[num] = table.Rows[num]["staff_name"].ToString();
                            }
                            WfUtil.SetPassContextData(passContextData);
                        }
                        else
                        {
                            LoggingService.WarnFormatted("业务：{0}环节：{1}({2})不能从自动获取下一步经办人的语句中获取下一步经办人", new object[] { this.actinst.Proinst.ProjectId, this.actinst.Id, this.actinst.ActdefName });
                            passContextData.ToStaffIds = new string[0];
                            passContextData.ToStaffNames = new string[0];
                        }
                    }
                }
            }
            return true;
        }

        public void Complete()
        {
            if ((base.WfState == WfStateType.Closed) && (base.HowClosed == HowClosedType.Completed))
            {
                throw new AlreadyCompleteException("任务已经完成！");
            }
            this.executor.Complete();
            this.Container.ActivityComplete(this);
        }

        public override object GetDefineOject()
        {
            return QueryHelper.Get<Prodef>("Prodef_" + base.ProdefId, base.ProdefId).Actdefs[this.ActdefId];
        }

        private IList<Transition> GetTransFrom(Actdef fromActdef)
        {
            IList<Transition> froms;
            try
            {
                froms = fromActdef.Froms;
            }
            catch (ApplicationException exception)
            {
                throw new WfException(exception.Message, exception);
            }
            return froms;
        }

        internal void QueueNext()
        {
            if (!(this.executor is WfActivityCompletionImpl))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("将创建下一步活动的工作...", new object[0]);
                }
                Actdef defineOject = this.GetDefineOject() as Actdef;
                IList<Transition> transFrom = this.GetTransFrom(defineOject);
                this.executor.Pass(transFrom);
            }
        }

        private string ReplaceSystemParams(string xpression)
        {
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            return xpression.Replace("{PROJECT_ID}", this.actinst.Proinst.ProjectId).Replace("{PROINST_ID}", this.actinst.Proinst.Id).Replace("{ACTINST_ID}", this.actinst.Id).Replace("{ACTDEF_ID}", this.actinst.ActdefId).Replace("{STAFF_ID}", smIdentity.UserId).Replace("{STAFF_NAME}", smIdentity.UserName);
        }

        public override void Resume()
        {
            if (base.CanResume())
            {
                base.Resume();
                foreach (WfAssigninst assigninst in this.actinst.WfResinst.Assigns)
                {
                    WfFactory.GetWfAssignment(assigninst).Resume();
                }
            }
        }

        private void SetAssignAbnormaled(WfAbnormalType abnormal)
        {
            foreach (WfAssigninst assigninst in this.actinst.WfResinst.Assigns)
            {
                WfFactory.GetWfAssignment(assigninst).SetAbnormal(abnormal);
            }
        }

        public void Start()
        {
            base.Store();
            this.executor.Start();
        }

        public override void Suspend()
        {
            if (base.WfState == WfStateType.Open)
            {
                base.Suspend();
                this.SetAssignAbnormaled(WfAbnormalType.SUSPENDED);
            }
        }

        public override void Terminate()
        {
            if (base.WfState == WfStateType.Open)
            {
                base.Terminate();
                this.SetAssignAbnormaled(WfAbnormalType.TERMINATED);
            }
        }

        public string ActdefId
        {
            get
            {
                return this.actinst.ActdefId;
            }
        }

        public IWfProcess Container
        {
            get
            {
                return this.process;
            }
        }

        public ActdefType Type
        {
            get
            {
                return this.actinst.Type;
            }
        }
    }
}

