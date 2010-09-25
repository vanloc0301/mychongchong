namespace SkyMap.Net.Tools.Workflow
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Windows.Forms;

    public class WfTempletAppendixNode : AbstractDomainObjectNode<WfTempletAppendix>
    {
        public override ObjectNode AddChild()
        {
            WfAppendix appendix = new WfAppendix();
            appendix.DupliNum = 0;
            appendix.Name = "新收件资料";
            appendix.OldNum = 1;
            appendix.Source = "原件";
            appendix.WfTempletAppendix = base.DomainObject;
            appendix.Save();
            return base.AddSingleNode<WfAppendix, WfAppendixNode>(appendix);
        }

        public override void Delete()
        {
            (base.Parent as WfTempletAppendixsNode).DomainObject.TempletAppendixs.Remove(base.DomainObject);
            base.Delete();
        }

        protected override void Initialize()
        {
            if (base.DomainObject.WfAppendixs != null)
            {
                base.AddNodes<WfAppendix, WfAppendixNode>(base.DomainObject.WfAppendixs);
            }
        }

        public override void Paste()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            if (dataObject.GetDataPresent(typeof(WfAppendix)))
            {
                WfAppendix data = (WfAppendix) dataObject.GetData(typeof(WfAppendix));
                if (!base.DomainObject.WfAppendixs.Contains(data))
                {
                    WfAppendix item = data.Clone<WfAppendix>();
                    item.WfTempletAppendix = base.DomainObject;
                    base.DomainObject.WfAppendixs.Add(item);
                    item.Save();
                    base.AddSingleNode<WfAppendix, WfAppendixNode>(item);
                }
            }
        }
    }
}

