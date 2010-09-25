namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class CUniversalAuth : DomainObject, ISaveAs
    {
        private string participantId = string.Empty;
        private string participantIdValue;
        private string participantName = string.Empty;
        private string participantType = string.Empty;
        private string resourceId;
        private string resourceName;
        private CAuthType type;

        public CUniversalAuth()
        {
            this.ParticipantIdValue = string.Empty;
            this.resourceId = string.Empty;
            this.resourceName = string.Empty;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        public override string ToString()
        {
            string str = this.participantName + "-" + this.resourceName;
            if (str == "-")
            {
                str = "新通用权限";
            }
            return str;
        }

        private string Name
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        [DisplayName("参与者ID"), Browsable(false)]
        public string ParticipantId
        {
            get
            {
                return this.participantId;
            }
            set
            {
                this.participantId = value;
            }
        }

        [Browsable(false)]
        public string ParticipantIdValue
        {
            get
            {
                return this.participantIdValue;
            }
            set
            {
                this.participantIdValue = value;
            }
        }

        [DisplayName("参与者"), Editor("SkyMap.Net.Tools.Organize.UniversalAuthParticipantEditor,SkyMap.Net.Tools.Organize", typeof(UITypeEditor))]
        public string ParticipantName
        {
            get
            {
                return this.participantName;
            }
            set
            {
                this.participantName = value;
            }
        }

        [Browsable(false)]
        public string ParticipantType
        {
            get
            {
                return this.participantType;
            }
            set
            {
                this.participantType = value;
            }
        }

        [Browsable(false)]
        public string ResourceId
        {
            get
            {
                return this.resourceId;
            }
            set
            {
                this.resourceId = value;
            }
        }

        [DisplayName("资源名称"), Editor("SkyMap.Net.Tools.Organize.UniversalAuthResourceEditor,SkyMap.Net.Tools.Organize", typeof(UITypeEditor))]
        public string ResourceName
        {
            get
            {
                return this.resourceName;
            }
            set
            {
                this.resourceName = value;
            }
        }

        [Browsable(false)]
        public CAuthType Type
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

