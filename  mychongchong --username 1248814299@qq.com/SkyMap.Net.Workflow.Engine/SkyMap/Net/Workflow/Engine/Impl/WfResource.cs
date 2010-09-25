namespace SkyMap.Net.Workflow.Engine.Impl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Messaging;

    [Serializable]
    public class WfResource : IWfResource
    {
        private IWfActivity wfActivity;
        private WfResinst wfResinst;

        public WfResource(WfResinst wfresinst)
        {
            this.wfResinst = wfresinst;
            this.wfActivity = WfFactory.GetWfActivity(this.wfResinst.Actinst);
        }

        public WfResource(IWfActivity wfActivity, CParticipant part)
        {
            this.Init(wfActivity, part);
            this.CreateAssignment();
            this.Store();
        }

        public void Assign()
        {
            this.wfResinst.IsAssigned = true;
            int num = 0;
            foreach (WfAssigninst assigninst in this.wfResinst.Assigns)
            {
                if (assigninst.Status == AssignStatusType.Not_Accepted)
                {
                    this.wfResinst.IsAssigned = false;
                }
                else
                {
                    num++;
                }
            }
            if (num == 1)
            {
                this.wfActivity.Activate();
            }
        }

        public void CallBack()
        {
            if (this.IsAssigned || (this.wfActivity.WhileOpen == WhileOpenType.Running))
            {
                throw new CannotCallBackException("it was accepted:" + this.Key);
            }
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            daoInstance.Put(this.wfResinst, DAOType.DELETE);
            this.OnDelete(daoInstance);
            this.wfResinst = null;
            this.wfActivity = null;
        }

        private void CreateAssignment()
        {
            IWfAssignment wfAssignment = null;
            if (this.wfResinst.Assigns != null)
            {
                return;
            }
            this.wfResinst.Assigns = new List<WfAssigninst>();
            bool flag2 = true;
            if (flag2.Equals(CallContext.GetData("CreateAndAccept")))
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                wfAssignment = WfFactory.GetWfAssignment(this, smIdentity.UserId, smIdentity.UserName);
                wfAssignment.Store();
                wfAssignment.Accept();
            }
            else if (!((this.wfResinst.AssignRule == AssignRuleType.ANY) || StringHelper.IsNull(this.wfResinst.StaffId)))
            {
                wfAssignment = WfFactory.GetWfAssignment(this, this.wfResinst.StaffId, this.wfResinst.StaffName);
            }
            else
            {
                switch (this.wfResinst.AssignRule)
                {
                    case AssignRuleType.ALL:
                        this.CreateAssignment(this.wfResinst.ParticipantType, this.wfResinst.ParticipantId);
                        goto Label_01A9;

                    case AssignRuleType.LEASTWORKITEMS:
                        throw new WfException("No implement the assign type :LEASTWORKITEMS");

                    case AssignRuleType.FCFA:
                        wfAssignment = WfFactory.GetWfAssignment(this);
                        goto Label_01A9;

                    case AssignRuleType.ANY:
                    {
                        WfLogicalPassContextData passContextData = WfUtil.GetPassContextData();
                        if (passContextData != null)
                        {
                            for (int i = 0; i < passContextData.ToStaffIds.Length; i++)
                            {
                                WfFactory.GetWfAssignment(this, passContextData.ToStaffIds[i], passContextData.ToStaffNames[i]).Store();
                            }
                        }
                        return;
                    }
                }
                throw new WfException("No implement the assign type +" + Enum.GetName(typeof(AssignRuleType), this.wfResinst.AssignRule));
            }
        Label_01A9:
            if (wfAssignment != null)
            {
                wfAssignment.Store();
            }
        }

        private void CreateAssignment(string partType, string partIdValue)
        {
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            IList<IList<string>> list = QueryHelper.List<IList<string>>("SkyMap.Net.Workflow", "GetStaffIdNamesByPart_" + partType + "_" + partIdValue, "GetStaffIdNamesByPart", new string[] { "partType", "partIdValue" }, (object[]) new string[] { partType, partIdValue });
            for (int i = 0; i < list.Count; i++)
            {
                WfFactory.GetWfAssignment(this, (string) (list[i] as IList)[0], (string) (list[i] as IList)[1]).Store();
            }
        }

        private void DeleteToInstance(Actinst from, Actinst to, IDA0 dao)
        {
            Actinst actinst = to;
            do
            {
                IEnumerator<WfRouteInst> enumerator = actinst.RouteToMe.GetEnumerator();
                enumerator.MoveNext();
                WfRouteInst current = enumerator.Current;
                dao.Put(actinst, DAOType.DELETE);
                actinst = current.From;
                if ((actinst.FromCount > 1) || (actinst.ToCount > 1))
                {
                    throw new CannotCallBackException("Cannot call a 'multi branch or multi branch' instance:" + actinst.Id);
                }
            }
            while (actinst.Id != from.Id);
        }

        public IList GetListWorkItem()
        {
            IList list = new ArrayList();
            foreach (WfAssigninst assigninst in this.wfResinst.Assigns)
            {
                list.Add(WfFactory.GetWfAssignment(assigninst));
            }
            return list;
        }

        public WfResinst GetWfResinst()
        {
            return this.wfResinst;
        }

        public int HowManyWorkItem()
        {
            return this.wfResinst.Assigns.Count;
        }

        private void Init(IWfActivity wfActivity, CParticipant part)
        {
            Actinst instanceObject = wfActivity.GetInstanceObject() as Actinst;
            this.wfActivity = wfActivity;
            this.wfResinst = instanceObject.WfResinst;
            if (this.wfResinst == null)
            {
                this.wfResinst = new WfResinst();
                this.SetWfResInst(part, instanceObject);
            }
            WfLogicalPassContextData passContextData = WfUtil.GetPassContextData();
            if (passContextData != null)
            {
                this.SetWfResInst(passContextData);
            }
        }

        private void OnDelete(IDA0 dao)
        {
            string fromActinstId = this.wfResinst.FromActinstId;
            if (fromActinstId.IndexOf(',') > 0)
            {
                throw new CannotCallBackException("不能撤回多个分支到发送任务");
            }
            Actinst from = dao.Load(typeof(Actinst), fromActinstId) as Actinst;
            if (from == null)
            {
                throw new CannotCallBackException("不能找到前一步环节任务");
            }
            this.RefreshFrom(from);
            this.DeleteToInstance(from, this.wfResinst.Actinst, dao);
        }

        private void RefreshFrom(Actinst from)
        {
            WfFactory.GetWfActivity(from).ChangeStatus(WfStatusType.WF_RUNNING);
            from.FromCount--;
            WfResinst wfResinst = from.WfResinst;
            foreach (WfAssigninst assigninst in wfResinst.Assigns)
            {
                WfFactory.GetWfAssignment(assigninst).ChangeStatus(AssignStatusType.Accepted);
            }
        }

        private void SetWfResInst(WfLogicalPassContextData passData)
        {
            if (StringHelper.IsNull(this.wfResinst.StaffId))
            {
                this.wfResinst.StaffId = string.Empty;
                this.wfResinst.StaffName = string.Empty;
                if (passData.ToStaffIds != null)
                {
                    for (int i = 0; i < passData.ToStaffIds.Length; i++)
                    {
                        if (i == 0)
                        {
                            this.wfResinst.StaffId = this.wfResinst.StaffId + passData.ToStaffIds[i];
                            this.wfResinst.StaffName = this.wfResinst.StaffName + passData.ToStaffNames[i];
                        }
                        else
                        {
                            this.wfResinst.StaffId = this.wfResinst.StaffId + "," + passData.ToStaffIds[i];
                            this.wfResinst.StaffName = this.wfResinst.StaffName + "," + passData.ToStaffNames[i];
                        }
                    }
                }
            }
            if (StringHelper.IsNull(this.wfResinst.FromActinstId))
            {
                this.wfResinst.FromActinstId = passData.FromActivityKey;
                this.wfResinst.FromActinstName = passData.FromActivityName;
            }
            else if (this.wfResinst.FromActinstId != passData.FromActivityKey)
            {
                this.wfResinst.FromActinstId = this.wfResinst.FromActinstId + ',' + passData.FromActivityKey;
                this.wfResinst.FromActinstName = this.wfResinst.FromActinstName + ',' + passData.FromActivityName;
            }
            if (StringHelper.IsNull(this.wfResinst.FromStaffId))
            {
                this.wfResinst.FromStaffId = passData.FromStaffId;
                this.wfResinst.FromStaffName = passData.FromStaffName;
            }
            else
            {
                this.wfResinst.FromStaffId = this.wfResinst.FromStaffId + ',' + passData.FromStaffId;
                this.wfResinst.FromStaffName = this.wfResinst.FromStaffName + ',' + passData.FromStaffName;
            }
            if (StringHelper.IsNull(this.wfResinst.FromAssignId))
            {
                this.wfResinst.FromAssignId = passData.FromAssignId;
            }
            else
            {
                this.wfResinst.FromAssignId = passData.FromAssignId;
            }
            this.wfResinst.ExcludeStaffId = passData.ExcludeStaffId;
        }

        private void SetWfResInst(CParticipant part, Actinst actinst)
        {
            this.wfResinst.Actinst = actinst;
            this.wfResinst.ParticipantId = part.Id;
            this.wfResinst.ParticipantName = part.Name;
            this.wfResinst.IsAssigned = false;
            this.wfResinst.AssignRule = part.AssignRule;
            string type = part.ParticipantEntity.Type;
            this.wfResinst.ParticipantType = type;
            switch (type.ToUpper())
            {
                case "ROLE":
                    this.wfResinst.RoleId = part.ParticipantEntity.IdValue;
                    this.wfResinst.RoleName = part.Name;
                    return;

                case "DEPT":
                    this.wfResinst.DeptId = part.ParticipantEntity.IdValue;
                    this.wfResinst.DeptName = part.Name;
                    return;

                case "STAFF":
                {
                    this.wfResinst.StaffId = part.ParticipantEntity.IdValue;
                    CStaff staff = OGMService.GetStaff(this.wfResinst.StaffId);
                    if (staff != null)
                    {
                        this.wfResinst.StaffName = staff.Name;
                    }
                    return;
                }
                case "ALL":
                case "AUTO":
                    return;
            }
            throw new WfException("Not supported participant type");
        }

        public void Store()
        {
            IDA0 daoInstance = DaoUtil.GetDaoInstance("SkyMap.Net.Workflow");
            Actinst instanceObject = this.wfActivity.GetInstanceObject() as Actinst;
            if (instanceObject.WfResinst == null)
            {
                daoInstance.Put(this.wfResinst, DAOType.SAVE);
                instanceObject.WfResinst = this.wfResinst;
            }
            else
            {
                daoInstance.Put(this.wfResinst, DAOType.UPDATE);
            }
        }

        public void WorkItemCompleted()
        {
            foreach (WfAssigninst assigninst in this.wfResinst.Assigns)
            {
                if (assigninst.Status != AssignStatusType.Completed)
                {
                    return;
                }
            }
            this.wfActivity.Complete();
        }

        public bool IsAssigned
        {
            get
            {
                return this.wfResinst.IsAssigned;
            }
        }

        public string Key
        {
            get
            {
                return this.wfResinst.Id;
            }
        }

        public string Name
        {
            get
            {
                return this.wfResinst.Actinst.Name;
            }
        }

        public IWfActivity WfActivity
        {
            get
            {
                return this.wfActivity;
            }
        }
    }
}

