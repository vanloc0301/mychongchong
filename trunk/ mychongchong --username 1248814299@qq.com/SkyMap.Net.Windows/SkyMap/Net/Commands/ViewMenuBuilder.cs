namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public abstract class ViewMenuBuilder : ISubmenuBuilder
    {
        protected ViewMenuBuilder()
        {
        }

        public ToolStripItem[] BuildSubmenu(Codon codon, object owner)
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            foreach (PadDescriptor descriptor in WorkbenchSingleton.Workbench.PadContentCollection)
            {
                if (descriptor.Category == this.Category)
                {
                    list.Add(new MyMenuItem(descriptor));
                }
            }
            return list.ToArray();
        }

        protected abstract string Category { get; }

        private class MyMenuItem : MenuCommand
        {
            private PadDescriptor padDescriptor;

            public MyMenuItem(PadDescriptor padDescriptor) : base((string) null, (EventHandler) null)
            {
                this.padDescriptor = padDescriptor;
                this.Text = SkyMap.Net.Core.StringParser.Parse(padDescriptor.Title);
                if (!string.IsNullOrEmpty(padDescriptor.Icon))
                {
                    base.Image = IconService.GetBitmap(padDescriptor.Icon);
                }
                if (!string.IsNullOrEmpty(padDescriptor.Shortcut))
                {
                    base.ShortcutKeys = MenuCommand.ParseShortcut(padDescriptor.Shortcut);
                }
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                this.padDescriptor.BringPadToFront();
            }
        }
    }
}

