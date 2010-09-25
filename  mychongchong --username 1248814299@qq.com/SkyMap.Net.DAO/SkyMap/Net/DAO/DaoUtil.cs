namespace SkyMap.Net.DAO
{
    using System;

    public sealed class DaoUtil
    {
        private DaoUtil()
        {
        }

        public static IDA0 GetDaoInstance(string nameSpace)
        {
            string dAOConfigFile = SupportClass.GetDAOConfigFile(nameSpace);
            IDA0 dao = DaoAttribute.GetDaoAttr().GetDao(dAOConfigFile);
            if (dao == null)
            {
                throw new DaoNullException();
            }
            return dao;
        }
    }
}

