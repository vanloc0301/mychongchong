namespace LiteLib
{
    using System;
    using System.IO;
    using System.Text;

    public class HashTree
    {
        public int Count;
        public bool Modified;
        public HashTreeNode Root;

        public bool Add(ref string str)
        {
            HashTreeNode root;
            int hashCode = str.GetHashCode();
            if (this.Root == null)
            {
                this.Root = new HashTreeNode();
                root = this.Root;
            }
            else
            {
                root = this.Root;
                while (true)
                {
                    if (root.Code == hashCode)
                    {
                        return false;
                    }
                    if (hashCode < root.Code)
                    {
                        if (root.Small == null)
                        {
                            root.Small = new HashTreeNode();
                            root = root.Small;
                            break;
                        }
                        root = root.Small;
                    }
                    else
                    {
                        if (root.Great == null)
                        {
                            root.Great = new HashTreeNode();
                            root = root.Great;
                            break;
                        }
                        root = root.Great;
                    }
                }
            }
            root.Code = hashCode;
            this.Modified = true;
            this.Count++;
            return true;
        }

        public void Clear()
        {
            this.Root = null;
            this.Count = 0;
            this.Modified = false;
        }

        public void Open(string fileName, Encoding code)
        {
            string str;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream, code);
            while ((str = reader.ReadLine()) != null)
            {
                string[] strArray = str.Split(new char[] { '\t' });
                if (strArray.Length > 0)
                {
                    this.Add(ref strArray[0]);
                }
            }
            reader.Close();
            stream.Close();
        }
    }
}

