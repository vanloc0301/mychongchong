namespace SkyMap.Net.Workflow.XPDL.ExtendElement
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class WfAppendix : AbstractWfElement, ISaveAs
    {
        private int dupliNum;
        private bool isArch;
        private int oldNum;
        private string source;
        private SkyMap.Net.Workflow.XPDL.ExtendElement.WfTempletAppendix wfTempletAppendix;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        [DisplayName("复印件份数")]
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

        [DisplayName("是否需要归档")]
        public bool IsArch
        {
            get
            {
                return this.isArch;
            }
            set
            {
                this.isArch = value;
            }
        }

        [DisplayName("原件份数")]
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

        [DisplayName("资料来源")]
        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        [Browsable(false)]
        public SkyMap.Net.Workflow.XPDL.ExtendElement.WfTempletAppendix WfTempletAppendix
        {
            get
            {
                return this.wfTempletAppendix;
            }
            set
            {
                this.wfTempletAppendix = value;
            }
        }
    }
}

