namespace SkyMap.Net.Tools.Workflow
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class WfTempletAppendixsNode : AbstractDomainObjectNode<WfTempletAppendixs>
    {
        public override ObjectNode AddChild()
        {
            WfTempletAppendix item = new WfTempletAppendix();
            item.Name = "新收件资料集合";
            item.WfAppendixs = new List<WfAppendix>();
            base.DomainObject.TempletAppendixs.Add(item);
            item.Save();
            base.DomainObject.Update();
            return base.AddSingleNode<WfTempletAppendix, WfTempletAppendixNode>(item);
        }

        protected override void Initialize()
        {
            if (base.DomainObject.TempletAppendixs != null)
            {
                base.AddNodes<WfTempletAppendix, WfTempletAppendixNode>(base.DomainObject.TempletAppendixs);
            }
        }

        public override void Paste()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            if (dataObject.GetDataPresent(typeof(WfTempletAppendix)))
            {
                WfTempletAppendix data = (WfTempletAppendix) dataObject.GetData(typeof(WfTempletAppendix));
                if (!base.DomainObject.TempletAppendixs.Contains(data))
                {
                    base.DomainObject.TempletAppendixs.Add(data);
                    base.DomainObject.Update();
                    base.AddSingleNode<WfTempletAppendix, WfTempletAppendixNode>(data);
                }
            }
        }
    }
}

