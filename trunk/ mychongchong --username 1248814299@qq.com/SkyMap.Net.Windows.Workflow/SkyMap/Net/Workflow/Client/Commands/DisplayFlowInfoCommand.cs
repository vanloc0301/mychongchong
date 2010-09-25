namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.View;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Windows.Forms;

    public class DisplayFlowInfoCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                WaitDialogHelper.Show();
                try
                {
                    int firstSelectedIndex = BoxHelper.GetFirstSelectedIndex(owner);
                    WorkItem workItem = BoxHelper.GetWorkItem(BoxHelper.GetDataRowView(owner, firstSelectedIndex), owner);
                    if (!StringHelper.IsNull(workItem.ProinstId))
                    {
                        FlowInfo info = new FlowInfo();
                        info.ProinstId = workItem.ProinstId;
                        info.Dock = DockStyle.Fill;
                        SmForm form = new SmForm();
                        form.Text = info.Text;
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ClientSize = info.Size;
                        form.Controls.Add(info);
                        form.ShowDialog();
                    }
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }
    }
}

