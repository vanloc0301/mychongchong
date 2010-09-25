namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using System;

    [Serializable]
    public class WfAssignment : IWfAssignment
    {
        private WfAssigninst wfAssigninst;
        private IWfResource wfResource;

        public WfAssignment(IWfResource wfresource) : this(wfresource, null, null)
        {
        }

        public WfAssignment(WfAssigninst wfassigninst)
        {
            this.wfAssigninst = wfassigninst;
            this.wfResource = WfFactory.GetWfResource(this.wfAssigninst.WfResinst);
        }

        public WfAssignment(IWfResource wfresource, string staffid, string staffname)
        {
            this.wfResource = wfresource;
            this.wfAssigninst = new WfAssigninst();
            this.wfAssigninst.WfResinst = wfresource.GetWfResinst();
            this.wfAssigninst.WfResinst.Assigns.Add(this.wfAssigninst);
            this.wfAssigninst.AbnormalStatus = WfAbnormalType.NO_ABNORMAL;
            this.wfAssigninst.FromDate = new DateTime?(DateTimeHelper.GetNow());
            this.wfAssigninst.StaffId = staffid;
            this.wfAssigninst.StaffName = staffname;
            this.ChangeStatus(AssignStatusType.Not_Accepted);
        }

        public void Accept()
        {
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            if (!(StringHelper.IsNull(this.wfAssigninst.StaffId) || !(this.wfAssigninst.StaffId != smIdentity.UserId)))
            {
                WfLogicalAbnormalContextData abnormalData = new WfLogicalAbnormalContextData();
                abnormalData.ReceiveStaffId = smIdentity.UserId;
                abnormalData.ReceiveStaffName = smIdentity.UserName;
                WfUtil.SetAbnormalContextData(abnormalData);
                this.SetDelegate();
                WfUtil.FreeAbnormalContextData();
            }
            if (StringHelper.IsNull(this.wfAssigninst.StaffId))
            {
                this.wfAssigninst.StaffId = smIdentity.UserId;
                this.wfAssigninst.StaffName = smIdentity.UserName;
            }
            this.wfAssigninst.AcceptDate = new DateTime?(DateTimeHelper.GetNow());
            this.ChangeStatus(AssignStatusType.Accepted);
            this.wfResource.Assign();
            this.Store();
        }

        public void CancelAccept()
        {
            WfResinst wfResinst = this.wfResource.GetWfResinst();
            if ((wfResinst.AssignRule != AssignRuleType.ALL) && StringHelper.IsNull(wfResinst.StaffId))
            {
                this.wfAssigninst.StaffId = string.Empty;
                this.wfAssigninst.StaffName = string.Empty;
            }
            this.ChangeStatus(AssignStatusType.Not_Accepted);
            wfResinst.IsAssigned = false;
            bool flag = true;
            foreach (WfAssigninst assigninst in wfResinst.Assigns)
            {
                if (assigninst.Status == AssignStatusType.Accepted)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                Actinst actinst = wfResinst.Actinst;
                actinst.StartDate = null;
                actinst.Status = WfStatusType.WF_NOT_STARTED;
            }
            this.Store();
        }

        public void ChangeStatus(AssignStatusType status)
        {
            this.SendEMail(this.wfAssigninst.Status, status);
            this.wfAssigninst.Status = status;
        }

        public void ChangeStatus(WfAbnormalType abnormal)
        {
            this.wfAssigninst.AbnormalStatus = abnormal;
            if (abnormal == WfAbnormalType.SEND_BACKED)
            {
                WfMessageHelper.Send(this.wfAssigninst.StaffId, this.wfAssigninst.WfResinst.Actinst.Proinst.ProjectId, abnormal);
            }
        }

        public void Complete()
        {
            this.wfAssigninst.ToDate = new DateTime?(DateTimeHelper.GetNow());
            this.ChangeStatus(AssignStatusType.Completed);
            if (this.wfResource.IsAssigned)
            {
                this.wfResource.WorkItemCompleted();
            }
            this.Store();
        }

        private bool ExistAssigninst(WfResinst wfresinst, string staffid, string staffname)
        {
            return false;
        }

        public object GetInstanceObject()
        {
            return this.wfAssigninst;
        }

        public void Resume()
        {
            if ((this.Status == AssignStatusType.Abnormaled) && (((this.wfAssigninst.AbnormalStatus == WfAbnormalType.SUSPENDED) || (this.wfAssigninst.AbnormalStatus == WfAbnormalType.ABROTED)) || (this.wfAssigninst.AbnormalStatus == WfAbnormalType.TERMINATED)))
            {
                this.ChangeStatus(WfAbnormalType.NO_ABNORMAL);
                if (!this.wfAssigninst.AcceptDate.HasValue)
                {
                    this.ChangeStatus(AssignStatusType.Not_Accepted);
                }
                else
                {
                    this.ChangeStatus(AssignStatusType.Accepted);
                }
            }
        }

        public void SendBack(IWfAssignment sendBackAssign)
        {
            WfLogicalAbnormalContextData abnormalContextData = WfUtil.GetAbnormalContextData();
            abnormalContextData.ReceiveAssignId = sendBackAssign.Key;
            abnormalContextData.ReceiveActinstId = sendBackAssign.Activity.Key;
            abnormalContextData.ReceiveActinstName = sendBackAssign.Activity.Name;
            abnormalContextData.ReceiveStaffId = sendBackAssign.StaffId;
            abnormalContextData.ReceiveStaffName = sendBackAssign.StaffName;
            if (sendBackAssign.Status != AssignStatusType.Abnormaled)
            {
                sendBackAssign.ChangeStatus(AssignStatusType.Abnormaled);
                sendBackAssign.ChangeStatus(WfAbnormalType.SEND_BACKING);
            }
            else
            {
                sendBackAssign.ChangeStatus(AssignStatusType.Accepted);
                sendBackAssign.ChangeStatus(WfAbnormalType.NO_ABNORMAL);
            }
            sendBackAssign.Store();
            if (this.wfAssigninst.AbnormalStatus == WfAbnormalType.NO_ABNORMAL)
            {
                this.ChangeStatus(WfAbnormalType.SEND_BACKED);
                this.ChangeStatus(AssignStatusType.Abnormaled);
            }
            if (this.wfAssigninst.AbnormalStatus == WfAbnormalType.SEND_BACKING)
            {
                this.ChangeStatus(AssignStatusType.Completed);
                this.ChangeStatus(WfAbnormalType.NO_ABNORMAL);
            }
            this.Store();
        }

        private void SendEMail(AssignStatusType oldstatus, AssignStatusType newstatus)
        {
            if (!StringHelper.IsNull(this.wfAssigninst.StaffId))
            {
                WfMessageHelper.Send(this.wfAssigninst.StaffId, this.wfAssigninst.WfResinst.Actinst.Proinst.ProjectId, oldstatus, newstatus);
            }
            else
            {
                IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
                CParticipant participant = QueryHelper.Get<CParticipant>("CParticipant_" + this.wfAssigninst.WfResinst.ParticipantId, this.wfAssigninst.WfResinst.ParticipantId);
                WfMessageHelper.Send(participant.ParticipantEntity.Type, participant.ParticipantEntity.IdValue, this.wfAssigninst.WfResinst.Actinst.Proinst.ProjectId, oldstatus, newstatus);
            }
        }

        public void SetAbnormal(WfAbnormalType abnormal)
        {
            if ((this.Status == AssignStatusType.Accepted) || (this.Status == AssignStatusType.Not_Accepted))
            {
                this.ChangeStatus(AssignStatusType.Abnormaled);
                this.ChangeStatus(abnormal);
                this.Store();
            }
        }

        public void SetDelegate()
        {
            WfFactory.GetAbnormalAudit(this.wfAssigninst).Create(WfAbnormalType.DELEGATED);
        }

        public void Store()
        {
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            if (StringHelper.IsNull(this.wfAssigninst.Id))
            {
                this.wfAssigninst.Id = StringHelper.GetNewGuid();
                daoInstance.Put(this.wfAssigninst, DAOType.SAVE);
            }
            else
            {
                daoInstance.Put(this.wfAssigninst, DAOType.UPDATE);
            }
        }

        public WfAbnormalType AbnormalStatus
        {
            get
            {
                return this.wfAssigninst.AbnormalStatus;
            }
        }

        public IWfActivity Activity
        {
            get
            {
                return this.wfResource.WfActivity;
            }
        }

        public IWfResource Assignee
        {
            get
            {
                return this.wfResource;
            }
        }

        public DateTime? FromDate
        {
            get
            {
                return this.wfAssigninst.FromDate;
            }
        }

        public string Key
        {
            get
            {
                return this.wfAssigninst.Id;
            }
        }

        public IWfProcess Process
        {
            get
            {
                return this.Activity.Container;
            }
        }

        public string StaffId
        {
            get
            {
                return this.wfAssigninst.StaffId;
            }
        }

        public string StaffName
        {
            get
            {
                return this.wfAssigninst.StaffName;
            }
        }

        public AssignStatusType Status
        {
            get
            {
                return this.wfAssigninst.Status;
            }
        }
    }
}

