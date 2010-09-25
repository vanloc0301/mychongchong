namespace SkyMap.Net.Tools.Nodes
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DataForms;

    public class DAODataTableNode : AbstractDomainObjectNode<DAODataTable>
    {
        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/DAODataTableContextMenu";
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

