namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using System;

    public class ExtQueryEditCommand : QueryEditCommand, ITextEditCommand, ICommand
    {
        private string Field
        {
            get
            {
                if (base.Codon.Properties.Contains("field"))
                {
                    return base.Codon.Properties["field"];
                }
                return string.Empty;
            }
        }

        public override string Key
        {
            get
            {
                return this.Field;
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

