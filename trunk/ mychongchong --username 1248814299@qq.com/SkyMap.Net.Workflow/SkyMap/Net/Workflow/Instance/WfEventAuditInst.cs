namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public abstract class WfEventAuditInst : DomainObject
    {
        private string actinstId;
        private string actinstName;
        private string proinstId;
        private string proinstName;
        private DateTime? timeStamp;

        public override string ToString()
        {
            return ("Event of process : " + this.proinstName);
        }

        public string ActinstId
        {
            get
            {
                return this.actinstId;
            }
            set
            {
                this.actinstId = value;
            }
        }

        public string ActinstName
        {
            get
            {
                return this.actinstName;
            }
            set
            {
                this.actinstName = value;
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

        public string ProinstName
        {
            get
            {
                return this.proinstName;
            }
            set
            {
                this.proinstName = value;
            }
        }

        public DateTime? TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
            set
            {
                this.timeStamp = value;
            }
        }
    }
}

