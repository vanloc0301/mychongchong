namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class Package : AbstractCommonProdef, ISaveAs
    {
        private IDictionary<string, Prodef> prodefs;

        public virtual void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (Prodef prodef in this.Prodefs.Values)
            {
                prodef.SaveAs(unitOfWork);
            }
        }

        [Browsable(false)]
        public IDictionary<string, Prodef> Prodefs
        {
            get
            {
                return this.prodefs;
            }
            set
            {
                this.prodefs = value;
            }
        }
    }
}

