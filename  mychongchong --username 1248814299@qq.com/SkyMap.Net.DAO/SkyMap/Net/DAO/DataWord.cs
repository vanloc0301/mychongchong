namespace SkyMap.Net.DAO
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class DataWord : DomainObject
    {
        private string code;
        private DataWordType type;

        public DataWord()
        {
        }

        public DataWord(string name, DataWordType type) : base(name)
        {
            this.type = type;
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

        public DataWordType Type
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
    }
}

