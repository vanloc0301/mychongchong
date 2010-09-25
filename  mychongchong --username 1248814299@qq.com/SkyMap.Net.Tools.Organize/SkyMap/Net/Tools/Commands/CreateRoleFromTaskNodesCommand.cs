namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Tools.Organize;
    using System;
    using System.Collections.Generic;

    public class CreateRoleFromTaskNodesCommand : AbstractRoleSecurityCommand
    {
        private void AddChildTaskNodeRoleName(List<string> roleNames, TaskNode tn)
        {
            if (tn.Children.Count > 0)
            {
                foreach (TaskNode node in tn.Children)
                {
                    roleNames.Add(node.Name);
                    this.AddChildTaskNodeRoleName(roleNames, node);
                }
            }
        }

        public override void Run()
        {
            if (this.IsEnabled && (this.Owner is RoleTypeNode))
            {
                RoleTypeNode owner = (RoleTypeNode) this.Owner;
                CRoleType domainObject = owner.DomainObject;
                List<TaskNode> topTaskNode = TaskNode.GetTopTaskNode();
                List<string> roleNames = new List<string>();
                foreach (TaskNode node2 in topTaskNode)
                {
                    roleNames.Add(node2.Name);
                    this.AddChildTaskNodeRoleName(roleNames, node2);
                }
                owner.AddNodes<CRole, RoleNode>(OGMDAOService.Instance.CreateRoles(domainObject, roleNames.ToArray()));
            }
        }
    }
}

