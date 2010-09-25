namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using System;

    public class WfAbnormalAudit : IWfAbnormalAudit, IWfEventAudit
    {
        private WfAbnormalAuditInst abnormalAuditInst;
        private IDA0 dao;

        public WfAbnormalAudit()
        {
            this.dao = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            this.abnormalAuditInst = new WfAbnormalAuditInst();
            this.abnormalAuditInst.TimeStamp = new DateTime?(DateTimeHelper.GetNow());
        }

        public WfAbnormalAudit(WorkItem workItem)
        {
            this.dao = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            string abnormalAuditId = workItem.AbnormalAuditId;
            if (StringHelper.IsNull(abnormalAuditId))
            {
                this.CreateAbnormalAudit(workItem);
            }
            else
            {
                this.abnormalAuditInst = this.dao.Load(typeof(WfAbnormalAuditInst), abnormalAuditId) as WfAbnormalAuditInst;
                if (this.abnormalAuditInst == null)
                {
                    throw new WfException("Cannot load the AbnormalAudit:" + workItem.AbnormalAuditId);
                }
            }
        }

        public WfAbnormalAudit(WfAbnormalAuditInst instance)
        {
            this.dao = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            this.abnormalAuditInst = instance;
        }

        public WfAbnormalAudit(WfAssigninst assign)
        {
            this.dao = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            this.abnormalAuditInst = new WfAbnormalAuditInst();
            this.abnormalAuditInst.AssignId = assign.Id;
            this.abnormalAuditInst.ProinstId = assign.WfResinst.Actinst.Proinst.Id;
            this.abnormalAuditInst.ProinstName = assign.WfResinst.Actinst.Proinst.Name;
            this.abnormalAuditInst.ActinstId = assign.WfResinst.Id;
            this.abnormalAuditInst.ActinstName = assign.WfResinst.Actinst.Name;
            this.abnormalAuditInst.TimeStamp = new DateTime?(DateTimeHelper.GetNow());
            this.abnormalAuditInst.NeedDecision = false;
            this.abnormalAuditInst.OpStaffId = assign.StaffId;
            this.abnormalAuditInst.OpStaffName = assign.StaffName;
        }

        public void Create(WfAbnormalType type)
        {
            this.abnormalAuditInst.Type = type;
            this.abnormalAuditInst.ReleaseType = WfReleaseType.NotReleased;
            this.SetContextData();
            this.Store();
        }

        public void CreateAbnormalAudit(WorkItem workItem)
        {
            this.abnormalAuditInst = new WfAbnormalAuditInst();
            this.abnormalAuditInst.ActinstId = workItem.ActinstId;
            this.abnormalAuditInst.ActinstName = workItem.ActinstName;
            this.abnormalAuditInst.ProinstId = workItem.ProinstId;
            this.abnormalAuditInst.ProinstName = workItem.ProinstName;
            this.abnormalAuditInst.AssignId = workItem.AssignId;
            this.abnormalAuditInst.TimeStamp = new DateTime?(DateTimeHelper.GetNow());
            this.abnormalAuditInst.NeedDecision = false;
        }

        public void CreateAbnormalAudit(Proinst proinst)
        {
            this.abnormalAuditInst.ProinstId = proinst.Id;
            this.abnormalAuditInst.ProinstName = proinst.Name;
        }

        public void Decision(WfDecisionStatusType status)
        {
            this.abnormalAuditInst.DecisionStatus = status;
            this.abnormalAuditInst.DecisionTime = new DateTime?(DateTimeHelper.GetNow());
            WfLogicalAbnormalContextData abnormalContextData = WfUtil.GetAbnormalContextData();
            if (abnormalContextData != null)
            {
                this.abnormalAuditInst.DecisionMemo = abnormalContextData.DecisionMemo;
            }
            this.Store();
        }

        public void Release(WfReleaseType releaseType)
        {
            this.abnormalAuditInst.ReleaseTime = new DateTime?(DateTimeHelper.GetNow());
            this.abnormalAuditInst.ReleaseType = releaseType;
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            if (smIdentity != null)
            {
                this.abnormalAuditInst.ReleaseStaffId = smIdentity.UserId;
                this.abnormalAuditInst.ReleaseStaffName = smIdentity.UserName;
            }
            this.Store();
        }

        private void SetContextData()
        {
            if (StringHelper.IsNull(this.abnormalAuditInst.OpStaffId))
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                this.abnormalAuditInst.OpStaffId = smIdentity.UserId;
                this.abnormalAuditInst.OpStaffName = smIdentity.UserName;
            }
            WfLogicalAbnormalContextData abnormalContextData = WfUtil.GetAbnormalContextData();
            if (abnormalContextData != null)
            {
                this.abnormalAuditInst.OpReason = abnormalContextData.OpReason;
                this.abnormalAuditInst.ReceiveStaffId = abnormalContextData.ReceiveStaffId;
                this.abnormalAuditInst.ReceiveStaffName = abnormalContextData.ReceiveStaffName;
                this.abnormalAuditInst.ReceiveActinstId = abnormalContextData.ReceiveActinstId;
                this.abnormalAuditInst.ReceiveActinstName = abnormalContextData.ReceiveActinstName;
                this.abnormalAuditInst.ReceiveAssignId = abnormalContextData.ReceiveAssignId;
                if (abnormalContextData.DecisionNeed)
                {
                    this.abnormalAuditInst.NeedDecision = true;
                    this.abnormalAuditInst.DecisionStaffId = abnormalContextData.DecisionStaffId;
                    this.abnormalAuditInst.DecisionStaffName = abnormalContextData.DecisionStaffName;
                    this.abnormalAuditInst.DecisionStatus = WfDecisionStatusType.ON;
                }
            }
        }

        private void Store()
        {
            if (StringHelper.IsNull(this.abnormalAuditInst.Id))
            {
                this.abnormalAuditInst.Id = StringHelper.GetNewGuid();
                this.dao.Put(this.abnormalAuditInst, DAOType.SAVE);
            }
            else
            {
                this.dao.Put(this.abnormalAuditInst, DAOType.UPDATE);
            }
        }

        public string ActivityKey
        {
            get
            {
                return this.abnormalAuditInst.ActinstId;
            }
        }

        public string ActivityName
        {
            get
            {
                return this.abnormalAuditInst.ActinstName;
            }
        }

        public bool Decisioned
        {
            get
            {
                if (this.abnormalAuditInst.NeedDecision && (this.abnormalAuditInst.DecisionStatus != WfDecisionStatusType.ON))
                {
                    return false;
                }
                return true;
            }
        }

        public string EventType
        {
            get
            {
                return this.abnormalAuditInst.Type.ToString();
            }
        }

        public string ProcessKey
        {
            get
            {
                return this.abnormalAuditInst.ProinstId;
            }
        }

        public string ProcessName
        {
            get
            {
                return this.abnormalAuditInst.ProinstName;
            }
        }

        public bool Released
        {
            get
            {
                return (this.abnormalAuditInst.ReleaseType != WfReleaseType.NotReleased);
            }
        }

        public IWfExecutionObject Source
        {
            get
            {
                if (!StringHelper.IsNull(this.ActivityKey))
                {
                    return WfFactory.GetWfActivity(this.ActivityKey);
                }
                if (!StringHelper.IsNull(this.ProcessKey))
                {
                    return WfFactory.GetWfProcess(this.ProcessKey);
                }
                return null;
            }
        }

        public DateTime? TimeStamp
        {
            get
            {
                return this.abnormalAuditInst.TimeStamp;
            }
        }
    }
}

