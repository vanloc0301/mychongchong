namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;

    public abstract class AbstractSelectCommand : AbstractMenuCommand
    {
        protected GridPanel gp;

        protected AbstractSelectCommand()
        {
        }

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.gp = (GridPanel) this.Owner;
        }
    }
}

