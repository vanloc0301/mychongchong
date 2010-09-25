namespace SkyMap.Net.Core
{
    using System;

    public abstract class AbstractMenuCommand : AbstractCommand, IMenuCommand, ICommand
    {
        private bool isEnabled = true;
        private bool visible = true;

        protected AbstractMenuCommand()
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

        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }
    }
}

