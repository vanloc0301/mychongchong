namespace SkyMap.Net.Gui.Components
{
    using System;

    public class FirstPageNavigateCommand : AbstractPageNavigateCommand
    {
        protected override int PageIndex
        {
            get
            {
                return 1;
            }
        }
    }
}

