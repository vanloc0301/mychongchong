namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Data;
    using System.Data.Common;

    public class LoadFromDbCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            DTTableNode owner = this.Owner as DTTableNode;
            DTTable domainObject = owner.DomainObject;
            DbProviderFactory factory = DbProviderFactories.GetFactory(domainObject.DTDatabase.DSType);
            if (factory != null)
            {
                using (DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = domainObject.DTDatabase.DecryptConnectionString;
                    connection.Open();
                    DbCommand command = connection.CreateCommand();
                    command.CommandText = string.Format("select * from {0} where 1<>1", domainObject.Name);
                    DbDataReader reader = command.ExecuteReader();
                    DataTable schemaTable = null;
                    try
                    {
                        schemaTable = reader.GetSchemaTable();
                    }
                    finally
                    {
                        reader.Close();
                    }
                    owner.Expand();
                    UnitOfWork work = new UnitOfWork(typeof(DTColumn));
                    int num = 0;
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        DTColumn dtColumn = new DTColumn();
                        dtColumn.DTTable = domainObject;
                        dtColumn.Name = row["ColumnName"].ToString();
                        dtColumn.Type = row["DataType"].ToString();
                        dtColumn.MapXMLElementName = dtColumn.Name;
                        dtColumn.DisplayOrder = num++;
                        owner.AddDTColumnNode(dtColumn);
                        work.RegisterNew(dtColumn);
                    }
                    work.Commit();
                }
            }
        }
    }
}

