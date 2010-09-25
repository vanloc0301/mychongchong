namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;

    public abstract class AbstractPageNavigateCommand : AbstractMenuCommand
    {
        protected GridPanel gp;

        protected AbstractPageNavigateCommand()
        {
        }

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.gp = (GridPanel) this.Owner;
        }

        public override void Run()
        {
            if (this.gp != null)
            {
                int pageIndex = this.PageIndex;
                this.gp.DataSource = this.gp.PageNavigation(pageIndex);
                this.gp.PageIndex = pageIndex;
            }
        }

        protected abstract int PageIndex { get; }
    }
}

