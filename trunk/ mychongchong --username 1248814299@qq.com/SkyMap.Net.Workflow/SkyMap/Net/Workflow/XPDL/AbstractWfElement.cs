namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public abstract class AbstractWfElement : DomainObject, WfElement
    {
        protected AbstractWfElement()
        {
        }
    }
}

