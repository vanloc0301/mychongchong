namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Tools.Criteria;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using SkyMap.Net.Criteria;

    public class LoadDbColumnsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load Data Column ...");
            }
            TySearchtableNode owner = (TySearchtableNode) this.Owner;
            if (owner != null)
            {
                TySearchXNode parent = (TySearchXNode) owner.Parent;
                if (parent.DomainObject.DataSource != null)
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(parent.DomainObject.DataSource.DSType);
                    DataTable schemaTable = null;
                    using (DbConnection connection = factory.CreateConnection())
                    {
                        connection.ConnectionString = parent.DomainObject.DataSource.DecryptConnectionString;
                        connection.Open();
                        DbCommand command = connection.CreateCommand();
                        command.CommandText = string.Format("select * from {0} where 1<>1", owner.DomainObject.TableName);
                        DbDataReader reader = command.ExecuteReader();
                        schemaTable = reader.GetSchemaTable();
                        reader.Close();
                    }
                    if (schemaTable != null)
                    {
                        List<TyFieldsSelect> objs = RemotingSingletonProvider<CriteriaDAOService>.Instance.CreateFieldsFromSchemaTable(schemaTable, parent.DomainObject, owner.DomainObject.TableName);
                        owner.AddNodes<TyFieldsSelect, TyFieldsSelectNode>(objs);
                    }
                }
            }
        }
    }
}

