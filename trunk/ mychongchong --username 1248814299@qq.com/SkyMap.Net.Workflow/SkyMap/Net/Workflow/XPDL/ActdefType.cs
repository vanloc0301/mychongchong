namespace SkyMap.Net.Workflow.XPDL
{
    using System;

    [Serializable]
    public enum ActdefType
    {
        INITIAL,
        INTERACTION,
        COMPLETION,
        AND_BRANCH,
        OR_BRANCH,
        AND_MERGE,
        OR_MERGE,
        DUMMY,
        SUBFLOW,
        MN_MERGE
    }
}

