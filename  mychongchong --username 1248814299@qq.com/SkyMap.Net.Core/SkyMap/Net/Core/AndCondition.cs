namespace SkyMap.Net.Core
{
    using System;
    using System.Text;
    using System.Xml;

    public class AndCondition : ICondition
    {
        private ConditionFailedAction action = ConditionFailedAction.Exclude;
        private ICondition[] conditions;

        public AndCondition(ICondition[] conditions)
        {
            this.conditions = conditions;
        }

        public bool IsValid(object owner, Codon codon)
        {
            foreach (ICondition condition in this.conditions)
            {
                if (!condition.IsValid(owner, codon))
                {
                    return false;
                }
            }
            return true;
        }

        public static ICondition Read(XmlReader reader)
        {
            return new AndCondition(Condition.ReadConditionList(reader, "And"));
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
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < this.conditions.Length; i++)
                {
                    builder.Append(this.conditions[i].Name);
                    if ((i + 1) < this.conditions.Length)
                    {
                        builder.Append(" And ");
                    }
                }
                return builder.ToString();
            }
        }
    }
}

