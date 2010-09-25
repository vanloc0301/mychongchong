namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public class CommonTextBoxCommand : AbstractTextEditCommand
    {
        private static Dictionary<string, string> properties;

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            this.Run();
        }

        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("set Properties[{0}]='{1}'", new object[] { base.ID, (this.Owner as ToolBarTextBox).TextBox.Text });
            }
            Properties[base.ID] = (this.Owner as ToolBarTextBox).TextBox.Text;
        }

        public static Dictionary<string, string> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, string>();
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

