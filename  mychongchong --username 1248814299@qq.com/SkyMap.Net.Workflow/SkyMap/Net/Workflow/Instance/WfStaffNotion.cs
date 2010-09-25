namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class WfStaffNotion : DomainObject
    {
        private string assignId;
        private string content;
        private DateTime? date;
        private string proinstId;
        private string staffId;
        private string staffName;

        public override string ToString()
        {
            if (this.StaffName != null)
            {
                return ("经办人：" + this.StaffName + "的经办意见");
            }
            return string.Empty;
        }

        public string AssignId
        {
            get
            {
                return this.assignId;
            }
            set
            {
                this.assignId = value;
            }
        }

        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        public DateTime? Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
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

        public override string Id
        {
            get
            {
                return this.assignId;
            }
            set
            {
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

        public string ProinstId
        {
            get
            {
                return this.proinstId;
            }
            set
            {
                this.proinstId = value;
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
    }
}

