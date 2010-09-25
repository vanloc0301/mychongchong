namespace SkyMap.Net.Core
{
    using System;

    public interface IConditionEvaluator
    {
        bool IsValid(object caller, Condition condition, Codon codon);
    }
}

