namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class WfActivityAndBranchImpl : WfActivityAbstractImpl
    {
        public WfActivityAndBranchImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        public override void Activate()
        {
            base.Activate();
            base.wfActivity.Complete();
        }

        public override void Pass(IList<Transition> toTrans)
        {
            foreach (Transition transition in toTrans)
            {
                foreach (Condition condition in transition.Conditions)
                {
                    if (!base.wfActivity.CheckCondition(condition))
                    {
                        throw new NotMeetConditionException();
                    }
                }
                base.CreateNextActivity(transition.To);
            }
        }

        public override void Start()
        {
            base.wfActivity.Activate();
        }
    }
}

