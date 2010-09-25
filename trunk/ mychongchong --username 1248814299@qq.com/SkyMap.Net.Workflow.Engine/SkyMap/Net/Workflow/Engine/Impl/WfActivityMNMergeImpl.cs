namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    public class WfActivityMNMergeImpl : WfActivityAbstractImpl
    {
        public WfActivityMNMergeImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        private bool CheckMerged()
        {
            Actinst instanceObject = (Actinst) base.wfActivity.GetInstanceObject();
            Actdef defineOject = (Actdef) base.wfActivity.GetDefineOject();
            if (defineOject.MNMergeNum <= instanceObject.ToCount)
            {
                return true;
            }
            Actinst branchActinst = base.GetBranchActinst(base.wfActivity.Key);
            if (branchActinst == null)
            {
                throw new WfException("Workfow have an inexpectant error!");
            }
            return (branchActinst.FromCount == ((Actinst) base.wfActivity.GetInstanceObject()).ToCount);
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

