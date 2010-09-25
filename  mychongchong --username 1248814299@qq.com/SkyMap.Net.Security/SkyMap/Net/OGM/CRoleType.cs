namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class CRoleType : DomainObject, ISaveAs
    {
        private IList<CRoleType> children;
        private CRoleType parent;
        private IList<CRole> roles;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        [Browsable(false)]
        public IList<CRoleType> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                this.children = value;
            }
        }

        [DisplayName("父级类型")]
        public CRoleType Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        [Browsable(false)]
        public IList<CRole> Roles
        {
            get
            {
                return this.roles;
            }
            set
            {
                this.roles = value;
            }
        }
    }
}

