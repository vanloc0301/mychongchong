namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WfProcessMgr : IWfProcessMgr
    {
        private IList<IWfProcess> processList;
        private Prodef prodef;
        private ProcessMgrStateType state;

        public WfProcessMgr(Prodef prodef)
        {
            try
            {
                this.prodef = prodef;
                if (prodef.Status)
                {
                    this.state = ProcessMgrStateType.Enabled;
                }
                else
                {
                    this.state = ProcessMgrStateType.Disabled;
                }
                this.processList = new List<IWfProcess>();
            }
            catch (ApplicationException exception)
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.Debug("Except on new WfProcessMgr:" + exception.Message);
                }
                throw new WfException("创建WfProcessMgr时发生错误", exception);
            }
        }

        public IWfProcess CreateProcess(IWfRequester requester)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("WfManager will CreateProcess");
            }
            if (this.state == ProcessMgrStateType.Disabled)
            {
                throw new ProcessMgrNotEnableException();
            }
            if (requester == null)
            {
                throw new RequesterRequiredException();
            }
            IWfProcess wfProcess = WfFactory.GetWfProcess(this.prodef, this);
            try
            {
                wfProcess.Requester = requester;
            }
            catch (CannotChangeRequesterException exception)
            {
                throw new WfException(exception.Message, exception);
            }
            this.processList.Add(wfProcess);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("[WfProcessMgr.CreateProcess]:Process create");
            }
            return wfProcess;
        }

        public IList<IWfProcess> GetListProcess()
        {
            return this.processList;
        }

        public int HowManyProcess()
        {
            return this.processList.Count;
        }

        public string Description
        {
            get
            {
                return this.prodef.Description;
            }
        }

        public string Name
        {
            get
            {
                return this.prodef.Name;
            }
        }

        public ProcessMgrStateType ProcessMgrState
        {
            get
            {
                return this.state;
            }
        }

        public string Version
        {
            get
            {
                return this.prodef.Version;
            }
        }
    }
}

