namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Tools.Organize;
    using System;

    public class AddRoleTypeCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                CRoleType parentRt = null;
                if (this.Owner is RoleTypeNode)
                {
                    parentRt = ((RoleTypeNode) this.Owner).DomainObject;
                }
                CRoleType type2 = OGMDAOService.Instance.CreateNewRoleType(parentRt);
                if (this.Owner is TaskTreeNode)
                {
                    TaskTreeNode owner = (TaskTreeNode) this.Owner;
                    if (owner != null)
                    {
                        owner.AddSingleNode<CRoleType, RoleTypeNode>(type2);
                    }
                }
            }
        }
    }
}

