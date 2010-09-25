namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using System.Linq;

    public class RoleTypeNode : AbstractDomainObjectNode<CRoleType>
    {
        public RoleTypeNode()
        {
            this.ContextmenuAddinTreePath = "/Workbench/Pads/ObjectNodes/RoleTypeContextMenu";
        }

        public override ObjectNode AddChild()
        {
            CRole role = OGMDAOService.Instance.CreateRole(base.DomainObject);
            return base.AddSingleNode<CRole, RoleNode>(role);
        }

        protected override void Initialize()
        {
            base.AddNodes<CRoleType, RoleTypeNode>(base.DomainObject.Children);
            base.AddNodes<CRole, RoleNode>(base.DomainObject.Roles.OrderBy<CRole, string>(delegate (CRole p) {
                return p.Name;
            }).ToList<CRole>());
        }
    }
}

