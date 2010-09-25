namespace SkyMap.Net.Workflow.Client.Config
{
    using System;
    using System.Collections.Generic;

    public class CBoxConfig
    {
        private string _class;
        private IList<CColConfig> _colList = new List<CColConfig>();
        private string _dAONameSpace;
        private string _idField;
        private string _name;
        private string _openViewCommand;
        private string _queryCountName;
        private string _queryName;
        private IList<CMenuItemConfig> menus = new List<CMenuItemConfig>();
        private string[] queryParameters;
        private string toolbarPath;

        public void AddCol(CColConfig col)
        {
            this._colList.Add(col);
        }

        public void AddMenuItem(CMenuItemConfig menuItem)
        {
            this.menus.Add(menuItem);
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

        public IList<CColConfig> ColList
        {
            get
            {
                return this._colList;
            }
        }

        public string DAONameSpace
        {
            get
            {
                return this._dAONameSpace;
            }
            set
            {
                this._dAONameSpace = value;
            }
        }

        public string IdField
        {
            get
            {
                return this._idField;
            }
            set
            {
                this._idField = value;
            }
        }

        public IList<CMenuItemConfig> Menus
        {
            get
            {
                return this.menus;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string OpenViewCommand
        {
            get
            {
                return this._openViewCommand;
            }
            set
            {
                this._openViewCommand = value;
            }
        }

        public string QueryCountName
        {
            get
            {
                return this._queryCountName;
            }
            set
            {
                this._queryCountName = value;
            }
        }

        public string QueryName
        {
            get
            {
                return this._queryName;
            }
            set
            {
                this._queryName = value;
            }
        }

        public string[] QueryParameters
        {
            get
            {
                return this.queryParameters;
            }
            set
            {
                this.queryParameters = value;
            }
        }

        public string ToolbarPath
        {
            get
            {
                return this.toolbarPath;
            }
            set
            {
                this.toolbarPath = value;
            }
        }
    }
}

