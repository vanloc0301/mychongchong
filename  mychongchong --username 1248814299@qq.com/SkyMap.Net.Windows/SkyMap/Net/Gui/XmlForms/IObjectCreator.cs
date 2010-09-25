namespace SkyMap.Net.Gui.XmlForms
{
    using System;
    using System.Xml;

    public interface IObjectCreator
    {
        object CreateObject(string name, XmlElement el);
        Type GetType(string name);
    }
}

