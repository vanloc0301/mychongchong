namespace SkyMap.Net.Tools.Nodes
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;

    public class DAODataSetNode : AbstractDomainObjectNode<DAODataSet>
    {
        public override ObjectNode AddChild()
        {
            DAODataTable item = new DAODataTable();
            item.Id = StringHelper.GetNewGuid();
            item.Name = "NetDataTable";
            item.Description = "新数据表";
            item.TempletDataSet = base.DomainObject;
            item.Save();
            base.DomainObject.DAODataTables.Add(item);
            return base.AddSingleNode<DAODataTable, DAODataTableNode>(item);
        }

        protected override void Initialize()
        {
            if (base.DomainObject.DAODataTables != null)
            {
                base.AddNodes<DAODataTable, DAODataTableNode>(new List<DAODataTable>(base.DomainObject.DAODataTables));
            }
        }
    }
}

