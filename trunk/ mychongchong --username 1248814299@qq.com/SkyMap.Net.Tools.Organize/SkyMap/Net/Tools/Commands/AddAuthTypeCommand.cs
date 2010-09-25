namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using SkyMap.Net.Tools.Organize;

    public class AddAuthTypeCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                CAuthType type = OGMDAOService.Instance.CreateAuthType();
                owner.AddSingleNode<CAuthType, AuthTypeNode>(type);
            }
        }
    }
}

