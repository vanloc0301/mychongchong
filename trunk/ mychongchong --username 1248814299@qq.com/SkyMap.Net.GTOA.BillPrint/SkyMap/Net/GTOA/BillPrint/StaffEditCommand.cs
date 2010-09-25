namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using System;

    public class StaffEditCommand : AbstractTextEditCommand
    {
        public static string StaffName;

        public override void Run()
        {
            StaffName = (this.Owner as ToolBarTextBox).TextBox.Text;
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
                StaffName = (value as ToolBarTextBox).TextBox.Text = SecurityUtil.GetSmIdentity().UserName;
                SecurityUtil.CurrentPrincipalChanged += delegate (object sender, EventArgs e) {
                    StaffName = (this.Owner as ToolBarTextBox).TextBox.Text = SecurityUtil.GetSmIdentity().UserName;
                };
            }
        }
    }
}

