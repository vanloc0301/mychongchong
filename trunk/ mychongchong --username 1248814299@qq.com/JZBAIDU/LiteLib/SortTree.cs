namespace LiteLib
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class SortTree
    {
        public int Count;
        public bool Modified;
        public SortTreeNode Root;

        public SortTreeNode Add(ref string str)
        {
            SortTreeNode root;
            if (this.Root == null)
            {
                this.Root = new SortTreeNode();
                root = this.Root;
            }
            else
            {
                root = this.Root;
                while (true)
                {
                    if (root.Text == str)
                    {
                        root.Count++;
                        return root;
                    }
                    if (root.Text.CompareTo((string) str) > 0)
                    {
                        if (root.Small == null)
                        {
                            root.Small = new SortTreeNode();
                            root.Small.Parent = root;
                            root = root.Small;
                            break;
                        }
                        root = root.Small;
                    }
                    else
                    {
                        if (root.Great == null)
                        {
                            root.Great = new SortTreeNode();
                            root.Great.Parent = root;
                            root = root.Great;
                            break;
                        }
                        root = root.Great;
                    }
                }
            }
            root.Text = str;
            root.ID = this.Count++;
            root.Count++;
            this.Modified = true;
            return root;
        }

        private SortTreeNode Add(string strText, int nCount, object Tag)
        {
            SortTreeNode node = this.Add(ref strText);
            node.Count = nCount;
            node.Tag = Tag;
            return node;
        }

        public void Clear()
        {
            this.Root = null;
            this.Count = 0;
            this.Modified = false;
        }

        public void Open(string fileName, Encoding code, ListView list)
        {
            string str;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream, code);
            SortTreeNode node = null;
            int num = 0;
            ArrayList list2 = new ArrayList();
            while ((str = reader.ReadLine()) != null)
            {
                string[] strArray = str.Split(new char[] { '\t' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    string s = strArray[i];
                    switch (i)
                    {
                        case 0:
                            if (s != "")
                            {
                                node = new SortTreeNode {
                                    Text = s
                                };
                                list2.Add(node);
                                num = 0;
                            }
                            break;

                        case 1:
                            try
                            {
                                if (s != "")
                                {
                                    node.Count = int.Parse(s);
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message);
                            }
                            break;

                        default:
                            if (num++ > 0)
                            {
                                node.Tag = node.Tag + "\r\n";
                            }
                            node.Tag = node.Tag + s;
                            break;
                    }
                }
            }
            reader.Close();
            stream.Close();
            Random random = new Random();
            while (list2.Count > 0)
            {
                int index = random.Next(list2.Count);
                node = (SortTreeNode) list2[index];
                list2.RemoveAt(index);
                node = this.Add(node.Text, node.Count, node.Tag);
                if (list != null)
                {
                    int num4 = node.ID + 1;
                    ListViewItem item = list.Items.Add(num4.ToString());
                    item.SubItems.Add(node.Text);
                    item.SubItems.Add(node.Count.ToString());
                    item.Tag = node;
                }
            }
        }

        private bool OutNode(SortTreeNode node, StreamWriter writer, BitArray bits, ref int nCount)
        {
            if ((node == null) || bits.Get(node.ID))
            {
                return false;
            }
            string str = string.Concat(new object[] { node.Text, '\t', node.Count.ToString(), '\t' });
            if (node.Tag != null)
            {
                str = str + node.Tag.ToString().Replace("\r\n", "\r\n\t\t");
            }
            writer.WriteLine(str.TrimEnd(new char[] { '\t' }));
            bits.Set(node.ID, true);
            nCount--;
            return true;
        }

        public void Save(string fileName, Encoding code)
        {
            FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter writer = new StreamWriter(stream, code);
            BitArray bits = new BitArray(this.Count, false);
            SortTreeNode root = this.Root;
            int count = this.Count;
            while (count > 0)
            {
                if (!((root.Small == null) || bits.Get(root.Small.ID)))
                {
                    root = root.Small;
                }
                else if (!bits.Get(root.ID))
                {
                    this.OutNode(root, writer, bits, ref count);
                }
                else if (!((root.Great == null) || bits.Get(root.Great.ID)))
                {
                    root = root.Great;
                }
                else
                {
                    if (!bits.Get(root.ID))
                    {
                        this.OutNode(root, writer, bits, ref count);
                    }
                    root = root.Parent;
                }
            }
            writer.Close();
            stream.Close();
            this.Modified = false;
        }
    }
}

