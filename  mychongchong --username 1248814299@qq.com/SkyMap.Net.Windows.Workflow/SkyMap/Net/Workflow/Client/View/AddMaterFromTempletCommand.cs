namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;

    public class AddMaterFromTempletCommand : MatersEditCommand
    {
        internal static WfProinstMater AddMaterFormTemplet(WfAppendix appendix, string proinstId)
        {
            WfProinstMater mater = new WfProinstMater();
            mater.Id = StringHelper.GetNewGuid();
            mater.ProinstId = proinstId;
            mater.Name = appendix.Name;
            mater.OldNum = appendix.OldNum;
            mater.DupliNum = appendix.DupliNum;
            mater.Description = appendix.Description;
            mater.Selected = true;
            return mater;
        }

        public override void Run()
        {
            List<WfAppendix> selectedOrFocusAppendixs = base.me.GetSelectedOrFocusAppendixs();
            foreach (WfAppendix appendix in selectedOrFocusAppendixs)
            {
                WfProinstMater mater = AddMaterFormTemplet(appendix, base.me.ProinstId);
                base.me.CurrentUnitOfWork.RegisterNew(mater);
                base.me.AddMater(mater);
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return (base.me.GetSelectedOrFocusAppendixs().Count > 0);
            }
            set
            {
            }
        }
    }
}

