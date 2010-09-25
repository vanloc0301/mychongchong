namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    public class Runtime
    {
        private string assembly;
        private List<Properties> definedConditionEvaluators = new List<Properties>(1);
        private List<Properties> definedDoozers = new List<Properties>(1);
        private string hintPath;
        private bool isAssemblyLoaded;
        private System.Reflection.Assembly loadedAssembly = null;

        public Runtime(string assembly, string hintPath)
        {
            this.assembly = assembly;
            this.hintPath = hintPath;
        }

        public object CreateInstance(string instance)
        {
            System.Reflection.Assembly loadedAssembly = this.LoadedAssembly;
            if (loadedAssembly == null)
            {
                return null;
            }
            return loadedAssembly.CreateInstance(instance);
        }

        internal static Runtime Read(AddIn addIn, XmlTextReader reader, string hintPath)
        {
            if (reader.AttributeCount != 1)
            {
                throw new AddInLoadException("Import node requires ONE attribute.");
            }
            Runtime runtime = new Runtime(reader.GetAttribute(0), hintPath);
            if (!reader.IsEmptyElement)
            {
                while (reader.Read())
                {
                    string localName;
                    Properties properties;
                    XmlNodeType nodeType = reader.NodeType;
                    if (nodeType != XmlNodeType.Element)
                    {
                        if ((nodeType == XmlNodeType.EndElement) && (reader.LocalName == "Import"))
                        {
                            return runtime;
                        }
                    }
                    else
                    {
                        localName = reader.LocalName;
                        properties = Properties.ReadFromAttributes(reader);
                        string str2 = localName;
                        if (str2 == null)
                        {
                            goto Label_01A2;
                        }
                        if (!(str2 == "Doozer"))
                        {
                            if (str2 == "ConditionEvaluator")
                            {
                                goto Label_012C;
                            }
                            goto Label_01A2;
                        }
                        if (!reader.IsEmptyElement)
                        {
                            throw new AddInLoadException("Doozer nodes must be empty!");
                        }
                        LazyLoadDoozer doozer = new LazyLoadDoozer(addIn, properties);
                        if (AddInTree.Doozers.ContainsKey(doozer.Name))
                        {
                            throw new AddInLoadException("Duplicate doozer: " + doozer.Name);
                        }
                        AddInTree.Doozers.Add(doozer.Name, doozer);
                        runtime.definedDoozers.Add(properties);
                    }
                    goto Label_01B5;
                Label_012C:
                    if (!reader.IsEmptyElement)
                    {
                        throw new AddInLoadException("ConditionEvaluator nodes must be empty!");
                    }
                    LazyConditionEvaluator evaluator = new LazyConditionEvaluator(addIn, properties);
                    if (AddInTree.ConditionEvaluators.ContainsKey(evaluator.Name))
                    {
                        throw new AddInLoadException("Duplicate condition evaluator: " + evaluator.Name);
                    }
                    AddInTree.ConditionEvaluators.Add(evaluator.Name, evaluator);
                    runtime.definedConditionEvaluators.Add(properties);
                    goto Label_01B5;
                Label_01A2:
                    throw new AddInLoadException("Unknown node in Import section:" + localName);
                Label_01B5:;
                }
            }
            return runtime;
        }

        public string Assembly
        {
            get
            {
                return this.assembly;
            }
        }

        public List<Properties> DefinedConditionEvaluators
        {
            get
            {
                return this.definedConditionEvaluators;
            }
        }

        public List<Properties> DefinedDoozers
        {
            get
            {
                return this.definedDoozers;
            }
        }

        public System.Reflection.Assembly LoadedAssembly
        {
            get
            {
                if (!this.isAssemblyLoaded)
                {
                    LoggingService.Info("Loading addin " + this.assembly);
                    this.isAssemblyLoaded = true;
                    try
                    {
                        if (this.assembly[0] == ':')
                        {
                            this.loadedAssembly = System.Reflection.Assembly.Load(this.assembly.Substring(1));
                        }
                        else if (this.assembly[0] == '$')
                        {
                            int index = this.assembly.IndexOf('/');
                            if (index < 0)
                            {
                                throw new ApplicationException("Expected '/' in path beginning with '$'!");
                            }
                            string key = this.assembly.Substring(1, index - 1);
                            foreach (AddIn @in in AddInTree.AddIns)
                            {
                                if (@in.Enabled && @in.Manifest.Identities.ContainsKey(key))
                                {
                                    string assemblyFile = Path.Combine(Path.GetDirectoryName(@in.FileName), this.assembly.Substring(index + 1));
                                    this.loadedAssembly = System.Reflection.Assembly.LoadFrom(assemblyFile);
                                    break;
                                }
                            }
                            if (this.loadedAssembly == null)
                            {
                                throw new FileNotFoundException("Could not find referenced AddIn " + key);
                            }
                        }
                        else
                        {
                            this.loadedAssembly = System.Reflection.Assembly.LoadFrom(Path.Combine(this.hintPath, this.assembly));
                        }
                    }
                    catch (FileNotFoundException exception)
                    {
                        MessageService.ShowError("The addin '" + this.assembly + "' could not be loaded:\n" + exception.ToString());
                    }
                    catch (FileLoadException exception2)
                    {
                        MessageService.ShowError("The addin '" + this.assembly + "' could not be loaded:\n" + exception2.ToString());
                    }
                }
                return this.loadedAssembly;
            }
        }
    }
}

