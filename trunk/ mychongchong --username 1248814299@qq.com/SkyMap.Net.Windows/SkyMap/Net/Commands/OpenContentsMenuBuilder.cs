namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class OpenContentsMenuBuilder : ISubmenuBuilder
    {
        public ToolStripItem[] BuildSubmenu(Codon codon, object owner)
        {
            int count = WorkbenchSingleton.Workbench.ViewContentCollection.Count;
            if (count == 0)
            {
                return new ToolStripItem[0];
            }
            ToolStripItem[] itemArray = new ToolStripItem[count + 1];
            itemArray[0] = new MenuSeparator(null, null);
            for (int i = 0; i < count; i++)
            {
                IViewContent content = WorkbenchSingleton.Workbench.ViewContentCollection[i];
                if (content.WorkbenchWindow != null)
                {
                    MenuCheckBox box = new MyMenuItem(content);
                    box.Tag = content.WorkbenchWindow;
                    box.Checked = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == content.WorkbenchWindow;
                    box.Description = "Activate this window ";
                    itemArray[i + 1] = box;
                }
            }
            return itemArray;
        }

        private class MyMenuItem : MenuCheckBox
        {
            private IViewContent content;

            public MyMenuItem(IViewContent content) : base(SkyMap.Net.Core.StringParser.Parse(content.TitleName))
            {
                this.content = content;
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                base.Checked = true;
                this.content.WorkbenchWindow.SelectWindow();
            }
        }
    }
}

