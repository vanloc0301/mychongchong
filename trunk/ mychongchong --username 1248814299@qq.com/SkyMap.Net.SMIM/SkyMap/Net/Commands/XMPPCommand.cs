namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.SMIM;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Windows.Forms;

    public class XMPPCommand : AbstractCommand
    {
        private static string jabberServer;
        private static int port;
        private static bool useSSL = false;

        private void Application_ThreadExit(object sender, EventArgs e)
        {
            SMIMHelper.Exit();
        }

        public override void Run()
        {
            NameValueCollection section = (NameValueCollection) ConfigurationManager.GetSection("jabberSettings");
            if (section == null)
            {
                LoggingService.Warn("在配置文件中没有找到Jabber设置节!");
            }
            else
            {
                try
                {
                    jabberServer = section["JabberServer"];
                    port = int.Parse(section["JabberPort"]);
                    useSSL = bool.Parse(section["UseSSL"]);
                }
                catch
                {
                }
                if (jabberServer == null)
                {
                    LoggingService.Error("在Jabber配置节没有的找到JabberServer");
                }
                else
                {
                    this.ShowXMPP();
                    SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
                    Application.ThreadExit += new EventHandler(this.Application_ThreadExit);
                }
            }
        }

        private void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            this.ShowXMPP();
        }

        private void ShowXMPP()
        {
            try
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                if (smIdentity != null)
                {
                    CStaff staff = OGMService.GetStaff(smIdentity.UserId);
                    string userName = staff.UserName;
                    string s = staff.UserName;
                    if (!(StringHelper.IsNull(userName) || StringHelper.IsNull(s)))
                    {
                        SMIMHelper.Login(jabberServer, userName, s, staff.Name, port, useSSL);
                    }
                }
            }
            catch
            {
            }
        }
    }
}

