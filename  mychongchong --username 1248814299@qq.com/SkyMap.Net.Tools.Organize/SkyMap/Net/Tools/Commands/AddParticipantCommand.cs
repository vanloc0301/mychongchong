namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using SkyMap.Net.Tools.Organize;

    public class AddParticipantCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                CParticipant participant = OGMDAOService.Instance.CreateParticipant();
                owner.AddSingleNode<CParticipant, ParticipantNode>(participant);
            }
        }
    }
}

