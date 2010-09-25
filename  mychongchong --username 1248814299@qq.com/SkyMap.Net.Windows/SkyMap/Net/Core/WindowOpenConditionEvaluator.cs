namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public class WindowOpenConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (WorkbenchSingleton.Workbench != null)
            {
                string str = condition.Properties["openwindow"];
                if (str == "*")
                {
                    return (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null);
                }
                foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    Type type = content.GetType();
                    if (type.ToString() == str)
                    {
                        return true;
                    }
                    foreach (Type type2 in type.GetInterfaces())
                    {
                        if (type2.ToString() == str)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

