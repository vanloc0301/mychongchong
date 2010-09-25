namespace SkyMap.Net.Workflow.XPDL.ExtendElement
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class WfTempletAppendix : AbstractWfElement, ISaveAs
    {
        private IList<WfAppendix> wfAppendixs;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (WfAppendix appendix in this.WfAppendixs)
            {
                appendix.SaveAs(unitOfWork);
            }
        }

        [Browsable(false)]
        public virtual IList<WfAppendix> WfAppendixs
        {
            get
            {
                return this.wfAppendixs;
            }
            set
            {
                this.wfAppendixs = value;
            }
        }
    }
}

