namespace SkyMap.Net.Tools.Criteria
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DAO;

    public class TyDataSourceNode : AbstractDomainObjectNode<SMDataSource>
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

