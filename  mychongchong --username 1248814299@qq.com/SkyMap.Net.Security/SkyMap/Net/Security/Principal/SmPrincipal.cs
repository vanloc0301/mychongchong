namespace SkyMap.Net.Security.Principal
{
    using System;
    using System.Security.Principal;

    [Serializable]
    public class SmPrincipal : GenericPrincipal
    {
        private string[] _deptIds;
        private string[] _deptNames;
        private string[] _participants;
        private string[] _roleIds;
        private string[] _roleNames;

        public SmPrincipal(IIdentity identity, string[] roleIds, string[] roleNames) : base(identity, roleIds)
        {
            this._roleIds = roleIds;
            this._roleNames = roleNames;
        }

        public SmPrincipal(IIdentity identity, string[] roleIds, string[] roleNames, string[] deptIds, string[] deptNames, string[] participants) : base(identity, roleIds)
        {
            this._roleIds = roleIds;
            this._roleNames = roleNames;
            this._deptIds = deptIds;
            this._deptNames = deptNames;
            this._participants = participants;
        }

        public override bool IsInRole(string role)
        {
            foreach (string str in this._roleNames)
            {
                if (str == role)
                {
                    return true;
                }
            }
            return false;
        }

        public string[] DeptIds
        {
            get
            {
                return this._deptIds;
            }
        }

        public string[] DeptNames
        {
            get
            {
                return this._deptNames;
            }
        }

        public string[] Participants
        {
            get
            {
                return this._participants;
            }
        }

        public string[] RoleIds
        {
            get
            {
                return this._roleIds;
            }
        }

        public string[] RoleNames
        {
            get
            {
                return this._roleNames;
            }
        }
    }
}

