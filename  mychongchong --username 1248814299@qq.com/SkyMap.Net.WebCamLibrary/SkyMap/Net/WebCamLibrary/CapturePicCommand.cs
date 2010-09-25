namespace SkyMap.Net.WebCamLibrary
{
    using SkyMap.Net.Core;
    using System;

    public abstract class CapturePicCommand : AbstractMenuCommand
    {
        protected CapturePicCommand()
        {
        }

        public override void Run()
        {
            LiveShow owner = this.Owner as LiveShow;
            if (owner != null)
            {
                owner.TakeAPicture(this.FilePre);
            }
        }

        protected abstract string FilePre { get; }
    }
}

