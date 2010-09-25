namespace SkyMap.Net.Gui
{
    using System;
    using SkyMap.Net.DAO;

    public class DataWordNode : AbstractDomainObjectNode<DataWord>
    {
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

