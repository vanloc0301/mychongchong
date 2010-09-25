namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class Actdef : AbstractWfElement
    {
        private WfActdefFormPermission actdefFormPermission;
        private string applicationId = string.Empty;
        private string applicationName = string.Empty;
        private IList<Transition> froms;
        private bool isDefaultInit;
        private bool isSubflowSync;
        private double limit;
        private int mnMergeNum;
        private string participantId = string.Empty;
        private string participantName = string.Empty;
        private bool passNeedInteraction;
        private SkyMap.Net.Workflow.XPDL.Prodef prodef;
        private bool status;
        private string subflowProdefId = string.Empty;
        private string subflowProdefName = string.Empty;
        private IList<Transition> tos;
        private ActdefType type;
        private int xPos;
        private int yPos;

        [DisplayName("表单权限"), Description("使用'F1'快捷键可以打开表单权限编辑窗口")]
        public WfActdefFormPermission ActdefFormPermission
        {
            get
            {
                return this.actdefFormPermission;
            }
            set
            {
                this.actdefFormPermission = value;
            }
        }

        [Browsable(false)]
        public string ApplicationId
        {
            get
            {
                return this.applicationId;
            }
            set
            {
                this.applicationId = value;
            }
        }

        [Description("使用'F1'快捷键可以选择外部应用程序"), DisplayName("外部应用名称")]
        public string ApplicationName
        {
            get
            {
                return this.applicationName;
            }
            set
            {
                this.applicationName = value;
            }
        }

        [Browsable(false)]
        public IList<Transition> Froms
        {
            get
            {
                return this.froms;
            }
            set
            {
                this.froms = value;
            }
        }

        [DisplayName("缺省起始活动")]
        public bool IsDefaultInit
        {
            get
            {
                return this.isDefaultInit;
            }
            set
            {
                this.isDefaultInit = value;
            }
        }

        [DisplayName("同步执行子流程")]
        public bool IsSubflowSync
        {
            get
            {
                return this.isSubflowSync;
            }
            set
            {
                this.isSubflowSync = value;
            }
        }

        [DisplayName("期限")]
        public double Limit
        {
            get
            {
                return this.limit;
            }
            set
            {
                this.limit = value;
            }
        }

        [DisplayName("MN汇聚的N值")]
        public int MNMergeNum
        {
            get
            {
                return this.mnMergeNum;
            }
            set
            {
                this.mnMergeNum = value;
            }
        }

        [Browsable(false)]
        public string ParticipantId
        {
            get
            {
                return this.participantId;
            }
            set
            {
                this.participantId = value;
            }
        }

        [DisplayName("参与考"), Description("使用'F1'快捷键可以选择活动参与者")]
        public string ParticipantName
        {
            get
            {
                return this.participantName;
            }
            set
            {
                this.participantName = value;
            }
        }

        [DisplayName("转出时手动选择")]
        public bool PassNeedInteraction
        {
            get
            {
                return this.passNeedInteraction;
            }
            set
            {
                this.passNeedInteraction = value;
            }
        }

        [Browsable(false)]
        public SkyMap.Net.Workflow.XPDL.Prodef Prodef
        {
            get
            {
                return this.prodef;
            }
            set
            {
                this.prodef = value;
            }
        }

        [DisplayName("有效")]
        public bool Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        [Browsable(false)]
        public string SubflowProdefId
        {
            get
            {
                return this.subflowProdefId;
            }
            set
            {
                this.subflowProdefId = value;
            }
        }

        [DisplayName("子流程"), Description("使用'F1'快捷键可以选择子流程")]
        public string SubflowProdefName
        {
            get
            {
                return this.subflowProdefName;
            }
            set
            {
                this.subflowProdefName = value;
            }
        }

        [Browsable(false)]
        public IList<Transition> Tos
        {
            get
            {
                return this.tos;
            }
            set
            {
                this.tos = value;
            }
        }

        [DisplayName("类型")]
        public ActdefType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        [DisplayName("XPos")]
        public int XPos
        {
            get
            {
                return this.xPos;
            }
            set
            {
                this.xPos = value;
            }
        }

        [DisplayName("YPos")]
        public int YPos
        {
            get
            {
                return this.yPos;
            }
            set
            {
                this.yPos = value;
            }
        }
    }
}

