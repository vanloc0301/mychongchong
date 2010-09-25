namespace SkyMap.Net.Gui
{
    using System;

    internal class OffDisplayException : Exception
    {
        public OffDisplayException() : base("off screen")
        {
        }
    }
}

