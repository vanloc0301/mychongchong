namespace SkyMap.Net.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class DAOObjects
    {
        private List<DomainObject> dirtyObjects = new List<DomainObject>();
        private bool isChanged = false;
        private List<DomainObject> newObjects = new List<DomainObject>();
        private List<DomainObject> removedObjects = new List<DomainObject>();

        public event EventHandler Changed;

        public event EventHandler Cleared;

        public void Clear()
        {
            if (this.isChanged)
            {
                this.newObjects.Clear();
                this.dirtyObjects.Clear();
                this.removedObjects.Clear();
                this.OnCleared();
            }
        }

        protected void OnChanged()
        {
            if (!this.isChanged)
            {
                if (((this.newObjects.Count != 0) || (this.dirtyObjects.Count != 0)) || (this.removedObjects.Count != 0))
                {
                    if (this.Changed != null)
                    {
                        this.Changed(this, new EventArgs());
                    }
                    this.isChanged = true;
                }
            }
            else if (((this.newObjects.Count == 0) && (this.dirtyObjects.Count == 0)) && (this.removedObjects.Count == 0))
            {
                this.OnCleared();
            }
        }

        protected void OnCleared()
        {
            if (this.Cleared != null)
            {
                this.Cleared(this, new EventArgs());
            }
            this.isChanged = false;
        }

        public void RegisterDirty(DomainObject obj)
        {
            if (!(this.newObjects.Contains(obj) || this.dirtyObjects.Contains(obj)))
            {
                this.dirtyObjects.Add(obj);
                this.OnChanged();
            }
        }

        public void RegisterNew(DomainObject obj)
        {
            this.newObjects.Add(obj);
            this.OnChanged();
        }

        public void RegisterRemoved(DomainObject obj)
        {
            if (this.newObjects.Contains(obj))
            {
                this.newObjects.Remove(obj);
                this.OnChanged();
            }
            else
            {
                this.dirtyObjects.Remove(obj);
                if (!this.removedObjects.Contains(obj))
                {
                    this.removedObjects.Add(obj);
                }
            }
            this.OnChanged();
        }

        public void Remove(DomainObject obj)
        {
            this.newObjects.Remove(obj);
            this.dirtyObjects.Remove(obj);
            this.removedObjects.Remove(obj);
            this.OnChanged();
        }

        public List<DomainObject> DirtyObjects
        {
            get
            {
                return this.dirtyObjects;
            }
        }

        public bool IsChanged
        {
            get
            {
                return this.isChanged;
            }
        }

        public List<DomainObject> NewObjects
        {
            get
            {
                return this.newObjects;
            }
        }

        public List<DomainObject> RemovedObjects
        {
            get
            {
                return this.removedObjects;
            }
        }
    }
}

