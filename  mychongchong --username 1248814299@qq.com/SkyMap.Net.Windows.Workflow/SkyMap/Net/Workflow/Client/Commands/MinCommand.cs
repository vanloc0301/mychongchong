﻿namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.Data;

    public class MinCommand : ColumnStaticCommand
    {
        protected override SummaryItemType Type
        {
            get
            {
                return SummaryItemType.Min;
            }
        }
    }
}

