namespace SkyMap.Net.Gui
{
    using System;

    public interface IUndoHandler
    {
        void Redo();
        void Undo();

        bool EnableRedo { get; }

        bool EnableUndo { get; }
    }
}

