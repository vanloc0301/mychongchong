namespace SkyMap.Net.Components
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class GridAttribute : Attribute
    {
        private string[] captions;
        private string countField;
        private bool editable;
        private string[] fields;
        private string[] groupFields;
        private bool isUseCheckBox;
        private string menuPath;
        private Navigate pageNavigation;
        private string[] sumFields;
        private string toolbarPath;

        public GridAttribute(string menuPath, string toolbarPath, bool editable, string[] fields, string[] captions, string[] groupFields)
        {
            if ((fields == null) || (captions == null))
            {
                throw new ArgumentNullException("字段与及字段显示名称数组不能为空!");
            }
            if (fields.Length != captions.Length)
            {
                throw new ArgumentException("字段与及字段显示名称数组长度不相等!");
            }
            this.fields = fields;
            this.captions = captions;
            this.groupFields = groupFields;
            this.menuPath = menuPath;
            this.toolbarPath = toolbarPath;
            this.editable = editable;
        }

        public GridAttribute(string menuPath, string toolbarPath, bool editable, string[] fields, string[] captions, string[] groupFields, string[] sumFields) : this(menuPath, toolbarPath, editable, fields, captions, groupFields)
        {
            this.sumFields = sumFields;
        }

        public GridAttribute(string menuPath, string toolbarPath, bool editable, string[] fields, string[] captions, string[] groupFields, string[] sumFields, string countField) : this(menuPath, toolbarPath, editable, fields, captions, groupFields, sumFields)
        {
            this.countField = countField;
        }

        public GridAttribute(string menuPath, string toolbarPath, bool editable, string[] fields, string[] captions, string[] groupFields, string[] sumFields, string countField, bool isUseCheckBox) : this(menuPath, toolbarPath, editable, fields, captions, groupFields, sumFields, countField)
        {
            this.isUseCheckBox = isUseCheckBox;
        }

        public string[] Captions
        {
            get
            {
                return this.captions;
            }
        }

        public string CountField
        {
            get
            {
                return this.countField;
            }
        }

        public bool Editable
        {
            get
            {
                return this.editable;
            }
        }

        public string[] Fields
        {
            get
            {
                return this.fields;
            }
        }

        public string[] GroupFields
        {
            get
            {
                return this.groupFields;
            }
        }

        public bool IsUseCheckBox
        {
            get
            {
                return this.isUseCheckBox;
            }
        }

        public string MenuPath
        {
            get
            {
                return this.menuPath;
            }
        }

        public Navigate PageNavigation
        {
            get
            {
                return this.pageNavigation;
            }
            set
            {
                this.pageNavigation = value;
            }
        }

        public string[] SumFields
        {
            get
            {
                return this.sumFields;
            }
        }

        public string ToolbarPath
        {
            get
            {
                return this.toolbarPath;
            }
        }
    }
}

