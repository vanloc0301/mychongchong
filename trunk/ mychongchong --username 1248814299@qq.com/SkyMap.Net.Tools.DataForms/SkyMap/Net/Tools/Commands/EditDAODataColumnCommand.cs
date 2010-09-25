namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Commands;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Tools.Nodes;
    using SkyMap.Net.Tools.DataForms;

    public class EditDAODataColumnCommand : ShowEditViewCommand<DataSetViewContent, DAODataTable>
    {
        protected override DAODataTable DomainObject
        {
            get
            {
                DAODataTableNode owner = (DAODataTableNode) this.Owner;
                if (owner != null)
                {
                    return owner.DomainObject;
                }
                return null;
            }
        }
    }
}

