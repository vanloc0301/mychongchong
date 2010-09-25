namespace SkyMap.Net.Workflow.Client.View
{
    using System;

    [Flags]
    public enum WfViewState
    {
        CanNavFirst = 0x10,
        CanNavLast = 0x80,
        CanNavNext = 0x40,
        CanNavPrevious = 0x20,
        CanPass = 2,
        CanPrint = 8,
        CanRemove = 4,
        CanSave = 1,
        None = 0
    }
}

