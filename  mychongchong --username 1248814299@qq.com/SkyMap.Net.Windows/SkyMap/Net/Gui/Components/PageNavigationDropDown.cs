namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public class PageNavigationDropDown : AbstractMenuCommand
    {
        private ToolBarDropDownButton dropDownButton;

        private void GenerateDropDownItems()
        {
            ToolStripItem[] toolStripItems = (ToolStripItem[]) AddInTree.GetTreeNode("/GridPanel/Toolbar/PageNavigation").BuildChildItems(this.Owner).ToArray(typeof(ToolStripItem));
            foreach (ToolStripItem item in toolStripItems)
            {
                if (item is IStatusUpdate)
                {
                    ((IStatusUpdate) item).UpdateStatus();
                }
            }
            this.dropDownButton.DropDownItems.AddRange(toolStripItems);
        }

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.dropDownButton = (ToolBarDropDownButton) this.Owner;
            this.GenerateDropDownItems();
        }

        public override void Run()
        {
        }
    }
}

