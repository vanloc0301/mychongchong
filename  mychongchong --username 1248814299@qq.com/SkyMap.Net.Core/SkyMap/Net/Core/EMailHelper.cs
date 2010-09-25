namespace SkyMap.Net.Core
{
    using log4net;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    public static class EMailHelper
    {
        private static string _from;
        private static bool _isAuth = false;
        private static string _server;
        private static string _smtpPwd;
        private static string _smtpUser;
        private static bool isUseEMail = false;
        private static ILog log = LogManager.GetLogger(typeof(EMailHelper));

        static EMailHelper()
        {
            NameValueCollection section = (NameValueCollection) ConfigurationManager.GetSection("mailSettings");
            if (section != null)
            {
                try
                {
                    isUseEMail = Convert.ToBoolean(section["IsUseEMail"]);
                    _server = section["SmtpHost"];
                    _isAuth = Convert.ToBoolean(section["SmtpAuth"]);
                    _from = section["From"];
                    if (_isAuth)
                    {
                        _smtpUser = section["SmtpUser"];
                        _smtpPwd = section["SmtpPwd"];
                    }
                }
                catch
                {
                }
            }
            if (isUseEMail && (_server == null))
            {
                throw new CoreException("没有找到的Smtp主机设置!");
            }
            if (isUseEMail && (_isAuth && ((_smtpUser == null) || (_smtpPwd == null))))
            {
                throw new CoreException("没有配置正确的Smtp发送用户认证信息");
            }
        }

        private static void _client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if ((e.Error != null) && log.IsErrorEnabled)
                {
                    log.Error("发送邮件有错误发生", e.Error);
                }
            }
            catch
            {
            }
        }

        private static MailMessage CreateMailMessage(string from, string[] tos, string subject, string htmlMessage)
        {
            MailMessage message = CreateMailMessage(from, tos[0], subject, htmlMessage);
            for (int i = 1; i < tos.Length; i++)
            {
                message.To.Add(tos[i]);
            }
            return message;
        }

        private static MailMessage CreateMailMessage(string from, string to, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            return message;
        }

        private static void Send(MailMessage mailMessage)
        {
            try
            {
                SmtpClient client = new SmtpClient(_server, 0x19);
                if (_isAuth)
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPwd);
                }
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.SendCompleted += new SendCompletedEventHandler(EMailHelper._client_SendCompleted);
                client.SendAsync(mailMessage, null);
            }
            catch
            {
            }
        }

        public static void Send(string to, string subject, string htmlMessage)
        {
            if ((to != null) && (to.Length != 0))
            {
                Send(CreateMailMessage(_from, to, subject, htmlMessage));
            }
        }

        public static void Send(string[] tos, string subject, string htmlMessage)
        {
            if (tos.Length != 0)
            {
                Send(CreateMailMessage(_from, tos, subject, htmlMessage));
            }
        }

        public static bool IsUseEMail
        {
            get
            {
                return isUseEMail;
            }
        }
    }
}

