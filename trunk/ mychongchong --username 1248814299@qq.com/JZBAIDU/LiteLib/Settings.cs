namespace LiteLib
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;

    public class Settings
    {
        private static XmlNode GetCtrlNode(Control ctrl)
        {
            string fileName = null;
            XmlDocument doc = null;
            return GetCtrlNode(ctrl, ref fileName, ref doc);
        }

        private static XmlNode GetCtrlNode(Control ctrl, ref string fileName, ref XmlDocument doc)
        {
            fileName = string.Concat(new object[] { Application.StartupPath, '\\', (string) ctrl.Tag, ".xml" });
            doc = new XmlDocument();
            XmlNode newChild = null;
            string xpath = ctrl.FindForm().Name + '-' + ctrl.Name;
            if (File.Exists(fileName))
            {
                doc.Load(fileName);
                newChild = doc.DocumentElement.SelectSingleNode(xpath);
            }
            else
            {
                doc.LoadXml("<Settings>\r\n</Settings>");
            }
            if (newChild == null)
            {
                newChild = doc.CreateNode(XmlNodeType.Element, xpath, "");
                doc.DocumentElement.AppendChild(newChild);
            }
            return newChild;
        }

        public static string GetValue(string name)
        {
            return (string) Application.UserAppDataRegistry.GetValue(name);
        }

        public static void GetValue(Control ctrl)
        {
            string name = ctrl.FindForm().Name + '-' + ctrl.Name;
            if ((ctrl.Tag != null) && (ctrl.Tag.ToString().Trim().Length > 0))
            {
                XmlNode ctrlNode;
                switch (ctrl.GetType().Name)
                {
                    case "TextBox":
                        ctrl.Text = GetValue((string) ctrl.Tag, ctrl.Text);
                        break;

                    case "CheckBox":
                    {
                        CheckBox box = (CheckBox) ctrl;
                        box.Checked = GetValue((string) ctrl.Tag, box.Checked);
                        break;
                    }
                    case "RadioButton":
                    {
                        RadioButton button = (RadioButton) ctrl;
                        button.Checked = GetValue((string) ctrl.Tag, button.Checked);
                        break;
                    }
                    case "NumericUpDown":
                    {
                        NumericUpDown down = (NumericUpDown) ctrl;
                        down.Value = decimal.Parse(GetValue((string) ctrl.Tag, down.Value.ToString()));
                        break;
                    }
                    case "TabControl":
                    {
                        TabControl control = (TabControl) ctrl;
                        control.SelectedIndex = int.Parse(GetValue((string) ctrl.Tag, control.SelectedIndex.ToString()));
                        break;
                    }
                    case "ComboBox":
                    {
                        ComboBox box2 = (ComboBox) ctrl;
                        try
                        {
                            ctrlNode = GetCtrlNode(ctrl);
                            if ((ctrlNode != null) && (ctrlNode.ChildNodes.Count > 0))
                            {
                                box2.Items.Clear();
                                foreach (XmlNode node2 in ctrlNode.ChildNodes)
                                {
                                    box2.Items.Add(node2.InnerText);
                                }
                            }
                        }
                        catch (XmlException)
                        {
                        }
                        box2.Text = GetValue(name, box2.Text);
                        break;
                    }
                    case "ListView":
                    {
                        ListView view = (ListView) ctrl;
                        try
                        {
                            ctrlNode = GetCtrlNode(ctrl);
                            if ((ctrlNode != null) && (ctrlNode.ChildNodes.Count > 0))
                            {
                                view.Items.Clear();
                                foreach (XmlNode node2 in ctrlNode.ChildNodes)
                                {
                                    string[] strArray = node2.InnerText.Split(new char[] { '\t' });
                                    ListViewItem item = view.Items.Add(strArray[0]);
                                    for (int i = 1; i < strArray.Length; i++)
                                    {
                                        item.SubItems.Add(strArray[i]);
                                    }
                                    if (view.CheckBoxes)
                                    {
                                        XmlAttribute attribute = node2.Attributes["Checked"];
                                        if (attribute != null)
                                        {
                                            item.Checked = attribute.Value.ToLower() == "true";
                                        }
                                    }
                                }
                            }
                        }
                        catch (XmlException)
                        {
                        }
                        break;
                    }
                }
            }
        }

        public static void GetValue(Form form)
        {
            Control ctl = null;
            while ((ctl = form.GetNextControl(ctl, true)) != null)
            {
                GetValue(ctl);
            }
        }

        public static bool GetValue(string name, bool defaultValue)
        {
            return (GetValue(name, defaultValue.ToString()).ToLower() == "true");
        }

        public static int GetValue(string name, int defaultValue)
        {
            string s = GetValue(name, defaultValue.ToString());
            if (s != "")
            {
                return int.Parse(s);
            }
            return -1;
        }

        public static string GetValue(string name, string defaultValue)
        {
            return (string) Application.UserAppDataRegistry.GetValue(name, defaultValue);
        }

        public static void SetValue(Control ctrl)
        {
            string name = ctrl.FindForm().Name + '-' + ctrl.Name;
            if ((ctrl.Tag != null) && (ctrl.Tag.ToString().Trim().Length > 0))
            {
                string str2;
                XmlDocument document;
                XmlNode node;
                XmlNode node2;
                switch (ctrl.GetType().Name)
                {
                    case "TextBox":
                        SetValue((string) ctrl.Tag, ctrl.Text);
                        break;

                    case "CheckBox":
                    {
                        CheckBox box = (CheckBox) ctrl;
                        SetValue((string) ctrl.Tag, box.Checked);
                        break;
                    }
                    case "RadioButton":
                    {
                        RadioButton button = (RadioButton) ctrl;
                        SetValue((string) ctrl.Tag, button.Checked);
                        break;
                    }
                    case "NumericUpDown":
                    {
                        NumericUpDown down = (NumericUpDown) ctrl;
                        SetValue((string) ctrl.Tag, down.Value.ToString());
                        break;
                    }
                    case "TabControl":
                    {
                        TabControl control = (TabControl) ctrl;
                        SetValue((string) ctrl.Tag, control.SelectedIndex.ToString());
                        break;
                    }
                    case "ComboBox":
                    {
                        ComboBox box2 = (ComboBox) ctrl;
                        try
                        {
                            str2 = null;
                            document = null;
                            node = GetCtrlNode(ctrl, ref str2, ref document);
                            node.RemoveAll();
                            foreach (string str3 in box2.Items)
                            {
                                node2 = document.CreateNode(XmlNodeType.Element, "Item", "");
                                node2.InnerText = str3;
                                node.AppendChild(node2);
                            }
                            document.Save(str2);
                        }
                        catch (XmlException)
                        {
                        }
                        SetValue(name, box2.Text);
                        break;
                    }
                    case "ListView":
                    {
                        ListView view = (ListView) ctrl;
                        try
                        {
                            str2 = null;
                            document = null;
                            node = GetCtrlNode(ctrl, ref str2, ref document);
                            node.RemoveAll();
                            foreach (ListViewItem item in view.Items)
                            {
                                node2 = document.CreateNode(XmlNodeType.Element, "Item", "");
                                if (view.CheckBoxes)
                                {
                                    XmlAttribute attribute = document.CreateAttribute("Checked");
                                    attribute.Value = item.Checked.ToString();
                                    node2.Attributes.Append(attribute);
                                }
                                string str4 = "";
                                foreach (ListViewItem.ListViewSubItem item2 in item.SubItems)
                                {
                                    str4 = str4 + item2.Text + '\t';
                                }
                                node2.InnerText = str4.TrimEnd(new char[] { '\t' });
                                node.AppendChild(node2);
                            }
                            document.Save(str2);
                        }
                        catch (XmlException)
                        {
                        }
                        break;
                    }
                }
            }
        }

        public static void SetValue(Form form)
        {
            Control ctl = null;
            while ((ctl = form.GetNextControl(ctl, true)) != null)
            {
                SetValue(ctl);
            }
        }

        public static void SetValue(string name, bool value)
        {
            SetValue(name, value.ToString());
        }

        public static void SetValue(string name, string value)
        {
            Application.UserAppDataRegistry.SetValue(name, value);
        }

        public string this[string name]
        {
            get
            {
                try
                {
                    return (string) Application.UserAppDataRegistry.GetValue(name);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    Application.UserAppDataRegistry.SetValue(name, value);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

