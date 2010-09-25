namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;

    public class LocalizedPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor basePropertyDescriptor;
        private TypeConverter customTypeConverter;
        private string localizedCategory;
        private string localizedDescription;
        private string localizedName;

        public LocalizedPropertyDescriptor(PropertyDescriptor basePropertyDescriptor) : base(basePropertyDescriptor)
        {
            this.localizedName = string.Empty;
            this.localizedDescription = string.Empty;
            this.localizedCategory = string.Empty;
            this.customTypeConverter = null;
            LocalizedPropertyAttribute attribute = null;
            foreach (Attribute attribute2 in basePropertyDescriptor.Attributes)
            {
                attribute = attribute2 as LocalizedPropertyAttribute;
                if (attribute != null)
                {
                    break;
                }
            }
            if (attribute != null)
            {
                this.localizedName = attribute.Name;
                this.localizedDescription = attribute.Description;
                this.localizedCategory = attribute.Category;
            }
            else
            {
                this.localizedName = basePropertyDescriptor.Name;
                this.localizedDescription = basePropertyDescriptor.Description;
                this.localizedCategory = basePropertyDescriptor.Category;
            }
            this.basePropertyDescriptor = basePropertyDescriptor;
            if (basePropertyDescriptor.PropertyType == typeof(bool))
            {
                this.customTypeConverter = new BooleanTypeConverter();
            }
        }

        public override bool CanResetValue(object component)
        {
            return this.basePropertyDescriptor.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return this.basePropertyDescriptor.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            this.basePropertyDescriptor.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            if ((this.customTypeConverter != null) && (value.GetType() != this.PropertyType))
            {
                this.basePropertyDescriptor.SetValue(component, this.customTypeConverter.ConvertFrom(value));
            }
            else
            {
                this.basePropertyDescriptor.SetValue(component, value);
            }
            if (component is LocalizedObject)
            {
                ((LocalizedObject) component).InformSetValue(this, component, value);
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.basePropertyDescriptor.ShouldSerializeValue(component);
        }

        public override string Category
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse(this.localizedCategory);
            }
        }

        public override Type ComponentType
        {
            get
            {
                return this.basePropertyDescriptor.ComponentType;
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                if (this.customTypeConverter != null)
                {
                    return this.customTypeConverter;
                }
                return base.Converter;
            }
        }

        public override string Description
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse(this.localizedDescription);
            }
        }

        public override string DisplayName
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse(this.localizedName);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this.basePropertyDescriptor.IsReadOnly;
            }
        }

        public override string Name
        {
            get
            {
                return this.basePropertyDescriptor.Name;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.basePropertyDescriptor.PropertyType;
            }
        }
    }
}

