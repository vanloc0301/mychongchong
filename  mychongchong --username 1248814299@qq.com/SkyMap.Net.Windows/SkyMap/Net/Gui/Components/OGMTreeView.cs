namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    public class OGMTreeView : TreeView, SkyMap.Net.Gui.Components.IContainerControl
    {
        private const string csuffix = ".cfg.cxml";
        private int mImageIndexOfDept = 0;
        private int mImageIndexOfStaff = 1;
        private const string suffix = ".cfg.xml";

        public OGMTreeView()
        {
            ImageList list = new ImageList();
            list.ImageSize = new Size(0x10, 0x10);
            list.Images.Add(ResourceService.GetIcon("ICON.OGM.Dept"));
            list.Images.Add(ResourceService.GetIcon("ICON.OGM.Staff"));
            base.Size = new Size(200, 200);
            base.ImageList = list;
        }

        private TreeNode AddDeptNode(TreeNode parentNode, CDept dept)
        {
            if (this.LoadAllDept() || dept.IsActive)
            {
                this.ForceCreateControl();
                return (base.Invoke(new AddNode(this.AddNodeDelegate), new object[] { (parentNode == null) ? base.Nodes : parentNode.Nodes, dept.Name, this.mImageIndexOfDept, dept, true }) as TreeNode);
            }
            return null;
        }

        private TreeNode AddDeptStaffNode(TreeNode parentNode, CStaffDept sd)
        {
            this.ForceCreateControl();
            return (base.Invoke(new AddNode(this.AddNodeDelegate), new object[] { parentNode.Nodes, sd.Staff.Name, this.mImageIndexOfStaff, sd, false }) as TreeNode);
        }

        private TreeNode AddNodeDelegate(TreeNodeCollection nodes, string text, int imageindex, object tag, bool haschild)
        {
            TreeNode node = nodes.Add(text);
            node.ImageIndex = imageindex;
            node.SelectedImageIndex = imageindex;
            node.Tag = tag;
            if (haschild)
            {
                node.Nodes.Add("Load...");
            }
            return node;
        }

        private TreeNode AddStaffNode(TreeNodeCollection nc, CStaff staff)
        {
            this.ForceCreateControl();
            if (base.IsHandleCreated)
            {
                return (base.Invoke(new AddNode(this.AddNodeDelegate), new object[] { nc, staff.Name, this.mImageIndexOfStaff, staff, false }) as TreeNode);
            }
            return null;
        }

        protected void ForceCreateControl()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("将验证控件句构是否已经创建，如果没有将等待直到控件句柄创建...");
            }
            for (int i = 0; !base.IsHandleCreated && (i < 0x2710); i++)
            {
                Form form = base.FindForm();
                if (!((form == null) || form.IsHandleCreated))
                {
                    form.CreateControl();
                }
                base.CreateControl();
            }
        }

        public void InitOGMTree()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("初始化部门人员列表...");
            }
            IList<CDept> topDepts = OGMService.GetTopDepts();
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("获取了顶级部门人员列表...");
            }
            this.RefreshDeptTree(topDepts);
        }

        public void InitStaffsTree(IList<CStaff> staffs)
        {
            base.Nodes.Clear();
            foreach (CStaff staff in staffs)
            {
                this.AddStaffNode(base.Nodes, staff);
            }
        }

        public void InitStaffsTree(string groupName, IList<CStaff> staffs)
        {
            base.Nodes.Clear();
            TreeNode node = base.Nodes.Add(groupName);
            foreach (CStaff staff in staffs)
            {
                this.AddStaffNode(node.Nodes, staff);
            }
            node.Expand();
        }

        private bool LoadAllDept()
        {
            try
            {
                return Convert.ToBoolean(ConfigurationSettings.AppSettings["LoadAllDept"]);
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            return true;
        }

        private void LoadDeptChildren(TreeNode node, CDept dept)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("加载部门:{0}的子部门与人员", new object[] { dept.Name });
            }
            node.Nodes.Clear();
            foreach (CDept dept2 in dept.Children)
            {
                this.AddDeptNode(node, dept2);
            }
            foreach (CStaffDept dept3 in OGMService.LazyLoadStaffDepts(dept))
            {
                this.AddDeptStaffNode(node, dept3);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (!e.Node.IsVisible)
            {
                e.Node.EnsureVisible();
            }
            base.OnAfterSelect(e);
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            if (((node.Tag is CDept) && (node.Nodes.Count == 1)) && (node.Nodes[0].Text == "Load..."))
            {
                CDept tag = (CDept) node.Tag;
                this.LoadDeptChildren(node, tag);
            }
            base.OnBeforeExpand(e);
        }

        private bool QueryObject(TreeNodeCollection nodes, object obj)
        {
            foreach (TreeNode node in nodes)
            {
                if (obj.Equals(node.Tag))
                {
                    base.SelectedNode = node;
                    return true;
                }
                return this.QueryObject(node.Nodes, obj);
            }
            return false;
        }

        public bool QueryPopup(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return (((base.SelectedNode != null) && base.SelectedNode.Tag.Equals(obj)) || this.QueryObject(base.Nodes, obj));
        }

        public object QueryValue()
        {
            if (base.SelectedNode != null)
            {
                return base.SelectedNode.Tag;
            }
            return null;
        }

        private void RefreshDeptTree(IList<CDept> noParentDepts)
        {
            base.Nodes.Clear();
            foreach (CDept dept in noParentDepts)
            {
                this.AddDeptNode(null, dept);
            }
        }

        public void SelectDept(string id)
        {
            CDept targetDept = OGMService.GetDept(id);
            this.SelectDept(base.Nodes, targetDept);
        }

        private void SelectDept(TreeNodeCollection nodes, CDept targetDept)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("在列表中查找部门:{0}", new object[] { targetDept.Name });
            }
            List<CDept> list = new List<CDept>();
            list.Add(targetDept);
            while (!string.IsNullOrEmpty(list[0].ParentID))
            {
                list.Insert(0, list[0].Parent);
            }
            foreach (CDept dept in list)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    TreeNode node = nodes[i];
                    if (node.Tag is CDept)
                    {
                        CDept tag = (CDept) node.Tag;
                        if (tag.Id == dept.Id)
                        {
                            if ((node.Nodes.Count == 1) && (node.Nodes[0].Text == "Load..."))
                            {
                                this.LoadDeptChildren(node, dept);
                            }
                            if (targetDept.Id == tag.Id)
                            {
                                base.SelectedNode = node;
                                return;
                            }
                            nodes = node.Nodes;
                            break;
                        }
                    }
                }
            }
        }

        public void SelectStaff(string staffId)
        {
            this.SelectStaff(base.Nodes, staffId);
        }

        private void SelectStaff(TreeNodeCollection nodes, string id)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is CStaffDept)
                {
                    CStaffDept tag = (CStaffDept) node.Tag;
                    if (tag.Staff.Id == id)
                    {
                        base.SelectedNode = node;
                        break;
                    }
                }
                if (node.Tag is CStaff)
                {
                    CStaff staff = node.Tag as CStaff;
                    if (staff.Id == id)
                    {
                        base.SelectedNode = node;
                        break;
                    }
                }
                else
                {
                    this.SelectStaff(node.Nodes, id);
                }
            }
        }

        public object SelectStaffInDept(string deptId, string staffId)
        {
            CDept targetDept = OGMService.GetDept(deptId);
            if (targetDept != null)
            {
                this.SelectDept(base.Nodes, targetDept);
                if (base.SelectedNode != null)
                {
                    this.SelectStaff(base.SelectedNode.Nodes, staffId);
                }
                if ((base.SelectedNode != null) && (base.SelectedNode.Tag is CStaffDept))
                {
                    return base.SelectedNode.Tag;
                }
            }
            return null;
        }
    }
}

