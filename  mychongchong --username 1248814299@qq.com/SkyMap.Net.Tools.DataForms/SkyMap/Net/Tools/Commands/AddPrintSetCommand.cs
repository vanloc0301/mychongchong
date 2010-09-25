namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Tools.DataForms;

    public class AddPrintSetCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                PrintSet set = new PrintSet();
                set.Id = StringHelper.GetNewGuid();
                set.Name = "新报表集合";
                set.Description = "新报表集合";
                set.TempletPrints = new List<TempletPrint>();
                set.Save();
                owner.AddSingleNode<PrintSet, PrintSetNode>(set);
            }
        }
    }
}

