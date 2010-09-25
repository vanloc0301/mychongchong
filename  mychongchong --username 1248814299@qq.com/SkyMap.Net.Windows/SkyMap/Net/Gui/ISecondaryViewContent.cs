namespace SkyMap.Net.Gui
{
    using System;

    public interface ISecondaryViewContent : IBaseViewContent, IDisposable
    {
        void NotifyAfterSave(bool successful);
        void NotifyBeforeSave();
    }
}

