namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DialogPanelDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            string input = codon.Properties["label"];
            if ((subItems == null) || (subItems.Count == 0))
            {
                if (codon.Properties.Contains("class"))
                {
                    return new DefaultDialogPanelDescriptor(codon.Id, SkyMap.Net.Core.StringParser.Parse(input), codon.AddIn, codon.Properties["class"]);
                }
                return new DefaultDialogPanelDescriptor(codon.Id, SkyMap.Net.Core.StringParser.Parse(input));
            }
            return new DefaultDialogPanelDescriptor(codon.Id, SkyMap.Net.Core.StringParser.Parse(input), new List<IDialogPanelDescriptor>((IDialogPanelDescriptor[]) subItems.ToArray(typeof(IDialogPanelDescriptor))));
        }

        public bool HandleConditions
        {
            get
            {
                return false;
            }
        }
    }
}

