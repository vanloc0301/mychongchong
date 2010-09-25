namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.View;
    using System;

    public class ExistFailSavedFileEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (!(caller is WfView))
            {
                return false;
            }
            try
            {
                return (caller as WfView).DataForm.ExistFailSavedFile();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

