namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Tools.Criteria;

    public class AddTyQueryCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                TyQuery query = new TyQuery();
                query.Id = StringHelper.GetNewGuid();
                query.Name = "新查询";
                query.DisplayOrder = owner.Nodes.Count + 1;
                query.Sql = "select * from 表名 where {0}";
                query.QueryWheres = new List<TyQueryWhere>();
                query.Save();
                owner.AddSingleNode<TyQuery, TyQueryNode>(query);
            }
        }
    }
}

