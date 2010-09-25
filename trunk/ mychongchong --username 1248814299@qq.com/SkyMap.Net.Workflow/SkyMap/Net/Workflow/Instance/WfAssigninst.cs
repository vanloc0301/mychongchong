namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WfAssigninst : DomainObject
    {
        private WfAbnormalType abnormalStatus;
        private DateTime? acceptDate;
        private DateTime? fromDate;
        private string staffId;
        private string staffName;
        private WfStaffNotion staffNotion;
        private AssignStatusType status;
        private DateTime? toDate;
        private IList<WfAbnormalAuditInst> wfAbnormalAudits;
        private SkyMap.Net.Workflow.Instance.WfResinst wfResinst;

        public override string ToString()
        {
            return this.wfResinst.Actinst.Name;
        }

        public WfAbnormalType AbnormalStatus
        {
            get
            {
                return this.abnormalStatus;
            }
            set
            {
                this.abnormalStatus = value;
            }
        }

        public DateTime? AcceptDate
        {
            get
            {
                return this.acceptDate;
            }
            set
            {
                this.acceptDate = value;
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

        public DateTime? FromDate
        {
            get
            {
                return this.fromDate;
            }
            set
            {
                this.fromDate = value;
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

        public WfStaffNotion StaffNotion
        {
            get
            {
                return this.staffNotion;
            }
            set
            {
                this.staffNotion = value;
            }
        }

        public AssignStatusType Status
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

        public DateTime? ToDate
        {
            get
            {
                return this.toDate;
            }
            set
            {
                this.toDate = value;
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

        public SkyMap.Net.Workflow.Instance.WfResinst WfResinst
        {
            get
            {
                return this.wfResinst;
            }
            set
            {
                this.wfResinst = value;
            }
        }
    }
}

