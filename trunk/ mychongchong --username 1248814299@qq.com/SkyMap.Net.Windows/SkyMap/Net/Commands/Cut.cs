namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Gui;
    using System;

    public class Cut : AbstractClipboardCommand
    {
        protected override bool GetEnabled(IClipboardHandler editable)
        {
            return editable.EnableCut;
        }

        protected override void Run(IClipboardHandler editable)
        {
            editable.Cut();
        }
    }
}

