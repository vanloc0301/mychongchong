namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    public sealed class AddIn
    {
        private AddInAction action = AddInAction.Disable;
        private string addInFileName = null;
        private List<string> bitmapResources = new List<string>();
        private bool enabled;
        private static bool hasShownErrorMessage = false;
        private AddInManifest manifest = new AddInManifest();
        private Dictionary<string, ExtensionPath> paths = new Dictionary<string, ExtensionPath>();
        private SkyMap.Net.Core.Properties properties = new SkyMap.Net.Core.Properties();
        private List<Runtime> runtimes = new List<Runtime>();
        private List<string> stringResources = new List<string>();

        private AddIn()
        {
        }

        public object CreateObject(string className)
        {
            foreach (Runtime runtime in this.runtimes)
            {
                object obj2 = runtime.CreateInstance(className);
                if (obj2 != null)
                {
                    return obj2;
                }
            }
            if (hasShownErrorMessage)
            {
                LoggingService.Error("Cannot create object: " + className);
            }
            else
            {
                hasShownErrorMessage = true;
                MessageService.ShowError("Cannot create object: " + className + "\nFuture missing objects will not cause an error message.");
            }
            return null;
        }

        public ExtensionPath GetExtensionPath(string pathName)
        {
            if (!this.paths.ContainsKey(pathName))
            {
                return (this.paths[pathName] = new ExtensionPath(pathName, this));
            }
            return this.paths[pathName];
        }

        public static AddIn Load(TextReader textReader)
        {
            return Load(textReader, null);
        }

        public static AddIn Load(string fileName)
        {
            AddIn in2;
            try
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    AddIn @in = Load(reader, Path.GetDirectoryName(fileName));
                    @in.addInFileName = fileName;
                    in2 = @in;
                }
            }
            catch (Exception exception)
            {
                throw new AddInLoadException(string.Concat(new object[] { "Can't load ", fileName, " ", exception }));
            }
            return in2;
        }

        public static AddIn Load(TextReader textReader, string hintPath)
        {
            AddIn addIn = new AddIn();
            using (XmlTextReader reader = new XmlTextReader(textReader))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        string localName = reader.LocalName;
                        if ((localName == null) || !(localName == "AddIn"))
                        {
                            throw new AddInLoadException("Unknown add-in file.");
                        }
                        addIn.properties = SkyMap.Net.Core.Properties.ReadFromAttributes(reader);
                        SetupAddIn(reader, addIn, hintPath);
                    }
                }
            }
            return addIn;
        }

        private static void SetupAddIn(XmlTextReader reader, AddIn addIn, string hintPath)
        {
            while (reader.Read())
            {
                string str3;
                string localName;
                if ((reader.NodeType == XmlNodeType.Element) && reader.IsStartElement())
                {
                    localName = reader.LocalName;
                    switch (localName)
                    {
                        case "StringResources":
                        case "BitmapResources":
                            if (reader.AttributeCount != 1)
                            {
                                throw new AddInLoadException("BitmapResources requires ONE attribute.");
                            }
                            break;

                        case "Runtime":
                            if (reader.IsEmptyElement)
                            {
                                goto Label_02E9;
                            }
                            goto Label_01DB;

                        case "Include":
                            if (reader.AttributeCount != 1)
                            {
                                throw new AddInLoadException("Include requires ONE attribute.");
                            }
                            goto Label_020B;

                        case "Path":
                            if (reader.AttributeCount != 1)
                            {
                                throw new AddInLoadException("Import node requires ONE attribute.");
                            }
                            goto Label_0293;

                        case "Manifest":
                            addIn.Manifest.ReadManifestSection(reader, hintPath);
                            goto Label_02E9;

                        default:
                            throw new AddInLoadException("Unknown root path node:" + reader.LocalName);
                    }
                    string item = SkyMap.Net.Core.StringParser.Parse(reader.GetAttribute("file"));
                    if (reader.LocalName == "BitmapResources")
                    {
                        addIn.BitmapResources.Add(item);
                    }
                    else
                    {
                        addIn.StringResources.Add(item);
                    }
                }
                goto Label_02E9;
            Label_014B:
                if ((reader.NodeType == XmlNodeType.EndElement) && (reader.LocalName == "Runtime"))
                {
                    goto Label_02E9;
                }
                if ((reader.NodeType == XmlNodeType.Element) && reader.IsStartElement())
                {
                    localName = reader.LocalName;
                    if ((localName == null) || !(localName == "Import"))
                    {
                        throw new AddInLoadException("Unknown node in runtime section :" + reader.LocalName);
                    }
                    addIn.runtimes.Add(Runtime.Read(addIn, reader, hintPath));
                }
            Label_01DB:
                if (reader.Read())
                {
                    goto Label_014B;
                }
                goto Label_02E9;
            Label_020B:
                if (!reader.IsEmptyElement)
                {
                    throw new AddInLoadException("Include nodes must be empty!");
                }
                if (hintPath == null)
                {
                    throw new AddInLoadException("Cannot use include nodes when hintPath was not specified (e.g. when AddInManager reads a .addin file)!");
                }
                string url = Path.Combine(hintPath, reader.GetAttribute(0));
                using (XmlTextReader reader2 = new XmlTextReader(url))
                {
                    SetupAddIn(reader2, addIn, Path.GetDirectoryName(url));
                }
                goto Label_02E9;
            Label_0293:
                str3 = reader.GetAttribute(0);
                ExtensionPath extensionPath = addIn.GetExtensionPath(str3);
                if (!reader.IsEmptyElement)
                {
                    ExtensionPath.SetUp(extensionPath, reader, "Path");
                }
            Label_02E9:;
            }
        }

        public override string ToString()
        {
            return ("[AddIn: " + this.Name + "]");
        }

        public AddInAction Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }

        public List<string> BitmapResources
        {
            get
            {
                return this.bitmapResources;
            }
            set
            {
                this.bitmapResources = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            internal set
            {
                this.enabled = value;
                this.Action = value ? AddInAction.Enable : AddInAction.Disable;
            }
        }

        public string FileName
        {
            get
            {
                return this.addInFileName;
            }
        }

        public AddInManifest Manifest
        {
            get
            {
                return this.manifest;
            }
        }

        public string Name
        {
            get
            {
                return this.properties["name"];
            }
        }

        public Dictionary<string, ExtensionPath> Paths
        {
            get
            {
                return this.paths;
            }
        }

        public SkyMap.Net.Core.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }

        public List<Runtime> Runtimes
        {
            get
            {
                return this.runtimes;
            }
        }

        public List<string> StringResources
        {
            get
            {
                return this.stringResources;
            }
            set
            {
                this.stringResources = value;
            }
        }

        public System.Version Version
        {
            get
            {
                return this.manifest.PrimaryVersion;
            }
        }
    }
}

