namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Tools.Nodes;

    public class AddDAODataFormCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                DAODataForm form = new DAODataForm();
                form.Id = StringHelper.GetNewGuid();
                form.Name = "新数据表单";
                form.Description = "新数据表单";
                form.BindTables = new List<DAODataTable>();
                form.DataControls = new List<DataControl>();
                form.Save();
                owner.AddSingleNode<DAODataForm, DAODataFormNode>(form);
            }
        }
    }
}

