namespace SkyMap.Net.Core
{
    using System;

    public class LazyConditionEvaluator : IConditionEvaluator
    {
        private AddIn addIn;
        private string className;
        private string name;

        public LazyConditionEvaluator(AddIn addIn, Properties properties)
        {
            this.addIn = addIn;
            this.name = properties["name"];
            this.className = properties["class"];
        }

        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            IConditionEvaluator evaluator = (IConditionEvaluator) this.addIn.CreateObject(this.className);
            if (evaluator == null)
            {
                return false;
            }
            AddInTree.ConditionEvaluators[this.name] = evaluator;
            return evaluator.IsValid(caller, condition, codon);
        }

        public override string ToString()
        {
            return string.Format("[LazyLoadConditionEvaluator: className = {0}, name = {1}]", this.className, this.name);
        }

        public string ClassName
        {
            get
            {
                return this.className;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

