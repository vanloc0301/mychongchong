namespace SkyMap.Net.Gui.TreeGrid
{
    using System;

    public class CollectionItemEventArgs<T> : EventArgs
    {
        private T item;

        public CollectionItemEventArgs(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get
            {
                return this.item;
            }
        }
    }
}

