namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraEditors.Controls;
    using System;

    public class ChineseControlLocalizer : Localizer
    {
        public override string GetLocalizedString(StringId id)
        {
            switch (id)
            {
                case StringId.NavigatorFirstButtonHint:
                    return "首个";

                case StringId.NavigatorPreviousButtonHint:
                    return "前一个";

                case StringId.NavigatorPreviousPageButtonHint:
                    return "前页";

                case StringId.NavigatorNextButtonHint:
                    return "下一个";

                case StringId.NavigatorNextPageButtonHint:
                    return "下页";

                case StringId.NavigatorLastButtonHint:
                    return "末个";

                case StringId.NavigatorAppendButtonHint:
                    return "添加";

                case StringId.NavigatorRemoveButtonHint:
                    return "删除";

                case StringId.NavigatorEditButtonHint:
                    return "编辑";

                case StringId.NavigatorEndEditButtonHint:
                    return "结束编辑";

                case StringId.NavigatorCancelEditButtonHint:
                    return "取消";

                case StringId.NavigatorTextStringFormat:
                    return "当前：{0}，共：{1}";

                case StringId.XtraMessageBoxOkButtonText:
                    return "确定";

                case StringId.XtraMessageBoxCancelButtonText:
                    return "取消";

                case StringId.XtraMessageBoxYesButtonText:
                    return "是";

                case StringId.XtraMessageBoxNoButtonText:
                    return "否";

                case StringId.XtraMessageBoxAbortButtonText:
                    return "终止";

                case StringId.XtraMessageBoxRetryButtonText:
                    return "重试";

                case StringId.XtraMessageBoxIgnoreButtonText:
                    return "忽略";

                case StringId.TextEditMenuUndo:
                    return "撤消";

                case StringId.TextEditMenuCut:
                    return "剪切";

                case StringId.TextEditMenuCopy:
                    return "复制";

                case StringId.TextEditMenuPaste:
                    return "粘贴";

                case StringId.TextEditMenuDelete:
                    return "删除";

                case StringId.TextEditMenuSelectAll:
                    return "全选";
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

