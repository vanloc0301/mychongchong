namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ProcessMgrNotEnableException : WfException
    {
        public ProcessMgrNotEnableException()
        {
        }

        public ProcessMgrNotEnableException(string message) : base(message)
        {
        }

        protected ProcessMgrNotEnableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ProcessMgrNotEnableException(string message, Exception InnerException) : base(message, InnerException)
        {
        }
    }
}

