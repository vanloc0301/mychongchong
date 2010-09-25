namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DataForms;

    public class TempletPrintNode : AbstractDomainObjectNode<TempletPrint>
    {
        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/TempletPrintContextMenu";
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

