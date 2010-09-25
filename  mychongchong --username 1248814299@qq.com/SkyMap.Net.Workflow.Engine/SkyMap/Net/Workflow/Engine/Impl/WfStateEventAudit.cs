namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using System;

    [Serializable]
    public class WfStateEventAudit : WfEventAudit, IWfStateEventAudit, IWfEventAudit
    {
        private WfStatusType newState;
        private WfStatusType oldState;

        public WfStateEventAudit(IWfExecutionObject source, WfStatusType oldstate, WfStatusType newstate) : base(source)
        {
            this.oldState = oldstate;
            this.newState = newstate;
            this.Store();
        }

        private void Store()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("将新建WfEventAudit实例...");
            }
            WfStateEventAuditInst wfAuditInst = new WfStateEventAuditInst();
            wfAuditInst.OldState = this.oldState;
            wfAuditInst.NewState = this.newState;
            base.Store(wfAuditInst);
        }

        public WfStatusType NewState
        {
            get
            {
                return this.newState;
            }
        }

        public WfStatusType OldState
        {
            get
            {
                return this.oldState;
            }
        }
    }
}

