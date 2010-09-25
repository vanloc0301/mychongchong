namespace SkyMap.Net.Workflow.Instance
{
    using System;

    [Serializable]
    public class WfStateEventAuditInst : WfEventAuditInst
    {
        private WfStatusType newState;
        private WfStatusType oldState;

        public WfStatusType NewState
        {
            get
            {
                return this.newState;
            }
            set
            {
                this.newState = value;
            }
        }

        public WfStatusType OldState
        {
            get
            {
                return this.oldState;
            }
            set
            {
                this.oldState = value;
            }
        }
    }
}

