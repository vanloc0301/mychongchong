namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;

    public class Condition : ICondition
    {
        private ConditionFailedAction action;
        private string name;
        private SkyMap.Net.Core.Properties properties;

        public Condition(string name, SkyMap.Net.Core.Properties properties)
        {
            this.name = name;
            this.properties = properties;
            this.action = properties.Get<ConditionFailedAction>("action", ConditionFailedAction.Exclude);
        }

        public bool IsValid(object caller, Codon codon)
        {
            bool flag;
            try
            {
                flag = AddInTree.ConditionEvaluators[this.name].IsValid(caller, this, codon);
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Condition evaluator " + this.name + " not found!");
            }
            return flag;
        }

        public static ICondition Read(XmlReader reader)
        {
            SkyMap.Net.Core.Properties properties = SkyMap.Net.Core.Properties.ReadFromAttributes(reader);
            return new Condition(properties["name"], properties);
        }

        public static ICondition ReadComplexCondition(XmlReader reader)
        {
            SkyMap.Net.Core.Properties properties = SkyMap.Net.Core.Properties.ReadFromAttributes(reader);
            reader.Read();
            ICondition condition = null;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    goto Label_008F;
                }
                string localName = reader.LocalName;
                if (localName == null)
                {
                    goto Label_008F;
                }
                if (!(localName == "And"))
                {
                    if (localName == "Or")
                    {
                        goto Label_0072;
                    }
                    if (localName == "Not")
                    {
                        goto Label_007B;
                    }
                    if (localName == "Condition")
                    {
                        goto Label_0084;
                    }
                    goto Label_008F;
                }
                condition = AndCondition.Read(reader);
                break;
            Label_0072:
                condition = OrCondition.Read(reader);
                break;
            Label_007B:
                condition = NegatedCondition.Read(reader);
                break;
            Label_0084:
                condition = Read(reader);
                break;
            Label_008F:;
            }
            if (condition != null)
            {
                ConditionFailedAction action = properties.Get<ConditionFailedAction>("action", ConditionFailedAction.Exclude);
                condition.Action = action;
            }
            return condition;
        }

        public static ICondition[] ReadConditionList(XmlReader reader, string endElement)
        {
            List<ICondition> list = new List<ICondition>();
            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if ((nodeType == XmlNodeType.EndElement) && (reader.LocalName == endElement))
                    {
                        return list.ToArray();
                    }
                }
                else
                {
                    string localName = reader.LocalName;
                    if (localName != null)
                    {
                        if (!(localName == "And"))
                        {
                            if (localName == "Or")
                            {
                                goto Label_009C;
                            }
                            if (localName == "Not")
                            {
                                goto Label_00AB;
                            }
                            if (localName == "Condition")
                            {
                                goto Label_00BA;
                            }
                        }
                        else
                        {
                            list.Add(AndCondition.Read(reader));
                        }
                    }
                }
                goto Label_00CB;
            Label_009C:
                list.Add(OrCondition.Read(reader));
                goto Label_00CB;
            Label_00AB:
                list.Add(NegatedCondition.Read(reader));
                goto Label_00CB;
            Label_00BA:
                list.Add(Read(reader));
            Label_00CB:;
            }
            return list.ToArray();
        }

        public ConditionFailedAction Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }

        public string this[string key]
        {
            get
            {
                return this.properties[key];
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public SkyMap.Net.Core.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }
    }
}

