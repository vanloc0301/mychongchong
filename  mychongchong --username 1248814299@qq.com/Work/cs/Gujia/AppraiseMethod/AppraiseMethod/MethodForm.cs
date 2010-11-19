using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Taramon.Exceller;
using AppraiseMethod.Excel;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Threading;
using DevExpress.XtraVerticalGrid.Rows;

namespace AppraiseMethod
{
    public partial class MethodForm : Form
    {
        private string strprojectid;
        private Thread invokeThread;
        private delegate void invokeDelegate();

        static DateTime beforeTime;            //Excel启动之前时间
        static DateTime afterTime;
        double qph;
        ILand gy;
        LandInput li = new LandInput();
        ExcelMapper mapper = new ExcelMapper();
        // string excelFileName = Path.Combine(Directory.GetCurrentDirectory(), @"D:\Hi\cs\AppraiseMethod\AppraiseMethod\房地产土地估价.xls");
        string excelFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\addins\AppraiseMethod\房地产土地估价.xls";
        //市场比较法中标记是否显示
        bool showCustomization = false;

        DataSet ds;
        Dictionary<string, string> diction;
        public MethodForm()
        {
            InitializeComponent();
        }

        public MethodForm(string projectid)
        {
            InitializeComponent();
            this.strprojectid = projectid;

        }

        public String Project_Id
        {
            get
            {
                return this.strprojectid;
            }
        }

        public Double Qph
        {
            set { this.qph = value; }
            get { return this.qph; }
        }

        private void StartMethod()
        {
            beforeTime = DateTime.Now;
            li.Qph = this.qph.ToString();
            // mapper.Write(li, @"c:\aaa.xls",excelFileName);
            // mapper.Write(li, excelFileName, true);
            try
            {
                //=====================
                if (!string.IsNullOrEmpty(li.Type))
                {
                    if (li.Type == "工业")
                    {
                        gy = new LandGy();
                        if (this.landDatumValue.gyszys.Count == 1)
                        {
                            #region 工业具体条件
                            ArrayList[] al = new ArrayList[11];
                            ArrayList tmpal = new ArrayList();
                            tmpal.Clear();
                            int j = 0;
                            for (int i = 1; i < 56; i++)
                            {
                                tmpal.Add(this.landDatumValue.gyszys.Rows[0][string.Format("s{0}", i + 1)].ToString());
                                if (i % 5 == 0 && i != 0)
                                {
                                    al[j] = new ArrayList();
                                    for (int t = 0; t < 5; t++)
                                    {
                                        al[j].Add(tmpal[t]);
                                    }
                                    tmpal.Clear();
                                    j++;
                                }

                            }
                            ((LandGy)gy).Range_Jttj_1 = al[0];
                            ((LandGy)gy).Range_Jttj_2 = al[1];
                            ((LandGy)gy).Range_Jttj_3 = al[2];
                            ((LandGy)gy).Range_Jttj_4 = al[3];
                            ((LandGy)gy).Range_Jttj_5 = al[4];
                            ((LandGy)gy).Range_Jttj_6 = al[5];
                            ((LandGy)gy).Range_Jttj_7 = al[6];
                            ((LandGy)gy).Range_Jttj_8 = al[7];
                            ((LandGy)gy).Range_Jttj_9 = al[8];
                            ((LandGy)gy).Range_Jttj_10 = al[9];
                            ((LandGy)gy).Range_Jttj_11 = al[10];
                            #endregion
                            #region 工业具体修正系数
                            ArrayList[] al1 = new ArrayList[11];
                            ArrayList tmpal1 = new ArrayList();
                            tmpal1.Clear();
                            int j1 = 0;
                            for (int i = 1; i < 56; i++)
                            {
                                tmpal1.Add(this.landDatumValue.gyszxs.Rows[0][string.Format("s{0}", i + 1)].ToString());
                                if (i % 5 == 0 && i != 0)
                                {
                                    al1[j1] = new ArrayList();
                                    for (int t = 0; t < 5; t++)
                                    {
                                        al1[j1].Add(tmpal1[t]);
                                    }
                                    tmpal1.Clear();
                                    j1++;
                                }

                            }
                            ((LandGy)gy).Range_Xzxs_1 = al1[0];
                            ((LandGy)gy).Range_Xzxs_2 = al1[1];
                            ((LandGy)gy).Range_Xzxs_3 = al1[2];
                            ((LandGy)gy).Range_Xzxs_4 = al1[3];
                            ((LandGy)gy).Range_Xzxs_5 = al1[4];
                            ((LandGy)gy).Range_Xzxs_6 = al1[5];
                            ((LandGy)gy).Range_Xzxs_7 = al1[6];
                            ((LandGy)gy).Range_Xzxs_8 = al1[7];
                            ((LandGy)gy).Range_Xzxs_9 = al1[8];
                            ((LandGy)gy).Range_Xzxs_10 = al1[9];
                            ((LandGy)gy).Range_Xzxs_11 = al1[10];
                            #endregion
                            #region 修正项名称
                            ArrayList tmpalxz = new ArrayList();
                            tmpalxz.Clear();
                            tmpalxz.AddRange(new string[] { "1、区域因素_交通条件_道路通达度", "2、区域因素_交通条件_离港口距离", "3、区域因素_基础设施_供电保证率", "4、区域因素_基础设施_供水保证率", "5、区域因素_基础设施_排水状况", 
                                "6、区域因素_产业聚集程度","7、区域因素_城市规划状况","8、个别因素_宗地面积","9、个别因素_地形条件","10、个别因素_地质状况与地基承载力","11、个别因素_宗地形状"}.ToList<string>());
                            if (tmpalxz.Count != 11) throw new Exception("工业修正需要11项");
                            ((LandGy)gy).Range_Xz = tmpalxz;
                            #endregion
                            #region 修正项等级 eg:优，良...
                            ArrayList tmpalxzsz = new ArrayList();
                            tmpalxzsz.Clear();
                            tmpalxzsz.AddRange(new string[] { "优", "较优", "一般", "较劣", "劣" }.ToList<string>());
                            if (tmpalxzsz.Count != 5) throw new Exception("工业修正项等级需要5项");
                            ((LandGy)gy).Range_Xzsz = tmpalxzsz;
                            #endregion
                        }

                    }
                    else if (li.Type == "商住")
                    {
                        gy = new LandSz();
                        if (this.landDatumValue.szszys.Count == 1)
                        {
                            #region 商住具体条件
                            ArrayList[] al = new ArrayList[23];
                            ArrayList tmpal = new ArrayList();
                            tmpal.Clear();
                            int j = 0;
                            for (int i = 1; i < 116; i++)
                            {
                                tmpal.Add(this.landDatumValue.szszys.Rows[0][string.Format("s{0}", i + 1)].ToString());
                                if (i % 5 == 0 && i != 0)
                                {
                                    al[j] = new ArrayList();
                                    for (int t = 0; t < 5; t++)
                                    {
                                        al[j].Add(tmpal[t]);
                                    }
                                    tmpal.Clear();
                                    j++;
                                }

                            }
                            ((LandSz)gy).Range_Jttj_1 = al[0];
                            ((LandSz)gy).Range_Jttj_2 = al[1];
                            ((LandSz)gy).Range_Jttj_3 = al[2];
                            ((LandSz)gy).Range_Jttj_4 = al[3];
                            ((LandSz)gy).Range_Jttj_5 = al[4];
                            ((LandSz)gy).Range_Jttj_6 = al[5];
                            ((LandSz)gy).Range_Jttj_7 = al[6];
                            ((LandSz)gy).Range_Jttj_8 = al[7];
                            ((LandSz)gy).Range_Jttj_9 = al[8];
                            ((LandSz)gy).Range_Jttj_10 = al[9];
                            ((LandSz)gy).Range_Jttj_11 = al[10];
                            ((LandSz)gy).Range_Jttj_12 = al[11];
                            ((LandSz)gy).Range_Jttj_13 = al[12];
                            ((LandSz)gy).Range_Jttj_14 = al[13];
                            ((LandSz)gy).Range_Jttj_15 = al[14];
                            ((LandSz)gy).Range_Jttj_16 = al[15];
                            ((LandSz)gy).Range_Jttj_17 = al[16];
                            ((LandSz)gy).Range_Jttj_18 = al[17];
                            ((LandSz)gy).Range_Jttj_19 = al[18];
                            ((LandSz)gy).Range_Jttj_20 = al[19];
                            ((LandSz)gy).Range_Jttj_21 = al[20];
                            ((LandSz)gy).Range_Jttj_22 = al[21];
                            ((LandSz)gy).Range_Jttj_23 = al[22];
                            #endregion
                            #region 商住具体修正系数
                            ArrayList[] al1 = new ArrayList[23];
                            ArrayList tmpal1 = new ArrayList();
                            tmpal1.Clear();
                            int j1 = 0;
                            for (int i = 1; i < 116; i++)
                            {
                                tmpal1.Add(this.landDatumValue.szszxs.Rows[0][string.Format("s{0}", i + 1)].ToString());
                                if (i % 5 == 0 && i != 0)
                                {
                                    al1[j1] = new ArrayList();
                                    for (int t = 0; t < 5; t++)
                                    {
                                        al1[j1].Add(tmpal1[t]);
                                    }
                                    tmpal1.Clear();
                                    j1++;
                                }

                            }
                            ((LandSz)gy).Range_Xzxs_1 = al1[0];
                            ((LandSz)gy).Range_Xzxs_2 = al1[1];
                            ((LandSz)gy).Range_Xzxs_3 = al1[2];
                            ((LandSz)gy).Range_Xzxs_4 = al1[3];
                            ((LandSz)gy).Range_Xzxs_5 = al1[4];
                            ((LandSz)gy).Range_Xzxs_6 = al1[5];
                            ((LandSz)gy).Range_Xzxs_7 = al1[6];
                            ((LandSz)gy).Range_Xzxs_8 = al1[7];
                            ((LandSz)gy).Range_Xzxs_9 = al1[8];
                            ((LandSz)gy).Range_Xzxs_10 = al1[9];
                            ((LandSz)gy).Range_Xzxs_11 = al1[10];
                            ((LandSz)gy).Range_Xzxs_12 = al1[11];
                            ((LandSz)gy).Range_Xzxs_13 = al1[12];
                            ((LandSz)gy).Range_Xzxs_14 = al1[13];
                            ((LandSz)gy).Range_Xzxs_15 = al1[14];
                            ((LandSz)gy).Range_Xzxs_16 = al1[15];
                            ((LandSz)gy).Range_Xzxs_17 = al1[16];
                            ((LandSz)gy).Range_Xzxs_18 = al1[17];
                            ((LandSz)gy).Range_Xzxs_19 = al1[18];
                            ((LandSz)gy).Range_Xzxs_20 = al1[19];
                            ((LandSz)gy).Range_Xzxs_21 = al1[20];
                            ((LandSz)gy).Range_Xzxs_22 = al1[21];
                            ((LandSz)gy).Range_Xzxs_23 = al1[22];
                            #endregion
                            #region 修正项名称
                            ArrayList tmpalxz = new ArrayList();
                            tmpalxz.Clear();
                            tmpalxz.AddRange(new string[] { "1、区域因素_繁华程度_商服中心距离", "2、区域因素_交通条件_道路通达度" ,"3、区域因素_基本设施状况_供电保证率","4、区域因素_基本设施状况_供水保证率","5、区域因素_基本设施状况_排水状况","6、区域因素_基本设施状况_公园距离","7、区域因素_基本设施状况_体育场馆距离", 
                            "8、区域因素_基本设施状况_中学距离","9、区域因素_基本设施状况_小学距离","10、区域因素_基本设施状况_幼儿园距离","11、区域因素_环境优劣度_治安状况","12、区域因素_环境优劣度_绿化状况","13、区域因素_环境优劣度_大气污染","14、区域因素_环境优劣度_噪声污染","15、区域因素_人口状况_人口密度","16、区域因素_城市规划_城市规划状况","17、个别因素_临街状况","18、个别因素_临街深度","19、个别因素_宽深比","20、个别因素_宗地面积","21、个别因素_宗地形状","22、个别因素_地形条件","23、个别因素_地质状况与地基承载力"}.ToList<string>());
                            if (tmpalxz.Count != 23) throw new Exception("商住修正需要23项");
                            ((LandSz)gy).Range_Xz = tmpalxz;
                            #endregion
                            #region 修正项等级 eg:优，良...
                            ArrayList tmpalxzsz = new ArrayList();
                            tmpalxzsz.Clear();
                            tmpalxzsz.AddRange(new string[] { "优", "较优", "一般", "较劣", "劣" }.ToList<string>());
                            if (tmpalxzsz.Count != 5) throw new Exception("商住修正项等级需要5项");
                            ((LandSz)gy).Range_Xzsz = tmpalxzsz;
                            #endregion
                        }
                    }
                    else if (li.Type == "住宅")
                    {
                        gy = new LandZz();
                        if (this.landDatumValue.zzszys.Count == 1)
                        {
                            #region 住宅具体条件
                            ArrayList[] al = new ArrayList[19];
                            ArrayList tmpal = new ArrayList();
                            tmpal.Clear();
                            int j = 0;
                            for (int i = 1; i < 96; i++)
                            {
                                tmpal.Add(this.landDatumValue.zzszys.Rows[0][string.Format("s{0}", i + 1)].ToString());
                                if (i % 5 == 0 && i != 0)
                                {
                                    al[j] = new ArrayList();
                                    for (int t = 0; t < 5; t++)
                                    {
                                        al[j].Add(tmpal[t]);
                                    }
                                    tmpal.Clear();
                                    j++;
                                }

                            }
                            ((LandZz)gy).Range_Jttj_1 = al[0];
                            ((LandZz)gy).Range_Jttj_2 = al[1];
                            ((LandZz)gy).Range_Jttj_3 = al[2];
                            ((LandZz)gy).Range_Jttj_4 = al[3];
                            ((LandZz)gy).Range_Jttj_5 = al[4];
                            ((LandZz)gy).Range_Jttj_6 = al[5];
                            ((LandZz)gy).Range_Jttj_7 = al[6];
                            ((LandZz)gy).Range_Jttj_8 = al[7];
                            ((LandZz)gy).Range_Jttj_9 = al[8];
                            ((LandZz)gy).Range_Jttj_10 = al[9];
                            ((LandZz)gy).Range_Jttj_11 = al[10];
                            ((LandZz)gy).Range_Jttj_12 = al[11];
                            ((LandZz)gy).Range_Jttj_13 = al[12];
                            ((LandZz)gy).Range_Jttj_14 = al[13];
                            ((LandZz)gy).Range_Jttj_15 = al[14];
                            ((LandZz)gy).Range_Jttj_16 = al[15];
                            ((LandZz)gy).Range_Jttj_17 = al[16];
                            ((LandZz)gy).Range_Jttj_18 = al[17];
                            ((LandZz)gy).Range_Jttj_19 = al[18];
                            #endregion
                            #region 住宅具体修正系数
                            ArrayList[] al1 = new ArrayList[19];
                            ArrayList tmpal1 = new ArrayList();
                            tmpal1.Clear();
                            int j1 = 0;
                            for (int i = 1; i < 96; i++)
                            {
                                tmpal1.Add(this.landDatumValue.zzszxs.Rows[0][string.Format("s{0}", i + 1)].ToString());
                                if (i % 5 == 0 && i != 0)
                                {
                                    al1[j1] = new ArrayList();
                                    for (int t = 0; t < 5; t++)
                                    {
                                        al1[j1].Add(tmpal1[t]);
                                    }
                                    tmpal1.Clear();
                                    j1++;
                                }

                            }
                            ((LandZz)gy).Range_Xzxs_1 = al1[0];
                            ((LandZz)gy).Range_Xzxs_2 = al1[1];
                            ((LandZz)gy).Range_Xzxs_3 = al1[2];
                            ((LandZz)gy).Range_Xzxs_4 = al1[3];
                            ((LandZz)gy).Range_Xzxs_5 = al1[4];
                            ((LandZz)gy).Range_Xzxs_6 = al1[5];
                            ((LandZz)gy).Range_Xzxs_7 = al1[6];
                            ((LandZz)gy).Range_Xzxs_8 = al1[7];
                            ((LandZz)gy).Range_Xzxs_9 = al1[8];
                            ((LandZz)gy).Range_Xzxs_10 = al1[9];
                            ((LandZz)gy).Range_Xzxs_11 = al1[10];
                            ((LandZz)gy).Range_Xzxs_12 = al1[11];
                            ((LandZz)gy).Range_Xzxs_13 = al1[12];
                            ((LandZz)gy).Range_Xzxs_14 = al1[13];
                            ((LandZz)gy).Range_Xzxs_15 = al1[14];
                            ((LandZz)gy).Range_Xzxs_16 = al1[15];
                            ((LandZz)gy).Range_Xzxs_17 = al1[16];
                            ((LandZz)gy).Range_Xzxs_18 = al1[17];
                            ((LandZz)gy).Range_Xzxs_19 = al1[18];
                            #endregion
                            #region 修正项名称
                            ArrayList tmpalxz = new ArrayList();
                            tmpalxz.Clear();
                            tmpalxz.AddRange(new string[] { "1、区域因素_繁华程度_商服中心距离", "2、区域因素_交通条件_道路通达度", "3、区域因素_基本设施状况_供电保证率", "4、区域因素_基本设施状况_供水保证率", "5、区域因素_基本设施状况_排水状况", "6、区域因素_基本设施状况_公园距离", "7、区域因素_基本设施状况_体育场馆距离", "8、区域因素_基本设施状况_中学距离", "9、区域因素_基本设施状况_小学距离", "10、区域因素_基本设施状况_幼儿园距离", "11、区域因素_环境优劣度_治安状况", "12、区域因素_环境优劣度_绿化状况", "13、区域因素_环境优劣度_大气污染", "14、区域因素_环境优劣度_噪声污染", "15、个别因素_城市规划", "16、个别因素_宗地面积", "17、个别因素_宗地形状", "18、个别因素_地形条件", "19、个别因素_地质条件" }.ToList<string>());
                            if (tmpalxz.Count != 19) throw new Exception("住宅修正需要19项");
                            ((LandZz)gy).Range_Xz = tmpalxz;
                            #endregion
                            #region 修正项等级 eg:优，良...
                            ArrayList tmpalxzsz = new ArrayList();
                            tmpalxzsz.Clear();
                            tmpalxzsz.AddRange(new string[] { "优", "较优", "一般", "较劣", "劣" }.ToList<string>());
                            if (tmpalxzsz.Count != 5) throw new Exception("住宅修正项等级需要5项");
                            ((LandZz)gy).Range_Xzsz = tmpalxzsz;
                            #endregion
                        }
                    }
                    else
                    {

                    }
                    mapper.Write(gy, @"c:\tmp.xls", excelFileName);
                    //=====================
                    //try
                    //{
                    //    if (gy != null)
                    //    {
                    //        mapper.Read(gy, excelFileName);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception(ex.Message.ToString());
                    //}
                }
                this.Invoke(new invokeDelegate(UpdateUI));
            }
            finally
            {
                //取消杀excel进程
                //afterTime = DateTime.Now;
                //KillExcel.KillExcelProcess(beforeTime, afterTime);
            }
            //try
            //{

            //}
            //catch(Exception ex) 
            //{
            //    throw new Exception(ex.Message.ToString());
            //}
        }

        private void UpdateUI()
        {
            try
            {
                //=====================
                //if (li.Qph_From == li.Qph)
                //{
                if (li.Type == "工业")
                {
                    chkbox工业.Checked = true;
                    chkbox商住.Checked = false;
                    chkbox别墅.Checked = false;
                    chkbox住宅.Checked = false;
                }
                else if (li.Type == "商住")
                {
                    chkbox商住.Checked = true;
                    chkbox工业.Checked = false;
                    chkbox别墅.Checked = false;
                    chkbox住宅.Checked = false;
                }
                else if (li.Type == "住宅")
                {

                }
                else
                {
                    chkbox别墅.Checked = false;
                    chkbox工业.Checked = false;
                    chkbox商住.Checked = false;
                    chkbox住宅.Checked = false;
                }

                if (li.Type == "住宅")
                {
                    if (chkbox住宅.Checked && chkbox别墅.Checked)
                    {
                        MessageBox.Show("请选择住宅或别墅，您选择了两项!");
                        return;
                    }
                    if (!chkbox住宅.Checked && !chkbox别墅.Checked)
                    {
                        MessageBox.Show("请选择住宅，别墅其中之一");
                        return;
                    }
                }
                if (li.Type == "住宅")
                {
                    this.txtTdhyll.Text = Convert.ToString(0.0675);
                    this.txtTdsdnx.Text = "70";
                    if (chkbox住宅.Checked)
                    {
                        this.rjlszTableAdapter1.Fill(this.landDatumValue.rjlsz, "住宅");
                    }
                    else if (chkbox别墅.Checked)
                    {
                        this.rjlszTableAdapter1.Fill(this.landDatumValue.rjlsz, "别墅");
                    }
                }
                if (li.Type == "工业")
                {
                    this.txtTdhyll.Text = Convert.ToString(0.07);
                    this.txtTdsdnx.Text = "50";
                    this.rjlszTableAdapter1.Fill(this.landDatumValue.rjlsz, "工业");

                }
                if (li.Type == "商住")
                {
                    this.txtTdhyll.Text = Convert.ToString(0.0625);
                    this.txtTdsdnx.Text = "70";
                    this.rjlszTableAdapter1.Fill(this.landDatumValue.rjlsz, "商住");
                }
                //=====================
                try
                {
                    if (gy != null)
                    {
                        InitGrid();
                        #region 在数据库中存在数据
                        if (ds != null && diction != null)
                        {
                            if (this.ds.Tables.Count == 1)
                            {
                                DataRow dr;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    dr = ds.Tables[0].Rows[i];
                                    string tmp = "";
                                    if (diction.TryGetValue(dr["修正"].ToString(), out tmp))
                                    {
                                        object tmpobj;
                                        int tmpint = 0;
                                        tmpobj = (object)gy.GetType().InvokeMember(string.Format("Range_Jttj_{0}", i + 1), BindingFlags.GetProperty, null, gy, null);
                                        ArrayList al = (ArrayList)tmpobj;
                                        if (al.Contains(tmp))
                                        {
                                            tmpint = al.IndexOf(tmp);
                                            dr["具体条件"] = al[al.IndexOf(tmp)].ToString();

                                            tmpobj = (object)gy.GetType().InvokeMember("Range_Xzsz", BindingFlags.GetProperty, null, gy, null);

                                            al = (ArrayList)tmpobj;
                                            dr["修正选择"] = al[tmpint].ToString();

                                            tmpobj = (object)gy.GetType().InvokeMember(string.Format("Range_Xzxs_{0}", i + 1), BindingFlags.GetProperty, null, gy, null);
                                            al = (ArrayList)tmpobj;
                                            dr["修正系数"] = al[tmpint].ToString();
                                        }
                                    }
                                }

                                this.gridViewLand.FocusedRowHandle = -1;
                                AutoEvalute();
                            }
                        }
                        #endregion
                        else
                        {
                            txtTdjb.Text = li.Tdjb.ToString();
                            txtJzdj.Text = li.Jzdj.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }

                //}
            }
            finally
            {
                LandDatumEnableControl();
                //afterTime = DateTime.Now;
                //KillExcel.KillExcelProcess(beforeTime, afterTime);
                //KillExcel.KillExcelProcess();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            diction = null; //如果点击获取还原数据按钮，如果数据库有记录的话,diction将不会是null;
            GetData();
        }

        private void LandDatumNoEnableControl()
        {
            this.simpleButton1.Enabled = false;
            this.progress1.Enabled = true;
            this.progress1.Visible = true;
            this.progress1.Text = "正在处理中...请稍等!";
            this.txtQph.Enabled = false;
        }

        private void LandDatumEnableControl()
        {
            this.progress1.Text = "完成!";
            this.progress1.Visible = true;
            this.progress1.Enabled = false;
            this.simpleButton1.Enabled = true;
            this.txtQph.Enabled = true;
        }

        private void GetData()
        {
            LandDatumNoEnableControl();
            if (ds != null)
            {
                gy = null;
                this.gridViewLand.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewLand_FocusedRowChanged);
                if (ds.Tables.Count == 1)
                {
                    ds.Tables[0].Clear();
                    this.gridControlGy.DataSource = ds.Tables[0].DefaultView;
                    this.gridViewLand.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewLand_FocusedRowChanged);
                }
            }
            else
            {
                ds = new DataSet();
            }
            if (double.TryParse(this.txtQph.Text.ToString(), out this.qph))
            {
                string landtype;
                SearchLandDatumValue(this.qph, out landtype);
                invokeThread = new Thread(new ThreadStart(StartMethod));
                invokeThread.Start();
            }
            else
            {
                LandDatumEnableControl();
            }
        }

        /// <summary>
        /// 根据区片号获得相关信息;
        /// </summary>
        /// <param name="qph"></param>
        /// <param name="landtype">返回区片号对应的土地用途</param>
        private void SearchLandDatumValue(double qph, out string landtype)
        {
            this.landDatumValue.Clear();
            this.gyqpTableAdapter1.Fill(this.landDatumValue.gyqp, qph);
            if (this.landDatumValue.gyqp.Rows.Count == 1)
            {
                this.gyszxsTableAdapter1.Fill(this.landDatumValue.gyszxs, qph);
                this.gyszysTableAdapter1.Fill(this.landDatumValue.gyszys, Convert.ToDouble(this.landDatumValue.gyqp.Rows[0]["tdjb"]));
                landtype = "工业";
                li.Jzdj = Convert.ToDouble(this.landDatumValue.gyqp.Rows[0]["qpj"]);
                li.Tdjb = Convert.ToDouble(this.landDatumValue.gyqp.Rows[0]["tdjb"]);
                li.Qph = this.qph.ToString();
                li.Type = landtype;
                return;
            }
            this.szqpTableAdapter1.Fill(this.landDatumValue.szqp, qph);
            if (this.landDatumValue.szqp.Rows.Count == 1)
            {
                this.szszxsTableAdapter1.Fill(this.landDatumValue.szszxs, qph);
                this.szszysTableAdapter1.Fill(this.landDatumValue.szszys, Convert.ToDouble(this.landDatumValue.szqp.Rows[0]["tdjb"]));
                landtype = "商住";
                li.Jzdj = Convert.ToDouble(this.landDatumValue.szqp.Rows[0]["qpj"]);
                li.Tdjb = Convert.ToDouble(this.landDatumValue.szqp.Rows[0]["tdjb"]);
                li.Qph = this.qph.ToString();
                li.Type = landtype;
                return;
            }
            this.zzqpTableAdapter1.Fill(this.landDatumValue.zzqp, qph);
            if (this.landDatumValue.zzqp.Rows.Count == 1)
            {
                this.zzszxsTableAdapter1.Fill(this.landDatumValue.zzszxs, qph);
                this.zzszysTableAdapter1.Fill(this.landDatumValue.zzszys, Convert.ToDouble(this.landDatumValue.zzqp.Rows[0]["tdjb"]));
                landtype = "住宅";
                li.Jzdj = Convert.ToDouble(this.landDatumValue.zzqp.Rows[0]["qpj"]);
                li.Tdjb = Convert.ToDouble(this.landDatumValue.zzqp.Rows[0]["tdjb"]);
                li.Qph = this.qph.ToString();
                li.Type = landtype;
                return;
            }
            landtype = "";
            li.Jzdj = 0d;
            li.Tdjb = 0d;
            li.Qph = this.qph.ToString();
            li.Type = landtype;
            return;
        }

        private void InitGrid()
        {
            if (gy == null)
            {
                this.simpleButton1.Enabled = true;
                return;
            }
            this.gridViewLand.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewLand_FocusedRowChanged);
            if (ds == null) ds = new DataSet();
            DataTable dt;
            DataRow dr;
            if (ds.Tables.Count == 1) ds.Tables.RemoveAt(0);//如果ds存在table将其删除，如果不删除table,它的id列的值将不会从1开始，而是在原有记数的基础上继续增加,程序FocusedRowChanged事件中需要根据id进行判断,故将table删除，重新自动生成table;
            if (ds.Tables.Count == 1)
            {
                dt = ds.Tables[0];
            }
            else
            {
                dt = new DataTable();
                DataColumn dc;
                //---------------------添加字段----------------
                //建立字段1
                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.Int32");
                dc.ColumnName = "id";
                dc.AutoIncrement = true;//标识
                dc.AutoIncrementSeed = 1;//标识种子
                dc.AutoIncrementStep = 1;//标识递增量
                dt.Columns.Add(dc);
                //建立字段2
                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "修正";
                dc.AllowDBNull = true;
                dt.Columns.Add(dc);
                //建立字段3
                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "具体条件";
                dc.AllowDBNull = true;
                dt.Columns.Add(dc);
                //建立字段4
                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "修正选择";
                dc.AllowDBNull = true;
                dt.Columns.Add(dc);
                //建立字段5
                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "修正系数";
                dc.AllowDBNull = true;
                dt.Columns.Add(dc);
                //设置主键
                DataColumn[] newdc = new DataColumn[1];//可设置多个字段为主键
                newdc[0] = dt.Columns["id"];
                dt.PrimaryKey = newdc;
            }


            //---------------------添加记录----------------
            for (int i = 0; i < gy.Range_Xz.Count; i++)
            {
                //记录1
                dr = dt.NewRow();

                dr["修正"] = gy.Range_Xz[i].ToString();

                object tmp;
                tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Jttj_{0}", i + 1), BindingFlags.GetProperty, null, gy, null);
                ArrayList al = (ArrayList)tmp;
                dr["具体条件"] = al[0].ToString();

                tmp = (object)gy.GetType().InvokeMember("Range_Xzsz", BindingFlags.GetProperty, null, gy, null);

                al = (ArrayList)tmp;
                dr["修正选择"] = al[0].ToString();

                tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Xzxs_{0}", i + 1), BindingFlags.GetProperty, null, gy, null);
                al = (ArrayList)tmp;
                dr["修正系数"] = al[0].ToString();
                dt.Rows.Add(dr);
            }

            //----把表添加到DataSet并绑定到DataList----
            if (ds.Tables.Count == 0)
            {
                ds.Tables.Add(dt);
            }
            ds.Tables[0].TableName = "AppraiseData";
            this.gridControlGy.DataSource = ds.Tables[0].DefaultView;
            LandDatumEnableControl();
            this.gridViewLand.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewLand_FocusedRowChanged);
        }

        private void repositoryItemComboBox具体条件_EditValueChanged(object sender, EventArgs e)
        {
            int i = (sender as DevExpress.XtraEditors.ComboBoxEdit).SelectedIndex;
            if (i < 0) return;
            int id = (int)(this.gridViewLand as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow()["id"];
            DataRow dr = (this.gridViewLand as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow();
            object tmp = (object)gy.GetType().InvokeMember("Range_Xzsz", BindingFlags.GetProperty, null, gy, null);
            ArrayList al = (ArrayList)tmp;
            dr["修正选择"] = al[i].ToString();
            tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Xzxs_{0}", id), BindingFlags.GetProperty, null, gy, null);
            al = (ArrayList)tmp;
            dr["修正系数"] = al[i].ToString();
        }

        private void repositoryItemComboBox具体条件_Click(object sender, EventArgs e)
        {


        }

        private void gridViewLand_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gy != null)
            {
                int id = (int)(sender as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow()["id"];
                this.repositoryItemComboBox具体条件.Items.Clear();
                object tmp;
                tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Jttj_{0}", id), BindingFlags.GetProperty, null, gy, null);
                ArrayList al = (ArrayList)tmp;
                foreach (string str in al)
                {
                    this.repositoryItemComboBox具体条件.Items.Add(str);
                }
            }
        }

        private void MethodForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetFMFormulaTemplate.FM_FormulaTemplate' table. You can move, or remove it, as needed.
            this.fM_FormulaTemplateTableAdapter.Fill(this.dataSetFMFormulaTemplate.FM_FormulaTemplate);
            #region method
            //this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, "假设开发法");
            //this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, "收益法");
            //this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, "成本法");
            //this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, "市场法");            
            #endregion
            #region Market
            // TODO: This line of code loads data into the 'marketFieldSelectDataSet.FM_FCMarketComporisonApproachTemplate' table. You can move, or remove it, as needed.
            this.fM_FCMarketComporisonApproachTemplateTableAdapter.Fill(this.marketFieldSelectDataSet.FM_FCMarketComporisonApproachTemplate);
            // TODO: This line of code loads data into the 'marketDataSet.FM_FCMarketComporisonApproach' table. You can move, or remove it, as needed.
            this.fM_FCMarketComporisonApproachTableAdapter1.Fill(this.marketDataSet.FM_FCMarketComporisonApproach, this.strprojectid);
            this.fM_MARKETHIDECOLUMNTableAdapter.Fill(this.marketDataSet.FM_MARKETHIDECOLUMN, this.strprojectid);
            BindMarketData(); //市场比较法数据绑定；
            //DataRelation dr = marketDataSet.Relations.Add("projectid", marketDataSet.Tables["FM_FCMarketComporisonApproach"].Columns["PROJECT_ID"], marketDataSet.Tables["FM_MARKETHIDECOLUMN"].Columns["PROJECT_ID"]);
            #endregion
        }

        private void xtraTabPage5_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Market
        private void vgc_Market_InitNewRecord(object sender, DevExpress.XtraVerticalGrid.Events.RecordIndexEventArgs e)
        {

        }

        private void btMarketCustomize_Click(object sender, System.EventArgs e)
        {
            if (this.vgc_Market.CustomizationForm != null)
            {
                showCustomization = true;
            }
            else
            {
                showCustomization = false;
            }
            DoCustomize(!showCustomization);
        }

        void DoCustomize(bool show)
        {
            showCustomization = show;
            if (showCustomization)
            {
                this.vgc_Market.RowsCustomization(btMarketCustomize.PointToScreen(new Point(0, btMarketCustomize.Bottom - btMarketCustomize.Top)));
            }
            else
            {
                this.vgc_Market.DestroyCustomization();
            }
        }

        //=============取消隐藏的column;
        public class RowOperationVisible : RowOperation
        {
            public RowOperationVisible() { }
            public override void Execute(BaseRow row)
            {
                if (row.Visible == false && row.OptionsRow.AllowMoveToCustomizationForm)
                {
                    row.Visible = true;
                }
            }
        }

        //=============得到显示的column对应的数据库Field,用于保存到数据库中;
        public class RowOperationVisibleGetField : RowOperation
        {
            public ArrayList al = new ArrayList();
            public RowOperationVisibleGetField()
            {
                al.Clear();
            }

            public override void Execute(BaseRow row)
            {
                //if (row.Visible == false && row.OptionsRow.AllowMoveToCustomizationForm)
                //{
                //    if (!al.Contains(row.Properties.FieldName))
                //    {
                //        al.Add(row.Properties.FieldName);
                //    }
                //}
                if (row.Visible == true)
                {
                    if (!al.Contains(row.Properties.FieldName))
                    {
                        al.Add(row.Properties.FieldName);
                    }
                }
            }
        }

        //=========================从数据库中得到要显示的列
        public class RowOperationVisibleFromDb : RowOperation
        {
            public string[] al;
            public RowOperationVisibleFromDb()
            {

            }

            public override void Execute(BaseRow row)
            {
                //if (row.Visible == true)
                //{
                if (al.Contains(row.Properties.FieldName) || al.Contains(row.Properties.FieldName.Replace("打分", "").ToString()))
                {
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
                //}
            }
        }
        //===============删除选中的记录
        //public class RowOperationDelete : RowOperation
        //{
        //    public RowOperationDelete() { }
        //    public override void Execute(BaseRow row)
        //    {
        //        bool bdel;
        //        if (row.Visible == true && !row.OptionsRow.AllowMoveToCustomizationForm && row.Properties.Caption.Contains("删除"))
        //        {
        //            row.Visible = true;
        //            bdel = bool.Parse(((DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit)row.Properties.RowEdit).ValueChecked.ToString());
        //        }
        //    }
        //}

        //将DevExpress.XtraVerticalGrid.VGridControl vgc_Market中隐藏的列保存到数据库
        private void btMarketSave_Click(object sender, EventArgs e)
        {
            RowOperationVisible Rv = new RowOperationVisible();
            this.vgc_Market.RowsIterator.DoOperation((DevExpress.XtraVerticalGrid.Rows.RowOperation)Rv);

        }

        private void btnMarketSaveToXls_Click(object sender, EventArgs e)
        {
            this.vgc_Market.ExportToXls(@"c:\market.xls");

            //RowOperationVisibleGetField Rv = new RowOperationVisibleGetField();
            //this.vgc_Market.RowsIterator.DoOperation((DevExpress.XtraVerticalGrid.Rows.RowOperation)Rv);
            //ArrayList al = Rv.al;
            ////转换成数组
            //string[] arrString = (string[])al.ToArray(typeof(string));
            //StringBuilder sb = new StringBuilder();
            //foreach (string str in arrString)
            //{
            //    sb.Append(str);
            //    sb.Append("#");
            //}
            //DataTable dt = this.marketDataSet.Tables["FM_MARKETHIDECOLUMN"];
            //DataRow dr;
            //if (dt.Rows.Count == 0)
            //{
            //    dr = dt.NewRow();
            //    dr["ID"] = Guid.NewGuid();
            //    dr["PROJECT_ID"] = this.strprojectid;
            //    dr["HIDECOLUMN"] = sb.ToString();
            //    dt.Rows.Add(dr);
            //}
            //else
            //{
            //    dr = dt.Rows[0];
            //    dr["HIDECOLUMN"] = sb.ToString();
            //}
            MarketUpdateResult();
            SubmitMarketChanges();

        }

        private void vgc_Market_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && e.Control)
            {
                try
                {
                    DataView sourceView = ((AppraiseMethod.MarketDataSet)(((System.Windows.Forms.BindingSource)(this.vgc_Market.DataSource)).DataSource)).FM_FCMarketComporisonApproach.DefaultView;
                    if (!sourceView.Table.Rows[this.vgc_Market.FocusedRecord]["比较项目"].ToString().Contains("估价对象"))
                    {
                        DialogResult result = MessageBox.Show(string.Format("删除ID为{0}的比较案例", sourceView.Table.Rows[this.vgc_Market.FocusedRecord]["ID"]), "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            sourceView.Delete(this.vgc_Market.FocusedRecord);
                            SubmitMarketChanges();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        //删除[选中项目]列为false,[比较项目]列不包含估价对象的行
        private void 删除案例_Click(object sender, EventArgs e)
        {
            DataTable ds = this.marketDataSet.Tables["FM_FCMarketComporisonApproach"];
            foreach (DataRow dr in ds.Rows)
            {
                bool bl = false;
                if (bool.TryParse(dr["选中项目"].ToString(), out bl))
                {
                    if (bl != true && !dr["比较项目"].ToString().Contains("估价对象"))
                    {
                        dr.Delete();
                    }
                }
            }
            SubmitMarketChanges();
        }

        private void SubmitMarketChanges()
        {
            if (this.marketDataSet.HasChanges())
            {
                this.fM_FCMarketComporisonApproachTableAdapter1.Update(this.marketDataSet);
                this.fM_MARKETHIDECOLUMNTableAdapter.Update(this.marketDataSet);
                this.marketDataSet.AcceptChanges();
            }
            BindMarketData();
        }

        private void BindMarketData()
        {
            if (marketDataSet.Tables["FM_MARKETHIDECOLUMN"].Rows.Count != 0)
            {
                string tmpstr = marketDataSet.Tables["FM_MARKETHIDECOLUMN"].Rows[0]["HIDECOLUMN"].ToString();
                string[] al;
                al = tmpstr.Split(new char[] { '#' });
                //for (int i = 0; i < vgc_Market.Rows.Count; i++)
                //{
                //    BaseRow bb = (BaseRow)vgc_Market.Rows[i];
                //    if (!al.Contains(bb.Properties.FieldName))
                //    {
                //        bb.Visible = false;
                //    }
                //}
                RowOperationVisibleFromDb Rv = new RowOperationVisibleFromDb();
                Rv.al = al;
                this.vgc_Market.RowsIterator.DoOperation((DevExpress.XtraVerticalGrid.Rows.RowOperation)Rv);
            }

            this.vgc_Market.DataSource = this.marketDataSet;
            this.vgc_Market.DataMember = "FM_FCMarketComporisonApproach";
        }


        private void 添加案例_Click(object sender, EventArgs e)
        {
            string tmpstr = "";
            double qz = 0;
            DataTable sourceTable = this.marketDataSet.Tables["FM_FCMarketComporisonApproach"];
            #region 定义默认比较项目的名称
            if (sourceTable.Rows.Count == 0)
            {
                tmpstr = "估价对象";
                qz = 1;
            }
            else if (sourceTable.Rows.Count == 1)
            {
                tmpstr = "实例A";
                qz = 0.333;
            }
            else if (sourceTable.Rows.Count == 2)
            {
                tmpstr = "实例B";
                qz = 0.333;
            }
            else if (sourceTable.Rows.Count == 3)
            {
                tmpstr = "实例C";
                qz = 0.333;
            }
            else if (sourceTable.Rows.Count == 4)
            {
                tmpstr = "实例D";
            }
            else
            {
                tmpstr = "实例";
            }
            #endregion
            DataRow row = sourceTable.NewRow();
            row["id"] = Guid.NewGuid();
            row["PROJECT_ID"] = this.strprojectid;
            row["比较项目"] = tmpstr;
            row["交易价格"] = 0;
            row["选中项目"] = true;
            row["权重"] = qz;
            sourceTable.Rows.Add(row);
            SubmitMarketChanges();
        }

        private void vgc_Market_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            try
            {
                //==============
                string str = e.Value.ToString();
                int ifocus = ((DevExpress.XtraVerticalGrid.VGridControlBase)(sender)).FocusedRecord;//当前行
                int igjdx = -1; ; //记录估价对象所在的行;
                string fieldname = ((DevExpress.XtraVerticalGrid.Rows.EditorRow)((DevExpress.XtraVerticalGrid.Events.RowEventArgs)(e)).Row).Properties.FieldName.ToString();
                List<string> ls = new List<string>();
                List<string> lstag = new List<string>();
                string[] strarray;
                foreach (object a in (((System.Collections.CollectionBase)((((DevExpress.XtraEditors.Repository.RepositoryItemComboBox)(((((DevExpress.XtraVerticalGrid.Rows.EditorRow)((DevExpress.XtraVerticalGrid.Events.RowEventArgs)(e)).Row).Properties.RowEdit))))).Properties.Items))))
                {
                    ls.Add(a.ToString());
                }
                string strtag = (((DevExpress.XtraEditors.Repository.RepositoryItemComboBox)(((((DevExpress.XtraVerticalGrid.Rows.EditorRow)((DevExpress.XtraVerticalGrid.Events.RowEventArgs)(e)).Row).Properties.RowEdit))))).Properties.Tag.ToString();
                strarray = strtag.Split(new char[] { '#' });
                lstag = strarray.ToList<string>();

                DataTable sourceTable = this.marketDataSet.Tables["FM_FCMarketComporisonApproach"];
                if (sourceTable.Rows.Count > 0)
                {
                    for (int i = 0; i < sourceTable.Rows.Count; i++)
                    {
                        if (sourceTable.Rows[i]["比较项目"].ToString() == "估价对象")
                        {
                            igjdx = i;
                            break;
                        }
                    }
                    #region 非估价对象相关因素改变
                    if (igjdx >= 0 && ifocus != igjdx) 
                    {
                        DataRow drgjdx = sourceTable.Rows[igjdx];
                        DataRow drfocus = sourceTable.Rows[ifocus];
                        ModifyDf(sourceTable, drgjdx, drfocus, ls, lstag, fieldname);
                    }
                    #endregion
                    #region 估价对象相关因素改变
                    else
                    {
                        DataRow drgjdx = sourceTable.Rows[igjdx];
                        for (int ii = 0; ii < sourceTable.Rows.Count; ii++)
                        {
                            DataRow drfocus = sourceTable.Rows[ii];
                            if (ii != igjdx)
                            {
                                ModifyDf(sourceTable, drgjdx, drfocus, ls, lstag, fieldname);
                            }
                        }
                    }
                    #endregion

                }
            }
            catch
            { }
            finally
            {
                //
                Marketcalc(e.RecordIndex);
            }
        }

        private void ModifyDf(DataTable sourceTable,DataRow drgjdx, DataRow drfocus,List<string> ls,List<string>lstag,string fieldname)
        {
            if (ls.Contains(drgjdx[fieldname].ToString()) && ls.Contains(drfocus[fieldname].ToString()))
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].ToString() == drgjdx[fieldname].ToString())
                    {
                        double tmpi = double.Parse(lstag[i]);
                        for (int j = 0; j < ls.Count; j++)
                        {
                            if (ls[j].ToString() == drfocus[fieldname].ToString())
                            {
                                double tmpj = double.Parse(lstag[j]);
                                string tmpfieldname = string.Format("{0}打分", fieldname);
                                if (sourceTable.Columns.Contains(tmpfieldname))
                                {
                                    drfocus[tmpfieldname] = 100 - (tmpi - tmpj);
                                }
                            }
                        }
                    }
                }

            }
        }

        private void Marketcalc(int tmpi)
        {
            double sum = 1;//当前选中单元格所在行的总修正系数
            double bj = 0; //最后确定估价对象的价格;
            int igj = 0; //查找估价对象所在记录的index;
            bool bfind = false;
            //for (int i = 0; i < vgc_Market.RecordCount; i++)
            //{
            //    sum = sum + Convert.ToDouble(vgc_Market.GetCellValue(e.Row, i));
            //}
            //if (sum > 0)
            //    e.Row.Appearance.BackColor = Color.Red;
            //int i = e.CellIndex;
            this.vgc_Market.CellValueChanged -= new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler(this.vgc_Market_CellValueChanged);
            DataTable sourceTable = this.marketDataSet.Tables["FM_FCMarketComporisonApproach"];
            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                if (sourceTable.Rows[i]["比较项目"].ToString() == "估价对象")
                {
                    bfind = true;
                    igj = i;
                    break;
                }
            }
            if (!bfind) return;

            RowOperationVisibleGetField Rv = new RowOperationVisibleGetField();
            this.vgc_Market.RowsIterator.DoOperation((DevExpress.XtraVerticalGrid.Rows.RowOperation)Rv);
            ArrayList al = Rv.al;
            string[] str = (string[])al.ToArray(typeof(string));
            DataRow dr = sourceTable.Rows[tmpi];
            #region 将估价对象所有打分的列，不管任何情况都设为100
            foreach (DataColumn dc in sourceTable.Columns)
            {
                if (dc.ColumnName.Contains("打分") && str.Contains(dc.ColumnName.Substring(0, dc.ColumnName.Length - 2)))
                {
                    sourceTable.Rows[igj][dc.ColumnName] = 100;
                }
            }
            #endregion
            #region 打分的列的值如果为空，将其设为100;
            if (igj != tmpi)
            {
                foreach (DataColumn dc in sourceTable.Columns)
                {
                    if (dc.ColumnName.Contains("打分") && str.Contains(dc.ColumnName.Substring(0, dc.ColumnName.Length - 2)))
                    {
                        if (!string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                        {

                        }
                        else
                        {
                            dr[dc.ColumnName] = 100;
                        }

                    }
                }
            }
            #endregion
            foreach (DataColumn dc in sourceTable.Columns)
            {
                if (dc.ColumnName.Contains("打分") && str.Contains(dc.ColumnName.Substring(0, dc.ColumnName.Length - 2)))
                {
                    if (!string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                    {
                        sum = sum * (Convert.ToDouble(sourceTable.Rows[igj][dc.ColumnName].ToString()) / Convert.ToDouble(dr[dc.ColumnName].ToString()));
                    }
                    else
                    {
                    }

                }
            }

            sourceTable.Rows[igj]["交易价格"] = 0;
            sourceTable.Rows[igj]["总修正系数"] = 1;
            sourceTable.Rows[igj]["比准价格元平方米"] = DBNull.Value;
            dr["总修正系数"] = sum.ToString("#.###");
            try
            {

                double dj; //交易价格
                if (double.TryParse(dr["交易价格"].ToString(), out dj))
                {
                    if (tmpi != igj) dr["比准价格元平方米"] = ((double)Convert.ToDouble(dr["交易价格"]) * Convert.ToDouble(dr["总修正系数"])).ToString("#");
                    for (int i = 0; i < sourceTable.Rows.Count; i++)
                    {
                        if (i != igj)
                        {
                            if (double.TryParse(sourceTable.Rows[i]["比准价格元平方米"].ToString(), out dj))
                            {
                                bj = bj + Convert.ToDouble(sourceTable.Rows[i]["比准价格元平方米"]) * Convert.ToDouble(sourceTable.Rows[i]["权重"]);
                            }
                        }
                    }
                    sourceTable.Rows[igj]["比准价格元平方米"] = (bj).ToString("#");
                }
            }
            catch
            {

            }

            this.vgc_Market.CellValueChanged += new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler(this.vgc_Market_CellValueChanged);
        }

        /// <summary>
        /// 重新记算市场比较法
        /// </summary>
        public void MarketUpdateResult()
        {
            DataTable sourceTable = this.marketDataSet.Tables["FM_FCMarketComporisonApproach"];
            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                Marketcalc(i);
            }
        }

        private void vgc_Market_EndDragRow(object sender, DevExpress.XtraVerticalGrid.Events.EndDragRowEventArgs e)
        {

        }

        private void gridView2_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {

        }

        private void repositoryItemCheckEdit2_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = this.marketDataSet.Tables["FM_MARKETHIDECOLUMN"];
            DataRow dr;
            string str = ((System.Data.DataRowView)(this.gridView2.GetFocusedRow())).Row["name1"].ToString();
            if (Convert.ToBoolean(((DevExpress.XtraEditors.CheckEdit)(sender)).EditValue))
            {
                if (dt.Rows.Count == 0)
                {
                    //dr = dt.NewRow();
                    //DataTable tmpdt = this.marketFieldSelectDataSet.Tables["FM_FCMarketComporisonApproachTemplate"];
                    //StringBuilder tmp = new StringBuilder() ;
                    //for (int i = 0; i < tmpdt.Rows.Count; i++)
                    //{
                    //   tmp.Append(string.Format("{0}#", tmpdt.Rows[i]["name1"].ToString()));
                    //}
                    //dr["ID"] = Guid.NewGuid();
                    //dr["PROJECT_ID"] = this.strprojectid;
                    //dr["HIDECOLUMN"] = tmp.ToString();
                    //dt.Rows.Add(dr);
                }
                else
                {
                    dr = dt.Rows[0];
                    if (!dr["HIDECOLUMN"].ToString().Contains(str + "#"))
                    {
                        string tmp = dr["HIDECOLUMN"].ToString();
                        tmp = String.Format("{0}{1}#", tmp, str);
                        dr["HIDECOLUMN"] = tmp;
                    }

                }
            }
            else
            {
                if (dt.Rows.Count == 0)
                {
                    //dr = dt.NewRow();
                    //DataTable tmpdt = this.marketFieldSelectDataSet.Tables["FM_FCMarketComporisonApproachTemplate"];
                    //StringBuilder tmp = new StringBuilder();
                    //for (int i = 0; i < tmpdt.Rows.Count; i++)
                    //{
                    //    tmp.Append(string.Format("{0}#", tmpdt.Rows[i]["name1"].ToString()));
                    //}
                    //dr["ID"] = Guid.NewGuid();
                    //dr["PROJECT_ID"] = this.strprojectid;
                    //dr["HIDECOLUMN"] = tmp.ToString();
                    //dt.Rows.Add(dr);
                }
                else
                {
                    dr = dt.Rows[0];
                    if (dr["HIDECOLUMN"].ToString().Contains(str + "#"))
                    {
                        string tmp = dr["HIDECOLUMN"].ToString().Replace(str + "#", "");
                        //tmp = dr["HIDECOLUMN"].ToString().Replace(str + "打分#", "");
                        dr["HIDECOLUMN"] = tmp;
                    }

                }
            }
            // SubmitMarketChanges();

        }

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "sel")
            {
                DataTable dt = this.marketDataSet.Tables["FM_MARKETHIDECOLUMN"];
                DataRow dr;
                if (dt.Rows.Count == 0)
                {
                    //====================== 如果FM_MARKETHIDECOLUMN中没有任何记录，则新增一条记录;                 
                    DataTable tmpdt = this.marketFieldSelectDataSet.Tables["FM_FCMarketComporisonApproachTemplate"];
                    dr = dt.NewRow();
                    StringBuilder tmp = new StringBuilder();
                    for (int i = 0; i < tmpdt.Rows.Count; i++)
                    {
                        tmp.Append(string.Format("{0}#", tmpdt.Rows[i]["name1"].ToString()));
                    }
                    dr["ID"] = Guid.NewGuid();
                    dr["PROJECT_ID"] = this.strprojectid;
                    dr["HIDECOLUMN"] = tmp.ToString();
                    dt.Rows.Add(dr);
                    if (marketDataSet.HasChanges())
                    {
                        this.fM_MARKETHIDECOLUMNTableAdapter.Update(this.marketDataSet);
                        this.marketDataSet.AcceptChanges();
                    }
                    //======================
                    e.Value = true;
                }
                else
                {
                    dr = dt.Rows[0];
                    DataTable dt1 = this.marketFieldSelectDataSet.FM_FCMarketComporisonApproachTemplate;
                    if (dr["HIDECOLUMN"].ToString().Contains(dt1.Rows[e.RowHandle]["name1"].ToString() + "#"))
                    {
                        e.Value = true;
                    }
                    else
                    {
                        e.Value = false;
                    }
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.gcMarket.Visible = true;
            this.vgc_Market.Visible = false;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.vgc_Market.Visible = true;
            this.gcMarket.Visible = false;
            SubmitMarketChanges();
        }
        #endregion

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            LanddatumMethod(true, "基准地价修正法");
        }

        /// <summary>
        /// //将基准地价相关的数据入库或者出库
        /// </summary>
        /// <param name="flag">true 入库, false出库</param>
        private void LanddatumMethod(bool flag, string methodname)
        {
            this.LandDatumNoEnableControl();
            this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, methodname);
            if (flag)
            {
                #region 入库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    foreach (DataRow dr in fmMethodDataSet.FM_Method.Rows)
                    {
                        dr.Delete();
                    }
                }
                string tdrt = "";
                if (chkbox别墅.Checked) tdrt = "别墅";
                if (chkbox工业.Checked) tdrt = "工业";
                if (chkbox商住.Checked) tdrt = "商住";
                if (chkbox住宅.Checked) tdrt = "住宅";
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "区片号", txtQph.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土地用途", tdrt, typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土地级别", txtTdjb.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "基准地价", txtJzdj.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "容积率", txtRjl.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "期日修正系数", txtQrxzxs.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土地还原利率", txtTdhyll.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "宗地剩余使用年限", txtTdsrlx.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "基准地价设定年限", txtTdsdnx.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "容积率系数", txtRjlxs.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "年期系数", txtNqxs.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土地面积", txt土地面积.Text.ToString(), typeof(double).ToString());
                if (ds != null)
                {
                    if (ds.Tables.Count == 1)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, dr[1].ToString(), dr[2].ToString(), typeof(string).ToString());
                        }
                    }
                }
                this.fM_MethodTableAdapter1.Update(this.fmMethodDataSet);
                this.fmMethodDataSet.AcceptChanges();
                AutoEvalute();
                #endregion
            }
            else
            {
                #region 出库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    diction = new Dictionary<string, string>();
                    diction.Clear();
                    List<DataSetMethod.FM_MethodRow> rlist = this.fmMethodDataSet.FM_Method.ToList();
                    foreach (DataSetMethod.FM_MethodRow fr in rlist)
                    {
                        diction.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
                    }
                    string tmp;
                    if (diction.TryGetValue("区片号", out tmp))
                    {
                        txtQph.Text = tmp;
                    }
                    if (diction.TryGetValue("土地面积", out tmp))
                    {
                        txt土地面积.Text = tmp;
                    }
                    //if (diction.TryGetValue("土地级别", out tmp))
                    //{
                    //    //if (chkbox别墅.Checked) tdrt = "别墅";
                    //    //if (chkbox工业.Checked) tdrt = "工业";
                    //    //if (chkbox商住.Checked) tdrt = "商住";
                    //    //if (chkbox住宅.Checked) tdrt = "住宅";
                    //    //if (tmp == "别墅") 
                    //}
                    if (diction.TryGetValue("土地用途", out tmp))
                    {
                        if (tmp == "别墅") chkbox别墅.Checked = true;
                        if (tmp == "工业") chkbox工业.Checked = true;
                        if (tmp == "商住") chkbox商住.Checked = true;
                        if (tmp == "住宅") chkbox住宅.Checked = true;
                    }
                    if (diction.TryGetValue("土地级别", out tmp))
                    {
                        txtTdjb.Text = tmp;
                    }
                    if (diction.TryGetValue("基准地价", out tmp))
                    {
                        txtJzdj.Text = tmp;
                    }
                    if (diction.TryGetValue("容积率", out tmp))
                    {
                        txtRjl.Text = tmp;
                    }
                    if (diction.TryGetValue("期日修正系数", out tmp))
                    {
                        this.txtQrxzxs.Text = tmp;
                    }
                    if (diction.TryGetValue("土地还原利率", out tmp))
                    {
                        this.txtTdhyll.Text = tmp;
                    }
                    if (diction.TryGetValue("宗地剩余使用年限", out tmp))
                    {
                        txtTdsrlx.Text = tmp;
                    }
                    if (diction.TryGetValue("基准地价设定年限", out tmp))
                    {
                        txtTdsdnx.Text = tmp;
                    }
                    if (diction.TryGetValue("容积率系数", out tmp))
                    {
                        txtRjlxs.Text = tmp;
                    }
                    if (diction.TryGetValue("年期系数", out tmp))
                    {
                        txtNqxs.Text = tmp;
                    }
                }
                #endregion
            }
            this.LandDatumEnableControl();
        }

        /// <summary>
        /// //将[假设开发法-工业用地]相关的数据入库或者出库
        /// </summary>
        /// <param name="flag">true 入库, false出库</param>
        private void JsfMethod(bool flag, string methodname)
        {
            Dictionary<string, string> jsfVariables = new Dictionary<string, string>();
            List<DataSetMethod.FM_MethodRow> rlist;
            this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, methodname);
            if (flag)
            {
                #region 入库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    foreach (DataRow dr in fmMethodDataSet.FM_Method.Rows)
                    {
                        dr.Delete();
                    }
                }
                //#############
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土地面积", txt土地面积.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "总建筑面积", txt建筑面积.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "开发利润率", txtJsf开发利润率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "买方购买该宗地使用权应负担的税费率", txtJsf买方负担的税费率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "市场售价", txtjsf市场售价.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "安装工程建筑费用", txtJsf安装工程建筑费用.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "室外工程建筑费用", txtJsf室外工程建筑费用.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土建工程建筑费用", txtJsf土建工程建筑费用.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "报建费", txtJsf报建费.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "专业费率", txtJsf专业费率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "管理费率", txtJsf管理费率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "基准年利息率", txtJsf基准年利息率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "销售周期", txtJsf销售周期.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "开发建设周期", txtJsf开发建设周期.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "销售税金率", txtJsf销售税金率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "销售费用率", txtJsf销售费用率.Text.ToString(), typeof(double).ToString());
                this.fM_MethodTableAdapter1.Update(this.fmMethodDataSet);
                this.fmMethodDataSet.AcceptChanges();
                #endregion
            }
            else
            {
                #region 出库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    string tmp;
                    jsfVariables.Clear();
                    rlist = this.fmMethodDataSet.FM_Method.ToList();
                    foreach (DataSetMethod.FM_MethodRow fr in rlist)
                    {
                        jsfVariables.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
                    }

                    if (jsfVariables.TryGetValue("土地面积", out tmp))
                    {
                        txt土地面积.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("总建筑面积", out tmp))
                    {
                        txt建筑面积.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("开发利润率", out tmp))
                    {
                        txtJsf开发利润率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("买方购买该宗地使用权应负担的税费率", out tmp))
                    {
                        txtJsf买方负担的税费率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("市场售价", out tmp))
                    {
                        txtjsf市场售价.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("安装工程建筑费用", out tmp))
                    {
                        txtJsf安装工程建筑费用.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("室外工程建筑费用", out tmp))
                    {
                        txtJsf室外工程建筑费用.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("土建工程建筑费用", out tmp))
                    {
                        txtJsf土建工程建筑费用.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("报建费", out tmp))
                    {
                        txtJsf报建费.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("专业费率", out tmp))
                    {
                        txtJsf专业费率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("管理费率", out tmp))
                    {
                        txtJsf管理费率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("基准年利息率", out tmp))
                    {
                        txtJsf基准年利息率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("开发建设周期", out tmp))
                    {
                        txtJsf开发建设周期.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("销售周期", out tmp))
                    {
                        txtJsf销售周期.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("销售税金率", out tmp))
                    {
                        txtJsf销售税金率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("销售费用率", out tmp))
                    {
                        txtJsf销售费用率.Text = tmp;
                    }
                }
                #endregion
            }
            StartExecute("假设开发法", "工业用地");
            //jsfVariables.Clear();
            //rlist = this.fmMethodDataSet.FM_Method.ToList();
            //foreach (DataSetMethod.FM_MethodRow fr in rlist)
            //{
            //    jsfVariables.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
            //}
            //bool breturn = CanEvaluate(jsfVariables, "假设开发法", "工业用地");
            //if (breturn)
            //{
            //    FormulaEngine.ClearVariableTable();
            //    foreach (object ob in jsfVariables)
            //    {
            //        FormulaEngine.ht[((System.Collections.Generic.KeyValuePair<string, string>)(ob)).Key] = ((System.Collections.Generic.KeyValuePair<string, string>)(ob)).Value;
            //    }

            //    #region 不允许ht中存在空值,如果存在空值，就不进行计算了
            //    foreach (object tmp in FormulaEngine.ht)
            //    {
            //        if (string.IsNullOrEmpty(Convert.ToString(((System.Collections.DictionaryEntry)(tmp)).Value)))
            //        {
            //            return;
            //        }
            //    }
            //    #endregion
            //    //formulacanevaluate 记录FM_FormulaTemplate表中定义的公式;
            //    Dictionary<string, bool> formulacanevaluate = new Dictionary<string, bool>();
            //    this.fM_FormulaTemplateTableAdapter.FillBy(this.dataSetFMFormulaTemplate.FM_FormulaTemplate, "假设开发法", "工业用地");

            //    if (this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Count > 0)
            //    {
            //        foreach (DataRow dr in this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Rows)
            //        {
            //            formulacanevaluate.Add(dr["MethodFormula"].ToString(), false);
            //        }


            //        Dictionary<string, bool> formulaevalute = new Dictionary<string, bool>();//保存可以执行的,并已经执行的formula
            //        do
            //        {
            //            foreach (object obj in formulacanevaluate)
            //            {
            //                byte nResult = 0;
            //                double dValue = 0.0;
            //                string pErr = "";
            //                string tmpexpress = ((System.Collections.Generic.KeyValuePair<string, bool>)(obj)).Key;
            //                bool brunok = FormulaEngine.CheckCanEvaluate(tmpexpress, ref nResult, ref dValue, ref pErr);
            //                if (brunok)
            //                {
            //                    formulaevalute[tmpexpress] = true;
            //                }
            //            }
            //            foreach (string tmpstr in formulaevalute.Keys)
            //            {
            //                if (formulacanevaluate.ContainsKey(tmpstr))
            //                {
            //                    formulacanevaluate.Remove(tmpstr);
            //                }
            //            }
            //            formulaevalute.Clear();
            //        }
            //        while (formulacanevaluate.Count != 0);

            //        DataTable dt = FormulaEngineHtToTable();
            //        gdcJsfGy.DataSource = dt.DefaultView;
            //    }
            //    else
            //    {
            //        MessageBox.Show("FM_FormulaTemplate表中没有找到定义的公式!");
            //    }
            //}

        }


        /// <summary>
        /// //将[假设开发法-商品房]相关的数据入库或者出库
        /// </summary>
        /// <param name="flag">true 入库, false出库</param>
        private void JsfspfMethod(bool flag, string methodname)
        {
            Dictionary<string, string> jsfVariables = new Dictionary<string, string>();
            List<DataSetMethod.FM_MethodRow> rlist;
            this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, methodname);
            if (flag)
            {
                #region 入库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    foreach (DataRow dr in fmMethodDataSet.FM_Method.Rows)
                    {
                        dr.Delete();
                    }
                }
                //#############
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "假商房地产单价",  this.txtJsfspf房地产单价.Text.ToString(), typeof(double).ToString());
                this.fM_MethodTableAdapter1.Update(this.fmMethodDataSet);
                this.fmMethodDataSet.AcceptChanges();
                #endregion
            }
            else
            {
                #region 出库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    string tmp;
                    jsfVariables.Clear();
                    rlist = this.fmMethodDataSet.FM_Method.ToList();
                    foreach (DataSetMethod.FM_MethodRow fr in rlist)
                    {
                        jsfVariables.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
                    }

                    if (jsfVariables.TryGetValue("假商房地产单价", out tmp))
                    {
                        txtJsf销售费用率.Text = tmp;
                    }
                }
                #endregion
            }
            StartExecute("假设开发法", "商品房");           
        }
        /// <summary>
        /// 将[成本法]相关的数据入库或出库
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="methodname"></param>
        private void CbfMethod(bool flag, string methodname)
        {
            Dictionary<string, string> jsfVariables = new Dictionary<string, string>();
            List<DataSetMethod.FM_MethodRow> rlist;
            this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, methodname);
            if (flag)
            {
                #region 入库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    foreach (DataRow dr in fmMethodDataSet.FM_Method.Rows)
                    {
                        dr.Delete();
                    }
                }
                //#############
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "建筑物的成新率", txtCbf建筑物的成新率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "前期费用", txtCbf前期费用.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "土建成本", txtCbf土建成本.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "建安费", txtCbf建安费.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "取土建总造价的百分率", txtCbf取土建总造价的百分率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "房产面积", txt建筑面积.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "基准地价修正法土地总价", txtCbf基准地价修正法土地总价.Text.ToString(), typeof(double).ToString());

                this.fM_MethodTableAdapter1.Update(this.fmMethodDataSet);
                this.fmMethodDataSet.AcceptChanges();
                #endregion
            }
            else
            {
                #region 出库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    string tmp;
                    jsfVariables.Clear();
                    rlist = this.fmMethodDataSet.FM_Method.ToList();
                    foreach (DataSetMethod.FM_MethodRow fr in rlist)
                    {
                        jsfVariables.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
                    }

                    if (jsfVariables.TryGetValue("建筑物的成新率", out tmp))
                    {
                        txtCbf建筑物的成新率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("前期费用", out tmp))
                    {
                        txtCbf前期费用.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("土建成本", out tmp))
                    {
                        txtCbf土建成本.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("建安费", out tmp))
                    {
                        txtCbf建安费.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("取土建总造价的百分率", out tmp))
                    {
                        txtCbf取土建总造价的百分率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("基准地价修正法土地总价", out tmp))
                    {
                        txtCbf基准地价修正法土地总价.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("房产面积", out tmp))
                    {
                        txt建筑面积.Text = tmp;
                    }

                }
                #endregion
            }
            StartExecute("成本法", "结合基准地价修正法");

        }

        /// <summary>
        /// 将[收益法]相关的数据入库或出库
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="methodname"></param>
        private void SyfMethod(bool flag, string methodname)
        {
            Dictionary<string, string> jsfVariables = new Dictionary<string, string>();
            List<DataSetMethod.FM_MethodRow> rlist;
            this.fM_MethodTableAdapter1.FillBy(this.fmMethodDataSet.FM_Method, this.Project_Id, methodname);
            if (flag)
            {
                #region 入库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    foreach (DataRow dr in fmMethodDataSet.FM_Method.Rows)
                    {
                        dr.Delete();
                    }
                }
                //#############
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "成本法房屋现值", txtSyf成本法房屋现值.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "收益率", txtSyf收益率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "收益年限", txtSyf收益年限.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "收益总面积", txtSyf收益总面积.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "月租金", txtSyf月租金.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "可出租时间", txtSyf可出租时间.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "空置时间", txtSyf空置时间.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "管理费率", txtSyf管理费率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "建筑物现值", txtSyf建筑物现值.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "维修费率", txtSyf维修费率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "保险费率", txtSyf保险费率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "房产税率", txtSyf房产税率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "营业税率", txtSyf营业税率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "城市维护建设税率", txtSyf城市维护建设税率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "教育费附加率", txtSyf教育费附加率.Text.ToString(), typeof(double).ToString());
                this.fmMethodDataSet.FM_Method.AddFM_MethodRow(Guid.NewGuid(), this.Project_Id, methodname, "个人所得税率", txtSyf个人所得税率.Text.ToString(), typeof(double).ToString());
                this.fM_MethodTableAdapter1.Update(this.fmMethodDataSet);
                this.fmMethodDataSet.AcceptChanges();
                #endregion
            }
            else
            {
                #region 出库
                if (this.fmMethodDataSet.FM_Method.Count > 0)
                {
                    string tmp;
                    jsfVariables.Clear();
                    rlist = this.fmMethodDataSet.FM_Method.ToList();
                    foreach (DataSetMethod.FM_MethodRow fr in rlist)
                    {
                        jsfVariables.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
                    }

                    if (jsfVariables.TryGetValue("成本法房屋现值", out tmp))
                    {
                        txtSyf成本法房屋现值.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("收益率", out tmp))
                    {
                        txtSyf收益率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("收益年限", out tmp))
                    {
                        txtSyf收益年限.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("收益总面积", out tmp))
                    {
                        txtSyf收益总面积.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("月租金", out tmp))
                    {
                        txtSyf月租金.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("可出租时间", out tmp))
                    {
                        txtSyf可出租时间.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("空置时间", out tmp))
                    {
                        txtSyf空置时间.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("管理费率", out tmp))
                    {
                        txtSyf管理费率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("建筑物现值", out tmp))
                    {
                        txtSyf建筑物现值.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("维修费率", out tmp))
                    {
                        txtSyf维修费率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("保险费率", out tmp))
                    {
                        txtSyf保险费率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("房产税率", out tmp))
                    {
                        txtSyf房产税率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("营业税率", out tmp))
                    {
                        txtSyf营业税率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("城市维护建设税率", out tmp))
                    {
                        txtSyf城市维护建设税率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("教育费附加率", out tmp))
                    {
                        txtSyf教育费附加率.Text = tmp;
                    }
                    if (jsfVariables.TryGetValue("个人所得税率", out tmp))
                    {
                        txtSyf个人所得税率.Text = tmp;
                    }
                }
                #endregion
            }
            StartExecute("收益还原法", "结合成本法");

        }

        private void StartExecute(string methodbigname, string methodsmallname)
        {
            Dictionary<string, string> jsfVariables = new Dictionary<string, string>();
            jsfVariables.Clear();
            List<DataSetMethod.FM_MethodRow> rlist = this.fmMethodDataSet.FM_Method.ToList();
            foreach (DataSetMethod.FM_MethodRow fr in rlist)
            {
                jsfVariables.Add(fr.ParameterName.ToString(), fr.ParameterValue.ToString());
            }
            bool breturn = CanEvaluate(jsfVariables, methodbigname, methodsmallname);
            if (breturn)
            {
                FormulaEngine.ClearVariableTable();
                foreach (object ob in jsfVariables)
                {
                    FormulaEngine.ht[((System.Collections.Generic.KeyValuePair<string, string>)(ob)).Key] = ((System.Collections.Generic.KeyValuePair<string, string>)(ob)).Value;
                }

                #region 不允许ht中存在空值,如果存在空值，就不进行计算了
                foreach (object tmp in FormulaEngine.ht)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(((System.Collections.DictionaryEntry)(tmp)).Value)))
                    {
                        MessageBox.Show(String.Format("{0}不能为空!", ((System.Collections.DictionaryEntry)(tmp)).Key));
                        return;
                    }
                    else
                    {
                        double dbreturn;
                        if (!double.TryParse(Convert.ToString(((System.Collections.DictionaryEntry)(tmp)).Value), out dbreturn))
                        {
                            MessageBox.Show(String.Format("{0} ={1}，{0}必须为数值类型!", ((System.Collections.DictionaryEntry)(tmp)).Key, Convert.ToString(((System.Collections.DictionaryEntry)(tmp)).Value)));
                            return;
                        }
                    }
                }
                #endregion
                //formulacanevaluate 记录FM_FormulaTemplate表中定义的公式;
                Dictionary<string, bool> formulacanevaluate = new Dictionary<string, bool>();
                this.fM_FormulaTemplateTableAdapter.FillBy(this.dataSetFMFormulaTemplate.FM_FormulaTemplate, methodbigname, methodsmallname);

                if (this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Count > 0)
                {
                    foreach (DataRow dr in this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Rows)
                    {
                        formulacanevaluate.Add(dr["MethodFormula"].ToString(), false);
                    }

                    Dictionary<string, bool> formulaevalute = new Dictionary<string, bool>();//保存可以执行的,并已经执行的formula
                    //为了防止参数输入有误后进入死循环；
                    int maxevalcount = 100;//最多执行次数;
                    int runnum = 0;
                    do
                    {
                        foreach (object obj in formulacanevaluate)
                        {
                            byte nResult = 0;
                            double dValue = 0.0;
                            string pErr = "";
                            string tmpexpress = ((System.Collections.Generic.KeyValuePair<string, bool>)(obj)).Key;
                            bool brunok = FormulaEngine.CheckCanEvaluate(tmpexpress, ref nResult, ref dValue, ref pErr);
                            if (brunok)
                            {
                                formulaevalute[tmpexpress] = true;
                            }
                        }
                        foreach (string tmpstr in formulaevalute.Keys)
                        {
                            if (formulacanevaluate.ContainsKey(tmpstr))
                            {
                                formulacanevaluate.Remove(tmpstr);
                            }
                        }
                        formulaevalute.Clear();
                        runnum++;
                        if (runnum >= maxevalcount) break;
                    }
                    while (formulacanevaluate.Count != 0);
                    if (runnum >= maxevalcount) MessageBox.Show("请检查输入的参数!", "提示：");
                    DataTable dt = FormulaEngineHtToTable();
                    gdcJsfGy.DataSource = dt.DefaultView;
                }
                else
                {
                    MessageBox.Show("FM_FormulaTemplate表中没有找到定义的公式!");
                }
            }
        }

        /// <summary>
        /// 将FormulaEngine中的HashTable转换成DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable FormulaEngineHtToTable()
        {
            DataTable dt;
            dt = new DataTable();
            DataColumn dc;
            //---------------------添加字段----------------
            //建立字段1
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.Int32");
            dc.ColumnName = "ID";
            dc.AutoIncrement = true;//标识
            dc.AutoIncrementSeed = 1;//标识种子
            dc.AutoIncrementStep = 1;//标识递增量
            dt.Columns.Add(dc);
            //建立字段2
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "名称";
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            //建立字段3
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "结果";
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);

            //设置主键
            DataColumn[] newdc = new DataColumn[1];//可设置多个字段为主键
            newdc[0] = dt.Columns["id"];
            dt.PrimaryKey = newdc;
            if (FormulaEngine.ht.Count >= 0)
            {

                foreach (object tmp in FormulaEngine.ht)
                {
                    DataRow dr = dt.NewRow();
                    dr["名称"] = ((System.Collections.DictionaryEntry)(tmp)).Key;
                    dr["结果"] = Convert.ToDouble(((System.Collections.DictionaryEntry)(tmp)).Value).ToString("#.##");
                    dt.Rows.Add(dr);
                }

            }
            return dt;

        }
        private void xtraTabPage2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            LanddatumMethod(false, "基准地价修正法");
            GetData();
        }

        private void textEdit11_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void sbCreateReport_Click(object sender, EventArgs e)
        {
            AutoEvalute();
        }

        private void AutoEvalute()
        {
            try
            {
                #region 土地使用权年期修正系数
                double YearCollectionResult, ProfitMargin, RemainYears, MaxUseYears;
                ProfitMargin = Convert.ToDouble(this.txtTdhyll.Text.ToString());
                RemainYears = Convert.ToDouble(this.txtTdsrlx.Text.ToString());
                MaxUseYears = Convert.ToDouble(this.txtTdsdnx.Text.ToString());
                YearCollectionResult = ((1.0 - (1.0 / Math.Pow(1.0 + ProfitMargin, RemainYears))) / (1.0 - (1.0 / Math.Pow(1.0 + ProfitMargin, (double)MaxUseYears))));
                this.txtNqxs.Text = YearCollectionResult.ToString("#.###");
                #endregion
                #region 容积率修正系数
                double minrange = 0, minrate = 0, maxrange = 0, maxrate = 0;
                double rjl = 0;
                double rjlxs = 0;
                double tmprjl = 0;
                double minrjl = 0;
                double maxrjl = 0;
                rjl = double.Parse(this.txtRjl.Text.ToString());

                if (landDatumValue.Tables["rjlsz"].Rows.Count > 0)
                {
                    minrjl = double.Parse(landDatumValue.Tables["rjlsz"].Rows[0]["rjl"].ToString());
                    maxrjl = double.Parse(landDatumValue.Tables["rjlsz"].Rows[landDatumValue.Tables["rjlsz"].Rows.Count - 1]["rjl"].ToString());
                    #region
                    if (minrjl <= rjl && rjl <= maxrjl)
                    {
                        tmprjl = rjl;
                    }
                    else if (rjl < minrjl)
                    {
                        tmprjl = minrjl;
                    }
                    else if (rjl > maxrjl)
                    {
                        tmprjl = maxrjl;
                    }
                    #endregion
                    if (this.landDatumValue.Tables["rjlsz"] != null)
                    {
                        this.landDatumValue.Tables["rjlsz"].DefaultView.RowFilter = string.Format("rjl > = {0}", tmprjl);
                        this.landDatumValue.Tables["rjlsz"].DefaultView.Sort = "rjl asc";
                        if (this.landDatumValue.Tables["rjlsz"].DefaultView.Count > 0)
                        {
                            maxrange = Convert.ToDouble(this.landDatumValue.Tables["rjlsz"].DefaultView[0]["rjl"].ToString());
                            maxrate = Convert.ToDouble(this.landDatumValue.Tables["rjlsz"].DefaultView[0]["szxs"].ToString());
                            this.landDatumValue.Tables["rjlsz"].DefaultView.RowFilter = string.Format("rjl < = {0}", "2.0");
                            this.landDatumValue.Tables["rjlsz"].DefaultView.Sort = "rjl desc";
                            minrange = Convert.ToDouble(this.landDatumValue.Tables["rjlsz"].DefaultView[0]["rjl"].ToString());
                            minrate = Convert.ToDouble(this.landDatumValue.Tables["rjlsz"].DefaultView[0]["szxs"].ToString());
                            if (minrange != maxrange)
                            {
                                rjlxs = maxrate + (((minrate - maxrate) * (rjl - maxrange)) / (minrange - maxrange));
                            }
                            else
                            {
                                rjlxs = minrate;
                            }
                        }

                    }

                }
                else
                {
                    rjlxs = 0;
                }
                this.txtRjlxs.Text = rjlxs.ToString("#.###");

                #endregion
                double result, AreaPrice, num = 0, num2;
                AreaPrice = Convert.ToDouble(txtJzdj.Text.ToString());
                num2 = Convert.ToDouble(this.txtQrxzxs.Text.ToString());
                DataTable dt;
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        num = num + Convert.ToDouble(dr["修正系数"].ToString());
                    }
                    result = (((((double)AreaPrice) * (1.0 + (((double)num)))) * YearCollectionResult) * num2) * ((double)rjlxs);
                    double tmpdb;
                    if (double.TryParse(this.txt土地面积.Text.ToString(), out tmpdb))
                    {
                        txtCbf基准地价修正法土地总价.Text = (result * double.Parse(this.txt土地面积.Text.ToString())).ToString("#");
                    }
                    else
                    {
                        MessageBox.Show("请输入土地面积!", "提示：");
                    }
                    MessageBox.Show(string.Format("基准地价-评估结果为:{0}元/每平方米", result.ToString("#")));
                }
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message.ToString());
            }

        }

        private void txtRjl_EditValueChanged(object sender, EventArgs e)
        {
            txtRjlxs.Text = "";
        }

        private void txtTdhyll_EditValueChanged(object sender, EventArgs e)
        {
            txtNqxs.Text = "";
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridView3_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["ID"], Guid.NewGuid());
            view.SetRowCellValue(e.RowHandle, view.Columns["MethodOrder"], view.DataRowCount + 1);
        }


        /// <summary>
        /// 保存FormulaTemplate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (SkyMap.Net.Security.SecurityUtil.GetSmPrincipal().IsInRole(this.GetType().FullName))
            {
                if (dataSetFMFormulaTemplate.HasChanges())
                {
                    this.fM_FormulaTemplateTableAdapter.Update(dataSetFMFormulaTemplate);
                    this.dataSetFMFormulaTemplate.AcceptChanges();
                }
            }
            else
            {
                MessageBox.Show("无相关权限!","提示：");
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Dictionary<string, double> ls = new Dictionary<string, double>();
            Dictionary<string, double> ls1 = new Dictionary<string, double>();
            FormulaEngine fe = new FormulaEngine();
            byte nResult = 0;
            double dValue = 0.0;
            string pErr = "";
            //FormulaEngine.ht["a"] = 10;
            //FormulaEngine.ht["b"] = 20;
            //FormulaEngine.ht["d"] = 20;
            this.fM_FormulaTemplateTableAdapter.FillBy(this.dataSetFMFormulaTemplate.FM_FormulaTemplate, "假设开发法", "商品房");

            if (this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Count > 0)
            {
                foreach (DataRow dr in this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Rows)
                {
                    FormulaEngine.GetMasterVariables(dr["MethodFormula"].ToString(), ref nResult, ref dValue, ref pErr, ref ls, ref ls1);
                }

                foreach (string tmp in ls1.Keys)
                {
                    if (ls.ContainsKey(tmp))
                    {
                        ls.Remove(tmp);
                    }
                }
                string masterstr;
                StringBuilder sb = new StringBuilder();
                foreach (string tmp in ls.Keys)
                {
                    sb.AppendFormat("{0}#", tmp);
                }
                masterstr = sb.ToString();

            }

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            JsfMethod(true, "假设开发法-工业用地");
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            JsfMethod(false, "假设开发法-工业用地");
        }


        /// <summary>
        /// 检查FM_FormulaTemplate中变量是否已经赋值
        /// </summary>
        /// <param name="dicvalue"></param>
        /// <param name="bigclass"></param>
        /// <param name="smallclass"></param>
        /// <returns>true,表示可以继续计算，false表示有些值没有输入，不能继续计算</returns>
        private bool CanEvaluate(Dictionary<string, string> dicvalue, string bigclass, string smallclass)
        {
            Dictionary<string, double> ls = new Dictionary<string, double>();
            Dictionary<string, double> ls1 = new Dictionary<string, double>();
            FormulaEngine fe = new FormulaEngine();
            byte nResult = 0;
            double dValue = 0.0;
            string pErr = "";
            this.fM_FormulaTemplateTableAdapter.FillBy(this.dataSetFMFormulaTemplate.FM_FormulaTemplate, bigclass, smallclass);

            if (this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Count > 0)
            {
                foreach (DataRow dr in this.dataSetFMFormulaTemplate.FM_FormulaTemplate.Rows)
                {
                    FormulaEngine.GetMasterVariables(dr["MethodFormula"].ToString(), ref nResult, ref dValue, ref pErr, ref ls, ref ls1);
                }

                foreach (string tmp in ls1.Keys)
                {
                    if (ls.ContainsKey(tmp))
                    {
                        ls.Remove(tmp);
                    }
                }

                foreach (string tmp in ls.Keys)
                {
                    if (!dicvalue.ContainsKey(tmp))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show(string.Format("CanEvaluate 在FM_FormulaTemplate表中无找到任何记录:{0}-{1}", bigclass, smallclass));
                return false;
            }
        }

        private void textEdit19_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btCbf1Save_Click(object sender, EventArgs e)
        {
            CbfMethod(true, "成本法结合基准地价修正法");
        }

        private void btCbf1Res_Click(object sender, EventArgs e)
        {
            CbfMethod(false, "成本法结合基准地价修正法");
        }

        private void btSyf1Save_Click(object sender, EventArgs e)
        {
            SyfMethod(true, "收益法结合成本法");
        }

        private void btSyf1Res_Click(object sender, EventArgs e)
        {
            SyfMethod(false, "收益法结合成本法");
        }

        private void textEdit29_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void xtraTabPage6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sbJsfspfSave_Click(object sender, EventArgs e)
        {
            JsfspfMethod(true, "假设开发法-商品房");
        }


    }
}
