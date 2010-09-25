namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;

    [Serializable]
    public abstract class DomainObject
    {
        private string description;
        private int displayOrder;
        private bool enableRemove;
        private bool enableUpdate;
        private string id;
        private bool isActive;
        private string name;
        private int replicationVersion;

        public DomainObject()
        {
            this.isActive = true;
            this.enableRemove = true;
            this.enableUpdate = true;
            this.name = string.Empty;
            this.description = string.Empty;
        }

        protected DomainObject(string name)
        {
            this.isActive = true;
            this.enableRemove = true;
            this.enableUpdate = true;
            this.name = name;
        }

        public virtual T Clone<T>() where T: DomainObject
        {
            return (T) base.MemberwiseClone();
        }

        public virtual void Delete()
        {
            UnitOfWork work = new UnitOfWork(base.GetType());
            work.RegisterRemoved(this);
            work.Commit();
        }

        public override bool Equals(object obj)
        {
            if ((obj != null) && base.GetType().Equals(obj.GetType()))
            {
                if ((obj as DomainObject).Id == null)
                {
                    return base.Equals(obj);
                }
                if ((obj as DomainObject).Id.Equals(this.Id))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual void Save()
        {
            UnitOfWork work = new UnitOfWork(base.GetType());
            if (string.IsNullOrEmpty(this.Id))
            {
                this.Id = StringHelper.GetNewGuid();
            }
            work.RegisterNew(this);
            work.Commit();
        }

        public override string ToString()
        {
            string name = this.Name;
            if (!string.IsNullOrEmpty(this.Description))
            {
                name = name + "(" + this.Description + ")";
            }
            return name;
        }

        public virtual void Update()
        {
            UnitOfWork work = new UnitOfWork(base.GetType());
            work.RegisterDirty(this);
            work.Commit();
        }

        [DisplayName("描述")]
        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        [DisplayName("显示顺序")]
        public virtual int DisplayOrder
        {
            get
            {
                return this.displayOrder;
            }
            set
            {
                this.displayOrder = value;
            }
        }

        [DisplayName("能否删除")]
        public virtual bool EnableRemove
        {
            get
            {
                return this.enableRemove;
            }
            set
            {
                this.enableRemove = value;
            }
        }

        [DisplayName("能否修改")]
        public virtual bool EnableUpdate
        {
            get
            {
                return this.enableUpdate;
            }
            set
            {
                this.enableUpdate = value;
            }
        }

        [ReadOnly(true)]
        public virtual string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("是否有效")]
        public virtual bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
            }
        }

        [DisplayName("名称"), ParenthesizePropertyName(true)]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [Browsable(false)]
        public virtual int ReplicationVersion
        {
            get
            {
                return this.replicationVersion;
            }
            set
            {
                this.replicationVersion = value;
            }
        }
    }
}

