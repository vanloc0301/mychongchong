namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.XMLExchange.GUI.Nodes;
    using SkyMap.Net.XMLExchange.Model;
    using System;

    public class RemoveAllColumnsInTableCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            DTTableNode owner = this.Owner as DTTableNode;
            DTTable domainObject = owner.DomainObject;
            if (domainObject.DTColumns.Count > 0)
            {
                UnitOfWork work = new UnitOfWork(typeof(DTColumn));
                foreach (DTColumn column in domainObject.DTColumns)
                {
                    work.RegisterRemoved(column);
                }
                work.Commit();
                domainObject.DTColumns.Clear();
                owner.Nodes.Clear();
            }
        }
    }
}

