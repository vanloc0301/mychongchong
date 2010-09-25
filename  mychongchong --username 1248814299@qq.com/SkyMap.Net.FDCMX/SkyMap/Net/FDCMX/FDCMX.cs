namespace SkyMap.Net.FDCMX
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using Microsoft.Reporting.WinForms;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class FDCMX : AbstractDataForm
    {
        private BindingSource BSource;
        private SimpleButton Btn_select;
        private IContainer components = null;
        private ComboBoxEdit Fdate_Month;
        private ComboBoxEdit Fdate_Year;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private ComboBoxEdit Odate_Month;
        private ComboBoxEdit Odate_Year;
        private ReportDataSource reportData = new ReportDataSource();
        private ReportViewer ReportV_FDCMX;

        public FDCMX()
        {
            this.InitializeComponent();
        }

        private void Btn_select_Click(object sender, EventArgs e)
        {
            string str3;
            int num;
            string str = this.Fdate_Year.Text + "-" + this.Fdate_Month.Text;
            string str2 = this.Odate_Year.Text + "-" + this.Odate_Month.Text;
            if (Convert.ToDateTime(str) < Convert.ToDateTime(str2))
            {
                num = (((Convert.ToInt32(this.Odate_Year.Text) - Convert.ToInt32(this.Fdate_Year.Text)) * 12) + Convert.ToInt32(this.Odate_Month.Text)) - Convert.ToInt32(this.Fdate_Month.Text);
            }
            else
            {
                str3 = str;
                str = str2;
                str2 = str3;
                num = (((Convert.ToInt32(this.Fdate_Year.Text) - Convert.ToInt32(this.Odate_Year.Text)) * 12) + Convert.ToInt32(this.Fdate_Month.Text)) - Convert.ToInt32(this.Odate_Month.Text);
            }
            str3 = "(" + this.Fdate_Year.Text + "年" + this.Fdate_Month.Text + "月至" + this.Odate_Year.Text + "年" + this.Odate_Month.Text + "月)";
            this.SelectData(Convert.ToDateTime(str), Convert.ToDateTime(str2), SecurityUtil.GetSmPrincipal().DeptNames[0], str3, num + 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FDCMX_Load(object sender, EventArgs e)
        {
            for (int i = DateTime.Today.Year; i >= DateTime.Today.AddYears(-50).Year; i--)
            {
                this.Fdate_Year.Properties.Items.Add(i.ToString() ?? "");
                this.Odate_Year.Properties.Items.Add(i.ToString() ?? "");
            }
            this.Fdate_Month.Text = DateTime.Today.Month.ToString();
            this.Odate_Month.Text = DateTime.Today.Month.ToString();
            this.Fdate_Year.Text = DateTime.Today.Year.ToString();
            this.Odate_Year.Text = DateTime.Today.Year.ToString();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.ReportV_FDCMX = new ReportViewer();
            this.Fdate_Year = new ComboBoxEdit();
            this.Odate_Year = new ComboBoxEdit();
            this.label1 = new Label();
            this.label2 = new Label();
            this.Btn_select = new SimpleButton();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.Odate_Month = new ComboBoxEdit();
            this.Fdate_Month = new ComboBoxEdit();
            this.BSource = new BindingSource(this.components);
            this.Fdate_Year.Properties.BeginInit();
            this.Odate_Year.Properties.BeginInit();
            this.Odate_Month.Properties.BeginInit();
            this.Fdate_Month.Properties.BeginInit();
            ((ISupportInitialize) this.BSource).BeginInit();
            base.SuspendLayout();
            this.ReportV_FDCMX.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.ReportV_FDCMX.LocalReport.ReportEmbeddedResource = "SkyMap.Net.FDCMX.Report_FDCMX.rdlc";
            this.ReportV_FDCMX.Location = new Point(0x11, 40);
            this.ReportV_FDCMX.Name = "ReportV_FDCMX";
            this.ReportV_FDCMX.Size = new Size(0x327, 510);
            this.ReportV_FDCMX.TabIndex = 0;
            this.Fdate_Year.Location = new Point(0x56, 11);
            this.Fdate_Year.Name = "Fdate_Year";
            this.Fdate_Year.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.Fdate_Year.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.Fdate_Year.Size = new Size(0x48, 0x17);
            this.Fdate_Year.TabIndex = 3;
            this.Odate_Year.Location = new Point(0x138, 11);
            this.Odate_Year.Name = "Odate_Year";
            this.Odate_Year.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.Odate_Year.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.Odate_Year.Size = new Size(0x48, 0x17);
            this.Odate_Year.TabIndex = 4;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "统计日期：";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x115, 0x11);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1d, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = " 至 ";
            this.Btn_select.Location = new Point(0x1f7, 11);
            this.Btn_select.Name = "Btn_select";
            this.Btn_select.Size = new Size(0x4b, 0x17);
            this.Btn_select.TabIndex = 7;
            this.Btn_select.Text = "查询";
            this.Btn_select.Click += new EventHandler(this.Btn_select_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xa4, 0x11);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x11, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "年";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(390, 0x11);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x11, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "年";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0xfe, 0x11);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x11, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "月";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(480, 0x11);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x11, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "月";
            this.Odate_Month.Location = new Point(0x19d, 11);
            this.Odate_Month.Name = "Odate_Month";
            this.Odate_Month.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.Odate_Month.Properties.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            this.Odate_Month.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.Odate_Month.Size = new Size(0x3d, 0x17);
            this.Odate_Month.TabIndex = 12;
            this.Fdate_Month.Location = new Point(0xbb, 11);
            this.Fdate_Month.Name = "Fdate_Month";
            this.Fdate_Month.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.Fdate_Month.Properties.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            this.Fdate_Month.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.Fdate_Month.Size = new Size(0x3d, 0x17);
            this.Fdate_Month.TabIndex = 13;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Fdate_Month);
            base.Controls.Add(this.Odate_Month);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.Btn_select);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.Odate_Year);
            base.Controls.Add(this.Fdate_Year);
            base.Controls.Add(this.ReportV_FDCMX);
            base.Name = "FDCMX";
            base.Size = new Size(0x344, 0x229);
            base.Load += new EventHandler(this.FDCMX_Load);
            this.Fdate_Year.Properties.EndInit();
            this.Odate_Year.Properties.EndInit();
            this.Odate_Month.Properties.EndInit();
            this.Fdate_Month.Properties.EndInit();
            ((ISupportInitialize) this.BSource).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override bool LoadMe()
        {
            return true;
        }

        private void SelectData(DateTime Fdate, DateTime Odate, string zq, string F, int count)
        {
            DataTable table = QueryHelper.ExecuteSql("FDC", string.Empty, "select zrlx as '转让类型',count(zrlx) as '宗数',sum(Tdmj) as '土地面积',sum(Ksjg) as '产价' From YW_ZQRSF  where Sjrq>='" + Fdate.ToShortDateString() + "' and Sjrq<'" + Odate.AddMonths(1).ToShortDateString() + "' and Zq Like '%" + zq + "%' and (zrlx='空地' or zrlx='房地产') group by Zrlx order by zrlx");
            this.BSource.DataSource = table;
            this.reportData.Name = "BSource";
            this.reportData.Value = this.BSource;
            this.ReportV_FDCMX.LocalReport.DataSources.Add(this.reportData);
            this.ReportV_FDCMX.LocalReport.ReportEmbeddedResource = "SkyMap.Net.FDCMX.Report_FDCMX.rdlc";
            ReportParameter item = new ReportParameter("MonthCount", count.ToString());
            List<ReportParameter> parameters = new List<ReportParameter>();
            ReportParameter parameter2 = new ReportParameter("DateTime", F);
            ReportParameter parameter3 = new ReportParameter("CDept", SecurityUtil.GetSmPrincipal().DeptNames[0]);
            ReportParameter parameter4 = new ReportParameter();
            ReportParameter parameter5 = new ReportParameter();
            ReportParameter parameter6 = new ReportParameter();
            ReportParameter parameter7 = new ReportParameter();
            ReportParameter parameter8 = new ReportParameter();
            ReportParameter parameter9 = new ReportParameter();
            parameter7 = new ReportParameter("kdzrmj", "0");
            parameter8 = new ReportParameter("kdcj", "0");
            parameter9 = new ReportParameter("kdsum", "0");
            parameter4 = new ReportParameter("fdcsum", "0");
            parameter5 = new ReportParameter("fdczrmj", "0");
            parameter6 = new ReportParameter("fdccj", "0");
            if (table.Rows.Count > 0)
            {
                string str4;
                string str5;
                string str6;
                if (table.Rows[0].ItemArray[0].ToString() == "房地产")
                {
                    string str;
                    string str2;
                    string str3;
                    if (table.Rows[0].ItemArray[1].ToString() == "")
                    {
                        str = "0";
                    }
                    else
                    {
                        str = table.Rows[0].ItemArray[1].ToString();
                    }
                    if (table.Rows[0].ItemArray[2].ToString() == "")
                    {
                        str2 = "0";
                    }
                    else
                    {
                        str2 = table.Rows[0].ItemArray[2].ToString();
                    }
                    if (table.Rows[0].ItemArray[3].ToString() == "")
                    {
                        str3 = "0";
                    }
                    else
                    {
                        str3 = table.Rows[0].ItemArray[3].ToString();
                    }
                    parameter4 = new ReportParameter("fdcsum", str);
                    parameter5 = new ReportParameter("fdczrmj", str2);
                    parameter6 = new ReportParameter("fdccj", str3);
                    if (table.Rows.Count == 2)
                    {
                        if (table.Rows[1].ItemArray[2].ToString() == "")
                        {
                            str4 = "0";
                        }
                        else
                        {
                            str4 = table.Rows[1].ItemArray[2].ToString();
                        }
                        if (table.Rows[1].ItemArray[3].ToString() == "")
                        {
                            str5 = "0";
                        }
                        else
                        {
                            str5 = table.Rows[1].ItemArray[3].ToString();
                        }
                        if (table.Rows[1].ItemArray[1].ToString() == "")
                        {
                            str6 = "0";
                        }
                        else
                        {
                            str6 = table.Rows[1].ItemArray[1].ToString();
                        }
                        parameter7 = new ReportParameter("kdzrmj", str4);
                        parameter8 = new ReportParameter("kdcj", str5);
                        parameter9 = new ReportParameter("kdsum", str6);
                    }
                }
                else if (table.Rows[0].ItemArray[0].ToString() == "空地")
                {
                    if (table.Rows[0].ItemArray[2].ToString() == "")
                    {
                        str4 = "0";
                    }
                    else
                    {
                        str4 = table.Rows[0].ItemArray[2].ToString();
                    }
                    if (table.Rows[0].ItemArray[3].ToString() == "")
                    {
                        str5 = "0";
                    }
                    else
                    {
                        str5 = table.Rows[0].ItemArray[3].ToString();
                    }
                    if (table.Rows[0].ItemArray[1].ToString() == "")
                    {
                        str6 = "0";
                    }
                    else
                    {
                        str6 = table.Rows[0].ItemArray[1].ToString();
                    }
                    parameter7 = new ReportParameter("kdzrmj", str4);
                    parameter8 = new ReportParameter("kdcj", str5);
                    parameter9 = new ReportParameter("kdsum", str6);
                }
            }
            parameters.Add(parameter9);
            parameters.Add(parameter6);
            parameters.Add(parameter4);
            parameters.Add(parameter8);
            parameters.Add(parameter7);
            parameters.Add(parameter5);
            parameters.Add(parameter2);
            parameters.Add(parameter3);
            parameters.Add(item);
            this.ReportV_FDCMX.LocalReport.SetParameters(parameters);
            this.ReportV_FDCMX.RefreshReport();
        }
    }
}

