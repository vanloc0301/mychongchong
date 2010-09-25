namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class CStaffLog : DomainObject
    {
        private string compute;
        private string iP;
        private DateTime? logOutTime;
        private string logSystem;
        private DateTime logTime;
        private string staffId;
        private string staffName;
        private bool status;

        public override string ToString()
        {
            return (this.StaffName + " log at " + this.LogTime);
        }

        public string Compute
        {
            get
            {
                return this.compute;
            }
            set
            {
                this.compute = value;
            }
        }

        public string IP
        {
            get
            {
                return this.iP;
            }
            set
            {
                this.iP = value;
            }
        }

        public DateTime? LogOutTime
        {
            get
            {
                return this.logOutTime;
            }
            set
            {
                this.logOutTime = value;
            }
        }

        public string LogSystem
        {
            get
            {
                return this.logSystem;
            }
            set
            {
                this.logSystem = value;
            }
        }

        public virtual DateTime LogTime
        {
            get
            {
                return this.logTime;
            }
            set
            {
                this.logTime = value;
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

        public bool Status
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
    }
}

