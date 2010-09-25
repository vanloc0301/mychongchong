namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public static class MenuService
    {
        private static bool isContextMenuOpen;

        public static void AddItemsToMenu(ToolStripItemCollection collection, object owner, string addInTreePath)
        {
            ArrayList list = AddInTree.GetTreeNode(addInTreePath).BuildChildItems(owner);
            foreach (object obj2 in list)
            {
                if (obj2 is ToolStripItem)
                {
                    collection.Add((ToolStripItem) obj2);
                    if (obj2 is IStatusUpdate)
                    {
                        ((IStatusUpdate) obj2).UpdateStatus();
                    }
                }
                else
                {
                    ISubmenuBuilder builder = (ISubmenuBuilder) obj2;
                    collection.AddRange(builder.BuildSubmenu(null, owner));
                }
            }
        }

        private static void ContextMenuClosed(object sender, EventArgs e)
        {
            isContextMenuOpen = false;
        }

        private static void ContextMenuOpened(object sender, EventArgs e)
        {
            isContextMenuOpen = true;
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            foreach (object obj2 in strip.Items)
            {
                if (obj2 is IStatusUpdate)
                {
                    ((IStatusUpdate) obj2).UpdateStatus();
                }
            }
        }

        public static ContextMenuStrip CreateContextMenu(object owner, string addInTreePath)
        {
            AddInTreeNode treeNode;
            if (addInTreePath == null)
            {
                LoggingService.Warn("addInTreePath为空，无法创建右键上下文菜单...");
                return null;
            }
            try
            {
                treeNode = AddInTree.GetTreeNode(addInTreePath);
            }
            catch (TreePathNotFoundException)
            {
                LoggingService.WarnFormatted("Toolbar path '{0}' not found!", new object[] { addInTreePath });
                return null;
            }
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(new ToolStripMenuItem("dummy"));
            contextMenu.Opening += delegate {
                contextMenu.Items.Clear();
                if (treeNode.ChildNodes.Count > 0)
                {
                    foreach (AddInTreeNode node in treeNode.ChildNodes.Values)
                    {
                        if (contextMenu.Items.Count > 0)
                        {
                            contextMenu.Items.Add(new ToolBarSeparator());
                        }
                        else
                        {
                            contextMenu.Items.AddRange(ToolbarService.CreateToolStripItems(owner, node));
                        }
                    }
                }
                else
                {
                    contextMenu.Items.AddRange(ToolbarService.CreateToolStripItems(owner, treeNode));
                }
            };
            contextMenu.Opened += new EventHandler(MenuService.ContextMenuOpened);
            contextMenu.Closed += new ToolStripDropDownClosedEventHandler(MenuService.ContextMenuClosed);
            return contextMenu;
        }

        public static void CreateQuickInsertMenu(TextBoxBase targetControl, Control popupControl, string[,] quickInsertMenuItems)
        {
            ContextMenuStrip quickInsertMenu = new ContextMenuStrip();
            for (int i = 0; i < quickInsertMenuItems.GetLength(0); i++)
            {
                if (quickInsertMenuItems[i, 0] == "-")
                {
                    quickInsertMenu.Items.Add(new MenuSeparator());
                }
                else
                {
                    MenuCommand command = new MenuCommand(quickInsertMenuItems[i, 0], new QuickInsertMenuHandler(targetControl, quickInsertMenuItems[i, 1]).EventHandler);
                    quickInsertMenu.Items.Add(command);
                }
            }
            new QuickInsertHandler(popupControl, quickInsertMenu);
        }

        public static void ShowContextMenu(object owner, string addInTreePath, Control parent, int x, int y)
        {
            CreateContextMenu(owner, addInTreePath).Show(parent, new Point(x, y));
        }

        public static bool IsContextMenuOpen
        {
            get
            {
                return isContextMenuOpen;
            }
        }

        private class QuickInsertHandler
        {
            private Control popupControl;
            private ContextMenuStrip quickInsertMenu;

            public QuickInsertHandler(Control popupControl, ContextMenuStrip quickInsertMenu)
            {
                this.popupControl = popupControl;
                this.quickInsertMenu = quickInsertMenu;
                popupControl.Click += new EventHandler(this.showQuickInsertMenu);
            }

            private void showQuickInsertMenu(object sender, EventArgs e)
            {
                Point position = new Point(this.popupControl.Width, 0);
                this.quickInsertMenu.Show(this.popupControl, position);
            }
        }

        private class QuickInsertMenuHandler
        {
            private TextBoxBase targetControl;
            private string text;

            public QuickInsertMenuHandler(TextBoxBase targetControl, string text)
            {
                this.targetControl = targetControl;
                this.text = text;
            }

            private void PopupMenuHandler(object sender, EventArgs e)
            {
                this.targetControl.SelectedText = this.targetControl.SelectedText + this.text;
            }

            public System.EventHandler EventHandler
            {
                get
                {
                    return new System.EventHandler(this.PopupMenuHandler);
                }
            }
        }
    }
}

