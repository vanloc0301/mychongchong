namespace SkyMap.Net.Core
{
    using System;

    public abstract class AbstractTextEditCommand : AbstractCommand, ITextEditCommand, ICommand
    {
        private bool isEnabled = true;

        protected AbstractTextEditCommand()
        {
        }

        public virtual bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
            }
        }
    }
}

