namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using System;
    using System.Drawing;

    public class GraphUnit
    {
        private string mDescription;
        protected DrawType mGraphType;
        private string mID;
        private string mName;
        private string mRelationalID;
        private bool mSelected;
        private string mTag;

        public virtual void Draw(Graphics g)
        {
        }

        public virtual bool TestCapture(int x, int y)
        {
            return false;
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public DrawType GraphType
        {
            get
            {
                return this.mGraphType;
            }
        }

        public string ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public string RelationalID
        {
            get
            {
                return this.mRelationalID;
            }
            set
            {
                this.mRelationalID = value;
            }
        }

        public bool Selected
        {
            get
            {
                return this.mSelected;
            }
            set
            {
                this.mSelected = value;
            }
        }

        public string Tag
        {
            get
            {
                return this.mTag;
            }
            set
            {
                this.mTag = value;
            }
        }
    }
}

