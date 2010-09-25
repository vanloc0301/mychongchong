namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class TaskNode : DomainObject, ISaveAs
    {
        private string addCommand;
        private string assembly;
        private IList<TaskNode> children;
        private string contextmenuAddinTreePath;
        private string icon;
        private string loadChildrenCommand;
        private TaskNode parent;
        private string pasteCommand;
        private string selectCommand;

        public TaskNode()
        {
        }

        public TaskNode(string name, TaskNode parent) : base(name)
        {
            this.parent = parent;
        }

        public static List<TaskNode> GetTopTaskNode()
        {
            string key = "ALL_TaskNode";
            if (!DAOCacheService.Contains(key))
            {
                DAOCacheService.Put(key, QueryHelper.List<TaskNode>(key, new string[] { "DisplayOrder" }));
            }
            IList list = (IList) DAOCacheService.Get(key);
            List<TaskNode> list2 = new List<TaskNode>();
            foreach (TaskNode node in list)
            {
                if ((node.parent == null) && node.IsActive)
                {
                    list2.Add(node);
                }
            }
            return list2;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            unitOfWork.RegisterNew<TaskNode>(this.Children);
        }

        [DisplayName("添加COMMAND")]
        public string AddCommand
        {
            get
            {
                return this.addCommand;
            }
            set
            {
                this.addCommand = value;
            }
        }

        [DisplayName("Command所在程序集")]
        public string Assembly
        {
            get
            {
                return this.assembly;
            }
            set
            {
                this.assembly = value;
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.InfoFormatted("Assembly设置值为：{0}", new object[] { value });
                }
            }
        }

        [Browsable(false)]
        public IList<TaskNode> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                this.children = value;
            }
        }

        [DisplayName("节点右键菜单路径")]
        public string ContextmenuAddinTreePath
        {
            get
            {
                return this.contextmenuAddinTreePath;
            }
            set
            {
                this.contextmenuAddinTreePath = value;
            }
        }

        [DisplayName("图标")]
        public string Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
            }
        }

        [DisplayName("加载子节点COMMAND")]
        public string LoadChildrenCommand
        {
            get
            {
                return this.loadChildrenCommand;
            }
            set
            {
                this.loadChildrenCommand = value;
            }
        }

        [Browsable(false)]
        public TaskNode Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        [DisplayName("粘贴COMMAND")]
        public string PasteCommand
        {
            get
            {
                return this.pasteCommand;
            }
            set
            {
                this.pasteCommand = value;
            }
        }

        [DisplayName("选取时COMMAND")]
        public string SelectCommand
        {
            get
            {
                return this.selectCommand;
            }
            set
            {
                this.selectCommand = value;
            }
        }
    }
}

