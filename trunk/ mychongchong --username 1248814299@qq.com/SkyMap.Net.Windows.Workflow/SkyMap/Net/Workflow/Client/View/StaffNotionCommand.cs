namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Instance;
    using System;

    public abstract class StaffNotionCommand : AbstractMenuCommand
    {
        protected StaffNotion sn;

        protected StaffNotionCommand()
        {
        }

        internal static bool IsCanEditOrDel(WfStaffNotion wsn)
        {
            if (wsn == null)
            {
                return false;
            }
            return (wsn.StaffId == SecurityUtil.GetSmIdentity().UserId);
        }

        public override object Owner
        {
            get
            {
                return this.sn;
            }
            set
            {
                this.sn = value as StaffNotion;
            }
        }
    }
}

