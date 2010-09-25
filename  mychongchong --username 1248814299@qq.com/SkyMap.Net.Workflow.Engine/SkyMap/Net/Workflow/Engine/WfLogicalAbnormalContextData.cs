namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Remoting.Messaging;

    [Serializable]
    public class WfLogicalAbnormalContextData : ILogicalThreadAffinative
    {
        private string decisionMemo;
        private bool decisionNeed;
        private string decisionStaffId;
        private string decisionStaffName;
        private string opReason;
        private string receiveActinstId;
        private string receiveActinstName;
        private string receiveAssignId;
        private string receiveStaffId;
        private string receiveStaffName;

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

        public bool DecisionNeed
        {
            get
            {
                return this.decisionNeed;
            }
            set
            {
                this.decisionNeed = value;
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
    }
}

