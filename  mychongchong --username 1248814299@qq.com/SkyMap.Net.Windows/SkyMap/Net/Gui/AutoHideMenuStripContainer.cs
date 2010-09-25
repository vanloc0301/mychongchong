namespace SkyMap.Net.Gui
{
    using System;
    using System.Windows.Forms;

    public class AutoHideMenuStripContainer : AutoHideContainer
    {
        private Padding? defaultPadding;
        protected bool dropDownOpened;

        public AutoHideMenuStripContainer(MenuStrip menuStrip) : base(menuStrip)
        {
            menuStrip.AutoSize = false;
            menuStrip.ItemAdded += new ToolStripItemEventHandler(this.OnMenuItemAdded);
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                this.AddEventHandlersForItem(item);
            }
        }

        private void AddEventHandlersForItem(ToolStripMenuItem menuItem)
        {
            menuItem.DropDownOpened += delegate {
                this.dropDownOpened = true;
            };
            menuItem.DropDownClosed += delegate {
                this.dropDownOpened = false;
                if (!base.mouseIn)
                {
                    base.ShowOverlay = false;
                }
            };
        }

        protected override void OnControlMouseLeave(object sender, EventArgs e)
        {
            base.mouseIn = false;
            if (!this.dropDownOpened)
            {
                base.ShowOverlay = false;
            }
        }

        private void OnMenuItemAdded(object sender, EventArgs e)
        {
            this.AddEventHandlersForItem((ToolStripMenuItem) sender);
        }

        protected override void Reformat()
        {
            if (!this.defaultPadding.HasValue)
            {
                this.defaultPadding = new Padding?(((MenuStrip) base.control).Padding);
            }
            ((MenuStrip) base.control).Padding = this.AutoHide ? Padding.Empty : this.defaultPadding.Value;
            base.Reformat();
        }
    }
}

