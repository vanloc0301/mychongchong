namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class TyQueryWhere : DomainObject
    {
        private SkyMap.Net.Criteria.ColumnType columnType;
        private string defaultValue;
        private string tableName;
        private SkyMap.Net.Criteria.TyQuery tyQuery;

        [DisplayName("列类型")]
        public SkyMap.Net.Criteria.ColumnType ColumnType
        {
            get
            {
                return this.columnType;
            }
            set
            {
                this.columnType = value;
            }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Description("当列类型为值列表，可以有以下三种设置方法：\r\n1.直接以“,”为分隔符分隔\r\n2.使用DICT:字典类型名称\r\n3.SQL[-命名空间]:SQL语句（字段必须包括：Text,Value)"), DisplayName("默认值或值列表设置")]
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
            }
        }

        [DisplayName("列名")]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        [DisplayName("表名")]
        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }

        [Browsable(false)]
        public SkyMap.Net.Criteria.TyQuery TyQuery
        {
            get
            {
                return this.tyQuery;
            }
            set
            {
                this.tyQuery = value;
            }
        }
    }
}

