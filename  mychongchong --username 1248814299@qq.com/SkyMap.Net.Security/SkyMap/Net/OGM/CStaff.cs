namespace SkyMap.Net.OGM
{
    using SkyMap.Net.Components;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class CStaff : CAbstractParticipant, ISaveAs
    {
        private AdminLevelType adminLevel;
        private string email;
        private string emailpassword;
        private DateTime? et;
        private string password;
        private DateTime? st;
        private IList<CStaffDept> staffDepts = new List<CStaffDept>();
        private IList<CStaffRole> staffRoles = new List<CStaffRole>();
        private string userName;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (CStaffDept dept in this.staffDepts)
            {
                unitOfWork.RegisterNew(dept);
            }
        }

        [DisplayName("管理员权限")]
        public AdminLevelType AdminLevel
        {
            get
            {
                return this.adminLevel;
            }
            set
            {
                this.adminLevel = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DisplayName("所属部门"), Editor("SkyMap.Net.Gui.Components.GridEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), Grid(null, null, false, new string[] { "Dept", "Position", "Order" }, new string[] { "姓名", "任职", "顺序" }, new string[] {  })]
        public List<CStaffDept> DesignStaffDepts
        {
            get
            {
                return new List<CStaffDept>(this.staffDepts);
            }
        }

        [Editor("SkyMap.Net.Gui.Components.GridEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DisplayName("所属角色"), Grid(null, null, false, new string[] { "Role", "Order" }, new string[] { "角色", "顺序" }, new string[] {  })]
        public List<CStaffRole> DesignStaffRoles
        {
            get
            {
                return new List<CStaffRole>(this.staffRoles);
            }
        }

        [DisplayName("密码")]
        public string EditPassword
        {
            get
            {
                CryptoHelper helper = new CryptoHelper(CryptoTypes.encTypeDES);
                return helper.Decrypt(this.password, this.Id);
            }
            set
            {
                this.password = new CryptoHelper(CryptoTypes.encTypeDES).Encrypt(value, this.Id);
            }
        }

        [DisplayName("电子邮件")]
        public string EMail
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        [DisplayName("电子邮件密码")]
        public string EMailPassword
        {
            get
            {
                return this.emailpassword;
            }
            set
            {
                this.emailpassword = value;
            }
        }

        [DisplayName("离职日期")]
        public DateTime? Et
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

        [Browsable(false)]
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        [DisplayName("入职日期")]
        public DateTime? St
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

        [Browsable(false)]
        public IList<CStaffRole> StaffRoles
        {
            get
            {
                return this.staffRoles;
            }
            set
            {
                this.staffRoles = value;
            }
        }

        [DisplayName("用户名(须唯一)")]
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }
    }
}

