namespace SkyMap.Net.Gui.XmlForms
{
    using System;

    public interface IPropertyValueCreator
    {
        bool CanCreateValueForType(Type propertyType);
        object CreateValue(Type propertyType, string valueString);
    }
}

