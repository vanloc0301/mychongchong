namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class WfMessageHelper
    {
        private const string subject = "你有新的系统消息邮件";

        private static string CreateMessageBody(string projectId, WfAbnormalType abnormal)
        {
            string str = null;
            if (abnormal != WfAbnormalType.SEND_BACKED)
            {
                return str;
            }
            return string.Format("系统提示您:有任务\"<b>{0}</b>\"被退回，请到退回箱查看并处理！", projectId);
        }

        private static string CreateMessageBody(string projectId, AssignStatusType oldStatus, AssignStatusType newStatus)
        {
            string str = null;
            AssignStatusType type = newStatus;
            if ((type == AssignStatusType.Not_Accepted) && (oldStatus == AssignStatusType.Not_Accepted))
            {
                str = string.Format("系统提示您:有新的任务\"<b>{0}</b>\"分配给你，你还没有签收！", projectId);
            }
            return str;
        }

        private static string GetHtmlMessage(string message)
        {
            if (message != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<html><head></head><body>");
                builder.Append("<div>");
                builder.Append(message);
                builder.Append("</div>");
                builder.Append("<br/><div><b>这是系统邮件，请不要回复</b></div>");
                builder.Append("</body></html>");
                return builder.ToString();
            }
            return null;
        }

        public static void Send(string toStaffId, string projectId, WfAbnormalType abnormal)
        {
            if (EMailHelper.IsUseEMail)
            {
                string message = CreateMessageBody(projectId, abnormal);
                if (message != null)
                {
                    string htmlMessage = GetHtmlMessage(message);
                    if (htmlMessage != null)
                    {
                        CStaff staff = QueryHelper.Get<CStaff>("CStaff_" + toStaffId, toStaffId);
                        if (staff.EMail != null)
                        {
                            EMailHelper.Send(staff.EMail, "你有新的系统消息邮件", htmlMessage);
                        }
                    }
                }
            }
        }

        public static void Send(string toStaffId, string projectId, AssignStatusType oldStatus, AssignStatusType newStatus)
        {
            if (EMailHelper.IsUseEMail)
            {
                string message = CreateMessageBody(projectId, oldStatus, newStatus);
                if (message != null)
                {
                    string htmlMessage = GetHtmlMessage(message);
                    if (htmlMessage != null)
                    {
                        CStaff staff = QueryHelper.Get<CStaff>("CStaff_" + toStaffId, toStaffId);
                        if (staff.EMail != null)
                        {
                            EMailHelper.Send(staff.EMail, "你有新的系统消息邮件", htmlMessage);
                        }
                    }
                }
            }
        }

        public static void Send(string partType, string partIdValue, string projectId, AssignStatusType oldStatus, AssignStatusType newStatus)
        {
            if (EMailHelper.IsUseEMail)
            {
                string message = CreateMessageBody(projectId, oldStatus, newStatus);
                if (message != null)
                {
                    string htmlMessage = GetHtmlMessage(message);
                    IList<CStaff> staffs = OGMService.GetStaffs(partType, partIdValue);
                    if (staffs.Count > 0)
                    {
                        List<string> list2 = new List<string>(staffs.Count);
                        foreach (CStaff staff in staffs)
                        {
                            if (!StringHelper.IsNull(staff.EMail))
                            {
                                list2.Add(staff.EMail);
                            }
                        }
                        EMailHelper.Send(list2.ToArray(), "你有新的系统消息邮件", htmlMessage);
                    }
                }
            }
        }
    }
}

