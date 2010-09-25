namespace ZBPM
{
    partial class ZBPMReportCJYW
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
            this.components = new System.ComponentModel.Container();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btn_select = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.m_dateStart = new DevExpress.XtraEditors.DateEdit();
            this.Chk_ywlx = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_ywlx = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.Chk_jyfs = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_jyfs = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.Chk_zq = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_zq = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.zszbpmDataSet = new ZBPM.zszbpmDataSet();
            this.vWtdzbpmbBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vW_tdzbpm_bTableAdapter = new ZBPM.zszbpmDataSetTableAdapters.VW_tdzbpm_bTableAdapter();
            this.Chk_是否成交 = new DevExpress.XtraEditors.CheckEdit();
            this.chk土地用途 = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_yt = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.cmb成交后状态 = new DevExpress.XtraEditors.CheckedListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_ywlx.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_ywlx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_jyfs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_jyfs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_zq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_zq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zszbpmDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vWtdzbpmbBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_是否成交.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk土地用途.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_yt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb成交后状态)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.Location = new System.Drawing.Point(-3, 233);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(874, 192);
            this.toolTipController.SetSuperTip(this.reportViewer1, null);
            this.reportViewer1.TabIndex = 18;
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(339, 8);
            this.btn_select.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(75, 23);
            this.btn_select.TabIndex = 17;
            this.btn_select.Text = " 查询 ";
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.toolTipController.SetSuperTip(this.label2, null);
            this.label2.TabIndex = 16;
            this.label2.Text = "结束时间";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.toolTipController.SetSuperTip(this.label1, null);
            this.label1.TabIndex = 15;
            this.label1.Text = "起始时间";
            // 
            // m_dateEnd
            // 
            this.m_dateEnd.EditValue = new System.DateTime(2005, 8, 29, 0, 0, 0, 0);
            this.m_dateEnd.Location = new System.Drawing.Point(245, 10);
            this.m_dateEnd.Name = "m_dateEnd";
            this.m_dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.m_dateEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.m_dateEnd.Size = new System.Drawing.Size(88, 21);
            this.m_dateEnd.TabIndex = 53;
            // 
            // m_dateStart
            // 
            this.m_dateStart.EditValue = new System.DateTime(2005, 8, 29, 0, 0, 0, 0);
            this.m_dateStart.Location = new System.Drawing.Point(90, 9);
            this.m_dateStart.Name = "m_dateStart";
            this.m_dateStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.m_dateStart.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.m_dateStart.Size = new System.Drawing.Size(88, 21);
            this.m_dateStart.TabIndex = 52;
            // 
            // Chk_ywlx
            // 
            this.Chk_ywlx.Location = new System.Drawing.Point(32, 54);
            this.Chk_ywlx.Name = "Chk_ywlx";
            this.Chk_ywlx.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_ywlx.Properties.Appearance.Options.UseFont = true;
            this.Chk_ywlx.Properties.Caption = "业务类型";
            this.Chk_ywlx.Size = new System.Drawing.Size(75, 19);
            this.Chk_ywlx.TabIndex = 54;
            // 
            // CLB_ywlx
            // 
            this.CLB_ywlx.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("转让"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("出让")});
            this.CLB_ywlx.Location = new System.Drawing.Point(32, 79);
            this.CLB_ywlx.Name = "CLB_ywlx";
            this.CLB_ywlx.Size = new System.Drawing.Size(104, 95);
            this.CLB_ywlx.TabIndex = 55;
            // 
            // Chk_jyfs
            // 
            this.Chk_jyfs.Location = new System.Drawing.Point(158, 54);
            this.Chk_jyfs.Name = "Chk_jyfs";
            this.Chk_jyfs.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_jyfs.Properties.Appearance.Options.UseFont = true;
            this.Chk_jyfs.Properties.Caption = "交易方式";
            this.Chk_jyfs.Size = new System.Drawing.Size(75, 19);
            this.Chk_jyfs.TabIndex = 56;
            // 
            // CLB_jyfs
            // 
            this.CLB_jyfs.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("招标"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("拍卖"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("挂牌"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("网上竞价")});
            this.CLB_jyfs.Location = new System.Drawing.Point(158, 79);
            this.CLB_jyfs.Name = "CLB_jyfs";
            this.CLB_jyfs.Size = new System.Drawing.Size(104, 95);
            this.CLB_jyfs.TabIndex = 57;
            // 
            // Chk_zq
            // 
            this.Chk_zq.Location = new System.Drawing.Point(284, 54);
            this.Chk_zq.Name = "Chk_zq";
            this.Chk_zq.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_zq.Properties.Appearance.Options.UseFont = true;
            this.Chk_zq.Properties.Caption = "镇区";
            this.Chk_zq.Size = new System.Drawing.Size(75, 19);
            this.Chk_zq.TabIndex = 58;
            // 
            // CLB_zq
            // 
            this.CLB_zq.Location = new System.Drawing.Point(284, 79);
            this.CLB_zq.Name = "CLB_zq";
            this.CLB_zq.Size = new System.Drawing.Size(104, 95);
            this.CLB_zq.TabIndex = 59;
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(755, 148);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(100, 21);
            this.textEdit1.TabIndex = 61;
            this.textEdit1.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(674, 146);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 62;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Visible = false;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // zszbpmDataSet
            // 
            this.zszbpmDataSet.DataSetName = "zszbpmDataSet";
            this.zszbpmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // vWtdzbpmbBindingSource
            // 
            this.vWtdzbpmbBindingSource.DataMember = "VW_tdzbpm_b";
            this.vWtdzbpmbBindingSource.DataSource = this.zszbpmDataSet;
            // 
            // vW_tdzbpm_bTableAdapter
            // 
            this.vW_tdzbpm_bTableAdapter.ClearBeforeFill = true;
            // 
            // Chk_是否成交
            // 
            this.Chk_是否成交.Location = new System.Drawing.Point(536, 54);
            this.Chk_是否成交.Name = "Chk_是否成交";
            this.Chk_是否成交.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_是否成交.Properties.Appearance.Options.UseFont = true;
            this.Chk_是否成交.Properties.Caption = "是否成交";
            this.Chk_是否成交.Size = new System.Drawing.Size(75, 19);
            this.Chk_是否成交.TabIndex = 58;
            this.Chk_是否成交.CheckedChanged += new System.EventHandler(this.Chk_是否成交_CheckedChanged);
            // 
            // chk土地用途
            // 
            this.chk土地用途.Location = new System.Drawing.Point(410, 54);
            this.chk土地用途.Name = "chk土地用途";
            this.chk土地用途.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk土地用途.Properties.Appearance.Options.UseFont = true;
            this.chk土地用途.Properties.Caption = "土地用途";
            this.chk土地用途.Size = new System.Drawing.Size(75, 19);
            this.chk土地用途.TabIndex = 63;
            // 
            // CLB_yt
            // 
            this.CLB_yt.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("工业"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("商业"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("商住"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("住宅"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("仓储"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("工业\\商住"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("居住"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("商住\\办公"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("办公及其它"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("商品住宅"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("商业\\商住"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("商业\\市政绿化"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("码头\\广场")});
            this.CLB_yt.Location = new System.Drawing.Point(410, 79);
            this.CLB_yt.Name = "CLB_yt";
            this.CLB_yt.Size = new System.Drawing.Size(104, 95);
            this.CLB_yt.TabIndex = 64;
            // 
            // cmb成交后状态
            // 
            this.cmb成交后状态.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("付款中业务"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("土地交付中业务"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("结案业务"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("违约业务")});
            this.cmb成交后状态.Location = new System.Drawing.Point(536, 79);
            this.cmb成交后状态.Name = "cmb成交后状态";
            this.cmb成交后状态.Size = new System.Drawing.Size(104, 95);
            this.cmb成交后状态.TabIndex = 126;
            this.cmb成交后状态.Visible = false;
            // 
            // ZBPMReportCJYW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmb成交后状态);
            this.Controls.Add(this.CLB_yt);
            this.Controls.Add(this.chk土地用途);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.CLB_zq);
            this.Controls.Add(this.Chk_是否成交);
            this.Controls.Add(this.Chk_zq);
            this.Controls.Add(this.CLB_jyfs);
            this.Controls.Add(this.Chk_jyfs);
            this.Controls.Add(this.CLB_ywlx);
            this.Controls.Add(this.Chk_ywlx);
            this.Controls.Add(this.m_dateEnd);
            this.Controls.Add(this.m_dateStart);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ZBPMReportCJYW";
            this.Size = new System.Drawing.Size(874, 478);
            this.toolTipController.SetSuperTip(this, null);
            this.Load += new System.EventHandler(this.ZBPMReportCJYW_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_ywlx.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_ywlx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_jyfs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_jyfs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_zq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_zq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zszbpmDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vWtdzbpmbBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_是否成交.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk土地用途.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_yt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb成交后状态)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DevExpress.XtraEditors.SimpleButton btn_select;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit m_dateEnd;
        private DevExpress.XtraEditors.DateEdit m_dateStart;
        private DevExpress.XtraEditors.CheckEdit Chk_ywlx;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_ywlx;
        private DevExpress.XtraEditors.CheckEdit Chk_jyfs;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_jyfs;
        private DevExpress.XtraEditors.CheckEdit Chk_zq;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_zq;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private zszbpmDataSet zszbpmDataSet;
        private System.Windows.Forms.BindingSource vWtdzbpmbBindingSource;
        private ZBPM.zszbpmDataSetTableAdapters.VW_tdzbpm_bTableAdapter vW_tdzbpm_bTableAdapter;
        private DevExpress.XtraEditors.CheckEdit Chk_是否成交;
        private DevExpress.XtraEditors.CheckEdit chk土地用途;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_yt;
        private DevExpress.XtraEditors.CheckedListBoxControl cmb成交后状态;

    }
}
