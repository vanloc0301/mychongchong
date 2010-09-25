namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Tools.Organize;
    using System;

    public class AddDeptCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            ObjectNode owner = (ObjectNode) this.Owner;
            if (owner != null)
            {
                CDept domainObject = null;
                if (owner is DeptNode)
                {
                    domainObject = ((DeptNode) owner).DomainObject;
                }
                CDept dept2 = OGMDAOService.Instance.CreateDept(domainObject);
                owner.AddSingleNode<CDept, DeptNode>(dept2);
            }
        }
    }
}

