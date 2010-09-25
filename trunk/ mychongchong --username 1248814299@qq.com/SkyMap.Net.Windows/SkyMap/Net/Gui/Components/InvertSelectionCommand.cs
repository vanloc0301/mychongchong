namespace SkyMap.Net.Gui.Components
{
    using System;

    public class InvertSelectionCommand : AbstractSelectCommand
    {
        public override void Run()
        {
            base.gp.InvertSelection();
        }
    }
}

