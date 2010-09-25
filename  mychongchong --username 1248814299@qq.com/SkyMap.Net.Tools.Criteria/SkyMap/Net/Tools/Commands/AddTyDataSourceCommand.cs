namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Tools.Criteria;

    public class AddTyDataSourceCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                SMDataSource source = RemotingSingletonProvider<CriteriaDAOService>.Instance.CreateDataSource();
                owner.AddSingleNode<SMDataSource, TyDataSourceNode>(source);
            }
        }
    }
}

