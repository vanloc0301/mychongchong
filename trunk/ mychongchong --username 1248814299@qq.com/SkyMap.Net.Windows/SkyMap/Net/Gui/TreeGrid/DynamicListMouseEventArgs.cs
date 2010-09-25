namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    public class DynamicListMouseEventArgs : MouseEventArgs
    {
        private DynamicList list;

        public DynamicListMouseEventArgs(DynamicList list, MouseEventArgs me) : base(me.Button, me.Clicks, me.X, me.Y, me.Delta)
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

