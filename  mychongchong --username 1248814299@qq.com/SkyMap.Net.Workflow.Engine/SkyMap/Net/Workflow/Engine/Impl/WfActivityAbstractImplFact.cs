namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    public sealed class WfActivityAbstractImplFact
    {
        private WfActivityAbstractImplFact()
        {
        }

        public static WfActivityAbstractImpl GetConcretImpl(WfActivity wfActivity)
        {
            switch (wfActivity.Type)
            {
                case ActdefType.INITIAL:
                    return new WfActivityInitialImpl(wfActivity);

                case ActdefType.INTERACTION:
                    return new WfActivityInteractionImpl(wfActivity);

                case ActdefType.COMPLETION:
                    return new WfActivityCompletionImpl(wfActivity);

                case ActdefType.AND_BRANCH:
                    return new WfActivityAndBranchImpl(wfActivity);

                case ActdefType.OR_BRANCH:
                    return new WfActivityOrBranchImpl(wfActivity);

                case ActdefType.AND_MERGE:
                    return new WfActivityAndMergeImpl(wfActivity);

                case ActdefType.OR_MERGE:
                    return new WfActivityOrMergeImpl(wfActivity);

                case ActdefType.DUMMY:
                    return new WfActivityDummyImpl(wfActivity);

                case ActdefType.SUBFLOW:
                    return new WfActivitySubflowImpl(wfActivity);

                case ActdefType.MN_MERGE:
                    return new WfActivityMNMergeImpl(wfActivity);
            }
            throw new WfException("No implement actdef type");
        }
    }
}

