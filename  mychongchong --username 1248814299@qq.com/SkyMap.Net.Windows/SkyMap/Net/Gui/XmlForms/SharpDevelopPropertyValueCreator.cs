namespace SkyMap.Net.Gui.XmlForms
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;

    public class SharpDevelopPropertyValueCreator : IPropertyValueCreator
    {
        public bool CanCreateValueForType(Type propertyType)
        {
            return ((propertyType == typeof(Icon)) || (propertyType == typeof(Image)));
        }

        public object CreateValue(Type propertyType, string valueString)
        {
            if (propertyType == typeof(Icon))
            {
                return ResourceService.GetIcon(valueString);
            }
            if (propertyType == typeof(Image))
            {
                return ResourceService.GetBitmap(valueString);
            }
            return null;
        }
    }
}

