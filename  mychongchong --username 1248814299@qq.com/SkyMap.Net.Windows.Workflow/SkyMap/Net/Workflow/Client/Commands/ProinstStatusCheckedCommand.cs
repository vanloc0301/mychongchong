namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class ProinstStatusCheckedCommand : YwCriteriaCheckedCommand
    {
        protected override string Key
        {
            get
            {
                return "p.PROINST_STATUS";
            }
        }

        protected override Type LinkCommandType
        {
            get
            {
                return typeof(QueryEditProinstStatusCommand);
            }
        }
    }
}

