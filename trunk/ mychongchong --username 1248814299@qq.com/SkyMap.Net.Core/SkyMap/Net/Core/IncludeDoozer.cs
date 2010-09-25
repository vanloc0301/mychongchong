namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class IncludeDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            string path = codon.Properties["item"];
            string str2 = codon.Properties["path"];
            if ((path != null) && (path.Length > 0))
            {
                return AddInTree.BuildItem(path, caller, codon);
            }
            if ((str2 != null) && (str2.Length > 0))
            {
                return new IncludeReturnItem(caller, str2);
            }
            MessageService.ShowMessage("<Include> requires the attribute 'item' (to include one item) or the attribute 'path' (to include multiple items)");
            return null;
        }

        public bool HandleConditions
        {
            get
            {
                return false;
            }
        }

        private class IncludeReturnItem : IBuildItemsModifier
        {
            private object caller;
            private string path;

            public IncludeReturnItem(object caller, string path)
            {
                this.caller = caller;
                this.path = path;
            }

            public void Apply(IList items)
            {
                try
                {
                    AddInTreeNode treeNode = AddInTree.GetTreeNode(this.path);
                    foreach (object obj2 in treeNode.BuildChildItems(this.caller))
                    {
                        items.Add(obj2);
                    }
                }
                catch (TreePathNotFoundException)
                {
                    MessageService.ShowError("IncludeDoozer: AddinTree-Path not found: " + this.path);
                }
            }
        }
    }
}

