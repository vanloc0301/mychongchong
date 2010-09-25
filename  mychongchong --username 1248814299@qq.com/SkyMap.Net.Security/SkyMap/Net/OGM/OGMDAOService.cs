namespace SkyMap.Net.OGM
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class OGMDAOService : MarshalByRefObject
    {
        private static OGMDAOService instance;

        public CAuthType CreateAuthType()
        {
            CAuthType type = new CAuthType();
            type.Id = StringHelper.GetNewGuid();
            type.Name = "新的权限管理";
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(type);
            currentUnitOfWork.Commit();
            return type;
        }

        public CDept CreateDept(CDept dept)
        {
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            CDept dept2 = new CDept();
            dept2.Id = StringHelper.GetNewGuid();
            dept2.Parent = dept;
            dept2.Name = "新部门";
            dept2.St = DateTime.Today;
            currentUnitOfWork.RegisterNew(dept2);
            currentUnitOfWork.Commit();
            return dept2;
        }

        public CRoleType CreateNewRoleType(CRoleType parentRt)
        {
            CRoleType item = new CRoleType();
            item.Id = StringHelper.GetNewGuid();
            item.Name = "新角色类型";
            item.Parent = parentRt;
            item.Children = new List<CRoleType>();
            item.Roles = new List<CRole>();
            if (parentRt != null)
            {
                parentRt.Children.Add(item);
            }
            item.Save();
            return item;
        }

        private static CStaffDept CreateNotSavedStaffDept(CStaff staff, CDept dept)
        {
            CStaffDept item = new CStaffDept();
            item.Id = StringHelper.GetNewGuid();
            item.Staff = staff;
            item.Dept = dept;
            item.Order = dept.StaffDepts.Count;
            dept.StaffDepts.Add(item);
            staff.StaffDepts.Add(item);
            return item;
        }

        public CParticipant CreateParticipant()
        {
            CParticipant participant = new CParticipant();
            participant.Id = StringHelper.GetNewGuid();
            participant.Name = "新参与者";
            participant.ParticipantEntity = new ParticipantEntity();
            participant.ParticipantEntity.Type = "ROLE";
            participant.ParticipantEntity.IdValue = string.Empty;
            participant.AssignRule = AssignRuleType.FCFA;
            participant.Description = string.Empty;
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(participant);
            currentUnitOfWork.Commit();
            return participant;
        }

        public CRole CreateRole(CRoleType rt)
        {
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            CRole item = new CRole();
            item.Id = StringHelper.GetNewGuid();
            item.Type = rt;
            item.Name = "新角色";
            item.StaffRoles = new List<CStaffRole>();
            rt.Roles.Add(item);
            currentUnitOfWork.RegisterNew(item);
            currentUnitOfWork.Commit();
            return item;
        }

        public List<CRole> CreateRoles(CRoleType rt, string[] roleNames)
        {
            List<CRole> list = new List<CRole>();
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            foreach (string str in roleNames)
            {
                if (!OGMService.RoleExists(str))
                {
                    CRole item = new CRole();
                    item.Id = StringHelper.GetNewGuid();
                    item.Type = rt;
                    item.Name = str;
                    item.StaffRoles = new List<CStaffRole>();
                    rt.Roles.Add(item);
                    list.Add(item);
                    currentUnitOfWork.RegisterNew(item);
                }
            }
            try
            {
                currentUnitOfWork.Commit();
            }
            catch
            {
                return null;
            }
            return list;
        }

        public CStaff CreateStaff(CDept dept)
        {
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            CStaff staff = new CStaff();
            staff.Id = StringHelper.GetNewGuid();
            staff.Id = StringHelper.GetNewGuid();
            staff.Name = "新人员";
            staff.Description = string.Empty;
            staff.StaffDepts = new List<CStaffDept>();
            staff.StaffRoles = new List<CStaffRole>();
            staff.St = new DateTime?(DateTime.Now);
            staff.UserName = staff.Id;
            staff.AdminLevel = AdminLevelType.NotAdmin;
            currentUnitOfWork.RegisterNew(staff);
            CStaffDept dept2 = CreateNotSavedStaffDept(staff, dept);
            currentUnitOfWork.RegisterNew(dept2);
            currentUnitOfWork.Commit();
            return staff;
        }

        public CStaffDept CreateStaffDept(CStaff staff, CDept dept)
        {
            CStaffDept dept2 = CreateNotSavedStaffDept(staff, dept);
            dept2.Save();
            return dept2;
        }

        public CStaffRole CreateStaffRole(CStaff staff, CRole role)
        {
            LoggingService.DebugFormatted("角色ID:{0},人员ID:{1}", new object[] { role.Id, staff.Id });
            CStaffRole item = new CStaffRole();
            item.Id = StringHelper.GetNewGuid();
            item.Staff = staff;
            item.Role = role;
            item.Order = role.StaffRoles.Count;
            staff.StaffRoles.Add(item);
            role.StaffRoles.Add(item);
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(item);
            currentUnitOfWork.RegisterDirty(role);
            currentUnitOfWork.Commit();
            return item;
        }

        public CUniversalAuth CreateUniversalAuth(CAuthType authType)
        {
            CUniversalAuth item = new CUniversalAuth();
            item.Id = StringHelper.GetNewGuid();
            item.Type = authType;
            authType.UniversalAuths.Add(item);
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(item);
            currentUnitOfWork.Commit();
            return item;
        }

        public void Remove(DomainObject domainObject)
        {
            domainObject.Delete();
        }

        public bool RoleExists(string roleName)
        {
            return (QueryHelper.List<CRole>(string.Empty, new string[] { "Name" }, new string[] { roleName }).Count != 0);
        }

        public IList<CAuthType> AuthTypes
        {
            get
            {
                return QueryHelper.List<CAuthType>("ALL_CAuthType");
            }
        }

        private UnitOfWork CurrentUnitOfWork
        {
            get
            {
                return new UnitOfWork(base.GetType());
            }
        }

        public static OGMDAOService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OGMDAOService();
                }
                return instance;
            }
        }

        public IList<CParticipant> Participants
        {
            get
            {
                return QueryHelper.List<CParticipant>("ALL_CParticipant");
            }
        }
    }
}

