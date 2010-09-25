namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Dialogs;
    using System;

    public class EditPasswordCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            EditPassword password = new EditPassword();
            password.ShowDialog();
            password.Close();
        }
    }
}

