namespace SkyMap.Net.DAO
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited=true)]
    public sealed class TransactionAttribute : Attribute
    {
        private string txName;
        private Type type;

        public TransactionAttribute(string name)
        {
            this.txName = SupportClass.GetDAOConfigFile(name);
        }

        public TransactionAttribute(Type type)
        {
            this.type = type;
        }

        public string PropertyName
        {
            get
            {
                return this.txName;
            }
        }

        public Type UnitOfWorkType
        {
            get
            {
                return this.type;
            }
        }
    }
}

