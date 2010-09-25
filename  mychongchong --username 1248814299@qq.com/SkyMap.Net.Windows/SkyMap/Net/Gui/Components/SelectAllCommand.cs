namespace SkyMap.Net.Gui.Components
{
    using System;

    public class SelectAllCommand : AbstractSelectCommand
    {
        public override void Run()
        {
            base.gp.SelectAll();
        }
    }
}

