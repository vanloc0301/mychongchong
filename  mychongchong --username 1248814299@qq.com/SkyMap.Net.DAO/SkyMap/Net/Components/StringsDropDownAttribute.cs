namespace SkyMap.Net.Components
{
    using System;
    using System.Collections;

    [AttributeUsage(AttributeTargets.Property)]
    public class StringsDropDownAttribute : DropDownAttribute
    {
        public string[] dataSource;

        public override IEnumerable DataSource
        {
            get
            {
                return this.dataSource;
            }
        }
    }
}

