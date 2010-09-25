namespace SkyMap.Net.Workflow.Client.Config
{
    using System;

    public class CColConfig
    {
        private string _caption;
        private string _fieldName;
        private int _visibleIndex;
        private int _width = -1;
        private string formatString;
        private string formatType;

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

        public string FieldName
        {
            get
            {
                return this._fieldName;
            }
            set
            {
                this._fieldName = value;
            }
        }

        public string FormatString
        {
            get
            {
                return this.formatString;
            }
            set
            {
                this.formatString = value;
            }
        }

        public string FormatType
        {
            get
            {
                return this.formatType;
            }
            set
            {
                this.formatType = value;
            }
        }

        public int VisibleIndex
        {
            get
            {
                return this._visibleIndex;
            }
            set
            {
                this._visibleIndex = value;
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }
    }
}

