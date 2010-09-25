namespace SkyMap.Net.WebCamLibrary
{
    using System;

    public class ZrfCapturePicCommand : CapturePicCommand
    {
        protected override string FilePre
        {
            get
            {
                return "转让方";
            }
        }
    }
}

