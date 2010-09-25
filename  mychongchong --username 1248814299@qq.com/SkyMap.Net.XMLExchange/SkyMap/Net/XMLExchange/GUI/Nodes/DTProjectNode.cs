namespace SkyMap.Net.XMLExchange.GUI.Nodes
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Windows.Forms;

    public class DTProjectNode : AbstractDomainObjectNode<DTProject>
    {
        public override ObjectNode AddChild()
        {
            UnitOfWork work = new UnitOfWork(typeof(DTDatabase));
            DTDatabase database = new DTDatabase();
            database.Name = "新数据库";
            database.DSType = "System.Data.SqlClient";
            database.DTProject = base.DomainObject;
            work.RegisterNew(database);
            work.Commit();
            base.DomainObject.DTDatabases.Add(database);
            return this.AddDTDatabaseNode(database);
        }

        private DTDatabaseNode AddDTDatabaseNode(DTDatabase dtDatabase)
        {
            return base.AddSingleNode<DTDatabase, DTDatabaseNode>(dtDatabase);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (base.DomainObject != null)
            {
                base.Nodes.Clear();
                foreach (DTDatabase database in base.DomainObject.DTDatabases)
                {
                    this.AddDTDatabaseNode(database);
                }
            }
        }

        public override void Paste()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            if (dataObject.GetDataPresent(typeof(DTDatabase)))
            {
                DTDatabase data = (DTDatabase) dataObject.GetData(typeof(DTDatabase));
                if (data != null)
                {
                    DTDatabase dtDatabase = data.Clone(base.DomainObject);
                    dtDatabase.Save();
                    foreach (DTTable table in data.DTTables)
                    {
                        DTTable dtTable = table.Clone(dtDatabase);
                        dtTable.Save();
                        foreach (DTColumn column in table.DTColumns)
                        {
                            column.Clone(dtTable).Save();
                        }
                    }
                    this.AddDTDatabaseNode(dtDatabase);
                }
            }
        }

        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/DTProjectContextMenu";
            }
            set
            {
            }
        }
    }
}

