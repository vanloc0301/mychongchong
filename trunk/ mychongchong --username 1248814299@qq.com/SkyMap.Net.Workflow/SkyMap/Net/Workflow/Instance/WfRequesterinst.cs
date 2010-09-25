namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class WfRequesterinst : DomainObject
    {
        private string actinstId;
        private string actinstName;
        private string staffId;
        private string staffName;

        public override string ToString()
        {
            if (StringHelper.IsNull(this.actinstId))
            {
                return this.StaffName;
            }
            return this.actinstName;
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

