namespace SkyMap.Net.Gui.Components
{
    using System;

    public class PreviousPageNavigateCommand : AbstractPageNavigateCommand
    {
        protected override int PageIndex
        {
            get
            {
                int num = base.gp.PageIndex - 1;
                if (num < 1)
                {
                    num = 1;
                }
                return num;
            }
        }
    }
}

