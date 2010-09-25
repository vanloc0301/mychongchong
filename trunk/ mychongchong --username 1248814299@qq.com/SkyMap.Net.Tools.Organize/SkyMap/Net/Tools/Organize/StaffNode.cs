namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using System.Windows.Forms;

    public class StaffNode : AbstractDomainObjectNode<CStaff>
    {
        public override ObjectNode AddChild()
        {
            return null;
        }

        public override void Delete()
        {
            if (base.DomainObject.StaffDepts.Count == 1)
            {
                base.Delete();
            }
            else if (base.DomainObject.StaffDepts.Count > 1)
            {
                CDept domainObject = ((DeptNode) base.Parent).DomainObject;
                foreach (CStaffDept dept2 in domainObject.StaffDepts)
                {
                    if (dept2.Staff.Equals(base.DomainObject))
                    {
                        domainObject.StaffDepts.Remove(dept2);
                        OGMDAOService.Instance.Remove(dept2);
                        base.Remove();
                    }
                }
            }
            else
            {
                MessageHelper.ShowInfo("There a error");
            }
        }

        public override DataObject DragDropDataObject
        {
            get
            {
                return new DataObject(base.DomainObject);
            }
        }

        public override bool EnableAddChild
        {
            get
            {
                return false;
            }
        }
    }
}

