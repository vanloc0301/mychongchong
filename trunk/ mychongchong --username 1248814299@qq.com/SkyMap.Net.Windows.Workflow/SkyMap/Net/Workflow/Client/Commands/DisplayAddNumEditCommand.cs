namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using System;

    public class DisplayAddNumEditCommand : AbstractSpinEditCommand
    {
        private static int num = 1;

        public override void Run()
        {
            num = Convert.ToInt32((this.Owner as ToolBarSpinEdit).EditValue);
        }

        public static int Num
        {
            get
            {
                return num;
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
                if (value != null)
                {
                    (value as ToolBarSpinEdit).SetBound(1, 30);
                }
            }
        }
    }
}

