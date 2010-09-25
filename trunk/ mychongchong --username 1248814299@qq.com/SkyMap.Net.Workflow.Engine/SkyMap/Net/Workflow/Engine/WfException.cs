namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class WfException : CoreException
    {
        public WfException()
        {
        }

        public WfException(string message) : base(message)
        {
        }

        protected WfException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public WfException(string message, Exception InnerException) : base(message, InnerException)
        {
        }
    }
}

