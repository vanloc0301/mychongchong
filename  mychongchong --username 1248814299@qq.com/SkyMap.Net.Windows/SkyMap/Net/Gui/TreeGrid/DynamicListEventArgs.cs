namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Diagnostics;

    public class DynamicListEventArgs : EventArgs
    {
        private DynamicList list;

        public DynamicListEventArgs(DynamicList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            this.list = list;
        }

        public DynamicList List
        {
            [DebuggerStepThrough]
            get
            {
                return this.list;
            }
        }
    }
}

