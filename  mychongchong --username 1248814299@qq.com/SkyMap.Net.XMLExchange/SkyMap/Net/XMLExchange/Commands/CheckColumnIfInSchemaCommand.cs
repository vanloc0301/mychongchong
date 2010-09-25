namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Windows.Forms;
    using System.Xml.Schema;

    public class CheckColumnIfInSchemaCommand : AbstractMenuCommand
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
                LoggingService.Info("开始检查...");
                foreach (XmlSchema schema in set.Schemas(targetNamespace))
                {
                    foreach (XmlSchemaElement element in schema.Elements.Values)
                    {
                        LoggingService.InfoFormatted("XmlSchemaElement.Name=>{0}", new object[] { element.Name });
                        if (element.Name == domainObject.MapXMLElementName.Replace("{0}:", string.Empty))
                        {
                            XmlSchemaComplexType elementSchemaType = element.ElementSchemaType as XmlSchemaComplexType;
                            XmlSchemaSequence contentTypeParticle = elementSchemaType.ContentTypeParticle as XmlSchemaSequence;
                            UnitOfWork work = new UnitOfWork(typeof(DTColumn));
                            foreach (DTColumn column in domainObject.DTColumns)
                            {
                                bool flag = false;
                                LoggingService.InfoFormatted("验证列'{0}'是否需要导出...", new object[] { column.Name });
                                foreach (XmlSchemaElement element2 in contentTypeParticle.Items)
                                {
                                    if (element2.Name == column.MapXMLElementName.Replace("{0}:", string.Empty))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                LoggingService.InfoFormatted("列'{0}'{1}需要导出...", new object[] { column.Name, flag ? "是" : "不" });
                                if (flag && !column.IsActive)
                                {
                                    column.IsActive = true;
                                    column.IfExport = flag;
                                    work.RegisterDirty(column);
                                    num++;
                                }
                                else if (column.IfExport != flag)
                                {
                                    num++;
                                    column.IfExport = flag;
                                    work.RegisterDirty(column);
                                }
                            }
                            LoggingService.Info("提交数据库修改...");
                            work.Commit();
                            foreach (XmlSchemaElement element3 in contentTypeParticle.Items)
                            {
                                bool flag2 = false;
                                LoggingService.InfoFormatted("验证节点'{0}'是否存在...", new object[] { element3.Name });
                                foreach (DTColumn column2 in domainObject.DTColumns)
                                {
                                    LoggingService.DebugFormatted("节点'{0}'的首选属性名称是{1}", new object[] { element3.Name, ((XmlSchemaAttribute) ((XmlSchemaSimpleContentExtension) ((XmlSchemaSimpleContent) ((XmlSchemaComplexType) element3.SchemaType).ContentModel).Content).Attributes[0]).Name });
                                    if (element3.Name == column2.MapXMLElementName.Replace("{0}:", string.Empty))
                                    {
                                        flag2 = true;
                                        break;
                                    }
                                }
                                LoggingService.InfoFormatted("节点'{0}'在表中{1}存在...", new object[] { element3.Name, flag2 ? "" : "不" });
                                if (!flag2)
                                {
                                    str2 = str2 + string.Format("{0}({1})", element3.Name, element3.Annotation);
                                }
                            }
                            continue;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    LoggingService.Warn("在SCHEMA有定义,但没有出现的节点有:\r\n" + str2);
                    MessageBox.Show("在SCHEMA有定义,但没有出现的节点有:\r\n" + str2);
                }
                else
                {
                    MessageBox.Show(string.Format("成功更新了'{0}'列的是否需要导出属性", num));
                }
            }
        }
    }
}

