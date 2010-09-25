namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.POP3;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Windows.Forms;

    public class EMailTipsCommand : AbstractCommand
    {
        private static bool errUserOrPassword;
        private static int interval;
        private static DateTime? lastRequestTime = null;
        private static StringCollection mails;
        private static string password;
        private static string POP3Host;
        private static string user;

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(EMailTipsCommand.Application_Idle);
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            if (user == null)
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                if (smIdentity != null)
                {
                    CStaff staff = OGMService.GetStaff(smIdentity.UserId);
                    user = staff.EMail;
                    password = staff.EMailPassword;
                }
            }
            if ((!lastRequestTime.HasValue || (DateTime.Now.Subtract(lastRequestTime.Value).Milliseconds > interval)) && (!StringHelper.IsNull(user) && !StringHelper.IsNull(password)))
            {
                try
                {
                    if (!errUserOrPassword)
                    {
                        ObtainNewMails();
                    }
                }
                catch
                {
                }
            }
        }

        private static void ObtainNewMails()
        {
            Pop3Client client = new Pop3Client(user, password, POP3Host);
            try
            {
                client.OpenInbox();
            }
            catch (Pop3LoginException)
            {
                errUserOrPassword = true;
                return;
            }
            int num = 0;
            for (int i = 1; i <= client.MessageCount; i++)
            {
                string mailUidl = client.GetMailUidl(i);
                if (!mails.Contains(mailUidl))
                {
                    num++;
                    mails.Add(mailUidl);
                }
            }
            client.CloseConnection();
            if (num > 0)
            {
                SystemHintHelper.Show(string.Format("你共收到新的消息：{0}条", num));
            }
            lastRequestTime = new DateTime?(DateTime.Now);
        }

        public override void Run()
        {
            NameValueCollection section = (NameValueCollection) ConfigurationManager.GetSection("mailSettings");
            if (section == null)
            {
                LoggingService.Warn("在配置文件中没有找到Mail设置节!");
            }
            else
            {
                POP3Host = section["POP3Host"];
                if (POP3Host == null)
                {
                    LoggingService.Warn("没有正确配置POP3服务器!");
                }
                else
                {
                    SecurityUtil.CurrentPrincipalChanged += new EventHandler(EMailTipsCommand.SecurityUtil_CurrentPrincipalChanged);
                    try
                    {
                        interval = Convert.ToInt32(section["POP3Interval"]);
                    }
                    catch
                    {
                        interval = 0xea60;
                    }
                    mails = new StringCollection();
                }
            }
        }

        private static void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            try
            {
                if (user == null)
                {
                    Application.Idle += new EventHandler(EMailTipsCommand.Application_Idle);
                    Application.ApplicationExit += new EventHandler(EMailTipsCommand.Application_ApplicationExit);
                }
                mails.Clear();
                lastRequestTime = null;
                user = null;
                password = null;
                errUserOrPassword = false;
            }
            catch
            {
            }
        }
    }
}

