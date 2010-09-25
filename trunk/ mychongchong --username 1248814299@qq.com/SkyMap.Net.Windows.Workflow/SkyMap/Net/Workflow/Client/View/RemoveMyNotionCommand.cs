namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;

    public class RemoveMyNotionCommand : StaffNotionCommand
    {
        public override void Run()
        {
            List<WfStaffNotion> selectedOrFocusStaffNotions = base.sn.GetSelectedOrFocusStaffNotions();
            foreach (WfStaffNotion notion in selectedOrFocusStaffNotions)
            {
                if (StaffNotionCommand.IsCanEditOrDel(notion))
                {
                    base.sn.CurrentUnitOfWork.RegisterRemoved(notion);
                }
            }
            base.sn.RemoveSelectedOrFocusedWSN();
        }

        public override bool IsEnabled
        {
            get
            {
                List<WfStaffNotion> selectedOrFocusStaffNotions = base.sn.GetSelectedOrFocusStaffNotions();
                if (selectedOrFocusStaffNotions.Count == 0)
                {
                    return false;
                }
                foreach (WfStaffNotion notion in selectedOrFocusStaffNotions)
                {
                    if (!StaffNotionCommand.IsCanEditOrDel(notion))
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
            }
        }
    }
}

