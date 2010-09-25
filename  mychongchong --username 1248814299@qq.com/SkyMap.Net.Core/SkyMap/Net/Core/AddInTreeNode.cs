namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AddInTreeNode
    {
        private Dictionary<string, AddInTreeNode> childNodes = new Dictionary<string, AddInTreeNode>();
        private List<Codon> codons = new List<Codon>();
        private bool isSorted = false;

        public object BuildChildItem(string childItemID, object caller, ArrayList subItems)
        {
            foreach (Codon codon in this.codons)
            {
                if (codon.Id == childItemID)
                {
                    return codon.BuildItem(caller, subItems);
                }
            }
            throw new TreePathNotFoundException(childItemID);
        }

        public ArrayList BuildChildItems(object caller)
        {
            ArrayList items = new ArrayList(this.codons.Count);
            if (!this.isSorted)
            {
                this.codons = new TopologicalSort(this.codons).Execute();
                this.isSorted = true;
            }
            foreach (Codon codon in this.codons)
            {
                ArrayList subItems = null;
                if (this.childNodes.ContainsKey(codon.Id))
                {
                    subItems = this.childNodes[codon.Id].BuildChildItems(caller);
                }
                object obj2 = codon.BuildItem(caller, subItems);
                if (obj2 != null)
                {
                    IBuildItemsModifier modifier = obj2 as IBuildItemsModifier;
                    if (modifier != null)
                    {
                        modifier.Apply(items);
                    }
                    else
                    {
                        items.Add(obj2);
                    }
                }
            }
            return items;
        }

        public List<T> BuildChildItems<T>(object caller)
        {
            List<T> items = new List<T>(this.codons.Count);
            if (!this.isSorted)
            {
                this.codons = new TopologicalSort(this.codons).Execute();
                this.isSorted = true;
            }
            foreach (Codon codon in this.codons)
            {
                ArrayList subItems = null;
                if (this.childNodes.ContainsKey(codon.Id))
                {
                    subItems = this.childNodes[codon.Id].BuildChildItems(caller);
                }
                object obj2 = codon.BuildItem(caller, subItems);
                if (obj2 != null)
                {
                    IBuildItemsModifier modifier = obj2 as IBuildItemsModifier;
                    if (modifier != null)
                    {
                        modifier.Apply(items);
                    }
                    else
                    {
                        if (!(obj2 is T))
                        {
                            throw new InvalidCastException("The AddInTreeNode <" + codon.Name + " id='" + codon.Id + "' returned an instance of " + obj2.GetType().FullName + " but the type " + typeof(T).FullName + " is expected.");
                        }
                        items.Add((T) obj2);
                    }
                }
            }
            return items;
        }

        public Dictionary<string, AddInTreeNode> ChildNodes
        {
            get
            {
                return this.childNodes;
            }
        }

        public List<Codon> Codons
        {
            get
            {
                return this.codons;
            }
        }

        public class TopologicalSort
        {
            private List<Codon> codons;
            private Dictionary<string, int> indexOfName;
            private List<Codon> sortedCodons;
            private bool[] visited;

            public TopologicalSort(List<Codon> codons)
            {
                this.codons = codons;
                this.visited = new bool[codons.Count];
                this.sortedCodons = new List<Codon>(codons.Count);
                this.indexOfName = new Dictionary<string, int>(codons.Count);
                for (int i = 0; i < codons.Count; i++)
                {
                    this.visited[i] = false;
                    this.indexOfName[codons[i].Id] = i;
                }
            }

            public List<Codon> Execute()
            {
                this.InsertEdges();
                for (int i = 0; i < this.codons.Count; i++)
                {
                    this.Visit(i);
                }
                return this.sortedCodons;
            }

            private void InsertEdges()
            {
                for (int i = 0; i < this.codons.Count; i++)
                {
                    string insertBefore = this.codons[i].InsertBefore;
                    if ((insertBefore != null) && (insertBefore != ""))
                    {
                        if (this.indexOfName.ContainsKey(insertBefore))
                        {
                            string insertAfter = this.codons[this.indexOfName[insertBefore]].InsertAfter;
                            if ((insertAfter == null) || (insertAfter == ""))
                            {
                                this.codons[this.indexOfName[insertBefore]].InsertAfter = this.codons[i].Id;
                            }
                            else
                            {
                                this.codons[this.indexOfName[insertBefore]].InsertAfter = insertAfter + ',' + this.codons[i].Id;
                            }
                        }
                        else
                        {
                            LoggingService.WarnFormatted("Codon ({0}) specified in the insertbefore of the {1} codon does not exist!", new object[] { insertBefore, this.codons[i] });
                        }
                    }
                }
            }

            private void Visit(int codonIndex)
            {
                if (!this.visited[codonIndex])
                {
                    string[] strArray = this.codons[codonIndex].InsertAfter.Split(new char[] { ',' });
                    foreach (string str in strArray)
                    {
                        if ((str != null) && (str.Length != 0))
                        {
                            if (this.indexOfName.ContainsKey(str))
                            {
                                this.Visit(this.indexOfName[str]);
                            }
                            else
                            {
                                LoggingService.WarnFormatted("Codon ({0}) specified in the insertafter of the {1} codon does not exist!", new object[] { this.codons[codonIndex].InsertAfter, this.codons[codonIndex] });
                            }
                        }
                    }
                    this.sortedCodons.Add(this.codons[codonIndex]);
                    this.visited[codonIndex] = true;
                }
            }
        }
    }
}

