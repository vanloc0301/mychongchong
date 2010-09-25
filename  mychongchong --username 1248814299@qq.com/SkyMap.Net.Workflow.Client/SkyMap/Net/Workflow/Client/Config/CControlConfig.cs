namespace SkyMap.Net.Workflow.Client.Config
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    public static class CControlConfig
    {
        private static XmlDocument _xmlDoc = new XmlDocument();
        private const string controlConfigFile = "ControlConfig.xml";

        static CControlConfig()
        {
            string path = string.Concat(new object[] { PropertyService.DataDirectory, Path.DirectorySeparatorChar, "Workflow", Path.DirectorySeparatorChar, "ControlConfig.xml" });
            if (File.Exists(path))
            {
                _xmlDoc.Load(path);
            }
            else
            {
                Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Resources.ControlConfig.xml");
                if (manifestResourceStream == null)
                {
                    throw new WfClientException("The control config cannot be null");
                }
                _xmlDoc.Load(manifestResourceStream);
            }
        }

        public static CBoxConfig GetBoxConfig(string boxName)
        {
            lock (_xmlDoc)
            {
                XmlNodeList list = _xmlDoc.SelectNodes("/controlConfig/tlResult/box");
                foreach (XmlElement element in list)
                {
                    if (element.GetAttribute("name") == boxName)
                    {
                        CBoxConfig config = new CBoxConfig();
                        config.Name = boxName;
                        foreach (XmlAttribute attribute in element.Attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "class":
                                    config.Class = attribute.Value;
                                    break;

                                case "queryName":
                                    config.QueryName = attribute.Value;
                                    break;

                                case "idField":
                                    config.IdField = attribute.Value;
                                    break;

                                case "queryParameters":
                                    config.QueryParameters = StringHelper.Split(attribute.Value);
                                    break;

                                case "DAONameSpace":
                                    config.DAONameSpace = attribute.Value;
                                    break;

                                case "OpenViewCommand":
                                    config.OpenViewCommand = attribute.Value;
                                    break;

                                case "queryCountName":
                                    config.QueryCountName = attribute.Value;
                                    break;

                                case "toolbarPath":
                                    config.ToolbarPath = attribute.Value;
                                    break;
                            }
                        }
                        CColConfig col = null;
                        CMenuItemConfig menuItem = null;
                        XmlElement element2 = null;
                        foreach (XmlNode node in element.ChildNodes)
                        {
                            if (node is XmlElement)
                            {
                                element2 = node as XmlElement;
                                if (element2.Name.Equals("col"))
                                {
                                    col = new CColConfig();
                                    col.Caption = element2.GetAttribute("caption");
                                    col.FieldName = element2.GetAttribute("fieldName");
                                    col.VisibleIndex = Convert.ToInt32(element2.GetAttribute("visibleIndex"));
                                    col.FormatType = element2.GetAttribute("formatType");
                                    col.FormatString = element2.GetAttribute("formatString");
                                    if (element2.HasAttribute("width"))
                                    {
                                        col.Width = Convert.ToInt32(element2.GetAttribute("width"));
                                    }
                                    config.AddCol(col);
                                }
                                else if (element2.Name.Equals("menuItem"))
                                {
                                    menuItem = new CMenuItemConfig();
                                    menuItem.Text = element2.GetAttribute("text");
                                    menuItem.InvokeName = element2.GetAttribute("invokeName");
                                    string str = element2.GetAttribute("enableOnSelect");
                                    if (str != null)
                                    {
                                        menuItem.EnableOnSelect = Convert.ToBoolean(str);
                                    }
                                    menuItem.Access = element2.GetAttribute("acess");
                                    config.AddMenuItem(menuItem);
                                }
                            }
                        }
                        return config;
                    }
                }
            }
            throw new WfClientException("Cannot find configuration the box of " + boxName);
        }

        public static IList<CNavBarGroupConfig> GetNavBarGroups()
        {
            List<CNavBarGroupConfig> list = new List<CNavBarGroupConfig>();
            lock (_xmlDoc)
            {
                XmlNodeList list2 = _xmlDoc.SelectNodes("/controlConfig/navBar/group");
                foreach (XmlElement element in list2)
                {
                    CNavBarGroupConfig item = new CNavBarGroupConfig();
                    item.Caption = element.GetAttribute("caption");
                    item.Image = element.GetAttribute("image");
                    item.BackgroundImage = element.GetAttribute("backgroundImage");
                    item.StyleBackgroundName = element.GetAttribute("styleBackgroundName");
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node is XmlElement)
                        {
                            XmlElement element2 = node as XmlElement;
                            if (element2.Name == "item")
                            {
                                CNavBarItemConfig config2 = new CNavBarItemConfig();
                                foreach (XmlAttribute attribute in element2.Attributes)
                                {
                                    string name = attribute.Name;
                                    if (name != null)
                                    {
                                        if (!(name == "caption"))
                                        {
                                            if (name == "class")
                                            {
                                                goto Label_0195;
                                            }
                                            if (name == "image")
                                            {
                                                goto Label_01A5;
                                            }
                                            if (name == "link")
                                            {
                                                goto Label_01B5;
                                            }
                                            if (name == "isDefault")
                                            {
                                                goto Label_01C5;
                                            }
                                            if (name == "ifAuth")
                                            {
                                                goto Label_01DA;
                                            }
                                        }
                                        else
                                        {
                                            config2.Caption = attribute.Value;
                                        }
                                    }
                                    goto Label_01EF;
                                Label_0195:
                                    config2.Class = attribute.Value;
                                    goto Label_01EF;
                                Label_01A5:
                                    config2.Image = attribute.Value;
                                    goto Label_01EF;
                                Label_01B5:
                                    config2.Link = attribute.Value;
                                    goto Label_01EF;
                                Label_01C5:
                                    bool.TryParse(attribute.Value, out config2.IsDefault);
                                    goto Label_01EF;
                                Label_01DA:
                                    bool.TryParse(attribute.Value, out config2.IfAuth);
                                Label_01EF:;
                                }
                                item.Items.Add(config2);
                            }
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }
    }
}

