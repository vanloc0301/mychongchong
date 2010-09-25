namespace SkyMap.Net.Gui.XmlForms
{
    using SkyMap.Net.Core;
    using System;

    public class SharpDevelopStringValueFilter : IStringValueFilter
    {
        public string GetFilteredValue(string originalValue)
        {
            return SkyMap.Net.Core.StringParser.Parse(originalValue);
        }
    }
}

