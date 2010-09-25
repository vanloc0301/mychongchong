namespace SkyMap.Net.Gui.XmlForms
{
    using System;

    public class StringWrapper
    {
        private string text;

        public override string ToString()
        {
            return this.text;
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

