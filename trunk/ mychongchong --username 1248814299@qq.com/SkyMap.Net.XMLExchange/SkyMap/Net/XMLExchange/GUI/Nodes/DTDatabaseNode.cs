namespace SkyMap.Net.XMLExchange.GUI.Nodes
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Windows.Forms;

    public class DTDatabaseNode : AbstractDomainObjectNode<DTDatabase>
    {
        public override ObjectNode AddChild()
        {
            UnitOfWork work = new UnitOfWork(typeof(DTColumn));
            DTTable table = new DTTable();
            table.Name = "新数据表";
            table.DTDatabase = base.DomainObject;
            work.RegisterNew(table);
            work.Commit();
            base.DomainObject.DTTables.Add(table);
            return this.AddDTTableNode(table);
        }

        private DTTableNode AddDTTableNode(DTTable dtTable)
        {
            return base.AddSingleNode<DTTable, DTTableNode>(dtTable);
        }

        public override void Delete()
        {
            base.Delete();
            base.DomainObject.DTProject.DTDatabases.Remove(base.DomainObject);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (base.DomainObject != null)
            {
                base.Nodes.Clear();
                foreach (DTTable table in base.DomainObject.DTTables)
                {
                    this.AddDTTableNode(table);
                }
            }
        }

        public override void Paste()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            if (dataObject.GetDataPresent(typeof(DTTable)))
            {
                DTTable data = (DTTable) dataObject.GetData(typeof(DTTable));
                if (data != null)
                {
                    DTTable dtTable = data.Clone(base.DomainObject);
                    dtTable.Save();
                    foreach (DTColumn column in data.DTColumns)
                    {
                        column.Clone(dtTable).Save();
                    }
                    this.AddDTTableNode(dtTable);
                }
            }
        }
    }
}

