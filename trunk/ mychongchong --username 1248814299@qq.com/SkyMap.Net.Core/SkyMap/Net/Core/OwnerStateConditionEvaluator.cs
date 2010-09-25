namespace SkyMap.Net.Core
{
    using System;

    public class OwnerStateConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            if (caller is IOwnerState)
            {
                try
                {
                    string str = condition.Properties.Get<string>("ownerstate", string.Empty);
                    if (codon.Properties.Contains("ownerstate"))
                    {
                        str = codon.Properties["ownerstate"];
                    }
                    if (string.IsNullOrEmpty(str) || (str == "*"))
                    {
                        return true;
                    }
                    Enum internalState = ((IOwnerState) caller).InternalState;
                    Enum enum3 = (Enum) Enum.Parse(internalState.GetType(), str);
                    int num = int.Parse(internalState.ToString("D"));
                    int num2 = int.Parse(enum3.ToString("D"));
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("stateInt:{0}, conditionInt:{1}", new object[] { num, num2 });
                    }
                    return ((num & num2) > 0);
                }
                catch (Exception)
                {
                    throw new ApplicationException(string.Format("[{0}] can't parse '" + condition.Properties["ownerstate"] + "'. Not a valid value.", codon.Id));
                }
            }
            return false;
        }
    }
}

