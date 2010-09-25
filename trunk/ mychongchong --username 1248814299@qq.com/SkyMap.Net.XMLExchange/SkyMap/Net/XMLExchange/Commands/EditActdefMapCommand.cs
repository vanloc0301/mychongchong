namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Data.Common;

    public class EditActdefMapCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is DTTableNode)
            {
                DTTableNode owner = this.Owner as DTTableNode;
                ActdefMap map = new ActdefMap(DbProviderFactories.GetFactory(owner.DomainObject.DTDatabase.DSType), owner.DomainObject.DTDatabase.DecryptConnectionString, owner.DomainObject.DTDatabase.DTProject.Name, owner.DomainObject.Name);
                PropertyGridEditPanel.Instance.SelectedObject = map;
            }
        }
    }
}

