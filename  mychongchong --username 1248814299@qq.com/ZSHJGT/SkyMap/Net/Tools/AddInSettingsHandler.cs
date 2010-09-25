namespace SkyMap.Net.Tools
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Runtime.InteropServices;
    using System.Xml;

    public class AddInSettingsHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ArrayList list = new ArrayList();
            XmlNode namedItem = section.Attributes.GetNamedItem("ignoreDefaultPath");
            if (namedItem != null)
            {
                try
                {
                    list.Add(Convert.ToBoolean(namedItem.Value));
                }
                catch (InvalidCastException)
                {
                    list.Add(false);
                }
            }
            else
            {
                list.Add(false);
            }
            foreach (XmlNode node2 in section.SelectNodes("AddInDirectory"))
            {
                XmlNode node3 = node2.Attributes.GetNamedItem("path");
                if (node3 != null)
                {
                    list.Add(node3.Value);
                }
            }
            return list;
        }

        public static string[] GetAddInDirectories(out bool ignoreDefaultPath)
        {
            ArrayList config = ConfigurationSettings.GetConfig("AddInDirectories") as ArrayList;
            if (config != null)
            {
                int count = config.Count;
                if (count <= 1)
                {
                    ignoreDefaultPath = false;
                    return null;
                }
                ignoreDefaultPath = (bool) config[0];
                string[] strArray = new string[count - 1];
                for (int i = 0; i < (count - 1); i++)
                {
                    strArray[i] = config[i + 1] as string;
                }
                return strArray;
            }
            ignoreDefaultPath = false;
            return null;
        }
    }
}

