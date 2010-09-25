namespace SkyMap.Net.FDCRSFMXForm
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Security;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FDCRSFMXF : AbstractDataForm
    {
        private SimpleButton Btn_Yes;
        private CheckBox chb_selDate;
        private ComboBoxEdit cmb_Sfwtlp;
        private IContainer components = null;
        private DateEdit Fdate;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private DateEdit Odate;
        private TextEdit txt_fdczl;
        private TextEdit txt_fwyt;
        private TextEdit txt_PId;
        private TextEdit txt_srf;
        private TextEdit txt_tdyt;
        private TextEdit txt_zq;
        private TextEdit txt_zrf;
        private TextEdit txt_zrfs;
        private TextEdit txt_zrlx;

        public FDCRSFMXF()
        {
            this.InitializeComponent();
        }

        private void Btn_Yes_Click(object sender, EventArgs e)
        {
            string str = "";
            if (this.txt_PId.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "PROJECT_ID='" + this.txt_PId.Text + "' ";
            }
            if (this.txt_zrf.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "Zrf like '%" + this.txt_zrf.Text + "%' ";
            }
            if (this.txt_zq.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "zq like '%" + this.txt_zq.Text + "%' ";
            }
            if (this.txt_srf.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "Srf like '%" + this.txt_srf.Text + "%' ";
            }
            if (this.txt_tdyt.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "tdyt like '%" + this.txt_tdyt.Text + "%' ";
            }
            if (this.txt_fwyt.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "fwyt like '%" + this.txt_fwyt.Text + "%' ";
            }
            if (this.txt_zrlx.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "zrlx like '%" + this.txt_zrlx.Text + "%' ";
            }
            if (this.txt_zrfs.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "Zrfs like '%" + this.txt_zrfs.Text + "%' ";
            }
            if (this.txt_fdczl.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "fdczl like '%" + this.txt_fdczl.Text + "%' ";
            }
            if (this.cmb_Sfwtlp.Text != "")
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                str = str + "Sfwtlp like '%" + this.cmb_Sfwtlp.Text + "%' ";
            }
            if (this.chb_selDate.Checked)
            {
                if (!(str == ""))
                {
                    str = str + " and ";
                }
                string str2 = str;
                str = str2 + "Sjrq>='" + this.Fdate.Text + "' and Sjrq<='" + this.Odate.Text + "' ";
            }
            if (!(str == ""))
            {
                str = " where " + str;
            }
            Class1.aa = str;
            new RSF().ShowDialog();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FDCRSFMXF_Load(object sender, EventArgs e)
        {
            this.txt_zq.Text = SecurityUtil.GetSmPrincipal().DeptNames[0].ToString();
        }

        private void InitializeComponent()
        {
            this.chb_selDate = new CheckBox();
            this.Btn_Yes = new SimpleButton();
            this.groupBox2 = new GroupBox();
            this.label11 = new Label();
            this.Fdate = new DateEdit();
            this.Odate = new DateEdit();
            this.label12 = new Label();
            this.groupBox1 = new GroupBox();
            this.cmb_Sfwtlp = new ComboBoxEdit();
            this.label1 = new Label();
            this.label10 = new Label();
            this.label9 = new Label();
            this.txt_PId = new TextEdit();
            this.txt_zrlx = new TextEdit();
            this.label8 = new Label();
            this.txt_zrfs = new TextEdit();
            this.label6 = new Label();
            this.txt_fdczl = new TextEdit();
            this.label7 = new Label();
            this.label5 = new Label();
            this.txt_zrf = new TextEdit();
            this.txt_zq = new TextEdit();
            this.label2 = new Label();
            this.txt_srf = new TextEdit();
            this.txt_tdyt = new TextEdit();
            this.txt_fwyt = new TextEdit();
            this.label4 = new Label();
            this.label3 = new Label();
            this.groupBox2.SuspendLayout();
            this.Fdate.Properties.BeginInit();
            this.Odate.Properties.BeginInit();
            this.groupBox1.SuspendLayout();
            this.cmb_Sfwtlp.Properties.BeginInit();
            this.txt_PId.Properties.BeginInit();
            this.txt_zrlx.Properties.BeginInit();
            this.txt_zrfs.Properties.BeginInit();
            this.txt_fdczl.Properties.BeginInit();
            this.txt_zrf.Properties.BeginInit();
            this.txt_zq.Properties.BeginInit();
            this.txt_srf.Properties.BeginInit();
            this.txt_tdyt.Properties.BeginInit();
            this.txt_fwyt.Properties.BeginInit();
            base.SuspendLayout();
            this.chb_selDate.AutoSize = true;
            this.chb_selDate.Location = new Point(0x15, 0xdb);
            this.chb_selDate.Name = "chb_selDate";
            this.chb_selDate.Size = new Size(0x54, 0x10);
            this.chb_selDate.TabIndex = 30;
            this.chb_selDate.Text = "按日期查询";
            this.chb_selDate.UseVisualStyleBackColor = true;
            this.Btn_Yes.ButtonStyle = BorderStyles.Office2003;
            this.Btn_Yes.Location = new Point(0x111, 0x127);
            this.Btn_Yes.Name = "Btn_Yes";
            this.Btn_Yes.Size = new Size(0x4b, 0x17);
            this.Btn_Yes.TabIndex = 0x20;
            this.Btn_Yes.Text = "统计";
            this.Btn_Yes.Click += new EventHandler(this.Btn_Yes_Click);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.Fdate);
            this.groupBox2.Controls.Add(this.Odate);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Location = new Point(15, 0xe7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x251, 0x3a);
            this.groupBox2.TabIndex = 0x1f;
            this.groupBox2.TabStop = false;
            this.label11.AutoSize = true;
            this.label11.Location = new Point(10, 0x1a);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x41, 12);
            this.label11.TabIndex = 0x16;
            this.label11.Text = "收件日期：";
            this.Fdate.EditValue = new DateTime(0x7d6, 7, 0x19, 0, 0, 0, 0);
            this.Fdate.Location = new Point(0x51, 20);
            this.Fdate.Name = "Fdate";
            this.Fdate.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.Fdate.Size = new Size(0x99, 0x17);
            this.Fdate.TabIndex = 0x15;
            this.Odate.EditValue = new DateTime(0x7d6, 7, 0x19, 0, 0, 0, 0);
            this.Odate.Location = new Point(0x113, 20);
            this.Odate.Name = "Odate";
            this.Odate.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.Odate.Size = new Size(0x99, 0x17);
            this.Odate.TabIndex = 0x16;
            this.label12.AutoSize = true;
            this.label12.Location = new Point(240, 0x1a);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x1d, 12);
            this.label12.TabIndex = 0x17;
            this.label12.Text = " 至 ";
            this.groupBox1.Controls.Add(this.cmb_Sfwtlp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txt_PId);
            this.groupBox1.Controls.Add(this.txt_zrlx);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txt_zrfs);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txt_fdczl);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_zrf);
            this.groupBox1.Controls.Add(this.txt_zq);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_srf);
            this.groupBox1.Controls.Add(this.txt_tdyt);
            this.groupBox1.Controls.Add(this.txt_fwyt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(15, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x251, 190);
            this.groupBox1.TabIndex = 0x1d;
            this.groupBox1.TabStop = false;
            this.cmb_Sfwtlp.Location = new Point(0x65, 0x9c);
            this.cmb_Sfwtlp.Name = "cmb_Sfwtlp";
            this.cmb_Sfwtlp.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cmb_Sfwtlp.Properties.Items.AddRange(new object[] { "是", "否" });
            this.cmb_Sfwtlp.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.cmb_Sfwtlp.Size = new Size(100, 0x17);
            this.cmb_Sfwtlp.TabIndex = 0x15;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x12, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "业务号：";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x12, 0x2e);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x35, 12);
            this.label10.TabIndex = 0x13;
            this.label10.Text = "转让方：";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(6, 0xa2);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x59, 12);
            this.label9.TabIndex = 0x12;
            this.label9.Text = "是否问题楼盘：";
            this.txt_PId.Location = new Point(0x4d, 11);
            this.txt_PId.Name = "txt_PId";
            this.txt_PId.Size = new Size(0xcd, 0x17);
            this.txt_PId.TabIndex = 1;
            this.txt_zrlx.Location = new Point(0x4d, 0x62);
            this.txt_zrlx.Name = "txt_zrlx";
            this.txt_zrlx.Size = new Size(0xcd, 0x17);
            this.txt_zrlx.TabIndex = 5;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x143, 0x11);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x29, 12);
            this.label8.TabIndex = 0x11;
            this.label8.Text = "镇区：";
            this.txt_zrfs.EditValue = "";
            this.txt_zrfs.Location = new Point(370, 0x62);
            this.txt_zrfs.Name = "txt_zrfs";
            this.txt_zrfs.Size = new Size(0xcd, 0x17);
            this.txt_zrfs.TabIndex = 7;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x12b, 0x68);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x41, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "转让方式：";
            this.txt_fdczl.Location = new Point(0x5b, 0x7f);
            this.txt_fdczl.Name = "txt_fdczl";
            this.txt_fdczl.Size = new Size(0x1e4, 0x17);
            this.txt_fdczl.TabIndex = 3;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(8, 0x85);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x4d, 12);
            this.label7.TabIndex = 0x10;
            this.label7.Text = "房地产座落：";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(8, 0x68);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x41, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "转让类型：";
            this.txt_zrf.Location = new Point(0x4d, 40);
            this.txt_zrf.Name = "txt_zrf";
            this.txt_zrf.Size = new Size(0xcd, 0x17);
            this.txt_zrf.TabIndex = 2;
            this.txt_zq.Location = new Point(370, 11);
            this.txt_zq.Name = "txt_zq";
            this.txt_zq.Properties.ReadOnly = true;
            this.txt_zq.Size = new Size(0xcd, 0x17);
            this.txt_zq.TabIndex = 8;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x137, 0x2e);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "受让方：";
            this.txt_srf.Location = new Point(370, 40);
            this.txt_srf.Name = "txt_srf";
            this.txt_srf.Size = new Size(0xcd, 0x17);
            this.txt_srf.TabIndex = 4;
            this.txt_tdyt.Location = new Point(0x4d, 0x45);
            this.txt_tdyt.Name = "txt_tdyt";
            this.txt_tdyt.Size = new Size(0xcd, 0x17);
            this.txt_tdyt.TabIndex = 9;
            this.txt_fwyt.Location = new Point(370, 0x45);
            this.txt_fwyt.Name = "txt_fwyt";
            this.txt_fwyt.Size = new Size(0xcd, 0x17);
            this.txt_fwyt.TabIndex = 10;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x12b, 0x4b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x41, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "房屋用途：";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 0x4b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x41, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "土地用途：";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.chb_selDate);
            base.Controls.Add(this.Btn_Yes);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Name = "FDCRSFMXF";
            base.Size = new Size(620, 0x14c);
            base.Load += new EventHandler(this.FDCRSFMXF_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.Fdate.Properties.EndInit();
            this.Odate.Properties.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cmb_Sfwtlp.Properties.EndInit();
            this.txt_PId.Properties.EndInit();
            this.txt_zrlx.Properties.EndInit();
            this.txt_zrfs.Properties.EndInit();
            this.txt_fdczl.Properties.EndInit();
            this.txt_zrf.Properties.EndInit();
            this.txt_zq.Properties.EndInit();
            this.txt_srf.Properties.EndInit();
            this.txt_tdyt.Properties.EndInit();
            this.txt_fwyt.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override bool LoadMe()
        {
            return true;
        }
    }
}

