namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.Windows.Forms;

    public class MonitorDateCommand : AbstractBoxCommand, IPopupEditCommand, ICommand
    {
        private DateEditEx dte = new DateEditEx();

        public override void Run()
        {
        }

        public string Operator
        {
            get
            {
                return " between ";
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
                ToolBarPopupEdit edit = value as ToolBarPopupEdit;
                edit.Control.Width = 200;
                this.dte.Dock = DockStyle.Fill;
                edit.PopupControl.Controls.Add(this.dte);
            }
        }

        public string Value
        {
            get
            {
                return ("'" + this.dte.StartDate.ToString("yyyy-MM-dd") + "' and '" + this.dte.EndDate.AddDays(1.0).ToString("yyyy-MM-dd") + "'");
            }
        }
    }
}

