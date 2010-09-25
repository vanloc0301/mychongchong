namespace SkyMap.Net.Workflow.Client
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class WfClientException : CoreException
    {
        public WfClientException()
        {
        }

        public WfClientException(string message) : base(message)
        {
        }

        protected WfClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public WfClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

