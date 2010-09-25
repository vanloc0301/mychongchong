namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;

    public class LoadAndCreateUniversalAuth : LoadAndCreateAuth
    {
        public override void Create(object owner)
        {
            CUniversalAuth auth = OGMDAOService.Instance.CreateUniversalAuth(base.AuthType);
            (owner as AuthTypeNode).AddSingleNode<CUniversalAuth, UniversalAuthNode>(auth);
        }

        public override void Load(object owner)
        {
            (owner as AuthTypeNode).AddNodes<CUniversalAuth, UniversalAuthNode>(base.AuthType.UniversalAuths);
        }
    }
}

