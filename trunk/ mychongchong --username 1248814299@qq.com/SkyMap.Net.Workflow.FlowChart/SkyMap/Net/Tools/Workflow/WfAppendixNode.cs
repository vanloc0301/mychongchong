namespace SkyMap.Net.Tools.Workflow
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;

    public class WfAppendixNode : AbstractDomainObjectNode<WfAppendix>
    {
        public override void Delete()
        {
            (base.Parent as WfTempletAppendixNode).DomainObject.WfAppendixs.Remove(base.DomainObject);
            base.Delete();
        }

        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

