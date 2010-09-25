namespace SkyMap.Net.Tools.Nodes
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DataForms;

    public class DAODataFormNode : AbstractDomainObjectNode<DAODataForm>
    {
        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/DAODataFormContextMenu";
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

