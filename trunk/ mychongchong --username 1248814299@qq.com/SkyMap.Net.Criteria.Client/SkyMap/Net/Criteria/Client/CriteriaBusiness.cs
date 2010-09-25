namespace SkyMap.Net.Criteria.Client
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using System.Reflection;

    public class CriteriaBusiness
    {
        protected DataSet _dssimplecx;
        protected DataTable _dtcondition;
        protected const string _relfltcond = "filter_conditon";
        protected const string _relsfld = "search_field";
        protected const string _relsflt = "seach_filter";
        protected const string _relstbl = "search_table";
        protected const string _tbltrfilter = "TR_Filter";

        protected string _tblFilterConditionName;
        protected string _tblfiltername;
        protected string _tblperm;
        protected string _tblsearchname;
        protected string _tblsearchtable;
        protected string _tblselectname;

        public CriteriaBusiness(string tblsearch, string tblselect, string tblfilter, string tbltable, string tblFilterCondition, string tblpermission)
        {
            this._tblsearchname = tblsearch;
            this._tblselectname = tblselect;
            this._tblfiltername = tblfilter;
            this._tblsearchtable = tbltable;
            this._tblFilterConditionName = tblFilterCondition;
            this._tblperm = tblpermission;
        }

        protected void AddDataRelations(DataSet ds, DataRow drsearch, string[] TableNames)
        {
            try
            {
                LoggingService.DebugFormatted("是否是多表联查:{0}", new object[] { drsearch["ISMANY"] });
                object obj2 = drsearch["ISMANY"].ToString();
                if (!Convert.IsDBNull(obj2) && Convert.ToBoolean(obj2))
                {
                    string str5 = drsearch["id"].ToString();
                    foreach (DataRow row in drsearch.GetChildRows("search_table"))
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        string str2 = row["Relation_TABLE_NAME"].ToString();
                        if (!string.IsNullOrEmpty(str2) && (this.ExistTable(TableNames, tableName) && this.ExistTable(TableNames, str2)))
                        {
                            string str3 = row["TABLE_KEY"].ToString();
                            string str4 = row["Relation_TABLE_KEY"].ToString();
                            DataColumn childColumn = ds.Tables[tableName].Columns[str3];
                            DataColumn parentColumn = ds.Tables[str2].Columns[str4];
                            if (LoggingService.IsInfoEnabled)
                            {
                                LoggingService.InfoFormatted("Create relation:{0}-{1}", new object[] { childColumn.ColumnName, parentColumn.ColumnName });
                            }
                            if ((childColumn != null) && (parentColumn != null))
                            {
                                ds.Relations.Add("rel" + ds.Relations.Count, parentColumn, childColumn, false);
                                LoggingService.DebugFormatted("Added relation '{0}'", new object[] { "rel" + ds.Relations.Count });
                            }
                        }
                    }
                }
            }
            catch (ApplicationException exception)
            {
                throw new ApplicationException("设置数据集表单关系时出错", exception);
            }
        }

        public void AddNewCondtion(DataRow filterX)
        {
            if (filterX == null)
            {
                throw new CoreException("FilterX row cannot be null");
            }
            DataRow[] childRows = filterX.GetChildRows("filter_conditon");
            this._dtcondition.Rows.Clear();
            foreach (DataRow row in childRows)
            {
                this._dtcondition.Rows.Add(row.ItemArray);
            }
        }

        public void AddNewCondition(TrFilter trFilter)
        {
            this._dtcondition.Rows.Clear();
            foreach (TrFilterCondition trc in trFilter.TrFilterConditions)
            {
                DataRow row = this._dtcondition.NewRow();
                row["Relation"] = trc.Relation;
                row["Filter_Id"] = trc.TrFilter.Id.ToString();
                row["Condition_Name"] =trc.ConditionName;
                row["Field_ID"] = trc.FieldId;
                row["Field_Type"] = trc.FieldType;
                row["ID"] = trc.Id;
                row["TABLE_NAME"] = trc.TABLENAME;
                row["Compare_Value"] = trc.CompareValue;
                row["Operation"] = trc.Operation;
                this._dtcondition.Rows.Add(row);
            }
        }

        public void AddNewCondtion(string conditionName, string fieldId, string fieldType, string tableName)
        {
            DataRow row = this._dtcondition.NewRow();
            row["Relation"] = "且";
            if (conditionName.Length > 0)
            {
                row["Condition_Name"] = conditionName;
                row["Field_ID"] = fieldId;
                row["Field_Type"] = fieldType;
                row["ID"] = this._dtcondition.Rows.Count;
                row["TABLE_NAME"] = tableName;
            }
            this._dtcondition.Rows.Add(row);
        }

        public void CheckDtCondition()
        {
            if (this._dtcondition.Rows.Count == 0)
            {
                throw new ConditionInputException("你还没有设定任何查询条件");
            }
            string format = "";
            int num = 1;
            string[] strArray = new string[] { "Relation", "Condition_Name", "Operation", "Compare_Value" };
            foreach (DataRow row in this._dtcondition.Rows)
            {
                foreach (string str3 in strArray)
                {
                    string str = row[str3].ToString();
                    if ((str != "") && (str != null))
                    {
                        goto Label_0158;
                    }
                    string str4 = str3;
                    if (str4 != null)
                    {
                        if (!(str4 == "Relation"))
                        {
                            if (str4 == "Condition_Name")
                            {
                                goto Label_0114;
                            }
                            if (str4 == "Operation")
                            {
                                goto Label_011C;
                            }
                            if (str4 == "Compare_Value")
                            {
                                goto Label_0124;
                            }
                        }
                        else
                        {
                            format = "在第{0}行没有选择关系";
                        }
                    }
                    goto Label_012C;
                Label_0114:
                    format = "在第{0}行没有选择字段";
                    goto Label_012C;
                Label_011C:
                    format = "在第{0}行没有选择比较关系";
                    goto Label_012C;
                Label_0124:
                    format = "在第{0}行没有输入比较值";
                Label_012C:
                    if (format.Length > 0)
                    {
                        throw new ConditionInputException(string.Format(format, num));
                    }
                    num++;
                Label_0158: ;
                }
            }
        }

        public void DeleteFilterX(DataRow filterX)
        {
            string str = filterX["ID"].ToString();
            string sql = "delete from " + this._tblfiltername + " where ID='" + str + "';\ndelete from " + this._tblFilterConditionName + " where Filter_Id='" + str + "';";
            QueryHelper.ExecuteSqlScalar("SkyMap.Net.Criteria", sql);
            this.DeleteFilterXConditions(filterX);
            filterX.Delete();
        }

        public void DeleteFilterX(TrFilter filter)
        {
            IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.Criteria");
            dao.Put(filter, DAOType.DELETE);
            dao.Execute();
            dao.Close();
            foreach (TyQuery tyquery in QueryClientHelper.TyQuerys)
            {
                if (tyquery == filter.TyQuery)
                {
                    foreach (TrFilter trfilter in tyquery.TrFilters)
                    {
                        if (trfilter == filter)
                        {
                            tyquery.TrFilters.Remove(filter);
                            return;
                        }
                    }
                }
            }
        }

        private void DeleteFilterXConditions(DataRow filterX)
        {
            DataRow[] childRows = filterX.GetChildRows("filter_conditon");
            foreach (DataRow row in childRows)
            {
                row.Delete();
            }
        }

        private void DeleteFilterXConditions(TrFilter filterX)
        {
            IDA0 da = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.Criteria");
            foreach (TrFilterCondition tr in filterX.TrFilterConditions)
            {
                da.Put(tr, DAOType.DELETE);
            }
            da.Execute();
            da.Close();
            IList<TrFilterCondition> lcon = filterX.TrFilterConditions;
            lcon.Clear();

            //foreach (TyQuery tyquery in QueryClientHelper.TyQuerys)
            //{
            //    if (tyquery == filterX.TyQuery)
            //    {
            //        foreach (TrFilter trfilter in tyquery.TrFilters)
            //        {
            //            if (trfilter == filterX)
            //            {
            //                trfilter.TrFilterConditions.Clear();
            //                break;
            //            }
            //        }
            //        break;
            //    }
            //}

        }

        private bool ExistTable(string[] TableNames, string TableName)
        {
            for (int i = 0; i < TableNames.Length; i++)
            {
                if (TableName == TableNames[i])
                {
                    return true;
                }
            }
            LoggingService.WarnFormatted("查询结果中不存中表：{0}", new object[] { TableName });
            return false;
        }

        public virtual IList<string> GetAuthQuerys()
        {
            return SecurityUtil.GetCurrentPrincipalAuthResourcesByType<string>("TY_QUERY");
        }

        public DataSet GetDataSet(DataRow drsearch)
        {
            DataSet set2;
            try
            {
                List<string> list2;
                string preCondition = this.ParsePreCondition(drsearch["PreCondition"].ToString());
                string whereByConditions = this.GetWhereByConditions(preCondition);
                string fromByRelation = this.GetFromByRelation(drsearch);
                string[] sqls = this.GetSelects(drsearch, out list2).ToArray();
                string[] tables = list2.ToArray();
                for (int i = 0; i < sqls.Length; i++)
                {
                    string[] strArray3;
                    IntPtr ptr;
                    (strArray3 = sqls)[(int)(ptr = (IntPtr)i)] = strArray3[(int)ptr] + fromByRelation + whereByConditions;
                    LoggingService.Info(sqls[i]);
                }
                string dsOID = drsearch["DATASOURCE_OID"].ToString();
                string str5 = drsearch["name"].ToString();
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("查询X'{0}'使用的数据源是'{1}'", new object[] { str5, dsOID });
                }
                DataSet ds = RemotingSingletonProvider<CriteriaDAOService>.Instance.Execute(dsOID, sqls, tables);
                ds.DataSetName = drsearch["name"].ToString();
                this.AddDataRelations(ds, drsearch, tables);
                set2 = ds;
            }
            catch (ApplicationException exception)
            {
                throw exception;
            }
            return set2;
        }

        protected string GetDispName(string searchxid, string TableName, string FieldName)
        {
            string filterExpression = "id='" + searchxid + "' and Name='" + FieldName + "' and TABLE_NAME='" + TableName + "'";
            DataTable table = this._dssimplecx.Tables[this._tblselectname];
            if (table.Select(filterExpression).Length > 0)
            {
                DataRow row = table.Select(filterExpression)[0];
                if (row["Is_display"].ToString() == "是")
                {
                    return row["Display_Name"].ToString();
                }
                return "";
            }
            return "";
        }

        public DataRow[] GetFilterChildRows(DataRow drsearch)
        {
            return drsearch.GetChildRows("seach_filter");
        }

        protected string GetFromByRelation(DataRow drsearchx)
        {
            string str3;
            try
            {
                object obj2 = drsearchx["ISMANY"];
                StringBuilder builder = new StringBuilder(500);
                string str = drsearchx["TableName"].ToString();
                builder.Append(" from " + str);
                if (Convert.IsDBNull(obj2))
                {
                    return builder.ToString();
                }
                if (!Convert.ToBoolean(drsearchx["ISMANY"].ToString()))
                {
                    return builder.ToString();
                }
                string str2 = "";
                bool flag = false;
                foreach (DataRow row in drsearchx.GetChildRows("search_table"))
                {
                    if ((str2 != "") && (str2 == row["TABLE_NAME"].ToString()))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        str2 = row["TABLE_NAME"].ToString();
                    }
                    if (str2 != str)
                    {
                        if (!flag)
                        {
                            builder.Append(" inner join ");
                            builder.Append(str2);
                            builder.Append(" on ");
                        }
                        else
                        {
                            builder.Append(" and ");
                        }
                        builder.Append(str2);
                        builder.Append(".");
                        builder.Append(row["TABLE_KEY"].ToString());
                        builder.Append("=");
                        builder.Append(row["Relation_TABLE_NAME"].ToString());
                        builder.Append(".");
                        builder.Append(row["Relation_TABLE_KEY"].ToString());
                        builder.Append(Environment.NewLine);
                    }
                }
                str3 = builder.ToString();
            }
            catch (ApplicationException exception)
            {
                LoggingService.Error(exception);
                throw new ApplicationException("获取FROM子句时发生错误,请检查后配置是否正确" + exception.Message, exception);
            }
            return str3;
        }

        private string GetNewGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        protected string GetPermissionSQl(string PermissionTbl, string PermissionFld)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            if (smPrincipal == null)
            {
                return null;
            }
            SmIdentity identity = smPrincipal.Identity as SmIdentity;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("用户管理员权限为：{0}", new object[] { identity.AdminLevel });
            }
            if ((identity.AdminLevel == AdminLevelType.Admin) || (identity.AdminLevel == AdminLevelType.AdminData))
            {
                return "";
            }
            return "";
        }

        internal static string GetRealOp(string CnOp)
        {
            switch (CnOp)
            {
                case "包含":
                    return " like '%";

                case "等于":
                    return " = '";

                case "不等于":
                    return " <> '";

                case "大于":
                    return " > '";

                case "小于":
                    return " < '";

                case "大于或等于":
                    return " >= '";

                case "小于或等于":
                    return " <= '";
            }
            throw new ApplicationException("未知操作比较符:" + CnOp);
        }

        internal static string GetRealRel(string CnRel)
        {
            switch (CnRel)
            {
                case "且":
                    return " and ";

                case "或":
                    return " or ";
            }
            throw new ApplicationException("未知关系操作符");
        }

        public DataRow[] GetSelectChildRows(DataRow drsearch)
        {
            return drsearch.GetChildRows("search_field");
        }

        protected DataRow GetSelectDataRow(string id)
        {
            string filterExpression = "ID='" + id + "'";
            DataTable table = this._dssimplecx.Tables[this._tblselectname];
            return table.Select(filterExpression)[0];
        }

        protected List<string> GetSelects(DataRow drsearch, out List<string> tables)
        {
            List<string> list2;
            try
            {
                string item = string.Empty;
                List<string> list = new List<string>();
                tables = new List<string>();
                StringBuilder builder = new StringBuilder(0x800);
                foreach (DataRow row in drsearch.GetChildRows("search_field"))
                {
                    string str = row["TABLE_NAME"].ToString();
                    if ((item.Length > 0) && (str != item))
                    {
                        builder.Remove(builder.Length - 1, 1);
                        builder.Insert(0, "select distinct ");
                        list.Add(builder.ToString());
                        tables.Add(item);
                        item = str;
                        builder.Remove(0, builder.Length);
                    }
                    if (item.Length == 0)
                    {
                        item = str;
                    }
                    if (row["Is_display"].ToString() == "是")
                    {
                        builder.Append(str);
                        builder.Append(".");
                        builder.Append(row["Name"]);
                        builder.Append(" as ");
                        builder.Append(row["Display_Name"]);
                        builder.Append(",");
                    }
                }
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                    builder.Insert(0, "select ");
                    list.Add(builder.ToString());
                    tables.Add(item);
                }
                list2 = list;
            }
            catch (ApplicationException exception)
            {
                throw new ApplicationException("获取显示SELECT子句时出错", exception);
            }
            return list2;
        }

        private string GetWhereByConditions(string preCondition)
        {
            string str2;
            try
            {
                StringBuilder builder = new StringBuilder(0x800);
                bool flag = true;
                foreach (DataRow row2 in this._dtcondition.Rows)
                {
                    DataRow selectDataRow = this.GetSelectDataRow(row2["Field_ID"].ToString());
                    if (!flag)
                    {
                        builder.Append(GetRealRel(row2["Relation"].ToString()));
                    }
                    else
                    {
                        flag = false;
                    }
                    builder.Append(selectDataRow["TABLE_NAME"] + "." + selectDataRow["Name"]);
                    string realOp = GetRealOp(row2["Operation"].ToString());
                    builder.Append(realOp);
                    builder.Append(row2["Compare_Value"]);
                    if (realOp.IndexOf("%") > 0)
                    {
                        builder.Append("%");
                    }
                    builder.Append("'");
                    builder.Append(Environment.NewLine);
                }
                if (builder.Length > 0)
                {
                    builder.Insert(0, string.Format(" where ({0}) and ( ", preCondition) + Environment.NewLine);
                    builder.Append(")");
                }
                str2 = builder.ToString();
            }
            catch (ApplicationException exception)
            {
                throw new ApplicationException("构造查询WHERE语句时发生错误:" + Environment.NewLine + exception.Message, exception);
            }
            return str2;
        }

        protected void InitDataTableCondition()
        {
            if (this.DsSimplyCx.Tables.Contains(this._tblFilterConditionName))
            {
                this._dtcondition = this.DsSimplyCx.Tables[this._tblFilterConditionName].Clone();
                //this._dtcondition.Columns.Add("TABLE_NAME", typeof(string));
            }
        }

        protected void LoadDsSimpleCx()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("提示:准备获取通用查询配置定义的数据集...");
            }
            try
            {
                string str = "select * from " + this._tblsearchname;
                string str2 = "select *  from " + this._tblselectname;
                string str3 = "select * from " + this._tblsearchtable;
                str2 = str2 + string.Format(" order by {0},{1}", "TABLE_NAME", "Display_Index");
                str3 = str3 + " order by id,TABLE_ORDER asc ";
                string str4 = "select * from " + this._tblfiltername;
                string str5 = "select * from " + this._tblFilterConditionName;
                string[] sqls = new string[] { str, str2, str3, str4, str5 };
                string[] names = new string[] { this._tblsearchname, this._tblselectname, this._tblsearchtable, this._tblfiltername, this._tblFilterConditionName };
                this._dssimplecx = QueryHelper.ExecuteSqls("SkyMap.Net.GenerQuery", "ALL_TY_TREE_DATASET", sqls, names);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("提示:从数据库载入查询数据完成...准备添加表间关系...");
                }
                this._dssimplecx.Relations.Clear();
                this._dssimplecx.Relations.Add("seach_filter", this._dssimplecx.Tables[this._tblsearchname].Columns["id"], this._dssimplecx.Tables[this._tblfiltername].Columns["Searchx_id"], false);
                this._dssimplecx.Relations.Add("search_field", this._dssimplecx.Tables[this._tblsearchname].Columns["id"], this._dssimplecx.Tables[this._tblselectname].Columns["Searchx_id"], false);
                this._dssimplecx.Relations.Add("search_table", this._dssimplecx.Tables[this._tblsearchname].Columns["id"], this._dssimplecx.Tables[this._tblsearchtable].Columns["Searchx_id"], false);
                this._dssimplecx.Relations.Add("filter_conditon", this._dssimplecx.Tables[this._tblfiltername].Columns["ID"], this._dssimplecx.Tables[this._tblFilterConditionName].Columns["Filter_Id"], false);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("通用查询数据集获取成功...");
                }
            }
            catch (ApplicationException exception)
            {
                throw new ApplicationException("载入简单查询数据集时发生错误：" + Environment.NewLine + exception.Message, exception);
            }
        }

        internal string ParsePreCondition(string preConditon)
        {
            if (StringHelper.IsNull(preConditon))
            {
                return "1=1";
            }
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            SmIdentity identity = smPrincipal.Identity as SmIdentity;
            return preConditon.Replace("{SYS:STAFFID}", identity.UserId).Replace("{SYS:STAFFNAME}", identity.UserName).Replace("{SYS:DEPTID}", smPrincipal.DeptIds[0]).Replace("{SYS:DEPTNAME}", smPrincipal.DeptNames[0]).Replace("{SYS:NOW}", DateTimeHelper.GetNow().ToLongDateString());
        }

        public void SaveFilterX(DataRow searchX, ref DataRow filterX, string filterName)
        {
            if (searchX == null)
            {
                throw new CoreException("searchX cannot be null");
            }
            if (StringHelper.IsNull(filterName))
            {
                throw new CoreException("Filter name cannot be null");
            }
            if (this._dtcondition.Rows.Count == 0)
            {
                throw new CoreException("Condition row count cannot be 0");
            }
            StringBuilder builder = new StringBuilder(0x3e8);
            string newGuid = string.Empty;
            if (filterX == null)
            {
                newGuid = this.GetNewGuid();
                string str2 = searchX["id"].ToString();
                builder.Append("insert into " + this._tblfiltername + "(ID,NAME,Searchx_id) values('" + newGuid + "','" + filterName + "','" + str2 + "');\n");
                filterX = this._dssimplecx.Tables[this._tblfiltername].NewRow();
                filterX["ID"] = newGuid;
                filterX["NAME"] = filterName;
                filterX["Searchx_id"] = str2;
                this._dssimplecx.Tables[this._tblfiltername].Rows.Add(filterX);
            }
            else
            {
                if (filterName != filterX["NAME"].ToString())
                {
                    newGuid = filterX["ID"].ToString();
                    builder.Append("update " + this._tblfiltername + " set NAME='" + filterName + "' where ID='" + newGuid + "';\n");
                    builder.Append("delete from ").Append(this._tblFilterConditionName).Append(" where ").Append("Filter_Id").Append("='").Append(newGuid).Append("';\n");
                    filterX["NAME"] = filterName;
                }
                this.DeleteFilterXConditions(filterX);
            }
            string str3 = "insert into " + this._tblFilterConditionName + "(";
            int count = this._dtcondition.Columns.Count;
            int num2 = 0;
            foreach (DataColumn column in this._dtcondition.Columns)
            {
                str3 = str3 + column.ColumnName;
                num2++;
                if (num2 < count)
                {
                    str3 = str3 + ",";
                }
            }
            str3 = str3 + ") values (";
            int num3 = 0;
            foreach (DataRow row2 in this._dtcondition.Rows)
            {
                builder.Append(str3);
                num2 = 0;
                DataRow row = this._dssimplecx.Tables[this._tblFilterConditionName].NewRow();
                foreach (DataColumn column in this._dtcondition.Columns)
                {
                    num2++;
                    string columnName = column.ColumnName;
                    if (columnName == null)
                    {
                        goto Label_0415;
                    }
                    if (!(columnName == "ID"))
                    {
                        if (columnName == "Filter_Id")
                        {
                            goto Label_03E9;
                        }
                        goto Label_0415;
                    }
                    builder.Append("'").Append(newGuid).Append(num3).Append("'");
                    row["ID"] = newGuid + num3.ToString();
                    goto Label_0462;
                Label_03E9:
                    builder.Append("'").Append(newGuid).Append("'");
                    row["Filter_Id"] = newGuid;
                    goto Label_0462;
                Label_0415:
                    builder.Append("'").Append(row2[column.ColumnName].ToString()).Append("'");
                    row[column.ColumnName] = row2[column.ColumnName];
                Label_0462:
                    if (num2 < count)
                    {
                        builder.Append(",");
                    }
                    if (num2 == count)
                    {
                        builder.Append(");\n");
                    }
                }
                num3++;
                this._dssimplecx.Tables[this._tblFilterConditionName].Rows.Add(row);
            }
            if (builder.Length > 0)
            {
                QueryHelper.ExecuteSqlScalar("SkyMap.Net.Criteria", builder.ToString());
            }
        }

        public void SaveFilterX(TyQuery searchX, ref TrFilter filterX, string filterName)
        {
            int num = 0;

            if (searchX == null)
            {
                throw new CoreException("TyQuery cannot be null");
            }
            if (StringHelper.IsNull(filterName))
            {
                throw new CoreException("Filter name cannot be null");
            }
            if (this._dtcondition.Rows.Count == 0)
            {
                throw new CoreException("Condition row count cannot be 0");
            }
            StringBuilder builder = new StringBuilder(0x3e8);
            string newGuid = string.Empty;
            if (filterX == null)
            {
                newGuid = this.GetNewGuid();
                string str2 = searchX.Id.ToString();
                filterX = new TrFilter();
                filterX.Id = newGuid;
                filterX.Name = filterName;
                filterX.TyQuery = searchX;
                searchX.TrFilters.Add(filterX);
                IDA0 da = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.Criteria");
                da.Put(filterX, DAOType.SAVE);
                da.Execute();
                da.Close();
                //foreach (TyQuery tyquery in QueryClientHelper.TyQuerys)
                //{
                //    if (tyquery == filterX.TyQuery)
                //    {
                //        tyquery.TrFilters.Add(filterX);
                //        break;
                //    }
                //}
            }
            else
            {
                if (filterName != filterX.Name.ToString())
                {
                    newGuid = filterX.Id.ToString();
                    filterX.Name = filterName;
                    IDA0 da = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.Criteria");
                    da.Put(filterX, DAOType.UPDATE);
                    da.Execute();
                    da.Close();
                    foreach (TyQuery tyquery in QueryClientHelper.TyQuerys)
                    {
                        if (tyquery == filterX.TyQuery)
                        {
                            foreach (TrFilter trfilter in tyquery.TrFilters)
                            {
                                if (trfilter.Id == filterX.Id)
                                {
                                    trfilter.Name = filterX.Name;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                this.DeleteFilterXConditions(filterX);
            }
            
            foreach (DataRow row in this._dtcondition.Rows)
            {
                TrFilterCondition trc = new TrFilterCondition();
                trc.TrFilter = filterX;
                trc.Relation = row["Relation"].ToString();
                trc.ConditionName = row["Condition_Name"].ToString();
                trc.FieldId = row["Field_ID"].ToString();
                trc.FieldType = row["Field_Type"].ToString();
                trc.Id = filterX.Id + num.ToString();
                trc.TABLENAME = row["TABLE_NAME"].ToString();
                trc.CompareValue = row["Compare_Value"].ToString();
                trc.Operation = row["Operation"].ToString();   
                filterX.TrFilterConditions.Add(trc);
                num++;
            }
         
            if (this._dtcondition.Rows.Count > 0)
            {
                IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.Criteria");
                foreach (TrFilterCondition tr in filterX.TrFilterConditions)
                {
                    dao.Put(tr, DAOType.SAVE);
                }
                dao.Execute();
                dao.Close();

                //foreach (TyQuery tyquery in QueryClientHelper.TyQuerys)
                //{
                //    if (tyquery == filterX.TyQuery)
                //    {
                //        foreach (TrFilter trfilter in tyquery.TrFilters)
                //        {
                //            if (trfilter == filterX)
                //            {
                //                foreach (TrFilterCondition tmp in filterX.TrFilterConditions)
                //                {
                //                    trfilter.TrFilterConditions.Add((tmp as DomainObject).Clone<TrFilterCondition>());                                 
                //                }
                //                break;
                //            }
                //        }
                //        break;
                //    }
                //}
               // QueryHelper.ExecuteSqlScalar("SkyMap.Net.Criteria", builder.ToString());
            }
        }

        public DataSet DsSimplyCx
        {
            get
            {
                if (this._dssimplecx == null)
                {
                    this.LoadDsSimpleCx();
                }
                return this._dssimplecx;
            }
        }

        public DataTable DtCondition
        {
            get
            {
                if (this._dtcondition == null)
                {
                    this.InitDataTableCondition();
                }
                return this._dtcondition;
            }
        }

        public DataTable DtSearchX
        {
            get
            {
                return this.DsSimplyCx.Tables[this._tblsearchname];
            }
        }

        public virtual bool IsCanAccessAll
        {
            get
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                return ((smIdentity.AdminLevel == AdminLevelType.Admin) || (smIdentity.AdminLevel == AdminLevelType.AdminData));
            }
        }
    }
}

