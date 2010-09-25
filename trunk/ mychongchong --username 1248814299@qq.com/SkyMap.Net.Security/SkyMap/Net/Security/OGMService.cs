namespace SkyMap.Net.Security
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Remoting.Messaging;

    public static class OGMService
    {
        private const string ALL = "ALL";
        private const string ALLSTAFF = "ALLSTAFF";
        private const string CACHEKEY_DEPT = "OGM_DEPTS";
        private const string CACHEKEY_ROLE = "OGM_ROLES";
        private const string CACHEKEY_ROLETYPE = "OGM_ROLETYPE";
        private const string CACHEKEY_STAFF = "OGM_STAFFS";
        private static object curretnUserCert;
        private const string DEPT = "DEPT";
        private const string ROLE = "ROLE";
        private const string STAFF = "STAFF";

        static OGMService()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OGMService.CurrentDomain_ProcessExit);
        }

        public static void CheckLoginStatus()
        {
        }

        public static bool CheckPassword(CStaff staff, string password)
        {
            string str = string.Empty;
            str = new CryptoHelper(CryptoTypes.encTypeDES).Encrypt(password, staff.Id);
            try
            {
                return (Convert.ToInt32(QueryHelper.ExecuteSqlScalar("SkyMap.Net.OGM", string.Format("select count(1) from OG_STAFF where staff_id='{0}' and (STAFF_PWD='{1}' or (STAFF_PWD IS NULL AND '{1}'=''))", staff.Id, str))) == 1);
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return false;
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            RegisterLoginOut();
        }

        public static IList<CDept> GetAllDept()
        {
            List<CDept> list = new List<CDept>();
            foreach (CDept dept in Depts)
            {
                if (dept.IsActive)
                {
                    list.Add(dept);
                }
            }
            return list;
        }

        public static CDept GetDept(string deptID)
        {
            foreach (CDept dept in Depts)
            {
                if (dept.Id == deptID)
                {
                    return dept;
                }
            }
            return null;
        }

        private static T GetParticipant<T>(IList<T> parts, string partIdValue) where T: CAbstractParticipant
        {
            foreach (T local in parts)
            {
                if (local.Id == partIdValue)
                {
                    return local;
                }
            }
            throw new SecurityException("Cannot find the participant :" + partIdValue);
        }

        private static string[] GetParticipantsOfStaff(string staffId)
        {
            string[] strArray = null;
            string key = "ParticipantsOfStaff_" + staffId;
            if (DAOCacheService.Contains(key))
            {
                return (string[]) DAOCacheService.Get(key);
            }
            strArray = new List<string>(QueryHelper.List<string>(typeof(CStaff).Namespace, "GetParticipantsOfStaff_" + staffId, "GetParticipantsOfStaff", new string[] { "staff" }, (object[]) new string[] { staffId })).ToArray();
            DAOCacheService.Put(key, strArray);
            return strArray;
        }

        public static CRole GetRole(string roleID)
        {
            foreach (CRole role in Roles)
            {
                if (role.Id == roleID)
                {
                    return role;
                }
            }
            return null;
        }

        public static CStaff GetStaff(string id)
        {
            foreach (CStaff staff in Staffs)
            {
                if (staff.Id == id)
                {
                    return staff;
                }
            }
            throw new SecurityException("Cannot find the staff : " + id);
        }

        public static CStaff GetStaffByUserName(string userName)
        {
            foreach (CStaff staff in Staffs)
            {
                if (staff.UserName == userName)
                {
                    return staff;
                }
            }
            throw new SecurityException(string.Format("Cannot find the staff : {0}", userName));
        }

        public static CStaffDept GetStaffDept(string deptId, string staffId)
        {
            try
            {
                foreach (CDept dept in Depts)
                {
                    if (dept.Id == deptId)
                    {
                        foreach (CStaffDept dept2 in dept.StaffDepts)
                        {
                            if (dept2.StaffID == staffId)
                            {
                                return dept2;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            return null;
        }

        public static List<CStaff> GetStaffs()
        {
            List<CStaff> list = new List<CStaff>();
            foreach (CStaff staff in Staffs)
            {
                if (staff.IsActive)
                {
                    list.Add(staff);
                }
            }
            return list;
        }

        public static List<CStaff> GetStaffs(CRole role)
        {
            List<CStaff> staffsByIds = null;
            string key = "CStaff_RoleID_" + role.Id;
            if (!DAOCacheService.Contains(key))
            {
                List<string> staffIds = new List<string>();
                foreach (CStaffRole role2 in role.StaffRoles)
                {
                    if (role2.IsActive)
                    {
                        staffIds.Add(role2.StaffID);
                    }
                }
                staffsByIds = GetStaffsByIds(staffIds);
                DAOCacheService.Put(key, staffsByIds);
                return staffsByIds;
            }
            return (DAOCacheService.Get(key) as List<CStaff>);
        }

        public static List<CStaff> GetStaffs(IList<string> roleIds)
        {
            List<CStaff> list = new List<CStaff>();
            foreach (string str in roleIds)
            {
                CRole role = GetRole(str);
                if (role != null)
                {
                    list.AddRange(GetStaffs(role));
                }
            }
            return list;
        }

        public static List<CStaff> GetStaffs(CDept dept)
        {
            string key = "CStaff_Dept_" + dept.Id;
            if (!DAOCacheService.Contains(key))
            {
                List<string> staffIds = new List<string>(dept.StaffDepts.Count);
                foreach (CStaffDept dept2 in dept.StaffDepts)
                {
                    if (dept2.IsActive)
                    {
                        staffIds.Add(dept2.StaffID);
                    }
                }
                List<CStaff> staffsByIds = GetStaffsByIds(staffIds);
                DAOCacheService.Put(key, staffsByIds);
                return staffsByIds;
            }
            return (DAOCacheService.Get(key) as List<CStaff>);
        }

        private static List<CStaff> GetStaffs(CStaff staff)
        {
            List<CStaff> list = new List<CStaff>();
            list.Add(staff);
            return list;
        }

        public static List<CStaff> GetStaffs(CDept dept, bool includeChildDept)
        {
            List<CStaff> list = new List<CStaff>();
            list.AddRange(GetStaffs(dept));
            if (includeChildDept)
            {
                foreach (CDept dept2 in dept.Children)
                {
                    list.AddRange(GetStaffs(dept2, true));
                }
            }
            return list;
        }

        public static List<CStaff> GetStaffs(string partType, string partIdValue)
        {
            switch (partType.ToUpper())
            {
                case "ROLE":
                    return GetStaffs(GetParticipant<CRole>(Roles, partIdValue));

                case "DEPT":
                    return GetStaffs(GetParticipant<CDept>(Depts, partIdValue));

                case "STAFF":
                    return GetStaffs(GetParticipant<CStaff>(Staffs, partIdValue));

                case "ALL":
                    if (partIdValue != "ALLSTAFF")
                    {
                        throw new SecurityException("Cannot know the all participant type!");
                    }
                    return GetStaffs();
            }
            throw new SecurityException("Not implement the participant type :" + partType);
        }

        private static List<CStaff> GetStaffsByIds(IList<string> staffIds)
        {
            List<CStaff> list = new List<CStaff>(staffIds.Count);
            foreach (CStaff staff in Staffs)
            {
                if (staffIds.Contains(staff.Id))
                {
                    staffIds.Remove(staff.Id);
                    if (staff.IsActive)
                    {
                        list.Add(staff);
                    }
                }
                if (staffIds.Count == 0)
                {
                    return list;
                }
            }
            return list;
        }

        public static IList<CStaff> GetStaffsOfCurrentStaffDepts()
        {
            int num;
            string[] deptIds = SecurityUtil.GetSmPrincipal().DeptIds;
            List<string> list = new List<string>();
            CDept participant = null;
            List<CStaff> list2 = new List<CStaff>();
            CStaff[] collection = null;
        Label_0020:
            num = 0;
            while (num < deptIds.Length)
            {
                participant = GetParticipant<CDept>(Depts, deptIds[num]);
                if (participant != null)
                {
                    collection = GetStaffs(participant).ToArray();
                    if (collection.Length > 0)
                    {
                        list2.AddRange(collection);
                    }
                    if (participant.Parent != null)
                    {
                        list.Add(participant.Parent.Id);
                    }
                }
                num++;
            }
            if ((list2.Count == 0) && (list.Count > 0))
            {
                deptIds = list.ToArray();
                goto Label_0020;
            }
            return list2;
        }

        public static List<CDept> GetTopDepts()
        {
            List<CDept> list = new List<CDept>(1);
            foreach (CDept dept in Depts)
            {
                if (string.IsNullOrEmpty(dept.ParentID))
                {
                    list.Add(dept);
                }
            }
            return list;
        }

        internal static void InitOGMData()
        {
            LoggingService.DebugFormatted("将一次性的获取所有人员，角色类型，角色与部门...", new object[0]);
            KeyValuePair<string, object[]>[] querys = new KeyValuePair<string, object[]>[] { new KeyValuePair<string, object[]>("GetStaffs", null), new KeyValuePair<string, object[]>("GetRoleTypes", null), new KeyValuePair<string, object[]>("GetRoles", null), new KeyValuePair<string, object[]>("GetDepts", null) };
            ArrayList list = QueryHelper.List<CStaff, CRoleType, CRole, CDept>(typeof(CDept).Namespace, "ALL_Staff_RoleType_Role_Dept", querys);
            DAOCacheService.Put("OGM_STAFFS", list[0]);
            DAOCacheService.Put("OGM_ROLETYPE", list[1]);
            DAOCacheService.Put("OGM_ROLES", list[2]);
            DAOCacheService.Put("OGM_DEPTS", list[3]);
        }

        public static List<CStaffDept> LazyLoadStaffDepts(CDept detp)
        {
            LoggingService.DebugFormatted("获取部门：{0}所有有效员工...", new object[] { detp.Name });
            List<CStaffDept> list = new List<CStaffDept>();
            IList<CStaff> staffs = Staffs;
            foreach (CStaffDept dept in detp.StaffDepts)
            {
                if (dept.IsActive)
                {
                    foreach (CStaff staff in staffs)
                    {
                        if (staff.IsActive && staff.StaffDepts.Contains(dept))
                        {
                            dept.Staff = staff;
                            LoggingService.DebugFormatted("{0}-{1},在部门状态：{2},本身状态:{3}", new object[] { detp.Name, staff.Name, dept.IsActive, staff.IsActive });
                            list.Add(dept);
                            break;
                        }
                    }
                }
            }
            return list;
        }

        private static void RegisterLogin(SmPrincipal smPrin)
        {
            string hostName = Dns.GetHostName();
            CallContext.SetData("LogHostName", hostName);
            CallContext.SetData("LogHostIP", Dns.GetHostAddresses(hostName));
        }

        private static void RegisterLoginOut()
        {
        }

        public static bool RoleExists(string roleName)
        {
            foreach (CRole role in Roles)
            {
                if (role.Name == roleName)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetNewPassword(CStaff staff, string password)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("将修改用户'{0}'的密码", new object[] { staff.Name });
            }
            staff.EditPassword = password;
            UnitOfWork work = new UnitOfWork(typeof(CStaff));
            work.RegisterDirty(staff);
            work.Commit();
            DAOCacheService.UpdateVersion();
            InitOGMData();
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("修改密码完成!");
            }
        }

        public static void SetSmPrincipal(CStaff staff)
        {
            try
            {
                int num2;
                SmIdentity identity = new SmIdentity(staff);
                int count = staff.StaffRoles.Count;
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();
                CStaffRole role = null;
                for (num2 = 0; num2 < count; num2++)
                {
                    role = staff.StaffRoles[num2];
                    if ((role.Role != null) && role.IsActive)
                    {
                        list.Add(role.Role.Id);
                        list2.Add(role.Role.Name);
                    }
                }
                count = staff.StaffDepts.Count;
                List<string> list3 = new List<string>();
                List<string> list4 = new List<string>();
                CStaffDept dept = null;
                for (num2 = 0; num2 < count; num2++)
                {
                    dept = staff.StaffDepts[num2];
                    if ((dept.Dept != null) && dept.IsActive)
                    {
                        list3.Add(dept.Dept.Id);
                        list4.Add(dept.Dept.Name);
                    }
                }
                string[] participantsOfStaff = GetParticipantsOfStaff(staff.Id);
                SmPrincipal smPrincipal = new SmPrincipal(identity, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), participantsOfStaff);
                SecurityUtil.SetSmPrincipal(smPrincipal);
                RegisterLogin(smPrincipal);
            }
            catch (ApplicationException exception)
            {
                throw exception;
            }
        }

        public static void SetSmPrincipal(CStaff staff, CDept dept)
        {
            try
            {
                int num2;
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("'{0}'登录了系统!", new object[] { staff.Name });
                }
                SmIdentity identity = new SmIdentity(staff);
                int count = staff.StaffRoles.Count;
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();
                CStaffRole role = null;
                for (num2 = 0; num2 < count; num2++)
                {
                    role = staff.StaffRoles[num2];
                    if ((role.Role != null) && role.IsActive)
                    {
                        list.Add(role.Role.Id);
                        list2.Add(role.Role.Name);
                    }
                }
                count = staff.StaffDepts.Count;
                List<string> list3 = new List<string>();
                List<string> list4 = new List<string>();
                list3.Add(dept.Id);
                list4.Add(dept.Name);
                CStaffDept dept2 = null;
                for (num2 = 0; num2 < count; num2++)
                {
                    dept2 = staff.StaffDepts[num2];
                    if (((dept2.Dept != null) && dept2.IsActive) && (dept2.Dept.Id != dept.Id))
                    {
                        list3.Add(dept2.Dept.Id);
                        list4.Add(dept2.Dept.Name);
                    }
                }
                string[] participantsOfStaff = GetParticipantsOfStaff(staff.Id);
                SmPrincipal smPrincipal = new SmPrincipal(identity, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), participantsOfStaff);
                SecurityUtil.SetSmPrincipal(smPrincipal);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("成功设定系统用户标识,准备在服务器对些登录进行登记!", new object[0]);
                }
                RegisterLogin(smPrincipal);
            }
            catch (Exception exception)
            {
                throw new SecurityException("Set current Principle raise error:\r\n" + exception.Message, exception);
            }
        }

        public static object CurrentUserCert
        {
            get
            {
                return curretnUserCert;
            }
            set
            {
                curretnUserCert = value;
            }
        }

        public static IList<CDept> Depts
        {
            get
            {
                if (!DAOCacheService.Contains("OGM_STAFFS"))
                {
                    InitOGMData();
                }
                return (DAOCacheService.Get("OGM_DEPTS") as IList<CDept>);
            }
        }

        public static IList<CRole> Roles
        {
            get
            {
                if (!DAOCacheService.Contains("OGM_STAFFS"))
                {
                    InitOGMData();
                }
                return (DAOCacheService.Get("OGM_ROLES") as IList<CRole>);
            }
        }

        public static IList<CStaff> Staffs
        {
            get
            {
                if (!DAOCacheService.Contains("OGM_STAFFS"))
                {
                    InitOGMData();
                }
                return (DAOCacheService.Get("OGM_STAFFS") as IList<CStaff>);
            }
        }

        public static IList<CRoleType> TopRoleTypes
        {
            get
            {
                if (!DAOCacheService.Contains("OGM_ROLETYPE"))
                {
                    InitOGMData();
                }
                return (DAOCacheService.Get("OGM_ROLETYPE") as IList<CRoleType>);
            }
        }
    }
}

