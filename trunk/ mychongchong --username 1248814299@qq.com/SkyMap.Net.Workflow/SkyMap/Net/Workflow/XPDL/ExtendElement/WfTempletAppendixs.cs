namespace SkyMap.Net.Workflow.XPDL.ExtendElement
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class WfTempletAppendixs : AbstractWfElement, ISaveAs
    {
        private IList<WfTempletAppendix> templetAppendixs;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            WfTempletAppendixs appendixs = this.Clone<WfTempletAppendixs>();
            appendixs.TempletAppendixs = new List<WfTempletAppendix>();
            foreach (WfTempletAppendix appendix in this.TempletAppendixs)
            {
                appendix.SaveAs(unitOfWork);
                appendixs.TempletAppendixs.Add(appendix);
            }
            unitOfWork.RegisterNew(appendixs);
        }

        [Browsable(false)]
        public IList<WfTempletAppendix> TempletAppendixs
        {
            get
            {
                return this.templetAppendixs;
            }
            set
            {
                this.templetAppendixs = value;
            }
        }
    }
}

