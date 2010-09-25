namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.Core;
    using System;

    public static class SMIMHelper
    {
        private static frmMain smimForm;

        public static void Exit()
        {
            if (smimForm != null)
            {
                smimForm.CanClose = true;
                smimForm.Close();
            }
        }

        public static void Login(string server, string user, string password, string nickname, int port, bool useSSL)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("SMIM准备登录到Server:{0}\r\nUser:{1}", new object[] { server, user });
            }
            if (smimForm == null)
            {
                smimForm = new frmMain();
                smimForm.Show();
            }
            smimForm.Login(server, user, password, nickname, port, useSSL);
        }
    }
}

