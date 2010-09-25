namespace SkyMap.Net.DataForms
{
    using Nini.Config;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Security.Policy;
    using SkyMap.Net.Core;

    public sealed class SupportClass
    {
        private static XmlConfigSource config;
        private const string dataBindProperty = "DataBindProperty";
        private static string DataFormsConfigPath = PropertyService.ConfigDirectory;
        private const string dataFormSection = "DataForm";
        private static StringCollection DBPKeys;
        private static string notNeedFindChildList;
        private static Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>(5);
        private static string source = "DataForms.config";

        static SupportClass()
        {
            string path = Path.Combine(DataFormsConfigPath, source);
            if (File.Exists(path))
            {
                config = new XmlConfigSource(path);
            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, source);
                if (File.Exists(path))
                {
                    config = new XmlConfigSource(path);
                }
            }
            if (config == null)
            {
                throw new NullReferenceException("找不到表单配置文件:" + path);
            }
            string[] keys = config.Configs["DataBindProperty"].GetKeys();
            DBPKeys = new StringCollection();
            DBPKeys.AddRange(keys);
        }

        public static bool ContainDataBindPropertyKey(Type type)
        {
            return DBPKeys.Contains(GetFullTypeString(type));
        }

        public static PropertyInfo GetBindPropertyInfo(Type type, string propertyName)
        {
            string key = type.FullName + "," + propertyName;
            if (!propertyInfos.ContainsKey(key))
            {
                PropertyInfo property = type.GetProperty(propertyName);
                propertyInfos.Add(key, property);
                return property;
            }
            return propertyInfos[key];
        }

        internal static string GetDataControllerTypeName()
        {
            return config.Configs["DataForm"].GetString("DataFormControllerType", string.Empty);
        }

        internal static string GetDataEngineName()
        {
            return config.Configs["DataForm"].GetString("DataEngine", string.Empty);
        }

        internal static string GetDataFormBaseDir()
        {
            string str = config.Configs["DataForm"].GetString("BaseDir", string.Empty);
            if (str.StartsWith("http://") || str.StartsWith("ftp://"))
            {
                IEnumerator enumerator = SecurityManager.PolicyHierarchy();
                enumerator.MoveNext();
                for (PolicyLevel level = enumerator.Current as PolicyLevel; level != null; level = enumerator.Current as PolicyLevel)
                {
                    if (level.Label == "Machine")
                    {
                        foreach (NamedPermissionSet set in level.NamedPermissionSets)
                        {
                            if (set.Name == "FullTrust")
                            {
                                UrlMembershipCondition membershipCondition = new UrlMembershipCondition(str + "*");
                                PolicyStatement policy = new PolicyStatement(set);
                                UnionCodeGroup group = new UnionCodeGroup(membershipCondition, policy);
                                level.RootCodeGroup.AddChild(group);
                            }
                        }
                        return str;
                    }
                    enumerator.MoveNext();
                }
                return str;
            }
            return string.Concat(new object[] { "file://", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, str });
        }

        private static string GetFullTypeString(Type type)
        {
            return type.FullName;
        }

        public static string GetPropertyName(Type type)
        {
            return config.Configs["DataBindProperty"].GetString(GetFullTypeString(type));
        }

        internal static IConfig GetTraceConfig()
        {
            return config.Configs["Trace"];
        }

        public static bool IsNeedFindChild(Type type)
        {
            if (notNeedFindChildList == null)
            {
                notNeedFindChildList = config.Configs["DataBindProperty"].GetString("NotNeedFindChildList", string.Empty);
            }
            return (notNeedFindChildList.IndexOf(type.FullName) <= 0);
        }
    }
}

