namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Workflow.Engine.Impl;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    [Serializable]
    public sealed class WfFactory
    {
        private WfFactory()
        {
        }

        public static IWfAbnormalAudit GetAbnormalAudit()
        {
            return new WfAbnormalAudit();
        }

        public static IWfAbnormalAudit GetAbnormalAudit(WorkItem workItem)
        {
            if (workItem == null)
            {
                throw new WfException("WorkItem cannot be null");
            }
            return new WfAbnormalAudit(workItem);
        }

        public static IWfAbnormalAudit GetAbnormalAudit(WfAbnormalAuditInst instance)
        {
            if (instance == null)
            {
                throw new WfException("Abnormal audit instance cannot be null");
            }
            return new WfAbnormalAudit(instance);
        }

        public static IWfAbnormalAudit GetAbnormalAudit(WfAssigninst assign)
        {
            if (assign == null)
            {
                throw new WfException("Assign instance cannot be null");
            }
            return new WfAbnormalAudit(assign);
        }

        public static IWfActivity GetWfActivity(Actinst actinst)
        {
            if (actinst == null)
            {
                throw new WfException("Actinst cannot be null");
            }
            return new WfActivity(actinst);
        }

        public static IWfActivity GetWfActivity(string actinstid)
        {
            if (StringHelper.IsNull(actinstid))
            {
                throw new WfException("ActinstId cannot be null ");
            }
            Actinst actinst = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Load(typeof(Actinst), actinstid) as Actinst;
            return GetWfActivity(actinst);
        }

        public static IWfActivity GetWfActivity(Actdef actdef, IWfProcess process)
        {
            if (actdef == null)
            {
                throw new WfException("Actdef cannot be null");
            }
            if (process == null)
            {
                throw new WfException("WfProcess cannot be null");
            }
            return new WfActivity(actdef, process);
        }

        public static IWfAssignment GetWfAssignment(IWfResource wfresource)
        {
            if (wfresource == null)
            {
                throw new WfException("WfResource cannot be null");
            }
            return new WfAssignment(wfresource);
        }

        public static IWfAssignment GetWfAssignment(WfAssigninst assign)
        {
            if (assign == null)
            {
                throw new WfException("WfAssigninst cannot be null");
            }
            return new WfAssignment(assign);
        }

        public static IWfAssignment GetWfAssignment(string assignId)
        {
            if (StringHelper.IsNull(assignId))
            {
                throw new WfException("Assignment id cannot be null");
            }
            WfAssigninst assign = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Load(typeof(WfAssigninst), assignId) as WfAssigninst;
            return GetWfAssignment(assign);
        }

        public static IWfAssignment GetWfAssignment(IWfResource wfresource, string staffid, string staffname)
        {
            if (wfresource == null)
            {
                throw new WfException("WfResource cannot be null");
            }
            if (StringHelper.IsNull(staffid) || StringHelper.IsNull(staffname))
            {
                throw new WfException("Staff cannot be null");
            }
            return new WfAssignment(wfresource, staffid, staffname);
        }

        public static IWfProcess GetWfProcess(Proinst proinst)
        {
            if (proinst == null)
            {
                throw new WfException("Proinst cannot be null");
            }
            return new WfProcess(proinst);
        }

        public static IWfProcess GetWfProcess(string processKey)
        {
            if (StringHelper.IsNull(processKey))
            {
                throw new WfException("ProcessKey cannot be null");
            }
            Proinst proinst = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Load(typeof(Proinst), processKey) as Proinst;
            return GetWfProcess(proinst);
        }

        public static IWfProcess GetWfProcess(Prodef prodef, IWfProcessMgr processMgr)
        {
            if (prodef == null)
            {
                throw new WfException("Process definition object cannot be null");
            }
            if (processMgr == null)
            {
                throw new WfException("ProcessMgr cannot be null");
            }
            return new WfProcess(prodef, processMgr);
        }

        public static IWfProcessMgr GetWfProcessMgr(Prodef prodef)
        {
            if (prodef == null)
            {
                throw new WfException("Prodef cannot be null");
            }
            return new WfProcessMgr(prodef);
        }

        public static IWfRequester GetWfRequester(IWfActivity wfactivity)
        {
            if (wfactivity == null)
            {
                throw new WfException("Wfactivity cannot be null");
            }
            return new WfRequester(wfactivity);
        }

        public static IWfRequester GetWfRequester(WfRequesterinst wfrequestinst)
        {
            if (wfrequestinst == null)
            {
                throw new WfException("WfRequesterInst cannot be null");
            }
            return new WfRequester(wfrequestinst);
        }

        public static IWfRequester GetWfRequester(string staffid, string staffname)
        {
            if ((staffid.Length == 0) || (staffname.Length == 0))
            {
                throw new WfException("Staff cannot be null");
            }
            return new WfRequester(staffid, staffname);
        }

        public static IWfResource GetWfResource(WfResinst wfresinst)
        {
            if (wfresinst == null)
            {
                throw new WfException("WfResinst cannot be null");
            }
            return new WfResource(wfresinst);
        }

        public static IWfResource GetWfResource(string wfResId)
        {
            WfResinst wfresinst = (WfResinst) DaoUtil.GetDaoInstance("SkyMap.Net.Workflow").Load(typeof(WfResinst), wfResId);
            return GetWfResource(wfresinst);
        }

        public static IWfResource GetWfResource(IWfActivity wfactivity, string partId)
        {
            if (wfactivity == null)
            {
                throw new WfException("WfActivity cannot be null");
            }
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            CParticipant part = QueryHelper.Get<CParticipant>("CParticipant_" + partId, partId);
            if (part == null)
            {
                throw new WfException("Participant cannot be null");
            }
            return new WfResource(wfactivity, part);
        }
    }
}

