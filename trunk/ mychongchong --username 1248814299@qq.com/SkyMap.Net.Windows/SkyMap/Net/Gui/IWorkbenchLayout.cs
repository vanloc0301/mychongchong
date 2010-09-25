namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.CompilerServices;

    public interface IWorkbenchLayout
    {
        event EventHandler ActiveWorkbenchWindowChanged;

        void ActivatePad(PadDescriptor content);
        void ActivatePad(string fullyQualifiedTypeName);
        void Attach(IWorkbench workbench);
        void Detach();
        void HidePad(PadDescriptor content);
        bool IsVisible(PadDescriptor padContent);
        void LoadConfiguration();
        void OnActiveWorkbenchWindowChanged(EventArgs e);
        void RedrawAllComponents();
        void ShowPad(PadDescriptor content);
        IWorkbenchWindow ShowView(IViewContent content);
        void StoreConfiguration();

        object ActiveContent { get; }

        IWorkbenchWindow ActiveWorkbenchwindow { get; }
    }
}

