namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public static class ResourceService
    {
        private static Dictionary<string, Bitmap> bitmapCache = new Dictionary<string, Bitmap>();
        private static Font courierNew10;
        private static string currentLanguage;
        private static Dictionary<string, Icon> iconCache = new Dictionary<string, Icon>();
        private static List<ResourceManager> icons = new List<ResourceManager>();
        private const string imageResources = "BitmapResources";
        private static Hashtable localIcons = null;
        private static List<ResourceManager> localIconsResMgrs = new List<ResourceManager>();
        private static Hashtable localStrings = null;
        private static List<ResourceManager> localStringsResMgrs = new List<ResourceManager>();
        private static List<ResourceAssembly> resourceAssemblies = new List<ResourceAssembly>();
        private static string resourceDirectory;
        private const string stringResources = "StringResources";
        private static List<ResourceManager> strings = new List<ResourceManager>();
        private const string uiLanguageProperty = "CoreProperties.UILanguage";

        public static  event EventHandler LanguageChanged;

        public static Bitmap GetBitmap(string name)
        {
            lock (bitmapCache)
            {
                Bitmap imageResource;
                if (!bitmapCache.TryGetValue(name, out imageResource))
                {
                    imageResource = (Bitmap) GetImageResource(name);
                    bitmapCache[name] = imageResource;
                }
                return imageResource;
            }
        }

        public static Icon GetIcon(string name)
        {
            lock (iconCache)
            {
                Icon icon;
                if (!iconCache.TryGetValue(name, out icon))
                {
                    object imageResource = GetImageResource(name);
                    if (imageResource == null)
                    {
                        return null;
                    }
                    if (imageResource is Icon)
                    {
                        icon = (Icon) imageResource;
                    }
                    else
                    {
                        icon = Icon.FromHandle(((Bitmap) imageResource).GetHicon());
                    }
                    iconCache[name] = icon;
                }
                return icon;
            }
        }

        private static object GetImageResource(string name)
        {
            object obj2 = null;
            if ((localIcons != null) && (localIcons[name] != null))
            {
                obj2 = localIcons[name];
            }
            else
            {
                foreach (ResourceManager manager in localIconsResMgrs)
                {
                    obj2 = manager.GetObject(name);
                    if (obj2 != null)
                    {
                        break;
                    }
                }
                if (obj2 == null)
                {
                    foreach (ResourceManager manager in icons)
                    {
                        try
                        {
                            obj2 = manager.GetObject(name);
                        }
                        catch (Exception)
                        {
                        }
                        if (obj2 != null)
                        {
                            break;
                        }
                    }
                }
            }
            if (obj2 == null)
            {
                LoggingService.DebugFormatted("没有图标资源：{0}", new object[] { name });
            }
            return obj2;
        }

        public static string GetString(string name)
        {
            if ((localStrings != null) && (localStrings[name] != null))
            {
                return localStrings[name].ToString();
            }
            string str = null;
            foreach (ResourceManager manager in localStringsResMgrs)
            {
                try
                {
                    str = manager.GetString(name);
                }
                catch (Exception)
                {
                }
                if (str != null)
                {
                    break;
                }
            }
            if (str == null)
            {
                foreach (ResourceManager manager in strings)
                {
                    try
                    {
                        str = manager.GetString(name);
                    }
                    catch (Exception)
                    {
                    }
                    if (str != null)
                    {
                        break;
                    }
                }
            }
            if (str == null)
            {
                LoggingService.DebugFormatted("没找到字符串资源：{0}", new object[] { name });
                throw new ResourceNotFoundException("string >" + name + "<");
            }
            return str;
        }

        public static void InitializeService(string resourceDirectory)
        {
            if (ResourceService.resourceDirectory != null)
            {
                throw new InvalidOperationException("Service is already initialized.");
            }
            if (resourceDirectory == null)
            {
                throw new ArgumentNullException("resourceDirectory");
            }
            ResourceService.resourceDirectory = resourceDirectory;
            PropertyService.PropertyChanged += new PropertyChangedEventHandler(ResourceService.OnPropertyChange);
            LoadLanguageResources(Language);
        }

        private static Hashtable Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                Hashtable hashtable = new Hashtable();
                ResourceReader reader = new ResourceReader(fileName);
                foreach (DictionaryEntry entry in reader)
                {
                    hashtable.Add(entry.Key, entry.Value);
                }
                reader.Close();
                return hashtable;
            }
            return null;
        }

        private static Hashtable Load(string name, string language)
        {
            return Load(string.Concat(new object[] { resourceDirectory, Path.DirectorySeparatorChar, name, ".", language, ".resources" }));
        }

        public static Font LoadFont(string fontName, int size)
        {
            return LoadFont(fontName, size, FontStyle.Regular);
        }

        public static Font LoadFont(string fontName, int size, FontStyle style)
        {
            try
            {
                return new Font(fontName, (float) size, style);
            }
            catch (Exception exception)
            {
                LoggingService.Warn(exception);
                return SystemInformation.MenuFont;
            }
        }

        public static Font LoadFont(string fontName, int size, GraphicsUnit unit)
        {
            return LoadFont(fontName, size, FontStyle.Regular, unit);
        }

        public static Font LoadFont(string fontName, int size, FontStyle style, GraphicsUnit unit)
        {
            try
            {
                return new Font(fontName, (float) size, style, unit);
            }
            catch (Exception exception)
            {
                LoggingService.Warn(exception);
                return SystemInformation.MenuFont;
            }
        }

        private static void LoadLanguageResources(string language)
        {
            iconCache.Clear();
            bitmapCache.Clear();
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            catch (Exception)
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(language.Split(new char[] { '-' })[0]);
                }
                catch (Exception)
                {
                }
            }
            localStrings = Load("StringResources", language);
            if ((localStrings == null) && (language.IndexOf('-') > 0))
            {
                localStrings = Load("StringResources", language.Split(new char[] { '-' })[0]);
            }
            localIcons = Load("BitmapResources", language);
            if ((localIcons == null) && (language.IndexOf('-') > 0))
            {
                localIcons = Load("BitmapResources", language.Split(new char[] { '-' })[0]);
            }
            localStringsResMgrs.Clear();
            localIconsResMgrs.Clear();
            currentLanguage = language;
            foreach (ResourceAssembly assembly in resourceAssemblies)
            {
                assembly.Load();
            }
        }

        private static void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if ((e.Key == "CoreProperties.UILanguage") && (e.NewValue != e.OldValue))
            {
                LoadLanguageResources((string) e.NewValue);
                if (LanguageChanged != null)
                {
                    LanguageChanged(null, e);
                }
            }
        }

        public static void RegisterImages(string baseResourceName, Assembly assembly)
        {
            RegisterNeutralImages(new ResourceManager(baseResourceName, assembly));
            ResourceAssembly item = new ResourceAssembly(assembly, baseResourceName, true);
            resourceAssemblies.Add(item);
            item.Load();
        }

        public static void RegisterNeutralImages(ResourceManager imageManager)
        {
            icons.Add(imageManager);
        }

        public static void RegisterNeutralStrings(ResourceManager stringManager)
        {
            strings.Add(stringManager);
        }

        public static void RegisterStrings(string baseResourceName, Assembly assembly)
        {
            RegisterNeutralStrings(new ResourceManager(baseResourceName, assembly));
            ResourceAssembly item = new ResourceAssembly(assembly, baseResourceName, false);
            resourceAssemblies.Add(item);
            item.Load();
        }

        public static Font CourierNew10
        {
            get
            {
                if (courierNew10 == null)
                {
                    courierNew10 = LoadFont("Courier New", 10);
                }
                return courierNew10;
            }
        }

        public static string Language
        {
            get
            {
                return PropertyService.Get<string>("CoreProperties.UILanguage", Thread.CurrentThread.CurrentUICulture.Name);
            }
            set
            {
                PropertyService.Set<string>("CoreProperties.UILanguage", value);
            }
        }

        private class ResourceAssembly
        {
            private Assembly assembly;
            private string baseResourceName;
            private bool isIcons;

            public ResourceAssembly(Assembly assembly, string baseResourceName, bool isIcons)
            {
                this.assembly = assembly;
                this.baseResourceName = baseResourceName;
                this.isIcons = isIcons;
            }

            public void Load()
            {
                string str = "Loading resources " + this.baseResourceName + "." + ResourceService.currentLanguage + ": ";
                ResourceManager item = null;
                if (this.assembly.GetManifestResourceInfo(this.baseResourceName + "." + ResourceService.currentLanguage + ".resources") != null)
                {
                    LoggingService.Info(str + " loading from main assembly");
                    item = new ResourceManager(this.baseResourceName + "." + ResourceService.currentLanguage, this.assembly);
                }
                else if ((ResourceService.currentLanguage.IndexOf('-') > 0) && (this.assembly.GetManifestResourceInfo(this.baseResourceName + "." + ResourceService.currentLanguage.Split(new char[] { '-' })[0] + ".resources") != null))
                {
                    LoggingService.Info(str + " loading from main assembly (no country match)");
                    item = new ResourceManager(this.baseResourceName + "." + ResourceService.currentLanguage.Split(new char[] { '-' })[0], this.assembly);
                }
                else
                {
                    item = this.TrySatellite(ResourceService.currentLanguage);
                    if ((item == null) && (ResourceService.currentLanguage.IndexOf('-') > 0))
                    {
                        item = this.TrySatellite(ResourceService.currentLanguage.Split(new char[] { '-' })[0]);
                    }
                }
                if (item == null)
                {
                    LoggingService.Warn(str + "NOT FOUND");
                }
                else if (this.isIcons)
                {
                    ResourceService.localIconsResMgrs.Add(item);
                }
                else
                {
                    ResourceService.localStringsResMgrs.Add(item);
                }
            }

            private ResourceManager TrySatellite(string language)
            {
                string str = Path.GetFileNameWithoutExtension(this.assembly.Location) + ".resources.dll";
                str = Path.Combine(Path.Combine(Path.GetDirectoryName(this.assembly.Location), language), str);
                if (File.Exists(str))
                {
                    LoggingService.Info("Loging resources " + this.baseResourceName + " loading from satellite " + language);
                    return new ResourceManager(this.baseResourceName, Assembly.LoadFrom(str));
                }
                return null;
            }
        }
    }
}

