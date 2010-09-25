namespace SkyMap.Net.Tools.Organize
{
    using System;

    public class ParticipantType
    {
        private string mDescription;
        private string mType;

        public ParticipantType(string ptype, string pdescription)
        {
            this.mType = ptype;
            this.mDescription = pdescription;
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public string Type
        {
            get
            {
                return this.mType;
            }
            set
            {
                this.mType = value;
            }
        }
    }
}

