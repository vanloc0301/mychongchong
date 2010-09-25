namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;

    public class LoginAsAdminCommand : AbstractCommand
    {
        public override void Run()
        {
            CStaff staff = new CStaff();
            staff.Id = "admin";
            staff.Name = "admin";
            staff.AdminLevel = AdminLevelType.Admin;
            SmIdentity identity = new SmIdentity(staff);
            SmPrincipal smPrincipal = new SmPrincipal(identity, null, null);
            SecurityUtil.SetSmPrincipal(smPrincipal);
        }
    }
}

