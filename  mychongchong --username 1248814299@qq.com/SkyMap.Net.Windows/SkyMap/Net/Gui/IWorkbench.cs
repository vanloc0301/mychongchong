namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public interface IWorkbench : IMementoCapable
    {
        event EventHandler ActiveWorkbenchWindowChanged;

        event ViewContentEventHandler ViewClosed;

        event ViewContentEventHandler ViewOpened;

        void CloseAllViews();
        void CloseContent(IViewContent content);
        PadDescriptor GetPad(Type type);
        void RedrawAllComponents();
        void ShowPad(PadDescriptor content);
        void ShowView(IViewContent content);

        object ActiveContent { get; }

        IWorkbenchWindow ActiveWorkbenchWindow { get; }

        List<PadDescriptor> PadContentCollection { get; }

        string Title { get; set; }

        List<IViewContent> ViewContentCollection { get; }

        IWorkbenchLayout WorkbenchLayout { get; set; }
    }
}

