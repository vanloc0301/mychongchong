namespace SkyMap.Net.Tools.Holidays
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public class TreeViewHelper
    {
        private static ImageList il = new ImageList();

        static TreeViewHelper()
        {
            il.Images.Add(ResourceService.GetIcon("Icons.16x16.ClosedFolderBitmap"));
            il.Images.Add(ResourceService.GetIcon("Icons.16x16.OpenFolderBitmap"));
        }

        public static TreeNode AddTreeNode(TreeNodeCollection nodes, string text)
        {
            TreeNode node = nodes.Add(text);
            node.ImageIndex = 0;
            return node;
        }

        public static TreeNode AddTreeNode(TreeNodeCollection nodes, string text, object tag)
        {
            TreeNode node = AddTreeNode(nodes, text);
            node.Tag = tag;
            return node;
        }

        private static void NodeExpandCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.IsExpanded)
            {
                e.Node.ImageIndex = 1;
            }
            else
            {
                e.Node.ImageIndex = 0;
            }
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        public static void SetImageList(TreeView tv)
        {
            tv.ImageList = il;
            tv.AfterExpand += new TreeViewEventHandler(TreeViewHelper.NodeExpandCollapse);
            tv.AfterCollapse += new TreeViewEventHandler(TreeViewHelper.NodeExpandCollapse);
        }
    }
}

