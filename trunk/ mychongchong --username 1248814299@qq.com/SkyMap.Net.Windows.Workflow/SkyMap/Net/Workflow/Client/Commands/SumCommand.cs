namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.Data;

    public class SumCommand : ColumnStaticCommand
    {
        protected override SummaryItemType Type
        {
            get
            {
                return SummaryItemType.Sum;
            }
        }
    }
}

