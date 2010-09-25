namespace SkyMap.Net.DataForms
{
     using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using System.Windows.Forms;
    using NCalc;

    [ComVisible(true), PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    public static class WebBrowserExtensions
    {
        public const string ATTR_DBCOLUMN = "dbcolumn";
        public const string ATTR_DBTABLE = "dbtable";
        public const string ATTR_DK = "dk";
        public const string ATTR_FORMAT = "format";
        public const string ATTR_HJ = "hj";
        public const string ATTR_HQ = "hq";
        public const string ATTR_INHERITDATA = "jc";
        public const string ATTR_JS = "js";
        public const string ATTR_QM = "qm";
        public const string ATTR_SFCATE = "sfcategory";
        public const string ATTR_TITLE = "title";
        public const string ATTR_VALUE = "value";
        public static string[] tags = new string[] { "input", "select" };

        public static void AttachValueToDataSource(DataSet ds, HtmlDocument doc)
        {
            LoggingService.DebugFormatted("绑定HTML表单数据：{0} 到数据源...", new object[] { doc.Url });
            foreach (string str in tags)
            {
                HtmlElementCollection elementsByTagName = doc.GetElementsByTagName(str);
                foreach (HtmlElement element in elementsByTagName)
                {
                    DataRow row;
                    string attribute = element.GetAttribute("dbtable");
                    string str3 = element.GetAttribute("dbcolumn");
                    if (GetDataRow(ds, attribute, out row) && (row != null))
                    {
                        if (row.RowState == DataRowState.Unchanged)
                        {
                            row.SetModified();
                        }
                        if (!(string.IsNullOrEmpty(str3) || !row.Table.Columns.Contains(str3)))
                        {
                            string str4 = element.GetAttribute("value").Trim();
                            row[str3] = (str4 == string.Empty) ? Convert.DBNull : str4;
                            LoggingService.DebugFormatted("设置表：{0}列:{1}的值为：{2}", new object[] { attribute, str3, str4 });
                        }
                    }
                }
            }
        }

        public static void Calc(HtmlDocument doc, DataSet ds, Dictionary<string, InvokeResult> invokes)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (string str in tags)
            {
                HtmlElementCollection elementsByTagName = doc.GetElementsByTagName(str);
                foreach (HtmlElement element in elementsByTagName)
                {
                    string attribute = element.GetAttribute("sfcategory");
                    if (!string.IsNullOrEmpty(attribute))
                    {
                        string format = element.GetAttribute("format");
                        if ((format == null) || (format.Length == 0))
                        {
                            format = "0.00";
                        }
                        LoggingService.DebugFormatted("计算税费：{0}", new object[] { element.GetAttribute("dbcolumn") });
                        double? nullable = UseGetDataCalc(doc, ds, element, invokes);
                        if (!nullable.HasValue)
                        {
                            nullable = HtmlElementCalc(doc, element, invokes);
                        }
                        if (nullable.HasValue)
                        {
                            if (format.LastIndexOf('.') >= 0)
                            {
                                nullable = new double?(MathHelper.Round(nullable.Value, (format.Length - format.LastIndexOf('.')) - 1));
                            }
                            else
                            {
                                nullable = new double?(MathHelper.Round(nullable.Value, 0));
                            }
                            SetHtmlElementValue(element, nullable.Value.ToString(format));
                            nullable = new double?(Convert.ToDouble(nullable.Value.ToString(format)));
                            if (element.GetAttribute("type") != "hidden")
                            {
                                string[] strArray = attribute.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string str4 in strArray)
                                {
                                    Dictionary<string, double> dictionary2;
                                    string str5;
                                    if (!dictionary.ContainsKey(str4))
                                    {
                                        dictionary.Add(str4, 0.0);
                                    }
                                    (dictionary2 = dictionary)[str5 = str4] = dictionary2[str5] + nullable.Value;
                                }
                            }
                        }
                        else
                        {
                            SetHtmlElementValue(element, string.Empty);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, double> pair in dictionary)
            {
                HtmlElement elementById = doc.GetElementById(pair.Key);
                if (elementById != null)
                {
                    SetHtmlElementValue(elementById, pair.Value.ToString("0.00"));
                }
            }
        }

        public static void GetData(HtmlDocument doc, DataSet ds, Dictionary<string, InvokeResult> invokes)
        {
            if ((doc == null) || (ds == null))
            {
                throw new ArgumentNullException("doc or ds");
            }
            List<HtmlElement> list = new List<HtmlElement>();
            foreach (string str in tags)
            {
                HtmlElementCollection elementsByTagName = doc.GetElementsByTagName(str);
                foreach (HtmlElement element in elementsByTagName)
                {
                    string attribute = element.GetAttribute("ref");
                    if ((attribute != null) && (attribute.Trim().Length > 0))
                    {
                        list.Add(element);
                    }
                    else
                    {
                        SetHtmlElementValue(doc, ds, element, invokes);
                    }
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("获取具有REF属性的数据...");
            }
            foreach (HtmlElement element in list)
            {
                SetHtmlElementValue(doc, ds, element, invokes);
            }
        }

        public static bool GetDataRow(DataSet ds, string tableName, out DataRow row)
        {
            if (ds.Tables.Contains(tableName))
            {
                DataTable table = ds.Tables[tableName];
                if (table.Rows.Count <= 0)
                {
                    LoggingService.DebugFormatted("表{0}还没有可获取使用的数据行", new object[] { tableName });
                    row = null;
                    return true;
                }
                row = table.Rows[0];
            }
            else
            {
                LoggingService.DebugFormatted("当前数据库不包含表{0}", new object[] { tableName });
                row = null;
                return false;
            }
            return true;
        }

        private static string GetMatchString(string[,] tblAndCols, DataSet ds)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tblAndCols.GetLength(0); i++)
            {
                DataRow row;
                if (i > 0)
                {
                    builder.Append("|");
                }
                if ((GetDataRow(ds, tblAndCols[i, 0], out row) && (row != null)) && row.Table.Columns.Contains(tblAndCols[i, 1]))
                {
                    builder.Append(row[tblAndCols[i, 1]]);
                }
            }
            return builder.ToString();
        }

        public static void Hj(HtmlDocument doc, string category)
        {
            double num = 0.0;
            foreach (string str in tags)
            {
                HtmlElementCollection elementsByTagName = doc.GetElementsByTagName(str);
                foreach (HtmlElement element in elementsByTagName)
                {
                    string id = element.Id;
                    string attribute = element.GetAttribute("sfcategory");
                    if ((element.GetAttribute("type") != "hidden") && !string.IsNullOrEmpty(attribute))
                    {
                        foreach (string str4 in attribute.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            double num2;
                            if ((str4 == category) && double.TryParse(element.GetAttribute("value"), out num2))
                            {
                                num += num2;
                            }
                        }
                    }
                }
            }
            try
            {
                SetHtmlElementValue(doc.GetElementById(category), num.ToString("0.00"));
            }
            catch
            {
            }
        }

        private static double? HtmlElementCalc(HtmlDocument doc, HtmlElement field, Dictionary<string, InvokeResult> invokes)
        {
            string attribute = field.GetAttribute("invoke");
            if (!string.IsNullOrEmpty(attribute))
            {
                if ((invokes != null) && invokes.ContainsKey(attribute))
                {
                    return invokes[attribute](field.GetAttribute("params"));
                }
                if (attribute != "qj")
                {
                    throw new ApplicationException(string.Format("没有找到计算{0}值的方法:{1}", field.Id, attribute));
                }
                return QJCalc(doc, field.GetAttribute("params"));
            }
            return SimpleMultiCalc(doc, field.Id);
        }

        public static string ParseGetData(HtmlDocument doc, string parseObject, DataSet ds, string getdata, Dictionary<string, InvokeResult> invokes)
        {
            string[,] strArray2;
            DataRow row;
            double num6;
            LoggingService.DebugFormatted("解析字符串:{0}", new object[] { getdata });
            string[] strArray = getdata.Split(new char[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string s = string.Empty;
            Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
            if (strArray.Length == 1)
            {
                LoggingService.DebugFormatted("仅有一行配置:{0}..", new object[] { strArray[0] });
                if ((strArray[0].IndexOf("|") < 0) && (strArray[0].Split(new char[] { '.' }).Length == 2))
                {
                    strArray2 = ParseTableAndColumn(strArray[0]);
                    LoggingService.DebugFormatted("看是不是可以直接从表{0}中获取字段{1}的值:", new object[] { strArray2[0, 0], strArray2[0, 1] });
                    if (GetDataRow(ds, strArray2[0, 0], out row) && row.Table.Columns.Contains(strArray2[0, 1]))
                    {
                        if (row != null)
                        {
                            s = row[strArray2[0, 1]].ToString();
                        }
                        else
                        {
                            s = string.Empty;
                        }
                    }
                    else
                    {
                        LoggingService.DebugFormatted("没有找到表{0}或列{1},将调用ParseSingeString来解析", new object[] { strArray2[0, 0], strArray2[0, 1] });
                        s = ParseSingeString(doc, ds, strArray[0], invokes);
                    }
                }
                else
                {
                    s = ParseSingeString(doc, ds, strArray[0], invokes);
                }
                try
                {
                    if (!s.EndsWith("%"))
                    {
                        double.Parse(s);
                    }
                }
                catch
                {
                    s = string.Empty;
                }
                LoggingService.DebugFormatted("{0} 解析[{1}]获取的值为: {2}", new object[] { parseObject, strArray[0], s });
                return s;
            }
            strArray2 = ParseTableAndColumn(strArray[0]);
            bool flag = false;
            string str2 = string.Empty;
            for (int i = 1; i < (strArray.Length - 1); i++)
            {
                string[] strArray3 = strArray[i].Split(new char[] { '|' });
                flag = true;
                bool flag2 = false;
                if (strArray3.Length != (strArray2.GetLength(0) + 1))
                {
                    LoggingService.WarnFormatted("配置项：[{0}] 长度：{1} 与列数长度{2}不一致,将跳过这一项配置", new object[] { strArray[i], strArray3.Length - 1, strArray2.GetLength(0) });
                    continue;
                }
                for (int j = 0; j < strArray2.GetLength(0); j++)
                {
                    string[] strArray4 = strArray3[j].Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray4.Length != 0)
                    {
                        double num3;
                        double num4;
                        row = null;
                        if (dictionary.ContainsKey(strArray2[j, 0]))
                        {
                            row = dictionary[strArray2[j, 0]];
                        }
                        else
                        {
                            if (!GetDataRow(ds, strArray2[j, 0], out row))
                            {
                                throw new ApplicationException(string.Format("没有找到表{0}", strArray2[j, 0]));
                            }
                            dictionary.Add(strArray2[j, 0], row);
                        }
                        object dBNull = Convert.DBNull;
                        if (row != null)
                        {
                            dBNull = row[strArray2[j, 1]];
                        }
                        if (strArray4.Length == 1)
                        {
                            if (strArray3[j].Trim() != "*")
                            {
                                Expression expression;
                                strArray3[j] = strArray3[j].Trim();
                                if (TryParseExpression(strArray3[j], out expression))
                                {
                                    expression.Parameters["p"] = dBNull;
                                    try
                                    {
                                        object obj3 = expression.Evaluate();
                                        bool flag4 = false;
                                        if (flag4.Equals(obj3))
                                        {
                                            flag = false;
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        LoggingService.Error(exception);
                                        flag = false;
                                    }
                                }
                                else if (((strArray3[j].Trim().Length > 0) && !strArray3[j].Equals(dBNull.ToString())) && ((!double.TryParse(strArray3[j], out num3) || !double.TryParse(dBNull.ToString(), out num4)) || (num3 != num4)))
                                {
                                    flag = false;
                                }
                                if (!flag)
                                {
                                    LoggingService.InfoFormatted("【{0}】验证:【{1}】, 与 【{2}】,结果是不匹配(【{3}】)", new object[] { parseObject, strArray3[j], dBNull, strArray[i] });
                                    break;
                                }
                            }
                            continue;
                        }
                        flag2 = false;
                        for (int k = 0; k < strArray4.Length; k++)
                        {
                            if ((strArray4[k].Trim().Length > 0) && strArray4[k].Equals(dBNull.ToString()))
                            {
                                flag2 = true;
                                break;
                            }
                            LoggingService.DebugFormatted("{0}:{1}", new object[] { strArray3[0], dBNull.ToString() });
                            if ((double.TryParse(strArray3[j], out num3) && double.TryParse(dBNull.ToString(), out num4)) && (num3 == num4))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2)
                        {
                            flag = false;
                            LoggingService.InfoFormatted("【{0}】验证:【{1}】, 与 【{2}】,结果是不匹配(【{3}】)", new object[] { parseObject, strArray3[j], dBNull, strArray[i] });
                            break;
                        }
                    }
                }
                if (flag)
                {
                    str2 = "[" + strArray[i] + "]";
                    s = strArray3[strArray3.Length - 1];
                    break;
                }
            }
            if (!flag)
            {
                s = strArray[strArray.Length - 1];
            }
            if (LoggingService.IsInfoEnabled)
            {
                string matchString = GetMatchString(strArray2, ds);
                LoggingService.InfoFormatted("【{4}】:【{0}】 匹配:【{1}】 【{2}】,初步结果是:【{3}】", new object[] { str2, matchString, flag ? "成功" : "失败", s, parseObject });
            }
            if (((!s.StartsWith("'") || !s.EndsWith("'")) && (!string.IsNullOrEmpty(s) && !s.EndsWith("%"))) && !double.TryParse(s, out num6))
            {
                if (s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
                {
                    strArray2 = ParseTableAndColumn(s);
                    if (ds.Tables.Contains(strArray2[0, 0]) && ds.Tables[strArray2[0, 0]].Columns.Contains(strArray2[0, 1]))
                    {
                        if (GetDataRow(ds, strArray2[0, 0], out row))
                        {
                            if (row != null)
                            {
                                s = row[strArray2[0, 1]].ToString();
                            }
                            else
                            {
                                s = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        s = ParseSingeString(doc, ds, s, invokes);
                    }
                }
                else
                {
                    s = ParseSingeString(doc, ds, s, invokes);
                }
            }
            if (s.StartsWith("'") && s.EndsWith("'"))
            {
                return s.Substring(1, s.Length - 2);
            }
            if (!((string.IsNullOrEmpty(s) || s.EndsWith("%")) || double.TryParse(s, out num6)))
            {
                s = string.Empty;
            }
            return s;
        }

        private static string ParseSingeString(HtmlDocument doc, DataSet ds, string result, Dictionary<string, InvokeResult> invokes)
        {
            double num;
            if (!double.TryParse(result, out num))
            {
                Expression expression;
                int num2;
                if (result.EndsWith("%") && double.TryParse(result.Substring(0, result.Length - 1), out num))
                {
                    return result;
                }
                if (result.Equals("''"))
                {
                    return string.Empty;
                }
                string[,] strArray = ParseTableAndColumn(result, new char[] { '|', '#' });
                if (TryParseExpression(strArray[0, 0], out expression))
                {
                    double num3;
                    for (num2 = 1; num2 < strArray.GetLength(0); num2++)
                    {
                        object obj2;
                        if (TryParseColumnValue(ds, strArray[num2, 0], strArray[num2, 1], out obj2))
                        {
                            expression.Parameters["p" + num2.ToString()] = obj2;
                        }
                        else
                        {
                            LoggingService.WarnFormatted("无法为表达式:{0}中参数:p{1} 获取 {2}.{3} 的值,可能其为空值或找不到表或列,无法计算,所以返回零值", new object[] { strArray[0, 0], num2, strArray[num2, 0], strArray[num2, 1] });
                            return "0";
                        }
                    }
                    object obj3 = expression.Evaluate();
                    LoggingService.InfoFormatted("表达式:{0} 的值是: {1}", new object[] { strArray[0, 0], obj3 });
                    if (double.TryParse(obj3.ToString(), out num3))
                    {
                        if (num3 >= 0.0)
                        {
                            return MathHelper.Round(num3, 2).ToString();
                        }
                        return "0";
                    }
                    return obj3.ToString();
                }
                for (num2 = 0; num2 < strArray.GetLength(0); num2++)
                {
                    if (ds.Tables.Contains(strArray[num2, 0]) && ds.Tables[strArray[num2, 0]].Columns.Contains(strArray[num2, 1]))
                    {
                        DataRow row;
                        if (GetDataRow(ds, strArray[num2, 0], out row))
                        {
                            if (row == null)
                            {
                                return string.Empty;
                            }
                            object obj4 = row[strArray[num2, 1]];
                            if (!(Convert.IsDBNull(obj4) || (obj4.ToString().Length <= 0)))
                            {
                                return obj4.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (strArray[0, 0] == "invoke")
                        {
                            return invokes[strArray[0, 1]](string.Empty).ToString();
                        }
                        try
                        {
                            return doc.GetElementById(strArray[0, 0]).GetAttribute(strArray[0, 1]);
                        }
                        catch (Exception exception)
                        {
                            LoggingService.Error(exception);
                        }
                    }
                }
            }
            return result;
        }

        private static string[,] ParseTableAndColumn(string input)
        {
            return ParseTableAndColumn(input, new char[] { '|' });
        }

        private static string[,] ParseTableAndColumn(string input, params char[] splitChars)
        {
            string[] strArray = input.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            string[,] strArray2 = new string[strArray.Length, 2];
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray3 = strArray[i].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray3.Length == 2)
                {
                    strArray2[i, 0] = strArray3[0];
                    strArray2[i, 1] = strArray3[1];
                }
                else
                {
                    strArray2[i, 0] = strArray3[0];
                    strArray2[i, 1] = null;
                    LoggingService.WarnFormatted("配置错误:{0},格式应为:表名.列名", new object[] { input });
                }
            }
            return strArray2;
        }

        private static double? QJCalc(HtmlDocument doc, string p)
        {
            string[] strArray = p.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            double? nullable = null;
            HtmlElement elementById = doc.GetElementById(strArray[0]);
            if (elementById != null)
            {
                double num;
                if (double.TryParse(elementById.GetAttribute("value"), out num))
                {
                    int length = strArray.Length;
                    for (int i = 1; i < length; i++)
                    {
                        double num4;
                        if (double.TryParse(strArray[i], out num4))
                        {
                            if (length > (i + 1))
                            {
                                if (num <= num4)
                                {
                                    double num5;
                                    if (double.TryParse(strArray[i + 1], out num5))
                                    {
                                        return new double?(num5);
                                    }
                                    LoggingService.WarnFormatted("{0} 不是数字", new object[] { strArray[i + 1] });
                                    return nullable;
                                }
                            }
                            else
                            {
                                nullable = new double?(num4);
                            }
                        }
                        else
                        {
                            LoggingService.WarnFormatted("{0} 不是数字", new object[] { strArray[i] });
                            return nullable;
                        }
                        i++;
                    }
                }
                return nullable;
            }
            LoggingService.WarnFormatted("找不到控件 {0} ", new object[] { strArray[0] });
            return nullable;
        }

        public static void SetHtmlElementValue(HtmlElement he, string value)
        {
            LoggingService.DebugFormatted("将设置{0}-{1}的值：{2}", new object[] { he.Document.Url.ToString(), he.Id, value });
            he.SetAttribute("value", value);
            if (he.TagName.ToLower() == "select")
            {
                string attribute = he.GetAttribute("valuechange");
                if ((attribute != null) && (attribute.Length > 0))
                {
                    he.Document.InvokeScript(attribute, new string[] { value });
                }
            }
        }

        private static void SetHtmlElementValue(HtmlDocument doc, DataSet ds, HtmlElement he, Dictionary<string, InvokeResult> invokes)
        {
            string attribute = he.GetAttribute("ref");
            if ((attribute != null) && (attribute.Trim().Length > 0))
            {
                HtmlElement elementById = doc.GetElementById(attribute);
                if (elementById != null)
                {
                    double? nullable = HtmlElementCalc(doc, elementById, invokes);
                    SetHtmlElementValue(he, nullable.HasValue ? MathHelper.Round(nullable.Value, 2).ToString("0.00") : string.Empty);
                }
            }
            else
            {
                string getdata = he.GetAttribute("getdata").Trim().Replace("\r", "").Replace("\n", "");
                if ((getdata != null) && (getdata.Length > 0))
                {
                    SetHtmlElementValue(he, ParseGetData(doc, he.GetAttribute("dbcolumn"), ds, getdata, invokes));
                }
            }
        }

        private static double? SimpleMultiCalc(HtmlDocument doc, string fieldId)
        {
            double? nullable3;
            double num2;
            double? nullable = 0.0;
            int num = 1;
            string str = fieldId.Remove(fieldId.Length - 1);
            HtmlElement elementById = doc.GetElementById(str + "_" + num.ToString());
            nullable = 0.0;
            if (elementById != null)
            {
                nullable = 1.0;
                do
                {
                    try
                    {
                        double? nullable4;
                        string str2 = elementById.GetAttribute("value").Trim();
                        LoggingService.DebugFormatted("{0}-{1}:{2}", new object[] { elementById.Id, elementById.GetAttribute("dbcolumn"), str2 });
                        if (elementById.GetAttribute("iskj") == "1")
                        {
                            if (!string.IsNullOrEmpty(str2))
                            {
                                LoggingService.DebugFormatted("计算扣减额", new object[0]);
                                nullable3 = nullable;
                                num2 = double.Parse(str2);
                                nullable = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() - num2) : ((double?) (nullable4 = null));
                            }
                        }
                        else
                        {
                            if ((str2 == null) || (str2.Length == 0))
                            {
                                nullable = 0.0;
                                break;
                            }
                            if (str2.EndsWith("%"))
                            {
                                nullable3 = nullable;
                                nullable = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() / 100.0) : ((double?) (nullable4 = null));
                                str2 = str2.Remove(str2.Length - 1);
                            }
                            nullable3 = nullable;
                            num2 = double.Parse(str2);
                            nullable = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() * num2) : ((double?) (nullable4 = null));
                        }
                        num++;
                        elementById = doc.GetElementById(str + "_" + num.ToString());
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error(exception);
                        return null;
                    }
                }
                while (elementById != null);
            }
            if ((doc.GetElementById(str + "_c") != null) && (doc.GetElementById(str + "_d") != null))
            {
                string str3 = doc.GetElementById(str + "_c").GetAttribute("value").Trim();
                string str4 = doc.GetElementById(str + "_d").GetAttribute("value").Trim();
                if ((((str3 != null) && (str3.Length != 0)) && (str4 != null)) && (str4.Length != 0))
                {
                    nullable3 = nullable;
                    num2 = double.Parse(doc.GetElementById(str + "_c").GetAttribute("value").Trim()) / double.Parse(doc.GetElementById(str + "_d").GetAttribute("value").Trim());
                    nullable = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() * num2) : null;
                }
            }
            LoggingService.DebugFormatted("{0}:{1}", new object[] { doc.GetElementById(fieldId).GetAttribute("dbcolumn"), nullable });
            return nullable;
        }

        private static bool TryParseColumnValue(DataSet ds, string tableName, string columnName, out object value)
        {
            DataRow row;
            if (((ds.Tables.Contains(tableName) && ds.Tables[tableName].Columns.Contains(columnName)) && GetDataRow(ds, tableName, out row)) && (row != null))
            {
                object obj2 = row[columnName];
                if (!(Convert.IsDBNull(obj2) || (obj2.ToString().Length <= 0)))
                {
                    value = obj2;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public static bool TryParseExpression(string evalExpression, out Expression expression)
        {
            LoggingService.DebugFormatted("测试{0}是否以'eval('开头：{1}，')'结束：{2}...", new object[] { evalExpression, evalExpression.StartsWith("eval(", StringComparison.OrdinalIgnoreCase), evalExpression.EndsWith(")") });
            if (evalExpression.StartsWith("eval(", StringComparison.OrdinalIgnoreCase) && evalExpression.EndsWith(")"))
            {
                string str = evalExpression.Substring(5, evalExpression.Length - 6);
                expression = new Expression(str);
                expression.EvaluateFunction += delegate (string name, FunctionArgs args) {
                    object obj2;
                    LoggingService.DebugFormatted("将解析{0}函数...", new object[] { name });
                    if (name == "IsNull")
                    {
                        obj2 = args.Parameters[0].Evaluate();
                        args.Result = (obj2 == null) || string.IsNullOrEmpty(obj2.ToString());
                        LoggingService.DebugFormatted("解析{0}函数的结果是：{1}", new object[] { name, args.Result });
                    }
                    else if (name.StartsWith("Substring_"))
                    {
                        obj2 = args.Parameters[0].Evaluate();
                        string[] strArray = name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray.Length == 3)
                        {
                            int num;
                            int num2;
                            string str1 = strArray[1];
                            string str2 = strArray[2];
                            if ((!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2)) && (int.TryParse(str1, out num) && int.TryParse(str2, out num2)))
                            {
                                if ((obj2 == null) || string.IsNullOrEmpty(obj2.ToString()))
                                {
                                    args.Result = string.Empty;
                                }
                                else
                                {
                                    args.Result = obj2.ToString().Substring(num, num2);
                                }
                                LoggingService.DebugFormatted("将解析{0}函数的结果是:{1}", new object[] { name, args.Result });
                                return;
                            }
                        }
                        LoggingService.WarnFormatted("配置的{0}（{}）有不正确的数据", new object[] { obj2, name });
                    }
                };
            }
            else
            {
                expression = null;
            }
            return (expression != null);
        }

        private static double? UseGetDataCalc(HtmlDocument doc, DataSet ds, HtmlElement field, Dictionary<string, InvokeResult> invokes)
        {
            string attribute = field.GetAttribute("getdata_1");
            double? nullable = null;
            int num = 1;
            while (!string.IsNullOrEmpty(attribute))
            {
                double num2;
                if (double.TryParse(ParseGetData(doc, field.GetAttribute("dbcolumn"), ds, attribute, invokes), out num2))
                {
                    nullable = new double?(nullable.HasValue ? (nullable.Value * num2) : num2);
                }
                num++;
                attribute = field.GetAttribute(string.Format("getdata_{0}", num));
            }
            return nullable;
        }
    }
}

