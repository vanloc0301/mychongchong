namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Instance;
    using System;

    public class AddMaterCommand : MatersEditCommand
    {
        public override void Run()
        {
            WfProinstMater mater = new WfProinstMater();
            mater.Id = StringHelper.GetNewGuid();
            mater.Name = "新增材料";
            mater.ProinstId = base.me.ProinstId;
            mater.OldNum = 1;
            mater.DupliNum = 0;
            mater.Description = string.Empty;
            mater.Selected = true;
            base.me.CurrentUnitOfWork.RegisterNew(mater);
            base.me.AddMater(mater);
        }
    }
}

