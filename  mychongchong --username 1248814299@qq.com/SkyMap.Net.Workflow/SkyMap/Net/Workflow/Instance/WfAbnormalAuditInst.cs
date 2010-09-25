namespace SkyMap.Net.Workflow.Instance
{
    using System;

    [Serializable]
    public class WfAbnormalAuditInst : WfEventAuditInst
    {
        private string assignId;
        private string decisionMemo;
        private string decisionStaffId;
        private string decisionStaffName;
        private WfDecisionStatusType decisionStatus;
        private DateTime? decisionTime;
        private bool needDecision;
        private string opReason;
        private string opStaffId;
        private string opStaffName;
        private string receiveActinstId;
        private string receiveActinstName;
        private string receiveAssignId;
        private string receiveStaffId;
        private string receiveStaffName;
        private string releaseStaffId;
        private string releaseStaffName;
        private DateTime? releaseTime;
        private WfReleaseType releaseType;
        private WfAbnormalType type;

        public string AssignId
        {
            get
            {
                return this.assignId;
            }
            set
            {
                this.assignId = value;
            }
        }

        public string DecisionMemo
        {
            get
            {
                return this.decisionMemo;
            }
            set
            {
                this.decisionMemo = value;
            }
        }

        public string DecisionStaffId
        {
            get
            {
                return this.decisionStaffId;
            }
            set
            {
                this.decisionStaffId = value;
            }
        }

        public string DecisionStaffName
        {
            get
            {
                return this.decisionStaffName;
            }
            set
            {
                this.decisionStaffName = value;
            }
        }

        public WfDecisionStatusType DecisionStatus
        {
            get
            {
                return this.decisionStatus;
            }
            set
            {
                this.decisionStatus = value;
            }
        }

        public DateTime? DecisionTime
        {
            get
            {
                return this.decisionTime;
            }
            set
            {
                this.decisionTime = value;
            }
        }

        public bool NeedDecision
        {
            get
            {
                return this.needDecision;
            }
            set
            {
                this.needDecision = value;
            }
        }

        public string OpReason
        {
            get
            {
                return this.opReason;
            }
            set
            {
                this.opReason = value;
            }
        }

        public string OpStaffId
        {
            get
            {
                return this.opStaffId;
            }
            set
            {
                this.opStaffId = value;
            }
        }

        public string OpStaffName
        {
            get
            {
                return this.opStaffName;
            }
            set
            {
                this.opStaffName = value;
            }
        }

        public string ReceiveActinstId
        {
            get
            {
                return this.receiveActinstId;
            }
            set
            {
                this.receiveActinstId = value;
            }
        }

        public string ReceiveActinstName
        {
            get
            {
                return this.receiveActinstName;
            }
            set
            {
                this.receiveActinstName = value;
            }
        }

        public string ReceiveAssignId
        {
            get
            {
                return this.receiveAssignId;
            }
            set
            {
                this.receiveAssignId = value;
            }
        }

        public string ReceiveStaffId
        {
            get
            {
                return this.receiveStaffId;
            }
            set
            {
                this.receiveStaffId = value;
            }
        }

        public string ReceiveStaffName
        {
            get
            {
                return this.receiveStaffName;
            }
            set
            {
                this.receiveStaffName = value;
            }
        }

        public string ReleaseStaffId
        {
            get
            {
                return this.releaseStaffId;
            }
            set
            {
                this.releaseStaffId = value;
            }
        }

        public string ReleaseStaffName
        {
            get
            {
                return this.releaseStaffName;
            }
            set
            {
                this.releaseStaffName = value;
            }
        }

        public DateTime? ReleaseTime
        {
            get
            {
                return this.releaseTime;
            }
            set
            {
                this.releaseTime = value;
            }
        }

        public WfReleaseType ReleaseType
        {
            get
            {
                return this.releaseType;
            }
            set
            {
                this.releaseType = value;
            }
        }

        public WfAbnormalType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

