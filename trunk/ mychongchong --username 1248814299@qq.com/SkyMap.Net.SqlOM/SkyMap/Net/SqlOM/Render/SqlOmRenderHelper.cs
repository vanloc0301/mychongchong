namespace SkyMap.Net.SqlOM.Render
{
    using System;
    using System.Configuration;

    public class SqlOmRenderHelper
    {
        private static ISqlOmRenderer instance;

        static SqlOmRenderHelper()
        {
            string typeName = ConfigurationSettings.AppSettings["SqlOmRenderer"];
            Type type = null;
            if (typeName != null)
            {
                type = Type.GetType(typeName, false);
            }
            if (type == null)
            {
                type = typeof(SqlServerRenderer);
            }
            instance = Activator.CreateInstance(type) as ISqlOmRenderer;
        }

        public static ISqlOmRenderer Instance
        {
            get
            {
                return instance;
            }
        }
    }
}

