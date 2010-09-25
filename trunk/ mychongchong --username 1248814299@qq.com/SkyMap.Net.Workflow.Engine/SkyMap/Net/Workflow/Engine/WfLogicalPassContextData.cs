namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Remoting.Messaging;

    [Serializable]
    public class WfLogicalPassContextData : ILogicalThreadAffinative
    {
        private string _fromActivityKey;
        private string _fromActivityName;
        private string _fromAssignId;
        private string _fromStaffId;
        private string _fromStaffName;
        private string _selectedTranId;
        private string[] _toStaffIds;
        private string[] _toStaffNames;
        private string excludeStaffId;

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

        public string FromActivityKey
        {
            get
            {
                return this._fromActivityKey;
            }
            set
            {
                this._fromActivityKey = value;
            }
        }

        public string FromActivityName
        {
            get
            {
                return this._fromActivityName;
            }
            set
            {
                this._fromActivityName = value;
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

        public string SelectedTranId
        {
            get
            {
                return this._selectedTranId;
            }
            set
            {
                this._selectedTranId = value;
            }
        }

        public string[] ToStaffIds
        {
            get
            {
                return this._toStaffIds;
            }
            set
            {
                this._toStaffIds = value;
            }
        }

        public string[] ToStaffNames
        {
            get
            {
                return this._toStaffNames;
            }
            set
            {
                this._toStaffNames = value;
            }
        }
    }
}

