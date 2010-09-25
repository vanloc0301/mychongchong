namespace SkyMap.Net.Workflow.XPDL
{
    using System;

    [Serializable]
    public class Application : AbstractWfElement
    {
        private string assembly;
        private bool executionType;
        private string procedure;
        private string typeClass;

        public virtual string Assembly
        {
            get
            {
                return this.assembly;
            }
            set
            {
                this.assembly = value;
            }
        }

        public bool ExecutionType
        {
            get
            {
                return this.executionType;
            }
            set
            {
                this.executionType = value;
            }
        }

        public string Procedure
        {
            get
            {
                return this.procedure;
            }
            set
            {
                this.procedure = value;
            }
        }

        public string TypeClass
        {
            get
            {
                return this.typeClass;
            }
            set
            {
                this.typeClass = value;
            }
        }
    }
}

