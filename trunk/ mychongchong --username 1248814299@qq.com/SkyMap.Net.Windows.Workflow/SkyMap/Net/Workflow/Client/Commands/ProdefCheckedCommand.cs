namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class ProdefCheckedCommand : YwCriteriaCheckedCommand
    {
        protected override string Key
        {
            get
            {
                return "p.PRODEF_ID";
            }
        }

        protected override Type LinkCommandType
        {
            get
            {
                return typeof(QueryEditProdefCommand);
            }
        }
    }
}

