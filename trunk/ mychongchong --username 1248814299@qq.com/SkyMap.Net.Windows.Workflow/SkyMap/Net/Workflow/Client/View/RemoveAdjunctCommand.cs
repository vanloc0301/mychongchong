namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Windows.Forms;

    public class RemoveAdjunctCommand : AddAdjunctCommand
    {
        public override void Run()
        {
            WfAdjunct focusAdjunct = base.ae.GetFocusAdjunct();
            if ((focusAdjunct != null) && (MessageHelper.ShowOkCancelInfo(ResourceService.GetString("Global.Message.Delete")) == DialogResult.OK))
            {
                base.ae.CurrentUnitOfWork.RegisterRemoved(focusAdjunct);
                base.ae.CurrentUnitOfWork.Remove(focusAdjunct.File);
                base.ae.RemoveAdjunct(focusAdjunct);
            }
        }

        public override bool IsEnabled
        {
            get
            {
                WfAdjunct focusAdjunct = base.ae.GetFocusAdjunct();
                return ((focusAdjunct != null) && base.IsHaveEditAdjuctPermission(focusAdjunct));
            }
            set
            {
            }
        }
    }
}

