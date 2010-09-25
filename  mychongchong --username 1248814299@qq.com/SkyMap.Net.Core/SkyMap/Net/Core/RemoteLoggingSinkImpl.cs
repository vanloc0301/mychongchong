namespace SkyMap.Net.Core
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository;
    using System;
    using log4net;

    public class RemoteLoggingSinkImpl : MarshalByRefObject, RemotingAppender.IRemoteLoggingSink
    {
        private readonly ILoggerRepository m_repository = LogManager.GetRepository();

        public RemoteLoggingSinkImpl()
        {
            if (this.m_repository == null)
            {
                LoggingService.Error("不能初始化远程日志记录器");
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void LogEvents(LoggingEvent[] events)
        {
            if (events != null)
            {
                foreach (LoggingEvent event2 in events)
                {
                    if (event2 != null)
                    {
                        this.m_repository.Log(event2);
                    }
                }
            }
        }
    }
}

