namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WfRequester : IWfRequester
    {
        private WfRequesterinst wfRequesterinst;

        public WfRequester(IWfActivity wfactivity)
        {
            this.wfRequesterinst = new WfRequesterinst();
            this.wfRequesterinst.ActinstId = wfactivity.Key;
            this.wfRequesterinst.ActinstName = wfactivity.Name;
            this.Store();
        }

        public WfRequester(WfRequesterinst wfrequesterinst)
        {
            this.wfRequesterinst = wfrequesterinst;
        }

        public WfRequester(string staffid, string staffname)
        {
            this.wfRequesterinst = this.GetRequester(staffid, staffname);
            if (this.wfRequesterinst == null)
            {
                this.wfRequesterinst = new WfRequesterinst();
                this.wfRequesterinst.StaffId = staffid;
                this.wfRequesterinst.StaffName = staffname;
                this.Store();
            }
        }

        public object GetInstanceObject()
        {
            return this.wfRequesterinst;
        }

        private WfRequesterinst GetRequester(string staffid, string staffname)
        {
            IList<WfRequesterinst> list = QueryHelper.List<WfRequesterinst>("SkyMap.Net.Workflow", "GetRequester_" + staffid + "_" + staffname, "GetRequester", new object[] { staffid, staffname });
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public void ReceiveEvent(IWfEventAudit audit)
        {
            if (audit is IWfStateEventAudit)
            {
                IWfStateEventAudit audit2 = audit as IWfStateEventAudit;
                if (audit2.NewState == WfStatusType.WF_COMPLETED)
                {
                    string actinstId = this.wfRequesterinst.ActinstId;
                    if (!StringHelper.IsNull(this.wfRequesterinst.ActinstId))
                    {
                        IWfActivity wfActivity = WfFactory.GetWfActivity(actinstId);
                        if (wfActivity != null)
                        {
                            Actdef defineOject = wfActivity.GetDefineOject() as Actdef;
                            if ((defineOject.Type == ActdefType.SUBFLOW) && defineOject.IsSubflowSync)
                            {
                                wfActivity.Activate();
                            }
                        }
                    }
                }
            }
        }

        public void RegisterProcess(IWfProcess process)
        {
        }

        public void Store()
        {
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            if (StringHelper.IsNull(this.wfRequesterinst.Id))
            {
                this.wfRequesterinst.Id = StringHelper.GetNewGuid();
                daoInstance.Put(this.wfRequesterinst, DAOType.SAVE);
            }
            else
            {
                daoInstance.Put(this.wfRequesterinst, DAOType.UPDATE);
            }
        }
    }
}

