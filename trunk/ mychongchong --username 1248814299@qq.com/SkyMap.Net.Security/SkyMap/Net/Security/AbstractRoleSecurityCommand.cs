namespace SkyMap.Net.Security
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security.Principal;
    using System;

    public abstract class AbstractRoleSecurityCommand : AbstractMenuCommand
    {
        protected AbstractRoleSecurityCommand()
        {
        }

        public override bool IsEnabled
        {
            get
            {
                bool flag = false;
                SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
                SmIdentity identity = smPrincipal.Identity as SmIdentity;
                if (identity.AdminLevel != AdminLevelType.NotAdmin)
                {
                    flag = true;
                }
                if (!flag)
                {
                    flag = smPrincipal.IsInRole(base.GetType().FullName);
                }
                if (!flag)
                {
                    LoggingService.WarnFormatted("初步的权限检查对于用户:{0}没有权限使用'{1}'", new object[] { identity.Name, base.GetType().Name });
                }
                return flag;
            }
            set
            {
            }
        }

        public override bool Visible
        {
            get
            {
                return this.IsEnabled;
            }
            set
            {
            }
        }
    }
}

