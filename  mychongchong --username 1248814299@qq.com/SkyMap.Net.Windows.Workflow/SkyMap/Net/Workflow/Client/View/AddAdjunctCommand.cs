namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Instance;
    using System;

    public class AddAdjunctCommand : AdjunctEditCommand
    {
        public override void Run()
        {
            WfAdjunct adjunct = new WfAdjunct();
            if (base.SelectFileToAdjuct(adjunct))
            {
                adjunct.Id = StringHelper.GetNewGuid();
                adjunct.ProinstId = base.ae.ProinstId;
                adjunct.CreateDate = new DateTime?(DateTimeHelper.GetNow());
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                adjunct.CreateStaffId = smIdentity.UserId;
                adjunct.CreateStaffName = smIdentity.UserName;
                base.ae.CurrentUnitOfWork.RegisterNew(adjunct);
                base.ae.CurrentUnitOfWork.RegisterDirty(adjunct.File);
                base.ae.AddAdjunct(adjunct);
            }
        }
    }
}

