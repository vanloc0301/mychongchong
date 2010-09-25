namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;

    public class AddDataWordTypeCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ((ObjectNode) this.Owner).AddSingleNode<DataWordType, DataWordTypeNode>(DataWordService.Instance.CreateDataWordType());
            }
        }
    }
}

