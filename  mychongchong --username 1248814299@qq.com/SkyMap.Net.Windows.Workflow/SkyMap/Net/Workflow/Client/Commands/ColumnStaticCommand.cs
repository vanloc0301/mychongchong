namespace SkyMap.Net.Workflow.Client.Commands
{
    using DevExpress.Data;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public abstract class ColumnStaticCommand : AbstractMenuCommand
    {
        protected ColumnStaticCommand()
        {
        }

        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if ((owner != null) && ((owner.view.FocusedColumn != null) && (owner.view.FocusedColumn.SummaryItem.SummaryType != this.Type)))
            {
                owner.view.OptionsView.ShowFooter = true;
                owner.view.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
                owner.view.FocusedColumn.SummaryItem.SummaryType = this.Type;
                for (int i = owner.view.GroupSummary.Count - 1; i > -1; i--)
                {
                    GridSummaryItem item = owner.view.GroupSummary[i];
                    if (item.FieldName == owner.view.FocusedColumn.FieldName)
                    {
                        owner.view.GroupSummary.RemoveAt(i);
                        break;
                    }
                }
                owner.view.GroupSummary.Add(this.Type, owner.view.FocusedColumn.FieldName);
            }
        }

        protected abstract SummaryItemType Type { get; }
    }
}

