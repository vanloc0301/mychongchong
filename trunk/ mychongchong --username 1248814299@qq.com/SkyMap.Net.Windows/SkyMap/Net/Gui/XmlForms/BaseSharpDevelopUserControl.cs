namespace SkyMap.Net.Gui.XmlForms
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public abstract class BaseSharpDevelopUserControl : XmlUserControl
    {
        public void SetEnabledStatus(bool enabled, params string[] controlNames)
        {
            foreach (string str in controlNames)
            {
                Control control = base.ControlDictionary[str];
                if (control == null)
                {
                    MessageService.ShowError(str + " not found!");
                }
                else
                {
                    control.Enabled = enabled;
                }
            }
        }

        protected override void SetupXmlLoader()
        {
            base.xmlLoader.StringValueFilter = new SharpDevelopStringValueFilter();
            base.xmlLoader.PropertyValueCreator = new SharpDevelopPropertyValueCreator();
        }
    }
}

