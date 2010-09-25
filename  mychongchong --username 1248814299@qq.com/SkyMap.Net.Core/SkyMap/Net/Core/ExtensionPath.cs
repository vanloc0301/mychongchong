namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class ExtensionPath
    {
        private SkyMap.Net.Core.AddIn addIn;
        private List<Codon> codons = new List<Codon>();
        private string name;

        public ExtensionPath(string name, SkyMap.Net.Core.AddIn addIn)
        {
            this.addIn = addIn;
            this.name = name;
        }

        public static void SetUp(ExtensionPath extensionPath, XmlTextReader reader, string endElement)
        {
            Stack<ICondition> stack = new Stack<ICondition>();
            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;
                if (nodeType == XmlNodeType.Element)
                {
                    string localName = reader.LocalName;
                    if (localName == "Condition")
                    {
                        ICondition item = Condition.Read(reader);
                        stack.Push(item);
                    }
                    else if (localName == "ComplexCondition")
                    {
                        stack.Push(Condition.ReadComplexCondition(reader));
                    }
                    else
                    {
                        Codon codon = new Codon(extensionPath.AddIn, localName, Properties.ReadFromAttributes(reader), stack.ToArray());
                        extensionPath.codons.Add(codon);
                        if (!reader.IsEmptyElement)
                        {
                            SetUp(extensionPath.AddIn.GetExtensionPath(extensionPath.Name + "/" + codon.Id), reader, localName);
                        }
                    }
                }
                else if (nodeType == XmlNodeType.EndElement)
                {
                    if ((reader.LocalName == "Condition") || (reader.LocalName == "ComplexCondition"))
                    {
                        stack.Pop();
                    }
                    else if (reader.LocalName == endElement)
                    {
                        break;
                    }
                }
            }
        }

        public SkyMap.Net.Core.AddIn AddIn
        {
            get
            {
                return this.addIn;
            }
        }

        public List<Codon> Codons
        {
            get
            {
                return this.codons;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

