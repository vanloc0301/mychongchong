namespace SkyMap.Net.Tools.Criteria
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Criteria;

    public class TyQueryWhereNode : AbstractDomainObjectNode<TyQueryWhere>
    {
        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

