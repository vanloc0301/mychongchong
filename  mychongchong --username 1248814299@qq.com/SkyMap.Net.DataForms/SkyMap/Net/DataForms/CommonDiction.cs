namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class CommonDiction : DomainObject
    {
        private string path;
        private string staffId = string.Empty;
        private string staffName;

        public CommonDiction()
        {
            this.StaffName = string.Empty;
            this.path = string.Empty;
        }

        public override string ToString()
        {
            return (string.IsNullOrEmpty(this.Name) ? this.Description : this.Name);
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public string StaffId
        {
            get
            {
                return this.staffId;
            }
            set
            {
                this.staffId = value;
            }
        }

        public string StaffName
        {
            get
            {
                return this.staffName;
            }
            set
            {
                this.staffName = value;
            }
        }
    }
}

