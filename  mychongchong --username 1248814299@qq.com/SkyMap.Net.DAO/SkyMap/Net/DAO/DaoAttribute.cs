namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Runtime.Remoting.Activation;
    using System.Runtime.Remoting.Contexts;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;

    [Serializable, AttributeUsage(AttributeTargets.Class)]
    public class DaoAttribute : ContextAttribute, IContributeObjectSink
    {
        private HybridDictionary daos;

        public DaoAttribute() : base(PropertyName)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("初始化DaoAttribute");
            }
            this.daos = new HybridDictionary();
        }

        internal void BeginTransaction(string name)
        {
            lock (this.daos)
            {
                IDA0 instance = this.daos[name] as IDA0;
                if (instance == null)
                {
                    instance = DAOFactory.GetInstance(name);
                    this.daos.Add(name, instance);
                }
                instance.BeginTransaction();
                LoggingService.InfoFormatted("开启DAO事务:{0}", new object[] { name });
            }
        }

        internal void Close()
        {
            lock (this.daos)
            {
                try
                {
                    foreach (IDA0 ida in this.daos.Values)
                    {
                        ida.Close();
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
                this.daos.Clear();
                LoggingService.Info("关闭所有打开的数据库连接");
            }
        }

        internal void Commit(string name)
        {
            (this.daos[name] as IDA0).CommitTransaction();
        }

        internal bool ExistDao(string name)
        {
            lock (this.daos)
            {
                IDA0 ida = this.daos[name] as IDA0;
                return (ida != null);
            }
        }

        internal IDbConnection GetConnection(string name)
        {
            return null;
        }

        internal IDA0 GetDao(string name)
        {
            lock (this.daos)
            {
                IDA0 instance = this.daos[name] as IDA0;
                if (instance == null)
                {
                    instance = DAOFactory.GetInstance(name);
                    this.daos.Add(name, instance);
                }
                return instance;
            }
        }

        internal static DaoAttribute GetDaoAttr()
        {
            return (Thread.CurrentContext.GetProperty(PropertyName) as DaoAttribute);
        }

        public IMessageSink GetObjectSink(MarshalByRefObject mbo, IMessageSink im)
        {
            return new DaoMessageSink(this, im);
        }

        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            ctorMsg.ContextProperties.Add(this);
        }

        public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
        {
            return (ctx.GetProperty(PropertyName) != null);
        }

        internal void OpenDao(string name)
        {
            lock (this.daos)
            {
                IDA0 instance = this.daos[name] as IDA0;
                if (instance == null)
                {
                    instance = DAOFactory.GetInstance(name);
                    this.daos.Add(name, instance);
                    LoggingService.InfoFormatted("打开DAO:{0}", new object[] { name });
                }
            }
        }

        internal void RollBack(string name)
        {
            (this.daos[name] as IDA0).RollBackTransaction();
        }

        internal static string PropertyName
        {
            get
            {
                return "SkyMap.Dao";
            }
        }
    }
}

