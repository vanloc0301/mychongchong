namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class WfAdjunctFile : DomainObject
    {
        private WfAdjunct adjunct;
        private byte[] data;

        public WfAdjunctFile()
        {
        }

        public WfAdjunctFile(WfAdjunct adj)
        {
            if (adj == null)
            {
                throw new ArgumentNullException("WfAdjunct cannot be null");
            }
            this.adjunct = adj;
        }

        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        public override string Description
        {
            get
            {
                if (this.adjunct == null)
                {
                    return null;
                }
                return this.adjunct.Name;
            }
            set
            {
            }
        }

        public override string Id
        {
            get
            {
                if (this.adjunct == null)
                {
                    return null;
                }
                return this.adjunct.Id;
            }
            set
            {
            }
        }

        public override string Name
        {
            get
            {
                if (this.adjunct == null)
                {
                    return null;
                }
                return this.adjunct.Name;
            }
            set
            {
            }
        }

        public override int ReplicationVersion
        {
            get
            {
                if (this.adjunct == null)
                {
                    return 0;
                }
                return this.adjunct.ReplicationVersion;
            }
            set
            {
            }
        }

        public string Type
        {
            get
            {
                if (this.adjunct == null)
                {
                    return null;
                }
                return this.adjunct.Type;
            }
        }
    }
}

