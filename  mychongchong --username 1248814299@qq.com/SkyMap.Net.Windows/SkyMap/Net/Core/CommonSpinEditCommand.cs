namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public class CommonSpinEditCommand : AbstractSpinEditCommand
    {
        private static Dictionary<string, int> properties;

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.Run();
        }

        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("set Properties[{0}]='{1}'", new object[] { base.ID, Convert.ToInt32((this.Owner as ToolBarSpinEdit).EditValue) });
            }
            Properties[base.ID] = Convert.ToInt32((this.Owner as ToolBarSpinEdit).EditValue);
        }

        public static Dictionary<string, int> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, int>();
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

