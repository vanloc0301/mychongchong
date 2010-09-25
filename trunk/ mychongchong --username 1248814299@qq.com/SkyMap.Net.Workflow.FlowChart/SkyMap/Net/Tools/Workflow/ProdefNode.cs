namespace SkyMap.Net.Tools.Workflow
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Workflow.XPDL;

    public class ProdefNode : AbstractDomainObjectNode<Prodef>
    {
        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/ProdefContextMenu";
            }
            set
            {
            }
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

