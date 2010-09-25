namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using System.Windows.Forms;

    public class DeptNode : AbstractDomainObjectNode<CDept>
    {
        public override ObjectNode AddChild()
        {
            CStaff staff = OGMDAOService.Instance.CreateStaff(base.DomainObject);
            return base.AddSingleNode<CStaff, StaffNode>(staff);
        }

        public override void DoDragDrop(IDataObject dataObject, DragDropEffects effect)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("'{0}' Do Drag Drop ...", new object[] { base.Text });
            }
            this.Expanding();
            if (dataObject.GetDataPresent(typeof(StaffNode)))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("'可以执行Copy ...", new object[] { base.Text });
                }
                StaffNode data = (StaffNode) dataObject.GetData(typeof(StaffNode));
                CStaff domainObject = data.DomainObject;
                if (!base.DomainObject.Contains(domainObject))
                {
                    base.AddSingleNode<CStaff, StaffNode>(domainObject);
                    CStaffDept dept = OGMDAOService.Instance.CreateStaffDept(domainObject, base.DomainObject);
                }
            }
        }

        public override DragDropEffects GetDragDropEffect(IDataObject dataObject, DragDropEffects proposedEffect)
        {
            if (dataObject.GetDataPresent(typeof(StaffNode)))
            {
                StaffNode data = (StaffNode) dataObject.GetData(typeof(StaffNode));
                CStaff domainObject = data.DomainObject;
                if (!base.DomainObject.Contains(domainObject))
                {
                    return DragDropEffects.Copy;
                }
            }
            return DragDropEffects.None;
        }

        protected override void Initialize()
        {
            if ((base.DomainObject.Children != null) && (base.DomainObject.Children.Count > 0))
            {
                base.AddNodes<CDept, DeptNode>(base.DomainObject.Children);
            }
            foreach (CStaffDept dept in base.DomainObject.StaffDepts)
            {
                if (dept.Staff != null)
                {
                    base.AddSingleNode<CStaff, StaffNode>(dept.Staff);
                }
            }
        }

        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/DeptContextMenu";
            }
            set
            {
            }
        }
    }
}

