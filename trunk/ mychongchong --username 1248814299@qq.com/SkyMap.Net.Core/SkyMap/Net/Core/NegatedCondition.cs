namespace SkyMap.Net.Core
{
    using System;
    using System.Xml;

    public class NegatedCondition : ICondition
    {
        private ConditionFailedAction action = ConditionFailedAction.Exclude;
        private ICondition condition;

        public NegatedCondition(ICondition condition)
        {
            this.condition = condition;
        }

        public bool IsValid(object owner, Codon codon)
        {
            return !this.condition.IsValid(owner, codon);
        }

        public static ICondition Read(XmlReader reader)
        {
            return new NegatedCondition(Condition.ReadConditionList(reader, "Not")[0]);
        }

        public ConditionFailedAction Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }

        public string Name
        {
            get
            {
                return ("Not " + this.condition.Name);
            }
        }
    }
}

