namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Commands;
    using SkyMap.Net.Tools.Workflow;
    using SkyMap.Net.Workflow.XPDL;
    using SkyMap.Net.Workflow.FlowChart;

    public class EditFlowCommand : ShowEditViewCommand<ActdefViewContent, Prodef>
    {
        protected override Prodef DomainObject
        {
            get
            {
                ProdefNode owner = (ProdefNode) this.Owner;
                if (owner != null)
                {
                    return owner.DomainObject;
                }
                return null;
            }
        }
    }
}

