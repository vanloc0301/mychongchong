namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;

    public class WindowsWaiUI : WaitUI
    {
        public override void Close()
        {
            WaitDialogHelper.Close();
        }

        public override void Show()
        {
            WaitDialogHelper.Show();
        }
    }
}

