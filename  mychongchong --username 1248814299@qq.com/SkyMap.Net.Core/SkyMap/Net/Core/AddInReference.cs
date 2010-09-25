namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class AddInReference : ICloneable
    {
        private static Version entryVersion;
        private Version maximumVersion;
        private Version minimumVersion;
        private string name;

        public AddInReference(string name) : this(name, new Version(0, 0, 0, 0), new Version(0x7fffffff, 0x7fffffff))
        {
        }

        public AddInReference(string name, Version specificVersion) : this(name, specificVersion, specificVersion)
        {
        }

        public AddInReference(string name, Version minimumVersion, Version maximumVersion)
        {
            this.Name = name;
            if (minimumVersion == null)
            {
                throw new ArgumentNullException("minimumVersion");
            }
            if (maximumVersion == null)
            {
                throw new ArgumentNullException("maximumVersion");
            }
            this.minimumVersion = minimumVersion;
            this.maximumVersion = maximumVersion;
        }

        public bool Check(Dictionary<string, Version> addIns, out Version versionFound)
        {
            return (addIns.TryGetValue(this.name, out versionFound) && ((this.CompareVersion(versionFound, this.minimumVersion) >= 0) && (this.CompareVersion(versionFound, this.maximumVersion) <= 0)));
        }

        public AddInReference Clone()
        {
            return new AddInReference(this.name, this.minimumVersion, this.maximumVersion);
        }

        private int CompareVersion(Version a, Version b)
        {
            if (a.Major != b.Major)
            {
                return ((a.Major > b.Major) ? 1 : -1);
            }
            if (a.Minor != b.Minor)
            {
                return ((a.Minor > b.Minor) ? 1 : -1);
            }
            if ((a.Build >= 0) && (b.Build >= 0))
            {
                if (a.Build != b.Build)
                {
                    return ((a.Build > b.Build) ? 1 : -1);
                }
                if ((a.Revision < 0) || (b.Revision < 0))
                {
                    return 0;
                }
                if (a.Revision != b.Revision)
                {
                    return ((a.Revision > b.Revision) ? 1 : -1);
                }
            }
            return 0;
        }

        public static AddInReference Create(Properties properties, string hintPath)
        {
            AddInReference reference = new AddInReference(properties["addin"]);
            string version = properties["version"];
            if ((version != null) && (version.Length > 0))
            {
                int index = version.IndexOf('-');
                if (index > 0)
                {
                    reference.minimumVersion = ParseVersion(version.Substring(0, index), hintPath);
                    reference.maximumVersion = ParseVersion(version.Substring(index + 1), hintPath);
                    return reference;
                }
                reference.maximumVersion = reference.minimumVersion = ParseVersion(version, hintPath);
            }
            return reference;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AddInReference))
            {
                return false;
            }
            AddInReference reference = (AddInReference) obj;
            return (((this.name == reference.name) && (this.minimumVersion == reference.minimumVersion)) && (this.maximumVersion == reference.maximumVersion));
        }

        public override int GetHashCode()
        {
            return ((this.name.GetHashCode() ^ this.minimumVersion.GetHashCode()) ^ this.maximumVersion.GetHashCode());
        }

        internal static Version ParseVersion(string version, string hintPath)
        {
            if ((version == null) || (version.Length == 0))
            {
                return new Version(0, 0, 0, 0);
            }
            if (version.StartsWith("@"))
            {
                if (version == "@EntryAssemblyVersion")
                {
                    if (entryVersion == null)
                    {
                        entryVersion = Assembly.GetEntryAssembly().GetName().Version;
                    }
                    return entryVersion;
                }
                if (hintPath != null)
                {
                    string fileName = Path.Combine(hintPath, version.Substring(1));
                    try
                    {
                        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
                        return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
                    }
                    catch (FileNotFoundException exception)
                    {
                        throw new AddInLoadException("Cannot get version '" + version + "': " + exception.Message);
                    }
                }
                return new Version(0, 0, 0, 0);
            }
            return new Version(version);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public override string ToString()
        {
            if (this.minimumVersion.ToString() == "0.0.0.0")
            {
                if (this.maximumVersion.Major == 0x7fffffff)
                {
                    return this.name;
                }
                return (this.name + ", version <" + this.maximumVersion.ToString());
            }
            if (this.maximumVersion.Major == 0x7fffffff)
            {
                return (this.name + ", version >" + this.minimumVersion.ToString());
            }
            if (this.minimumVersion == this.maximumVersion)
            {
                return (this.name + ", version " + this.minimumVersion.ToString());
            }
            return (this.name + ", version " + this.minimumVersion.ToString() + "-" + this.maximumVersion.ToString());
        }

        public Version MaximumVersion
        {
            get
            {
                return this.maximumVersion;
            }
        }

        public Version MinimumVersion
        {
            get
            {
                return this.minimumVersion;
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
                if (value == null)
                {
                    throw new ArgumentNullException("name");
                }
                if (value.Length == 0)
                {
                    throw new ArgumentException("name cannot be an empty string", "name");
                }
                this.name = value;
            }
        }
    }
}

