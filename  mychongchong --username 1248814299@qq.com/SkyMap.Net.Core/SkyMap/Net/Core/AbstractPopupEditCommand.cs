namespace SkyMap.Net.Core
{
    using System;

    public abstract class AbstractPopupEditCommand : AbstractCommand, IPopupEditCommand, ICommand
    {
        private bool isEnabled = true;

        protected AbstractPopupEditCommand()
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

