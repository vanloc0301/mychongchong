namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Tools.Criteria;

    public class AddTySearchXCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                TySearchx searchx = RemotingSingletonProvider<CriteriaDAOService>.Instance.CreateSearchX();
                owner.AddSingleNode<TySearchx, TySearchXNode>(searchx);
            }
        }
    }
}

