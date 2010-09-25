namespace SkyMap.Net.DataForms
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class ParticipantFormPermission : FormPermission
    {
        private string participantId;
        private string participantName;

        public override string ToString()
        {
            string str = string.Format("{0}-{1}", this.ParticipantName, this.DaoDataFormName);
            if (str == "-")
            {
                str = "新通用参与者表单权限";
            }
            return str;
        }

        [Browsable(false)]
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

        [Editor("SkyMap.Net.Tools.DataForms.ParticipantFormPermEditor,SkyMap.Net.Tools.DataForms", typeof(UITypeEditor)), DisplayName("参与者")]
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
    }
}

