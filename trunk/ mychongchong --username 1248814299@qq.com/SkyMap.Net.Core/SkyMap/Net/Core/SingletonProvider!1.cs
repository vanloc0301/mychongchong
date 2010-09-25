namespace SkyMap.Net.Core
{
    using System;

    public static class SingletonProvider<T> where T: new()
    {
        public static T Instance
        {
            get
            {
                return SingletonCreator.instance;
            }
        }

        private static class SingletonCreator
        {
            internal static readonly T instance;

            static SingletonCreator()
            {
                SingletonProvider<T>.SingletonCreator.instance = new T();
            }
        }
    }
}

