namespace SkyMap.Net.SqlOM
{
    using System;

    public class UpdateTerm
    {
        private string fieldName;
        private SqlExpression val;

        public UpdateTerm(string fieldName, SqlExpression val)
        {
            this.fieldName = fieldName;
            this.val = val;
        }

        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }

        public SqlExpression Value
        {
            get
            {
                return this.val;
            }
        }
    }
}

