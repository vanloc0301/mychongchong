namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;

    public class LocalizedProperty : PropertyDescriptor
    {
        private string category;
        private object defaultValue;
        private string description;
        private string localizedName;
        private string name;
        private string type;
        private TypeConverter typeConverterObject;

        public LocalizedProperty(string name, string type, string category, string description) : base(name, null)
        {
            this.typeConverterObject = null;
            this.defaultValue = null;
            this.category = category;
            this.description = description;
            this.name = name;
            this.type = type;
        }

        public override bool CanResetValue(object component)
        {
            return (this.defaultValue != null);
        }

        public override object GetValue(object component)
        {
            string str = SkyMap.Net.Core.StringParser.Properties["Properties." + this.Name];
            if (this.typeConverterObject is BooleanTypeConverter)
            {
                return bool.Parse(str);
            }
            return str;
        }

        public override void ResetValue(object component)
        {
            this.SetValue(component, this.defaultValue);
        }

        public override void SetValue(object component, object val)
        {
            if (this.typeConverterObject != null)
            {
                SkyMap.Net.Core.StringParser.Properties["Properties." + this.Name] = this.typeConverterObject.ConvertFrom(val).ToString();
            }
            else
            {
                SkyMap.Net.Core.StringParser.Properties["Properties." + this.Name] = val.ToString();
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override string Category
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse(this.category);
            }
        }

        public override Type ComponentType
        {
            get
            {
                return Type.GetType(this.type);
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                if (this.typeConverterObject != null)
                {
                    return this.typeConverterObject;
                }
                return base.Converter;
            }
        }

        public object DefaultValue
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

        public override string Description
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse(this.description);
            }
        }

        public override string DisplayName
        {
            get
            {
                if ((this.localizedName != null) && (this.localizedName.Length > 0))
                {
                    return this.LocalizedName;
                }
                return this.Name;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public string LocalizedName
        {
            get
            {
                if (this.localizedName == null)
                {
                    return null;
                }
                return SkyMap.Net.Core.StringParser.Parse(this.localizedName);
            }
            set
            {
                this.localizedName = value;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return Type.GetType(this.type);
            }
        }

        public TypeConverter TypeConverterObject
        {
            get
            {
                return this.typeConverterObject;
            }
            set
            {
                this.typeConverterObject = value;
            }
        }
    }
}

