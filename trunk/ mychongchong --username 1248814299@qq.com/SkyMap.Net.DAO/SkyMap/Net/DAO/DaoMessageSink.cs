namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;

    public class DaoMessageSink : IMessageSink
    {
        private DaoAttribute daoAttr;
        private IMessageSink imNext;

        internal DaoMessageSink(DaoAttribute daoAttribute, IMessageSink ims)
        {
            this.imNext = ims;
            this.daoAttr = daoAttribute;
        }

        public IMessageCtrl AsyncProcessMessage(IMessage im, IMessageSink ims)
        {
            return this.imNext.AsyncProcessMessage(im, ims);
        }

        public IMessage SyncProcessMessage(IMessage imCall)
        {
            string propertyName;
            Exception exception2;
            LoggingService.Info("将同步处理消息");
            if (!(imCall is IMethodMessage))
            {
                LoggingService.Info("不是IMethodMessage");
                return this.imNext.SyncProcessMessage(imCall);
            }
            IMethodMessage message = imCall as IMethodCallMessage;
            MethodBase methodBase = message.MethodBase;
            LoggingService.InfoFormatted("将同步处理息的方法名是：{0}", new object[] { methodBase.Name });
            if (methodBase.IsConstructor)
            {
                return this.imNext.SyncProcessMessage(imCall);
            }
            bool flag = Attribute.GetCustomAttribute(methodBase, typeof(NoDaoAttribute)) != null;
            LoggingService.InfoFormatted("是否自动打开Session：{0}", new object[] { !flag });
            if (flag)
            {
                return this.imNext.SyncProcessMessage(imCall);
            }
            TransactionAttribute[] customAttributes = (TransactionAttribute[]) Attribute.GetCustomAttributes(message.MethodBase, typeof(TransactionAttribute));
            foreach (TransactionAttribute attribute in customAttributes)
            {
                propertyName = attribute.PropertyName;
                LoggingService.InfoFormatted("将启动事物：{0}", new object[] { propertyName });
                this.daoAttr.OpenDao(propertyName);
                this.daoAttr.BeginTransaction(propertyName);
            }
            IMessage message2 = this.imNext.SyncProcessMessage(imCall);
            IMethodReturnMessage message3 = message2 as IMethodReturnMessage;
            if (message3.Exception == null)
            {
                LoggingService.InfoFormatted("方法执行完成,将提交事物", new object[0]);
                try
                {
                    foreach (TransactionAttribute attribute in customAttributes)
                    {
                        propertyName = attribute.PropertyName;
                        this.daoAttr.Commit(propertyName);
                    }
                }
                catch (Exception exception1)
                {
                    exception2 = exception1;
                    LoggingService.Error("提交事物失败", exception2);
                    throw new DaoSynchronizationException("Commit transition error", exception2);
                }
                finally
                {
                    if (this.daoAttr != null)
                    {
                        this.daoAttr.Close();
                    }
                }
                return message2;
            }
            LoggingService.InfoFormatted("方法执行完成,但有错误，将回滚事务", new object[0]);
            try
            {
                foreach (TransactionAttribute attribute in customAttributes)
                {
                    propertyName = attribute.PropertyName;
                    this.daoAttr.RollBack(propertyName);
                }
            }
            catch (Exception exception3)
            {
                exception2 = exception3;
                LoggingService.Error(exception2);
            }
            finally
            {
                if (this.daoAttr != null)
                {
                    this.daoAttr.Close();
                }
            }
            return message2;
        }

        public IMessageSink NextSink
        {
            get
            {
                return this.imNext;
            }
        }
    }
}

