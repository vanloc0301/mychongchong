namespace SkyMap.Net.Workflow.XPDL.ExtendElement
{
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    [Serializable]
    public class WfActdefFormPermission : FormPermission
    {
        private SkyMap.Net.Workflow.XPDL.Actdef actdef;
        private string mustFields;

        public WfActdefFormPermission()
        {
        }

        public WfActdefFormPermission(SkyMap.Net.Workflow.XPDL.Actdef actdef)
        {
            this.actdef = actdef;
        }

        public SkyMap.Net.Workflow.XPDL.Actdef Actdef
        {
            get
            {
                return this.actdef;
            }
            set
            {
                this.actdef = value;
            }
        }

        public override string Id
        {
            get
            {
                return this.actdef.Id;
            }
            set
            {
            }
        }

        public string MustFields
        {
            get
            {
                return this.mustFields;
            }
            set
            {
                this.mustFields = value;
            }
        }
    }
}

