namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public abstract class WfActivityAbstractImpl
    {
        protected WfActivity wfActivity;

        public WfActivityAbstractImpl(WfActivity wfactivity)
        {
            this.wfActivity = wfactivity;
        }

        public virtual void Activate()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("Activate the instance name:" + this.wfActivity.Name + ",id:" + this.wfActivity.Key);
            }
            if (this.wfActivity.WfState == WfStateType.Closed)
            {
                throw new AlreadyClosedException();
            }
            if (this.wfActivity.WhileOpen == WhileOpenType.Running)
            {
                throw new AlreadyRunningException();
            }
            if (this.wfActivity.WhyNotRunning == WhyNotRunningType.Suspended)
            {
                throw new CannotStartException("the instance has suspended");
            }
            (this.wfActivity.GetInstanceObject() as Actinst).StartDate = new DateTime?(DateTimeHelper.GetNow());
            this.wfActivity.ChangeStatus(WfStatusType.WF_RUNNING);
            if (!((this is WfActivityInitialImpl) || (this.wfActivity.Container.WhileOpen != WhileOpenType.NotRunning)))
            {
                this.wfActivity.Container.ChangeStatus(WfStatusType.WF_RUNNING);
            }
        }

        public virtual void Complete()
        {
            this.wfActivity.ChangeStatus(WfStatusType.WF_COMPLETED);
            this.wfActivity.Store();
            this.wfActivity.QueueNext();
        }

        protected void CreateNextActivity(Actdef nextactdef)
        {
            IWfActivity mergeActivity = this.GetMergeActivity(nextactdef);
            if (mergeActivity == null)
            {
                mergeActivity = WfFactory.GetWfActivity(nextactdef, this.wfActivity.Container);
            }
            if ((mergeActivity.WfState == WfStateType.Open) && (mergeActivity.WhileOpen == WhileOpenType.NotRunning))
            {
                mergeActivity.Start();
            }
            this.CreateRouteInstance(mergeActivity);
            if ((mergeActivity.WfState == WfStateType.Open) && (mergeActivity.WhileOpen == WhileOpenType.Running))
            {
                mergeActivity.Complete();
            }
            this.CreateWfResource(mergeActivity);
        }

        private void CreateRouteInstance(IWfActivity toActivity)
        {
            if (!StringHelper.IsNull(toActivity.Key))
            {
                WfRouteInst inst = new WfRouteInst();
                inst.Id = StringHelper.GetNewGuid();
                inst.From = this.wfActivity.GetInstanceObject() as Actinst;
                inst.To = toActivity.GetInstanceObject() as Actinst;
                inst.TimeStamp = new DateTime?(DateTimeHelper.GetNow());
                inst.ProinstId = toActivity.Container.Key;
                Actinst instanceObject = (Actinst) this.wfActivity.GetInstanceObject();
                instanceObject.FromCount++;
                Actinst actinst2 = (Actinst) toActivity.GetInstanceObject();
                actinst2.ToCount++;
                DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Put(inst, DAOType.SAVE);
            }
        }

        internal void CreateWfResource(IWfActivity wfactivity)
        {
            if (wfactivity.Type == ActdefType.INTERACTION)
            {
                Actdef defineOject = wfactivity.GetDefineOject() as Actdef;
                if (StringHelper.IsNull(defineOject.ParticipantId))
                {
                    throw new WfException("The Actdef (" + defineOject.Id + "-" + defineOject.Name + ") Participant cannot be null");
                }
                WfFactory.GetWfResource(wfactivity, defineOject.ParticipantId);
            }
        }

        protected Actinst GetBranchActinst(string toKey)
        {
            IList<WfRouteInst> list = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Query<WfRouteInst>("GetRouteTo", new object[] { toKey });
            if (list.Count == 0)
            {
                return null;
            }
            Actinst from = null;
            WfRouteInst inst = list[0];
            from = inst.From;
            ActdefType type = from.Type;
            if ((type != ActdefType.AND_BRANCH) && (type != ActdefType.OR_BRANCH))
            {
                if (((type == ActdefType.AND_MERGE) || (type == ActdefType.OR_MERGE)) || (type == ActdefType.MN_MERGE))
                {
                    from = this.GetBranchActinst(from.Id);
                    from = this.GetBranchActinst(from.Id);
                }
                else
                {
                    from = this.GetBranchActinst(from.Id);
                }
            }
            return from;
        }

        protected IWfActivity GetMergeActivity(Actdef nextactdef)
        {
            if (((nextactdef.Type == ActdefType.AND_MERGE) || (nextactdef.Type == ActdefType.OR_MERGE)) || (nextactdef.Type == ActdefType.MN_MERGE))
            {
                IList<Actinst> list = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Query<Actinst>("GetActinstByProinstAndActdef", new object[] { this.wfActivity.Key, nextactdef.Id });
                if (list.Count == 0)
                {
                    return null;
                }
                Actinst branchActinst = this.GetBranchActinst(this.wfActivity.Key);
                if ((branchActinst == null) || (branchActinst.FromCount == 1))
                {
                    return null;
                }
                foreach (Actinst actinst2 in list)
                {
                    if (this.GetBranchActinst(actinst2.Id).Id == branchActinst.Id)
                    {
                        return WfFactory.GetWfActivity(actinst2);
                    }
                }
            }
            return null;
        }

        public virtual void Pass(IList<Transition> toTrans)
        {
            if ((toTrans.Count > 1) || (toTrans.Count == 0))
            {
                throw new WfException(string.Format("以活动定义ID:{0} 开始的路由数量不正确，应且仅有1个路由,而现在有：{1}个", this.wfActivity.ActdefId, toTrans.Count));
            }
            Transition toTran = toTrans[0];
            this.Pass(toTran);
        }

        protected void Pass(Transition toTran)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将对路由：{0}({1})进行共{2}个转出的条件测试", new object[] { toTran.Id, toTran.Name, toTran.Conditions.Count });
            }
            foreach (SkyMap.Net.Workflow.XPDL.Condition condition in toTran.Conditions)
            {
                if (!this.wfActivity.CheckCondition(condition))
                {
                    throw new NotMeetConditionException();
                }
            }
            this.CreateNextActivity(toTran.To);
        }

        public abstract void Start();
    }
}

