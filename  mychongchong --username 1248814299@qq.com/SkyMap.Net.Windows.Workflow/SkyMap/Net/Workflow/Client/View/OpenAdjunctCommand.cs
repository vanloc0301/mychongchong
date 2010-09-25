namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Gui;
    using System;

    public class OpenAdjunctCommand : AdjunctEditCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                WaitDialogHelper.Show();
                try
                {
                    base.OpenAdjunct();
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return (base.ae.GetFocusAdjunct() != null);
            }
            set
            {
            }
        }
    }
}

