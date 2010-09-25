namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using System;

    public class DeptEditCommand : AbstractTextEditCommand
    {
        public static string DeptName;

        public override void Run()
        {
            DeptName = (this.Owner as ToolBarTextBox).TextBox.Text;
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
                DeptName = (value as ToolBarTextBox).TextBox.Text = SecurityUtil.GetSmPrincipal().DeptNames[0];
                SecurityUtil.CurrentPrincipalChanged += delegate (object sender, EventArgs e) {
                    DeptName = (this.Owner as ToolBarTextBox).TextBox.Text = SecurityUtil.GetSmPrincipal().DeptNames[0];
                };
            }
        }
    }
}

