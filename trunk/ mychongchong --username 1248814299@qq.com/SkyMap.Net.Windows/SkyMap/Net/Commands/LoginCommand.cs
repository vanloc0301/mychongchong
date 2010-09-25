namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Dialogs;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows.Forms;

    public class LoginCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            CancelEventArgs e = new CancelEventArgs();
            SecurityUtil.CurrentPrincipalWillChanged(e);
            if (!e.Cancel)
            {
                NameValueCollection section = (NameValueCollection) ConfigurationManager.GetSection("securitySettings");
                if ((section != null) && ((section["SecurityLogin"] != null) && "true".Equals(section["SecurityLogin"].ToLower())))
                {
                    new LoginByCert().Run();
                }
                else
                {
                    UserLogin login = new UserLogin();
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("Show login dialog");
                    }
                    DialogResult result = login.ShowDialog();
                    login.Close();
                }
                try
                {
                    SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
                    StatusBarService.SetLoginInfo(smPrincipal.DeptNames[0], (smPrincipal.Identity as SmIdentity).UserName);
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}

