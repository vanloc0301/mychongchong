namespace SkyMap.Net.XMLExchange.GUI.Nodes
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.XMLExchange.Model;

    public class DTColumnNode : AbstractDomainObjectNode<DTColumn>
    {
        public override ObjectNode AddChild()
        {
            return null;
        }

        public override void Delete()
        {
            base.Delete();
            base.DomainObject.DTTable.DTColumns.Remove(base.DomainObject);
        }

        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }

        public override bool EnablePaste
        {
            get
            {
                return false;
            }
        }
    }
}

