namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WfResinst : DomainObject
    {
        private string _fromActinstId;
        private string _fromActinstName;
        private string _fromAssignId;
        private string _fromStaffId;
        private string _fromStaffName;
        private SkyMap.Net.Workflow.Instance.Actinst actinst;
        private AssignRuleType assignRule;
        private IList<WfAssigninst> assigns;
        private string deptId;
        private string deptName;
        private string excludeStaffId;
        private bool isAssigned;
        private string participantId;
        private string participantName;
        private string participantType;
        private string roleId;
        private string roleName;
        private string staffId;
        private string staffName;
        private string subProinstId;
        private string subProinstName;

        public override string ToString()
        {
            return this.actinst.Name;
        }

        public SkyMap.Net.Workflow.Instance.Actinst Actinst
        {
            get
            {
                return this.actinst;
            }
            set
            {
                this.actinst = value;
            }
        }

        public AssignRuleType AssignRule
        {
            get
            {
                return this.assignRule;
            }
            set
            {
                this.assignRule = value;
            }
        }

        public IList<WfAssigninst> Assigns
        {
            get
            {
                return this.assigns;
            }
            set
            {
                this.assigns = value;
            }
        }

        public string DeptId
        {
            get
            {
                return this.deptId;
            }
            set
            {
                this.deptId = value;
            }
        }

        public string DeptName
        {
            get
            {
                return this.deptName;
            }
            set
            {
                this.deptName = value;
            }
        }

        private string Description
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public string ExcludeStaffId
        {
            get
            {
                return this.excludeStaffId;
            }
            set
            {
                this.excludeStaffId = value;
            }
        }

        public string FromActinstId
        {
            get
            {
                return this._fromActinstId;
            }
            set
            {
                this._fromActinstId = value;
            }
        }

        public string FromActinstName
        {
            get
            {
                return this._fromActinstName;
            }
            set
            {
                this._fromActinstName = value;
            }
        }

        public string FromAssignId
        {
            get
            {
                return this._fromAssignId;
            }
            set
            {
                this._fromAssignId = value;
            }
        }

        public string FromStaffId
        {
            get
            {
                return this._fromStaffId;
            }
            set
            {
                this._fromStaffId = value;
            }
        }

        public string FromStaffName
        {
            get
            {
                return this._fromStaffName;
            }
            set
            {
                this._fromStaffName = value;
            }
        }

        public override string Id
        {
            get
            {
                return this.actinst.Id;
            }
            set
            {
            }
        }

        public bool IsAssigned
        {
            get
            {
                return this.isAssigned;
            }
            set
            {
                this.isAssigned = value;
            }
        }

        private string Name
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public string ParticipantId
        {
            get
            {
                return this.participantId;
            }
            set
            {
                this.participantId = value;
            }
        }

        public string ParticipantName
        {
            get
            {
                return this.participantName;
            }
            set
            {
                this.participantName = value;
            }
        }

        public string ParticipantType
        {
            get
            {
                return this.participantType;
            }
            set
            {
                this.participantType = value;
            }
        }

        public string RoleId
        {
            get
            {
                return this.roleId;
            }
            set
            {
                this.roleId = value;
            }
        }

        public string RoleName
        {
            get
            {
                return this.roleName;
            }
            set
            {
                this.roleName = value;
            }
        }

        public string StaffId
        {
            get
            {
                return this.staffId;
            }
            set
            {
                this.staffId = value;
            }
        }

        public string StaffName
        {
            get
            {
                return this.staffName;
            }
            set
            {
                this.staffName = value;
            }
        }

        public string SubProinstId
        {
            get
            {
                return this.subProinstId;
            }
            set
            {
                this.subProinstId = value;
            }
        }

        public string SubProinstName
        {
            get
            {
                return this.subProinstName;
            }
            set
            {
                this.subProinstName = value;
            }
        }
    }
}

