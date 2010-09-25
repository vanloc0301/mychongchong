namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42")]
    public enum QueryQueryType
    {
        [XmlEnum("01")]
        Item01 = 0,
        [XmlEnum("02")]
        Item02 = 1
    }
}

