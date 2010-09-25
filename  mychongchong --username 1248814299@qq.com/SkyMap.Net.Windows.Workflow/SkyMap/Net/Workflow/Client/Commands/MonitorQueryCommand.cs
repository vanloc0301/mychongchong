namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class MonitorQueryCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if (owner != null)
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    IStatusUpdate toolStripItem = owner.GetToolStripItem(typeof(MonitorDateCommand));
                    if (toolStripItem != null)
                    {
                        MonitorDateCommand command = toolStripItem.Command as MonitorDateCommand;
                        owner.DataSource = BoxHelper.GetData(owner, "SkyMap.Net.Workflow", base.ID, new string[] { command.Operator, command.Value });
                        if (base.Codon != null)
                        {
                            if (base.Codon.Properties.Contains("group"))
                            {
                                owner.SetGridGroupFields(base.Codon.Properties["group"].Split(new char[] { ',' }));
                            }
                            if (base.Codon.Properties.Contains("sum") || base.Codon.Properties.Contains("count"))
                            {
                                string[] sumFields = null;
                                string countField = null;
                                if (base.Codon.Properties.Contains("sum"))
                                {
                                    sumFields = base.Codon.Properties["sum"].Split(new char[] { ',' });
                                }
                                if (base.Codon.Properties.Contains("count"))
                                {
                                    countField = base.Codon.Properties["count"];
                                }
                                owner.SetGridFooter(sumFields, countField);
                            }
                        }
                        owner.RefreshColumnAutoWidth();
                    }
                    else
                    {
                        LoggingService.WarnFormatted("没有找到MonitorDateCommand:{0}", new object[] { owner.GetToolStripItem(typeof(MonitorDateCommand)) });
                    }
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

