namespace SkyMap.Net.Security
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Security.Permissions;
    using System.Security.Principal;

    [Serializable, PermissionSet(SecurityAction.Assert, Unrestricted=true)]
    public class LogicalSecurityContextData : ILogicalThreadAffinative
    {
        private IPrincipal _principal;

        public LogicalSecurityContextData(IPrincipal p)
        {
            this._principal = p;
        }

        public IPrincipal Principal
        {
            get
            {
                return this._principal;
            }
        }
    }
}

