namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public interface IDialogPanelDescriptor
    {
        List<IDialogPanelDescriptor> ChildDialogPanelDescriptors { get; set; }

        IDialogPanel DialogPanel { get; set; }

        string ID { get; }

        string Label { get; set; }
    }
}

