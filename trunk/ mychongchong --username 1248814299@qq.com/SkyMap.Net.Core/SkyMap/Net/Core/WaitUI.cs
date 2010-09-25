namespace SkyMap.Net.Core
{
    using System;
    using System.Configuration;

    public class WaitUI
    {
        public virtual void Close()
        {
        }

        public static WaitUI Create()
        {
            string typeName = ConfigurationManager.AppSettings["WaitUI"];
            if (typeName != null)
            {
                Type type = Type.GetType(typeName);
                if ((type != null) && type.IsSubclassOf(typeof(WaitUI)))
                {
                    return (Activator.CreateInstance(type) as WaitUI);
                }
            }
            return new WaitUI();
        }

        public virtual void Show()
        {
        }
    }
}

