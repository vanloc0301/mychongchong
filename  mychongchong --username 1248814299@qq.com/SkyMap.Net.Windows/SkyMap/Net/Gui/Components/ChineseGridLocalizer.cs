namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraGrid.Localization;
    using System;

    public class ChineseGridLocalizer : GridLocalizer
    {
        public override string GetLocalizedString(GridStringId id)
        {
            switch (id)
            {
                case GridStringId.FileIsNotFoundError:
                    return "没有找到文件： {0} ";

                case GridStringId.ColumnViewExceptionMessage:
                    return " 你是否想改正这个值 ?";

                case GridStringId.CustomizationCaption:
                    return "定制列头";

                case GridStringId.CustomizationColumns:
                    return "定制列";

                case GridStringId.CustomizationBands:
                    return "Bands";

                case GridStringId.PopupFilterAll:
                    return "(所有)";

                case GridStringId.PopupFilterCustom:
                    return "(高级查询)";

                case GridStringId.PopupFilterBlanks:
                    return "(等于空)";

                case GridStringId.PopupFilterNonBlanks:
                    return "(非空)";

                case GridStringId.CustomFilterDialogFormCaption:
                    return "高级过滤查询";

                case GridStringId.CustomFilterDialogCaption:
                    return "过滤条件:";

                case GridStringId.CustomFilterDialogRadioAnd:
                    return "且";

                case GridStringId.CustomFilterDialogRadioOr:
                    return "或";

                case GridStringId.CustomFilterDialogOkButton:
                    return "确定(&O)";

                case GridStringId.CustomFilterDialog2FieldCheck:
                    return "字段";

                case GridStringId.CustomFilterDialogCancelButton:
                    return "取消(&C)";

                case GridStringId.CustomFilterDialogConditionEQU:
                    return "=";

                case GridStringId.CustomFilterDialogConditionNEQ:
                    return "";

                case GridStringId.CustomFilterDialogConditionGT:
                    return ">";

                case GridStringId.CustomFilterDialogConditionGTE:
                    return ">=";

                case GridStringId.CustomFilterDialogConditionLT:
                    return "<";

                case GridStringId.CustomFilterDialogConditionLTE:
                    return "<=";

                case GridStringId.CustomFilterDialogConditionBlanks:
                    return "为空";

                case GridStringId.CustomFilterDialogConditionNonBlanks:
                    return "非空";

                case GridStringId.CustomFilterDialogConditionLike:
                    return "包含";

                case GridStringId.CustomFilterDialogConditionNotLike:
                    return "不包含";

                case GridStringId.MenuFooterSum:
                    return "Sum";

                case GridStringId.MenuFooterMin:
                    return "Min";

                case GridStringId.MenuFooterMax:
                    return "Max";

                case GridStringId.MenuFooterCount:
                    return "Count";

                case GridStringId.MenuFooterAverage:
                    return "Average";

                case GridStringId.MenuFooterNone:
                    return "None";

                case GridStringId.MenuFooterSumFormat:
                    return "SUM={0:#.##}";

                case GridStringId.MenuFooterMinFormat:
                    return "MIN={0}";

                case GridStringId.MenuFooterMaxFormat:
                    return "MAX={0}";

                case GridStringId.MenuFooterCountFormat:
                    return "{0}";

                case GridStringId.MenuFooterAverageFormat:
                    return "AVR={0:#.##}";

                case GridStringId.MenuColumnSortAscending:
                    return "Sort Ascending";

                case GridStringId.MenuColumnSortDescending:
                    return "Sort Descending";

                case GridStringId.MenuColumnGroup:
                    return "Group By This Field";

                case GridStringId.MenuColumnUnGroup:
                    return "UnGroup";

                case GridStringId.MenuColumnColumnCustomization:
                    return "Runtime Column Customization";

                case GridStringId.MenuColumnBestFit:
                    return "Best Fit";

                case GridStringId.MenuColumnFilter:
                    return "Can Filter";

                case GridStringId.MenuColumnClearFilter:
                    return "Clear Filter";

                case GridStringId.MenuColumnBestFitAllColumns:
                    return "Best Fit (all columns)";

                case GridStringId.MenuGroupPanelFullExpand:
                    return "Full Expand";

                case GridStringId.MenuGroupPanelFullCollapse:
                    return "Full Collapse";

                case GridStringId.MenuGroupPanelClearGrouping:
                    return "Clear Grouping";

                case GridStringId.CardViewQuickCustomizationButton:
                    return "排序查询";

                case GridStringId.CardViewQuickCustomizationButtonFilter:
                    return "查询";

                case GridStringId.CardViewQuickCustomizationButtonSort:
                    return "排序";

                case GridStringId.GridGroupPanelText:
                    return "拖动分组的列到这里";
            }
            return base.GetLocalizedString(id);
        }

        public override string Language
        {
            get
            {
                return "Chinese";
            }
        }
    }
}

