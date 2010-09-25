namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.Data;

    public class AvgCommand : ColumnStaticCommand
    {
        protected override SummaryItemType Type
        {
            get
            {
                return SummaryItemType.Average;
            }
        }
    }
}

