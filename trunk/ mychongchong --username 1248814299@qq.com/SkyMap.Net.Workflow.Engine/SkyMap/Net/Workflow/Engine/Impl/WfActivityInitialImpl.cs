namespace SkyMap.Net.Workflow.Engine.Impl
{
    using System;

    public class WfActivityInitialImpl : WfActivityAbstractImpl
    {
        public WfActivityInitialImpl(WfActivity wfActivity) : base(wfActivity)
        {
        }

        public override void Activate()
        {
            base.Activate();
            base.wfActivity.Complete();
        }

        public override void Start()
        {
            base.wfActivity.Activate();
        }
    }
}

