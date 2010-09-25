namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    public class TaskTreeNode : ObjectNode
    {
        private AbstractCommand addCommand;
        private Assembly assembly;
        private static Dictionary<string, Assembly> assemblys = new Dictionary<string, Assembly>();
        private AbstractCommand loadChildrenCommand;
        private AbstractCommand pasteCommand;
        private AbstractCommand selectCommand;
        private TaskNode taskNode;

        public TaskTreeNode(TaskNode taskNode)
        {
            this.taskNode = taskNode;
            base.Text = taskNode.Name;
            if (!string.IsNullOrEmpty(taskNode.ContextmenuAddinTreePath))
            {
                this.ContextmenuAddinTreePath = taskNode.ContextmenuAddinTreePath;
            }
            if (!string.IsNullOrEmpty(taskNode.Icon))
            {
                base.SetIcon(taskNode.Icon);
            }
            lock (assemblys)
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("任务节点的程序集是：{0}", new object[] { taskNode.Assembly });
                }
                if (!string.IsNullOrEmpty(taskNode.Assembly))
                {
                    if (!assemblys.ContainsKey(taskNode.Assembly))
                    {
                        this.assembly = Assembly.Load(taskNode.Assembly);
                        assemblys.Add(taskNode.Assembly, this.assembly);
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("加载了组件:{0}", new object[] { taskNode.Assembly });
                        }
                    }
                    else
                    {
                        this.assembly = assemblys[taskNode.Assembly];
                    }
                }
            }
            if (!(((taskNode.Children.Count <= 0) && !this.EnableAddChild) && string.IsNullOrEmpty(taskNode.LoadChildrenCommand)))
            {
                base.Nodes.Add(new TreeNode("Loading"));
            }
        }

        public override ObjectNode AddChild()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("尝试添加子节点命令：{0}", new object[] { this.taskNode.AddCommand });
            }
            if (((this.assembly != null) && (this.addCommand == null)) && !string.IsNullOrEmpty(this.taskNode.AddCommand))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("创建添加子节点命令：{0}", new object[] { this.taskNode.AddCommand });
                }
                this.addCommand = (AbstractCommand) this.assembly.CreateInstance(this.taskNode.AddCommand, true); 
            }
            if (this.addCommand != null)
            {
                this.addCommand.Owner = this;
                this.addCommand.Run();
            }
            return null;
        }

        public override void Expanding()
        {
            if (!base.isInitialized)
            {
                base.isInitialized = true;
                if (base.autoClearNodes)
                {
                    base.Nodes.Clear();
                }
                this.Initialize();
                base.UpdateVisibility();
            }
        }

        public static bool IfAuthNode(TaskNode tn)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            SmIdentity identity = (SmIdentity) smPrincipal.Identity;
            return (((identity.AdminLevel == AdminLevelType.Admin) || (identity.AdminLevel == AdminLevelType.AdminDef)) || smPrincipal.IsInRole(tn.Name));
        }

        protected override void Initialize()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("节点‘{0}’ Initialize...", new object[] { base.Text });
            }
            base.Initialize();
            if ((this.taskNode.Children != null) && (this.taskNode.Children.Count > 0))
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.Debug("自动加载预定义的子节点...");
                }
                foreach (TaskNode node in this.taskNode.Children)
                {
                    if (IfAuthNode(node))
                    {
                        new TaskTreeNode(node).AddTo(this);
                    }
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("准备调用命令'{0}'来加载子节点...", new object[] { this.taskNode.LoadChildrenCommand });
            }
            if (!string.IsNullOrEmpty(this.taskNode.LoadChildrenCommand))
            {
                if (this.assembly != null)
                {
                    if (this.loadChildrenCommand == null)
                    {
                        this.loadChildrenCommand = (AbstractCommand) this.assembly.CreateInstance(this.taskNode.LoadChildrenCommand, true);
                    }
                    if (this.loadChildrenCommand != null)
                    {
                        this.loadChildrenCommand.Owner = this;
                        this.loadChildrenCommand.Run();
                    }
                    else
                    {
                        MessageHelper.ShowInfo("不能创建命令:{0}", this.taskNode.LoadChildrenCommand);
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("找不到命名所在程序:{0}", this.taskNode.Assembly);
                }
            }
        }

        public override void Paste()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("Will run paste command : {0}", new object[] { this.taskNode.PasteCommand });
            }
            if (!(((this.pasteCommand != null) || (this.assembly == null)) || string.IsNullOrEmpty(this.taskNode.PasteCommand)))
            {
                this.pasteCommand = (AbstractCommand) this.assembly.CreateInstance(this.taskNode.PasteCommand);
                this.pasteCommand.Owner = this;
            }
            if (this.pasteCommand != null)
            {
                this.pasteCommand.Run();
            }
        }

        public override void SaveAs()
        {
            if (MessageHelper.ShowYesNoInfo("是否要依次另存其下各节点到另一个数据库？") == DialogResult.Yes)
            {
                foreach (ObjectNode node in base.Nodes)
                {
                    node.SaveAs();
                }
            }
        }

        public override void Select()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("Will run select command : {0}", new object[] { this.taskNode.SelectCommand });
            }
            if (!(((this.selectCommand != null) || (this.assembly == null)) || string.IsNullOrEmpty(this.taskNode.SelectCommand)))
            {
                this.selectCommand = (AbstractCommand) this.assembly.CreateInstance(this.taskNode.SelectCommand);
                this.selectCommand.Owner = this;
            }
            if (this.selectCommand != null)
            {
                this.selectCommand.Run();
            }
        }

        public override bool EnableAddChild
        {
            get
            {
                return ((this.assembly != null) && !string.IsNullOrEmpty(this.taskNode.AddCommand));
            }
        }

        public override bool EnablePaste
        {
            get
            {
                return !string.IsNullOrEmpty(this.taskNode.PasteCommand);
            }
        }
    }
}

