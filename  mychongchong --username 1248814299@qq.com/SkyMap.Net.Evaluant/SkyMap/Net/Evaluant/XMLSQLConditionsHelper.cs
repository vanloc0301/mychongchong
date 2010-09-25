namespace SkyMap.Net.Evaluant
{
   
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Xml;
    using NCalc;


    public static class XMLSQLConditionsHelper
    {
        public static bool Eval(SQLCondition sc)
        {
            if (!string.IsNullOrEmpty(sc.SQL))
            {
                switch (sc.Type)
                {
                    case SQLType.Query:
                    {
                        DataTable dt = QueryHelper.ExecuteSql(sc.NameSpace, string.Empty, sc.SQL);
                        return (string.IsNullOrEmpty(sc.EvalExpression) || Eval(dt, sc.EvalExpression));
                    }
                    case SQLType.Execute:
                    {
                        int parameterValue = QueryHelper.ExecuteSqlNonQuery(sc.NameSpace, sc.SQL);
                        return (string.IsNullOrEmpty(sc.EvalExpression) || Eval(parameterValue, sc.EvalExpression));
                    }
                }
            }
            return false;
        }

        public static bool Eval(DataTable dt, string strExpression)
        {
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            Expression expression = new Expression(strExpression);
            foreach (DataColumn column in dt.Columns)
            {
                expression.Parameters[column.ColumnName] = dt.Rows[0][column.ColumnName];
            }
            bool flag = true;
            return flag.Equals(expression.Evaluate());
        }

        public static bool Eval(object parameterValue, string strExpression)
        {
            Expression expression = new Expression(strExpression);
            expression.Parameters["p"] = parameterValue;
            bool flag = true;
            return flag.Equals(expression.Evaluate());
        }

        public static List<SQLCondition> Parser(string xmlText)
        {
            List<SQLCondition> list3;
            List<SQLCondition> list = new List<SQLCondition>();
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlText);
                if (document.DocumentElement.Name == "conditions")
                {
                    foreach (XmlNode node in document.GetElementsByTagName("condition"))
                    {
                        SQLCondition item = new SQLCondition();
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            string name = node2.Name;
                            if (name != null)
                            {
                                if (!(name == "sql"))
                                {
                                    if (name == "type")
                                    {
                                        goto Label_00E5;
                                    }
                                    if (name == "eval")
                                    {
                                        goto Label_010A;
                                    }
                                    if (name == "namespace")
                                    {
                                        goto Label_011A;
                                    }
                                    if (name == "message")
                                    {
                                        goto Label_012A;
                                    }
                                }
                                else
                                {
                                    item.SQL = node2.InnerText;
                                }
                            }
                            continue;
                        Label_00E5:
                            item.Type = (SQLType) Enum.Parse(typeof(SQLType), node2.InnerText, true);
                            continue;
                        Label_010A:
                            item.EvalExpression = node2.InnerText;
                            continue;
                        Label_011A:
                            item.NameSpace = node2.InnerText;
                            continue;
                        Label_012A:
                            item.Message = node2.InnerText;
                        }
                        list.Add(item);
                    }
                }
                list3 = list;
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                throw exception;
            }
            return list3;
        }

        public static bool ParserAndEval(string xmlText, out string message)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将解析和验证：{0}", new object[] { xmlText });
            }
            foreach (SQLCondition condition in Parser(xmlText))
            {
                if (!Eval(condition))
                {
                    message = condition.Message;
                    LoggingService.DebugFormatted(message, new object[0]);
                    return false;
                }
            }
            message = null;
            return true;
        }
    }
}

