namespace SkyMap.Net.Gui
{
    using System;

    public interface IPropertyEditPanel : IDisposable
    {
        void CancelEdit();
        void PostEditor();
        void RedrawContent();
        void Save();

        object SelectedObject { get; set; }
    }
}

