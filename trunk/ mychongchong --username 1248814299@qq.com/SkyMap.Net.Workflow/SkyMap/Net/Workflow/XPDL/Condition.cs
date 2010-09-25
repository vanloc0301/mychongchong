namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class Condition : AbstractWfElement
    {
        private SkyMap.Net.Workflow.XPDL.Application application;
        private string type;
        private string xpression;

        [DisplayName("应用程序")]
        public SkyMap.Net.Workflow.XPDL.Application Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
            }
        }

        [StringsDropDown(dataSource=new string[] { "SQL", "AUTOSTAFFSQL", "EXCLUDESTAFFSQL", "AUTOROUTE", "APPLICATION" }), Description("目前仅实现SQL,AUTOSTAFFSQL,AUTOROUTE类型的条件"), DisplayName("类型"), Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public string Type
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

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Description("AUTOSTAFFSQL的表达式应该是：select staff_id,staff_name from OG_STAFF where 条件\r\nEXCLUDESTAFFSQL的表达式与AUTOSTAFFSQL基本相同，如简单赐除下一步经办人与当前经办相同的经办人，就可以简单使用：select '{STAFF_ID}' 即可\r\nSQL与AUTOROUTE的XML表达式格式是：\r\n<conditions>\r\n    <condition>\r\n        <sql>这里是SQL语句,对于工作流转出条件来说，可以包含以下参数：{PROJECT_ID}，{PROINST_ID}，{ACTINST_ID}，分别表示业务编号，业务ID,活动ID</sql>\r\n        <type>值为0或为1：0表示执行某个SELECT查询语句，返回对应的记录集，1表示执行如UPDATE,DELETE的语句,返回所影响行数</type>\r\n        <eval>布尔表达式，可以参考：http://www.codeplex.com/ncalc , http://www.evaluant.com/web/fr/DesktopDefault.aspx</eval>\r\n        <namespace>查询对应使用的命名空间，这是在DAO.CONFIG中配置的</namespace>\r\n        <message>当条件测试通不过时，应显示的错误消息</message>\r\n    </condition>\r\n</conditions>"), DisplayName("表达式")]
        public string Xpression
        {
            get
            {
                return this.xpression;
            }
            set
            {
                this.xpression = value;
            }
        }
    }
}

