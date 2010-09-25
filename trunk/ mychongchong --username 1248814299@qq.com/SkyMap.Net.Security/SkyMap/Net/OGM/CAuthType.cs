namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class CAuthType : DomainObject, ISaveAs
    {
        private string authGetClass;
        private string authSetClass;
        private string authTable;
        private string resourceIdField;
        private string resourceName = string.Empty;
        private string resourceNameField;
        private string resourceTable = string.Empty;
        private string typeCode;
        private IList<CUniversalAuth> universalAuths;

        public CAuthType()
        {
            this.ResourceNameField = string.Empty;
            this.resourceIdField = string.Empty;
            this.authTable = string.Empty;
            this.authSetClass = string.Empty;
            this.authGetClass = string.Empty;
            this.typeCode = string.Empty;
            this.universalAuths = new List<CUniversalAuth>();
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (CUniversalAuth auth in this.universalAuths)
            {
                auth.SaveAs(unitOfWork);
            }
        }

        [DisplayName("权限获取类")]
        public string AuthGetClass
        {
            get
            {
                return this.authGetClass;
            }
            set
            {
                this.authGetClass = value;
            }
        }

        [DisplayName("权限设置类")]
        public virtual string AuthSetClass
        {
            get
            {
                return this.authSetClass;
            }
            set
            {
                this.authSetClass = value;
            }
        }

        [DisplayName("权限表名")]
        public string AuthTable
        {
            get
            {
                return this.authTable;
            }
            set
            {
                this.authTable = value;
            }
        }

        [DisplayName("资源ID字段")]
        public string ResourceIdField
        {
            get
            {
                return this.resourceIdField;
            }
            set
            {
                this.resourceIdField = value;
            }
        }

        [DisplayName("资源名称")]
        public string ResourceName
        {
            get
            {
                return this.resourceName;
            }
            set
            {
                this.resourceName = value;
            }
        }

        [DisplayName("资源名称字段")]
        public string ResourceNameField
        {
            get
            {
                return this.resourceNameField;
            }
            set
            {
                this.resourceNameField = value;
            }
        }

        [DisplayName("资源表名")]
        public string ResourceTable
        {
            get
            {
                return this.resourceTable;
            }
            set
            {
                this.resourceTable = value;
            }
        }

        [DisplayName("类型代码")]
        public string TypeCode
        {
            get
            {
                return this.typeCode;
            }
            set
            {
                this.typeCode = value;
            }
        }

        [Browsable(false)]
        public IList<CUniversalAuth> UniversalAuths
        {
            get
            {
                return this.universalAuths;
            }
            set
            {
                this.universalAuths = value;
            }
        }
    }
}

