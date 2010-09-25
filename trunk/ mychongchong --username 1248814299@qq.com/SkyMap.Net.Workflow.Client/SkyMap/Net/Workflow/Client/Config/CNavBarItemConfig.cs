namespace SkyMap.Net.Workflow.Client.Config
{
    using System;

    public class CNavBarItemConfig
    {
        private string _caption;
        private string _class;
        private string _image;
        private string _link;
        public bool IfAuth;
        public bool IsDefault = false;

        public string Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }

        public string Class
        {
            get
            {
                return this._class;
            }
            set
            {
                this._class = value;
            }
        }

        public string Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
            }
        }

        public string Link
        {
            get
            {
                return this._link;
            }
            set
            {
                this._link = value;
            }
        }
    }
}

