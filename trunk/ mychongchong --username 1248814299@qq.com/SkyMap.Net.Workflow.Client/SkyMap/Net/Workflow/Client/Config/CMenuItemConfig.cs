namespace SkyMap.Net.Workflow.Client.Config
{
    using System;

    public class CMenuItemConfig
    {
        private string access;
        private bool enableOnSelect = false;
        private string invokeName;
        private string text;

        public string Access
        {
            get
            {
                return this.access;
            }
            set
            {
                this.access = value;
            }
        }

        public bool EnableOnSelect
        {
            get
            {
                return this.enableOnSelect;
            }
            set
            {
                this.enableOnSelect = value;
            }
        }

        public string InvokeName
        {
            get
            {
                return this.invokeName;
            }
            set
            {
                this.invokeName = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

