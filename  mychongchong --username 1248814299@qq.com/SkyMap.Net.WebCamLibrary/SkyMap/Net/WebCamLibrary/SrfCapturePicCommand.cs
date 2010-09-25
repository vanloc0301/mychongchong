namespace SkyMap.Net.WebCamLibrary
{
    using System;

    public class SrfCapturePicCommand : CapturePicCommand
    {
        protected override string FilePre
        {
            get
            {
                return "受让方";
            }
        }
    }
}

