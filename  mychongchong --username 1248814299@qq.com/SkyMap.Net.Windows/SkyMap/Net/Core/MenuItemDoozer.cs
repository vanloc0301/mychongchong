namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class MenuItemDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            string str = codon.Properties.Contains("type") ? codon.Properties["type"] : "Command";
            bool createCommand = codon.Properties["loadclasslazy"] == "false";
            switch (str)
            {
                case "Separator":
                    return new MenuSeparator(codon, caller);

                case "CheckBox":
                    return new MenuCheckBox(codon, caller);

                case "Item":
                case "Command":
                    return new MenuCommand(codon, caller, createCommand);

                case "Menu":
                    return new Menu(codon, caller, subItems);

                case "Builder":
                    return codon.AddIn.CreateObject(codon.Properties["class"]);
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

