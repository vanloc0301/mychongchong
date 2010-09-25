namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using SkyMap.Net.Tools.Workflow;

    public class AddConditionCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is ObjectNode)
            {
                ObjectNode owner = (ObjectNode) this.Owner;
                SkyMap.Net.Workflow.XPDL.Condition condition = new SkyMap.Net.Workflow.XPDL.Condition();
                condition.Id = StringHelper.GetNewGuid();
                condition.Name = "新条件";
                condition.Type = "SQL";
                condition.Xpression = "<conditions>\r\n    <condition>\r\n        <sql>这里是SQL语句</sql>\r\n        <type>值为0或为1：0表示执行某个SELECT查询语句，返回对应的记录集，1表示执行如UPDATE,DELETE的语句,返回所影响行数</type>\r\n        <eval>布尔表达式，可以参考：http://www.codeplex.com/ncalc , http://www.evaluant.com/web/fr/DesktopDefault.aspx</eval>\r\n        <namespace>查询对应使用的命名空间，这是在DAO.CONFIG中配置的</namespace>\r\n        <message>这里输入条件不成立时显示的消息</message>\r\n    </condition>\r\n</conditions>";
                condition.Save();
                owner.AddSingleNode<SkyMap.Net.Workflow.XPDL.Condition, ConditionNode>(condition).ActivateItem();
            }
        }
    }
}

