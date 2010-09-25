namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Workflow.Instance;
    using System;

    public class WfActivityAndMergeImpl : WfActivityAbstractImpl
    {
        public WfActivityAndMergeImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        private bool CheckMerged()
        {
            Actinst branchActinst = base.GetBranchActinst(base.wfActivity.Key);
            return ((branchActinst == null) || (branchActinst.FromCount == ((Actinst) base.wfActivity.GetInstanceObject()).ToCount));
        }

        public override void Complete()
        {
            if (this.CheckMerged())
            {
                base.Complete();
            }
        }

        public override void Start()
        {
            base.wfActivity.Activate();
        }
    }
}

