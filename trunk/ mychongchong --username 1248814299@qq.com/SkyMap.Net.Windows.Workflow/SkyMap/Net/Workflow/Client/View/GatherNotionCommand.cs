namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GatherNotionCommand : StaffNotionCommand
    {
        public override void Run()
        {
            WfStaffNotion myNotion = base.sn.GetMyNotion();
            if (myNotion != null)
            {
                StringBuilder builder = new StringBuilder();
                List<WfStaffNotion> selectedOrFocusStaffNotions = base.sn.GetSelectedOrFocusStaffNotions();
                foreach (WfStaffNotion notion2 in selectedOrFocusStaffNotions)
                {
                    if (!notion2.Equals(myNotion))
                    {
                        builder.Append(notion2.StaffName).Append("\r\n").Append(notion2.Content);
                    }
                }
                myNotion.Content = myNotion.Content + "\r\n" + builder.ToString();
                base.sn.ResumeBinding();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                WfStaffNotion myNotion = base.sn.GetMyNotion();
                if (myNotion != null)
                {
                    List<WfStaffNotion> selectedOrFocusStaffNotions = base.sn.GetSelectedOrFocusStaffNotions();
                    if (!((selectedOrFocusStaffNotions.Count <= 0) || selectedOrFocusStaffNotions.Contains(myNotion)))
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
            }
        }
    }
}

