namespace SkyMap.Net.Gui
{
    using System;

    public class ExtFolderNode : ExtTreeNode
    {
        private string closedIcon = null;
        private string openedIcon = null;

        public override void Collapsing()
        {
            base.Collapsing();
            if (this.closedIcon != null)
            {
                base.SetIcon(this.closedIcon);
            }
        }

        public override void Expanding()
        {
            base.Expanding();
            if (this.openedIcon != null)
            {
                base.SetIcon(this.openedIcon);
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            if (base.Nodes.Count == 0)
            {
                base.SetIcon(this.ClosedIcon);
            }
            else if (base.IsExpanded)
            {
                base.SetIcon(this.OpenedIcon);
            }
        }

        public string ClosedIcon
        {
            get
            {
                return this.closedIcon;
            }
            set
            {
                this.closedIcon = value;
                if (!((this.closedIcon == null) || base.IsExpanded))
                {
                    base.SetIcon(this.closedIcon);
                }
            }
        }

        public string OpenedIcon
        {
            get
            {
                return this.openedIcon;
            }
            set
            {
                this.openedIcon = value;
                if ((this.openedIcon != null) && base.IsExpanded)
                {
                    base.SetIcon(this.openedIcon);
                }
            }
        }
    }
}

