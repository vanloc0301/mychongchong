namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Tools.Workflow;

    public class AddWfTempletAppendixsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                WfTempletAppendixs appendixs = new WfTempletAppendixs();
                appendixs.Name = "收件资料模板集合的集合";
                appendixs.TempletAppendixs = new List<WfTempletAppendix>();
                appendixs.Save();
                owner.AddSingleNode<WfTempletAppendixs, WfTempletAppendixsNode>(appendixs).ActivateItem();
            }
        }
    }
}

