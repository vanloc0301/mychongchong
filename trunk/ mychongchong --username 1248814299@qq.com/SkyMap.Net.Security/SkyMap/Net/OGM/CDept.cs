namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class CDept : CAbstractParticipant, ISaveAs
    {
        private IList<CDept> children;
        private string clazz;
        private DateTime et;
        private int order;
        private CDept parent;
        private string parentID;
        private DateTime st;
        private IList<CStaffDept> staffDepts = new List<CStaffDept>();
        private string type;

        public bool Contains(CStaff staff)
        {
            if (this.staffDepts != null)
            {
                foreach (CStaffDept dept in this.staffDepts)
                {
                    if (dept.Staff.Equals(staff))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        [Browsable(false)]
        public IList<CDept> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                this.children = value;
            }
        }

        [DisplayName("级别")]
        public string Clazz
        {
            get
            {
                return this.clazz;
            }
            set
            {
                this.clazz = value;
            }
        }

        [DisplayName("结束时间")]
        public DateTime Et
        {
            get
            {
                return this.et;
            }
            set
            {
                this.et = value;
            }
        }

        [DisplayName("位置")]
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

        [DisplayName("上级部门")]
        public CDept Parent
        {
            get
            {
                if (!((this.parent != null) || string.IsNullOrEmpty(this.parentID)))
                {
                    this.parent = OGMService.GetDept(this.parentID);
                }
                return this.parent;
            }
            set
            {
                this.parent = value;
                this.parentID = (value == null) ? null : value.Id;
            }
        }

        [Browsable(false)]
        public string ParentID
        {
            get
            {
                return this.parentID;
            }
            set
            {
                this.parentID = value;
            }
        }

        [DisplayName("开始时间")]
        public DateTime St
        {
            get
            {
                return this.st;
            }
            set
            {
                this.st = value;
            }
        }

        [Browsable(false)]
        public IList<CStaffDept> StaffDepts
        {
            get
            {
                return this.staffDepts;
            }
            set
            {
                this.staffDepts = value;
            }
        }

        [DisplayName("类型")]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

