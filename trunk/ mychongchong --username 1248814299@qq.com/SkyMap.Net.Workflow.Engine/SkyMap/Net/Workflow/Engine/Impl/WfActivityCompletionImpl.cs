namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Workflow.XPDL;

    public class WfActivityCompletionImpl : WfActivityAbstractImpl
    {
        public WfActivityCompletionImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        public override void Activate()
        {
            base.Activate();
            base.wfActivity.Complete();
        }

        public override void Complete()
        {
            IList<IWfActivity> listStep = base.wfActivity.Container.GetListStep();
            foreach (IWfActivity activity in listStep)
            {
                if (((activity.Key != base.wfActivity.Key) && (activity.WfState != WfStateType.Closed)) && (activity.HowClosed != HowClosedType.Completed))
                {
                    throw new CannotCompleteException("The actinst have not complete, Id:" + activity.Key + ", Name:" + activity.Name);
                }
            }
            base.Complete();
        }

        public override void Pass(IList<Transition> toTrans)
        {
        }

        public override void Start()
        {
            base.wfActivity.Activate();
        }
    }
}

