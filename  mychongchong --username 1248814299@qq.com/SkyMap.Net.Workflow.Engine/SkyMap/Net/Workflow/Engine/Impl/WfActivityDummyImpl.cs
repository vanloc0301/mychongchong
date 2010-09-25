namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    public class WfActivityDummyImpl : WfActivityAbstractImpl
    {
        public WfActivityDummyImpl(WfActivity wfactivity) : base(wfactivity)
        {
        }

        public override void Activate()
        {
            base.wfActivity.Complete();
        }

        private void ExcuteApplication()
        {
            Actdef defineOject = base.wfActivity.GetDefineOject() as Actdef;
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            Application application = QueryHelper.Get<Application>("Application_" + defineOject.ApplicationId, defineOject.ApplicationId);
            try
            {
                if (application != null)
                {
                    object obj2 = Activator.CreateInstance(application.Assembly, application.TypeClass).Unwrap();
                    obj2.GetType().GetMethod(application.Procedure).Invoke(obj2, null);
                }
            }
            catch (ApplicationException exception)
            {
                throw new WfException("Can not execute application : " + application.Name, exception);
            }
        }

        public override void Start()
        {
            this.ExcuteApplication();
            base.wfActivity.Activate();
        }
    }
}

