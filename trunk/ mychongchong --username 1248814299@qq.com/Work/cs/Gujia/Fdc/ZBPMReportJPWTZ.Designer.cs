namespace ZBPM
{
    partial class ZBPMReportJPWTZ
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btn_select = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_town = new DevExpress.XtraEditors.LookUpEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_reportname = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cmb_reportType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_year = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.cmb_common = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chk是否累计 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_town.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_reportname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_reportType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_year.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_common.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk是否累计.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 14);
            this.toolTipController.SetSuperTip(this.label1, null);
            this.label1.TabIndex = 9;
            this.label1.Text = "请选择年度:";
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(381, 103);
            this.btn_select.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(75, 23);
            this.btn_select.TabIndex = 12;
            this.btn_select.Text = " 查询 ";
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 14);
            this.toolTipController.SetSuperTip(this.label2, null);
            this.label2.TabIndex = 9;
            this.label2.Text = "报表名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.toolTipController.SetSuperTip(this.label3, null);
            this.label3.TabIndex = 9;
            this.label3.Text = "镇   区:";
            // 
            // cmb_town
            // 
            this.cmb_town.Location = new System.Drawing.Point(130, 49);
            this.cmb_town.Name = "cmb_town";
            this.cmb_town.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_town.Properties.NullText = "";
            this.cmb_town.Size = new System.Drawing.Size(100, 21);
            this.cmb_town.TabIndex = 139;
            this.cmb_town.EditValueChanged += new System.EventHandler(this.cmb_town_EditValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.toolTipController.SetSuperTip(this.label4, null);
            this.label4.TabIndex = 9;
            this.label4.Text = "报表类型:";
            // 
            // cmb_reportname
            // 
            this.cmb_reportname.Location = new System.Drawing.Point(130, 23);
            this.cmb_reportname.Name = "cmb_reportname";
            this.cmb_reportname.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_reportname.Properties.Items.AddRange(new object[] {
            "招拍挂业务统计表.xls",
            "建设用地供应表.xls",
            "房地产价格监测表.xls",
            "土地出让金市级分成收入统计表.xls",
            "国有土地使用权配置统计表.xls",
            "土地成交地价款收入统计表.xls"});
            this.cmb_reportname.Size = new System.Drawing.Size(100, 21);
            this.cmb_reportname.TabIndex = 141;
            this.cmb_reportname.SelectedIndexChanged += new System.EventHandler(this.cmb_reportname_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(237, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 14);
            this.toolTipController.SetSuperTip(this.label5, null);
            this.label5.TabIndex = 142;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(267, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 14);
            this.toolTipController.SetSuperTip(this.label6, null);
            this.label6.TabIndex = 143;
            this.label6.Text = "(*)号标记内容必填";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(248, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 14);
            this.toolTipController.SetSuperTip(this.label7, null);
            this.label7.TabIndex = 142;
            this.label7.Text = "*";
            // 
            // cmb_reportType
            // 
            this.cmb_reportType.Location = new System.Drawing.Point(130, 76);
            this.cmb_reportType.Name = "cmb_reportType";
            this.cmb_reportType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_reportType.Properties.Items.AddRange(new object[] {
            "月度报表",
            "季度报表",
            "半年报表",
            "年度报表"});
            this.cmb_reportType.Size = new System.Drawing.Size(100, 21);
            this.cmb_reportType.TabIndex = 141;
            this.cmb_reportType.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // cmb_year
            // 
            this.cmb_year.Location = new System.Drawing.Point(130, 103);
            this.cmb_year.Name = "cmb_year";
            this.cmb_year.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_year.Size = new System.Drawing.Size(56, 21);
            this.cmb_year.TabIndex = 141;
            this.cmb_year.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(230, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 14);
            this.toolTipController.SetSuperTip(this.label8, null);
            this.label8.TabIndex = 142;
            this.label8.Text = "月";
            // 
            // cmb_common
            // 
            this.cmb_common.Location = new System.Drawing.Point(192, 103);
            this.cmb_common.Name = "cmb_common";
            this.cmb_common.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_common.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmb_common.Size = new System.Drawing.Size(38, 21);
            this.cmb_common.TabIndex = 141;
            this.cmb_common.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // chk是否累计
            // 
            this.chk是否累计.Location = new System.Drawing.Point(249, 24);
            this.chk是否累计.Name = "chk是否累计";
            this.chk是否累计.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk是否累计.Properties.Appearance.Options.UseFont = true;
            this.chk是否累计.Properties.Caption = "是否累计";
            this.chk是否累计.Size = new System.Drawing.Size(75, 19);
            this.chk是否累计.TabIndex = 144;
            this.chk是否累计.Visible = false;
            // 
            // ZBPMReportJPWTZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk是否累计);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmb_common);
            this.Controls.Add(this.cmb_year);
            this.Controls.Add(this.cmb_reportType);
            this.Controls.Add(this.cmb_reportname);
            this.Controls.Add(this.cmb_town);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ZBPMReportJPWTZ";
            this.Size = new System.Drawing.Size(500, 155);
            this.toolTipController.SetSuperTip(this, null);
            this.Load += new System.EventHandler(this.ZBPMReportJPWTZ_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmb_town.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_reportname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_reportType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_year.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_common.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk是否累计.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btn_select;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.LookUpEdit cmb_town;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_reportname;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_reportType;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_year;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_common;
        private DevExpress.XtraEditors.CheckEdit chk是否累计;
    }
}
