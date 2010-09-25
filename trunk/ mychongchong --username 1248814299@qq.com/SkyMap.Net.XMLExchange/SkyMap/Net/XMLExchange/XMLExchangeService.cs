namespace SkyMap.Net.XMLExchange
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.XMLExchange.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;

    public static class XMLExchangeService
    {
        private static DataSet DB2XML(string dsName, string where, DTProject dtProject)
        {
            DataSet set = new DataSet(string.IsNullOrEmpty(dsName) ? (string.IsNullOrEmpty(dtProject.MapXMLElementName) ? dtProject.Name : dtProject.MapXMLElementName) : dsName);
            StringBuilder builder = new StringBuilder();
            foreach (DTDatabase database in dtProject.DTDatabases)
            {
                if (database.IsActive && database.IfExport)
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.InfoFormatted("开始导出'{0}'", new object[] { database.Name });
                    }
                    DbConnection connection = DbProviderFactories.GetFactory(database.DSType).CreateConnection();
                    connection.ConnectionString = database.DecryptConnectionString;
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("数据库连接字符串是:{0}", new object[] { connection.ConnectionString });
                    }
                    connection.Open();
                    try
                    {
                        foreach (DTTable table in database.DTTables)
                        {
                            if ((!table.IsActive || !table.IfExport) || ((dsName != null) && (table.DataSetName != dsName)))
                            {
                                continue;
                            }
                            builder.Append("select ");
                            foreach (DTColumn column in table.DTColumns)
                            {
                                if (column.IsActive && column.IfExport)
                                {
                                    builder.Append(column.Name);
                                    if (!string.IsNullOrEmpty(column.MapXMLElementName))
                                    {
                                        builder.Append(" as ").Append(column.MapXMLElementName.Replace("{0}:", string.Empty));
                                    }
                                    builder.Append(",");
                                }
                            }
                            if (builder.ToString().EndsWith(","))
                            {
                                builder.Remove(builder.Length - 1, 1);
                            }
                            else
                            {
                                builder.Append("*");
                            }
                            builder.Append(" from ").Append(table.Name);
                            if (!string.IsNullOrEmpty(where))
                            {
                                builder.Append(" where ").Append(where);
                            }
                            DataTable table2 = new DataTable(string.IsNullOrEmpty(table.MapXMLElementName) ? table.Name : table.MapXMLElementName.Replace("{0}:", string.Empty));
                            DbCommand command = connection.CreateCommand();
                            command.CommandText = builder.ToString();
                            builder.Remove(0, builder.Length);
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("将执行SQL：{0}", new object[] { command.CommandText });
                            }
                            IDataReader reader = command.ExecuteReader();
                            try
                            {
                                table2.Load(reader);
                            }
                            finally
                            {
                                reader.Close();
                            }
                            set.Tables.Add(table2);
                        }
                        continue;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("导出完成.");
            }
            return set;
        }

        public static bool DB2XML(string property, string value, string xmlFile)
        {
            DTProject dtProject = GetDtProject(property, value);
            if (dtProject != null)
            {
                DataSet set = DB2XML(null, null, dtProject);
                if (set != null)
                {
                    set.WriteXml(xmlFile);
                    return true;
                }
            }
            return false;
        }

        public static DataSet DB2XML(string dsName, string property, string value, string where, ref string dsMapXMLElementName)
        {
            DTProject dtProject = GetDtProject(property, value);
            if (dtProject == null)
            {
                return null;
            }
            dsMapXMLElementName = dtProject.MapXMLElementName;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("要导出的数据集是:'{0}'", new object[] { dsName });
            }
            return DB2XML(dsName, where, dtProject);
        }

        public static DTProject GetDtProject(string property, string value)
        {
            IList<DTProject> list = null;
            string key = "ALL_ACTIVE_DT_PROJECT";
            if (!DAOCacheService.Contains(key))
            {
                list = QueryHelper.List<DTProject>(string.Empty, new string[] { "IsActive" }, new string[] { "1" });
                DAOCacheService.Put(key, list);
            }
            else
            {
                list = (IList<DTProject>) DAOCacheService.Get(key);
            }
            foreach (DTProject project in list)
            {
                string str2 = property;
                if (str2 == null)
                {
                    continue;
                }
                if (!(str2 == "ProdefID"))
                {
                    if (str2 == "MapXMLElementName")
                    {
                        goto Label_009E;
                    }
                    continue;
                }
                if (!(value == project.ProdefID))
                {
                    continue;
                }
                return project;
            Label_009E:
                if (value == project.MapXMLElementName)
                {
                    return project;
                }
            }
            LoggingService.WarnFormatted("没有找到属性为'{0}'值为‘{1}’的交换配置信息!", new object[] { property, value });
            return null;
        }

        public static string ObtainXMLTempletName(string property, string value)
        {
            DTProject dtProject = GetDtProject(property, value);
            if (dtProject != null)
            {
                return dtProject.Templet;
            }
            return string.Empty;
        }

        private static void SetColumnValue(XPathDocument[] documents, string namespacePrefix, string namespaceUri, XPathNavigator node, DataRow row, DTColumn dtcolumn)
        {
            XPathNavigator navigator;
            object obj2;
            Type type;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("开始设定'{0}<{1}>'的值:", new object[] { dtcolumn.Name, dtcolumn.Description });
            }
            if (!string.IsNullOrEmpty(namespacePrefix) && !string.IsNullOrEmpty(namespaceUri))
            {
                dtcolumn.NamespacePrefix = string.Format(dtcolumn.NamespacePrefix, namespacePrefix);
                dtcolumn.NamespaceUri = string.Format(dtcolumn.NamespaceUri, namespaceUri);
                dtcolumn.MapXMLElementName = string.Format(dtcolumn.MapXMLElementName, namespacePrefix);
            }
            XPathExpression expression = node.Compile(dtcolumn.MapXMLElementName);
            XmlNamespaceManager nsManager = null;
            if (!string.IsNullOrEmpty(dtcolumn.NamespacePrefix))
            {
                nsManager = new XmlNamespaceManager(node.NameTable);
                nsManager.AddNamespace(dtcolumn.NamespacePrefix, dtcolumn.NamespaceUri);
                expression.SetContext(nsManager);
            }
            string str = null;
            switch (expression.ReturnType)
            {
                case XPathResultType.Number:
                case XPathResultType.String:
                case XPathResultType.Boolean:
                    goto Label_02B7;

                case XPathResultType.NodeSet:
                    navigator = null;
                    try
                    {
                        if (nsManager != null)
                        {
                            navigator = node.SelectSingleNode(expression.Expression, nsManager);
                        }
                        else
                        {
                            navigator = node.SelectSingleNode(expression);
                        }
                    }
                    catch (XPathException)
                    {
                    }
                    if (navigator != null)
                    {
                        goto Label_028F;
                    }
                    if (dtcolumn.MapXMLElementName.StartsWith("/"))
                    {
                        LoggingService.WarnFormatted("找不到节点：{0},将遍历整个树查找", new object[] { expression.Expression });
                        foreach (XPathDocument document in documents)
                        {
                            if (document != null)
                            {
                                XPathNavigator navigator2 = document.CreateNavigator();
                                XPathNavigator navigator3 = null;
                                if (!string.IsNullOrEmpty(namespacePrefix) && !string.IsNullOrEmpty(namespaceUri))
                                {
                                    dtcolumn.NamespacePrefix = string.Format(dtcolumn.NamespacePrefix, namespacePrefix);
                                    dtcolumn.NamespaceUri = string.Format(dtcolumn.NamespaceUri, namespaceUri);
                                    dtcolumn.MapXMLElementName = string.Format(dtcolumn.MapXMLElementName, namespacePrefix);
                                }
                                if (!string.IsNullOrEmpty(dtcolumn.NamespacePrefix))
                                {
                                    XmlNamespaceManager resolver = new XmlNamespaceManager(navigator2.NameTable);
                                    resolver.AddNamespace(dtcolumn.NamespacePrefix, dtcolumn.NamespaceUri);
                                    navigator3 = navigator2.SelectSingleNode(dtcolumn.MapXMLElementName, resolver);
                                }
                                if (navigator3 != null)
                                {
                                    str = navigator3.Value;
                                    LoggingService.DebugFormatted("{0}:{1}:{2}", new object[] { navigator3.LocalName, navigator3.InnerXml, navigator3.Value });
                                    break;
                                }
                                LoggingService.WarnFormatted("仍然找不到节点：{0}!", new object[] { dtcolumn.MapXMLElementName });
                                return;
                            }
                        }
                        goto Label_028F;
                    }
                    LoggingService.WarnFormatted("找不到节点：{0}!", new object[] { dtcolumn.MapXMLElementName });
                    return;

                default:
                    goto Label_02E7;
            }
            return;
        Label_028F:
            try
            {
                if (navigator != null)
                {
                    str = navigator.Value;
                }
                goto Label_0306;
            }
            catch (Exception exception)
            {
                LoggingService.Error(string.Format("不能获取任何指定XPATH表达式｛{０}｝的节点", dtcolumn.MapXMLElementName), exception);
                return;
            }
        Label_02B7:
            obj2 = node.Evaluate(expression);
            try
            {
                str = Convert.ToString(obj2);
                goto Label_0306;
            }
            catch (FormatException)
            {
                LoggingService.WarnFormatted("获取的XPATH查询值:'{0}'不能转换为字符串!", new object[] { obj2 });
                goto Label_0306;
            }
        Label_02E7:;
            LoggingService.WarnFormatted("XPATH表达式｛{0}｝的解析有误，或其返回结果尚不被支持", new object[] { dtcolumn.MapXMLElementName });
        Label_0306:
            type = row.Table.Columns[dtcolumn.Name].DataType;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将设定'{0}<{1}>（{3}）（{4}）'的值为'{2}'", new object[] { dtcolumn.Name, dtcolumn.Description, str, dtcolumn.Type, type.FullName });
            }
            if (type == typeof(string))
            {
                try
                {
                    row[dtcolumn.Name] = str;
                    return;
                }
                catch (FormatException)
                {
                    LoggingService.WarnFormatted("设定'{0}<1>'的值为'{2}'时出错", new object[] { dtcolumn.Name, dtcolumn.Description, str });
                    return;
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                if (type.Equals(typeof(DateTime)))
                {
                    str = str.Replace('一', '1').Replace('二', '2').Replace('三', '3').Replace('四', '4').Replace('五', '5').Replace('六', '6').Replace('七', '7').Replace('八', '8').Replace('九', '9');
                }
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.IsValid(str))
                {
                    try
                    {
                        row[dtcolumn.Name] = converter.ConvertFrom(str);
                        return;
                    }
                    catch (FormatException)
                    {
                        LoggingService.WarnFormatted("设定'{0}<1>'的值为'{2}'时出错", new object[] { dtcolumn.Name, dtcolumn.Description, str });
                        return;
                    }
                }
                try
                {
                    row[dtcolumn.Name] = str;
                }
                catch (Exception exception2)
                {
                    LoggingService.ErrorFormatted("数据交换发生数据转换错误：表{{0}};列{{1}};值{{2}}\r\n{3}", new object[] { dtcolumn.DTTable.Name, dtcolumn.Name, str, exception2 });
                }
            }
        }

        public static bool XML2DB(string property, string value, string xmlFile)
        {
            DTProject dtProject = GetDtProject(property, value);
            if (dtProject != null)
            {
                if (System.IO.File.Exists(xmlFile))
                {
                    XPathDocument genericXMLDate = new XPathDocument(xmlFile);
                    XML2DB(dtProject, null, null, genericXMLDate, null, null, null, null, null, null);
                    return true;
                }
                LoggingService.ErrorFormatted("要交换的文件‘{0}’不存在！", new object[] { xmlFile });
                return false;
            }
            LoggingService.WarnFormatted("没有找到属性为'{0}',值为‘{1}’的交换配置信息!", new object[] { property, value });
            return false;
        }

        public static void XML2DB(DTProject dtProject, string namespacePrefix, string namespaceUri, XPathDocument genericXMLDate, XPathDocument formData, XPathDocument resultData, string prodef_id, string project_id, DateTime? applyingDate, DateTime? finishedDate)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("准备处理交换文件");
            }
            List<DbTransaction> list = new List<DbTransaction>(dtProject.DTDatabases.Count);
            List<DbConnection> list2 = new List<DbConnection>(dtProject.DTDatabases.Count);
            try
            {
                foreach (DTDatabase database in dtProject.DTDatabases)
                {
                    if (database.IsActive && database.IfImport)
                    {
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.InfoFormatted("开始交换到'{0}'", new object[] { database.Name });
                        }
                        DbProviderFactory dataFactory = DbProviderFactories.GetFactory(database.DSType);
                        DbConnection item = dataFactory.CreateConnection();
                        item.ConnectionString = database.DecryptConnectionString;
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("数据库连接字符串是:{0}", new object[] { item.ConnectionString });
                        }
                        item.Open();
                        DbTransaction transaction = item.BeginTransaction();
                        list.Add(transaction);
                        list2.Add(item);
                        XML2DB(dataFactory, item, transaction, database, namespacePrefix, namespaceUri, genericXMLDate, formData, resultData, prodef_id, project_id, applyingDate, finishedDate);
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.Info("交换完成.");
                        }
                    }
                }
                foreach (DbTransaction transaction2 in list)
                {
                    transaction2.Commit();
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("提交数据库事务");
                    }
                }
            }
            catch (Exception exception)
            {
                foreach (DbTransaction transaction3 in list)
                {
                    transaction3.Rollback();
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("遇到了错误,事务将回滚!");
                    }
                }
                LoggingService.Error(exception);
                throw exception;
            }
            finally
            {
                foreach (DbConnection connection2 in list2)
                {
                    connection2.Close();
                }
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("处理完成!");
            }
        }

        private static void XML2DB(DbProviderFactory dataFactory, DbConnection dbcn, DbTransaction tran, DTDatabase dtDatabase, string namespacePrefix, string namespaceUri, XPathDocument genericXMLDate, XPathDocument formData, XPathDocument resultData, string prodef_id, string project_id, DateTime? applyingDate, DateTime? finishedDate)
        {
            if (!string.IsNullOrEmpty(dtDatabase.BeforeProcedure))
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("执行交换前存储过程：{0}", new object[] { dtDatabase.BeforeProcedure });
                }
                DbCommand command = dbcn.CreateCommand();
                command.CommandText = dtDatabase.BeforeProcedure;
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = tran;
                command.ExecuteNonQuery();
            }
            XPathDocument[] documents = new XPathDocument[] { formData, resultData, genericXMLDate };
            foreach (DTTable table in dtDatabase.DTTables)
            {
                XPathDocument document;
                if (!table.IsActive || !table.IfImport)
                {
                    continue;
                }
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("准备交换数据至表：'{0}<{1}>'", new object[] { table.Name, table.Description });
                }
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("基础数据来自：{0}", new object[] { table.SourceType });
                }
                switch (table.SourceType)
                {
                    case SourceType.FormData:
                        document = formData;
                        break;

                    case SourceType.ResultData:
                        document = resultData;
                        break;

                    case SourceType.GenericXMLData:
                        document = genericXMLDate;
                        break;

                    default:
                        throw new NotSupportedException(string.Format("尚不支持的类型:{0}！", table.SourceType));
                }
                if (document != null)
                {
                    DbDataAdapter adapter = dataFactory.CreateDataAdapter();
                    DbCommand command2 = dbcn.CreateCommand();
                    command2.Transaction = tran;
                    command2.CommandText = string.Format("select * from {0} where 1<>1", string.IsNullOrEmpty(table.ImportToTable) ? table.Name : table.ImportToTable);
                    adapter.SelectCommand = command2;
                    DbCommandBuilder builder = dataFactory.CreateCommandBuilder();
                    builder.DataAdapter = adapter;
                    adapter.InsertCommand = builder.GetInsertCommand();
                    adapter.InsertCommand.Transaction = tran;
                    XPathNavigator navigator = document.CreateNavigator();
                    XPathNodeIterator iterator = null;
                    if (!string.IsNullOrEmpty(namespacePrefix) && !string.IsNullOrEmpty(namespaceUri))
                    {
                        table.NamespacePrefix = string.Format(table.NamespacePrefix, namespacePrefix);
                        table.NamespaceUri = string.Format(table.NamespaceUri, namespaceUri);
                        table.MapXMLElementName = string.Format(table.MapXMLElementName, namespacePrefix);
                    }
                    if (!string.IsNullOrEmpty(table.NamespacePrefix))
                    {
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("搜索:'{0}',前缀:'{1}',命名空间:'{2}'", new object[] { table.MapXMLElementName, table.NamespacePrefix, table.NamespaceUri });
                        }
                        XmlNamespaceManager resolver = new XmlNamespaceManager(navigator.NameTable);
                        resolver.AddNamespace(table.NamespacePrefix, table.NamespaceUri);
                        iterator = navigator.Select(table.MapXMLElementName, resolver);
                        if ((iterator.Count == 0) && !string.IsNullOrEmpty(table.DataSetName))
                        {
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("搜索:'/{0}:{1}/{2}'", new object[] { namespacePrefix, table.DataSetName, table.MapXMLElementName });
                            }
                            iterator = navigator.Select(string.Format("/{0}:{1}/{2}", namespacePrefix, table.DataSetName, table.MapXMLElementName), resolver);
                        }
                    }
                    else
                    {
                        iterator = navigator.Select(table.MapXMLElementName);
                    }
                    if (iterator.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        foreach (XPathNavigator navigator2 in iterator)
                        {
                            DataRow row = dataTable.NewRow();
                            foreach (DTColumn column in table.DTColumns)
                            {
                                if (!column.IsActive || !column.IfImport)
                                {
                                    continue;
                                }
                                if (!dataTable.Columns.Contains(column.Name))
                                {
                                    goto Label_0615;
                                }
                                if (!string.IsNullOrEmpty(column.MapXMLElementName))
                                {
                                    switch (column.MapXMLElementName.ToUpper())
                                    {
                                        case "[GUID]":
                                            try
                                            {
                                                row[column.Name] = StringHelper.GetNewGuid();
                                            }
                                            catch (Exception exception)
                                            {
                                                LoggingService.Error(exception);
                                            }
                                            goto Label_0580;

                                        case "[PROJECT_ID]":
                                            try
                                            {
                                                row[column.Name] = project_id;
                                            }
                                            catch (Exception exception2)
                                            {
                                                LoggingService.Error(exception2);
                                            }
                                            goto Label_0580;

                                        case "[PRODEF_ID]":
                                            try
                                            {
                                                row[column.Name] = prodef_id;
                                            }
                                            catch (Exception exception3)
                                            {
                                                LoggingService.Error(exception3);
                                            }
                                            goto Label_0580;

                                        case "[NUM]":
                                            try
                                            {
                                                row[column.Name] = dataTable.Rows.Count + 1;
                                            }
                                            catch (Exception exception4)
                                            {
                                                LoggingService.Error(exception4);
                                            }
                                            goto Label_0580;

                                        case "[APPLYING_DATE]":
                                            try
                                            {
                                                row[column.Name] = applyingDate;
                                            }
                                            catch (Exception exception5)
                                            {
                                                LoggingService.Error(exception5);
                                            }
                                            goto Label_0580;

                                        case "[FINISHED_DATE]":
                                            try
                                            {
                                                row[column.Name] = finishedDate;
                                            }
                                            catch (Exception exception6)
                                            {
                                                LoggingService.Error(exception6);
                                            }
                                            goto Label_0580;
                                    }
                                    try
                                    {
                                        SetColumnValue(documents, namespacePrefix, namespaceUri, navigator2, row, column);
                                    }
                                    catch (Exception exception7)
                                    {
                                        LoggingService.Error(string.Format("设置'{0}<{1}>'时出错", column.Name, column.Description), exception7);
                                    }
                                }
                            Label_0580:
                                if (!string.IsNullOrEmpty(column.DefaultValue) && Convert.IsDBNull(row[column.Name]))
                                {
                                    row[column.Name] = column.DefaultValue;
                                }
                                if (LoggingService.IsDebugEnabled && !Convert.IsDBNull(row[column.Name]))
                                {
                                    LoggingService.DebugFormatted("获取的列'{0}<{1}>'的值为:'{2}'", new object[] { column.Name, column.Description, row[column.Name] });
                                }
                                continue;
                            Label_0615:;
                                LoggingService.WarnFormatted("表‘{0}<{1}>’已经不包含列‘{2}<{3}>’", new object[] { table.Name, table.Description, column.Name, column.Description });
                            }
                            dataTable.Rows.Add(row);
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.Debug("添加了新数据,更新表");
                            }
                            adapter.Update(dataTable);
                        }
                        continue;
                    }
                    LoggingService.WarnFormatted("没有找到与表'{0}'对应的'{1}'节点!", new object[] { table.Name, table.MapXMLElementName });
                }
            }
            if (!string.IsNullOrEmpty(dtDatabase.AfterProcedure))
            {
                DbCommand command3 = dbcn.CreateCommand();
                command3.CommandText = dtDatabase.AfterProcedure;
                command3.CommandType = CommandType.StoredProcedure;
                command3.Transaction = tran;
                command3.ExecuteNonQuery();
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("执行导入后存储过程:'{0}'!", new object[] { dtDatabase.AfterProcedure });
                }
            }
        }
    }
}

