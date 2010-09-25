namespace ZBPM
{
    partial class ZBPMReportJQYW
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
            this.label2 = new System.Windows.Forms.Label();
            this.btn_select = new DevExpress.XtraEditors.SimpleButton();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.m_dateStart = new DevExpress.XtraEditors.DateEdit();
            this.m_dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.CLB_zq = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.Chk_zq = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_jyfs = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.Chk_jyfs = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_ywlx = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.Chk_ywlx = new DevExpress.XtraEditors.CheckEdit();
            this.chk土地用途 = new DevExpress.XtraEditors.CheckEdit();
            this.CLB_yt = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.Chk_End = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_zq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_zq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_jyfs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_jyfs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_ywlx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_ywlx.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk土地用途.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_yt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_End.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.toolTipController.SetSuperTip(this.label1, null);
            this.label1.TabIndex = 9;
            this.label1.Text = "起始时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.toolTipController.SetSuperTip(this.label2, null);
            this.label2.TabIndex = 10;
            this.label2.Text = "结束时间";
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(547, 15);
            this.btn_select.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(75, 23);
            this.btn_select.TabIndex = 12;
            this.btn_select.Text = " 查询 ";
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.Location = new System.Drawing.Point(0, 192);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(834, 284);
            this.toolTipController.SetSuperTip(this.reportViewer1, null);
            this.reportViewer1.TabIndex = 13;
            // 
            // m_dateStart
            // 
            this.m_dateStart.EditValue = new System.DateTime(2005, 8, 29, 0, 0, 0, 0);
            this.m_dateStart.Location = new System.Drawing.Point(90, 17);
            this.m_dateStart.Name = "m_dateStart";
            this.m_dateStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.m_dateStart.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.m_dateStart.Size = new System.Drawing.Size(88, 21);
            this.m_dateStart.TabIndex = 50;
            // 
            // m_dateEnd
            // 
            this.m_dateEnd.EditValue = new System.DateTime(2005, 8, 29, 0, 0, 0, 0);
            this.m_dateEnd.Location = new System.Drawing.Point(245, 17);
            this.m_dateEnd.Name = "m_dateEnd";
            this.m_dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.m_dateEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.m_dateEnd.Size = new System.Drawing.Size(88, 21);
            this.m_dateEnd.TabIndex = 51;
            // 
            // CLB_zq
            // 
            this.CLB_zq.Location = new System.Drawing.Point(298, 81);
            this.CLB_zq.Name = "CLB_zq";
            this.CLB_zq.Size = new System.Drawing.Size(84, 95);
            this.CLB_zq.TabIndex = 65;
            // 
            // Chk_zq
            // 
            this.Chk_zq.Location = new System.Drawing.Point(296, 56);
            this.Chk_zq.Name = "Chk_zq";
            this.Chk_zq.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_zq.Properties.Appearance.Options.UseFont = true;
            this.Chk_zq.Properties.Caption = "镇区";
            this.Chk_zq.Size = new System.Drawing.Size(75, 19);
            this.Chk_zq.TabIndex = 64;
            // 
            // CLB_jyfs
            // 
            this.CLB_jyfs.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("招标"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("拍卖"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("挂牌"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("网上竞价")});
            this.CLB_jyfs.Location = new System.Drawing.Point(164, 81);
            this.CLB_jyfs.Name = "CLB_jyfs";
            this.CLB_jyfs.Size = new System.Drawing.Size(75, 95);
            this.CLB_jyfs.TabIndex = 63;
            // 
            // Chk_jyfs
            // 
            this.Chk_jyfs.Location = new System.Drawing.Point(164, 56);
            this.Chk_jyfs.Name = "Chk_jyfs";
            this.Chk_jyfs.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_jyfs.Properties.Appearance.Options.UseFont = true;
            this.Chk_jyfs.Properties.Caption = "交易方式";
            this.Chk_jyfs.Size = new System.Drawing.Size(75, 19);
            this.Chk_jyfs.TabIndex = 62;
            // 
            // CLB_ywlx
            // 
            this.CLB_ywlx.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("转让"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("出让")});
            this.CLB_ywlx.Location = new System.Drawing.Point(30, 81);
            this.CLB_ywlx.Name = "CLB_ywlx";
            this.CLB_ywlx.Size = new System.Drawing.Size(75, 95);
            this.CLB_ywlx.TabIndex = 61;
            // 
            // Chk_ywlx
            // 
            this.Chk_ywlx.Location = new System.Drawing.Point(30, 56);
            this.Chk_ywlx.Name = "Chk_ywlx";
            this.Chk_ywlx.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_ywlx.Properties.Appearance.Options.UseFont = true;
            this.Chk_ywlx.Properties.Caption = "业务类型";
            this.Chk_ywlx.Size = new System.Drawing.Size(75, 19);
            this.Chk_ywlx.TabIndex = 60;
            // 
            // chk土地用途
            // 
            this.chk土地用途.Location = new System.Drawing.Point(428, 56);
            this.chk土地用途.Name = "chk土地用途";
            this.chk土地用途.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk土地用途.Properties.Appearance.Options.UseFont = true;
            this.chk土地用途.Properties.Caption = "土地用途";
            this.chk土地用途.Size = new System.Drawing.Size(75, 19);
            this.chk土地用途.TabIndex = 66;
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
            this.CLB_yt.Location = new System.Drawing.Point(430, 81);
            this.CLB_yt.Name = "CLB_yt";
            this.CLB_yt.Size = new System.Drawing.Size(107, 95);
            this.CLB_yt.TabIndex = 67;
            // 
            // Chk_End
            // 
            this.Chk_End.Location = new System.Drawing.Point(339, 19);
            this.Chk_End.Name = "Chk_End";
            this.Chk_End.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chk_End.Properties.Appearance.Options.UseFont = true;
            this.Chk_End.Properties.Caption = "公告结束时间在选择时间范围内";
            this.Chk_End.Size = new System.Drawing.Size(202, 19);
            this.Chk_End.TabIndex = 68;
            // 
            // ZBPMReportJQYW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Chk_End);
            this.Controls.Add(this.CLB_yt);
            this.Controls.Add(this.chk土地用途);
            this.Controls.Add(this.CLB_zq);
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
            this.Name = "ZBPMReportJQYW";
            this.Size = new System.Drawing.Size(834, 476);
            this.toolTipController.SetSuperTip(this, null);
            this.Load += new System.EventHandler(this.ZBPMReportJQYW_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_zq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_zq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_jyfs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_jyfs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_ywlx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_ywlx.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk土地用途.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CLB_yt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chk_End.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btn_select;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DevExpress.XtraEditors.DateEdit m_dateStart;
        private DevExpress.XtraEditors.DateEdit m_dateEnd;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_zq;
        private DevExpress.XtraEditors.CheckEdit Chk_zq;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_jyfs;
        private DevExpress.XtraEditors.CheckEdit Chk_jyfs;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_ywlx;
        private DevExpress.XtraEditors.CheckEdit Chk_ywlx;
        private DevExpress.XtraEditors.CheckEdit chk土地用途;
        private DevExpress.XtraEditors.CheckedListBoxControl CLB_yt;
        private DevExpress.XtraEditors.CheckEdit Chk_End;
    }
}
