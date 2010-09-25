namespace SkyMap.Net.Core
{
    using System;

    public interface IMementoCapable
    {
        Properties CreateMemento();
        void SetMemento(Properties memento);
    }
}

