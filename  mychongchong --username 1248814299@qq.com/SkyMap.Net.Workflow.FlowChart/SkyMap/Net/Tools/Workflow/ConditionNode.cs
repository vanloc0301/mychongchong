namespace SkyMap.Net.Tools.Workflow
{
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Workflow.XPDL;

    public class ConditionNode : AbstractDomainObjectNode<Condition>
    {
        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

