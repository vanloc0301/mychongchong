namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.Data;

    public class CountCommand : ColumnStaticCommand
    {
        protected override SummaryItemType Type
        {
            get
            {
                return SummaryItemType.Count;
            }
        }
    }
}

