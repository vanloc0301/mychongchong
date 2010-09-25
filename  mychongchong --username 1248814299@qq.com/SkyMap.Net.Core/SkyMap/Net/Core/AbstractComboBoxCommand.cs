namespace SkyMap.Net.Core
{
    using System;

    public abstract class AbstractComboBoxCommand : AbstractCommand, IComboBoxCommand, ICommand
    {
        private bool isEnabled = true;

        protected AbstractComboBoxCommand()
        {
        }

        public override void Run()
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

