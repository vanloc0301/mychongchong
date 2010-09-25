namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class DataWordService : MarshalByRefObject
    {
        private UnitOfWork currentUnitOfWork = new UnitOfWork(typeof(DataWord));

        public DataWord CreateDataWord(DataWordType dwType)
        {
            DataWord word = new DataWord("新常量", dwType);
            word.Id = StringHelper.GetNewGuid();
            this.currentUnitOfWork.RegisterNew(word);
            this.currentUnitOfWork.Commit();
            return word;
        }

        public DataWordType CreateDataWordType()
        {
            DataWordType type = new DataWordType("新类型");
            type.Id = StringHelper.GetNewGuid();
            type.DataWords = new List<DataWord>();
            this.currentUnitOfWork.RegisterNew(type);
            this.currentUnitOfWork.Commit();
            return type;
        }

        public IList<DataWord> FindDataWords(string typeCode)
        {
            string cacheKey = "ALL_DataWordType";
            IList<DataWordType> list = QueryHelper.List<DataWordType>(cacheKey);
            foreach (DataWordType type in list)
            {
                if (type.Code == typeCode)
                {
                    return type.DataWords;
                }
            }
            return null;
        }

        public static IList<DataWord> FindDataWordsByTypeCode(string typeCode)
        {
            string key = "DATAWORD_TYPE_" + typeCode;
            if (!DAOCacheService.Contains(key))
            {
                DAOCacheService.Put(key, Instance.FindDataWords(typeCode));
            }
            return (IList<DataWord>) DAOCacheService.Get(key);
        }

        public static DataWordService Instance
        {
            get
            {
                return new DataWordService();
            }
        }
    }
}

