namespace SkyMap.Net.XMLExchange.Model
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class DTLog : DomainObject
    {
        private string fileName;
        private bool isOK;
        private string projectID;
        private DateTime time;

        public DTLog()
        {
            this.Time = DateTime.Now;
        }

        public DTLog(string name, string projectID, string fileName, string description, bool isOk) : this()
        {
            this.Name = name;
            this.projectID = projectID;
            this.fileName = fileName;
            this.Description = description;
            this.isOK = isOk;
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        public bool IsOK
        {
            get
            {
                return this.isOK;
            }
            set
            {
                this.isOK = value;
            }
        }

        public string ProjectID
        {
            get
            {
                return this.projectID;
            }
            set
            {
                this.projectID = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }
    }
}

