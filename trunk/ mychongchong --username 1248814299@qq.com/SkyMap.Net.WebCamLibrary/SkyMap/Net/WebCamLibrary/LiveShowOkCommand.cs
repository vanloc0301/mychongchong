namespace SkyMap.Net.WebCamLibrary
{
    using SkyMap.Net.Core;
    using System;

    public class LiveShowOkCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            LiveShow owner = this.Owner as LiveShow;
            if (owner != null)
            {
                owner.DialogOkClose();
            }
        }
    }
}

