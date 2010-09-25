namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ArrowLines : CollectionBase
    {
        private int mIndex = 1;

        public void Add(ArrowLine o)
        {
            base.List.Add(o);
            o.ID = "ALINE" + this.mIndex.ToString();
            this.mIndex++;
        }

        public void Remove(ArrowLine o)
        {
            base.List.Remove(o);
        }

        public void Reset()
        {
            base.Clear();
            this.mIndex = 1;
        }

        public ArrowLine this[int index]
        {
            get
            {
                return (ArrowLine) base.List[index];
            }
        }
    }
}

