namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class WfActivityOrBranchImpl : WfActivityAbstractImpl
    {
        public WfActivityOrBranchImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        public override void Pass(IList<Transition> toTrans)
        {
            if (toTrans.Count <= 1)
            {
                throw new WfException("Actdef have error");
            }
            WfLogicalPassContextData passContextData = WfUtil.GetPassContextData();
            string selectedTranId = passContextData.SelectedTranId;
            if ((passContextData != null) && !StringHelper.IsNull(selectedTranId))
            {
                foreach (Transition transition in toTrans)
                {
                    if (transition.Id == selectedTranId)
                    {
                        base.Pass(transition);
                        return;
                    }
                }
                throw new WfException("The CallContext Data have error:PassContext data have error");
            }
            bool flag = false;
            foreach (Transition transition in toTrans)
            {
                bool flag2 = true;
                if (transition.Conditions.Count > 0)
                {
                    foreach (SkyMap.Net.Workflow.XPDL.Condition condition in transition.Conditions)
                    {
                        if (!base.wfActivity.CheckCondition(condition))
                        {
                            flag2 = false;
                        }
                    }
                }
                if (flag2)
                {
                    base.Pass(transition);
                    flag = true;
                }
            }
            if (!flag)
            {
                throw new NotMeetConditionException("No a branch meeting these conditions");
            }
        }

        public override void Start()
        {
            base.wfActivity.Activate();
        }
    }
}

