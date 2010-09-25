namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Data.Common;

    public class AlterTableAddOnlineIDColumnCommand : AbstractMenuCommand
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
                        command.CommandText = string.Format("alter table {0} add online_id varchar(50) null", domainObject.Name);
                        command.ExecuteNonQuery();
                        MessageHelper.ShowInfo("添加列'online_id'成功");
                    }
                }
            }
        }
    }
}

