namespace SkyMap.Net.Core
{
    using System;

    public abstract class AbstractDateTimeEditCommand : AbstractMenuCommand
    {
        private DateTime? editValue;

        protected AbstractDateTimeEditCommand()
        {
        }

        public DateTime? EditValue
        {
            get
            {
                return this.editValue;
            }
            set
            {
                if (value != this.editValue)
                {
                    this.editValue = value;
                }
            }
        }
    }
}

