namespace SkyMap.Net.Workflow.Client.AddIns
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Data;

    public class IsListCountThanZeroConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (!(caller is IWfBox))
            {
                return false;
            }
            try
            {
                DataView dataSource = (DataView) (caller as IWfBox).DataSource;
                return ((dataSource != null) && (dataSource.Count > 0));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

