namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using System;

    public class QueryEditProjectIdCommand : QueryEditCommand, ITextEditCommand, ICommand
    {
        public override string Key
        {
            get
            {
                return "p.PROJECT_ID";
            }
        }

        public override string Operator
        {
            get
            {
                return " like ";
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
                (value as ToolBarTextBox).Control.Width = 200;
            }
        }

        public override string Value
        {
            get
            {
                string str = (this.Owner as ToolBarTextBox).Text.Replace("'", "''");
                if ((str != null) && (str.Length > 0))
                {
                    return ("'%" + str + "%'");
                }
                return "'%'";
            }
        }
    }
}

