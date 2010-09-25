namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.Xml.Serialization;

    [Serializable, XmlType(Namespace="http://www.eappxml.gov.cn/eAppXML"), GeneratedCode("xsd", "2.0.50727.42")]
    public enum CxType
    {
        [XmlEnum("01")]
        Item01 = 0,
        [XmlEnum("02")]
        Item02 = 1,
        [XmlEnum("03")]
        Item03 = 2,
        [XmlEnum("04")]
        Item04 = 3,
        [XmlEnum("05")]
        Item05 = 4,
        [XmlEnum("06")]
        Item06 = 5,
        [XmlEnum("07")]
        Item07 = 6,
        [XmlEnum("08")]
        Item08 = 7,
        [XmlEnum("09")]
        Item09 = 8
    }
}

