namespace SkyMap.Net.XMLExchange.GUI.Nodes
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Windows.Forms;

    public class DTTableNode : AbstractDomainObjectNode<DTTable>
    {
        public override ObjectNode AddChild()
        {
            DTColumn dtColumn = new DTColumn(base.DomainObject, "新列");
            dtColumn.Save();
            return this.AddDTColumnNode(dtColumn);
        }

        public DTColumnNode AddDTColumnNode(DTColumn dtColumn)
        {
            return base.AddSingleNode<DTColumn, DTColumnNode>(dtColumn);
        }

        public override void Delete()
        {
            base.Delete();
            base.DomainObject.DTDatabase.DTTables.Remove(base.DomainObject);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (base.DomainObject != null)
            {
                base.Nodes.Clear();
                foreach (DTColumn column in base.DomainObject.DTColumns)
                {
                    this.AddDTColumnNode(column);
                }
            }
        }

        public override void Paste()
        {
            LoggingService.Info("Ready to paste...");
            IDataObject dataObject = null;
            try
            {
                LoggingService.Info("get clipbord data object...");
                dataObject = ClipboardWrapper.GetDataObject();
                LoggingService.Info("finish get data object");
            }
            catch
            {
            }
            if (dataObject != null)
            {
                LoggingService.Info("准备检查剪切板里的对象是否可粘贴...");
                if (dataObject.GetDataPresent(typeof(DTColumn)))
                {
                    LoggingService.Info("剪切板里的对象是可以粘贴的...");
                    DTColumn data = (DTColumn) dataObject.GetData(typeof(DTColumn));
                    if (data == null)
                    {
                        LoggingService.Info("剪切板里的对象是空对象");
                    }
                    DTColumn dtColumn = data.Clone(base.DomainObject);
                    dtColumn.Save();
                    this.AddDTColumnNode(dtColumn);
                }
            }
        }

        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/DTTableContextMenu";
            }
            set
            {
            }
        }
    }
}

