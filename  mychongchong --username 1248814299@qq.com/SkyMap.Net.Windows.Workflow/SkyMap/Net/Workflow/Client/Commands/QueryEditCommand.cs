namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public abstract class QueryEditCommand : AbstractBoxCommand
    {
        private bool isEnabled = false;

        protected QueryEditCommand()
        {
        }

        public virtual void AddToQueryParameters(Dictionary<string, string> queryParameters)
        {
            if (this.IsEnabled)
            {
                string str = string.Empty;
                if (this.Value.Length > 0)
                {
                    str = this.Key + this.Operator + this.Value;
                }
                queryParameters.Add(this.Key, str);
            }
        }

        public override void Run()
        {
        }

        public override bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
            }
        }

        public abstract string Key { get; }

        public virtual string Operator
        {
            get
            {
                return "=";
            }
        }

        public virtual string Value
        {
            get
            {
                string text = (this.Owner as ToolStripItem).Text;
                if (text.Length > 0)
                {
                    return ("'" + text + "'");
                }
                return string.Empty;
            }
        }
    }
}

