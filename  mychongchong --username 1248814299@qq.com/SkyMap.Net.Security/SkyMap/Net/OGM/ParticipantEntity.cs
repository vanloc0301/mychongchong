namespace SkyMap.Net.OGM
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class ParticipantEntity
    {
        private string mIdValue;
        private string type;

        public override string ToString()
        {
            return this.type;
        }

        [DisplayName("参与者对象实体ID")]
        public string IdValue
        {
            get
            {
                return this.mIdValue;
            }
            set
            {
                this.mIdValue = value;
            }
        }

        [DisplayName("类型")]
        public string Type
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

