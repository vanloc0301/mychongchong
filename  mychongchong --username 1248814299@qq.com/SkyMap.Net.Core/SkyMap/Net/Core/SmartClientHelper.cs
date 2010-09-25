namespace SkyMap.Net.Core
{
    using System;

    public static class SmartClientHelper
    {
        private static bool _online = true;

        public static bool Online
        {
            get
            {
                return _online;
            }
            set
            {
                _online = value;
            }
        }
    }
}

