namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Windows.Forms;
    using SkyMap.Net.XMLExchange.GUI.Nodes;

    public class PasteDTProjectCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            if (dataObject.GetDataPresent(typeof(DTProject)))
            {
                DTProject data = (DTProject) dataObject.GetData(typeof(DTProject));
                if (data != null)
                {
                    DTProject dtProject = new DTProject(data.Name, data.Description, data.MapXMLElementName);
                    dtProject.Save();
                    foreach (DTDatabase database in data.DTDatabases)
                    {
                        DTDatabase dtDatabase = new DTDatabase(dtProject, database.Name, database.Description, database.DSType, database.ConnectionString, database.BeforeProcedure, database.AfterProcedure);
                        dtDatabase.Save();
                        foreach (DTTable table in database.DTTables)
                        {
                            DTTable dtTable = new DTTable(table.NamespacePrefix, table.NamespaceUri, dtDatabase, table.Name, table.Description, table.MapXMLElementName, table.Hiberarchy, table.SourceType);
                            dtTable.Save();
                            foreach (DTColumn column in table.DTColumns)
                            {
                                new DTColumn(column.NamespacePrefix, column.NamespaceUri, dtTable, column.Name, column.Description, column.DisplayOrder, column.DefaultValue, column.MapXMLElementName, column.Type).Save();
                            }
                        }
                    }
                    ((TaskTreeNode) this.Owner).AddSingleNode<DTProject, DTProjectNode>(dtProject);
                }
            }
        }
    }
}

