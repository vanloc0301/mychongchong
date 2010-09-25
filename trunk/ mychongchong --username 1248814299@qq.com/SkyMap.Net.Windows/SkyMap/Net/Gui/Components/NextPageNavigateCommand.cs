namespace SkyMap.Net.Gui.Components
{
    using System;

    public class NextPageNavigateCommand : AbstractPageNavigateCommand
    {
        protected override int PageIndex
        {
            get
            {
                return (base.gp.PageIndex + 1);
            }
        }
    }
}

