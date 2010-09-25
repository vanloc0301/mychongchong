namespace SkyMap.Net.Core
{
    using System;

    public class CompareConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            StringComparison invariantCultureIgnoreCase;
            string str = condition.Properties["comparisonType"];
            if (string.IsNullOrEmpty(str))
            {
                invariantCultureIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
            }
            else
            {
                invariantCultureIgnoreCase = (StringComparison) Enum.Parse(typeof(StringComparison), str);
            }
            return string.Equals(SkyMap.Net.Core.StringParser.Parse(condition.Properties["string"]), SkyMap.Net.Core.StringParser.Parse(condition.Properties["equals"]), invariantCultureIgnoreCase);
        }
    }
}

