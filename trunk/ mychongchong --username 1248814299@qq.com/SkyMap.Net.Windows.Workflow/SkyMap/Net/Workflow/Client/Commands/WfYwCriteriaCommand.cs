namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WfYwCriteriaCommand : AbstractMenuCommand
    {
        public const string FieldProdefId = "p.PRODEF_ID";
        public const string FieldProinstStartDate = "p.PROINST_STARTDATE";
        public const string FieldProinstStatus = "p.PROINST_STATUS";
        public const string FieldProjectId = "p.PROJECT_ID";
        public const string FieldRequestStaff = "r.STAFF_ID";
        public const string FieldStaff = "s.STAFF_ID";

        public override void Run()
        {
            WfYwCriteria owner = this.Owner as WfYwCriteria;
            if (owner != null)
            {
                WaitDialogHelper.Show();
                try
                {
                    Dictionary<string, string> queryParameters = new Dictionary<string, string>();
                    foreach (IStatusUpdate update in owner.GetToolStripItems(typeof(QueryEditCommand)))
                    {
                        QueryEditCommand command = update.Command as QueryEditCommand;
                        if (command != null)
                        {
                            command.AddToQueryParameters(queryParameters);
                        }
                    }
                    StringBuilder builder = new StringBuilder("1=1\r");
                    foreach (string str in queryParameters.Values)
                    {
                        if (str.Length > 0)
                        {
                            builder.Append(" and ");
                            builder.Append(str).Append("\r");
                        }
                    }
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("SQL:{0}", new object[] { builder.ToString() });
                    }
                    owner.DataSource = BoxHelper.GetData(owner, new string[] { builder.ToString() });
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }
    }
}

