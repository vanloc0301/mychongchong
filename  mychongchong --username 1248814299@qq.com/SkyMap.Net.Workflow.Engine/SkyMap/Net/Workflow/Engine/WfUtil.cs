namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Messaging;
    using System.Web;

    public sealed class WfUtil
    {
        private const string AbnormalData_Tag = "AbnormalData_Tag";
        private const string CWF_ABORTED = "回收";
        private const string CWF_ALL_DELEGATED = "角色委托";
        private const string CWF_CALLBACK = "撤回";
        private const string CWF_CANCELACCEPTED = "取消签收";
        private const string CWF_COMPLETED = "办结";
        private const string CWF_DELEGATED = "委托";
        private const string CWF_NOT_STARTED = "待办";
        private const string CWF_RUNNING = "在办";
        private const string CWF_SENDBACKED = "退回";
        private const string CWF_SENDBACKING = "被退回";
        private const string CWF_SUSPENDED = "挂起";
        private const string CWF_TERMINATED = "退件";
        private const string PassData_Tag = "PassData_Tag";

        public static void FreeAbnormalContextData()
        {
            if (HttpContext.Current == null)
            {
                CallContext.FreeNamedDataSlot("AbnormalData_Tag");
            }
            else
            {
                HttpContext.Current.Items.Remove("AbnormalData_Tag");
            }
        }

        public static void FreePassContextData()
        {
            CallContext.FreeNamedDataSlot("PassData_Tag");
        }

        public static WfLogicalAbnormalContextData GetAbnormalContextData()
        {
            if (HttpContext.Current == null)
            {
                return (CallContext.GetData("AbnormalData_Tag") as WfLogicalAbnormalContextData);
            }
            return (HttpContext.Current.Items["AbnormalData_Tag"] as WfLogicalAbnormalContextData);
        }

        public static double GetCostTime(WfInstanceElement instance)
        {
            WfStatusType status = instance.Status;
            if (GetWfState(status) == WfStateType.Closed)
            {
                return instance.CostTime;
            }
            if (GetWhileOpen(status) == WhileOpenType.NotRunning)
            {
                return 0.0;
            }
            DateTime? lastStateDate = instance.LastStateDate;
            if (!lastStateDate.HasValue)
            {
                lastStateDate = instance.EndDate;
            }
            if (!lastStateDate.HasValue)
            {
                lastStateDate = instance.StartDate;
            }
            if (instance.EndDate.HasValue && (instance.CostTime == 0.0))
            {
                instance.CostTime = DateTimeHelper.GetCostTimeExcludingHoli(instance.StartDate.Value, instance.EndDate.Value);
            }
            return (instance.CostTime + DateTimeHelper.GetCostTimeExcludingHoli(lastStateDate.Value, DateTimeHelper.GetNow()));
        }

        public static HowClosedType GetHowClosed(WfStatusType status)
        {
            switch (status)
            {
                case WfStatusType.WF_COMPLETED:
                    return HowClosedType.Completed;

                case WfStatusType.WF_ABORTED:
                    return HowClosedType.Aborted;

                case WfStatusType.WF_TERMINATED:
                    return HowClosedType.Terminated;
            }
            throw new InvalidStatusException("Invalid HowColsedType");
        }

        public static WfLogicalPassContextData GetPassContextData()
        {
            return (CallContext.GetData("PassData_Tag") as WfLogicalPassContextData);
        }

        public static string GetSMNAbnormal(WfAbnormalType abl)
        {
            switch (abl)
            {
                case WfAbnormalType.CANCEL_ACCEPTED:
                    return "取消签收";

                case WfAbnormalType.ABROTED:
                    return "回收";

                case WfAbnormalType.SUSPENDED:
                    return "挂起";

                case WfAbnormalType.NO_ABNORMAL:
                    return string.Empty;

                case WfAbnormalType.CALL_BACK:
                    return "撤回";

                case WfAbnormalType.SEND_BACKED:
                    return "退回";

                case WfAbnormalType.SEND_BACKING:
                    return "被退回";

                case WfAbnormalType.TERMINATED:
                    return "退件";

                case WfAbnormalType.DELEGATED:
                    return "委托";

                case WfAbnormalType.ALL_DELEGATED:
                    return "角色委托";
            }
            throw new WfException("Cannot know the abnormal status : \n" + Enum.GetName(typeof(WfAbnormalType), abl));
        }

        public static string GetSMNAssignStatus(WfAssigninst assign)
        {
            WfAbnormalType abnormalStatus = assign.AbnormalStatus;
            switch (abnormalStatus)
            {
                case WfAbnormalType.NO_ABNORMAL:
                    switch (assign.Status)
                    {
                        case AssignStatusType.Not_Accepted:
                            return "待办";

                        case AssignStatusType.Accepted:
                            return "在办";

                        case AssignStatusType.Completed:
                            return "办结";
                    }
                    throw new WfException("The db date have error");
            }
            return GetSMNAbnormal(abnormalStatus);
        }

        public static string GetSMNState(WfStatusType status)
        {
            switch (status)
            {
                case WfStatusType.WF_RUNNING:
                    return "在办";

                case WfStatusType.WF_COMPLETED:
                    return "办结";

                case WfStatusType.WF_NOT_STARTED:
                    return "待办";

                case WfStatusType.WF_ABORTED:
                    return "回收";

                case WfStatusType.WF_SUSPENDED:
                    return "挂起";

                case WfStatusType.WF_TERMINATED:
                    return "退件";
            }
            throw new InvalidCastException();
        }

        public static IDictionary<WfStatusType, string> GetStatuses()
        {
            Dictionary<WfStatusType, string> dictionary = new Dictionary<WfStatusType, string>(6);
            WfStatusType[] values = (WfStatusType[]) Enum.GetValues(typeof(WfStatusType));
            foreach (WfStatusType type in values)
            {
                dictionary.Add(type, GetSMNState(type));
            }
            return dictionary;
        }

        public static WfStateType GetWfState(WfStatusType status)
        {
            switch (status)
            {
                case WfStatusType.WF_RUNNING:
                    return WfStateType.Open;

                case WfStatusType.WF_COMPLETED:
                    return WfStateType.Closed;

                case WfStatusType.WF_NOT_STARTED:
                    return WfStateType.Open;

                case WfStatusType.WF_ABORTED:
                    return WfStateType.Closed;

                case WfStatusType.WF_SUSPENDED:
                    return WfStateType.Closed;

                case WfStatusType.WF_TERMINATED:
                    return WfStateType.Closed;
            }
            throw new InvalidStatusException("Invalid wfstatetype");
        }

        public static WfStatusType GetWfStatus(string sName)
        {
            switch (sName.Trim())
            {
                case "在办":
                    return WfStatusType.WF_RUNNING;

                case "办结":
                    return WfStatusType.WF_COMPLETED;

                case "待办":
                    return WfStatusType.WF_NOT_STARTED;

                case "回收":
                    return WfStatusType.WF_ABORTED;

                case "挂起":
                    return WfStatusType.WF_SUSPENDED;

                case "退件":
                    return WfStatusType.WF_TERMINATED;
            }
            throw new WfException("Cannot know the status type : " + sName);
        }

        public static WhileOpenType GetWhileOpen(WfStatusType status)
        {
            switch (status)
            {
                case WfStatusType.WF_RUNNING:
                    return WhileOpenType.Running;

                case WfStatusType.WF_NOT_STARTED:
                    return WhileOpenType.NotRunning;
            }
            throw new InvalidStatusException("invalid WhileOpenType");
        }

        public static WhyNotRunningType GetWhyNotRunning(WfStatusType status)
        {
            switch (status)
            {
                case WfStatusType.WF_NOT_STARTED:
                    return WhyNotRunningType.NotStarted;

                case WfStatusType.WF_SUSPENDED:
                    return WhyNotRunningType.Suspended;
            }
            throw new InvalidStatusException("Invalid WhyNotRunningType");
        }

        public static void SetAbnormalContextData(WfLogicalAbnormalContextData abnormalData)
        {
            if (HttpContext.Current == null)
            {
                CallContext.SetData("AbnormalData_Tag", abnormalData);
            }
            else
            {
                HttpContext.Current.Items["AbnormalData_Tag"] = abnormalData;
            }
        }

        public static void SetPassContextData(WfLogicalPassContextData passData)
        {
            CallContext.SetData("PassData_Tag", passData);
        }
    }
}

