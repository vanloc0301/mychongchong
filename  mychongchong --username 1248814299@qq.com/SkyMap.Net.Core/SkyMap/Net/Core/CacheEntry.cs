namespace SkyMap.Net.Core
{
    using System;

    [Serializable]
    public class CacheEntry
    {
        private DateTime _dateCreated;
        private object _key;
        private object _value;

        public DateTime DateCreated
        {
            get
            {
                return this._dateCreated;
            }
            set
            {
                this._dateCreated = value;
            }
        }

        public object Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
    }
}

