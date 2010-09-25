namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public class ActiveViewContentUntitledConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (((WorkbenchSingleton.Workbench == null) || (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null)) || (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent == null))
            {
                return false;
            }
            if (!condition.Properties.Contains("activewindowuntitled"))
            {
                return WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsUntitled;
            }
            bool flag = bool.Parse(condition.Properties["activewindowuntitled"]);
            return (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsUntitled == flag);
        }
    }
}

