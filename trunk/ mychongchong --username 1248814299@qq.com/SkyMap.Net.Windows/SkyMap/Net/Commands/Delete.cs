namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Gui;
    using System;

    public class Delete : AbstractClipboardCommand
    {
        protected override bool GetEnabled(IClipboardHandler editable)
        {
            return editable.EnableDelete;
        }

        protected override void Run(IClipboardHandler editable)
        {
            editable.Delete();
        }
    }
}

