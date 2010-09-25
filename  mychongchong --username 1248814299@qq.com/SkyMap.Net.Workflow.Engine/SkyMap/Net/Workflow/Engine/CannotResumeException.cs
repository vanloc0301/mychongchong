namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotResumeException : WfException
    {
        public CannotResumeException()
        {
        }

        public CannotResumeException(string message) : base(message)
        {
        }

        protected CannotResumeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotResumeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

