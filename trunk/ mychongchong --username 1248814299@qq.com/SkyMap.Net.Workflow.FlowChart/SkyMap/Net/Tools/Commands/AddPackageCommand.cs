namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Tools.Workflow;

    public class AddPackageCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                Package package = new Package();
                package.Id = StringHelper.GetNewGuid();
                package.Name = "新分类";
                package.Prodefs = new Dictionary<string, Prodef>();
                package.Save();
                owner.AddSingleNode<Package, PackageNode>(package).ActivateItem();
            }
        }
    }
}

