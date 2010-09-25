namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using System.Windows.Forms;

    public class RoleNode : AbstractDomainObjectNode<CRole>
    {
        public override ObjectNode AddChild()
        {
            return null;
        }

        private void AddNodeFromDataObject(IDataObject dataObject)
        {
            if (this.GetDataPresent(dataObject))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("'可以执行Copy ...", new object[] { base.Text });
                }
                if (dataObject.GetDataPresent(typeof(CStaff)))
                {
                    CStaff data = (CStaff) dataObject.GetData(typeof(CStaff));
                    this.AddStaffNode(data);
                }
                else if (dataObject.GetDataPresent(typeof(CDept)))
                {
                    CDept dept = (CDept) dataObject.GetData(typeof(CDept));
                    this.AddNodeFromDept(dept);
                }
            }
        }

        private void AddNodeFromDept(CDept dept)
        {
            foreach (CStaffDept dept2 in dept.StaffDepts)
            {
                if (dept2.Staff != null)
                {
                    this.AddStaffNode(dept2.Staff);
                }
            }
            if ((dept.Children != null) && (dept.Children.Count > 0))
            {
                foreach (CDept dept3 in dept.Children)
                {
                    this.AddNodeFromDept(dept3);
                }
            }
        }

        private void AddStaffNode(CStaff staff)
        {
            if (!base.DomainObject.Contains(staff))
            {
                CStaffRole role = OGMDAOService.Instance.CreateStaffRole(staff, base.DomainObject);
                base.AddSingleNode<CStaffRole, StaffRoleNode>(role);
            }
        }

        public override void DoDragDrop(IDataObject dataObject, DragDropEffects effect)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("'{0}' Do Drag Drop ...", new object[] { base.Text });
            }
            this.Expanding();
            this.AddNodeFromDataObject(dataObject);
        }

        private bool GetDataPresent(IDataObject dataObject)
        {
            return (dataObject.GetDataPresent(typeof(CStaff)) || dataObject.GetDataPresent(typeof(CDept)));
        }

        public override DragDropEffects GetDragDropEffect(IDataObject dataObject, DragDropEffects proposedEffect)
        {
            if (this.GetDataPresent(dataObject))
            {
                return DragDropEffects.Copy;
            }
            return DragDropEffects.None;
        }

        protected override void Initialize()
        {
            base.AddNodes<CStaffRole, StaffRoleNode>(base.DomainObject.StaffRoles);
        }

        public override void Paste()
        {
            IDataObject dataObject = ClipboardWrapper.GetDataObject();
            this.AddNodeFromDataObject(dataObject);
        }
    }
}

