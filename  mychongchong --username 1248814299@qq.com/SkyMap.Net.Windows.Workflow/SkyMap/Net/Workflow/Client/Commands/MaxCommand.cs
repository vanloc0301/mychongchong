namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.Data;

    public class MaxCommand : ColumnStaticCommand
    {
        protected override SummaryItemType Type
        {
            get
            {
                return SummaryItemType.Max;
            }
        }
    }
}

