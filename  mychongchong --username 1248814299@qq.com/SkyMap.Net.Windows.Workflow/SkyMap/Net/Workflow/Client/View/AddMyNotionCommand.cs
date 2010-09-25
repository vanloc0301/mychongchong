namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Instance;
    using System;

    public class AddMyNotionCommand : StaffNotionCommand
    {
        public override void Run()
        {
            if (base.sn.CanAddMyNotion())
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                WfStaffNotion notion = new WfStaffNotion();
                notion.ProinstId = base.sn.ProinstId;
                notion.AssignId = base.sn.AssignId;
                notion.StaffId = smIdentity.UserId;
                notion.StaffName = smIdentity.UserName;
                notion.Date = new DateTime?(DateTimeHelper.GetNow());
                base.sn.CurrentUnitOfWork.RegisterNew(notion);
                base.sn.AddStaffNotion(notion);
            }
            else
            {
                MessageHelper.ShowInfo("你不能添加经办人意见");
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return base.sn.CanAddMyNotion();
            }
            set
            {
            }
        }
    }
}

