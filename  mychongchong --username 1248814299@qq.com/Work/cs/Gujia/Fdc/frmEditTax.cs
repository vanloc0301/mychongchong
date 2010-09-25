using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data;
using System.Data.SqlClient;
using SkyMap.Net.Core;
using SkyMap.Net.Gui;
using SkyMap.Net.DataForms;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace ZBPM
{
    /// <summary>
    /// Summary description for frmEditTax.
    /// </summary>
    [ComVisible(true)]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class frmEditTax : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 地块的id
        /// </summary>
        public string m_strDkid;
        public DataSet m_dstTax;
        public DataTable m_dtbTax;
        private WebBrowser htmlCalc;
        string[] tags = new string[] { "input", "select" };
        string zxdh = string.Empty;

        string dataPath = Path.Combine(PropertyService.DataDirectory, "resources" + Path.DirectorySeparatorChar + "DataForms" + Path.DirectorySeparatorChar + "ESF");
        private Button btGetData;
        private Button btCalc;
        private Button m_btnSave;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmEditTax()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            InitHtmls();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.htmlCalc = new System.Windows.Forms.WebBrowser();
            this.btGetData = new System.Windows.Forms.Button();
            this.btCalc = new System.Windows.Forms.Button();
            this.m_btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // htmlCalc
            // 
            this.htmlCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlCalc.Location = new System.Drawing.Point(8, 8);
            this.htmlCalc.MinimumSize = new System.Drawing.Size(20, 20);
            this.htmlCalc.Name = "htmlCalc";
            this.htmlCalc.Size = new System.Drawing.Size(652, 410);
            this.htmlCalc.TabIndex = 1;
            // 
            // btGetData
            // 
            this.btGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btGetData.Location = new System.Drawing.Point(8, 423);
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(75, 23);
            this.btGetData.TabIndex = 3;
            this.btGetData.Text = "获取数据";
            this.btGetData.UseVisualStyleBackColor = true;
            this.btGetData.Click += new System.EventHandler(this.btGetData_Click);
            // 
            // btCalc
            // 
            this.btCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCalc.Location = new System.Drawing.Point(100, 423);
            this.btCalc.Name = "btCalc";
            this.btCalc.Size = new System.Drawing.Size(75, 23);
            this.btCalc.TabIndex = 2;
            this.btCalc.Text = "税费计算";
            this.btCalc.UseVisualStyleBackColor = true;
            this.btCalc.Click += new System.EventHandler(this.btCalc_Click);
            // 
            // m_btnSave
            // 
            this.m_btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_btnSave.Location = new System.Drawing.Point(192, 423);
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(75, 23);
            this.m_btnSave.TabIndex = 4;
            this.m_btnSave.Text = "保存并关闭";
            this.m_btnSave.UseVisualStyleBackColor = true;
            this.m_btnSave.Click += new System.EventHandler(this.m_btnSave_Click);
            // 
            // frmEditTax
            // 
            this.ClientSize = new System.Drawing.Size(665, 491);
            this.Controls.Add(this.m_btnSave);
            this.Controls.Add(this.btGetData);
            this.Controls.Add(this.btCalc);
            this.Controls.Add(this.htmlCalc);
            this.MaximizeBox = false;
            this.Name = "frmEditTax";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地块税费交易信息";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }
        #endregion

        private void frmEditTax_Load(object sender, EventArgs e)
        {
//            this.承担方.ColumnEdit = SkyMap.Net.Gui.Components.DataWordLookUpEditHelper.Create("chengdanfang", "Name", "Code");
           
//            m_dstTax = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("SkyMap.Net.DAO", new string[]{@"SELECT *
//FROM YW_tdzbpm_tax where dk_id ="+m_strDkid}, new string[] { "" });
//            if (m_dstTax != null && m_dstTax.Tables.Count != 0)
//            {
//                m_dstTax.Tables[0].ExtendedProperties.Add("selectsql", @"SELECT *
//FROM YW_tdzbpm_tax where dk_id =" + m_strDkid);
//                gridtax.DataMember = m_dstTax.Tables[0].TableName;
//                gridtax.DataSource = m_dstTax;
//            }
//            m_btnSave.Enabled = false;
        }

        void InitHtmls()
        {

            WebBrowser[] wbs = new WebBrowser[] { this.htmlCalc };
            foreach (WebBrowser wb in wbs)
            {
                wb.AllowWebBrowserDrop = false;
                wb.IsWebBrowserContextMenuEnabled = false;
                //htmlCalc.WebBrowserShortcutsEnabled = false;
                wb.ObjectForScripting = this;

            }
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //htmlCalc.DocumentStream = assembly.GetManifestResourceStream("SkyMap.Net.DataForms.Calc.htm");
            htmlCalc.Url = new Uri(dataPath + Path.DirectorySeparatorChar + "ZBPMCalc.htm", UriKind.Absolute);
            htmlCalc.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(htmlCalc_DocumentCompleted);

    
        }


        private void AttachDataToCalcDocument()
        {
            m_dstTax = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("SkyMap.Net.DAO", new string[]{@"SELECT *
FROM YW_tdzbpm_td where dkid ="+m_strDkid}, new string[] { "" });
            m_dtbTax = m_dstTax.Tables[0];
            m_dtbTax.ExtendedProperties.Add("selectsql", @"SELECT *
FROM YW_tdzbpm_td where dkid =" + m_strDkid);

            if (m_dtbTax.Rows.Count == 0) return;
            HtmlDocument doc = htmlCalc.Document;

            foreach (string tag in tags)
            {
                HtmlElementCollection fields = doc.GetElementsByTagName(tag);
                foreach (HtmlElement field in fields)
                {
                    string column = field.GetAttribute("dbcolumn");
                    if (column != null && column.Length > 0)
                        if (m_dtbTax.Columns.Contains(column))
                        {
                            field.KeyDown -= new HtmlElementEventHandler(field_KeyDown);
                            field.MouseDown -= new HtmlElementEventHandler(field_KeyDown);
                            object value = m_dtbTax.Rows[0][column];
                            if (field.GetAttribute("type") != "hidden")
                            {
                                if (!Convert.IsDBNull(value))
                                {
                                    SetHtmlElementValue(field, value.ToString());
                                    //field.SetAttribute("value", value.ToString());
                                }
                                else
                                {
                                    SetHtmlElementValue(field, string.Empty);
                                    //field.SetAttribute("value", string.Empty);
                                }
                                field.KeyDown += new HtmlElementEventHandler(field_KeyDown);
                                field.MouseDown += new HtmlElementEventHandler(field_KeyDown);
                            }

                        }
                }
            }

        }

        void SetHtmlElementValue(HtmlElement he, string value)
        {
            he.SetAttribute("value", value);
            if (he.TagName.ToLower() == "select")
            {
                string onclick = he.GetAttribute("valuechange");
                if (onclick != null && onclick.Length > 0)
                    he.Document.InvokeScript(onclick, new string[] { value });
            }
        }
        /// <summary>
        /// 设置指定HtmlElement的"value"属性值.
        /// </summary>
        private void SetHtmlElementValue(DataTable dt, HtmlElement he)
        {
            string sRef = he.GetAttribute("ref");

            if (sRef != null && sRef.Trim().Length > 0)
            {
                HtmlElement refHe = this.htmlCalc.Document.GetElementById(sRef);
                if (refHe != null)
                {
                    double? v = HtmlElementCalc(this.htmlCalc.Document, refHe);
                    SetHtmlElementValue(he, v.HasValue ? v.Value.ToString("0.00") : string.Empty);
                }
                //he.SetAttribute("value",HtmlElementCalc(this.htmlCalc.Document,refHe).ToString("0.00"));
                return;

            }

            string getdata = he.GetAttribute("getdata").Trim().Replace("\r", "").Replace("\n", "");
            if (getdata != null && getdata.Length > 0)
            {
                //he.SetAttribute("value", ParseGetData(dt.Rows[0], getdata));
                SetHtmlElementValue(he, ParseGetData(dt.Rows[0], getdata));
            }
        }
        void field_KeyDown(object sender, HtmlElementEventArgs e)
        {

            //OnChanged(this, null);
            //if (LoggingService.IsInfoEnabled)
            //    LoggingService.Info("按键修改！");
            //m_hte = ((HtmlElement)sender);
        }
        void htmlCalc_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            AttachDataToCalcDocument();
            //try
            //{
            //    zxdh = htmlZqcs.Document.GetElementById("txt_Zqzxdh").GetAttribute("getdata");
            //}
            //catch (System.Exception exc)
            //{
            //    LoggingService.Error(exc.Message, exc);
            //}
        }

        private void btGetData_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = this.htmlCalc.Document;
            //this.PostEditor();

            if (m_dtbTax.Rows.Count == 0) return;

            List<HtmlElement> refFields = new List<HtmlElement>();
            foreach (string tag in tags)
            {
                HtmlElementCollection fields = doc.GetElementsByTagName(tag);

                foreach (HtmlElement he in fields)
                {
                    //如果具有ref属性说明需要最后来计算
                    string sRef = he.GetAttribute("ref");

                    if (sRef != null && sRef.Trim().Length > 0)
                        refFields.Add(he);
                    else
                        SetHtmlElementValue(m_dtbTax, he);
                }

            }

            if (LoggingService.IsDebugEnabled)
                LoggingService.Debug("获取具有REF属性的数据...");
            foreach (HtmlElement he in refFields)
            {
                SetHtmlElementValue(m_dtbTax, he);
            }

            //OnChanged(this, null);
            if (LoggingService.IsInfoEnabled)
                LoggingService.Info("获取数据修改！");
        }

        /// <summary>
        /// 分析输入getdata字符串,获得需要使用的税费计算值.
        /// </summary>
        /// <remarks>
        /// 例如:当getdata的值是:"Zrlx|Zrfs,房地产|赠与|0.00%,空地|赠与|0.00%,0.10%"时,会将它先用","分隔成数组ps,
        /// ps[0]表示辨别的字段例表,用"|"分隔多个字段.
        /// 然后从1到N-1(N=ps.Length-2),对其中的值与字段值进行匹配比较,例如如果当"Zrlx"等于"房地产","Zrfs"等于"赠与"时就会设置返回值为"0.00%"
        /// 只有发现不匹配就对下一个进行比较;
        /// 如果所有的都不匹配就使用最后一个默认值,在示例中也就是0.10%;
        /// 对于某些可能需要更复杂计算的就用HtmlElement.Tag来标记,需要单独处理.
        /// </remarks>
        private string ParseGetData(DataRow row, string getdata)
        {
            string[] ps = getdata.Split(new char[] { ',' });

            string result = string.Empty;

            if (ps.Length == 1)
            {

                //直接包含指定字段时
                if (row.Table.Columns.Contains(ps[0]))
                    result = row[ps[0]].ToString();
                else
                {
                    result = ParseSingeString(row, ps[0]);
                }
                try
                {
                    //如果是数字
                    if (!result.EndsWith("%"))
                        double.Parse(result);
                }
                catch
                {
                    result = string.Empty;
                }
                return result;
            }

            string[] fields = ps[0].Split(new char[] { '|' });
            string[] vs;

            for (int i = 1; i < ps.Length - 1; i++)
            {
                vs = ps[i].Split(new char[] { '|' });
                bool match = true;
                for (int n = 0; n < vs.Length - 1; n++)
                {
                    //如果字段值不为空,就与当前值比较,如果不一样说明不是这个条件退出
                    if (vs[n].Trim().Length > 0 && !vs[n].Equals(row[fields[n]]))
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    result = vs[vs.Length - 1];
                    break;
                }
            }
            //没有找到任何匹配的
            if (result == string.Empty)
                result = ps[ps.Length - 1];
            try
            {
                if (!result.EndsWith("%"))
                    //如果是数字
                    double.Parse(result);
            }
            catch
            {
                //不是数字时
                if (row.Table.Columns.Contains(result))
                    result = row[result].ToString();
                else
                {
                    result = ParseSingeString(row, result);
                }
                try
                {
                    //如果是数字
                    if (!result.EndsWith("%"))
                        double.Parse(result);
                }
                catch
                {
                    result = string.Empty;
                }
                if (result == "''") result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 计算指定的HtmlElement税费。
        /// </summary>
        /// <remarks>通用的算法如下：
        /// 首先获取HtmlElement的ID属性值，如sf_1a,（其中的a表示的是对转让方的收费,如果是b则表示是对受让方的收费)
        /// 然后根据前缀sf_1来获取:sf_1_1,sf_1_2... HtmlElement
        /// 将获取的各个HtmlElement的Value相乘,将获得的结果作为返回结果;
        /// 由于某些不能适用通用的算法,所以在标签里里加入"invoke"与"params"属性,如果有些标记就需要对它们进行单独的处理.
        /// </remarks>
        private double? HtmlElementCalc(HtmlDocument doc, HtmlElement field)
        {
            double? result = 0;
            switch (field.GetAttribute("invoke"))
            {
                case "TddcfCalc":
                    result = TddcfCalc(field.GetAttribute("params"));
                    break;
                case "FdccqzmfCalc":
                    result = FdccqzmfCalc(field.GetAttribute("params"));
                    break;
                default:
                    result = SimpleMultiCalc(doc, field.Id);
                    break;
            }
            return result;
        }


        /// <summary>
        /// 计算房地产产权证明费
        /// </summary>
        private double FdccqzmfCalc(string parameters)
        {
            double result = 0;
            string[] pms = parameters.Split(new char[] { ',' });

            if (pms.Length != 3)
            {
                return 0;
            }

            string heId = pms[0];
            //if ("空地".Equals(GetColumnValue("zrlx")))
            //{
            //    heId = heId + "_td";
            //}
            //else
            //{
            //    heId = heId + "_fc";
            //}
            //取两者之中大者
            double mj = 0, mj1 = 0, mj2 = 0;
            try
            {
                HtmlDocument doc = htmlCalc.Document;
                mj1 = Convert.ToDouble(GetHtmlElementValue(doc, heId + "_td"));
                mj2 = Convert.ToDouble(GetHtmlElementValue(doc, heId + "_fc"));


            }
            catch
            {

            }
            finally
            {
                mj = mj1 > mj2 ? mj1 : mj2;
            }
            if (mj == 0) return 0;

            string[] qj = pms[1].Split(new char[] { '|' });
            string[] qjv = pms[2].Split(new char[] { '|' });
            if (qjv.Length != (qj.Length + 1))
            {
                return 0;
            }

            try
            {
                for (int i = 0; i < qj.Length; i++)
                {
                    if ((i + 1) == qj.Length && mj > double.Parse(qj[i]))
                    {
                        result = double.Parse(qjv[i + 1]);
                        break;
                    }
                    else if (mj < double.Parse(qj[i]) || mj == double.Parse(qj[i]))
                    {
                        result = double.Parse(qjv[i]);
                        break;
                    }

                }
            }
            catch
            {
                return 0;
            }

            return result;
        }

        /// <summary>
        /// 通用的税费乘法计算.
        /// </summary>
        /// <remarks>简单的获取各个元素对应的"value"属性值,然后将它们相乘获得税费结果.</remarks>
        private static double? SimpleMultiCalc(HtmlDocument doc, string fieldId)
        {
            double? result = 0;
            int i = 1;
            string pre_heId;
            HtmlElement he;
            pre_heId = fieldId.Remove(fieldId.Length - 1);
            he = doc.GetElementById(pre_heId + "_" + i.ToString());
            result = 0;

            if (he != null)
            {
                result = 1;

                while (he != null)
                {
                    try
                    {
                        string value = he.GetAttribute("value").Trim();
                        if (value == null || value.Length == 0)
                        {
                            result = 0;
                            break;
                        }
                        else if (value.EndsWith("%"))
                        {
                            result = result / 100;
                            value = value.Remove(value.Length - 1);
                        }

                        result = result * double.Parse(value);
                        i++;
                        he = doc.GetElementById(pre_heId + "_" + i.ToString());
                    }
                    catch (System.Exception e)
                    {
                        return null;
                        break;
                    }
                }
            }
            if (doc.GetElementById(pre_heId + "_" + "c") != null && doc.GetElementById(pre_heId + "_" + "d") != null)
            {
                string c = doc.GetElementById(pre_heId + "_" + "c").GetAttribute("value").Trim();
                string d = doc.GetElementById(pre_heId + "_" + "d").GetAttribute("value").Trim();
                if (c != null && c.Length != 0 && d != null && d.Length != 0)
                {
                    result = result * (double.Parse(doc.GetElementById(pre_heId + "_" + "c").GetAttribute("value").Trim()) / double.Parse(doc.GetElementById(pre_heId + "_" + "d").GetAttribute("value").Trim()));

                }

            }
            return result;
        }

        /// <summary>
        /// 计算土地权属调查、地籍测绘费
        /// </summary>
        private int TddcfCalc(string parameters)
        {
            string[] pms = parameters.Split(new char[] { ',' });
            if (pms.Length != 6)
            {
                LoggingService.ErrorFormatted("Html element params length error : '{0}'", parameters);
                return 0;
            }

            int[] npms = new int[6];

            HtmlElement he = htmlCalc.Document.GetElementById(pms[0]);
            if (he == null)
            {
                LoggingService.ErrorFormatted("Cannot find html element : {0}", pms[0]);
                return 0;
            }
            else
            {
                string heValue = he.GetAttribute("value");
                if (heValue == null || heValue.Trim().Length == 0)
                {
                    return 0;
                }

                try
                {
                    npms[0] = (int)Convert.ToDouble(heValue);
                }
                catch (System.Exception e)
                {
                    //MessageHelper.ShowInfo("{0}必须输入数字!",he.GetAttribute("dbcolumn"));
                    return 0;
                }
            }

            for (int i = 1; i < 6; i++)
            {
                try
                {
                    npms[i] = int.Parse(pms[i]);
                }
                catch
                {
                    LoggingService.ErrorFormatted("Html element params '{0}' of '{1}' must numeric! ", parameters, i);
                }
            }

            if (npms[0] <= npms[1])
                return npms[2];

            return npms[2] + ((int)((npms[0] - npms[1]) / npms[3])) * npms[4] > npms[5] ? npms[5] : npms[2] + ((int)((npms[0] - npms[1]) / npms[3])) * npms[4];

        }
        /// <summary>
        /// 分析单个字符串获得税费计算参数.
        /// </summary>
        private string ParseSingeString(DataRow row, string result)
        {
            string[] fs = result.Split(new char[] { '|' });
            foreach (string f in fs)
            {
                if (row.Table.Columns.Contains(f))
                {
                    object o = row[f];
                    if (!Convert.IsDBNull(o) && o.ToString().Length > 0)
                        return o.ToString();
                }
                else
                {
                    string[] cs = result.Split(new char[] { '.' });
                    //如果是控件指定属性值
                    if (cs.Length == 2)
                    {
                        switch (cs[0])
                        {
                            case "txt_Fcjdmj":
                                return CalcFcjdmjYH();
                            case "txt_Srf":
                                return GetGYRNum().ToString();
                            default:
                                break;

                        }
                        //Control c = FindControl(cs[0]);
                        //if (c != null)
                        //{
                        //    switch (cs[1])
                        //    {
                        //        case "Tag":
                        //            return c.Tag.ToString();
                        //        default:
                        //            break;
                        //    }
                        //}
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 从指定ID的HtmlElement的值。
        /// </summary>
        private string GetHtmlElementValue(HtmlDocument doc, string id)
        {
            return doc.GetElementById(id).GetAttribute("value");
        }


        /// <summary>
        /// 当是自建房时根据基底面积来计算优惠数。
        /// </summary>
        private string CalcFcjdmjYH()
        {
            double jdmj = 0;
            //基底面积与个人自建房有关
            try
            {
                jdmj = Convert.ToDouble(GetColumnValue("用地面积"));
                //HtmlElement txt_交易底价 = htmlZqcs.Document.GetElementById("txt_交易底价");
                double result = ((jdmj - 60) / jdmj * Convert.ToDouble(GetColumnValue("交易底价")));//txt_交易底价.GetAttribute("value")));
                return result > 0 ? result.ToString("0.00") : "0.00";
            }
            catch
            {
                return "0.00";
            }

        }

        /// <summary>
        /// 获取共有人数量。
        /// </summary>
        private int GetGYRNum()
        {
            try
            {
                string srf = GetColumnValue("Cqr"); //htmlZqsj.Document.GetElementById("txt_Cqr");
                int i = srf.Split(new char[] { '\\' }).Length;
                return i - 1 > -1 ? i - 1 : 0;
            }
            catch
            {
                return 0;
            }

        }
        string GetColumnValue(string columnName)
        {
            DataTable dt = m_dtbTax;
            DataRow row = dt.Rows[0];
            return row[columnName].ToString();
        }

        /// <summary>
        /// 执行税费计算
        /// </summary>
        private void btCalc_Click(object sender, EventArgs e)
        {
            double countA = 0, countB = 0;

            HtmlDocument doc = htmlCalc.Document;
            foreach (string tag in tags)
            {
                HtmlElementCollection fields = doc.GetElementsByTagName(tag);

                foreach (HtmlElement field in fields)
                {
                    string fieldId = field.Id;
                    string format = field.GetAttribute("format");
                    if (format == null || format.Length == 0)
                        format = "0.00";
                    double? result = 0;
                    if (fieldId.EndsWith("a", StringComparison.OrdinalIgnoreCase) || fieldId.EndsWith("b", StringComparison.OrdinalIgnoreCase))
                    {
                        result = HtmlElementCalc(doc, field);

                        //field.SetAttribute("value", result.ToString(format));
                        if (result.HasValue)
                        {
                            SetHtmlElementValue(field, result.Value.ToString(format));
                            result = Convert.ToDouble(result.Value.ToString(format));
                            if (field.GetAttribute("type") != "hidden")
                            {
                                if (fieldId.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                                {
                                    countA += result.Value;
                                }
                                else
                                {
                                    countB += result.Value;
                                }
                            }
                        }
                        else
                            SetHtmlElementValue(field, string.Empty);


                    }
                }

            }

            try
            {
                HtmlElement heA = doc.GetElementById("sf_acount");
                //heA.SetAttribute("value", countA.ToString("0.00"));
                SetHtmlElementValue(heA, countA.ToString("0.00"));
                HtmlElement heB = doc.GetElementById("sf_bcount");
                //heB.SetAttribute("value", countB.ToString("0.00"));
                SetHtmlElementValue(heB, countB.ToString("0.00"));
            }
            catch
            {

            }

            //OnChanged(this, null);
            if (LoggingService.IsInfoEnabled)
                LoggingService.Info("税费计算修改！");

        }


        /// <summary>
        /// 合计卖方或买方税费，只合计不计算
        /// </summary>
        /// <param name="aorb"></param>
        public void HjSf(string aorb)
        {
            HtmlDocument doc = htmlCalc.Document;
            double result = 0;
            foreach (string tag in tags)
            {
                HtmlElementCollection fields = doc.GetElementsByTagName(tag);

                foreach (HtmlElement field in fields)
                {
                    string fieldId = field.Id;

                    if (field.GetAttribute("type") != "hidden" && fieldId.EndsWith(aorb, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            if (LoggingService.IsDebugEnabled)
                                LoggingService.DebugFormatted("{0}:{1}", fieldId, field.GetAttribute("value"));
                            result += Convert.ToDouble(field.GetAttribute("value"));
                        }
                        catch
                        {
                        }
                    }
                }

            }

            try
            {
                HtmlElement he = doc.GetElementById(string.Format("sf_{0}count", aorb));
                SetHtmlElementValue(he, result.ToString("0.00"));
            }
            catch
            {

            }

            //OnChanged(this, null);
            if (LoggingService.IsInfoEnabled)
                LoggingService.Info("税费计算修改！");
        }

        private void m_btnSave_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }
        protected  void Save()
        {
            try
            {
               
                if (m_dtbTax.Rows.Count == 0) return;
                if (m_dtbTax.Rows[0].RowState == DataRowState.Unchanged)
                    m_dtbTax.Rows[0].SetModified();
                HtmlDocument doc = htmlCalc.Document;

                AttachValueToDataSource(m_dtbTax, doc);

                //因为事件的顺序并非与真实一样，所以
                //镇区初审包含窗口收件所有的数据.
                //if (this.tblData.SelectedTabPage == pInfo)
                //    doc = this.htmlZqsj.Document;
                //else
                //    doc = this.htmlZqcs.Document;
                //AttachValueToDataSource(m_dtbTax, doc);
                SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
                sqlDataEngine.SaveData(m_dstTax);
            }
            catch (Exception e)
            {
                LoggingService.Error(e);
            }
        }

        private void AttachValueToDataSource(DataTable dt, HtmlDocument doc)
        {
            foreach (string tag in tags)
            {
                HtmlElementCollection fields = doc.GetElementsByTagName(tag);

                foreach (HtmlElement field in fields)
                {
                    string column = field.GetAttribute("dbcolumn");
                    if (column != null && column.Length > 0)
                        if (dt.Columns.Contains(column))
                        {
                            string value = field.GetAttribute("value").Trim();
                            dt.Rows[0][column] = (value == string.Empty) ? Convert.DBNull : value;
                        }
                }
            }
        }
      
    }
}

