namespace SkyMap.Net.Tools.Criteria
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Gui;
    using System;

    public class TyQueryNode : AbstractDomainObjectNode<TyQuery>
    {
        public override ObjectNode AddChild()
        {
            TyQueryWhere where = new TyQueryWhere();
            where.Id = StringHelper.GetNewGuid();
            where.ColumnType = ColumnType.字符串;
            where.Name = "新条件字段";
            where.Description = where.Name;
            where.TableName = "表名";
            where.DisplayOrder = base.DomainObject.QueryWheres.Count + 1;
            where.TyQuery = base.DomainObject;
            where.Save();
            return base.AddSingleNode<TyQueryWhere, TyQueryWhereNode>(where);
        }

        protected override void Initialize()
        {
            base.AddNodes<TyQueryWhere, TyQueryWhereNode>(base.DomainObject.QueryWheres);
        }
    }
}

