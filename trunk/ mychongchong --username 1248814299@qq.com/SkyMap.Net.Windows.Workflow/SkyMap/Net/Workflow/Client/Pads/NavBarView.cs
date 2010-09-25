namespace SkyMap.Net.Workflow.Client.Pads
{
    using DevExpress.XtraNavBar;
    using DevExpress.XtraNavBar.ViewInfo;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Config;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class NavBarView : AbstractPadContent
    {
        private NavBarControl navBar = new NavBarControl();

        public NavBarView()
        {
            this.navBar.HandleCreated += new EventHandler(this.navBar_HandleCreated);
        }

        private void InitNavBar()
        {
            this.navBar.Groups.Clear();
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("正在创建工作台...");
            }
            this.navBar.AllowDrop = false;
            this.navBar.View = new StandardSkinExplorerBarViewInfoRegistrator("Caramel");
            this.navBar.Dock = DockStyle.Fill;
            IList<CNavBarGroupConfig> navBarGroups = CControlConfig.GetNavBarGroups();
            this.navBar.Groups.Clear();
            for (int i = 0; i < navBarGroups.Count; i++)
            {
                CNavBarGroupConfig config = navBarGroups[i];
                NavBarGroup group = this.navBar.Groups.Add();
                group.Caption = config.Caption;
                if (i == 0)
                {
                    group.Expanded = true;
                }
                try
                {
                }
                catch (ResourceNotFoundException exception)
                {
                    LoggingService.Error("Cannot find image:" + config.Image + ";\n" + exception.Message);
                }
                foreach (CNavBarItemConfig config2 in config.Items)
                {
                    if (!(!config2.IfAuth || this.TestAuth()))
                    {
                        break;
                    }
                    SmNavBarItem item = new SmNavBarItem();
                    group.ItemLinks.Add(item);
                    item.BoxName = config2.Caption;
                    item.Link = config2.Link;
                    item.Class = config2.Class;
                    item.IfAuth = config2.IfAuth;
                    CBoxConfig boxConfig = CControlConfig.GetBoxConfig(config2.Caption);
                    if (((boxConfig != null) && (boxConfig.QueryCountName != null)) && (boxConfig.QueryCountName.Length > 0))
                    {
                        item.Caption = config2.Caption + "...";
                        item.Refresh();
                    }
                    else
                    {
                        item.Caption = config2.Caption;
                    }
                    try
                    {
                        Image image = new Bitmap(ResourceService.GetBitmap(config2.Image));
                        item.SmallImage = image;
                    }
                    catch (Exception exception2)
                    {
                        LoggingService.Error("Cannot find image:" + config.Image + ";\n" + exception2.Message);
                    }
                }
            }
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.NavbarItemCaptionRefresh);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("创建工作台完成...");
            }
        }

        private void navBar_HandleCreated(object sender, EventArgs e)
        {
            this.navBar.BeginInvoke(new Action(this.InitNavBar));
        }

        private void NavbarItemCaptionRefresh(object sender, EventArgs e)
        {
            this.navBar.BeginInvoke(new Action(this.InitNavBar));
        }

        private bool TestAuth()
        {
            AdminLevelType adminLevel = SecurityUtil.GetSmIdentity().AdminLevel;
            return ((adminLevel == AdminLevelType.Admin) || (adminLevel == AdminLevelType.AdminData));
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.navBar;
            }
        }
    }
}

