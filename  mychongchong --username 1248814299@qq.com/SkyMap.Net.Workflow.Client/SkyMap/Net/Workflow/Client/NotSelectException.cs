namespace SkyMap.Net.Workflow.Client
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotSelectException : WfClientException
    {
        public NotSelectException()
        {
        }

        public NotSelectException(string message) : base(message)
        {
        }

        protected NotSelectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotSelectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string Message
        {
            get
            {
                return "你没有选择任何业务！";
            }
        }
    }
}

