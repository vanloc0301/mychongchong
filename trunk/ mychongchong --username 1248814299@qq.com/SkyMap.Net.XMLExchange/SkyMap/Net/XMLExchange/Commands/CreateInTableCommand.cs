namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Data.Common;

    public class CreateInTableCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is DTTableNode)
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
                        command.CommandText = string.Format("select * into IN_{0} from {0} where 1<>1;\r\nalter table IN_{0} alter column ywid varchar(20) not null;\r\nalter table IN_{0} add CONSTRAINT [PK_IN_{0}] PRIMARY KEY CLUSTERED ([ywid])", domainObject.Name);
                        command.ExecuteNonQuery();
                        MessageHelper.ShowInfo("创建表'IN_{0}'成功", domainObject.Name);
                    }
                }
            }
        }
    }
}

