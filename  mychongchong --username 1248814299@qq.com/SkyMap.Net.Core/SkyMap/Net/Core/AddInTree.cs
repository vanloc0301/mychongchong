namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;

    public static class AddInTree
    {
        private static List<AddIn> addIns = new List<AddIn>();
        private static Dictionary<string, IConditionEvaluator> conditionEvaluators = new Dictionary<string, IConditionEvaluator>();
        private static Dictionary<string, IDoozer> doozers = new Dictionary<string, IDoozer>();
        private static AddInTreeNode rootNode = new AddInTreeNode();

        static AddInTree()
        {
            doozers.Add("Class", new ClassDoozer());
            doozers.Add("FileFilter", new FileFilterDoozer());
            doozers.Add("String", new StringDoozer());
            doozers.Add("Icon", new IconDoozer());
            doozers.Add("Include", new IncludeDoozer());
            conditionEvaluators.Add("Compare", new CompareConditionEvaluator());
            conditionEvaluators.Add("Ownerstate", new OwnerStateConditionEvaluator());
        }

        private static void AddExtensionPath(ExtensionPath path)
        {
            AddInTreeNode node = CreatePath(rootNode, path.Name);
            foreach (Codon codon in path.Codons)
            {
                node.Codons.Add(codon);
            }
        }

        public static object BuildItem(string path, object caller)
        {
            int num = path.LastIndexOf('/');
            string str = path.Substring(0, num + 1);
            string childItemID = path.Substring(num + 1);
            return GetTreeNode(str).BuildChildItem(childItemID, caller, BuildItems(path, caller, false));
        }

        public static object BuildItem(string path, object caller, Codon codon)
        {
            int length = path.LastIndexOf('/');
            string str = path.Substring(0, length);
            string str2 = path.Substring(length + 1);
            AddInTreeNode treeNode = GetTreeNode(str);
            foreach (Codon codon2 in treeNode.Codons)
            {
                if (codon2.Id == str2)
                {
                    Properties target = new Properties();
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.Debug("Include 需要新设置属性...");
                    }
                    codon2.Properties.CopyTo(target);
                    codon.Properties.CopyTo(target);
                    Codon codon3 = new Codon(codon2.AddIn, codon2.Name, target, codon2.Conditions);
                    return codon3.BuildItem(caller, BuildItems(path, caller, false));
                }
            }
            throw new TreePathNotFoundException(path);
        }

        public static List<T> BuildItems<T>(string path, object caller)
        {
            return BuildItems<T>(path, caller, true);
        }

        public static ArrayList BuildItems(string path, object caller, bool throwOnNotFound)
        {
            AddInTreeNode treeNode = GetTreeNode(path, throwOnNotFound);
            if (treeNode == null)
            {
                return new ArrayList();
            }
            return treeNode.BuildChildItems(caller);
        }

        public static List<T> BuildItems<T>(string path, object caller, bool throwOnNotFound)
        {
            AddInTreeNode treeNode = GetTreeNode(path, throwOnNotFound);
            if (treeNode == null)
            {
                return new List<T>();
            }
            return treeNode.BuildChildItems<T>(caller);
        }

        private static AddInTreeNode CreatePath(AddInTreeNode localRoot, string path)
        {
            if ((path == null) || (path.Length == 0))
            {
                return localRoot;
            }
            string[] strArray = path.Split(new char[] { '/' });
            AddInTreeNode node = localRoot;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!node.ChildNodes.ContainsKey(strArray[i]))
                {
                    node.ChildNodes[strArray[i]] = new AddInTreeNode();
                }
                node = node.ChildNodes[strArray[i]];
            }
            return node;
        }

        private static void DisableAddin(AddIn addIn, Dictionary<string, Version> dict, Dictionary<string, AddIn> addInDict)
        {
            addIn.Enabled = false;
            addIn.Action = AddInAction.DependencyError;
            foreach (string str in addIn.Manifest.Identities.Keys)
            {
                dict.Remove(str);
                addInDict.Remove(str);
            }
        }

        public static bool ExistsTreeNode(string path)
        {
            if ((path != null) && (path.Length != 0))
            {
                string[] strArray = path.Split(new char[] { '/' });
                AddInTreeNode rootNode = AddInTree.rootNode;
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (!rootNode.ChildNodes.ContainsKey(strArray[i]))
                    {
                        return false;
                    }
                    rootNode = rootNode.ChildNodes[strArray[i]];
                }
            }
            return true;
        }

        public static AddInTreeNode GetTreeNode(string path)
        {
            return GetTreeNode(path, true);
        }

        public static AddInTreeNode GetTreeNode(string path, bool throwOnNotFound)
        {
            if ((path == null) || (path.Length == 0))
            {
                return AddInTree.rootNode;
            }
            string[] strArray = path.Split(new char[] { '/' });
            AddInTreeNode rootNode = AddInTree.rootNode;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!rootNode.ChildNodes.ContainsKey(strArray[i]))
                {
                    if (throwOnNotFound)
                    {
                        throw new TreePathNotFoundException(path);
                    }
                    return null;
                }
                rootNode = rootNode.ChildNodes[strArray[i]];
            }
            return rootNode;
        }

        public static void InsertAddIn(AddIn addIn)
        {
            if (addIn.Enabled)
            {
                string str3;
                foreach (ExtensionPath path in addIn.Paths.Values)
                {
                    AddExtensionPath(path);
                }
                string directoryName = Path.GetDirectoryName(addIn.FileName);
                foreach (string str2 in addIn.BitmapResources)
                {
                    str3 = Path.Combine(directoryName, str2);
                    ResourceService.RegisterNeutralImages(ResourceManager.CreateFileBasedResourceManager(Path.GetFileNameWithoutExtension(str3), Path.GetDirectoryName(str3), null));
                }
                foreach (string str4 in addIn.StringResources)
                {
                    str3 = Path.Combine(directoryName, str4);
                    ResourceService.RegisterNeutralStrings(ResourceManager.CreateFileBasedResourceManager(Path.GetFileNameWithoutExtension(str3), Path.GetDirectoryName(str3), null));
                }
            }
            addIns.Add(addIn);
        }

        public static void Load(List<string> addInFiles, List<string> disabledAddIns)
        {
            AddIn @in;
            int num;
            List<AddIn> list = new List<AddIn>();
            Dictionary<string, Version> addIns = new Dictionary<string, Version>();
            Dictionary<string, AddIn> addInDict = new Dictionary<string, AddIn>();
            foreach (string str in addInFiles)
            {
                @in = AddIn.Load(str);
                @in.Enabled = true;
                if ((disabledAddIns != null) && (disabledAddIns.Count > 0))
                {
                    foreach (string str2 in @in.Manifest.Identities.Keys)
                    {
                        if (disabledAddIns.Contains(str2))
                        {
                            @in.Enabled = false;
                            break;
                        }
                    }
                }
                if (@in.Enabled)
                {
                    foreach (KeyValuePair<string, Version> pair in @in.Manifest.Identities)
                    {
                        if (addIns.ContainsKey(pair.Key))
                        {
                            MessageService.ShowError("Name '" + pair.Key + "' is used by '" + addInDict[pair.Key].FileName + "' and '" + str + "'");
                            @in.Enabled = false;
                            @in.Action = AddInAction.InstalledTwice;
                            break;
                        }
                        addIns.Add(pair.Key, pair.Value);
                        addInDict.Add(pair.Key, @in);
                    }
                }
                list.Add(@in);
            }
        Label_01EF:
            num = 0;
            while (num < list.Count)
            {
                @in = list[num];
                if (@in.Enabled)
                {
                    Version version;
                    foreach (AddInReference reference in @in.Manifest.Conflicts)
                    {
                        if (reference.Check(addIns, out version))
                        {
                            MessageService.ShowError(@in.Name + " conflicts with " + reference.ToString() + " and has been disabled.");
                            DisableAddin(@in, addIns, addInDict);
                            goto Label_01EF;
                        }
                    }
                    foreach (AddInReference reference in @in.Manifest.Dependencies)
                    {
                        if (!reference.Check(addIns, out version))
                        {
                            if (version != null)
                            {
                                MessageService.ShowError(@in.Name + " has not been loaded because it requires " + reference.ToString() + ", but version " + version.ToString() + " is installed.");
                            }
                            else
                            {
                                MessageService.ShowError(@in.Name + " has not been loaded because it requires " + reference.ToString() + ".");
                            }
                            DisableAddin(@in, addIns, addInDict);
                            goto Label_01EF;
                        }
                    }
                }
                num++;
            }
            foreach (AddIn @tmpin in list)
            {
                InsertAddIn(@tmpin);
            }
        }

        public static void RemoveAddIn(AddIn addIn)
        {
            if (addIn.Enabled)
            {
                throw new ArgumentException("Cannot remove enabled AddIns at runtime.");
            }
            addIns.Remove(addIn);
        }

        public static IList<AddIn> AddIns
        {
            get
            {
                return addIns.AsReadOnly();
            }
        }

        public static Dictionary<string, IConditionEvaluator> ConditionEvaluators
        {
            get
            {
                return conditionEvaluators;
            }
        }

        public static Dictionary<string, IDoozer> Doozers
        {
            get
            {
                return doozers;
            }
        }
    }
}

