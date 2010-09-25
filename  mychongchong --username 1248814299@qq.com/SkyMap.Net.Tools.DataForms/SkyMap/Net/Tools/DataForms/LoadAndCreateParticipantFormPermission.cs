namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using System;
    using System.Linq;

    public class LoadAndCreateParticipantFormPermission : LoadAndCreateAuth
    {
        public override void Create(object owner)
        {
            ParticipantFormPermission permission = DataFormDAOService.Instance.CreateParticipantFormPermission();
            (owner as ObjectNode).AddSingleNode<ParticipantFormPermission, ParticipantFormPermNode>(permission);
        }

        public override void Load(object owner)
        {
            (owner as ObjectNode).AddNodes<ParticipantFormPermission, ParticipantFormPermNode>(DataFormDAOService.Instance.ParticipantFormPerms.OrderBy<ParticipantFormPermission, string>(delegate (ParticipantFormPermission p) {
                return p.ToString();
            }).ToList<ParticipantFormPermission>());
        }
    }
}

