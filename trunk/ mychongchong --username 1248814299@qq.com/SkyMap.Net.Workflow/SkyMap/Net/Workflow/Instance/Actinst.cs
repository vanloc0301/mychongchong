namespace SkyMap.Net.Workflow.Instance
{
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Actinst : AbstractWfInstance
    {
        private string actdefId;
        private string actdefName;
        private IList<WfAssigninst> assigns;
        private int fromCount;
        private SkyMap.Net.Workflow.Instance.Proinst proinst;
        private ICollection<WfRouteInst> routeFromMe;
        private ICollection<WfRouteInst> routeToMe;
        private int toCount;
        private ActdefType type;
        private SkyMap.Net.Workflow.Instance.WfResinst wfResinst;

        public string ActdefId
        {
            get
            {
                return this.actdefId;
            }
            set
            {
                this.actdefId = value;
            }
        }

        public string ActdefName
        {
            get
            {
                return this.actdefName;
            }
            set
            {
                this.actdefName = value;
            }
        }

        public IList<WfAssigninst> Assigns
        {
            get
            {
                return this.assigns;
            }
            set
            {
                this.assigns = value;
            }
        }

        public int FromCount
        {
            get
            {
                return this.fromCount;
            }
            set
            {
                this.fromCount = value;
            }
        }

        public SkyMap.Net.Workflow.Instance.Proinst Proinst
        {
            get
            {
                return this.proinst;
            }
            set
            {
                this.proinst = value;
            }
        }

        public ICollection<WfRouteInst> RouteFromMe
        {
            get
            {
                return this.routeFromMe;
            }
            set
            {
                this.routeFromMe = value;
            }
        }

        public ICollection<WfRouteInst> RouteToMe
        {
            get
            {
                return this.routeToMe;
            }
            set
            {
                this.routeToMe = value;
            }
        }

        public int ToCount
        {
            get
            {
                return this.toCount;
            }
            set
            {
                this.toCount = value;
            }
        }

        public ActdefType Type
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

        public SkyMap.Net.Workflow.Instance.WfResinst WfResinst
        {
            get
            {
                return this.wfResinst;
            }
            set
            {
                this.wfResinst = value;
            }
        }
    }
}

