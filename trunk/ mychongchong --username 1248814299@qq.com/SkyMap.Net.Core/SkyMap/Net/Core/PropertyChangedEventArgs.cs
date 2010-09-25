namespace SkyMap.Net.Core
{
    using System;

    public class PropertyChangedEventArgs : EventArgs
    {
        private string key;
        private object newValue;
        private object oldValue;
        private SkyMap.Net.Core.Properties properties;

        public PropertyChangedEventArgs(SkyMap.Net.Core.Properties properties, string key, object oldValue, object newValue)
        {
            this.properties = properties;
            this.key = key;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public object NewValue
        {
            get
            {
                return this.newValue;
            }
        }

        public object OldValue
        {
            get
            {
                return this.oldValue;
            }
        }

        public SkyMap.Net.Core.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }
    }
}

