namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class BooleanTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(bool)) || (sourceType == typeof(string)));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(bool)) || (destinationType == typeof(string)));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return (value.ToString() == this.True);
            }
            return value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is bool)
            {
                return (((bool) value) ? this.True : this.False);
            }
            return value;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new object[] { this.True, this.False });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        private string False
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Gui.Components.BooleanTypeConverter.FalseString}");
            }
        }

        private string True
        {
            get
            {
                return SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Gui.Components.BooleanTypeConverter.TrueString}");
            }
        }
    }
}

