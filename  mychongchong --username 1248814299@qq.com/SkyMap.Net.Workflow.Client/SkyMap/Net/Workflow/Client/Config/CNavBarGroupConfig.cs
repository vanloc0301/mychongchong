namespace SkyMap.Net.Workflow.Client.Config
{
    using System;
    using System.Collections.Generic;

    public class CNavBarGroupConfig
    {
        private string _backgroundImage;
        private string _caption;
        private string _image;
        private IList<CNavBarItemConfig> _items = new List<CNavBarItemConfig>();
        private string styleBackgroundName;

        public string BackgroundImage
        {
            get
            {
                return this._backgroundImage;
            }
            set
            {
                this._backgroundImage = value;
            }
        }

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

        public IList<CNavBarItemConfig> Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        public string StyleBackgroundName
        {
            get
            {
                return this.StyleBackgroundName;
            }
            set
            {
                this.styleBackgroundName = value;
            }
        }
    }
}

