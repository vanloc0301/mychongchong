namespace SkyMap.Net.Criteria.Client
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ConditionInputException : CoreException
    {
        public ConditionInputException()
        {
        }

        public ConditionInputException(string message) : base(message)
        {
        }

        protected ConditionInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConditionInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

