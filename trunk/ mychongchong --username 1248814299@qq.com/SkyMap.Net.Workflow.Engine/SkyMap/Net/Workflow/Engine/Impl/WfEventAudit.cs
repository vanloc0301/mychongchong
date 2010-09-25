namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using System;

    [Serializable]
    public abstract class WfEventAudit : IWfEventAudit
    {
        protected IWfExecutionObject _source;
        protected DateTime _timeStamp;

        public WfEventAudit(IWfExecutionObject source)
        {
            this._source = source;
            this._timeStamp = DateTimeHelper.GetNow();
        }

        protected void Store(WfEventAuditInst wfAuditInst)
        {
            wfAuditInst.ProinstId = this.ProcessKey;
            wfAuditInst.ProinstName = this.ProcessName;
            wfAuditInst.ActinstId = this.ActivityKey;
            wfAuditInst.ActinstName = this.ActivityName;
            wfAuditInst.TimeStamp = new DateTime?(DateTimeHelper.GetNow());
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            DAOType uPDATE = DAOType.UPDATE;
            if (StringHelper.IsNull(wfAuditInst.Id))
            {
                wfAuditInst.Id = StringHelper.GetNewGuid();
                LoggingService.DebugFormatted("将新建WfEventAudit实例：{0}", new object[] { wfAuditInst.Id });
                uPDATE = DAOType.SAVE;
            }
            daoInstance.Put(wfAuditInst, uPDATE);
        }

        public string ActivityKey
        {
            get
            {
                if (this._source is IWfActivity)
                {
                    return this._source.Key;
                }
                return null;
            }
        }

        public string ActivityName
        {
            get
            {
                if (this._source is IWfActivity)
                {
                    return this._source.Name;
                }
                return null;
            }
        }

        public string EventType
        {
            get
            {
                return null;
            }
        }

        public string ProcessKey
        {
            get
            {
                if (this._source is IWfProcess)
                {
                    return this._source.Key;
                }
                if (this._source is IWfActivity)
                {
                    return ((IWfActivity) this._source).Container.Key;
                }
                return null;
            }
        }

        public string ProcessName
        {
            get
            {
                if (this._source is IWfProcess)
                {
                    return this._source.Name;
                }
                if (this._source is IWfActivity)
                {
                    return ((IWfActivity) this._source).Container.Name;
                }
                return null;
            }
        }

        public IWfExecutionObject Source
        {
            get
            {
                return this._source;
            }
        }

        public DateTime? TimeStamp
        {
            get
            {
                return new DateTime?(this._timeStamp);
            }
        }
    }
}

