namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class ProjectIdCheckedCommand : YwCriteriaCheckedCommand
    {
        protected override string Key
        {
            get
            {
                return "p.PROJECT_ID";
            }
        }

        protected override Type LinkCommandType
        {
            get
            {
                return typeof(QueryEditProjectIdCommand);
            }
        }
    }
}

