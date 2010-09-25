namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class DataFormDAOService : MarshalByRefObject
    {
        private static DataFormDAOService instance;

        public ParticipantFormPermission CreateParticipantFormPermission()
        {
            ParticipantFormPermission permission = new ParticipantFormPermission {
                Id = StringHelper.GetNewGuid()
            };
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(permission);
            currentUnitOfWork.Commit();
            return permission;
        }

        private UnitOfWork CurrentUnitOfWork
        {
            get
            {
                return new UnitOfWork(base.GetType());
            }
        }

        public static DataFormDAOService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataFormDAOService();
                }
                return instance;
            }
        }

        public IList<ParticipantFormPermission> ParticipantFormPerms
        {
            get
            {
                return QueryHelper.List<ParticipantFormPermission>("ALL_ParticipantFormPermission");
            }
        }

        public IList<PrintSet> PrintSets
        {
            get
            {
                return QueryHelper.List<PrintSet>("ALL_PrintSet");
            }
        }
    }
}

