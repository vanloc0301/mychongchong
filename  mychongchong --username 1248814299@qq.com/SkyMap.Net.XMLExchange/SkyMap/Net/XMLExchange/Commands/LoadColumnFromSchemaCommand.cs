namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Windows.Forms;
    using System.Xml.Schema;

    public class LoadColumnFromSchemaCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Title = "选择...";
            dialog.Filter = "schema 文件 (*.xsd)|*.xsd";
            int num = 0;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DTTableNode owner = this.Owner as DTTableNode;
                DTTable domainObject = owner.DomainObject;
                string targetNamespace = string.Format("{0}{1}/{2}", "http://www.eAppxml.gov.cn/", domainObject.DTDatabase.DTProject.MapXMLElementName.Substring(0, 2).ToLower(), domainObject.DTDatabase.DTProject.MapXMLElementName.ToLower());
                XmlSchemaSet set = new XmlSchemaSet();
                set.Add(targetNamespace, dialog.FileName);
                set.Compile();
                string str2 = string.Empty;
                foreach (XmlSchema schema in set.Schemas(targetNamespace))
                {
                    foreach (XmlSchemaElement element in schema.Elements.Values)
                    {
                        LoggingService.InfoFormatted("XmlSchemaElement.Name=>{0}", new object[] { element.Name });
                        if (element.Name == domainObject.MapXMLElementName.Replace("{0}:", string.Empty))
                        {
                            LoggingService.InfoFormatted("ElementSchemaType is {0},{1}", new object[] { element.ElementSchemaType.Name, element.ElementSchemaType.TypeCode });
                            XmlSchemaComplexType elementSchemaType = element.ElementSchemaType as XmlSchemaComplexType;
                            XmlSchemaSequence contentTypeParticle = elementSchemaType.ContentTypeParticle as XmlSchemaSequence;
                            UnitOfWork work = new UnitOfWork(typeof(DTColumn));
                            foreach (XmlSchemaElement element2 in contentTypeParticle.Items)
                            {
                                string name = string.Empty;
                                string description = string.Empty;
                                try
                                {
                                    name = ((XmlSchemaAttribute) ((XmlSchemaSimpleContentExtension) ((XmlSchemaSimpleContent) ((XmlSchemaComplexType) element2.SchemaType).ContentModel).Content).Attributes[0]).Name;
                                    if (element2.Annotation.Items.Count > 0)
                                    {
                                        description = ((XmlSchemaDocumentation) element2.Annotation.Items[0]).Markup[0].Value;
                                    }
                                    else
                                    {
                                        str2 = str2 + string.Format("列:＇{0}＇没有备注字段,请注意调整设置", name);
                                        description = name;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    LoggingService.Error(exception);
                                    str2 = str2 + exception.Message + "\r\n";
                                }
                                if (!string.IsNullOrEmpty(name))
                                {
                                    DTColumn column = new DTColumn(domainObject.NamespacePrefix, domainObject.NamespaceUri, domainObject, name, description, num * 10, string.Empty, "{0}:" + element2.Name, typeof(string).FullName);
                                    work.RegisterNew(column);
                                    owner.AddDTColumnNode(column);
                                    num++;
                                }
                            }
                            work.Commit();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    LoggingService.WarnFormatted("加载完成{0}列，但有错误:\r\n{1}", new object[] { num, str2 });
                    MessageBox.Show(string.Format("加载完成{0}列，但有错误:\r\n{1}", num, str2));
                }
                else
                {
                    MessageBox.Show(string.Format("从SCHEMA中成功添加了'{0}'列", num));
                }
            }
        }
    }
}

