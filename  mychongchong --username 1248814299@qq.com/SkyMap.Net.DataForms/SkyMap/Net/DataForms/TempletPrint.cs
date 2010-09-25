namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing.Design;
    using System.IO;

    [Serializable]
    public class TempletPrint : ManyToOneDataSource, ISaveAs
    {
        private byte[] data;
        private bool needAsk;
        private bool printPreview = true;
        private IList<PrintSet> printSets;
        private string sql;
        private string type;
        private int? version;

        public DataSet GetReportDataSource(bool handleAmount, IDictionary<string, string> vals)
        {
            DataTable table;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("配置的SQL是：{0}", new object[] { this.sql });
            }
            string sql = this.sql;
            if (vals != null)
            {
                foreach (KeyValuePair<string, string> pair in vals)
                {
                    sql = sql.Replace(pair.Key, pair.Value);
                }
            }
            string[] strArray = sql.Split(new char[] { '\n', '\r', ';' });
            int length = strArray.Length;
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string[]> list3 = new List<string[]>();
            bool flag = base.DataSource.Id == "SYSTEM_MAIN";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                if (strArray[i].Trim().Length > 0)
                {
                    int index = strArray[i].IndexOf(':');
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("第{0}行配置的SQL是：{1}", new object[] { i, strArray[i] });
                    }
                    string[] strArray2 = strArray[i].Substring(0, index).Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    string item = strArray[i].Substring(index + 1);
                    if (strArray2.Length == 1)
                    {
                        if (flag)
                        {
                            list.Add(strArray2[0]);
                            list2.Add(item);
                        }
                        else
                        {
                            dictionary.Add(strArray2[0], item);
                        }
                    }
                    else if (strArray2.Length > 1)
                    {
                        list3.Add(new string[] { strArray2[0], strArray2[1], item });
                    }
                }
            }
            DataSet set = new DataSet("ds");
            if (list2.Count > 0)
            {
                set = QueryHelper.ExecuteSqls("Default", string.Empty, list2.ToArray(), list.ToArray());
            }
            if (dictionary.Count > 0)
            {
                using (DbConnection connection = base.DataSource.CreateConnection())
                {
                    using (DbCommand command = connection.CreateCommand())
                    {
                        foreach (KeyValuePair<string, string> pair in dictionary)
                        {
                            command.CommandText = pair.Value;
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("将执行SQL:{0}", new object[] { pair.Value });
                            }
                            if (pair.Key != "<EXECUTE>")
                            {
                                if (!pair.Value.ToUpper().Trim().StartsWith("SELECT "))
                                {
                                    LoggingService.WarnFormatted("配置表：{0}的查询有问题，它不是SELECT语句：{1}", new object[] { pair.Key, pair.Value });
                                    continue;
                                }
                                using (DbDataReader reader = command.ExecuteReader())
                                {
                                    table = new DataTable(pair.Key);
                                    table.Load(reader);
                                    set.Tables.Add(table);
                                }
                            }
                            else
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            if (list3.Count > 0)
            {
                foreach (string[] strArray3 in list3)
                {
                    if (strArray3[1] == "<EXECUTE>")
                    {
                        QueryHelper.ExecuteSqlNonQuery(strArray3[0], strArray3[2]);
                    }
                    else
                    {
                        table = QueryHelper.ExecuteSql(strArray3[0], string.Empty, strArray3[2]);
                        table.TableName = strArray3[1];
                        set.Tables.Add(table);
                    }
                }
            }
            if (handleAmount)
            {
                foreach (DataTable tmptable in set.Tables)
                {
                    if (tmptable.Columns.Contains("Amount") && !tmptable.Columns.Contains("ChineseAmount"))
                    {
                        tmptable.Columns.Add("ChineseAmount", typeof(string));
                        tmptable.Columns.Add("ChineseAmount1", typeof(string));
                        foreach (DataRow row in tmptable.Rows)
                        {
                            if (!Convert.IsDBNull(row["Amount"]))
                            {
                                double amount = Convert.ToDouble(row["Amount"]);
                                row["ChineseAmount"] = AmountHelper.SimpleConverChinese(amount, Convert.ToInt32(Math.Ceiling(amount)));
                                row["ChineseAmount1"] = AmountHelper.convertsum1(amount.ToString());
                            }
                        }
                    }
                    if (!tmptable.Columns.Contains("PrintStaff"))
                    {
                        tmptable.Columns.Add("PrintStaff", typeof(string));
                        foreach (DataRow row in tmptable.Rows)
                        {
                            row["PrintStaff"] = SecurityUtil.GetSmIdentity().UserName;
                        }
                    }
                }
            }
            return set;
        }

        private static byte[] Read2Buffer(Stream stream, int BufferLen)
        {
            BinaryReader reader = new BinaryReader(stream);
            byte[] buffer = new byte[stream.Length];
            for (long i = 0L; i < stream.Length; i += 1L)
            {
                buffer[(int) ((IntPtr) i)] = reader.ReadByte();
            }
            return buffer;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        [Browsable(false)]
        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        [DisplayName("打印报表是否需要申请")]
        public bool NeedAsk
        {
            get
            {
                return this.needAsk;
            }
            set
            {
                this.needAsk = value;
            }
        }

        [DisplayName("能否预览")]
        public bool PrintPreview
        {
            get
            {
                return this.printPreview;
            }
            set
            {
                this.printPreview = value;
            }
        }

        [Browsable(false)]
        public IList<PrintSet> PrintSets
        {
            get
            {
                return this.printSets;
            }
            set
            {
                this.printSets = value;
            }
        }

        [DisplayName("报表数据源SQL"), Description("1.可以在SQL语句中使用：{StaffId}：当前用户ID，{StaffName}：当前用户姓名，\r\n {DeptId}：当前用户部门ID，{DeptName}：当前用户部门名称，{ProjectId}：业务编号,\r\n{ProinsId}：流程实例ID,{ActinsId}：当前活动实例ID,{ActdefId}：当前活动定义ID;\r\n2.定义的SQL语句用回车分行，格式是：\r\n1.数据源名:SQL -- 数据源名表示在RDLC之类的报表中添加的数据源名称，必须与实际对应的名称一样\r\n2.命名空间-数据源名:SQL --如果一个报表中的数据源是从多个数据库获取时，使用命名获取对应数据库配置\r\n3.<EXECUTE>:SQL -- 表示在执行这个报表时将需要执行的非SQL语句，比如需要打印时更新其它表的某个字段如已打印，或打印时间等\r\n4.命名空间-<EXECUTE>:SQL"), Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string Sql
        {
            get
            {
                return this.sql;
            }
            set
            {
                this.sql = value;
            }
        }

        [Editor("SkyMap.Net.Gui.Components.BlobPropertyEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DisplayName("模板文件")]
        public string TempletFile
        {
            get
            {
                return string.Format("{0}{1}", this.Name, this.Type);
            }
            set
            {
                using (FileStream stream = File.Open(value, FileMode.Open))
                {
                    if (stream != null)
                    {
                        this.Data = Read2Buffer(stream, -1);
                        stream.Close();
                        if (!(string.IsNullOrEmpty(this.type) || (this.type.IndexOf(",") >= 0)))
                        {
                            this.Type = Path.GetExtension(value);
                        }
                        if (StringHelper.IsNull(this.Name))
                        {
                            this.Name = Path.GetFileName(value).Replace(this.Type, "");
                        }
                        if (this.version.HasValue)
                        {
                            this.version += 1;
                        }
                        else
                        {
                            this.version = 1;
                        }
                    }
                }
            }
        }

        [DisplayName("报表类型(如:.doc)")]
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

        [DisplayName("版本号")]
        public int? Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
    }
}

