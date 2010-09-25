using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelOper;
using Microsoft.Office.Interop.Excel;
namespace ZBPM
{
    public partial class FjBb : SkyMap.Net.DataForms.AbstractDataForm
    {
        ExcelFromArrayList excelop = null;   //excel操作;
        public FjBb()
        {
            InitializeComponent();
        }

        public override bool LoadMe()
        {
            return true;
        }

        private void Btn修正体系_Click(object sender, EventArgs e)
        {
            string temp = System.Windows.Forms.Application.StartupPath + "\\生成结果\\修正体系.xls";
            string strexecl = System.Windows.Forms.Application.StartupPath + "\\fj模板\\修正体系模板.xls";
            string[] sheetname = new string[20];
            string[] sql = new string[20];
            string strwdt = string.Empty;
            string where = this.cbe修改类型.EditValue.ToString();
            if (string.IsNullOrEmpty(where) || where == "不修改")
            {
                where = " where yw_jzfj.[修改类型] ='' or yw_jzfj.[修改类型] is null";
            }
            else if (where == "修改过的")
            {
                where = " where yw_jzfj.[修改类型] is not null and yw_jzfj.[修改类型] != ''";
            }
            else if (where == "全部")
            {
                where = " where 1 =1 ";
            }
            else
            {
                where = string.Format("where yw_jzfj.[修改类型] like '%{0}%'", where);
            }

            Microsoft.Office.Interop.Excel.Workbook wb;
            excelop = excelop == null ? new ExcelFromArrayList() : excelop;
            wb = excelop.GetWorkBook(strexecl);
            // excelop.replaceCellValue("xxxx年第x季度", year + "年第" + cmb_common.SelectedText + "季度", excelop.GetSheet(1), 2, 1);
            //excelop.setCellValue("哈" + DateTime.Now.ToString("D"), excelop.GetSheet("车房区片价"), 20, 1);

            #region 车房区片价
            sheetname[0] = "车房区片价";
            sql[0] = @"SELECT
  '' as id,
  YW_JZFJ.[区片号],
  [yw_车房区片价].[区片价-按个],
  [yw_车房区片价].[区片价-按面积],
  [yw_车房区片价].[区片价-杂物房],
  [yw_车房区片价].[区片价-按摩托车位]
FROM
  [yw_车房区片价]
  INNER JOIN YW_JZFJ ON ([yw_车房区片价].project_id = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 朝向修正
            sheetname[1] = "朝向修正";
            sql[1] = @"SELECT 
  '' as id,
  YW_JZFJ.[区片号],
  [yw_朝向修正].[东],
  [yw_朝向修正].[东南],
  [yw_朝向修正].[南],
  [yw_朝向修正].[西南],
  [yw_朝向修正].[西],
  [yw_朝向修正].[西北],
  [yw_朝向修正].[北],
  [yw_朝向修正].[东北]
FROM
  [yw_朝向修正]
  INNER JOIN YW_JZFJ ON ([yw_朝向修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 电梯修正
            sheetname[2] = "电梯修正";
            sql[2] = @"SELECT 
  '' as id,
   YW_JZFJ.[区片号],
  [yw_电梯修正].[有电梯],
  [yw_电梯修正].[无电梯]
FROM
  [yw_电梯修正]
  INNER JOIN YW_JZFJ ON ([yw_电梯修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 结构类型修正
            sheetname[3] = "结构类型修正";
            sql[3] = @"SELECT 
  '' as id,
   YW_JZFJ.[区片号],
  [yw_结构类型修正].[钢筋混凝土],
  [yw_结构类型修正].[混合],
  [yw_结构类型修正].[砖木],
  [yw_结构类型修正].[其他]
FROM
  [yw_结构类型修正]
  INNER JOIN YW_JZFJ ON ([yw_结构类型修正].project_id = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 交通修正
            sheetname[4] = "交通修正";
            sql[4] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_交通修正].[不能通摩托车],
  [yw_交通修正].[可同摩托车，不同小汽车],
  [yw_交通修正].[可通1小车],
  [yw_交通修正].[可通2小车],
  [yw_交通修正].[可通3小车]
 
FROM
  [yw_交通修正]
  INNER JOIN YW_JZFJ ON ([yw_交通修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 建筑密度修正
            sheetname[5] = "建筑密度修正";
            sql[5] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_建筑密度修正].[建筑密度0-4],
  [yw_建筑密度修正].[建筑密度4],
  [yw_建筑密度修正].[建筑密度5],
  [yw_建筑密度修正].[建筑密度6],
  [yw_建筑密度修正].[建筑密度7],
  [yw_建筑密度修正].[建筑密度8],
  [yw_建筑密度修正].[建筑密度9],
  [yw_建筑密度修正].[建筑密度10]
FROM
  [yw_建筑密度修正]
  INNER JOIN YW_JZFJ ON ([yw_建筑密度修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 建筑面积修正
            sheetname[6] = "建筑面积修正";
            sql[6] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_建筑面积修正].[类型 ],
  [yw_建筑面积修正].[小于60平方米],
  [yw_建筑面积修正].[60~80平方米],
  [yw_建筑面积修正].[80~100平方米],
  [yw_建筑面积修正].[100~120平方米],
  [yw_建筑面积修正].[120~140平方米],
  [yw_建筑面积修正].[140~160平方米],
  [yw_建筑面积修正].[160~200平方米],
  [yw_建筑面积修正].[200~250平方米],
  [yw_建筑面积修正].[250平方米以上]
FROM
  [yw_建筑面积修正]
  INNER JOIN YW_JZFJ ON ([yw_建筑面积修正].project_id = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 建筑面积修正（楼型）
            sheetname[7] = "建筑面积修正（楼型）";
            sql[7] = @"SELECT   [建筑面积修正楼型].ID,
  [建筑面积修正楼型].[类型名称]
FROM
  [建筑面积修正楼型]";
            #endregion
            #region 电梯房楼层修正
            sheetname[8] = "电梯房楼层修正";
            sql[8] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_电梯房楼层修正].[楼层数],
  [yw_电梯房楼层修正].[1楼],
  [yw_电梯房楼层修正].[2楼],
  [yw_电梯房楼层修正].[3楼],
  [yw_电梯房楼层修正].[4楼],
  [yw_电梯房楼层修正].[5楼],
  [yw_电梯房楼层修正].[6楼],
  [yw_电梯房楼层修正].[7楼],
  [yw_电梯房楼层修正].[8楼],
  [yw_电梯房楼层修正].[9楼],
  [yw_电梯房楼层修正].[10楼],
  [yw_电梯房楼层修正].[11楼],
  [yw_电梯房楼层修正].[12楼],
  [yw_电梯房楼层修正].[13楼],
  [yw_电梯房楼层修正].[14楼],
  [yw_电梯房楼层修正].[15楼],
  [yw_电梯房楼层修正].[16楼],
  [yw_电梯房楼层修正].[17楼],
  [yw_电梯房楼层修正].[18楼],
  [yw_电梯房楼层修正].[19楼],
  [yw_电梯房楼层修正].[20楼],
  [yw_电梯房楼层修正].[21楼],
  [yw_电梯房楼层修正].[22楼],
  [yw_电梯房楼层修正].[23楼],
  [yw_电梯房楼层修正].[24楼],
  [yw_电梯房楼层修正].[25楼],
  [yw_电梯房楼层修正].[26楼],
  [yw_电梯房楼层修正].[27楼],
  [yw_电梯房楼层修正].[28楼],
  [yw_电梯房楼层修正].[29楼],
  [yw_电梯房楼层修正].[30楼],
  [yw_电梯房楼层修正].[31楼],
  [yw_电梯房楼层修正].[32楼],
  [yw_电梯房楼层修正].[33楼],
  [yw_电梯房楼层修正].[34楼],
  [yw_电梯房楼层修正].[35楼],
  [yw_电梯房楼层修正].[36楼]
FROM
  [yw_电梯房楼层修正]
  INNER JOIN YW_JZFJ ON ([yw_电梯房楼层修正].project_id = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            //            #region 无电梯房楼层修正
            //            sheetname[9] = "无电梯房楼层修正";
            //            sql[9] = @"SELECT 
            //  '' AS id,
            //  YW_JZFJ.[区片号],
            //  [yw_无电梯房楼层修正].[楼层数],
            //  [yw_无电梯房楼层修正].[1楼],
            //  [yw_无电梯房楼层修正].[2楼],
            //  [yw_无电梯房楼层修正].[3楼],
            //  [yw_无电梯房楼层修正].[4楼],
            //  [yw_无电梯房楼层修正].[5楼],
            //  [yw_无电梯房楼层修正].[6楼],
            //  [yw_无电梯房楼层修正].[7楼],
            //  [yw_无电梯房楼层修正].[8楼],
            //  [yw_无电梯房楼层修正].[9楼],
            //  [yw_无电梯房楼层修正].[10楼],
            //  [yw_无电梯房楼层修正].[11楼],
            //  [yw_无电梯房楼层修正].[12楼],
            //  [yw_无电梯房楼层修正].[13楼],
            //  [yw_无电梯房楼层修正].[14楼],
            //  [yw_无电梯房楼层修正].[15楼]
            //FROM
            //  [yw_无电梯房楼层修正]
            //  INNER JOIN YW_JZFJ ON ([yw_无电梯房楼层修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)";
            //            #endregion
            #region 临路情况修正
            sheetname[9] = "临路情况修正";
            sql[9] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_临路情况修正].[临主要交通干道],
  [yw_临路情况修正].[主要交通干道名称],
  [yw_临路情况修正].[临一般交通干道],
  [yw_临路情况修正].[一般交通干道名称],
  [yw_临路情况修正].[不临交通干道],
  [yw_临路情况修正].[临支路],
  [yw_临路情况修正].[支路名称],
  [yw_临路情况修正].[小区交通干道名称],
  [yw_临路情况修正].[临小区交通干道]
FROM
  [yw_临路情况修正]
  INNER JOIN YW_JZFJ ON ([yw_临路情况修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 楼龄修正
            sheetname[10] = "楼龄修正";
            sql[10] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_楼龄修正].[结构类型],
  [yw_楼龄修正].[1年],
  [yw_楼龄修正].[2年],
  [yw_楼龄修正].[3年],
  [yw_楼龄修正].[4年],
  [yw_楼龄修正].[5年],
  [yw_楼龄修正].[6年],
  [yw_楼龄修正].[7年],
  [yw_楼龄修正].[8年],
  [yw_楼龄修正].[10年],
  [yw_楼龄修正].[12年],
  [yw_楼龄修正].[13年],
  [yw_楼龄修正].[15年],
  [yw_楼龄修正].[18年],
  [yw_楼龄修正].[20年],
  [yw_楼龄修正].[22年],
  [yw_楼龄修正].[25年],
  [yw_楼龄修正].[26年],
  [yw_楼龄修正].[30年],
  [yw_楼龄修正].[35年],
  [yw_楼龄修正].[40年],
  [yw_楼龄修正].[45年],
  [yw_楼龄修正].[50年],
  [yw_楼龄修正].[55年],
  [yw_楼龄修正].[60年],
  [yw_楼龄修正].[65年],
  [yw_楼龄修正].[70年]
FROM
  YW_JZFJ
  INNER JOIN [yw_楼龄修正] ON (YW_JZFJ.PROJECT_ID = [yw_楼龄修正].PROJECT_ID)" + where;
            #endregion
            #region 楼龄修正（结构类型）
            sheetname[11] = "楼龄修正（结构类型）";
            sql[11] = @"SELECT 
  [楼龄修正结构类型].[结构类型],
  [楼龄修正结构类型].[结构说明]
FROM
  [楼龄修正结构类型]";
            #endregion
            #region 楼型修正
            sheetname[12] = "楼型修正";
            sql[12] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_楼型修正].[1梯1户及2户],
  [yw_楼型修正].[1梯3户及以上]
FROM
  [yw_楼型修正]
  INNER JOIN YW_JZFJ ON ([yw_楼型修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 区片信息
            sheetname[13] = "区片信息";
            sql[13] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_区片信息].[区片价],
  [yw_区片信息].[区片信息],
  [yw_区片信息].[区片备注],
  [yw_区片信息].[创建日期],
  [yw_区片信息].[修改日期],
  [yw_jzfj].[修改类型],
  [yw_jzfj].[修改备注],
  [yw_jzfj].[地价区片价]
FROM
  [yw_区片信息]
  INNER JOIN YW_JZFJ ON ([yw_区片信息].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 容积率修正
            sheetname[14] = "容积率修正";
            sql[14] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_容积率修正].[容积类型],
  [yw_容积率修正].[0_1],
  [yw_容积率修正].[0_2],
  [yw_容积率修正].[0_3],
  [yw_容积率修正].[0_35],
  [yw_容积率修正].[0_4],
  [yw_容积率修正].[0_5],
  [yw_容积率修正].[0_6],
  [yw_容积率修正].[0_65],
  [yw_容积率修正].[0_7],
  [yw_容积率修正].[0_8],
  [yw_容积率修正].[0_9],
  [yw_容积率修正].[1],
  [yw_容积率修正].[1_1],
  [yw_容积率修正].[1_2],
  [yw_容积率修正].[1_25],
  [yw_容积率修正].[1_3],
  [yw_容积率修正].[1_4],
  [yw_容积率修正].[1_5],
  [yw_容积率修正].[1_58],
  [yw_容积率修正].[1_6],
  [yw_容积率修正].[1_63],
  [yw_容积率修正].[1_7],
  [yw_容积率修正].[1_8],
  [yw_容积率修正].[1_9],
  [yw_容积率修正].[2],
  [yw_容积率修正].[2_1],
  [yw_容积率修正].[2_2],
  [yw_容积率修正].[2_3],
  [yw_容积率修正].[2_4],
  [yw_容积率修正].[2_5],
  [yw_容积率修正].[2_6],
  [yw_容积率修正].[2_7],
  [yw_容积率修正].[2_8],
  [yw_容积率修正].[2_9],
  [yw_容积率修正].[3],
  [yw_容积率修正].[3_1],
  [yw_容积率修正].[3_2],
  [yw_容积率修正].[3_3],
  [yw_容积率修正].[3_4],
  [yw_容积率修正].[3_5],
  [yw_容积率修正].[3_6],
  [yw_容积率修正].[3_7],
  [yw_容积率修正].[3_8],
  [yw_容积率修正].[3_9],
  [yw_容积率修正].[4],
  [yw_容积率修正].[4_1],
  [yw_容积率修正].[4_2],
  [yw_容积率修正].[4_3],
  [yw_容积率修正].[4_4],
  [yw_容积率修正].[4_5],
  [yw_容积率修正].[4_6],
  [yw_容积率修正].[4_7],
  [yw_容积率修正].[4_8],
  [yw_容积率修正].[4_9],
  [yw_容积率修正].[5]
FROM
  [yw_容积率修正]
  INNER JOIN YW_JZFJ ON ([yw_容积率修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 容积率修正（类型）
            sheetname[15] = "容积率修正（类型）";
            sql[15] = @"SELECT 
  [容积率修正类型].ID,
  [容积率修正类型].[容积率修正类型]
FROM
  [容积率修正类型]";
            #endregion
            #region 物业管理修正
            sheetname[16] = "物业管理修正";
            sql[16] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_物业管理修正].[有物业管理],
  [yw_物业管理修正].[无物业管理]
FROM
  [yw_物业管理修正]
  INNER JOIN YW_JZFJ ON ([yw_物业管理修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 复式修正
            sheetname[17] = "复式修正";
            sql[17] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_复式修正].[复式],
  [yw_复式修正].[不是复式]
FROM
  [yw_复式修正]
  INNER JOIN YW_JZFJ ON ([yw_复式修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 公摊修正
            sheetname[18] = "公摊修正";
            sql[18] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_公摊修正].[电梯房含公摊],
  [yw_公摊修正].[电梯房不含公摊],
  [yw_公摊修正].[非电梯房含公摊],
  [yw_公摊修正].[非电梯房不含公摊]
FROM
  [yw_公摊修正]
  INNER JOIN YW_JZFJ ON ([yw_公摊修正].PROJECT_ID = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            #region 车房类型修正
            sheetname[19] = "车房类型修正";
            sql[19] = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_车房类型修正].[小车房],
  [yw_车房类型修正].[小车库]
FROM
  [yw_车房类型修正]
  INNER JOIN YW_JZFJ ON ([yw_车房类型修正].project_id = YW_JZFJ.PROJECT_ID)" + where;
            #endregion
            GetRecord(sheetname, sql);
            strwdt = "select * from [yw_jzfj]" + where;
            System.Data.DataTable wdtdt = SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", strwdt);

            System.Data.DataTable tmpdt = new System.Data.DataTable();
            System.Data.DataTable dt = new System.Data.DataTable("YW_JZFJ");
            System.Data.DataColumn dc;
            dc = new DataColumn("ID");
            //dc.DataType = Type.GetType("System.String");
            dt.Columns.Add(dc);
            dc = new DataColumn("区片号");
            dt.Columns.Add(dc);
            dc = new DataColumn("楼层数");
            dt.Columns.Add(dc);
            dc = new DataColumn("1楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("2楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("3楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("4楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("5楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("6楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("7楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("8楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("9楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("10楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("11楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("12楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("13楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("14楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("15楼");
            dt.Columns.Add(dc);
            for (int i = 0; i < wdtdt.Rows.Count; i++)
            {
                tmpdt = Wdt(wdtdt.Rows[i]["PROJECT_ID"].ToString());
                if (tmpdt != null)
                {
                    if (tmpdt.Rows.Count > 0)
                    {
                        DataRow tmpdr = null;
                        foreach (DataRow dr in tmpdt.Rows)
                        {
                            tmpdr = dt.NewRow();
                            tmpdr["ID"] = dr["ID"].ToString();
                            tmpdr["区片号"] = dr["区片号"].ToString();
                            tmpdr["楼层数"] = dr["楼层数"].ToString();
                            tmpdr["1楼"] = dr["1楼"].ToString();
                            tmpdr["2楼"] = dr["2楼"].ToString();
                            tmpdr["3楼"] = dr["3楼"].ToString();
                            tmpdr["4楼"] = dr["4楼"].ToString();
                            tmpdr["5楼"] = dr["5楼"].ToString();
                            tmpdr["6楼"] = dr["6楼"].ToString();
                            tmpdr["7楼"] = dr["7楼"].ToString();
                            tmpdr["8楼"] = dr["8楼"].ToString();
                            tmpdr["9楼"] = dr["9楼"].ToString();
                            tmpdr["10楼"] = dr["10楼"].ToString();
                            tmpdr["11楼"] = dr["11楼"].ToString();
                            tmpdr["12楼"] = dr["12楼"].ToString();
                            tmpdr["13楼"] = dr["13楼"].ToString();
                            tmpdr["14楼"] = dr["14楼"].ToString();
                            tmpdr["15楼"] = dr["15楼"].ToString();
                            dt.Rows.Add(tmpdr);
                        }
                    }
                }
            }            
            excelop.setCellValue(dt, excelop.GetSheet("无电梯房楼层修正"), 2, 1);
            excelop.SaveAs(temp);
            excelop.GetWorkBook(temp);
            excelop.showExcel();
        }

        private void GetRecord(string[] sheetnme, string[] sql)
        {
            for (int i = 0; i < sheetnme.Count(); i++)
            {
                excelop.setCellValue(SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", sql[i]), excelop.GetSheet(sheetnme[i]), 2, 1);
            }
        }

        private System.Data.DataTable  Wdt(string qph)
        {
            string sql = @"SELECT 
  '' AS id,
  YW_JZFJ.[区片号],
  [yw_无电梯房楼层修正].[楼层数],
  [yw_无电梯房楼层修正].[1楼],
  [yw_无电梯房楼层修正].[2楼],
  [yw_无电梯房楼层修正].[3楼],
  [yw_无电梯房楼层修正].[4楼],
  [yw_无电梯房楼层修正].[5楼],
  [yw_无电梯房楼层修正].[6楼],
  [yw_无电梯房楼层修正].[7楼],
  [yw_无电梯房楼层修正].[8楼],
  [yw_无电梯房楼层修正].[9楼],
  [yw_无电梯房楼层修正].[10楼],
  [yw_无电梯房楼层修正].[11楼],
  [yw_无电梯房楼层修正].[12楼],
  [yw_无电梯房楼层修正].[13楼],
  [yw_无电梯房楼层修正].[14楼],
  [yw_无电梯房楼层修正].[15楼]
FROM
  [yw_无电梯房楼层修正]
  INNER JOIN YW_JZFJ ON ([yw_无电梯房楼层修正].PROJECT_ID = YW_JZFJ.PROJECT_ID ) where [yw_无电梯房楼层修正].PROJECT_ID='{0}'";

            string maxlc = @"SELECT 
  max([yw_无电梯房楼层修正].[楼层数]) as 最大楼层数 FROM
  [yw_无电梯房楼层修正]
  INNER JOIN YW_JZFJ ON ([yw_无电梯房楼层修正].PROJECT_ID = YW_JZFJ.PROJECT_ID ) where [yw_无电梯房楼层修正].PROJECT_ID='{0}'";
            sql = string.Format(sql, qph);
            maxlc = string.Format(maxlc, qph);
            System.Data.DataTable dt1 = SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", sql);
            if (dt1.Rows.Count <=0) return null;
            System.Data.DataTable dt2 = SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", maxlc);
            System.Data.DataTable dt = new System.Data.DataTable("YW_JZFJ");
            System.Data.DataColumn dc;
            dc = new DataColumn("ID");
            //dc.DataType = Type.GetType("System.String");
            dt.Columns.Add(dc);
            dc = new DataColumn("区片号");
            dt.Columns.Add(dc);
            dc = new DataColumn("楼层数");
            dt.Columns.Add(dc);
            dc = new DataColumn("1楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("2楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("3楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("4楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("5楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("6楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("7楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("8楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("9楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("10楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("11楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("12楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("13楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("14楼");
            dt.Columns.Add(dc);
            dc = new DataColumn("15楼");
            dt.Columns.Add(dc);
            DataRow dr;
            int intmaxlc = int.Parse(dt2.Rows[0][0].ToString());
            for (int i = 1; i <= intmaxlc; i++)
            {
                dr = dt.NewRow();
                dr["楼层数"] = i.ToString();
                dr["区片号"] = dt1.Rows[0]["区片号"].ToString();
                dt.Rows.Add(dr);
            }

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int ilou = int.Parse(dt1.Rows[i]["楼层数"].ToString());
                if (ilou <= 15)
                {
                    for (int ii = 0; ii < ilou; ii++)
                    {
                        dt.Rows[ii][ilou + 2] = dt1.Rows[i][ii + 3].ToString();
                    }
                }
                else
                {
                     SkyMap.Net.Gui.MessageHelper.ShowInfo(dt1.Rows[i]["区片号"].ToString() + "楼层数输入有误!");
                     return null;
                }

            }
            return dt;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable GetDtxz1()
        {
            string tmp = @"";
            return SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", tmp);
        }

    }
}
