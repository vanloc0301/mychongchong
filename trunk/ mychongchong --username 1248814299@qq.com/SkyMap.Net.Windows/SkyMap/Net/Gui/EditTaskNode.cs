namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;

    public class EditTaskNode : AbstractDomainObjectNode<TaskNode>
    {
        public override ObjectNode AddChild()
        {
            TaskNode node = new TaskNode("新工作权节点", base.DomainObject);
            node.Id = StringHelper.GetNewGuid();
            node.Save();
            return base.AddSingleNode<TaskNode, EditTaskNode>(node);
        }

        protected override void Initialize()
        {
            base.Initialize();
            base.AddNodes<TaskNode, EditTaskNode>(base.DomainObject.Children);
        }
    }
}

