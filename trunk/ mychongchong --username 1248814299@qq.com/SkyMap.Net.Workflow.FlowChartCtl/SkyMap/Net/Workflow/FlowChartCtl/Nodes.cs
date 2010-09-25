namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class Nodes : CollectionBase
    {
        private int mIndex = 1;

        public void Add(Node o)
        {
            base.List.Add(o);
            o.ID = "NODE" + this.mIndex.ToString();
            o.Name = "新节点" + this.mIndex.ToString();
            this.mIndex++;
        }

        public void Remove(Node o)
        {
            base.List.Remove(o);
        }

        public void Reset()
        {
            base.Clear();
            this.mIndex = 1;
        }

        public Node this[int index]
        {
            get
            {
                return (Node) base.List[index];
            }
        }
    }
}

