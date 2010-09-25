namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Tools.Nodes;

    public class AddDAODataSetCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                DAODataSet set = new DAODataSet();
                set.Id = StringHelper.GetNewGuid();
                set.Name = "新数据集";
                set.Description = "新数据集";
                set.DAODataTables = new List<DAODataTable>();
                set.Save();
                owner.AddSingleNode<DAODataSet, DAODataSetNode>(set);
            }
        }
    }
}

