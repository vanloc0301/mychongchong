namespace SkyMap.Net.SqlOM
{
    using System;

    public class SqlConstant
    {
        private SqlDataType type;
        private object val;

        public SqlConstant(SqlDataType type, object val)
        {
            this.type = type;
            this.val = val;
        }

        public static SqlConstant Date(DateTime val)
        {
            return new SqlConstant(SqlDataType.Date, val);
        }

        public static SqlConstant Number(decimal val)
        {
            return new SqlConstant(SqlDataType.Number, val);
        }

        public static SqlConstant Number(double val)
        {
            return new SqlConstant(SqlDataType.Number, val);
        }

        public static SqlConstant Number(int val)
        {
            return new SqlConstant(SqlDataType.Number, val);
        }

        public static SqlConstant String(string val)
        {
            return new SqlConstant(SqlDataType.String, val);
        }

        internal SqlDataType Type
        {
            get
            {
                return this.type;
            }
        }

        internal object Value
        {
            get
            {
                return this.val;
            }
        }
    }
}

