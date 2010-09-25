namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public abstract class AbstractWfInstance : DomainObject, WfInstanceElement
    {
        private double costTime;
        private DateTime? createDate;
        private DateTime? dueDate;
        private double dueTime;
        private DateTime? endDate;
        private DateTime? lastStateDate;
        private string packageId;
        private string packageName;
        private short priority;
        private string prodefId;
        private string prodefName;
        private string prodefVersion;
        private DateTime? startDate;
        private IList<WfStateEventAuditInst> stateEvents;
        private WfStatusType status;
        private IList<WfAbnormalAuditInst> wfAbnormalAudits;

        protected AbstractWfInstance()
        {
        }

        public double CostTime
        {
            get
            {
                return this.costTime;
            }
            set
            {
                this.costTime = value;
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return this.createDate;
            }
            set
            {
                this.createDate = value;
            }
        }

        public DateTime? DueDate
        {
            get
            {
                return this.dueDate;
            }
            set
            {
                this.dueDate = value;
            }
        }

        public double DueTime
        {
            get
            {
                return this.dueTime;
            }
            set
            {
                this.dueTime = value;
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
            }
        }

        public DateTime? LastStateDate
        {
            get
            {
                return this.lastStateDate;
            }
            set
            {
                this.lastStateDate = value;
            }
        }

        public string PackageId
        {
            get
            {
                return this.packageId;
            }
            set
            {
                this.packageId = value;
            }
        }

        public string PackageName
        {
            get
            {
                return this.packageName;
            }
            set
            {
                this.packageName = value;
            }
        }

        public short Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                this.priority = value;
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

        public string ProdefName
        {
            get
            {
                return this.prodefName;
            }
            set
            {
                this.prodefName = value;
            }
        }

        public string ProdefVersion
        {
            get
            {
                return this.prodefVersion;
            }
            set
            {
                this.prodefVersion = value;
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        public IList<WfStateEventAuditInst> StateEvents
        {
            get
            {
                return this.stateEvents;
            }
            set
            {
                this.stateEvents = value;
            }
        }

        public WfStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        public IList<WfAbnormalAuditInst> WfAbnormalAudits
        {
            get
            {
                return this.wfAbnormalAudits;
            }
            set
            {
                this.wfAbnormalAudits = value;
            }
        }
    }
}

