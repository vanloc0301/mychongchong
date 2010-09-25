namespace SkyMap.Net.OGM
{
    using SkyMap.Net.Components;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class CRole : CAbstractParticipant, ISaveAs
    {
        private IList<CStaffRole> staffRoles = new List<CStaffRole>();
        private CRoleType type;
        private string typeid;

        public bool Contains(CStaff staff)
        {
            if (this.staffRoles != null)
            {
                foreach (CStaffRole role in this.staffRoles)
                {
                    if (role.Staff.Equals(staff))
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

        [DisplayName("查看角色成员"), Editor("SkyMap.Net.Gui.Components.GridEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), Grid(null, null, false, new string[] { "Name", "Order" }, new string[] { "姓名", "位置" }, new string[] {  })]
        public List<CStaffRole> StaffRoles
        {
            get
            {
                return new List<CStaffRole>(this.staffRoles);
            }
            set
            {
                this.staffRoles = value;
            }
        }

        [DisplayName("类型"), Browsable(false)]
        public CRoleType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                this.typeid = value.Id;
            }
        }

        [Browsable(false)]
        public string TypeID
        {
            get
            {
                return this.typeid;
            }
            set
            {
                this.typeid = value;
            }
        }
    }
}

