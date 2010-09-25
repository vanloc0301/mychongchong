namespace SkyMap.Net.Commands
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;

    public abstract class AbstractEditViewContent : AbstractViewContent
    {
        protected AbstractEditViewContent()
        {
        }

        public abstract void Load(DomainObject domainObject, UnitOfWork unitOfWork);
    }
}

