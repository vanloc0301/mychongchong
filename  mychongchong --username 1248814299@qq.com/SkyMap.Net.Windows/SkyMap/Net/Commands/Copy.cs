namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Gui;
    using System;

    public class Copy : AbstractClipboardCommand
    {
        protected override bool GetEnabled(IClipboardHandler editable)
        {
            return editable.EnableCopy;
        }

        protected override void Run(IClipboardHandler editable)
        {
            editable.Copy();
        }
    }
}

