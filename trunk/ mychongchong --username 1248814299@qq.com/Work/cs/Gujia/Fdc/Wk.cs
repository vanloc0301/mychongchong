﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using SkyMap.Net.Core;
using SkyMap.Net.Gui;
using SkyMap.Net.DataForms;
using System.Collections;
using SkyMap.Net.Gui.Components;
using SkyMap.Net.DAO;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using SkyMap.Net.Workflow.Client.View;
using DevExpress.XtraEditors;
using SkyMap.Net.DataAccess;
using System.Reflection;
using Microsoft.Data.ConnectionUI;
using DevExpress.XtraGrid.Views.Base;
using System.Linq;
using System.Xml.Linq;
using Yogesh.ExcelXml;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using Taramon.Exceller;

using ZBPM.fj;
using ZBPM.yd;

using ExcelOper;
using System.Security.Cryptography;


namespace ZBPM
{
    public partial class Wk : WfAbstractDataForm
    {
        public Wk()
        {
            InitializeComponent();
            Init();
        }

        #region 全局变量
        private bool brun = false;
        private DataSet m_dstAll;

        private PrintSet m_printSet;
        private wk.wcexcel wexcel;
        private wk.wcckexcel wckexcel;
        private DataTable dt;
        private DataTable dtck;
        static DateTime beforeTime;            //Excel启动之前时间
        static DateTime afterTime;
        ExcelMapper mapper = new ExcelMapper();
        ExcelFromArrayList excelop = null;   //excel操作 样点导出功能时使用; 2010-08-11

        string excelFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\bin\wc\欠料明细.xls";
        string excelCkFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\bin\购料单\";
        string excelJgPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\bin\生成结果\";
        string ckpass = "123456";
        string strcolor = "10092543";
        #endregion

        private void Init()
        {

        }

        #region 对表单基类数据绑定事件的重写
        protected override void BeforeBindData()
        {
            base.BeforeBindData();
        }

        protected override void AfterBindData()
        {
            base.AfterBindData();
            m_dstAll = GetAllData();
            BBindData();
            //Thread t = new Thread(new ParameterizedThreadStart(this.GetExcelData));
            //t.Start("null");

            //this.txt_ProjectId.Text = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            //this.lbl_委托方.Text = base.GetControlBindValue(this.txt委托方).ToString();
            //this.lbl_业务来源.Text = base.GetControlBindValue(this.lue_业务来源).ToString();
            //SetControlState();

            //gridview_sf.ExpandAllGroups();

        }

        protected override void BeforeEndEdit()
        {
            base.BeforeEndEdit();
        }

        protected override void BeforeSave()
        {
            base.BeforeSave();
        }

        protected override void AfterSave()
        {
            base.AfterSave();
            SaveAllData();
        }

        protected override void SetControlDisable(Control ctr)
        {
            base.SetControlDisable(ctr);
        }

        public override void SetFormPermission(FormPermission formPermission, DAODataForm ddf, bool saveEnable)
        {
            base.SetFormPermission(formPermission, ddf, saveEnable);
        }

        protected override bool SelfBind(Control control, DataTable dataSource, string memberName, string[] valueCollection)
        {
            return base.SelfBind(control, dataSource, memberName, valueCollection);
        }
        #endregion

        private DataSet GetAllData()
        {
            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            m_dstAll = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[]{@"SELECT * 
FROM YW_bom where PROJECT_ID ='"+strProjectId+"' order by id asc","SELECT * FROM YW_wcexcel where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_bomexcel where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_ck where PROJECT_ID ='"+strProjectId+"' order by id asc","SELECT * FROM YW_wcckexcel where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_ckexcel where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_cktoday where PROJECT_ID ='"+strProjectId+"' order by id asc"}, new string[] { "YW_bom", "YW_wcexcel", "YW_bomexcel", "YW_ck", "YW_wcckexcel", "YW_ckexcel", "YW_cktoday" });
            if (m_dstAll != null && m_dstAll.Tables.Count != 0)
            {
                m_dstAll.Tables["YW_bom"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_bom where PROJECT_ID ='" + strProjectId + "' order by id asc");
                m_dstAll.Tables["YW_wcexcel"].ExtendedProperties.Add("selectsql", @"SELECT  *  FROM YW_wcexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_bomexcel"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_bomexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_ck"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_ck where PROJECT_ID ='" + strProjectId + "' order by id asc");
                m_dstAll.Tables["YW_wcckexcel"].ExtendedProperties.Add("selectsql", @"SELECT  *  FROM YW_wcckexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_ckexcel"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_ckexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_cktoday"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_cktoday where PROJECT_ID ='" + strProjectId + "' order by id asc");
            }
            m_dstAll.Tables["YW_bom"].TableNewRow += new DataTableNewRowEventHandler(Wk_BomNewRow);
            m_dstAll.Tables["YW_wcexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_bomexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_ck"].TableNewRow += new DataTableNewRowEventHandler(Wk_BomNewRow);
            m_dstAll.Tables["YW_wcckexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_ckexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_cktoday"].TableNewRow += new DataTableNewRowEventHandler(Wk_CkTodayNewRow);
            return m_dstAll;
        }
        private void SaveAllData()
        {
            this.gv_bom.PostEditor();
            this.BindingContext[this.gv_bom.DataSource].EndCurrentEdit();

            this.gv_bomexcel.PostEditor();
            this.BindingContext[this.gv_bomexcel.DataSource].EndCurrentEdit();

            this.gv_wcexcel.PostEditor();
            this.BindingContext[this.gv_wcexcel.DataSource].EndCurrentEdit();

            this.gv_ck.PostEditor();
            this.BindingContext[this.gv_ck.DataSource].EndCurrentEdit();

            this.gv_ckexcel.PostEditor();
            this.BindingContext[this.gv_ckexcel.DataSource].EndCurrentEdit();

            this.gv_wcckexcel.PostEditor();
            this.BindingContext[this.gv_wcckexcel.DataSource].EndCurrentEdit();

            //this.gv_cktotay.PostEditor();
            //this.BindingContext[this.gv_cktotay.DataSource].EndCurrentEdit();

            SMDataSource smDs = this.dataFormController.DAODataForm.DataSource;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_bom"]);
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_wcexcel"]);
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_bomexcel"]);
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_ck"]);
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_wcckexcel"]);
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_ckexcel"]);
            //sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_cktoday"]);
            sqlDataEngine.SaveData(smDs, m_dstAll);
            sqlDataEngine.RefreshDataset(smDs, m_dstAll);
        }
        private void BBindData()
        {
            gdc_bom.DataSource = m_dstAll;
            gdc_bom.DataMember = "yw_bom";

            gdc_wcexcel.DataSource = m_dstAll;
            gdc_wcexcel.DataMember = "yw_wcexcel";

            gdc_bomexcel.DataSource = m_dstAll;
            gdc_bomexcel.DataMember = "yw_bomexcel";

            gdc_ck.DataSource = m_dstAll;
            gdc_ck.DataMember = "yw_ck";

            gdc_ckexcel.DataSource = m_dstAll;
            gdc_ckexcel.DataMember = "yw_ckexcel";

            gdc_wcckexcel.DataSource = m_dstAll;
            gdc_wcckexcel.DataMember = "yw_wcckexcel";

            gdc_cktoday.DataSource = m_dstAll;
            gdc_cktoday.DataMember = "yw_cktoday";

        }

        void Wk_BomNewRow(object sender, DataTableNewRowEventArgs e)
        {
            int icount = m_dstAll.Tables["YW_bom"].Rows.Count + 1;
            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            e.Row["序号"] = icount;
            OnChanged(this, null);
        }

        void Wk_CkTodayNewRow(object sender, DataTableNewRowEventArgs e)
        {
            int icount = m_dstAll.Tables["YW_cktoday"].Rows.Count + 1;
            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            e.Row["序号"] = icount;
            OnChanged(this, null);
        }

        void Wk_ExcelNewRow(object sender, DataTableNewRowEventArgs e)
        {
            int icount = m_dstAll.Tables["YW_bom"].Rows.Count + 1;
            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            OnChanged(this, null);
        }
        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void ExcelControl(wk.wcexcel gh)
        {
            beforeTime = DateTime.Now;
            double tmpdb = 0;
            try
            {
                //mapper.Write(gh, excelFileName);
                mapper.Read(gh, excelFileName);
                mapper.Write(gh, @"c:\tmp.xls", excelFileName);
            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
            }
        }

        private void bt_导入excel数据_Click(object sender, EventArgs e)
        {
            base.Save();
            int inum = 1;
            DataTable wcexcel = m_dstAll.Tables["yw_wcexcel"];
            DataTable bomexcel = m_dstAll.Tables["yw_bomexcel"];

            if (wcexcel.Rows.Count == 1 && bomexcel.Rows.Count == 1)
            {
                if (wcexcel.Rows[0]["工作表"] == DBNull.Value)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
                if (wcexcel.Rows[0]["工作表"].ToString().Trim().Length == 0)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
                if (bomexcel.Rows[0]["工作表"] == DBNull.Value)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
                if (bomexcel.Rows[0]["工作表"].ToString().Trim().Length == 0)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
            }
            else
            {
                throw new Exception("两个Excel模板表记录数都必须只有1条记录！");
                return;
            }

            beforeTime = DateTime.Now;
            double tmpdb = 0;
            try
            {
                smGridControl1.DataSource = null;
                //mapper.Write(gh, excelFileName);
                wexcel = new wk.wcexcel();
                mapper.Read(wexcel, excelFileName, wcexcel, bomexcel);
                //mapper.Write(gh, @"c:\tmp.xls", excelFileName);
                //string strwc = string.Format("生产单号：{0}，厂款号：{1}，预备齐料期：{2}", wexcel.Scdh.ToString(), wexcel.Ckh.ToString(), wexcel.Qlq.ToString());
                string strwc = string.Format("生产单号：{0}，预备齐料期：{1}", wexcel.Scdh.ToString(), wexcel.Qlq.ToString());
                lblWcMsg.Text = strwc;
                dt = new DataTable("bomexcel");
                dt.Columns.Add("序号", System.Type.GetType("System.Int32"));
                dt.Columns.Add("物料名称", System.Type.GetType("System.String"));
                dt.Columns.Add("颜色", System.Type.GetType("System.String"));
                dt.Columns.Add("总用量", System.Type.GetType("System.String"));
                dt.Columns.Add("单位", System.Type.GetType("System.String"));
                dt.Columns.Add("供应商", System.Type.GetType("System.String"));
                dt.Columns.Add("来料数量", System.Type.GetType("System.String"));
                dt.Columns.Add("来料日期", System.Type.GetType("System.String"));
                dt.Columns.Add("采购复期", System.Type.GetType("System.String"));
                dt.Columns.Add("采购备注", System.Type.GetType("System.String"));
                for (int i = 0; i < wexcel.Xh.Count; i++)
                {
                    if (string.IsNullOrEmpty(wexcel.Wlmc[i].ToString())) continue;
                    DataRow dr = dt.NewRow();
                    dr["序号"] = inum; //wexcel.Xh[i].ToString();
                    dr["物料名称"] = wexcel.Wlmc[i].ToString().Split(new char[] { '@' })[0];
                    dr["颜色"] = wexcel.Ys[i].ToString().Split(new char[] { '@' })[0];
                    dr["总用量"] = wexcel.Zrl[i].ToString().Split(new char[] { '@' })[0];
                    dr["单位"] = wexcel.Dw[i].ToString().Split(new char[] { '@' })[0];
                    dr["供应商"] = wexcel.Gys[i].ToString().Split(new char[] { '@' })[0];
                    dr["来料数量"] = wexcel.Dhsl[i].ToString().Split(new char[] { '@' })[0];
                    dr["来料日期"] = wexcel.Dhrq[i].ToString().Split(new char[] { '@' })[0];
                    dr["采购复期"] = wexcel.Cgfq[i].ToString().Split(new char[] { '@' })[0];
                    dr["采购备注"] = "";// wexcel.Cgbz[i].ToString();
                    dt.Rows.Add(dr);
                    inum++;
                }
                smGridControl1.DataSource = dt;
                bt_读Execl写进数据库.Visible = true;

            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
            }
        }

        private void bt_读Execl写进数据库_Click(object sender, EventArgs e)
        {
            base.Save();
            Hashtable ht = new Hashtable();
            if (wexcel != null && dt != null && dt.Rows.Count > 0)
            {
                //this.txt_厂款号.Text = wexcel.Ckh.ToString();
                if (string.IsNullOrEmpty(txt_生产单号.Text.ToString()))
                {
                    txt_生产单号.Text = wexcel.Scdh.ToString();
                }
                DataTable dtbom = m_dstAll.Tables["yw_bom"];
                DataView dwbom = dtbom.DefaultView;
                if (dtbom.Rows.Count > 0)
                {
                    MessageBox.Show("已有物料清单,Excel中读取的数据将追加到现有的数据中！", "提醒：");
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //=====================
                    if (string.IsNullOrEmpty(dt.Rows[i]["物料名称"].ToString().Trim())) continue;
                    if (string.IsNullOrEmpty(dt.Rows[i]["总用量"].ToString().Trim())) continue;
                    dwbom.RowFilter = string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dt.Rows[i]["物料名称"].ToString()), DvRowFilter(dt.Rows[i]["颜色"].ToString()), DvRowFilter(dt.Rows[i]["总用量"].ToString()));
                    if (dwbom.Count == 1)
                    {
                        string strfilter = string.Format("序号={3}|物料名称='{0}'|颜色='{1}'|总用量='{2}'", DvRowFilter(dt.Rows[i]["物料名称"].ToString()), DvRowFilter(dt.Rows[i]["颜色"].ToString()), DvRowFilter(dt.Rows[i]["总用量"].ToString()), DvRowFilter(dwbom[0]["序号"].ToString()));
                        if (!ht.Contains(strfilter))
                        {
                            ht.Add(strfilter, 1);
                        }
                        else
                        {
                            ht[strfilter] = int.Parse(ht[strfilter].ToString()) + 1;
                        }
                        //dwbom[0]["单位"] = dtck.Rows[i]["单位"].ToString();
                        //dwbom[0]["供应商"] = dtck.Rows[i]["供应商"].ToString();
                        //dwbom[0]["收货数量"] = dtck.Rows[i]["来料数量"].ToString();
                        //dwbom[0]["收货日期"] = dtck.Rows[i]["来料日期"].ToString();
                        dwbom[0]["采购复期"] = dt.Rows[i]["采购复期"].ToString();
                    }
                    else if (dwbom.Count == 0)
                    {
                        DataRow dr = dtbom.NewRow();
                        dr["序号"] = dt.Rows[i]["序号"].ToString();
                        dr["物料名称"] = dt.Rows[i]["物料名称"].ToString();
                        dr["颜色"] = dt.Rows[i]["颜色"].ToString();
                        dr["总用量"] = dt.Rows[i]["总用量"].ToString();
                        dr["单位"] = dt.Rows[i]["单位"].ToString();
                        dr["供应商"] = dt.Rows[i]["供应商"].ToString();
                        dr["收货数量"] = dt.Rows[i]["来料数量"].ToString();
                        dr["收货日期"] = dt.Rows[i]["来料日期"].ToString();
                        dr["采购复期"] = dt.Rows[i]["采购复期"].ToString();
                        //dr["采购备注"] = dt.Rows[i]["采购备注"].ToString();
                        dtbom.Rows.Add(dr);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("存在相同的记录,物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString())));
                        break;
                    }

                    //====================

                }

            }
            base.Save();

        }

        private void bt_模板设置_Click(object sender, EventArgs e)
        {

        }



        private void AutoSetBomExcelModel(DataTable tmpdt, string colname, int istart, int iend)
        {
            string[] xh = tmpdt.Rows[0][colname].ToString().Split(new char[] { '|' });
            if (xh.Length == 1 || xh.Length == 2)
            {
                try
                {
                    List<int> li = ExcelColumnTranslator.showMatches(xh[0].ToString());
                    tmpdt.Rows[0][colname] = string.Format("{0}{1}|{2}{3}", ExcelColumnTranslator.ToName(li[1]), istart, ExcelColumnTranslator.ToName(li[1]), iend);
                }
                catch
                {
                    if (xh[0].Trim().Length == 0)
                    {
                        MessageBox.Show(string.Format("数据为空！请设置正确的【{0}】,格式形如:A1或A1|A12", colname));
                        return;
                    }
                    tmpdt.Rows[0][colname] = string.Format("{0}{1}|{2}{3}", xh[0], istart, xh[0], iend);
                }

            }
            else
            {
                MessageBox.Show(string.Format("请设置正确的【{0}】,格式形如:A1或A1|A12", colname));
                return;
            }
        }

        private void bt_find_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in m_dstAll.Tables["yw_wcexcel"].Rows)
            {
                dr.Delete();
            }
            foreach (DataRow dr in m_dstAll.Tables["yw_bomexcel"].Rows)
            {
                dr.Delete();
            }

            base.Save();
            if (cbe_工作表.Text.ToString() == "")
            {
                MessageBox.Show("请先获取工作表！", "提示");
                return;
            }
            this.txtSearch1.Text = string.Concat(cbe_工作表.Text.ToString(), this.txtSearch1.Text.Substring(this.txtSearch1.Text.ToString().IndexOf("|")));
            this.txtSearch2.Text = string.Concat(cbe_工作表.Text.ToString(), this.txtSearch2.Text.Substring(this.txtSearch2.Text.ToString().IndexOf("|")));
            if (this.txtSearch1.Text.ToString().Split(new char[] { '|' })[0] != this.txtSearch2.Text.ToString().Split(new char[] { '|' })[0])
            {
                MessageBox.Show("工作表设置需要一样!", "提示");
                return;
            }
            AutoSet(this.txtSearch1.Text.ToString(), m_dstAll.Tables["yw_wcexcel"], 0, this.excelFileName);
            AutoSet(this.txtSearch2.Text.ToString(), m_dstAll.Tables["yw_bomexcel"], 1, this.excelFileName);

        }

        private void AutoSet(string strtxt, DataTable dt, int inti, string excelfilename)
        {
            bool bflag = false;//当为true的时候自动运行 模板设置事件
            string[] str = strtxt.Split(new char[] { '|' });
            Hashtable ht = new Hashtable();
            ht.Add("工作表", str[0].ToString());
            for (int i = 1; i < str.Length; i++)
            {
                string strreturn = "";
                string strname = "";
                strname = str[i].Split(new char[] { '#' })[0].ToString();

                if (str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' }).Length == 1)
                {
                    strreturn = search(str[0].ToString(), str[i].Split(new char[] { '#' })[1].ToString(), inti, excelfilename, "");
                    if (strreturn != "")
                    {
                        ht.Add(strname, strreturn);
                    }
                }
                else
                {
                    for (int j = 0; j < str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' }).Length; j++)
                    {
                        strreturn = search(str[0].ToString(), str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' })[j].ToString(), inti, excelfilename, "");
                        if (strreturn != "")
                        {
                            ht.Add(strname, strreturn);
                            break;
                        }
                    }
                }
            }
            DataTable tmpdt = dt;
            DataRow dr = null;
            if (tmpdt.Rows.Count == 1)
            {
                dr = tmpdt.Rows[0];
            }
            else if (tmpdt.Rows.Count == 0)
            {
                dr = tmpdt.NewRow();
            }
            else
            {
                MessageBox.Show("模板表中只能允许1条记录");
                return;
            }
            foreach (DataColumn dc in tmpdt.Columns)
            {
                if (ht.Contains(dc.ColumnName))
                {
                    if (dc.ColumnName == "序号" && ht.Contains("预计齐料期") && ht.Contains("序号"))
                    {
                        bflag = true;
                        List<int> li1 = ExcelColumnTranslator.showMatches(ht["预计齐料期"].ToString());
                        List<int> li2 = ExcelColumnTranslator.showMatches(ht["序号"].ToString());
                        dr[dc.ColumnName] = string.Format("{0}|{1}{2}", ht[dc.ColumnName].ToString(), ExcelColumnTranslator.ToName(li2[1]), li1[0] - 1);
                    }
                    else
                    {
                        dr[dc.ColumnName] = ht[dc.ColumnName].ToString();
                    }
                }
            }
            if (tmpdt.Rows.Count == 0) tmpdt.Rows.Add(dr);
            if (bflag)
            {
                AutoSetRange(dt);
            }
            #region
            #endregion
        }

        private void AutoSetRange(DataTable tmpdt)
        {
            if (tmpdt.Rows.Count == 1)
            {
                string[] xh = tmpdt.Rows[0]["序号"].ToString().Split(new char[] { '|' });
                if (xh.Length == 2)
                {
                    List<int> li = ExcelColumnTranslator.showMatches(xh[0].ToString());
                    int istart = 0;
                    int iend = 0;
                    istart = li[0] + 1;
                    li = ExcelColumnTranslator.showMatches(xh[1].ToString());
                    iend = li[0] + 1;
                    #region 智能设置
                    AutoSetBomExcelModel(tmpdt, "物料名称", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "颜色", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "总用量", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "单位", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "供应商", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "来料数量", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "来料日期", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "采购复期", istart, iend);
                    //AutoSetBomExcelModel(tmpdt, "采购备注", istart, iend);
                    tmpdt.Rows[0]["记录数"] = (iend - istart + 1).ToString();
                    #endregion
                }
                else
                {
                    MessageBox.Show("请设置正确的【序号格式】,格式形如:A1|A12");
                    return;
                }

            }
            base.Save();
        }

        private string search(string sheetname, string strKeyWord, int inti, string excelfilename, string pass)
        {
            beforeTime = DateTime.Now;

            object filename = excelfilename;

            object MissingValue = Type.Missing;

            try
            {
                Microsoft.Office.Interop.Excel.Application ep = new Microsoft.Office.Interop.Excel.ApplicationClass();

                Microsoft.Office.Interop.Excel.Workbook ew = ep.Workbooks.Open(filename.ToString(), MissingValue,

                 MissingValue, MissingValue, pass,

                 pass, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue);

                Microsoft.Office.Interop.Excel.Worksheet ews;

                int iEWSCnt = ew.Worksheets.Count;

                int i = 0, j = 0;

                Microsoft.Office.Interop.Excel.Range oRange;

                object oText = strKeyWord.Trim().ToUpper();
                //for (i = 1; i <= iEWSCnt; i++)  {
                ews = null;

                ews = (Microsoft.Office.Interop.Excel.Worksheet)ew.Worksheets[sheetname];

                oRange = null;

                //=============inti=1时，表示要获取list;
                //   if (inti == 1)
                //   {
                //       oRange = ((Microsoft.Office.Interop.Excel.Range)ews.UsedRange).Find(

                //"序", MissingValue, MissingValue,

                //MissingValue, MissingValue, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext,

                //MissingValue, MissingValue, MissingValue);

                //       if (oRange != null && oRange.Cells.Rows.Count >= 1 && oRange.Cells.Columns.Count >= 1)
                //       {
                //===============
                oRange = ((Microsoft.Office.Interop.Excel.Range)ews.UsedRange).Find(

                oText, MissingValue, MissingValue,

                MissingValue, MissingValue, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext,

                MissingValue, MissingValue, MissingValue);

                if (oRange != null && oRange.Cells.Rows.Count >= 1 && oRange.Cells.Columns.Count >= 1)
                {
                    return string.Format("{0}{1}", ExcelColumnTranslator.ToName(int.Parse(oRange.Column.ToString()) - 1), int.Parse(oRange.Row.ToString()) + inti);
                }
                //    }
                //}


                return "";
                //} 
            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
            }

        }

        private void bt_GetWorkSheet_Click(object sender, EventArgs e)
        {
            beforeTime = DateTime.Now;

            object filename = excelFileName;

            object MissingValue = Type.Missing;

            try
            {
                Microsoft.Office.Interop.Excel.Application ep = new Microsoft.Office.Interop.Excel.ApplicationClass();

                Microsoft.Office.Interop.Excel.Workbook ew = ep.Workbooks.Open(filename.ToString(), MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue);

                Microsoft.Office.Interop.Excel.Worksheet ews;

                int iEWSCnt = ew.Worksheets.Count;

                int i = 0, j = 0;

                Microsoft.Office.Interop.Excel.Range oRange;
                cbe_工作表.Properties.Items.Clear();
                for (i = 1; i <= iEWSCnt; i++)
                {
                    ews = null;
                    ews = (Microsoft.Office.Interop.Excel.Worksheet)ew.Worksheets[i];

                    cbe_工作表.Properties.Items.Add(ews.Name.ToString());
                }
            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
                bt_GetWorkSheet.Enabled = false;
            }
        }

        private void cbe_文件ck_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bt_GetCkFile_Click(object sender, EventArgs e)
        {
            cbe_文件ck.Properties.Items.Clear();
            string[] fileNames = Directory.GetFiles(excelCkFilePath);
            for (int i = 0; i < fileNames.Length; i++)
            {
                string[] strArray2 = fileNames[i].Split(new char[] { '\\' });
                //string path = excelCkFilePath + @"\" + strArray2[strArray2.Length - 1];
                //if (System.IO.File.Exists(path))
                //{
                cbe_文件ck.Properties.Items.Add(strArray2[strArray2.Length - 1]);
                //}
                //string[] directories = Directory.GetDirectories(path);
                //foreach (string file in fileNames)
                //{

                //}
            }

        }

        private void bt_GetWorkSheetck_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbe_文件ck.Text.ToString())) return;
            beforeTime = DateTime.Now;
            object filename = string.Format("{0}{1}", this.excelCkFilePath, cbe_文件ck.Text.ToString());

            object MissingValue = Type.Missing;

            try
            {
                Microsoft.Office.Interop.Excel.Application ep = new Microsoft.Office.Interop.Excel.ApplicationClass();

                Microsoft.Office.Interop.Excel.Workbook ew = ep.Workbooks.Open(filename.ToString(), MissingValue,

                 MissingValue, MissingValue, ckpass,

                ckpass, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue, MissingValue, MissingValue,

                 MissingValue);

                Microsoft.Office.Interop.Excel.Worksheet ews;

                int iEWSCnt = ew.Worksheets.Count;

                int i = 0, j = 0;

                Microsoft.Office.Interop.Excel.Range oRange;
                cbe_工作表ck.Properties.Items.Clear();
                for (i = 1; i <= iEWSCnt; i++)
                {
                    ews = null;
                    ews = (Microsoft.Office.Interop.Excel.Worksheet)ew.Worksheets[i];

                    cbe_工作表ck.Properties.Items.Add(ews.Name.ToString());
                }
            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
                bt_GetWorkSheetck.Enabled = false;
            }
        }

        private void bt_findck_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbe_文件ck.Text.ToString())) return;
            string filename = string.Format("{0}{1}", this.excelCkFilePath, cbe_文件ck.Text.ToString());

            foreach (DataRow dr in m_dstAll.Tables["yw_wcckexcel"].Rows)
            {
                dr.Delete();
            }
            foreach (DataRow dr in m_dstAll.Tables["yw_ckexcel"].Rows)
            {
                dr.Delete();
            }

            base.Save();
            if (cbe_工作表ck.Text.ToString() == "")
            {
                MessageBox.Show("请先获取工作表！", "提示");
                return;
            }
            this.txtSearch1ck.Text = string.Concat(cbe_工作表ck.Text.ToString(), this.txtSearch1ck.Text.Substring(this.txtSearch1ck.Text.ToString().IndexOf("|")));
            this.txtSearch2ck.Text = string.Concat(cbe_工作表ck.Text.ToString(), this.txtSearch2ck.Text.Substring(this.txtSearch2ck.Text.ToString().IndexOf("|")));
            if (this.txtSearch1ck.Text.ToString().Split(new char[] { '|' })[0] != this.txtSearch2ck.Text.ToString().Split(new char[] { '|' })[0])
            {
                MessageBox.Show("工作表设置需要一样!", "提示");
                return;
            }
            AutoSetCk(this.txtSearch1ck.Text.ToString(), m_dstAll.Tables["yw_wcckexcel"], 0, filename);
            AutoSetCk(this.txtSearch2ck.Text.ToString(), m_dstAll.Tables["yw_ckexcel"], 1, filename);
        }

        private void AutoSetCk(string strtxt, DataTable dt, int inti, string excelfilename)
        {
            bool bflag = false;//当为true的时候自动运行 模板设置事件
            string[] str = strtxt.Split(new char[] { '|' });
            Hashtable ht = new Hashtable();
            ht.Add("工作表", str[0].ToString());
            for (int i = 1; i < str.Length; i++)
            {
                string strreturn = "";
                string strname = "";
                strname = str[i].Split(new char[] { '#' })[0].ToString();

                if (str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' }).Length == 1)
                {
                    strreturn = search(str[0].ToString(), str[i].Split(new char[] { '#' })[1].ToString(), inti, excelfilename, ckpass);
                    if (strreturn != "")
                    {
                        ht.Add(strname, strreturn);
                    }
                }
                else
                {
                    for (int j = 0; j < str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' }).Length; j++)
                    {
                        strreturn = search(str[0].ToString(), str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' })[j].ToString(), inti, excelfilename, ckpass);
                        if (strreturn != "")
                        {
                            ht.Add(strname, strreturn);
                            break;
                        }
                    }
                }
            }
            DataTable tmpdt = dt;
            DataRow dr = null;
            if (tmpdt.Rows.Count == 1)
            {
                dr = tmpdt.Rows[0];
            }
            else if (tmpdt.Rows.Count == 0)
            {
                dr = tmpdt.NewRow();
            }
            else
            {
                MessageBox.Show("模板表中只能允许1条记录");
                return;
            }
            #region 购料单没有记录结束标志，就手动设置一个
            if (!ht.Contains("预计齐料期"))
            {
                ht.Add("预计齐料期", "A300");
            }
            #endregion
            foreach (DataColumn dc in tmpdt.Columns)
            {
                if (ht.Contains(dc.ColumnName))
                {
                    if (dc.ColumnName == "序号" && ht.Contains("预计齐料期") && ht.Contains("序号"))
                    {
                        bflag = true;
                        List<int> li1 = ExcelColumnTranslator.showMatches(ht["预计齐料期"].ToString());
                        List<int> li2 = ExcelColumnTranslator.showMatches(ht["序号"].ToString());
                        dr[dc.ColumnName] = string.Format("{0}|{1}{2}", ht[dc.ColumnName].ToString(), ExcelColumnTranslator.ToName(li2[1]), li1[0] - 1);
                    }
                    else
                    {
                        dr[dc.ColumnName] = ht[dc.ColumnName].ToString();
                    }
                }
            }
            if (tmpdt.Rows.Count == 0) tmpdt.Rows.Add(dr);
            if (bflag)
            {
                AutoSetRangeCk(dt);
            }
            #region
            #endregion
        }

        private void AutoSetRangeCk(DataTable tmpdt)
        {
            if (tmpdt.Rows.Count == 1)
            {
                string[] xh = tmpdt.Rows[0]["序号"].ToString().Split(new char[] { '|' });
                if (xh.Length == 2)
                {
                    List<int> li = ExcelColumnTranslator.showMatches(xh[0].ToString());
                    int istart = 0;
                    int iend = 0;
                    istart = li[0] + 1;
                    li = ExcelColumnTranslator.showMatches(xh[1].ToString());
                    iend = li[0] + 1;
                    #region 智能设置
                    AutoSetBomExcelModel(tmpdt, "物料名称", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "颜色", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "总用量", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "单位", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "供应商", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "来料数量", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "来料日期", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "配色", istart, iend);
                    //AutoSetBomExcelModel(tmpdt, "采购备注", istart, iend);
                    tmpdt.Rows[0]["记录数"] = (iend - istart + 1).ToString();
                    #endregion
                }
                else
                {
                    MessageBox.Show("请设置正确的【序号格式】,格式形如:A1|A12");
                    return;
                }

            }
            base.Save();
        }

        private void bt_导入excel数据ck_Click(object sender, EventArgs e)
        {
            //System.Action ac = null;
            //ac = GetExcelData;
            //ac();
            //this.Save();

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetExcelDataGc));

            //Thread t = new Thread(new ParameterizedThreadStart(this.GetExcelDataGc));
            //t.Start("null");
        }


        private void GetExcelDataGc(object o)
        {
            GetExcelData(o);
            System.GC.Collect();
        }
        private void GetExcelData(object o)
        {
            string strhash = "";
            string filename = string.Format("{0}{1}", this.excelCkFilePath, base.GetControlBindValue(this.cbe_文件ck).ToString());

            try
            {
                strhash = getFilesMD5Hash(filename);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (strhash == base.GetControlBindValue(this.txt_hash).ToString()) return;

            this.Invoke(new System.Action(delegate()
            {
                this.Save();
            }));


            int inum = 1;
            if (string.IsNullOrEmpty(base.GetControlBindValue(this.cbe_文件ck).ToString())) return;
            if (string.IsNullOrEmpty(base.GetControlBindValue(this.cbe_工作表ck).ToString())) return;
            DataTable wcexcel = m_dstAll.Tables["yw_wcckexcel"];
            DataTable bomexcel = m_dstAll.Tables["yw_ckexcel"];

            if (wcexcel.Rows.Count == 1 && bomexcel.Rows.Count == 1)
            {
                if (wcexcel.Rows[0]["工作表"] == DBNull.Value)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
                if (wcexcel.Rows[0]["工作表"].ToString().Trim().Length == 0)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
                if (bomexcel.Rows[0]["工作表"] == DBNull.Value)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
                if (bomexcel.Rows[0]["工作表"].ToString().Trim().Length == 0)
                {
                    MessageBox.Show("模板表中工作表字段不能为空");
                    return;
                }
            }
            else
            {
                MessageBox.Show("模板表记录数都必须有且仅有1条记录！");
                return;
            }

            beforeTime = DateTime.Now;
            double tmpdb = 0;
            try
            {
                this.Invoke(new System.Action(delegate()
                {
                    WaitDialogHelper.Show();
                    this.gv_cktotay.ClearColumnsFilter();
                    gdc_cktoday.DataSource = null;
                    bt_导入excel数据ck.Visible = false;
                    bt_读Execl写进数据库ck.Visible = false;
                }));


                //mapper.Write(gh, excelFileName);

                //--------
                NewMethod(bomexcel, filename);
                //--------
                wckexcel = new wk.wcckexcel();
                mapper.Read(wckexcel, filename, wcexcel, bomexcel, ckpass);
                //mapper.Write(gh, @"c:\tmp.xls", excelFileName);
                //string strwc = string.Format("生产单号：{0}，厂款号：{1}，预备齐料期：{2}", wexcel.Scdh.ToString(), wexcel.Ckh.ToString(), wexcel.Qlq.ToString());
                string strwc = string.Format("生产单号：{0}", wckexcel.Scdh.ToString());
                lblWcMsg.Text = strwc;
                CreateTmpCkTodayTable();
                int bext = 0;

                double dzrl = 0d, ddhsl = 0d;

                try
                {
                    DataView dwywck = m_dstAll.Tables["yw_ck"].DefaultView;
                    for (int i = 0; i < wckexcel.Xh.Count; i++)
                    {
                        string[] wlmc = null;
                        string[] ys = null;
                        string[] zrl = null;
                        string[] dw = null;
                        string[] gys = null, dhsl = null, dhrq = null, ps = null;

                        if (string.IsNullOrEmpty(wckexcel.Wlmc[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Ys[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Zrl[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Dw[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Gys[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Dhsl[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Dhrq[i].ToString().Split(new char[] { '@' })[0]) &&
                            string.IsNullOrEmpty(wckexcel.Ps[i].ToString().Split(new char[] { '@' })[0]))
                        {
                            bext++;
                            if (bext > 31) break;  //如果连续6行没有数据则认为到达末尾
                            continue;
                        }
                        bext = 0;
                        wlmc = wckexcel.Wlmc[i].ToString().Split(new char[] { '@' });
                        ys = wckexcel.Ys[i].ToString().Split(new char[] { '@' });
                        zrl = wckexcel.Zrl[i].ToString().Split(new char[] { '@' });
                        dw = wckexcel.Dw[i].ToString().Split(new char[] { '@' });
                        gys = wckexcel.Gys[i].ToString().Split(new char[] { '@' });
                        dhsl = wckexcel.Dhsl[i].ToString().Split(new char[] { '@' });
                        dhrq = wckexcel.Dhrq[i].ToString().Split(new char[] { '@' });
                        ps = wckexcel.Ps[i].ToString().Split(new char[] { '@' });
                        DataRow dr = dtck.NewRow();
                        dr["序号"] = inum; //wexcel.Xh[i].ToString();
                        dr["物料名称"] = wlmc[0];
                        dr["颜色"] = ys[0];
                        dr["总用量"] = zrl[0];
                        dr["单位"] = dw[0];
                        dr["供应商"] = gys[0];
                        dr["来料数量"] = dhsl[0];
                        dr["来料日期"] = dhrq[0];
                        dr["配色"] = ps[0];
                        dr["标注"] = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", wlmc[1], ys[1], zrl[1], dw[1], gys[1], dhsl[1], dhrq[1], ps[1]);
                        if (wlmc[2] == strcolor && ys[2] == strcolor && zrl[2] == strcolor && dw[2] == strcolor && gys[2] == strcolor && dhsl[2] == strcolor && dhrq[2] == strcolor && ps[2] == strcolor)
                        {
                            dr["是否标色"] = 0;
                        }
                        if (double.TryParse(zrl[0], out dzrl))
                        {
                            if (double.TryParse(dhsl[0], out ddhsl))
                            {
                                if (ddhsl - dzrl >= 0)
                                    dr["是否标色"] = 1;
                            }
                        }
                        if (!string.IsNullOrEmpty(wlmc[0].ToString()))
                        {
                            dwywck = new DataView(m_dstAll.Tables["yw_ck"]);
                            dwywck.RowFilter = string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}' and 是否审核=1", DvRowFilter(wlmc[0].ToString()), DvRowFilter(ys[0].ToString()), DvRowFilter(zrl[0].ToString()));
                            if (dwywck.Count > 0)
                            {
                                //dr["是否审核"] = 1;
                                //dr["是否标色"] = 1;
                            }
                            else
                            {
                                dr["是否审核"] = 0;
                            }
                        }
                        else
                        {
                            dr["是否审核"] = 0;
                        }

                        dtck.Rows.Add(dr);
                        inum++;
                    }
                }
                catch
                {
                    throw new ExcelException("private void GetExcelData(object o)函数运行抛出异常");
                }
                finally
                {
                    try
                    {
                        this.Invoke(new System.Action(delegate()
                        {
                            try
                            {
                                DataTable dtcktoday = m_dstAll.Tables["yw_cktoday"];
                                foreach (DataRow dr in dtcktoday.Rows)
                                {
                                    dr.Delete();
                                }
                                int icktoday, ick;
                                foreach (DataRow dtcktodaydr in dtck.Rows)
                                {
                                    icktoday = 0;
                                    ick = 0;
                                    DataRow dr = dtcktoday.NewRow();
                                    dr["序号"] = dtcktodaydr["序号"];
                                    dr["物料名称"] = dtcktodaydr["物料名称"];
                                    dr["颜色"] = dtcktodaydr["颜色"];
                                    dr["总用量"] = dtcktodaydr["总用量"];
                                    dr["单位"] = dtcktodaydr["单位"];
                                    dr["供应商"] = dtcktodaydr["供应商"];
                                    dr["来料数量"] = dtcktodaydr["来料数量"];
                                    dr["来料日期"] = dtcktodaydr["来料日期"];
                                    dr["配色"] = dtcktodaydr["配色"];
                                    dr["标注"] = dtcktodaydr["标注"];
                                    //===============做标记看是否审核，对于含重复记录的不作判断，以后着色的时候，用了特别的颜色标示重复的记录;

                                    if ((icktoday = RowCount(dtck.DefaultView, dtcktodaydr["物料名称"], dtcktodaydr["颜色"], dtcktodaydr["总用量"])) >= (ick = RowCount(dwywck = new DataView(m_dstAll.Tables["yw_ck"]),dtcktodaydr["物料名称"], dtcktodaydr["颜色"], dtcktodaydr["总用量"])))
                                    {
                                        if (icktoday == 1 && ick == 1)
                                            dr["是否审核"] = 1;
                                        else
                                            dr["是否审核"] = 0;
                                    }
                                    else
                                    {
                                        MessageBox.Show(string.Format("物料表中有{0}条，物料名称为:{1},颜色为{2},总用量为{3}的记录，\r\n但是在已审表中发现有{1}条,\r\n请根据实际情况解决此不合法的规则[已审表的相同物料的记录数>物料表的记录数",icktoday,dtcktodaydr["物料名称"], dtcktodaydr["颜色"], dtcktodaydr["总用量"],ick),"警告");
                                    }
                                    //===============
                                    
                                    dr["是否标色"] = dtcktodaydr["是否标色"];
                                    dtcktoday.Rows.Add(dr);
                                }
                           
                                this.txt_hash.Text = strhash;
                                this.Save();
                            }
                            finally
                            {
                                gdc_cktoday.DataSource = m_dstAll;
                                gdc_cktoday.DataMember = "yw_cktoday";
                                gv_cktotay.RefreshData();
                                bt_读Execl写进数据库ck.Visible = true;
                                bt_导入excel数据ck.Visible = true;
                                WaitDialogHelper.Close();
                            }
                        }));
                    }
                    catch//ExcelException e)
                    {
                        //  MessageBox.Show(e.Message.ToString());
                    }
                }

            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
            }
        }

        private void CreateTmpCkTodayTable()
        {
            dtck = new DataTable("ckexcel");
            dtck.Columns.Add("序号", System.Type.GetType("System.Int32"));
            dtck.Columns.Add("物料名称", System.Type.GetType("System.String"));
            dtck.Columns.Add("颜色", System.Type.GetType("System.String"));
            dtck.Columns.Add("配色", System.Type.GetType("System.String"));
            dtck.Columns.Add("总用量", System.Type.GetType("System.String"));
            dtck.Columns.Add("单位", System.Type.GetType("System.String"));
            dtck.Columns.Add("供应商", System.Type.GetType("System.String"));
            dtck.Columns.Add("来料数量", System.Type.GetType("System.String"));
            dtck.Columns.Add("来料日期", System.Type.GetType("System.String"));
            dtck.Columns.Add("标注", System.Type.GetType("System.String"));
            dtck.Columns.Add("是否标色", System.Type.GetType("System.Boolean"));
            dtck.Columns.Add("是否审核", System.Type.GetType("System.Boolean"));
        }

        /// <summary>
        /// 根据物料名称来判断excel中要读取的记录数，自动智能设置模板
        /// </summary>
        /// <param name="bomexcel"></param>
        /// <param name="filename"></param>
        private void NewMethod(DataTable bomexcel, string filename)
        {
            wk.TestWlmc twlmc = new ZBPM.wk.TestWlmc();
            AutoSetTestWlmc(bomexcel, 10000);
            int iwlmccount = GetWlmcCount(bomexcel, filename, twlmc);
            AutoSetTestWlmc(bomexcel, iwlmccount);

        }


        private void AutoSetTestWlmc(DataTable tmpdt, int iend)
        {
            if (tmpdt.Rows.Count == 1)
            {
                string[] xh = tmpdt.Rows[0]["序号"].ToString().Split(new char[] { '|' });
                if (xh.Length == 2)
                {
                    List<int> li = ExcelColumnTranslator.showMatches(xh[0].ToString());
                    int istart = 0;
                    istart = li[0] + 1;
                    //li = ExcelColumnTranslator.showMatches(xh[1].ToString());
                    //iend = li[0] + 1;
                    #region 智能设置
                    AutoSetBomExcelModel(tmpdt, "序号", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "物料名称", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "颜色", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "总用量", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "单位", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "供应商", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "来料数量", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "来料日期", istart, iend);
                    AutoSetBomExcelModel(tmpdt, "配色", istart, iend);
                    //AutoSetBomExcelModel(tmpdt, "采购备注", istart, iend);
                    tmpdt.Rows[0]["记录数"] = (iend - istart + 1).ToString();
                    #endregion
                }
                else
                {
                    MessageBox.Show("请设置正确的【序号格式】,格式形如:A1|A12");
                    return;
                }

            }
        }
        /// <summary>
        /// 获得物料数目，准确控制读取excel记录数
        /// </summary>
        /// <param name="bomexcel"></param>
        /// <param name="filename"></param>
        /// <param name="twlmc"></param>
        /// <returns></returns>
        private int GetWlmcCount(DataTable bomexcel, string filename, wk.TestWlmc twlmc)
        {
            ArrayList alwlmc = mapper.Read(twlmc, filename, bomexcel, ckpass);
            //int bext = 0;
            //int btotal = 0;
            //for (int i = 0; i < alwlmc.Count; i++)
            //{
            //    if (String.IsNullOrEmpty(alwlmc[i].ToString().Split(new char[]{'@'})[0].ToString()))
            //    {
            //        bext++;
            //        if (bext >= 10) break;  //如果连续6行没有数据则认为到达末尾
            //    }
            //    btotal++;
            //    bext = 0;
            //}
            return int.Parse(alwlmc[0].ToString()) + 1;
        }

        private void smGridView4_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //e.Column.VisibleIndex % 2 == 0 && e.RowHandle % 2 == 1))
            try
            {
                DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).GetDataRow(e.RowHandle);
                String tmp = dr["是否标色"].ToString();
                String tmp1 = dr["是否审核"].ToString();
                string tmpwlmc = dr["物料名称"].ToString();
                //if (e.RowHandle != smGridView4.FocusedRowHandle)
                //{
                if (!string.IsNullOrEmpty(tmp) && tmp == "True" && !string.IsNullOrEmpty(tmp1) && tmp1 == "True" && !string.IsNullOrEmpty(tmpwlmc))
                {
                    e.Appearance.BackColor = Color.SkyBlue;
                    e.Appearance.BackColor2 = Color.SkyBlue;
                    return;
                }
                if (!string.IsNullOrEmpty(tmp) && tmp == "True" && ((!string.IsNullOrEmpty(tmp1) && tmp1 == "False") || (string.IsNullOrEmpty(tmp1))) && !string.IsNullOrEmpty(tmpwlmc))
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.BackColor2 = Color.Orange;
                    return;
                }
                if (!string.IsNullOrEmpty(tmp) && tmp == "True" && string.IsNullOrEmpty(tmpwlmc))
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.BackColor2 = Color.Red;
                    return;
                }
            }
            catch
            {
            }
            //}
        }

        /// <summary>
        /// 处理DataRow筛选条件的特殊字符
        /// </summary>
        /// <param name="rowFilter">行筛选条件表达式</param>
        /// <returns></returns>
        public static string DvRowFilter(string rowFilter)
        {
            //在DataView的RowFilter里面的特殊字符要用"[]"括起来，单引号要换成"''",他的表达式里面没有通配符的说法
            //return rowFilter
            //    .Replace("[", "[[ ")
            //    .Replace("]", " ]]")
            //    .Replace("*", "[*]")
            //    .Replace("%", "[%]")
            //    .Replace("[[ ", "[[]")
            //    .Replace(" ]]", "[]]")
            //    .Replace("\'", "''");
            return rowFilter;
        }

        private void bt_读Execl写进数据库ck_Click(object sender, EventArgs e)
        {
            string tmpwlmc, tmpys, tmpzrl;
            SMDataSource smDs = this.dataFormController.DAODataForm.DataSource;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            bool bsh = false;//记录审核标记
            DataTable dtcktoday = m_dstAll.Tables["yw_cktoday"];
            if (dtck == null)
            {
                CreateTmpCkTodayTable();
            }
            else
            {
                if (dtck.Rows.Count > 0)
                    dtck.Rows.Clear();
            }
            foreach (DataRow dtcktodaydr in dtcktoday.Rows)
            {
                DataRow dr = dtck.NewRow();
                dr["序号"] = dtcktodaydr["序号"];
                dr["物料名称"] = dtcktodaydr["物料名称"];
                dr["颜色"] = dtcktodaydr["颜色"];
                dr["总用量"] = dtcktodaydr["总用量"];
                dr["单位"] = dtcktodaydr["单位"];
                dr["供应商"] = dtcktodaydr["供应商"];
                dr["来料数量"] = dtcktodaydr["来料数量"];
                dr["来料日期"] = dtcktodaydr["来料日期"];
                dr["配色"] = dtcktodaydr["配色"];
                dr["标注"] = dtcktodaydr["标注"];
                dr["是否审核"] = dtcktodaydr["是否审核"];
                dr["是否标色"] = dtcktodaydr["是否标色"];
                dtck.Rows.Add(dr);
            }
            if (dtck != null && dtck.Rows.Count > 0)
            {
                if (wckexcel != null && string.IsNullOrEmpty(txt_生产单号.Text.ToString()))
                {
                    txt_生产单号.Text = wckexcel.Scdh.ToString();
                }
                DataTable dtywck = m_dstAll.Tables["yw_ck"];
                DataTable dtbom = m_dstAll.Tables["yw_bom"];
                DataView dwywck = dtywck.DefaultView;
                DataView dwbom = dtbom.DefaultView;
                DataView dwywcktoday = dtcktoday.DefaultView;
                Hashtable ht = new Hashtable();
                ht.Clear();
                for (int i = 0; i < dtck.Rows.Count; i++)
                {
                    bsh = false;
                    if (bool.TryParse(dtck.Rows[i]["是否审核"].ToString(), out bsh))
                    {
                        tmpwlmc = dtck.Rows[i]["物料名称"].ToString();
                        tmpys = dtck.Rows[i]["颜色"].ToString();
                        tmpzrl = dtck.Rows[i]["总用量"].ToString();
                        if (bsh)
                        {
                            if (string.IsNullOrEmpty(dtck.Rows[i]["物料名称"].ToString().Trim())) continue;
                            if (string.IsNullOrEmpty(dtck.Rows[i]["总用量"].ToString().Trim())) continue;

                            int icktoday, ick;
                            if ((icktoday = RowCount(dwywcktoday, tmpwlmc, tmpys, tmpzrl)) > (ick = RowCount(dwywck, tmpwlmc, tmpys, tmpzrl)))
                            {
                                if (icktoday == 1 && ick == 0)
                                    YsCkAddRow(dtywck, i);
                            }
                            else if (RowCount(dwywcktoday, tmpwlmc, tmpys, tmpzrl) == RowCount(dwywck, tmpwlmc, tmpys, tmpzrl))
                            {
                                dwbom = new DataView(dtbom);
                                dwbom.RowFilter = string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString()));
                                if (dwbom.Count == 1)
                                {
                                    dwbom[0].Delete();
                                    dtbom.Rows[0].Delete();
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}',只允许已审核表中的记录小于或等于excel中读取的记录", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString())));
                                return;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(dtck.Rows[i]["物料名称"].ToString().Trim())) continue;
                            if (string.IsNullOrEmpty(dtck.Rows[i]["总用量"].ToString().Trim())) continue;
                            int icktoday, ick;
                            if ((icktoday = RowCount(dwywcktoday, tmpwlmc, tmpys, tmpzrl)) > (ick = RowCount(dwywck, tmpwlmc, tmpys, tmpzrl)))
                            {
                                string strfilter = string.Format("物料名称='{0}'|颜色='{1}'|总用量='{2}'", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString()));
                                if (!ht.Contains(strfilter))
                                {
                                    ht.Add(strfilter, icktoday - ick);
                                }
                                else
                                {
                                    ht[strfilter] = icktoday - ick;// int.Parse(ht[strfilter].ToString()) - 1;
                                }
                                dwbom = new DataView(dtbom);
                                dwbom.RowFilter = string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString()));
                                if (dwbom.Count == 1)
                                {
                                    dwbom[0]["单位"] = dtck.Rows[i]["单位"].ToString();
                                    dwbom[0]["供应商"] = dtck.Rows[i]["供应商"].ToString();
                                    dwbom[0]["收货数量"] = dtck.Rows[i]["来料数量"].ToString();
                                    dwbom[0]["收货日期"] = dtck.Rows[i]["来料日期"].ToString();
                                    int istart, iend;
                                    if (int.Parse(ht[strfilter].ToString()) > 1)
                                    {
                                        istart = dwbom[0]["物控备注"].ToString().IndexOf("存在");
                                        iend = dwbom[0]["物控备注"].ToString().IndexOf("条相同的记录");

                                        if (istart != -1 && iend != -1 && iend > istart)
                                        {
                                            dwbom[0]["物控备注"] = dwbom[0]["物控备注"].ToString().Replace(dwbom[0]["物控备注"].ToString().Substring(istart + "存在".Length, iend - istart - "存在".Length), ht[strfilter].ToString());
                                        }
                                        else
                                        {
                                            dwbom[0]["物控备注"] = string.Format("存在{0}条相同的记录,{1}", ht[strfilter].ToString(), dwbom[0]["物控备注"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        istart = dwbom[0]["物控备注"].ToString().IndexOf("存在");
                                        iend = dwbom[0]["物控备注"].ToString().IndexOf("条相同的记录");

                                        if (istart != -1 && iend != -1 && iend > istart)
                                        {
                                            dwbom[0]["物控备注"] = dwbom[0]["物控备注"].ToString().Replace(dwbom[0]["物控备注"].ToString().Substring(istart + "存在".Length, iend - istart - "存在".Length), "1");
                                            dwbom[0]["物控备注"] = dwbom[0]["物控备注"].ToString().Replace("存在1条相同的记录,", "");
                                        }
                                    }
                                }
                                else if (dwbom.Count == 0)
                                {
                                    DataRow dr = dtbom.NewRow();
                                    dr["序号"] = dtck.Rows[i]["序号"].ToString();
                                    dr["物料名称"] = dtck.Rows[i]["物料名称"].ToString();
                                    dr["颜色"] = dtck.Rows[i]["颜色"].ToString();
                                    dr["总用量"] = dtck.Rows[i]["总用量"].ToString();
                                    dr["单位"] = dtck.Rows[i]["单位"].ToString();
                                    dr["供应商"] = dtck.Rows[i]["供应商"].ToString();
                                    dr["收货数量"] = dtck.Rows[i]["来料数量"].ToString();
                                    dr["收货日期"] = dtck.Rows[i]["来料日期"].ToString();
                                    if (int.Parse(ht[strfilter].ToString()) > 1)
                                    {
                                        dr["物控备注"] = string.Format("存在{0}条相同的记录", ht[strfilter].ToString());
                                    }
                                    dtbom.Rows.Add(dr);
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("欠料表中存在相同的记录,物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString())));
                                    break;
                                }
                            }
                            else if (RowCount(dwywcktoday, tmpwlmc, tmpys, tmpzrl) == RowCount(dwywck, tmpwlmc, tmpys, tmpzrl))
                            {

                            }
                            else
                            {
                                MessageBox.Show(string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}',只允许已审核表中的记录小于或等于excel中读取的记录", DvRowFilter(dtck.Rows[i]["物料名称"].ToString()), DvRowFilter(dtck.Rows[i]["颜色"].ToString()), DvRowFilter(dtck.Rows[i]["总用量"].ToString())));
                                return;
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("【是否审核】bool.TryParse发生错误");
                        break;
                    }

                }
                sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_bom"]);
                sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_ck"]);
                sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_cktoday"]);
                sqlDataEngine.RefreshDataset(smDs, m_dstAll);
                m_dstAll.Tables["YW_bom"].AcceptChanges();
                m_dstAll.Tables["YW_ck"].AcceptChanges();
                m_dstAll.Tables["YW_cktoday"].AcceptChanges();
                int itotal = 0, inum = 0;
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.DictionaryEntry objDE in ht)
                {
                    itotal = itotal + int.Parse(objDE.Value.ToString());
                    if (int.Parse(objDE.Value.ToString()) > 1)
                    {
                        inum++;
                        sb.Append(string.Format("{3}:欠料数据中{0}存在{1}条相同的记录，需要自已手动将总用量*{2}", objDE.Key.ToString(), objDE.Value.ToString(), objDE.Value.ToString(), inum.ToString()));
                        sb.Append("\r\n");
                    }
                }
                sb.Append(string.Format("欠料数据包含重复的记录，总共有{0}条\r\n", itotal.ToString()));
                base.Save();

                //===============检查欠料表中所有记录是否存在于购料单中
                ExistCq(sb, m_dstAll.Tables["yw_bom"], "欠料表");
                ////===============检查审核表中所有记录是否存在于购料单中
                //base.Save();
                ExistCq(sb, m_dstAll.Tables["yw_ck"], "审核表");
                txtTx.Text = sb.ToString();
                if (txtTx.Text.ToString().Trim() != "") MessageBox.Show("请查看提醒信息！", "注意:");
                base.Save();

            }

        }

        private void YsCkAddRow(DataTable dtywck, int i)
        {
            DataRow dr = dtywck.NewRow();
            dr["序号"] = dtck.Rows[i]["序号"].ToString();
            dr["物料名称"] = dtck.Rows[i]["物料名称"].ToString();
            dr["颜色"] = dtck.Rows[i]["颜色"].ToString();
            dr["总用量"] = dtck.Rows[i]["总用量"].ToString();
            dr["单位"] = dtck.Rows[i]["单位"].ToString();
            dr["供应商"] = dtck.Rows[i]["供应商"].ToString();
            dr["收货数量"] = dtck.Rows[i]["来料数量"].ToString();
            dr["收货日期"] = dtck.Rows[i]["来料日期"].ToString();
            dr["配色"] = dtck.Rows[i]["配色"].ToString();
            dr["是否标色"] = 1;
            dr["是否审核"] = 1;
            dtywck.Rows.Add(dr);
        }

        private int RowCount(DataView dv, string wlmc, string ys, string zrl)
        {
            DataView ddv = new DataView(dv.Table);
            ddv.RowFilter = string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(wlmc), DvRowFilter(ys), DvRowFilter(zrl));
            return ddv.Count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb">输出内容</param>
        /// <param name="tmpdt"></param>
        /// <param name="tmpstr">备注</param>
        private void ExistCq(StringBuilder sb, DataTable dtbomorcq, string tmpstr)
        {
            if (wckexcel != null && dtck != null && dtck.Rows.Count > 0)
            {
                DataTable dtbom = dtbomorcq;// m_dstAll.Tables["yw_bom"];
                DataTable tmpdt = dtck.Clone();
                foreach (DataRow oldDr in dtck.Rows)
                {
                    DataRow newDr = tmpdt.NewRow();
                    newDr.ItemArray = oldDr.ItemArray;
                    tmpdt.ImportRow(oldDr);
                }
                DataView dwck;// = tmpdt.DefaultView;

                for (int i = 0; i < dtbom.Rows.Count; i++)
                {
                    //=====================
                    if (string.IsNullOrEmpty(dtbom.Rows[i]["物料名称"].ToString().Trim())) continue;
                    if (string.IsNullOrEmpty(dtbom.Rows[i]["总用量"].ToString().Trim())) continue;
                    dwck = new DataView(tmpdt);
                    dwck.RowFilter = string.Format("物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dtbom.Rows[i]["物料名称"].ToString()), DvRowFilter(dtbom.Rows[i]["颜色"].ToString()), DvRowFilter(dtbom.Rows[i]["总用量"].ToString()));
                    if (dwck.Count >= 1)
                    {

                    }
                    else if (dwck.Count == 0)
                    {
                        string strfilter = string.Format("{4}中的数据在购料表中找不到:序号={3}|物料名称='{0}'|颜色='{1}'|总用量='{2}'\r\n", DvRowFilter(dtbom.Rows[i]["物料名称"].ToString()), DvRowFilter(dtbom.Rows[i]["颜色"].ToString()), DvRowFilter(dtbom.Rows[i]["总用量"].ToString()), DvRowFilter(dtbom.Rows[i]["序号"].ToString()), tmpstr);
                        sb.Append(strfilter);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("存在相同的记录,物料名称='{0}' and 颜色='{1}' and 总用量='{2}'", DvRowFilter(dtbom.Rows[i]["物料名称"].ToString()), DvRowFilter(dtbom.Rows[i]["颜色"].ToString()), DvRowFilter(dtbom.Rows[i]["总用量"].ToString())));
                        break;
                    }

                }
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //            string temp = "";
            //            string strexecl = "";
            //            string[] sheetname = new string[1];
            //            string[] sql = new string[1];
            //            string strwdt = string.Empty;
            //            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

            //            string where;

            //            this.Save();
            //            temp = System.Windows.Forms.Application.StartupPath + string.Format("\\生成结果\\{0}欠料表.xls", txt_生产单号.Text.ToString());
            //                where = " where yw_yddj.PROJECT_ID = '" + strProjectId + "'";
            //                strexecl = System.Windows.Forms.Application.StartupPath + "\\模板\\欠料表模板.xls";
            //                sheetname[0] = "欠料表";
            //                sql[0] = @"SELECT 
            //  yw_yddj.[序号],
            //  yw_yddj.[区片号],
            //  yw_yddj.[样点编号],
            //  yw_yddj.[区镇],
            //  yw_yddj.[地址],
            //  yw_yddj.[建筑面积],
            //  yw_yddj.[套内面积],
            //  yw_yddj.[土地面积],
            //  yw_yddj.[结构],
            //  yw_yddj.[朝向],
            //  yw_yddj.[竣工时间],
            //  yw_yddj.[楼龄],
            //  yw_yddj.[临路情况],
            //  yw_yddj.[交通情况],
            //  yw_yddj.[容积率],
            //  yw_yddj.[总价],
            //  yw_yddj.[单价],
            //  yw_yddj.[交易时点],
            //  yw_yddj.[评估时点],
            //  yw_yddj.[样点来源],
            //  yw_yddj.[备注]
            //FROM
            //  yw_yddj" + where;   
            //            Microsoft.Office.Interop.Excel.Workbook wb;
            //            excelop = excelop == null ? new ExcelFromArrayList() : excelop;
            //            wb = excelop.GetWorkBook(strexecl);
            //            for (int i = 0; i < sheetname.Count(); i++)
            //            {
            //                excelop.setCellValue(SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", sql[i]), excelop.GetSheet(sheetname[i]), 2, 1);
            //            }
            //            excelop.SaveAs(temp);
            //            excelop.GetWorkBook(temp);
            //            excelop.showExcel();
            string tmpstr = string.Format("{0}{1}.xls", excelJgPath, this.txt_生产单号.Text.ToString());
            this.gdc_bom.ExportToXls(tmpstr);
            System.Diagnostics.Process.Start(tmpstr);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (base.GetControlBindValue(this.cbe_文件ck) != null && base.GetControlBindValue(this.cbe_文件ck).ToString().Trim() != "")
                {
                    if (brun == false)
                    {
                        Thread t = new Thread(new ParameterizedThreadStart(this.GetExcelDataGc));
                        t.Start("null");
                        brun = true;
                    }
                }

            }
            finally
            {
                timer1.Enabled = false;
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            AutoSetRangeCk(m_dstAll.Tables["yw_ckexcel"]);
        }

        private void CkTodayFilter()
        {
            this.gv_cktotay.ClearColumnsFilter();
            DevExpress.XtraGrid.Views.Base.ColumnView view = this.gv_cktotay;
            DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo viewFilterInfo = new ViewColumnFilterInfo(view.Columns["是否标色"], new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[是否标色] ==1", ""));
            view.ActiveFilter.Add(viewFilterInfo);
            viewFilterInfo = new ViewColumnFilterInfo(view.Columns["是否审核"], new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[是否审核] ==0", ""));
            view.ActiveFilter.Add(viewFilterInfo);
            //view.SortInfo.Add(new DevExpress.XtraGrid.Columns.GridColumnSortInfo(new DevExpress.XtraGrid.Columns.GridColumn(""), DevExpress.Data.ColumnSortOrder.Descending));
        }

        private void bt_bsms_Click(object sender, EventArgs e)
        {
            CkTodayFilter();
        }

        public string getFilesMD5Hash(string file)
        {
            //MD5 hash provider for computing the hash of the file
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //open the file
            FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);

            //calculate the files hash
            md5.ComputeHash(stream);

            //close our stream
            stream.Close();

            //byte array of files hash
            byte[] hash = md5.Hash;

            //string builder to hold the results
            StringBuilder sb = new StringBuilder();

            //loop through each byte in the byte array
            foreach (byte b in hash)
            {
                //format each byte into the proper value and append
                //current value to return value
                sb.Append(string.Format("{0:X2}", b));
            }

            //return the MD5 hash of the file
            return sb.ToString();
        }


    }
}
