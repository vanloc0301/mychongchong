namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotMeetConditionException : WfException
    {
        public NotMeetConditionException()
        {
        }

        public NotMeetConditionException(string message) : base(message)
        {
        }

        protected NotMeetConditionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotMeetConditionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

