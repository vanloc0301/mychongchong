namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    public class WfActivitySubflowImpl : WfActivityAbstractImpl
    {
        public WfActivitySubflowImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        public override void Activate()
        {
            base.wfActivity.Complete();
        }

        private void CreateSubflow(Prodef prodef)
        {
            IWfProcessMgr wfProcessMgr = WfFactory.GetWfProcessMgr(prodef);
            IWfRequester wfRequester = WfFactory.GetWfRequester(base.wfActivity);
            wfProcessMgr.CreateProcess(wfRequester).Start();
        }

        public override void Start()
        {
            base.wfActivity.Store();
            Actdef defineOject = base.wfActivity.GetDefineOject() as Actdef;
            Prodef prodef = QueryHelper.Get<Prodef>("Prodef_" + defineOject.SubflowProdefId, defineOject.SubflowProdefId);
            if (prodef == null)
            {
                throw new WfException("Actdef have error on subflow prodef");
            }
            this.CreateSubflow(prodef);
            if (!defineOject.IsSubflowSync)
            {
                base.wfActivity.Activate();
            }
        }
    }
}

