namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public class WindowActiveConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (WorkbenchSingleton.Workbench != null)
            {
                string str = condition.Properties["activewindow"];
                if (str == "*")
                {
                    return (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null);
                }
                if ((WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null) || (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent == null))
                {
                    return false;
                }
                Type type = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent.GetType();
                if (type.FullName == str)
                {
                    return true;
                }
                foreach (Type type2 in type.GetInterfaces())
                {
                    if (type2.FullName == str)
                    {
                        return true;
                    }
                }
                while ((type = type.BaseType) != null)
                {
                    if (type.FullName == str)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

