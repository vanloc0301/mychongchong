namespace SkyMap.Net.Tools.Workflow
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class PackageNode : AbstractDomainObjectNode<Package>
    {
        public override ObjectNode AddChild()
        {
            Prodef prodef = new Prodef(base.DomainObject);
            prodef.Id = StringHelper.GetNewGuid();
            prodef.Actdefs = new Dictionary<string, Actdef>();
            prodef.Save();
            return base.AddSingleNode<Prodef, ProdefNode>(prodef);
        }

        protected override void Initialize()
        {
            if (base.DomainObject.Prodefs != null)
            {
                base.AddNodes<Prodef, ProdefNode>(new List<Prodef>(base.DomainObject.Prodefs.Values));
            }
        }
    }
}

