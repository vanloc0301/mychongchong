namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.OGM;

    public class ParticipantNode : AbstractDomainObjectNode<CParticipant>
    {
        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

