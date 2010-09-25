namespace SkyMap.Net.Workflow.Engine
{
    using System;

    [Serializable]
    public class WorkItem
    {
        private string _abnormalAuditId = string.Empty;
        private string _actdefId = string.Empty;
        private string _actinstId = string.Empty;
        private string _actinstName = string.Empty;
        private string _assignId = string.Empty;
        private bool _dealed;
        private string _proinstId = string.Empty;
        private string _proinstName = string.Empty;
        private string id = string.Empty;
        private string prodefId = string.Empty;
        private string projectId;

        public string AbnormalAuditId
        {
            get
            {
                return this._abnormalAuditId;
            }
            set
            {
                this._abnormalAuditId = value;
            }
        }

        public string ActdefId
        {
            get
            {
                return this._actdefId;
            }
            set
            {
                this._actdefId = value;
            }
        }

        public string ActinstId
        {
            get
            {
                return this._actinstId;
            }
            set
            {
                this._actinstId = value;
            }
        }

        public string ActinstName
        {
            get
            {
                return this._actinstName;
            }
            set
            {
                this._actinstName = value;
            }
        }

        public string AssignId
        {
            get
            {
                return this._assignId;
            }
            set
            {
                this._assignId = value;
            }
        }

        public bool Dealed
        {
            get
            {
                return this._dealed;
            }
            set
            {
                this._dealed = value;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string ProdefId
        {
            get
            {
                return this.prodefId;
            }
            set
            {
                this.prodefId = value;
            }
        }

        public string ProinstId
        {
            get
            {
                return this._proinstId;
            }
            set
            {
                this._proinstId = value;
            }
        }

        public string ProinstName
        {
            get
            {
                return this._proinstName;
            }
            set
            {
                this._proinstName = value;
            }
        }

        public string ProjectId
        {
            get
            {
                return this.projectId;
            }
            set
            {
                this.projectId = value;
            }
        }
    }
}

