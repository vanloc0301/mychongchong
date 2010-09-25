namespace SkyMap.Net.Internal.Undo
{
    using System;

    public interface IUndoableOperation
    {
        void Redo();
        void Undo();
    }
}

