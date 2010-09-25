namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class PrintSetNode : AbstractDomainObjectNode<PrintSet>
    {
        public override ObjectNode AddChild()
        {
            TempletPrint item = new TempletPrint();
            item.Id = StringHelper.GetNewGuid();
            item.Name = "新报表";
            item.Description = "新报表";
            item.PrintSets = new List<PrintSet>(1);
            item.PrintSets.Add(base.DomainObject);
            base.DomainObject.TempletPrints.Add(item);
            item.Save();
            return base.AddSingleNode<TempletPrint, TempletPrintNode>(item);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (base.DomainObject != null)
            {
                base.Nodes.Clear();
                base.AddNodes<TempletPrint, TempletPrintNode>(base.DomainObject.TempletPrints.OrderBy<TempletPrint, string>(delegate (TempletPrint p) {
                    return p.Name;
                }).ToList<TempletPrint>());
            }
        }

        public override void Paste()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            if (dataObject.GetDataPresent(typeof(TempletPrint)))
            {
                TempletPrint data = (TempletPrint) dataObject.GetData(typeof(TempletPrint));
                base.DomainObject.TempletPrints.Add(data);
                data.PrintSets.Add(base.DomainObject);
                data.Update();
                base.AddSingleNode<TempletPrint, TempletPrintNode>(data);
            }
        }
    }
}

