namespace SkyMap.Net.DataForms
{
    using System;

    internal sealed class DataEngineFactory
    {
        private static Type type;

        static DataEngineFactory()
        {
            string dataEngineName = SupportClass.GetDataEngineName();
            type = Type.GetType(dataEngineName);
            if (type == null)
            {
                throw new DataFormException("Cannot find data engine : " + dataEngineName);
            }
        }

        internal static IDataEngine CreateInstance()
        {
            return (Activator.CreateInstance(type) as IDataEngine);
        }
    }
}

