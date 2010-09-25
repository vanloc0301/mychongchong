namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Gui;
    using System;

    public class Paste : AbstractClipboardCommand
    {
        protected override bool GetEnabled(IClipboardHandler editable)
        {
            return editable.EnablePaste;
        }

        protected override void Run(IClipboardHandler editable)
        {
            editable.Paste();
        }
    }
}

