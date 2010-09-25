namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public class CommonDateTimeCommand : AbstractDateTimeEditCommand
    {
        private static Dictionary<string, DateTime> properties;

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.Run();
        }

        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("set Properties[{0}]='{1}'", new object[] { base.ID, Convert.ToDateTime((this.Owner as ToolBarDateTimeEdit).EditValue) });
            }
            Properties[base.ID] = Convert.ToDateTime((this.Owner as ToolBarDateTimeEdit).EditValue);
        }

        public static Dictionary<string, DateTime> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, DateTime>();
                }
                return properties;
            }
            set
            {
                properties = value;
            }
        }
    }
}

