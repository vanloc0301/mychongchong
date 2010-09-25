namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using SkyMap.Net.XMLExchange.GUI.Nodes;

    public class AddDTProjectCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            UnitOfWork work = new UnitOfWork(typeof(DTColumn));
            DTProject project = new DTProject();
            project.Name = "新XML数据交换项目";
            work.RegisterNew(project);
            work.Commit();
            ((TaskTreeNode) this.Owner).AddSingleNode<DTProject, DTProjectNode>(project);
        }
    }
}

