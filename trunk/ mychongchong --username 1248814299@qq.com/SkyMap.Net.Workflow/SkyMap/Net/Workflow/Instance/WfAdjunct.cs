namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class WfAdjunct : DomainObject
    {
        private DateTime? createDate;
        private string createStaffId = string.Empty;
        private string createStaffName = string.Empty;
        private WfAdjunctFile file;
        private DateTime? lastModiDate;
        private string lastModiStaffId;
        private string lastModiStaffName;
        private string proinstId;
        private string type = string.Empty;

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

        public string CreateStaffId
        {
            get
            {
                return this.createStaffId;
            }
            set
            {
                this.createStaffId = value;
            }
        }

        public string CreateStaffName
        {
            get
            {
                return this.createStaffName;
            }
            set
            {
                this.createStaffName = value;
            }
        }

        public WfAdjunctFile File
        {
            get
            {
                return this.file;
            }
            set
            {
                this.file = value;
            }
        }

        public DateTime? LastModiDate
        {
            get
            {
                return this.lastModiDate;
            }
            set
            {
                this.lastModiDate = value;
            }
        }

        public string LastModiStaffId
        {
            get
            {
                return this.lastModiStaffId;
            }
            set
            {
                this.lastModiStaffId = value;
            }
        }

        public string LastModiStaffName
        {
            get
            {
                return this.lastModiStaffName;
            }
            set
            {
                this.lastModiStaffName = value;
            }
        }

        public virtual string ProinstId
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

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

