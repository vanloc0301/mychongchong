namespace SkyMap.Net.Security.Principal
{
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.Security.Principal;

    [Serializable]
    public class SmIdentity : GenericIdentity
    {
        private AdminLevelType adminLevel;
        private static readonly string authenticationType = "SM";
        private string userId;
        private string userName;

        public SmIdentity(CStaff staff) : base(staff.Id, authenticationType)
        {
            this.userId = staff.Id;
            this.userName = staff.Name;
            this.adminLevel = staff.AdminLevel;
        }

        public AdminLevelType AdminLevel
        {
            get
            {
                return this.adminLevel;
            }
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
        }
    }
}

