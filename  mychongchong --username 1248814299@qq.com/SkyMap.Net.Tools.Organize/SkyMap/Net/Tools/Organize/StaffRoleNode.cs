namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.OGM;

    public class StaffRoleNode : AbstractDomainObjectNode<CStaffRole>
    {
        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

