namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.OGM;

    public class UniversalAuthNode : AbstractDomainObjectNode<CUniversalAuth>
    {
        public UniversalAuthNode()
        {
            this.ContextmenuAddinTreePath = "/Workbench/Pads/ObjectNodes/AuthTypeContextMenu";
        }

        public override ObjectNode AddChild()
        {
            return null;
        }

        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

