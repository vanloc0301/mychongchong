namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Gui;
    using System;

    public class SelectAll : AbstractClipboardCommand
    {
        protected override bool GetEnabled(IClipboardHandler editable)
        {
            return editable.EnableSelectAll;
        }

        protected override void Run(IClipboardHandler editable)
        {
            editable.SelectAll();
        }
    }
}

