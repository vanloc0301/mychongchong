namespace SkyMap.Net.Core
{
    using System;

    public class RemotingSingletonProvider<T> where T: new()
    {
        private RemotingSingletonProvider()
        {
        }

        public static T Instance
        {
            get
            {
                return new T();
            }
        }
    }
}

