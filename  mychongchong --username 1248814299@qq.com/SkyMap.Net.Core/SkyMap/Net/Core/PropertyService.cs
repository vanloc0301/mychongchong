namespace SkyMap.Net.Core
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    public static class PropertyService
    {
        private static string appTitle;
        private static string configDirectory;
        private static string dataDirectory;
        private static Properties properties;
        private static string propertyFileName;
        private static string propertyXmlRootNodeName;

        public static  event PropertyChangedEventHandler PropertyChanged;

        public static string Get(string property)
        {
            return properties[property];
        }

        public static T Get<T>(string property, T defaultValue)
        {
            return properties.Get<T>(property, defaultValue);
        }

        public static void InitializeService(string configDirectory, string dataDirectory, string propertiesName)
        {
            if (properties != null)
            {
                throw new InvalidOperationException("Service is already initialized.");
            }
            if (((configDirectory == null) || (dataDirectory == null)) || (propertiesName == null))
            {
                throw new ArgumentNullException();
            }
            properties = new Properties();
            PropertyService.configDirectory = configDirectory;
            PropertyService.dataDirectory = dataDirectory;
            propertyXmlRootNodeName = propertiesName;
            propertyFileName = propertiesName + ".xml";
            properties.PropertyChanged += new PropertyChangedEventHandler(PropertyService.PropertiesPropertyChanged);
        }

        public static void Load()
        {
            if (properties == null)
            {
                throw new InvalidOperationException("Service is not initialized.");
            }
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }
            if (!LoadPropertiesFromStream(Path.Combine(ConfigDirectory, propertyFileName)))
            {
                LoadPropertiesFromStream(FileUtility.Combine(new string[] { DataDirectory, "options", propertyFileName }));
            }
        }

        public static bool LoadPropertiesFromStream(string fileName)
        {
            LoggingService.DebugFormatted("载入{0}文件", new object[] { fileName });
            if (!File.Exists(fileName))
            {
                foreach (string str in FileUtility.SearchDirectory(ConfigDirectory, "*" + propertyFileName, false, false))
                {
                    File.Copy(Path.Combine(ConfigDirectory, str), fileName, true);
                    break;
                }
                if (!File.Exists(fileName))
                {
                    return false;
                }
            }
            try
            {
                using (XmlTextReader reader = new XmlTextReader(fileName))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && (reader.LocalName == propertyXmlRootNodeName))
                        {
                            properties.ReadProperties(reader, propertyXmlRootNodeName);
                            return true;
                        }
                    }
                }
            }
            catch (XmlException exception)
            {
                LoggingService.Error(exception);
            }
            return false;
        }

        private static void PropertiesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, e);
            }
        }

        public static void Save()
        {
            using (XmlTextWriter writer = new XmlTextWriter(Path.Combine(ConfigDirectory, propertyFileName), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartElement(propertyXmlRootNodeName);
                properties.WriteProperties(writer);
                writer.WriteEndElement();
            }
        }

        public static void Set<T>(string property, T value)
        {
            properties.Set<T>(property, value);
        }

        public static string ApplicationTitle
        {
            get
            {
                if (appTitle == null)
                {
                    appTitle = ConfigurationManager.AppSettings["AppName"];
                    if (string.IsNullOrEmpty(appTitle))
                    {
                        appTitle = ((AssemblyTitleAttribute) Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), true)[0]).Title;
                    }
                }
                return appTitle;
            }
        }

        public static string ConfigDirectory
        {
            get
            {
                if (configDirectory == null)
                {
                    configDirectory = Path.Combine(FileUtility.ApplicationRootPath, "Config");
                }
                return configDirectory;
            }
        }

        public static string DataDirectory
        {
            get
            {
                return dataDirectory;
            }
        }
    }
}

