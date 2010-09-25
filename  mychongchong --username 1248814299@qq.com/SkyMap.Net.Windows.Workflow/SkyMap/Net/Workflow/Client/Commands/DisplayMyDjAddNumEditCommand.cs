namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class DisplayMyDjAddNumEditCommand : AbstractSpinEditCommand
    {
        public override void Run()
        {
            Dj.AddProinstNum = Convert.ToInt32((this.Owner as ToolBarSpinEdit).EditValue);
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
                if (value != null)
                {
                    (value as ToolBarSpinEdit).SetBound(1, 30);
                }
            }
        }
    }
}

