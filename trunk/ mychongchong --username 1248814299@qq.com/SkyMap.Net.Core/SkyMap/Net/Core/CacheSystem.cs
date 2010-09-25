namespace SkyMap.Net.Core
{
    using Bamboo.Prevalence;
    using System;
    using System.Collections;

    [Serializable]
    internal class CacheSystem : MarshalByRefObject
    {
        private Hashtable _items = new Hashtable();

        public void Add(object key, object value)
        {
            lock (this._items)
            {
                CacheEntry entry = this._items[key] as CacheEntry;
                if (entry == null)
                {
                    entry = new CacheEntry();
                    entry.Key = key;
                    entry.Value = value;
                    entry.DateCreated = PrevalenceEngine.Now;
                    this._items.Add(key, entry);
                }
                else
                {
                    entry.Value = value;
                    this._items[key] = entry;
                }
            }
        }

        public void Clear()
        {
            lock (this._items)
            {
                this._items.Clear();
            }
        }

        public bool Contains(object key)
        {
            lock (this._items)
            {
                return this._items.Contains(key);
            }
        }

        public object Get(object key)
        {
            lock (this._items)
            {
                CacheEntry entry = this._items[key] as CacheEntry;
                if (entry == null)
                {
                    return null;
                }
                return entry.Value;
            }
        }

        public void Remove(object key)
        {
            lock (this._items)
            {
                this._items.Remove(key);
            }
        }
    }
}

