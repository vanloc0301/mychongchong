namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotRepeatDelegateException : CoreException
    {
        public CannotRepeatDelegateException()
        {
        }

        public CannotRepeatDelegateException(string message) : base(message)
        {
        }

        protected CannotRepeatDelegateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotRepeatDelegateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

