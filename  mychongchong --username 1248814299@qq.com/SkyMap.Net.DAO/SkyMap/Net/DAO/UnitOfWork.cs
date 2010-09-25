namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UnitOfWork
    {
        private string daoName;
        private List<DomainObject> dirtyObjects;
        private bool isChanged;
        private List<DomainObject> newObjects;
        private List<DomainObject> removedObjects;
        private string url;

        public event EventHandler Changed;

        public event EventHandler Cleared;

        public UnitOfWork(Type type)
        {
            this.newObjects = new List<DomainObject>();
            this.dirtyObjects = new List<DomainObject>();
            this.removedObjects = new List<DomainObject>();
            this.isChanged = false;
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.daoName = SupportClass.GetDAOConfigFile(type.Namespace);
        }

        public UnitOfWork(Type type, string url) : this(type)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.url = url;
        }

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

        public void Commit()
        {
            if (!this.isChanged)
            {
                if (LoggingService.IsWarnEnabled)
                {
                    LoggingService.Warn("No changes to commit");
                }
            }
            else
            {
                this.DBSession.Put(this.daoName, this.newObjects, this.dirtyObjects, this.removedObjects, true);
                this.Clear();
            }
        }

        public void Commit(DAOObjects daoObjects)
        {
            this.DBSession.Put(this.daoName, daoObjects.NewObjects, daoObjects.DirtyObjects, daoObjects.RemovedObjects, true);
            daoObjects.Clear();
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

        public void RegisterNew<T>(IEnumerable<T> objs) where T: DomainObject
        {
            foreach (DomainObject obj2 in objs)
            {
                if (!this.newObjects.Contains(obj2))
                {
                    this.newObjects.Add(obj2);
                }
            }
            this.OnChanged();
        }

        public void RegisterNew(DomainObject obj)
        {
            if (!this.newObjects.Contains(obj))
            {
                this.newObjects.Add(obj);
                if (string.IsNullOrEmpty(obj.Id))
                {
                    obj.Id = StringHelper.GetNewGuid();
                }
                this.OnChanged();
            }
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

        private IDBSession DBSession
        {
            get
            {
                return DBSessionFactory.GetInstanceByUrl(this.url);
            }
        }
    }
}

