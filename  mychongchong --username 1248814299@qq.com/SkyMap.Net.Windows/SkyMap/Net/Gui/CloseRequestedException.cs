namespace SkyMap.Net.Gui
{
    using System;

    internal class CloseRequestedException : Exception
    {
        public CloseRequestedException() : base("Close requested")
        {
        }
    }
}

