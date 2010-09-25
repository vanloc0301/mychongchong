namespace SkyMap.Net.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class ExtTreeViewComparer : IComparer<TreeNode>
    {
        public int Compare(TreeNode x, TreeNode y)
        {
            ExtTreeNode node = x as ExtTreeNode;
            ExtTreeNode node2 = y as ExtTreeNode;
            if ((node == null) || (node2 == null))
            {
                return x.Text.CompareTo(y.Text);
            }
            if (node.SortOrder != node2.SortOrder)
            {
                return Math.Sign((int) (node.SortOrder - node2.SortOrder));
            }
            return node.CompareString.CompareTo(node2.CompareString);
        }
    }
}

