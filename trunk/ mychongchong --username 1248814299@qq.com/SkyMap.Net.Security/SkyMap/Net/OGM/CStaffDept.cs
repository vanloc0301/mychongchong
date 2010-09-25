namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;

    [Serializable]
    public class CStaffDept : DomainObject
    {
        private CDept dept;
        private string deptID;
        private int order;
        private string position;
        private CStaff staff;
        private string staffID;

        public override string ToString()
        {
            return this.Staff.Name;
        }

        public CDept Dept
        {
            get
            {
                if (!((this.dept != null) || string.IsNullOrEmpty(this.deptID)))
                {
                    this.dept = OGMService.GetDept(this.deptID);
                }
                return this.dept;
            }
            set
            {
                this.dept = value;
                this.deptID = value.Id;
            }
        }

        public string DeptID
        {
            get
            {
                return this.deptID;
            }
            set
            {
                this.deptID = value;
            }
        }

        public override string Name
        {
            get
            {
                return this.staff.Name;
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

        public string Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public CStaff Staff
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

