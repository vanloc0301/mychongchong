namespace SkyMap.Net.Workflow.Engine.Impl
{
    using System;

    public class WfActivityOrMergeImpl : WfActivityAbstractImpl
    {
        public WfActivityOrMergeImpl(WfActivity wfactivity) : base(wfactivity)
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

