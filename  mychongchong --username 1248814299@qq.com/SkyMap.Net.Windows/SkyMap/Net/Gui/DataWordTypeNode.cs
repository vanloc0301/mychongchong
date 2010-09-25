namespace SkyMap.Net.Gui
{
    using SkyMap.Net.DAO;
    using System;

    public class DataWordTypeNode : AbstractDomainObjectNode<DataWordType>
    {
        public override ObjectNode AddChild()
        {
            return base.AddSingleNode<DataWord, DataWordNode>(DataWordService.Instance.CreateDataWord(base.DomainObject));
        }

        protected override void Initialize()
        {
            base.Initialize();
            base.Nodes.Clear();
            base.AddNodes<DataWord, DataWordNode>(base.DomainObject.DataWords);
        }
    }
}

