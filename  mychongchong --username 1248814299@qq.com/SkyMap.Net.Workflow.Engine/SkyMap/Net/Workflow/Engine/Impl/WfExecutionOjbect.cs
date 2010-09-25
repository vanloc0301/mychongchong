namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections;

    [Serializable]
    public abstract class WfExecutionOjbect : IWfExecutionObject
    {
        protected WfInstanceElement wfInstance;

        public WfExecutionOjbect(WfInstanceElement wfinstance)
        {
            this.wfInstance = wfinstance;
        }

        public WfExecutionOjbect(WfInstanceElement wfinstance, Actdef actdef)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Init the WfActivity");
            }
            this.wfInstance = wfinstance;
            this.wfInstance.Name = actdef.Name;
            Prodef prodef = actdef.Prodef;
            this.InitInstByProdef(prodef);
            this.InitElse();
        }

        public WfExecutionOjbect(WfInstanceElement wfinstance, Prodef prodef)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Init the WfProcess");
            }
            this.wfInstance = wfinstance;
            this.wfInstance.Name = prodef.Name;
            this.InitInstByProdef(prodef);
            this.InitElse();
        }

        public virtual void Abort()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Abort the instance:" + this.Name);
            }
            if (this.WfState == WfStateType.Closed)
            {
                throw new CannotStopException("The instance has closed");
            }
            this.ChangeStatus(WfStatusType.WF_ABORTED);
        }

        protected bool CanResume()
        {
            return (((this.wfInstance.Status == WfStatusType.WF_SUSPENDED) || (this.wfInstance.Status == WfStatusType.WF_ABORTED)) || (this.wfInstance.Status == WfStatusType.WF_TERMINATED));
        }

        public virtual void ChangeStatus(WfStatusType newstatus)
        {
            WfStatusType status = this.wfInstance.Status;
            this.wfInstance.Status = newstatus;
            if (this.WfState == WfStateType.Closed)
            {
                this.wfInstance.EndDate = new DateTime?(DateTimeHelper.GetNow());
                if (status == WfStatusType.WF_NOT_STARTED)
                {
                    this.wfInstance.CostTime = 0.0;
                }
                else
                {
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("计算工作流对象远行总用时", new object[0]);
                    }
                    this.wfInstance.CostTime += DateTimeHelper.GetCostTimeExcludingHoli(this.LastStateTime.Value, this.wfInstance.EndDate.Value);
                }
            }
            else
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("计算预定完成日期", new object[0]);
                }
                if (this.wfInstance.DueTime > 0.0)
                {
                    this.wfInstance.DueDate = new DateTime?(DateTimeHelper.GetDateExcludingHoli(DateTimeHelper.GetNow(), this.wfInstance.DueTime - this.wfInstance.CostTime));
                }
                else
                {
                    this.wfInstance.DueDate = new DateTime?(DateTimeHelper.GetNow());
                }
            }
            this.wfInstance.LastStateDate = new DateTime?(DateTimeHelper.GetNow());
        }

        public abstract object GetDefineOject();
        protected WfElement GetDefineOject(string defId, Type wfDefType)
        {
            WfElement element = null;
            try
            {
                element = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Load(wfDefType, defId) as WfElement;
            }
            catch (ApplicationException exception)
            {
                throw new WfException("Get type of:" + wfDefType.ToString() + " and id value equal " + defId + " have error", exception);
            }
            if (element == null)
            {
                throw new WfException("Cannot get type of: " + wfDefType.ToString() + " and id value equal " + defId);
            }
            return element;
        }

        public IList GetHistory()
        {
            return null;
        }

        public object GetInstanceObject()
        {
            return this.wfInstance;
        }

        protected void InitElse()
        {
            this.wfInstance.CostTime = 0.0;
            this.wfInstance.CreateDate = new DateTime?(DateTimeHelper.GetNow());
            this.wfInstance.LastStateDate = new DateTime?(DateTimeHelper.GetNow());
            this.wfInstance.Priority = 0;
            this.wfInstance.Status = WfStatusType.WF_NOT_STARTED;
        }

        protected void InitInstByProdef(Prodef prodef)
        {
            this.wfInstance.PackageId = prodef.Package.Id;
            this.wfInstance.PackageName = prodef.Package.Name;
            this.wfInstance.ProdefId = prodef.Id;
            this.wfInstance.ProdefName = prodef.Name;
            this.wfInstance.ProdefVersion = prodef.Version;
        }

        public virtual void Resume()
        {
            if (!this.CanResume())
            {
                throw new CannotResumeException("The status:" + this.wfInstance.Status.ToString() + " cannot resume");
            }
            this.ChangeStatus(WfStatusType.WF_RUNNING);
        }

        internal void Store()
        {
            if (this.wfInstance == null)
            {
                throw new WfException(base.GetType().ToString() + " is null");
            }
            if (StringHelper.IsNull(this.wfInstance.Id))
            {
                this.wfInstance.Id = StringHelper.GetNewGuid();
                DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Put(this.wfInstance, DAOType.SAVE);
            }
            else
            {
                DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Put(this.wfInstance, DAOType.UPDATE);
            }
        }

        public virtual void Suspend()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Suspend the instance name:" + this.Name + ",id:" + this.Key);
            }
            if (this.WfState == WfStateType.Closed)
            {
                throw new CannotSuspentException("the instance has closed");
            }
            this.ChangeStatus(WfStatusType.WF_SUSPENDED);
        }

        public virtual void Terminate()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Terminate the instance:" + this.Name);
            }
            if (this.WfState == WfStateType.Closed)
            {
                throw new CannotStopException("The instance has closed");
            }
            this.ChangeStatus(WfStatusType.WF_TERMINATED);
        }

        public string Description
        {
            get
            {
                return this.wfInstance.Description;
            }
            set
            {
                this.wfInstance.Description = value;
            }
        }

        public HowClosedType HowClosed
        {
            get
            {
                return WfUtil.GetHowClosed(this.wfInstance.Status);
            }
        }

        public string Key
        {
            get
            {
                return this.wfInstance.Id;
            }
        }

        public DateTime? LastStateTime
        {
            get
            {
                return this.wfInstance.LastStateDate;
            }
        }

        public string Name
        {
            get
            {
                return this.wfInstance.Name;
            }
            set
            {
                this.wfInstance.Name = value;
            }
        }

        public string PackageId
        {
            get
            {
                return this.wfInstance.PackageId;
            }
        }

        public string PackageName
        {
            get
            {
                return this.wfInstance.PackageName;
            }
        }

        public short Priority
        {
            get
            {
                return this.wfInstance.Priority;
            }
            set
            {
                this.wfInstance.Priority = value;
            }
        }

        public string ProdefId
        {
            get
            {
                return this.wfInstance.ProdefId;
            }
        }

        public string ProdefName
        {
            get
            {
                return this.wfInstance.ProdefName;
            }
        }

        public string ProdefVersion
        {
            get
            {
                return this.wfInstance.ProdefVersion;
            }
        }

        public string State
        {
            get
            {
                return WfUtil.GetSMNState(this.wfInstance.Status);
            }
        }

        public WfStateType WfState
        {
            get
            {
                return WfUtil.GetWfState(this.wfInstance.Status);
            }
        }

        public WhileOpenType WhileOpen
        {
            get
            {
                return WfUtil.GetWhileOpen(this.wfInstance.Status);
            }
        }

        public WhyNotRunningType WhyNotRunning
        {
            get
            {
                return WfUtil.GetWhyNotRunning(this.wfInstance.Status);
            }
        }
    }
}

