namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using System;

    public class MatersEditCommand : AbstractMenuCommand
    {
        protected MatersEdit me;

        public override void Run()
        {
        }

        public override object Owner
        {
            get
            {
                return this.me;
            }
            set
            {
                this.me = value as MatersEdit;
            }
        }
    }
}

