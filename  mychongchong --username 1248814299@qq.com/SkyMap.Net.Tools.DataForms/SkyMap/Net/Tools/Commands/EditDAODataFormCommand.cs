namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Commands;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Tools.Nodes;
    using SkyMap.Net.Tools.DataForms;

    public class EditDAODataFormCommand : ShowEditViewCommand<DataFormViewContent, DAODataForm>
    {
        protected override DAODataForm DomainObject
        {
            get
            {
                DAODataFormNode owner = (DAODataFormNode) this.Owner;
                if (owner != null)
                {
                    return owner.DomainObject;
                }
                return null;
            }
        }
    }
}

