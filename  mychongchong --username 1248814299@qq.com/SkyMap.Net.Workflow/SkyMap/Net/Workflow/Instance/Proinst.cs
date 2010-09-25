namespace SkyMap.Net.Workflow.Instance
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Proinst : AbstractWfInstance
    {
        private IList<Actinst> actinsts;
        private string formId;
        private IList<WfProinstMater> maters;
        private SkyMap.Net.Workflow.Instance.ProinstWorkMem proinsWorkMem;
        private string projectId;
        private WfRequesterinst requester;
        private string responsible;
        private IList<WfStaffNotion> staffNotions;
        private string style;
        private string templetDataSetId;
        private IList<WfAdjunct> wfAdjuncts;

        public IList<Actinst> Actinsts
        {
            get
            {
                return this.actinsts;
            }
            set
            {
                this.actinsts = value;
            }
        }

        public string FormId
        {
            get
            {
                return this.formId;
            }
            set
            {
                this.formId = value;
            }
        }

        public IList<WfProinstMater> Maters
        {
            get
            {
                return this.maters;
            }
            set
            {
                this.maters = value;
            }
        }

        public SkyMap.Net.Workflow.Instance.ProinstWorkMem ProinstWorkMem
        {
            get
            {
                return this.proinsWorkMem;
            }
            set
            {
                this.proinsWorkMem = value;
            }
        }

        public string ProjectId
        {
            get
            {
                return this.projectId;
            }
            set
            {
                this.projectId = value;
            }
        }

        public WfRequesterinst Requester
        {
            get
            {
                return this.requester;
            }
            set
            {
                this.requester = value;
            }
        }

        public string Responsible
        {
            get
            {
                return this.responsible;
            }
            set
            {
                this.responsible = value;
            }
        }

        public IList<WfStaffNotion> StaffNotions
        {
            get
            {
                return this.staffNotions;
            }
            set
            {
                this.staffNotions = value;
            }
        }

        public string Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        public string TempletDataSetId
        {
            get
            {
                return this.templetDataSetId;
            }
            set
            {
                this.templetDataSetId = value;
            }
        }

        public IList<WfAdjunct> WfAdjuncts
        {
            get
            {
                return this.wfAdjuncts;
            }
            set
            {
                this.wfAdjuncts = value;
            }
        }
    }
}

