namespace SkyMap.Net.Core
{
    using System;

    public interface IRemoteUpdate
    {
        void Execute();
        string GetRemoteLog();
    }
}

