namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class WfProinstMater : DomainObject
    {
        private int dupliNum;
        private int oldNum;
        private string proinstId;
        private bool selected;

        public int DupliNum
        {
            get
            {
                return this.dupliNum;
            }
            set
            {
                this.dupliNum = value;
            }
        }

        public int OldNum
        {
            get
            {
                return this.oldNum;
            }
            set
            {
                this.oldNum = value;
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

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
            }
        }
    }
}

