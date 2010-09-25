namespace SkyMap.Net.Workflow.Client.Pads
{
    using DevExpress.XtraNavBar;
    using SkyMap.Net.Commands;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Config;
    using SkyMap.Net.Workflow.Client.Services;
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class SmNavBarItem : NavBarItem
    {
        private string _boxName;
        private string _class;
        private string _link;
        public bool IfAuth;

        protected override void RaiseLinkClicked(NavBarItemLink link)
        {
            WaitDialogHelper.Show();
            try
            {
                if ((this._link != null) && (this._link.Length > 0))
                {
                    new LinkCommand(this.Link).Run();
                }
                else if (StringHelper.IsNull(this._class))
                {
                    BoxService.OpenBox(this._boxName);
                    this.Refresh();
                }
                else
                {
                    ICommand command2 = (ICommand) Assembly.GetExecutingAssembly().CreateInstance(this._class, true);
                    if (command2 == null)
                    {
                        Type type = Type.GetType(this._class);
                        if (type != null)
                        {
                            command2 = (ICommand) Activator.CreateInstance(type);
                        }
                    }
                    if (command2 != null)
                    {
                        command2.Owner = this;
                        command2.Run();
                    }
                }
            }
            finally
            {
                WaitDialogHelper.Close();
            }
            base.RaiseLinkClicked(link);
        }

        public void Refresh()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate (object senderd, DoWorkEventArgs ed) {
                CBoxConfig boxConfig = CControlConfig.GetBoxConfig(this._boxName);
                if (((boxConfig != null) && (boxConfig.QueryCountName != null)) && (boxConfig.QueryCountName.Length > 0))
                {
                    try
                    {
                        int ywnum = BoxHelper.GetDataCount(boxConfig);
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.InfoFormatted("获取了‘{0}’的业务数量为{1}", new object[] { this._boxName, ywnum });
                        }
                        Action method = delegate {
                            this.NavBar.BeginUpdate();
                            this.Caption = (ywnum >= 0) ? string.Format("{0}({1})", this._boxName, ywnum) : this._boxName;
                            this.NavBar.EndUpdate();
                        };
                        base.NavBar.BeginInvoke(method);
                    }
                    catch
                    {
                    }
                }
            };
            worker.RunWorkerCompleted += delegate (object senderw, RunWorkerCompletedEventArgs ea) {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("异步加载工作箱业务数量完成");
                }
                if (ea.Error != null)
                {
                    LoggingService.Error("异步加载工作箱业务数量发生错误", ea.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        public string BoxName
        {
            get
            {
                return this._boxName;
            }
            set
            {
                this._boxName = value;
            }
        }

        public string Class
        {
            get
            {
                return this._class;
            }
            set
            {
                this._class = value;
            }
        }

        public string Link
        {
            get
            {
                return this._link;
            }
            set
            {
                this._link = value;
            }
        }
    }
}

