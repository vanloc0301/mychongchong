using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SkyMap.Net.DAO;
using SkyMap.Net.Gui.Components;
using ExcelOper;
using System.Configuration;
using System.IO;
using SkyMap.Net.DataAccess;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Diagnostics;
using SkyMap.Net.Gui;
using SkyMap.Net.Core;
namespace ZBPM
{
    public partial class ZBPMReportJPWTZ : SkyMap.Net.DataForms.AbstractDataForm
    {

        ArrayList expressions;   //列计算公式；
        ExcelFromArrayList excelop = null;   //excel操作;
        ArrayList hexpressions;      //计算公式;    
        bool isZq = false;      //是否选择镇区
        string filename;      //文件名
        string temp = "";      //临时文件名
        string[] season = null;  //季节,半年
        int index;               //季节索引
        int year;                //年份
        string state = "已办";  //状态 
        string conDate = "";
        string endDate = "";   //成交价日期
        public ZBPMReportJPWTZ()
        {
            InitializeComponent();
        }

        public override bool LoadMe()
        {
            return true;
        }

        public delegate void CloseDialog();    // 定义委托
        public void closeDialog()
        {
            try
            {
                WaitDialogHelper.Close();
            }
            catch { }
        }
        #region   初始化数据

        private void ZBPMReportJPWTZ_Load(object sender, EventArgs e)
        {

            for (long i = DateTime.Now.Year; i >= 2000; i--)
            {
                cmb_year.Properties.Items.Add(i);
            }

            ////镇区
            if (cmb_town.Properties.DataSource == null)
            {
                DataWordLookUpEditHelper.Init(this.cmb_town, "ZSTOWNSHIP", "Name", "Code");

            }
            initData();

        }
        private void initData()
        {


            int unit = 10000;
            double area = 0.0015;
            expressions = new ArrayList();//计算公式列

            expressions.Add(" count(project_id) ");                   //已办业务宗数 以及对比
            expressions.Add(" sum( 用地面积)*" + area + " ");              //面积
            expressions.Add(" sum( 交易底价)/ " + unit + " ");            //底价
            expressions.Add(" sum( 交易底价)/(sum(  用地面积)*" + area + ")/" + unit + "  ");   //底价平均单价
            expressions.Add(" sum( 成交价)/" + unit + "  ");                //成交价
            expressions.Add(" sum( 成交价)/(sum(  用地面积)*" + area + ")/" + unit + "  ");  //成交平均单价
            expressions.Add(" sum(  超底价金额)/" + unit + "  ");    //超底价
            expressions.Add(" sum(  超底价金额)/10/" + unit + "  ");           // //超底价市级分成
            expressions.Add(" sum(  出让金)/" + unit + "  "); //出让金市级分成
            expressions.Add(" sum(  耕地占用税)/" + unit + "  ");       // 耕地占用税
            expressions.Add(" sum( 业务费 )/" + unit + "  ");    //业务费     



            hexpressions = new ArrayList();//计算公式行;

            hexpressions.Add("   and  业务状态='完成竞价' and  是否成交=1  and  (成交后状态='交付中'  or 成交后状态='付款中业务' )  ");   //已办业务 中的成交业务  中的交付中            
            hexpressions.Add("   and  业务状态='完成竞价' and  是否成交=1  and  成交后状态='土地交付中业务'   ");   //已办业务 中的成交业务  中的交付中            
            hexpressions.Add("   and  业务状态='完成竞价' and  是否成交=1  and  (成交后状态='结案'   or 成交后状态='结案业务'  )     ");//已办业务 中的成交业务结案
            hexpressions.Add("   and  业务状态='完成竞价' and  是否成交=1  and  (成交后状态='违约'  or 成交后状态='违约业务'  )    ");  //已办业务 中的成交业务违约
            hexpressions.Add("  and  业务状态='完成竞价' and  (是否成交=0 or 是否成交 is null ) ");   //已办业务 中的流拍业务
            hexpressions.Add(" and  业务状态 ='公告中'  ");    //公告中-----------------------------
            hexpressions.Add("  and  业务状态= '审批中'  ");      //在办业务---------------==审批中------------------------        


            season = new string[6];
            season[0] = "1,2,3";
            season[1] = "4,5,6";
            season[2] = "7,8,9";
            season[3] = "10,11,12";
            season[4] = "1,2,3,4,5,6";
            season[5] = "7,8,9,10,11,12";

            //
        }
        #endregion

        private void btn_select_Click(object sender, EventArgs e)
        {
            try
            {
                //     initData();  //初始化计算公式
                conDate = "现场竞价时间";
                endDate = "现场竞价时间";
                SkyMap.Net.Gui.WaitDialogHelper.Show();
                index = int.Parse(cmb_common.SelectedText == "" ? "2" : cmb_common.SelectedText) - 1;
                year = int.Parse(cmb_year.SelectedText);

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string path = config.AppSettings.Settings["templatepath"].Value.ToString();
                if (cmb_reportname.SelectedIndex == 0 && cmb_town.Text == "")
                    filename = System.Windows.Forms.Application.StartupPath + path + cmb_reportType.SelectedIndex + this.cmb_reportname.SelectedText.Trim();
                else filename = System.Windows.Forms.Application.StartupPath + path + this.cmb_reportname.SelectedText.Trim();

                temp = Path.GetTempPath() + Path.GetFileName(filename).ToString();
                ThreadStart ts = null;
                if (cmb_reportname.SelectedIndex == 0)   // 招拍挂
                {

                    if (cmb_town.Text != "")  //镇区
                    {
                        ts = new ThreadStart(zqProccessExcelData);
                        // filename = System.Windows.Forms.Application.StartupPath + path+"";
                        isZq = true;

                    }
                    else
                    {
                        ts = new ThreadStart(proccessExcelData);
                        isZq = false;
                    }

                }
                else if (cmb_reportname.SelectedIndex == 1)   //建设用地
                {
                    ts = new ThreadStart(jsrdCount);
                }
                else if (cmb_reportname.SelectedIndex == 2)  //房地产价格监测表
                {
                    ts = new ThreadStart(fdcPriceCount);
                }
                else if (cmb_reportname.SelectedIndex == 3) //土地出让金市级分成收入统计表
                {
                    ts = new ThreadStart(tdcycount);
                }
                else if (cmb_reportname.SelectedIndex == 4)  //国有土地使用权配置统计表.xls
                {
                    ts = new ThreadStart(gytdCount);
                }
                else if (cmb_reportname.SelectedIndex == 5)  //土地出成交地价款收入统计表
                {
                    ts = new ThreadStart(tdcycount);
                }



                Thread t = new Thread(ts);
                t.IsBackground = true;                
                t.Start();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo("请选择必选条件！");

                SkyMap.Net.Gui.WaitDialogHelper.Close();
            }

        }
        #region 建设用地供应情况表
        private void jsrdCount()
        {
            try
            {
                string[] rows = new string[4];
                rows[0] = "  ";
                rows[1] = " and  原土地用途='商业' ";
                rows[2] = " and  (原土地用途='工业'  or 原土地用途='仓储') ";
                rows[3] = " and  (原土地用途='商住' or 原土地用途='住宅' )";

                string[] cols = new string[5];

                cols[0] = "  count(project_id)  ";
                cols[1] = "  sum(  用地面积) ";
                cols[2] = @" sum(用地面积* case   isnumeric(容积率允许值标) 
                             when 1 then cast(容积率允许值标 as numeric(20,2)) 
                             else   case rtrim(容积率允许值标)  when ''  then  0.000  else 
                             cast(SUBSTRING( rtrim( 容积率允许值标),LEN(rtrim( 容积率允许值标))-2,3) as numeric(20,2)) end end)  ";
                cols[3] = "  sum(  用地面积)  ";
                cols[4] = "  sum(  成交价)  ";
                int[] cindex = new int[3];
                cindex[0] = 4;
                cindex[1] = 7;
                cindex[2] = 17;

                string sql = "";
                string where = "";
                string result = "";
                double current = 0;
                double unit = 0;//计算单位
                excelop = excelop == null ? new ExcelFromArrayList() : excelop;
                string title = chk是否累计.Checked ? "（累计）" : "";

                excelop.GetWorkBook(filename);
                excelop.SetBookTitle(year + "年第" + cmb_common.SelectedText + "季度国有建设用地供应情况" + title, excelop.GetSheet(1));
                string value = "";
                int rowIndex = 0;


                for (int i = 0; i < rows.Length; i++)
                {
                    rowIndex = 7;
                    rowIndex = i == 3 ? rowIndex + i + 1 : rowIndex + i;

                    for (int j = 0; j < cols.Length; j++)
                    {


                        where = j == 3 ? "  and 土地来源='新增'  " : "";
                        if (j != 0)
                        {
                            unit = 10000;
                        }
                        else unit = 1;
                        current = 0;
                        for (int k = 0; k <= index; k++)
                        {
                            sql = "select " + cols[j] + " from VW_YDCK where 业务类型=0  and 业务状态='完成竞价' and 是否成交=1 and (成交后状态!='违约' and 成交后状态!='违约业务')  and datepart(mm," + endDate + ") in (" + season[k] + ") and datepart(yy," + endDate + ")=" + year + rows[i] + where;
                            result = QueryHelper.ExecuteSqlScalar("", sql).ToString();
                            //累计

                            if (chk是否累计.Checked)
                            {

                                current += Math.Round(double.Parse(result == "" ? "0" : result) == 0 ? 0 : double.Parse(result) / unit, 2);

                            }
                            else  //不进行累计
                            {
                                sql = "select " + cols[j] + " from VW_YDCK where 业务类型=0  and 业务状态='完成竞价' and 是否成交=1 and (成交后状态!='违约' and 成交后状态!='违约业务')  and datepart(mm," + endDate + ") in (" + season[index] + ") and datepart(yy," + endDate + ")=" + year + rows[i] + where;
                                result = QueryHelper.ExecuteSqlScalar("", sql).ToString();

                                current = Math.Round(double.Parse(result == "" ? "0" : result) == 0 ? 0 : double.Parse(result) / unit, 2);
                                break;
                            }
                        }
                        value = current == 0 ? "" : current.ToString();
                        if (j == 0)
                        {
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[0] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[1] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[2] + j);
                        }
                        else if (j == 1)
                        {
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[0] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[1] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[2] + j);
                        }
                        else if (j == 2)
                        {
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[1] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[2] + j);
                        }
                        else if (j == 3)
                        {
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[0] + j - 1);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[1] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[2] + j);
                        }
                        else if (j == 4)
                        {
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[1] + j);
                            excelop.setCellValue(value, excelop.GetSheet(1), rowIndex, cindex[2] + j);
                        }
                    }

                }


                saveAndShowFile();

            }
            catch (Exception ex)
            {
                LoggingService.Debug("建设用地供应情况表统计出错" + ex.Message);
                MessageHelper.ShowError("建设用地供应情况表统计出错", ex);


            }
            finally
            {
                SkyMap.Net.Gui.WaitDialogHelper.Close();
            }


        }

        private void saveAndShowFile()
        {
            if (File.Exists(temp))
                File.Delete(temp);
            excelop.SaveAs(temp);
            excelop.GetWorkBook(temp);
            excelop.showExcel();
        }
        #endregion
        #region  国有土地使用权配置统计表

        private void gytdCount()
        {
            try
            {
                string[] cons = new string[2];
                cons[0] = " count(project_id) ";
                cons[1] = " sum( 用地面积) ";


                string[] wheres = new string[4];
                wheres[0] = "  ";
                wheres[1] = " and 交易方式=  'Z'  ";
                wheres[2] = "  and 交易方式=  'P'  ";
                wheres[3] = " and 交易方式=  'G'  ";


                string sql;
                double units = 1;  //计算单位
                double current = 0;
                double lj = 0;
                string result = "";
                ArrayList list = null;

                excelop = excelop == null ? new ExcelFromArrayList() : excelop;
                excelop.GetWorkBook(filename);
                excelop.replaceCellValue("xxxx年第x季度", year + "年第" + cmb_common.SelectedText + "季度", excelop.GetSheet(1), 2, 1);
                excelop.setCellValue("填表日期：" + DateTime.Now.ToString("D"), excelop.GetSheet(1), 20, 1);

                ArrayList rlists = null;

                for (int k = 0; k < wheres.Length; k++)
                {
                    rlists = new ArrayList();
                    list = new ArrayList();
                    for (int j = 0; j < cons.Length; j++)
                    {
                        lj = 0;
                        units = j == 1 ? 10000 : 1;

                        //合计
                        //累计
                        for (int i = 0; i <= index; i++)
                        {
                            sql = "select " + cons[j] + " from ywwftdpro where 业务类型=0  and (成交后状态!='违约' and 成交后状态!='违约业务') and 业务状态='完成竞价' and 是否成交=1 and datepart(mm," + endDate + ") in (" + season[i] + ") and datepart(yy," + endDate + ")=" + year + wheres[k];
                            result = QueryHelper.ExecuteSqlScalar("", sql).ToString();
                            if (index == i)
                                current = double.Parse(result == "" ? "0" : result) / units;
                            lj += double.Parse(result == "" ? "0" : result) / units;
                        }

                        list.Add(Math.Round(current, 2));
                        list.Add(Math.Round(lj, 2));

                    }
                    rlists.Add(list);

                    if (k == 0)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 6, 3, 1);
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 8, 3, 1);
                    }
                    else if (k == 1)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 10, 3, 1);

                    }
                    else if (k == 2)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 11, 3, 1);
                    }
                    else if (k == 3)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 12, 3, 1);
                    }

                }


                excelop.SaveAs(temp);
                excelop.GetWorkBook(temp);
                excelop.showExcel();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo("土地出让收入统计出错!");
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }
        }

        #endregion
        #region   房地产价格监测
        private void fdcPriceCount()
        {
            string[] tdrt = new string[4];
            tdrt[0] = "   原土地用途='住宅' ";
            tdrt[1] = "  原土地用途='商住' ";
            tdrt[2] = " (原土地用途='工业' or 原土地用途='仓储') ";
            tdrt[3] = " 原土地用途='商业' ";

            ArrayList cons = new ArrayList();
            cons.Add("   sum( 用地面积)   ");
            cons.Add(" sum( 成交价) ");
            cons.Add(" sum( 成交价)/sum( 用地面积) ");
            string sql = "";
            ArrayList results = new ArrayList();
            ArrayList cresult = null;
            string data = "";

            for (int i = 0; i < tdrt.Length; i++)
            {
                cresult = new ArrayList();
                for (int j = 0; j < cons.Count; j++)
                {
                    sql = "select " + cons[j] + "  from ywwftdpro where " + tdrt[i] + " and 业务状态='完成竞价' and 是否成交=1 and 业务类型=0 and (成交后状态!='违约' and 成交后状态!='违约业务') and datepart(mm," + endDate + ") in (" + season[index] + ") and datepart(yy," + endDate + ")=" + year + "";
                    data = QueryHelper.ExecuteSqlScalar("", sql).ToString();
                    cresult.Add(Math.Round(double.Parse(data == "" ? "0" : data), 2));

                }
                results.Add(cresult);
            }
            try
            {
                excelop = excelop == null ? new ExcelFromArrayList() : excelop;


                excelop.GetWorkBook(filename);
                string datey = "统计日期:" + year + "年第" + cmb_common.SelectedText + "季";
                excelop.setCellValue(datey, excelop.GetSheet(1), 2, 3);
                excelop.setCellValue("报告日期:" + DateTime.Now.ToString("d"), excelop.GetSheet(1), 9, 1);
                excelop.setCellValue(results, excelop.GetSheet(1), 4, 2, 1);
                saveAndShowFile();

            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo("文件生成错误！");
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }
        }
        #endregion
        #region  土地出让收入统计表
        private void tdcycount()
        {
            try
            {

                bool isflag=cmb_reportname.SelectedIndex == 5?true:false;
                string[] cons = new string[1];
                cons[0] = " sum(  出让金) ";
                if (isflag)
                {
                    cons[0] = "  sum( 成交价 ) ";
                }

                // cons[1] = "  ";


                string[] wheres = new string[4];
                wheres[0] = "  ";
                wheres[1] = " and 交易方式=  'Z'  ";
                wheres[2] = "  and 交易方式=  'P'  ";
                wheres[3] = " and 交易方式=  'G'  ";


                string sql;
                double units = 10000;  //计算单位
                double current = 0;
                double lj = 0;
                double lastlj = 0;
                string result = "";
                ArrayList list = null;

                excelop = excelop == null ? new ExcelFromArrayList() : excelop;
                excelop.GetWorkBook(filename);
                
                excelop.replaceCellValue("xxxx年第x季度", year + "年第" + cmb_common.SelectedText + "季度", excelop.GetSheet(1), 2, 1);
                excelop.setCellValue("填表日期：" + DateTime.Now.ToString("D"), excelop.GetSheet(1), 23, 1);
                ArrayList rlists = null;

                for (int k = 0; k < wheres.Length; k++)
                {
                    rlists = new ArrayList();
                    list = new ArrayList();
                    for (int j = 0; j < cons.Length; j++)
                    {
                        lj = 0;
                        lastlj = 0;

                        //合计
                        //累计
                        for (int i = 0; i <= index; i++)
                        {
                            sql = "select " + cons[j] + " from ywwftdpro where 业务类型=0 and 业务状态='完成竞价' and ((成交后状态!='违约' and 成交后状态!='违约业务') or 成交后状态 is null  ) and 是否成交=1  and datepart(mm," + endDate + ") in (" + season[i] + ") and datepart(yy," + endDate + ")=" + year + wheres[k];
                            if (isflag) { sql = sql + " and 成交后状态='土地交付中业务'  "; }
                            result = QueryHelper.ExecuteSqlScalar("", sql).ToString();
                            if (index == i)
                                current = double.Parse(result == "" ? "0" : result) / units;
                            lj += double.Parse(result == "" ? "0" : result) / units;
                        }

                        list.Add(Math.Round(current, 2));
                        list.Add(Math.Round(lj, 2));
                        lastlj = lj - current;

                        list.Add(Math.Round(lj != 0 ? (lastlj != 0 ? (lj / lastlj - 1) * 100 : 0) : 0, 2));


                        //实际缴纳价款;

                        lj = 0;
                        lastlj = 0;
                        string wh = isflag ? " and 成交后状态='土地交付中业务' " : " and (成交后状态='结案' or 成交后状态='结案业务' ) ";
                        for (int i = 0; i <= index; i++)
                        {
                            sql = "select " + cons[j] + " from ywwftdpro where 业务类型=0 and 业务状态='完成竞价' "+wh+" and 是否成交=1  and datepart(mm," + endDate + ") in (" + season[i] + ") and datepart(yy," + endDate + ")=" + year + wheres[k];
                            
                            result = QueryHelper.ExecuteSqlScalar("", sql).ToString();
                            if (index == i)
                                current = double.Parse(result == "" ? "0" : result) / units;
                            lj += double.Parse(result == "" ? "0" : result) / units;
                        }

                        list.Add(Math.Round(current, 2));
                        list.Add(Math.Round(lj, 2));
                        lastlj = lj - current;
                        list.Add(Math.Round(lj != 0 ? (lastlj != 0 ? (lj / lastlj - 1) * 100 : 0) : 0, 2));

                    }
                    rlists.Add(list);

                    if (k == 0)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 6, 3, 1);
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 8, 3, 1);
                    }
                    else if (k == 1)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 10, 3, 1);

                    }
                    else if (k == 2)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 11, 3, 1);
                    }
                    else if (k == 3)
                    {
                        excelop.setCellValue(rlists, excelop.GetSheet(1), 12, 3, 1);
                    }

                }


                excelop.SaveAs(temp);
                excelop.GetWorkBook(temp);
                excelop.showExcel();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo("土地出让收入统计出错!");
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }

        }
        #endregion
        #region   招拍挂报表

        #region  镇区
        ArrayList zqExpress = null;
        string zqwhere = "";
        private void zqProccessExcelData()
        {
            zqExpress = new ArrayList();
            //ArrayList zqExpress = new ArrayList();
            try
            {
                zqExpress.Add(" count(project_id) ");                   //已办业务宗数
                zqExpress.Add(" sum(  用地面积) ");              //面积
                zqExpress.Add(" sum(  交易底价)  ");            //底价
                zqExpress.Add(" sum(  交易底价)/sum(  用地面积)  ");   //底价平均单价
                zqExpress.Add(" sum(  成交价)   ");                //成交价
                zqExpress.Add(" sum(  成交价)/sum(  用地面积)   ");  //成交平均单价
                zqExpress.Add(" sum(  超底价金额)   ");    //超底价        
                string ymDate = cmb_year.SelectedText + "-" + (cmb_common.SelectedText == "" ? "1" : cmb_common.SelectedText) + "-1";
                string feild1 = "";
                string where1 = "";
                string where2 = "";
                zqwhere = " and 镇区='" + cmb_town.Properties.KeyValue + "' ";

                ArrayList lists = new ArrayList();

                //已办业务   
                where1 = "  and 业务状态='完成竞价' ";
                lists.Add(countData(ymDate, ref feild1, where1 + where2));


                //已办业务 中的成交业务
                where1 = "  and 业务状态='完成竞价' and 是否成交=1 and( 成交后状态!='违约' and 成交后状态!='违约业务' ) ";
                lists.Add(countData(ymDate, ref feild1, where1 + where2));


                //流拍以及后面所以的业务
                for (int i = 0; i < hexpressions.Count; i++)
                {

                    if (i == 3)
                    {
                        conDate = "现场竞价时间";
                    }
                    else if (i == 4)
                    {
                        conDate = "公告起始日期";
                    }
                    else if (i == 5)
                    {
                        conDate = "proinst_createdate";
                    }


                    where1 = hexpressions[i].ToString();
                    lists.Add(countData(ymDate, ref feild1, where1));

                    //出让 转让 //工业非工业 
                    ArrayList list = (quYangOrZhuan(ymDate, ref feild1, ref where1, ref where2));
                    for (int j = 0; j < list.Count; j++)
                    {
                        lists.Add(list[j]);
                    }
                }

                excelop = excelop == null ? new ExcelFromArrayList() : excelop;
                excelop.GetWorkBook(filename);
                excelop.SetBookTitle(year + "年" + cmb_common.SelectedText + "月招拍挂业务统计表(镇区)", excelop.GetSheet(1));
                excelop.setCellValue("镇区:" + cmb_town.Text, excelop.GetSheet(1), 2, 1);
                excelop.setCellValue(lists, excelop.GetSheet(1), 5, 7, 1);
                saveAndShowFile();
            }
            catch
            {
                MessageHelper.ShowInfo("镇区统计出错");
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }

        }



        #endregion
        private void proccessExcelData()
        {


            try
            {
                string ymDate = cmb_year.SelectedText + "-" + (cmb_common.SelectedText == "" ? "1" : cmb_common.SelectedText) + "-1";

                string feild1 = "";
                string where1 = "";
                string where2 = "";
                ArrayList lists = new ArrayList();



                //已办业务   
                where1 = "  and 业务状态='完成竞价' ";
                lists.Add(countData(ymDate, ref feild1, where1));

                //已办业务 中的成交业务
                where1 = "  and 业务状态='完成竞价' and 是否成交=1   and( (成交后状态!='违约' and  成交后状态!='违约业务') or 成交后状态 is null )  ";
                lists.Add(countData(ymDate, ref feild1, where1));


                for (int i = 0; i < hexpressions.Count; i++)
                {

                    if (i == 4)    //流拍
                    {
                        conDate = "现场竞价时间";
                    }
                    else if (i == 5)   //公告
                    {
                        conDate = "公告起始日期";
                    }
                    else if (i == 6)  //审批中
                    {
                        conDate = "proinst_createdate";
                    }


                    where1 = hexpressions[i].ToString();
                    lists.Add(countData(ymDate, ref feild1, where1));



                    //出让 转让 //工业非工业 
                    ArrayList list = (quYangOrZhuan(ymDate, ref feild1, ref where1, ref where2));
                    for (int j = 0; j < list.Count; j++)
                    {
                        lists.Add(list[j]);
                    }
                }

                excelop = new ExcelFromArrayList();
                excelop.GetWorkBook(filename);
                //月
                string title = cmb_year.SelectedText + "年第" + cmb_common.SelectedText + this.label8.Text + "度招拍挂业务统计表";
                if (cmb_reportType.SelectedIndex == 3)
                {
                    title = cmb_year.SelectedText + "年度招拍挂业务统计表";
                }
                excelop.SetBookTitle(title, excelop.GetSheet(1));
                excelop.setCellValue(lists, excelop.GetSheet(1), 5, 7, 1);

                saveAndShowFile();


            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("生成报表错误!", ex);
            }
            finally
            {
                // SkyMap.Net.Gui.WaitDialogHelper.Close();
                this.Invoke(new CloseDialog(closeDialog), null);

            }
        }
        #region 出让 与非出让
        /// <summary>
        /// 出让 与非出让
        /// </summary>
        /// <param name="ymDate">日期</param>
        /// <param name="feild1">字段</param>
        /// <param name="where1">条件1</param>
        /// <param name="where2">条件1</param>
        /// <returns> arrayList 数组 </returns>
        private ArrayList quYangOrZhuan(string ymDate, ref string feild1, ref string where1, ref string where2)
        {
            ArrayList listy = new ArrayList();

            try
            {
                // 出让 0
                where1 += " and 业务类型=0  ";
                listy.Add(countData(ymDate, ref feild1, where1));
                if (!isZq)
                {
                    ArrayList lists = gongyeYN(ymDate, ref feild1, where1, ref where2);
                    listy.Add(lists[0]); listy.Add(lists[1]);
                }



                // 转让 9
                where1 = where1.Replace("业务类型=0", "业务类型=9");
                listy.Add(countData(ymDate, ref feild1, where1));
                if (!isZq)
                {
                    ArrayList listsn = gongyeYN(ymDate, ref feild1, where1, ref where2);
                    listy.Add(listsn[0]); listy.Add(listsn[1]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "统计出让 转让出错");
            }

            return listy;

        }

        #endregion
        #region   非工业  工业
        /// <summary>
        /// 非工业  工业
        /// </summary>
        /// <param name="ymDate"></param>
        /// <param name="feild1"></param>
        /// <param name="where1"></param>
        /// <param name="where2"></param>
        /// <returns></returns>
        private ArrayList gongyeYN(string ymDate, ref string feild1, string where1, ref string where2)
        {
            //   //在办业务 转让 =9  非工业=N

            ArrayList list = new ArrayList();

            try
            {
                where2 = " and  版本='N'";
                list.Add(countData(ymDate, ref feild1, where1 + where2));

                //   //在办业务 出让 =0  工业=Y
                where2 = " and  版本='Y'";
                list.Add(countData(ymDate, ref feild1, where1 + where2));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "统计工业 非工业出错!");
            }
            return list;
        }
        #endregion
        #region 计算列的值;

        /// <summary>
        /// 计算列的值;
        /// </summary>
        /// <param name="ymDate">日期</param>
        /// <param name="feild1">字段</param>
        /// <param name="where1">条件</param>
        /// <returns>arraylist 数组</returns>
        private ArrayList countData(string ymDate, ref string feild1, string where1)
        {
            ArrayList list = new ArrayList();
            expressions = isZq ? zqExpress : expressions;
            for (int i = 0; i < expressions.Count; i++)
            {
                feild1 = expressions[i].ToString();
                double[] result = getDate(feild1, where1, ymDate);
                if (isZq)
                {
                    if (result[0] != 0)
                        result[0] = Math.Round(result[0], 6);
                    list.Add(result[0]);
                }
                else
                {
                    for (int j = 0; j < result.Length; j++)
                    {
                        if (result[j] != 0)
                            result[j] = Math.Round(result[j], 6);

                        if (j == 0)
                        {

                            list.Add(result[j]);
                        }
                        else
                        {
                            if (result[j] < 0)
                            {
                                list.Add(result[j] + "↓");
                            }
                            else if (result[j] > 0)
                            {
                                list.Add(result[j] + "↑");
                            }
                            else
                            {
                                list.Add(result[j]);
                            }
                        }

                    }
                }
            }
            return list;
        }

        #endregion
        #region 判断流程
        /// <summary>
        /// 当月  去年同期   上月 的对比
        /// </summary>
        /// <param name="feild1"></param>
        /// <param name="where"></param>
        /// <param name="ymDate"></param>
        /// <returns></returns>
        private double[] getDate(string feild1, string where, string ymDate)
        {

            if (cmb_reportType.SelectedIndex == 0)  //月
                return getMonth(feild1, where, ymDate);
            else if (cmb_reportType.SelectedIndex == 1)
                return getSeason(feild1, where, ymDate);  //季度 半年
            else
            {
                return getYearData(feild1, where, ymDate);  //年    
            }
        }

        #endregion
        #region  计算月
        /// <summary>
        /// 计算月
        /// </summary>
        /// <param name="feild1"></param>
        /// <param name="where"></param>
        /// <param name="ymDate"></param>
        /// <returns></returns>
        private double[] getMonth(string feild1, string where, string ymDate)
        {

            double[] data = new double[3];
            try
            {


                //当 月
                StringBuilder sbsqlsum = new StringBuilder();
                sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + zqwhere + " ");

                sbsqlsum.Append(" and datediff(mm," + conDate + ",cast('" + ymDate + "'as datetime))=0");

                String value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();
                if (value != "" && double.Parse(value) > 0)
                    data[0] = double.Parse(value.ToString());
                if (!isZq) //判断是否镇区   是镇区就不用核算该部分值
                {
                    ///去年同期
                    sbsqlsum.Remove(0, sbsqlsum.Length);
                    sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");
                    sbsqlsum.Append(" and datediff(mm," + conDate + ",dateadd(yy,-1,cast('" + ymDate + "'as datetime)))=0");
                    value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();

                    if (value != "" && double.Parse(value) > 0)
                        data[1] = getPercentData(data, value);

                    //上月
                    sbsqlsum.Remove(0, sbsqlsum.Length);
                    sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");
                    sbsqlsum.Append(" and datediff(mm," + conDate + ",dateadd(mm,-1,cast('" + ymDate + "'as datetime)))=0");
                    value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();
                    if (value != "" && double.Parse(value) > 0)
                        data[2] = Math.Round(getPercentData(data, value), 2);
                }

            }
            catch (Exception ex)
            {

                // throw new Exception(ex.Message + "统计当月/去年出错!");

            }
            return data;
        }

        #endregion
        #region  计算公式;
        /// <summary>
        /// 计算公式;
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private double getPercentData(double[] data, String value)
        {
            return data[0] > 0 ? (data[0] / double.Parse(value.ToString()) - 1) * 100 : 0;
        }

        #endregion
        #region  计算年
        /// <summary>
        /// 计算年
        /// </summary>
        /// <param name="feild1"></param>
        /// <param name="where"></param>
        /// <param name="ymDate"></param>
        /// <returns></returns>
        private double[] getYearData(string feild1, string where, string ymDate)
        {

            double[] data = new double[3];
            try
            {
                //当 年
                StringBuilder sbsqlsum = new StringBuilder();
                sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");

                sbsqlsum.Append(" and datediff(yy," + conDate + ",cast('" + ymDate + "'as datetime))=0");

                String value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();
                if (value != "" && double.Parse(value) > 0)
                    data[0] = double.Parse(value.ToString());

                ///去年同期
                sbsqlsum.Remove(0, sbsqlsum.Length);
                sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");
                sbsqlsum.Append(" and datediff(yy," + conDate + ",dateadd(yy,-1,cast('" + ymDate + "'as datetime)))=0");
                value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();

                if (value != "" && double.Parse(value) > 0)
                    data[1] = getPercentData(data, value);

                //上月
                //sbsqlsum.Remove(0, sbsqlsum.Length);
                //sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");
                //sbsqlsum.Append(" and datediff(mm,proinst_createdate,dateadd(mm,-1,cast('" + ymDate + "'as datetime)))=0");
                //value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();
                //if (value != "" && int.Parse(value) > 0)
                //    data[2] = getPercentData(data, value);

            }
            catch (Exception ex)
            {

                // throw new Exception(ex.Message + "统计当月/去年出错!");

            }
            return data;
        }

        #endregion
        #region 季度  半年
        /// <summary>
        /// 季度  半年
        /// </summary>
        /// <param name="feild1"></param>
        /// <param name="where"></param>
        /// <param name="ymDate"></param>
        /// <returns></returns>
        /// 


        private double[] getSeason(string feild1, string where, string ymDate)
        {



            double[] data = new double[3];
            try
            {

                int year2 = year;
                int index2 = 0;

                if (cmb_reportType.SelectedIndex == 1 && cmb_common.SelectedText == "1")  //季
                {
                    year2 = year - 1;
                    index2 = index + 3;
                }

                if (cmb_reportType.SelectedIndex == 2 && cmb_common.SelectedText == "1")  //半年
                {
                    year2 = year - 1;
                    index = 4;
                    index2 = 5;
                }
                if (cmb_reportType.SelectedIndex == 2 && cmb_common.SelectedText == "2")  //半年
                {
                    index2 = 4;
                    index = 5;
                }


                //当 季  半年
                StringBuilder sbsqlsum = new StringBuilder();
                sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");

                sbsqlsum.Append(" and  datepart(mm," + conDate + ") in (" + season[index] + ") and datepart(yy," + conDate + ")=" + year + "");

                String value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();
                if (value != "")
                    data[0] = double.Parse(value.ToString());

                ///去年同期
                sbsqlsum.Remove(0, sbsqlsum.Length);
                sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");
                sbsqlsum.Append("  and datepart(mm," + conDate + ") in (" + season[index] + ") and datepart(yy," + conDate + ")=" + (year - 1) + "");
                value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();

                if (value != "" && double.Parse(value) > 0)
                    data[1] = getPercentData(data, value);

                //上季 半年
                sbsqlsum.Remove(0, sbsqlsum.Length);
                sbsqlsum.Append("select " + feild1 + " from ywwftdpro where 1=1   " + where + " ");
                sbsqlsum.Append(" and datepart(mm," + conDate + ") in (" + season[index2] + ") and datepart(yy," + conDate + ")=" + year2 + "");
                value = QueryHelper.ExecuteSqlScalar("", sbsqlsum.ToString()).ToString();
                if (value != "" && double.Parse(value) > 0)
                    data[2] = getPercentData(data, value);


            }
            catch (Exception ex)
            {

                // throw new Exception(ex.Message + "统计当月/去年出错!");

            }
            return data;
        }
        #endregion
        #endregion

        #region 显示
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.cmb_year.BringToFront();
            this.cmb_year.Width = 56;
            if (this.cmb_reportType.SelectedIndex == 0)   //月度报表 
            {
                setCmbDisplay(12, "月");
            }
            else if (cmb_reportType.SelectedIndex == 1)  //季度
            {
                setCmbDisplay(4, "季");
            }
            else if (cmb_reportType.SelectedIndex == 2)  // 半年
            {
                setCmbDisplay(2, "半年");
            }
            else if (cmb_reportType.SelectedIndex == 3) //全年
            {
                this.cmb_year.Width = 100;
                setCmbDisplay(0, "年");
            }
        }
        private void setCmbDisplay(int flag, string lableText)
        {
            cmb_common.Properties.Items.Clear();
            this.label8.Text = lableText;
            for (int i = 1; i <= flag; i++)
            {
                cmb_common.Properties.Items.Add(i);
            }
        }

        private void cmb_town_EditValueChanged(object sender, EventArgs e)
        {
            this.cmb_reportType.Enabled = false;
        }

        private void cmb_reportname_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmb_reportType.Enabled = true;
            cmb_town.Enabled = true;
            this.cmb_reportType.SelectedIndex = 0;
            if (cmb_reportname.SelectedIndex == 1)
            {
                chk是否累计.Visible = true;

            }
            else
            {
                chk是否累计.Checked = false;
                chk是否累计.Visible = false;

            }

            if (cmb_reportname.SelectedIndex != 0)
            {
                // cmb_town.Refresh();
                cmb_town.Reset();
                cmb_town.ItemIndex = -1;
                this.cmb_town.Enabled = false;

                this.cmb_reportType.SelectedIndex = 1;
                this.cmb_reportType.Enabled = false;
            }
        }



        #endregion

    }
}
