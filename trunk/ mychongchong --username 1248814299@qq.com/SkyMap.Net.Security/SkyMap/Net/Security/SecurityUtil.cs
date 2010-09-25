namespace SkyMap.Net.Security
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting.Messaging;
    using System.Security.Principal;
    using System.Web;

    public sealed class SecurityUtil
    {
        private static HybridDictionary authTypes = new HybridDictionary();
        private const string Principal_Tag = "Principal";

        public static  event EventHandler CurrentPrincipalChanged;

        public static  event CancelEventHandler CurrentPrincipalChanging;

        private SecurityUtil()
        {
        }

        public static void CurrentPrincipalWillChanged(CancelEventArgs e)
        {
            try
            {
                if (CurrentPrincipalChanging != null)
                {
                    SmPrincipal smPrincipal = GetSmPrincipal();
                    CurrentPrincipalChanging(smPrincipal, e);
                }
            }
            catch (SecurityException)
            {
            }
        }

        public static IList<T> GetCurrentPrincipalAuthResourcesByType<T>(string typeCode)
        {
            if (StringHelper.IsNull(typeCode))
            {
                throw new SecurityException("Auth type code cannot be null");
            }
            CAuthType type = authTypes[typeCode] as CAuthType;
            if (type == null)
            {
                IList<CAuthType> list = QueryHelper.List<CAuthType>(typeof(CAuthType).Namespace, "GetAuthTypeByTypeCode_" + typeCode, "GetAuthTypeByTypeCode", new object[] { typeCode });
                if ((list == null) || (list.Count == 0))
                {
                    throw new SecurityException("Cannot get auth type of code:'" + typeCode + "'.");
                }
                if (list.Count > 1)
                {
                    throw new SecurityException("Get more than one auth type of code:'" + typeCode + "'.");
                }
                type = list[0];
                authTypes.Add(typeCode, type);
            }
            Type type2 = Type.GetType(type.AuthGetClass);
            if (type2 == null)
            {
                throw new SecurityException("Cannot get the type:'" + type.AuthGetClass + "'.");
            }
            IQueryAuthResource resource = Activator.CreateInstance(type2) as IQueryAuthResource;
            return resource.QueryCurrentPricipalAuthResources<T>(type);
        }

        public static SmIdentity GetSmIdentity()
        {
            IIdentity identity = GetSmPrincipal().Identity;
            if (!(identity is SmIdentity))
            {
                throw new SecurityException("CurrentPrincipal.Identity is not SmIdentity");
            }
            return (identity as SmIdentity);
        }

        public static SmPrincipal GetSmPrincipal()
        {
            IPrincipal principal = null;
            if (HttpContext.Current == null)
            {
                object data = CallContext.GetData("Principal");
                if (data != null)
                {
                    principal = ((LogicalSecurityContextData) data).Principal;
                }
            }
            else
            {
                principal = HttpContext.Current.Items["Principal"] as IPrincipal;
            }
            if (principal == null)
            {
                throw new SecurityException("CurrentThread.CurrentPrincipal is null");
            }
            if (!(principal is SmPrincipal))
            {
                throw new SecurityException("CurrentThread.CurrentPrincipal is not SmPrincipal");
            }
            return (principal as SmPrincipal);
        }

        public static bool IsCanAccess(string access)
        {
            string[] strArray = StringHelper.Split(access);
            SmIdentity smIdentity = GetSmIdentity();
            if (smIdentity.AdminLevel == AdminLevelType.Admin)
            {
                return true;
            }
            foreach (string str in strArray)
            {
                AdminLevelType notAdmin;
                try
                {
                    notAdmin = (AdminLevelType) Enum.Parse(typeof(AdminLevelType), str);
                    if (smIdentity.AdminLevel == notAdmin)
                    {
                        return true;
                    }
                }
                catch
                {
                    notAdmin = AdminLevelType.NotAdmin;
                }
            }
            return false;
        }

        public static void SetSmPrincipal(SmPrincipal smPrincipal)
        {
            if (HttpContext.Current == null)
            {
                CallContext.SetData("Principal", new LogicalSecurityContextData(smPrincipal));
                if (CurrentPrincipalChanged != null)
                {
                    CurrentPrincipalChanged(smPrincipal, new EventArgs());
                }
            }
            else
            {
                HttpContext.Current.Items["Principal"] = smPrincipal;
            }
        }
    }
}

