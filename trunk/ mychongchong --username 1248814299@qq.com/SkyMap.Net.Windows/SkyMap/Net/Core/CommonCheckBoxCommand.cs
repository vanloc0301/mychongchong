namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public class CommonCheckBoxCommand : AbstractSpinEditCommand
    {
        private static Dictionary<string, bool> properties;

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.Run();
        }

        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("set Properties[{0}]='{1}'", new object[] { base.ID, (this.Owner as ToolBarCheckBox).Checked });
            }
            Properties[base.ID] = (this.Owner as ToolBarCheckBox).Checked;
        }

        public static Dictionary<string, bool> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, bool>();
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

