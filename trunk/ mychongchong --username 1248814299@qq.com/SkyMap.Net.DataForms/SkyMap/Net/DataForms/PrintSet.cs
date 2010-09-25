namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class PrintSet : DomainObject, ISaveAs
    {
        private IList<TempletPrint> templetPrints;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            PrintSet set = this.Clone<PrintSet>();
            set.TempletPrints = new List<TempletPrint>();
            foreach (TempletPrint print in this.templetPrints)
            {
                print.SaveAs(unitOfWork);
                set.TempletPrints.Add(print);
            }
            unitOfWork.RegisterNew(set);
        }

        [Browsable(false)]
        public IList<TempletPrint> TempletPrints
        {
            get
            {
                return this.templetPrints;
            }
            set
            {
                this.templetPrints = value;
            }
        }
    }
}

