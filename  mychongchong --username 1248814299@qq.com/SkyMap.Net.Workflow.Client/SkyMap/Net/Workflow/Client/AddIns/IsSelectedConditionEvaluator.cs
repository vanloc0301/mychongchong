namespace SkyMap.Net.Workflow.Client.AddIns
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class IsSelectedConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (!(caller is IWfBox))
            {
                return false;
            }
            try
            {
                return (caller as IWfBox).IsSelected;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

