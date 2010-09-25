namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Serializable]
    public class DataControl : DomainObject, ISaveAs
    {
        private DAODataColumn mapColumn;
        private DAODataTable mapTable;
        private DAODataForm owner;
        private string type;
        private string valueDataSource;
        private string valueList;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        public DAODataColumn MapColumn
        {
            get
            {
                return this.mapColumn;
            }
            set
            {
                this.mapColumn = value;
            }
        }

        public DAODataTable MapTable
        {
            get
            {
                return this.mapTable;
            }
            set
            {
                this.mapTable = value;
            }
        }

        public DAODataForm Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        public string[] ValueCollection
        {
            get
            {
                string[] strArray = null;
                int num;
                if (StringHelper.IsNull(this.valueList))
                {
                    return strArray;
                }
                if (this.valueList.StartsWith("DICT:"))
                {
                    IList<DataWord> list = DataWordService.Instance.FindDataWords(this.valueList.Substring(5));
                    strArray = new string[list.Count];
                    for (num = 0; num < list.Count; num++)
                    {
                        strArray[num] = list[num].Name;
                    }
                    return strArray;
                }
                if (this.valueList.StartsWith("SQL:"))
                {
                    DataTable table = QueryHelper.ExecuteSql("Default", string.Empty, this.valueList.Substring(4));
                    strArray = new string[table.Rows.Count];
                    for (num = 0; num < table.Rows.Count; num++)
                    {
                        strArray[num] = table.Rows[num][0].ToString();
                    }
                    return strArray;
                }
                return StringHelper.Split(this.valueList);
            }
        }

        public string ValueDataSource
        {
            get
            {
                return this.valueDataSource;
            }
            set
            {
                this.valueDataSource = value;
            }
        }

        public string ValueList
        {
            get
            {
                return this.valueList;
            }
            set
            {
                this.valueList = value;
            }
        }
    }
}

