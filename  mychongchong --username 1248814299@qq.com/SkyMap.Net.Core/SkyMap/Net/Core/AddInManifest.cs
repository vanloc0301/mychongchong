namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class AddInManifest
    {
        private List<AddInReference> conflicts = new List<AddInReference>();
        private List<AddInReference> dependencies = new List<AddInReference>();
        private Dictionary<string, Version> identities = new Dictionary<string, Version>();
        private string primaryIdentity;
        private Version primaryVersion;

        private void AddIdentity(string name, string version, string hintPath)
        {
            if (name.Length == 0)
            {
                throw new AddInLoadException("Identity needs a name");
            }
            foreach (char ch in name)
            {
                if ((!char.IsLetterOrDigit(ch) && (ch != '.')) && (ch != '_'))
                {
                    throw new AddInLoadException("Identity name contains invalid character: '" + ch + "'");
                }
            }
            Version version2 = AddInReference.ParseVersion(version, hintPath);
            if (this.primaryVersion == null)
            {
                this.primaryVersion = version2;
            }
            if (this.primaryIdentity == null)
            {
                this.primaryIdentity = name;
            }
            this.identities.Add(name, version2);
        }

        public void ReadManifestSection(XmlReader reader, string hintPath)
        {
            if (reader.AttributeCount != 0)
            {
                throw new AddInLoadException("Manifest node cannot have attributes.");
            }
            if (reader.IsEmptyElement)
            {
                throw new AddInLoadException("Manifest node cannot be empty.");
            }
            while (reader.Read())
            {
                string localName;
                Properties properties;
                XmlNodeType nodeType = reader.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if ((nodeType == XmlNodeType.EndElement) && (reader.LocalName == "Manifest"))
                    {
                        break;
                    }
                }
                else
                {
                    localName = reader.LocalName;
                    properties = Properties.ReadFromAttributes(reader);
                    string str2 = localName;
                    if (str2 == null)
                    {
                        goto Label_00FB;
                    }
                    if (!(str2 == "Identity"))
                    {
                        if (str2 == "Dependency")
                        {
                            goto Label_00D1;
                        }
                        if (str2 == "Conflict")
                        {
                            goto Label_00E6;
                        }
                        goto Label_00FB;
                    }
                    this.AddIdentity(properties["name"], properties["version"], hintPath);
                }
                goto Label_010E;
            Label_00D1:
                this.dependencies.Add(AddInReference.Create(properties, hintPath));
                goto Label_010E;
            Label_00E6:
                this.conflicts.Add(AddInReference.Create(properties, hintPath));
                goto Label_010E;
            Label_00FB:
                throw new AddInLoadException("Unknown node in Manifest section:" + localName);
            Label_010E:;
            }
        }

        public List<AddInReference> Conflicts
        {
            get
            {
                return this.conflicts;
            }
        }

        public List<AddInReference> Dependencies
        {
            get
            {
                return this.dependencies;
            }
        }

        public Dictionary<string, Version> Identities
        {
            get
            {
                return this.identities;
            }
        }

        public string PrimaryIdentity
        {
            get
            {
                return this.primaryIdentity;
            }
        }

        public Version PrimaryVersion
        {
            get
            {
                return this.primaryVersion;
            }
        }
    }
}

