namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public static class StringParser
    {
        private static readonly Dictionary<string, string> properties;
        private static readonly Dictionary<string, object> propertyObjects;
        private static readonly Dictionary<string, IStringTagProvider> stringTagProviders;

        static StringParser()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            properties = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            stringTagProviders = new Dictionary<string, IStringTagProvider>(StringComparer.InvariantCultureIgnoreCase);
            propertyObjects = new Dictionary<string, object>();
            if (entryAssembly != null)
            {
                string location = entryAssembly.Location;
                propertyObjects["exe"] = FileVersionInfo.GetVersionInfo(location);
            }
            properties["USER"] = Environment.UserName;
        }

        private static string Get(object obj, string name)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(name);
            if (property != null)
            {
                return property.GetValue(obj, null).ToString();
            }
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                return field.GetValue(obj).ToString();
            }
            return null;
        }

        private static string GetValue(string propertyName, string[,] customTags)
        {
            string str;
            if (propertyName.StartsWith("res:"))
            {
                try
                {
                    return Parse(ResourceService.GetString(propertyName.Substring(4)), customTags);
                }
                catch (ResourceNotFoundException)
                {
                    return null;
                }
            }
            if (propertyName.Equals("DATE", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.ToShortDateString();
            }
            if (propertyName.Equals("TIME", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Now.ToShortTimeString();
            }
            if (propertyName.Equals("ProductName", StringComparison.OrdinalIgnoreCase))
            {
                return MessageService.ProductName;
            }
            if (customTags != null)
            {
                for (int i = 0; i < customTags.GetLength(0); i++)
                {
                    if (propertyName.Equals(customTags[i, 0], StringComparison.OrdinalIgnoreCase))
                    {
                        return customTags[i, 1];
                    }
                }
            }
            if (properties.ContainsKey(propertyName))
            {
                return properties[propertyName];
            }
            if (stringTagProviders.ContainsKey(propertyName))
            {
                return stringTagProviders[propertyName].Convert(propertyName);
            }
            int index = propertyName.IndexOf(':');
            if (index > 0)
            {
                str = propertyName.Substring(0, index);
                propertyName = propertyName.Substring(index + 1);
                string str3 = str.ToUpperInvariant();
                if (str3 == null)
                {
                    goto Label_0270;
                }
                if (!(str3 == "ADDINPATH"))
                {
                    if (str3 == "ENV")
                    {
                        return Environment.GetEnvironmentVariable(propertyName);
                    }
                    if (str3 == "RES")
                    {
                        try
                        {
                            return Parse(ResourceService.GetString(propertyName), customTags);
                        }
                        catch (ResourceNotFoundException)
                        {
                            return null;
                        }
                        goto Label_0266;
                    }
                    if (str3 == "PROPERTY")
                    {
                        goto Label_0266;
                    }
                    goto Label_0270;
                }
                foreach (AddIn @in in AddInTree.AddIns)
                {
                    if (@in.Manifest.Identities.ContainsKey(propertyName))
                    {
                        return Path.GetDirectoryName(@in.FileName);
                    }
                }
            }
            return null;
        Label_0266:
            return PropertyService.Get(propertyName);
        Label_0270:
            if (propertyObjects.ContainsKey(str))
            {
                return Get(propertyObjects[str], propertyName);
            }
            return null;
        }

        public static string Parse(string input)
        {
            return Parse(input, null);
        }

        public static void Parse(string[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = Parse(inputs[i], null);
            }
        }

        public static string Parse(string input, string[,] customTags)
        {
            if (input == null)
            {
                return null;
            }
            int startIndex = 0;
            StringBuilder builder = null;
            do
            {
                int num2 = startIndex;
                startIndex = input.IndexOf("${", startIndex);
                if (startIndex < 0)
                {
                    if (builder == null)
                    {
                        return input;
                    }
                    if (num2 < input.Length)
                    {
                        builder.Append(input, num2, input.Length - num2);
                    }
                    return builder.ToString();
                }
                if (builder == null)
                {
                    if (startIndex == 0)
                    {
                        builder = new StringBuilder();
                    }
                    else
                    {
                        builder = new StringBuilder(input, 0, startIndex, startIndex + 0x10);
                    }
                }
                else if (startIndex > num2)
                {
                    builder.Append(input, num2, startIndex - num2);
                }
                int index = input.IndexOf('}', startIndex + 1);
                if (index < 0)
                {
                    builder.Append("${");
                    startIndex += 2;
                }
                else
                {
                    string propertyName = input.Substring(startIndex + 2, (index - startIndex) - 2);
                    string str2 = GetValue(propertyName, customTags);
                    if (str2 == null)
                    {
                        builder.Append("${");
                        builder.Append(propertyName);
                        builder.Append('}');
                    }
                    else
                    {
                        builder.Append(str2);
                    }
                    startIndex = index + 1;
                }
            }
            while (startIndex < input.Length);
            return builder.ToString();
        }

        public static void RegisterStringTagProvider(IStringTagProvider tagProvider)
        {
            foreach (string str in tagProvider.Tags)
            {
                stringTagProviders[str] = tagProvider;
            }
        }

        public static Dictionary<string, string> Properties
        {
            get
            {
                return properties;
            }
        }

        public static Dictionary<string, object> PropertyObjects
        {
            get
            {
                return propertyObjects;
            }
        }
    }
}

