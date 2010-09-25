namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;

    public class BatchAddNamespaceCommand : AbstractCommand
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
                            table.MapXMLElementName = "{0}:" + table.MapXMLElementName;
                            table.NamespacePrefix = "{0}";
                            table.NamespaceUri = "{0}";
                            work.RegisterDirty(table);
                            num++;
                            foreach (DTColumn column in table.DTColumns)
                            {
                                column.MapXMLElementName = "{0}:" + column.MapXMLElementName;
                                column.NamespacePrefix = "{0}";
                                column.NamespaceUri = "{0}";
                                work.RegisterDirty(column);
                                num++;
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

