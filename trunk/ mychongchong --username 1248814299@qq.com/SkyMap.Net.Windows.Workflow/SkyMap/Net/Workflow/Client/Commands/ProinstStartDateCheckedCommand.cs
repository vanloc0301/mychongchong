namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class ProinstStartDateCheckedCommand : YwCriteriaCheckedCommand
    {
        protected override string Key
        {
            get
            {
                return "p.PROINST_STARTDATE";
            }
        }

        protected override Type LinkCommandType
        {
            get
            {
                return typeof(QueryEditProinstStartDateCommand);
            }
        }
    }
}

