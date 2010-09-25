namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public class ActiveWindowStateConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (((WorkbenchSingleton.Workbench == null) || (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null)) || (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent == null))
            {
                return false;
            }
            WindowState state = condition.Properties.Get<WindowState>("windowstate", WindowState.None);
            WindowState state2 = condition.Properties.Get<WindowState>("nowindowstate", WindowState.None);
            bool flag = false;
            if (state != WindowState.None)
            {
                if ((state & WindowState.Dirty) > WindowState.None)
                {
                    flag |= WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsDirty;
                }
                if ((state & WindowState.Untitled) > WindowState.None)
                {
                    flag |= WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsUntitled;
                }
                if ((state & WindowState.ViewOnly) > WindowState.None)
                {
                    flag |= WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsViewOnly;
                }
            }
            else
            {
                flag = true;
            }
            if (state2 != WindowState.None)
            {
                if ((state2 & WindowState.Dirty) > WindowState.None)
                {
                    flag &= !WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsDirty;
                }
                if ((state2 & WindowState.Untitled) > WindowState.None)
                {
                    flag &= !WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsUntitled;
                }
                if ((state2 & WindowState.ViewOnly) > WindowState.None)
                {
                    flag &= !WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent.IsViewOnly;
                }
            }
            return flag;
        }
    }
}

