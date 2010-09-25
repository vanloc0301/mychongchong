namespace SkyMap.Net.Tools.Criteria
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Gui;
    using System;

    public class TySearchXNode : AbstractDomainObjectNode<TySearchx>
    {
        public override ObjectNode AddChild()
        {
            TySearchtable searchtable = RemotingSingletonProvider<CriteriaDAOService>.Instance.CreateSearchtable(base.DomainObject);
            return base.AddSingleNode<TySearchtable, TySearchtableNode>(searchtable);
        }

        protected override void Initialize()
        {
            base.AddNodes<TySearchtable, TySearchtableNode>(base.DomainObject.TySearchtables);
        }
    }
}

