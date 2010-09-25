namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using System.Linq;
    using SkyMap.Net.Tools.Organize;

    public class LoadParticipantsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load tax type...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<CParticipant, ParticipantNode>(OGMDAOService.Instance.Participants.OrderBy<CParticipant, string>(delegate (CParticipant p) {
                    return p.Name;
                }).ToList<CParticipant>());
            }
        }
    }
}

