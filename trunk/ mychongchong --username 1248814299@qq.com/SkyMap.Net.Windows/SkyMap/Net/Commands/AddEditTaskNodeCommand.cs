namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;

    public class AddEditTaskNodeCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                TaskNode node = new TaskNode();
                node.Id = StringHelper.GetNewGuid();
                node.Name = "新工作控制节点";
                node.Description = node.Name;
                node.Save();
                ((ObjectNode) this.Owner).AddSingleNode<TaskNode, EditTaskNode>(node);
            }
        }
    }
}

