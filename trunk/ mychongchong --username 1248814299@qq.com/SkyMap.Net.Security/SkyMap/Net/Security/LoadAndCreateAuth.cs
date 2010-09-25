namespace SkyMap.Net.Security
{
    using SkyMap.Net.OGM;
    using System;

    public abstract class LoadAndCreateAuth
    {
        private CAuthType authType;

        protected LoadAndCreateAuth()
        {
        }

        public abstract void Create(object owner);
        public abstract void Load(object owner);

        public CAuthType AuthType
        {
            get
            {
                return this.authType;
            }
            set
            {
                this.authType = value;
            }
        }
    }
}

