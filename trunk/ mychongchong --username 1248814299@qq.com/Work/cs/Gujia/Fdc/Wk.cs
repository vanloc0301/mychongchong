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
        private DataSet m_dstAll;

        private PrintSet m_printSet;
        private wk.wcexcel wexcel;
        private DataTable dt;
        static DateTime beforeTime;            //Excel启动之前时间
        static DateTime afterTime;
        ExcelMapper mapper = new ExcelMapper();
        ExcelFromArrayList excelop = null;   //excel操作 样点导出功能时使用; 2010-08-11

        string excelFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\bin\wc\欠料明细.xls";
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
FROM YW_bom where PROJECT_ID ='"+strProjectId+"' order by id asc","SELECT * FROM YW_wcexcel where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_bomexcel where PROJECT_ID ='"+strProjectId+"'"}, new string[] { "YW_bom", "YW_wcexcel", "YW_bomexcel" });
            if (m_dstAll != null && m_dstAll.Tables.Count != 0)
            {
                m_dstAll.Tables["YW_bom"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_bom where PROJECT_ID ='" + strProjectId + "' order by id asc");
                m_dstAll.Tables["YW_wcexcel"].ExtendedProperties.Add("selectsql", @"SELECT  *  FROM YW_wcexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_bomexcel"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_bomexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_ck"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_ck where PROJECT_ID ='" + strProjectId + "' order by id asc");
                m_dstAll.Tables["YW_wcckexcel"].ExtendedProperties.Add("selectsql", @"SELECT  *  FROM YW_wcckexcel where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_ckexcel"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_ckexcel where PROJECT_ID ='" + strProjectId + "'");

            }
            m_dstAll.Tables["YW_bom"].TableNewRow += new DataTableNewRowEventHandler(Wk_BomNewRow);
            m_dstAll.Tables["YW_wcexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_bomexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_ck"].TableNewRow += new DataTableNewRowEventHandler(Wk_BomNewRow);
            m_dstAll.Tables["YW_wcckexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
            m_dstAll.Tables["YW_ckexcel"].TableNewRow += new DataTableNewRowEventHandler(Wk_ExcelNewRow);
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

            SMDataSource smDs = this.dataFormController.DAODataForm.DataSource;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_bom"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_wcexcel"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_bomexcel"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_ck"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_wcckexcel"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_ckexcel"]);
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
        }

        void Wk_BomNewRow(object sender, DataTableNewRowEventArgs e)
        {
            int icount = m_dstAll.Tables["YW_bom"].Rows.Count + 1;
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
                string strwc = string.Format("生产单号：{0}，预备齐料期：{1}", wexcel.Scdh.ToString(),  wexcel.Qlq.ToString());
                lblWcMsg.Text = strwc;
                dt = new DataTable("bomexcel");
                dt.Columns.Add("序号", System.Type.GetType("System.String"));
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
                    dr["序号"] = inum.ToString(); //wexcel.Xh[i].ToString();
                    dr["物料名称"] = wexcel.Wlmc[i].ToString().Split(new char[]{'@'})[0];
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
            if (wexcel != null && dt != null && dt.Rows.Count > 0)
            {
                //this.txt_厂款号.Text = wexcel.Ckh.ToString();
                txt_生产单号.Text = wexcel.Scdh.ToString();
                DataTable dtbom = m_dstAll.Tables["yw_bom"];
                if (dtbom.Rows.Count > 0)
                {
                    MessageBox.Show("已有物料清单,Excel中读取的数据将追加到现有的数据中！", "提醒：");
                }


                for (int i = 0; i < dt.Rows.Count; i++)
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

            }

        }

        private void bt_模板设置_Click(object sender, EventArgs e)
        {
            DataTable tmpdt = m_dstAll.Tables["yw_bomexcel"];
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
            this.txtSearch1.Text = string.Concat(cbe_工作表.Text.ToString(),this.txtSearch1.Text.Substring(this.txtSearch1.Text.ToString().IndexOf("|") ));
            this.txtSearch2.Text = string.Concat(cbe_工作表.Text.ToString(), this.txtSearch2.Text.Substring(this.txtSearch2.Text.ToString().IndexOf("|")));
            if (this.txtSearch1.Text.ToString().Split(new char[] { '|' })[0] != this.txtSearch2.Text.ToString().Split(new char[] { '|' })[0])
            {
                MessageBox.Show("工作表设置需要一样!", "提示");
                return;
            }
            AutoSet(this.txtSearch1.Text.ToString(), m_dstAll.Tables["yw_wcexcel"], 0);
            AutoSet(this.txtSearch2.Text.ToString(), m_dstAll.Tables["yw_bomexcel"], 1);

        }

        private void AutoSet(string strtxt, DataTable dt, int inti)
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
                    strreturn = search(str[0].ToString(), str[i].Split(new char[] { '#' })[1].ToString(), inti);
                    if (strreturn != "")
                    {
                        ht.Add(strname, strreturn);
                    }
                }
                else
                {
                    for (int j = 0; j < str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' }).Length; j++)
                    {
                        strreturn = search(str[0].ToString(), str[i].Split(new char[] { '#' })[1].Split(new char[] { '@' })[j].ToString(), inti);
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
                bt_模板设置_Click(null, null);
            }
            #region
            #endregion
        }

        private string search(string sheetname, string strKeyWord, int inti)
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

                object oText = strKeyWord.Trim().ToUpper();
                //for (i = 1; i <= iEWSCnt; i++)  {
                ews = null;

                ews = (Microsoft.Office.Interop.Excel.Worksheet)ew.Worksheets[sheetname];

                oRange = null;

                oRange = ((Microsoft.Office.Interop.Excel.Range)ews.UsedRange).Find(

                oText, MissingValue, MissingValue,

                MissingValue, MissingValue, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext,

                MissingValue, MissingValue, MissingValue);

                if (oRange != null && oRange.Cells.Rows.Count >= 1 && oRange.Cells.Columns.Count >= 1)
                {
                    return string.Format("{0}{1}", ExcelColumnTranslator.ToName(int.Parse(oRange.Column.ToString()) - 1), int.Parse(oRange.Row.ToString()) + inti);
                }

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



    }
}