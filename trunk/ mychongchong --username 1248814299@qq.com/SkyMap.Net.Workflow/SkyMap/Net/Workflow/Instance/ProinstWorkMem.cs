namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class ProinstWorkMem : DomainObject
    {
        private string workMem1;
        private string workMem2;
        private string workMem3;
        private string workMem4;

        public string WorkMem1
        {
            get
            {
                return this.workMem1;
            }
            set
            {
                this.workMem1 = value;
            }
        }

        public string WorkMem2
        {
            get
            {
                return this.workMem2;
            }
            set
            {
                this.workMem2 = value;
            }
        }

        public string WorkMem3
        {
            get
            {
                return this.workMem3;
            }
            set
            {
                this.workMem3 = value;
            }
        }

        public string WorkMem4
        {
            get
            {
                return this.workMem4;
            }
            set
            {
                this.workMem4 = value;
            }
        }
    }
}

