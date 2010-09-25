namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Serializable]
    public class WfProcess : WfExecutionOjbect, IWfProcess, IWfExecutionObject
    {
        private IWfProcessMgr manager;
        private Prodef prodef;
        private Proinst proinst;
        private IWfRequester requester;

        public WfProcess(Proinst proins) : base(proins)
        {
            this.proinst = proins;
            this.requester = WfFactory.GetWfRequester(proins.Requester);
        }

        public WfProcess(Prodef prodef, IWfProcessMgr processMgr) : base(new Proinst(), prodef)
        {
            this.manager = processMgr;
            this.proinst = base.wfInstance as Proinst;
            this.proinst.Responsible = prodef.ResponsibleId;
            this.proinst.DueTime = prodef.Duration;
            this.proinst.FormId = prodef.DaoDataFormId;
            this.proinst.TempletDataSetId = prodef.DaoDataSetId;
            this.proinst.Style = prodef.Style;
            this.proinst.ProjectId = this.GetNewProjectId(prodef.Package.Logo);
        }

        public override void Abort()
        {
            base.Abort();
            foreach (IWfActivity activity in this.GetListStep())
            {
                activity.Abort();
            }
        }

        public void ActivityComplete(IWfActivity activity)
        {
            if (activity.Type == ActdefType.COMPLETION)
            {
                this.Complete();
            }
            base.Store();
        }

        public override void ChangeStatus(WfStatusType newstatus)
        {
            IWfStateEventAudit audit = new WfStateEventAudit(this, this.proinst.Status, newstatus);
            this.requester.ReceiveEvent(audit);
            if (((base.WfState == WfStateType.Open) && (base.WhileOpen == WhileOpenType.NotRunning)) && (newstatus == WfStatusType.WF_RUNNING))
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("Started process:" + this.proinst.Id);
                }
                this.proinst.StartDate = new DateTime?(DateTimeHelper.GetNow());
            }
            base.ChangeStatus(newstatus);
        }

        private void Complete()
        {
            this.ChangeStatus(WfStatusType.WF_COMPLETED);
        }

        public IList<IWfActivity> GetActivitiesInState(string State)
        {
            return null;
        }

        public override object GetDefineOject()
        {
            if (this.prodef == null)
            {
                IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
                this.prodef = QueryHelper.Get<Prodef>("Prodef_" + this.proinst.ProdefId, this.proinst.ProdefId);
            }
            return this.prodef;
        }

        public IList<IWfActivity> GetListStep()
        {
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            IList<Actinst> actinsts = this.proinst.Actinsts;
            IList<IWfActivity> list2 = new List<IWfActivity>();
            if (actinsts != null)
            {
                foreach (Actinst actinst in actinsts)
                {
                    if (actinst.Type == ActdefType.INTERACTION)
                    {
                        list2.Add(WfFactory.GetWfActivity(actinst));
                    }
                }
            }
            return list2;
        }

        private string GetNewProjectId(string logo)
        {
            if (string.IsNullOrEmpty(logo) || logo.StartsWith("type:"))
            {
                throw new WfException("没有设定工作流的标值(Logo)");
            }
            string s = string.Empty;
            using (IDbConnection connection = DBSessionFactory.Instance.GetConnection("WF_SEQUENCE"))
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int year = DateTimeHelper.GetNow().Year;
                        string str2 = string.Format("insert into wf_sequence select '{0}',{1},1 where not exists(select 1 from wf_sequence where wf_sequence.PACKAGE_LOGO='{0}');\r\nupdate wf_sequence set Year_num = {1} , max_id=1 where package_logo ='{0}' and  Year_num != {1};\r\nupdate wf_sequence set Year_num = {1} , max_id=max_id+1 where package_logo ='{0}' and  Year_num = {1};\r\nselect max_id from wf_sequence where package_logo ='{0}' and Year_num = {1};", logo, year);
                        IDbCommand command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = str2;
                        if (LoggingService.IsDebugEnabled)
                        {
                            command.CommandType = CommandType.Text;
                        }
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                s = string.Format("{0}-{1}-{2}", logo, year, Convert.ToInt32(reader[0]).ToString("0000"));
                            }
                            LoggingService.Info(string.Format("{0}:{1} GetNewProjectId方法创建新的业务号:{2}", this.GetType().Assembly.ToString(), this.GetType().ToString(), s));
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        if (LoggingService.IsErrorEnabled)
                        {
                            LoggingService.Error(exception.Message + "\n" + exception.StackTrace);
                        }
                        throw new WfException("Get new project error", exception);
                    }
                }
            }
            if (StringHelper.IsNull(s))
            {
                LoggingService.Info("Cannot get new project id");
                throw new WfException("Cannot get new project id");
            }
            LoggingService.DebugFormatted("新的业务号是：{0}", new object[] { s });
            return s;
        }

        public int HowManyStep()
        {
            return this.GetListStep().Count;
        }

        public override void Resume()
        {
            base.Resume();
            foreach (IWfActivity activity in this.GetListStep())
            {
                activity.Resume();
            }
        }

        public void Start()
        {
            this.Start(null);
        }

        public void Start(string actdefId)
        {
            if (this.proinst.Status == WfStatusType.WF_RUNNING)
            {
                throw new AlreadyRunningException("Process is already running");
            }
            Actdef actdef = null;
            Prodef defineOject = this.GetDefineOject() as Prodef;
            if (actdefId == null)
            {
                if (defineOject == null)
                {
                    throw new WfException("Cannot get prodef object of :" + this.proinst.ProdefId);
                }
                foreach (Actdef actdef2 in defineOject.Actdefs.Values)
                {
                    if (((actdef2.Type == ActdefType.INITIAL) && actdef2.Status) && actdef2.IsDefaultInit)
                    {
                        actdef = actdef2;
                        break;
                    }
                }
            }
            else
            {
                actdef = defineOject.Actdefs[actdefId];
            }
            if (actdef == null)
            {
                throw new CannotStartException("No initial actdef available");
            }
            base.Store();
            this.StartActivity(actdef);
        }

        private void StartActivity(Actdef actdef)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Attemp to start actins:" + actdef.Name);
            }
            WfFactory.GetWfActivity(actdef, this).Start();
        }

        public override void Suspend()
        {
            base.Suspend();
            foreach (IWfActivity activity in this.GetListStep())
            {
                activity.Suspend();
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            foreach (IWfActivity activity in this.GetListStep())
            {
                activity.Terminate();
            }
        }

        public IWfProcessMgr Manager
        {
            get
            {
                return this.manager;
            }
        }

        public string ProjectID
        {
            get
            {
                return this.proinst.ProjectId;
            }
        }

        public IWfRequester Requester
        {
            get
            {
                return this.requester;
            }
            set
            {
                this.requester = value;
                this.proinst.Requester = this.requester.GetInstanceObject() as WfRequesterinst;
            }
        }
    }
}

