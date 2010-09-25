namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public static class ToolbarService
    {
        public static ToolStrip[] CreateToolbars(object owner, string addInTreePath)
        {
            AddInTreeNode treeNode;
            try
            {
                treeNode = AddInTree.GetTreeNode(addInTreePath);
            }
            catch (TreePathNotFoundException)
            {
                LoggingService.WarnFormatted("Toolbar path '{0}' not found!", new object[] { addInTreePath });
                return null;
            }
            List<ToolStrip> list = new List<ToolStrip>();
            if (treeNode.ChildNodes.Count > 0)
            {
                foreach (KeyValuePair<string, AddInTreeNode> pair in treeNode.ChildNodes)
                {
                    list.Add(CreateToolStrip(owner, pair.Value));
                }
            }
            else
            {
                list.Add(CreateToolStrip(owner, treeNode));
            }
            return list.ToArray();
        }

        public static ToolStrip CreateToolStrip(object owner, AddInTreeNode treeNode)
        {
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Items.AddRange(CreateToolStripItems(owner, treeNode));
            UpdateToolbar(toolStrip);
            new LanguageChangeWatcher(toolStrip);
            return toolStrip;
        }

        public static ToolStrip CreateToolStrip(object owner, string addInTreePath)
        {
            if (AddInTree.ExistsTreeNode(addInTreePath))
            {
                return CreateToolStrip(owner, AddInTree.GetTreeNode(addInTreePath));
            }
            LoggingService.WarnFormatted("不存在插件配置:{0}", new object[] { addInTreePath });
            return null;
        }

        public static ToolStripItem[] CreateToolStripItems(object owner, AddInTreeNode treeNode)
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            foreach (object obj2 in treeNode.BuildChildItems(owner))
            {
                if (obj2 is ToolStripItem)
                {
                    list.Add((ToolStripItem) obj2);
                }
                else
                {
                    ISubmenuBuilder builder = (ISubmenuBuilder) obj2;
                    list.AddRange(builder.BuildSubmenu(null, owner));
                }
            }
            return list.ToArray();
        }

        public static ToolStripItem[] CreateToolStripItems(object owner, string addInTreePath)
        {
            try
            {
                AddInTreeNode treeNode = AddInTree.GetTreeNode(addInTreePath);
                return CreateToolStripItems(owner, treeNode);
            }
            catch (TreePathNotFoundException)
            {
                LoggingService.WarnFormatted("Toolbar path '{0}' not found!", new object[] { addInTreePath });
                return null;
            }
        }

        public static void UpdateToolbar(ToolStrip toolStrip)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item is IStatusUpdate)
                {
                    ((IStatusUpdate) item).UpdateStatus();
                }
            }
        }

        public static void UpdateToolbarText(ToolStrip toolStrip)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item is IStatusUpdate)
                {
                    ((IStatusUpdate) item).UpdateText();
                }
            }
        }

        private class LanguageChangeWatcher
        {
            private ToolStrip toolStrip;

            public LanguageChangeWatcher(ToolStrip toolStrip)
            {
                this.toolStrip = toolStrip;
                toolStrip.Disposed += new EventHandler(this.Disposed);
                ResourceService.LanguageChanged += new EventHandler(this.OnLanguageChanged);
            }

            private void Disposed(object sender, EventArgs e)
            {
                ResourceService.LanguageChanged -= new EventHandler(this.OnLanguageChanged);
            }

            private void OnLanguageChanged(object sender, EventArgs e)
            {
                ToolbarService.UpdateToolbarText(this.toolStrip);
            }
        }
    }
}

