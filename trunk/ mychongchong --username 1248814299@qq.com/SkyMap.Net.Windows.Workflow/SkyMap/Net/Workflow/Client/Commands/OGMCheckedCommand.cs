namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using System;

    public class OGMCheckedCommand : YwCriteriaCheckedCommand
    {
        public override bool IsEnabled
        {
            get
            {
                return SecurityUtil.IsCanAccess("AdminData,Admin");
            }
            set
            {
            }
        }

        protected override string Key
        {
            get
            {
                return "s.STAFF_ID";
            }
        }

        protected override Type LinkCommandType
        {
            get
            {
                return typeof(QueryEditOGMCommand);
            }
        }

        public override object Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                (base.Owner as ToolBarCheckBox).Checked = true;
            }
        }
    }
}

