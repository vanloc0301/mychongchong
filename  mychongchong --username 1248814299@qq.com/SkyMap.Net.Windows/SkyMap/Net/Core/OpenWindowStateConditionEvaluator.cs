namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public class OpenWindowStateConditionEvaluator : IConditionEvaluator
    {
        private WindowState nowindowState = WindowState.None;
        private WindowState windowState = WindowState.None;

        private bool IsStateOk(IWorkbenchWindow window)
        {
            if ((window == null) || (window.ViewContent == null))
            {
                return false;
            }
            bool flag = false;
            if (this.windowState != WindowState.None)
            {
                if ((this.windowState & WindowState.Dirty) > WindowState.None)
                {
                    flag |= window.ViewContent.IsDirty;
                }
                if ((this.windowState & WindowState.Untitled) > WindowState.None)
                {
                    flag |= window.ViewContent.IsUntitled;
                }
                if ((this.windowState & WindowState.ViewOnly) > WindowState.None)
                {
                    flag |= window.ViewContent.IsViewOnly;
                }
            }
            else
            {
                flag = true;
            }
            if (this.nowindowState != WindowState.None)
            {
                if ((this.nowindowState & WindowState.Dirty) > WindowState.None)
                {
                    flag &= !window.ViewContent.IsDirty;
                }
                if ((this.nowindowState & WindowState.Untitled) > WindowState.None)
                {
                    flag &= !window.ViewContent.IsUntitled;
                }
                if ((this.nowindowState & WindowState.ViewOnly) > WindowState.None)
                {
                    flag &= !window.ViewContent.IsViewOnly;
                }
            }
            return flag;
        }

        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (WorkbenchSingleton.Workbench != null)
            {
                if ((WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null) || (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent == null))
                {
                    return false;
                }
                this.windowState = condition.Properties.Get<WindowState>("openwindowstate", WindowState.None);
                this.nowindowState = condition.Properties.Get<WindowState>("noopenwindowstate", WindowState.None);
                foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (this.IsStateOk(content.WorkbenchWindow))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

