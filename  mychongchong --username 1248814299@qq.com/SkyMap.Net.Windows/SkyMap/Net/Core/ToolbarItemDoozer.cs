namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class ToolbarItemDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            string str = codon.Properties.Contains("type") ? codon.Properties["type"] : "Item";
            bool createCommand = codon.Properties["loadclasslazy"] == "false";
            switch (str)
            {
                case "Separator":
                    return new ToolBarSeparator(codon, caller);

                case "CheckBox":
                    return new ToolBarCheckBox(codon, caller);

                case "Item":
                    return new ToolBarCommand(codon, caller, createCommand);

                case "ComboBox":
                    return new ToolBarComboBox(codon, caller);

                case "DropDownButton":
                    return new ToolBarDropDownButton(codon, caller);

                case "PopupEdit":
                    return new ToolBarPopupEdit(codon, caller);

                case "SpinEdit":
                    return new ToolBarSpinEdit(codon, caller);

                case "TextBox":
                    return new ToolBarTextBox(codon, caller);

                case "CalcEdit":
                    return new ToolBarCalcEdit(codon, caller);

                case "Builder":
                    return codon.AddIn.CreateObject(codon.Properties["class"]);

                case "Label":
                    return new ToolBarLabel(codon, caller);

                case "DateTime":
                    return new ToolBarDateTimeEdit(codon, caller);
            }
            throw new NotSupportedException("unsupported menu item type : " + str);
        }

        public bool HandleConditions
        {
            get
            {
                return true;
            }
        }
    }
}

