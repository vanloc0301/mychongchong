namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;

    [Serializable]
    public class CStaffRole : DomainObject
    {
        private int order;
        private CRole role;
        private string roleID;
        private CStaff staff;
        private string staffID;

        public override string ToString()
        {
            return this.Staff.Name;
        }

        public override string Name
        {
            get
            {
                return this.Staff.Name;
            }
            set
            {
            }
        }

        public int Order
        {
            get
            {
                return this.order;
            }
            set
            {
                this.order = value;
            }
        }

        public CRole Role
        {
            get
            {
                if (!((this.role != null) || string.IsNullOrEmpty(this.roleID)))
                {
                    this.role = OGMService.GetRole(this.roleID);
                }
                return this.role;
            }
            set
            {
                this.role = value;
                this.roleID = value.Id;
            }
        }

        public string RoleID
        {
            get
            {
                return this.roleID;
            }
            set
            {
                this.roleID = value;
            }
        }

        public virtual CStaff Staff
        {
            get
            {
                if (!((this.staff != null) || string.IsNullOrEmpty(this.staffID)))
                {
                    this.staff = OGMService.GetStaff(this.staffID);
                }
                return this.staff;
            }
            set
            {
                this.staff = value;
                this.staffID = value.Id;
            }
        }

        public string StaffID
        {
            get
            {
                return this.staffID;
            }
            set
            {
                this.staffID = value;
            }
        }
    }
}

