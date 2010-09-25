namespace SkyMap.Net.DAO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class DataWordType : DomainObject, ISaveAs
    {
        private string code;
        private IList<DataWord> dataWords;

        public DataWordType()
        {
        }

        public DataWordType(string name) : base(name)
        {
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            unitOfWork.RegisterNew<DataWord>(this.dataWords);
        }

        [DisplayName("代码")]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        [Browsable(false)]
        public IList<DataWord> DataWords
        {
            get
            {
                return this.dataWords;
            }
            set
            {
                this.dataWords = value;
            }
        }
    }
}

