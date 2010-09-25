namespace SkyMap.Net.Workflow.Client
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotBatchExecuteException : WfClientException
    {
        public CannotBatchExecuteException()
        {
        }

        public CannotBatchExecuteException(string message) : base(message)
        {
        }

        protected CannotBatchExecuteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotBatchExecuteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string Message
        {
            get
            {
                return "你选择了多个不同种类的业务活动，不能进行批处理";
            }
        }
    }
}

