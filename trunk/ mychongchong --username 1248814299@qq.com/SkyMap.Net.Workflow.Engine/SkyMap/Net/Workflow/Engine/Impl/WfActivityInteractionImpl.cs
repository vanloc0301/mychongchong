namespace SkyMap.Net.Workflow.Engine.Impl
{
    using System;

    public class WfActivityInteractionImpl : WfActivityAbstractImpl
    {
        public WfActivityInteractionImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        public override void Activate()
        {
            base.Activate();
            base.wfActivity.Store();
        }

        public override void Start()
        {
            base.wfActivity.Store();
        }
    }
}

