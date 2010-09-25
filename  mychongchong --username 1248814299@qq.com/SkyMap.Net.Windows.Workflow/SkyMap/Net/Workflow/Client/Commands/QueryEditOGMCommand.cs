namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class QueryEditOGMCommand : QueryEditCommand, IPopupEditCommand, ICommand
    {
        private TreeNode currentNode;
        private OGMTreeView ogmTV;

        public QueryEditOGMCommand()
        {
            this.IsEnabled = true;
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
        }

        public override void AddToQueryParameters(Dictionary<string, string> queryParameters)
        {
            if (this.IsEnabled)
            {
                queryParameters.Add(this.Key, string.Format("({0} in {1} or {2} in {1})", "s.STAFF_ID", this.Value, "r.STAFF_ID"));
            }
        }

        private bool CheckAuthDept(CDept dept)
        {
            if (SecurityUtil.IsCanAccess("AdminData,Admin"))
            {
                return true;
            }
            ArrayList list = new ArrayList();
            list.Add(dept.Id);
            for (CDept dept2 = dept.Parent; dept2 != null; dept2 = dept2.Parent)
            {
                list.Add(dept2.Id);
            }
            IList<string> currentPrincipalAuthResourcesByType = null;
            try
            {
                currentPrincipalAuthResourcesByType = SecurityUtil.GetCurrentPrincipalAuthResourcesByType<string>("YWCX_DEPT");
            }
            catch (Exception exception)
            {
                LoggingService.ErrorFormatted("发生安全异常：{0}\r\n{1}", new object[] { exception.Message, exception.StackTrace });
                return false;
            }
            foreach (string str in list)
            {
                if (currentPrincipalAuthResourcesByType.Contains(str))
                {
                    return true;
                }
            }
            return false;
        }

        private void ogmTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.ogmTV.AfterSelect -= new TreeViewEventHandler(this.ogmTV_AfterSelect);
            bool flag = false;
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            object obj2 = this.ogmTV.QueryValue();
            if (obj2 is CDept)
            {
                if (this.CheckAuthDept(obj2 as CDept))
                {
                    flag = true;
                }
            }
            else if (obj2 is CStaffDept)
            {
                CStaffDept dept = obj2 as CStaffDept;
                if (dept.Staff.Id == (smPrincipal.Identity as SmIdentity).UserId)
                {
                    flag = true;
                }
                else if (this.CheckAuthDept(dept.Dept))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                this.currentNode = e.Node;
                this.Run();
            }
            else
            {
                this.ogmTV.SelectedNode = this.currentNode;
                MessageHelper.ShowInfo("您没有权限查询该部门或个人业务的权限!");
            }
            (this.Owner as ToolBarPopupEdit).EditValue = this.ogmTV.QueryValue();
            this.ogmTV.AfterSelect += new TreeViewEventHandler(this.ogmTV_AfterSelect);
        }

        private void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            this.ogmTV.SelectStaffInDept(smPrincipal.DeptIds[0], (smPrincipal.Identity as SmIdentity).UserId);
            this.currentNode = this.ogmTV.SelectedNode;
            this.Run();
        }

        public override string Key
        {
            get
            {
                return "s.STAFF_ID";
            }
        }

        public override string Operator
        {
            get
            {
                return " in ";
            }
        }

        public override object Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                this.ogmTV = new OGMTreeView();
                this.ogmTV.InitOGMTree();
                SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
                this.ogmTV.SelectStaffInDept(smPrincipal.DeptIds[0], (smPrincipal.Identity as SmIdentity).UserId);
                this.ogmTV.Dock = DockStyle.Fill;
                this.currentNode = this.ogmTV.SelectedNode;
                (value as ToolBarPopupEdit).Control.Width = 200;
                (value as ToolBarPopupEdit).PopupControl.Controls.Add(this.ogmTV);
                this.Run();
                this.ogmTV.AfterSelect += new TreeViewEventHandler(this.ogmTV_AfterSelect);
            }
        }

        public override string Value
        {
            get
            {
                string str = string.Empty;
                if (this.ogmTV != null)
                {
                    object obj2 = this.ogmTV.QueryValue();
                    if (obj2 is CDept)
                    {
                        CDept dept = obj2 as CDept;
                        List<CStaff> staffs = OGMService.GetStaffs(dept, true);
                        int count = staffs.Count;
                        for (int i = 0; i < count; i++)
                        {
                            CStaff staff = staffs[i];
                            if (i == 0)
                            {
                                str = "(";
                            }
                            else
                            {
                                str = str + ",";
                            }
                            str = str + "'" + staff.Id + "'";
                            if (i == (count - 1))
                            {
                                str = str + ")";
                            }
                        }
                        return str;
                    }
                    if (obj2 is CStaffDept)
                    {
                        str = "('" + (obj2 as CStaffDept).Staff.Id + "')";
                    }
                }
                return str;
            }
        }
    }
}

