namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class WfRouteInst : DomainObject
    {
        private Actinst from;
        private string proinstId;
        private DateTime? timeStamp;
        private Actinst to;

        public override string ToString()
        {
            return (this.from.Name + "-" + this.to.Name);
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

        public Actinst From
        {
            get
            {
                return this.from;
            }
            set
            {
                this.from = value;
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

        public Actinst To
        {
            get
            {
                return this.to;
            }
            set
            {
                this.to = value;
            }
        }
    }
}

