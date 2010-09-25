namespace SkyMap.Net.Core
{
    using System;

    public interface ICondition
    {
        bool IsValid(object caller, Codon codon);

        ConditionFailedAction Action { get; set; }

        string Name { get; }
    }
}

