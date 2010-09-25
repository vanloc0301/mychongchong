namespace SkyMap.Net.Gui.XmlForms
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public abstract class XmlForm : Form
    {
        protected XmlLoader xmlLoader;

        public T Get<T>(string name) where T: Control
        {
            return this.xmlLoader.Get<T>(name);
        }

        protected void SetupFromXmlResource(string resourceName)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            resourceName = "Resources." + resourceName;
            this.SetupFromXmlStream(callingAssembly.GetManifestResourceStream(resourceName));
        }

        protected void SetupFromXmlStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            base.SuspendLayout();
            this.xmlLoader = new XmlLoader();
            this.SetupXmlLoader();
            if (stream != null)
            {
                this.xmlLoader.LoadObjectFromStream(this, stream);
            }
            RightToLeftConverter.ConvertRecursive(this);
            base.ResumeLayout(false);
        }

        protected virtual void SetupXmlLoader()
        {
        }

        public Dictionary<string, Control> ControlDictionary
        {
            get
            {
                return this.xmlLoader.ControlDictionary;
            }
        }
    }
}

