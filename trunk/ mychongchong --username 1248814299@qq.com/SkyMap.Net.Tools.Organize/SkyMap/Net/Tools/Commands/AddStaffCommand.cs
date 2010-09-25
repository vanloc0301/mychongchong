namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Tools.Organize;
    using System;

    public class AddStaffCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            DeptNode owner = (DeptNode) this.Owner;
            if (owner != null)
            {
                owner.AddChild();
                CStaff staff = OGMDAOService.Instance.CreateStaff(owner.DomainObject);
                owner.AddSingleNode<CStaff, StaffNode>(staff);
            }
        }
    }
}

