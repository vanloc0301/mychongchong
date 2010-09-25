namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;

    public class DAOCache : AbstractDAOCache
    {
        public override void Put()
        {
            base.PutTypeToCache<DAODataForm>(null);
            base.PutTypeToCache<DAODataSet>(new Action<IEnumerable<DAODataSet>>(this.PutRelDataForm));
            Action<IEnumerable<PrintSet>> action = delegate (IEnumerable<PrintSet> pss) {
                foreach (PrintSet set in pss)
                {
                    base.AddOneByOneToCache<TempletPrint>(set.TempletPrints, null);
                }
            };
            base.PutTypeToCache<PrintSet>(action);
            base.PutTypeToCache<ParticipantFormPermission>(null);
        }

        private void PutRelDataForm(IEnumerable<DAODataSet> dds)
        {
            foreach (DAODataSet set in dds)
            {
                base.AddOneByOneToCache<DAODataTable>(set.DAODataTables, null);
            }
        }
    }
}

