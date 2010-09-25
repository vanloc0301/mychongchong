namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;

    public class BatchModifyNamespaceCommand : AbstractCommand
    {
        public override void Run()
        {
            if (this.Owner is DTProjectNode)
            {
                DTProject domainObject = ((DTProjectNode) this.Owner).DomainObject;
                if (domainObject != null)
                {
                    int num = 0;
                    UnitOfWork work = new UnitOfWork(typeof(DTColumn));
                    foreach (DTDatabase database in domainObject.DTDatabases)
                    {
                        foreach (DTTable table in database.DTTables)
                        {
                            if (!string.IsNullOrEmpty(table.NamespacePrefix))
                            {
                                table.MapXMLElementName = table.MapXMLElementName.Replace(table.NamespacePrefix + ":", "{0}:");
                                table.NamespacePrefix = "{0}";
                                table.NamespaceUri = "{0}";
                                work.RegisterDirty(table);
                                num++;
                            }
                            foreach (DTColumn column in table.DTColumns)
                            {
                                if (!string.IsNullOrEmpty(column.NamespacePrefix))
                                {
                                    column.MapXMLElementName = column.MapXMLElementName.Replace(column.NamespacePrefix + ":", "{0}:");
                                    column.NamespacePrefix = "{0}";
                                    column.NamespaceUri = "{0}";
                                    work.RegisterDirty(column);
                                    num++;
                                }
                            }
                        }
                    }
                    work.Commit();
                    MessageHelper.ShowInfo("批处理修改了'{0}'项", num.ToString());
                }
            }
        }
    }
}

