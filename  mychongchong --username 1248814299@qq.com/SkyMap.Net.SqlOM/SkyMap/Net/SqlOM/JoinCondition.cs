namespace SkyMap.Net.SqlOM
{
    using System;

    public class JoinCondition
    {
        private string leftField;
        private string rightField;

        public JoinCondition(string field) : this(field, field)
        {
        }

        public JoinCondition(string leftField, string rightField)
        {
            this.leftField = leftField;
            this.rightField = rightField;
        }

        public string LeftField
        {
            get
            {
                return this.leftField;
            }
        }

        public string RightField
        {
            get
            {
                return this.rightField;
            }
        }
    }
}

