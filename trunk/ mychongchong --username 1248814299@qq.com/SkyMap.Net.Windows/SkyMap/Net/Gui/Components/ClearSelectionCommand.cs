namespace SkyMap.Net.Gui.Components
{
    using System;

    public class ClearSelectionCommand : AbstractSelectCommand
    {
        public override void Run()
        {
            base.gp.ClearSelection();
        }
    }
}

