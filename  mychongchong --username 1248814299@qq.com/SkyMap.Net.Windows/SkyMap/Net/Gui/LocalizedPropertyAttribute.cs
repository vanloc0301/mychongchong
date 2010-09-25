namespace SkyMap.Net.Gui
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public sealed class LocalizedPropertyAttribute : Attribute
    {
        private string category = string.Empty;
        private string description = string.Empty;
        private string name = string.Empty;

        public LocalizedPropertyAttribute(string name)
        {
            this.name = name;
        }

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}

