namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IDialogPanel
    {
        event EventHandler EnableFinishChanged;

        bool ReceiveDialogMessage(DialogMessage message);

        System.Windows.Forms.Control Control { get; }

        object CustomizationObject { get; set; }

        bool EnableFinish { get; }
    }
}

