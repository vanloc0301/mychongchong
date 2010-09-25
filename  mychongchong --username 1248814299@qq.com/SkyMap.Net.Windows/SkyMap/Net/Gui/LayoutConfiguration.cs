namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class LayoutConfiguration
    {
        private static readonly string configFile = "LayoutConfig.xml";
        public static string[] DefaultLayouts = new string[] { "Default", "Debug", "Plain" };
        private string displayName;
        private string fileName;
        public static List<LayoutConfiguration> Layouts = new List<LayoutConfiguration>();
        private string name;
        private bool readOnly;

        public static  event EventHandler LayoutChanged;

        static LayoutConfiguration()
        {
            LoadLayoutConfiguration();
        }

        public LayoutConfiguration()
        {
            this.displayName = null;
        }

        private LayoutConfiguration(XmlElement el)
        {
            this.displayName = null;
            this.name = el.GetAttribute("name");
            this.fileName = el.GetAttribute("file");
            this.readOnly = bool.Parse(el.GetAttribute("readonly"));
        }

        public static LayoutConfiguration GetLayout(string name)
        {
            foreach (LayoutConfiguration configuration in Layouts)
            {
                if (configuration.Name == name)
                {
                    return configuration;
                }
            }
            return null;
        }

        private static void LoadLayoutConfiguration()
        {
            string str = Path.Combine(PropertyService.ConfigDirectory, "layouts");
            if (File.Exists(Path.Combine(str, configFile)))
            {
                LoadLayoutConfiguration(Path.Combine(str, configFile));
            }
            LoadLayoutConfiguration(Path.Combine(Path.Combine(PropertyService.DataDirectory, "resources" + Path.DirectorySeparatorChar + "layouts"), configFile));
        }

        private static void LoadLayoutConfiguration(string layoutConfig)
        {
            XmlDocument document = new XmlDocument();
            document.Load(layoutConfig);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                Layouts.Add(new LayoutConfiguration(element));
            }
        }

        protected static void OnLayoutChanged(EventArgs e)
        {
            if (LayoutChanged != null)
            {
                LayoutChanged(null, e);
            }
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        public static LayoutConfiguration CurrentLayout
        {
            get
            {
                foreach (LayoutConfiguration configuration in Layouts)
                {
                    if (configuration.name == CurrentLayoutName)
                    {
                        return configuration;
                    }
                }
                return null;
            }
        }

        public static string CurrentLayoutFileName
        {
            get
            {
                string str = Path.Combine(PropertyService.ConfigDirectory, "layouts");
                LayoutConfiguration currentLayout = CurrentLayout;
                if (currentLayout != null)
                {
                    return Path.Combine(str, currentLayout.FileName);
                }
                return null;
            }
        }

        public static string CurrentLayoutName
        {
            get
            {
                return PropertyService.Get<string>("Workbench.CurrentLayout", "Default");
            }
            set
            {
                if (WorkbenchSingleton.InvokeRequired)
                {
                    throw new InvalidOperationException("Invoke required");
                }
                if (value != CurrentLayoutName)
                {
                    PropertyService.Set<string>("Workbench.CurrentLayout", value);
                    WorkbenchSingleton.Workbench.WorkbenchLayout.LoadConfiguration();
                    OnLayoutChanged(EventArgs.Empty);
                }
            }
        }

        public static string CurrentLayoutTemplateFileName
        {
            get
            {
                string str = Path.Combine(PropertyService.DataDirectory, "resources" + Path.DirectorySeparatorChar + "layouts");
                LayoutConfiguration currentLayout = CurrentLayout;
                if (currentLayout != null)
                {
                    return Path.Combine(str, currentLayout.FileName);
                }
                return null;
            }
        }

        public string DisplayName
        {
            get
            {
                return ((this.displayName == null) ? this.Name : this.displayName);
            }
            set
            {
                this.displayName = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                this.readOnly = value;
            }
        }
    }
}

