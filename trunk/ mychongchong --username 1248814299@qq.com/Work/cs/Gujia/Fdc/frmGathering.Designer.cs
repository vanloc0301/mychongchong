namespace ZBPM
{
    partial class frmGathering
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.grid_djksq = new SkyMap.Net.Gui.Components.SmGridControl();
            this.gridView_djksq = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.支付日期 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.支付比例 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.controlNavigator2 = new DevExpress.XtraEditors.ControlNavigator();
            this.m_btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid_djksq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_djksq)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_djksq
            // 
            this.grid_djksq.EmbeddedNavigator.Buttons.NextPage.Visible = false;
            this.grid_djksq.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
            this.grid_djksq.EmbeddedNavigator.Name = "";
            this.grid_djksq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.grid_djksq.Location = new System.Drawing.Point(17, 12);
            this.grid_djksq.MainView = this.gridView_djksq;
            this.grid_djksq.Name = "grid_djksq";
            this.grid_djksq.Size = new System.Drawing.Size(602, 211);
            this.grid_djksq.TabIndex = 99;
            this.grid_djksq.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_djksq});
            // 
            // gridView_djksq
            // 
            this.gridView_djksq.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.支付日期,
            this.支付比例,
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn2});
            this.gridView_djksq.GridControl = this.grid_djksq;
            this.gridView_djksq.Name = "gridView_djksq";
            this.gridView_djksq.OptionsView.ShowGroupPanel = false;
            // 
            // 支付日期
            // 
            this.支付日期.Caption = "支付日期";
            this.支付日期.FieldName = "支付日期";
            this.支付日期.Name = "支付日期";
            this.支付日期.Visible = true;
            this.支付日期.VisibleIndex = 0;
            // 
            // 支付比例
            // 
            this.支付比例.Caption = "支付比例";
            this.支付比例.FieldName = "支付比例";
            this.支付比例.Name = "支付比例";
            this.支付比例.Visible = true;
            this.支付比例.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "支付金额";
            this.gridColumn4.FieldName = "支付金额";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "实际支付日期";
            this.gridColumn1.FieldName = "实际支付日期";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "实际支付比例";
            this.gridColumn3.FieldName = "实际支付比例";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "实际支付金额";
            this.gridColumn2.FieldName = "实际支付金额";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 5;
            // 
            // controlNavigator2
            // 
            this.controlNavigator2.Location = new System.Drawing.Point(17, 229);
            this.controlNavigator2.Name = "controlNavigator2";
            this.controlNavigator2.NavigatableControl = this.grid_djksq;
            this.controlNavigator2.Size = new System.Drawing.Size(224, 24);
            this.controlNavigator2.TabIndex = 104;
            this.controlNavigator2.Text = "controlNavigator2";
            // 
            // m_btnSave
            // 
            this.m_btnSave.Location = new System.Drawing.Point(288, 230);
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(75, 23);
            this.m_btnSave.TabIndex = 101;
            this.m_btnSave.Text = "保存";
            this.m_btnSave.UseVisualStyleBackColor = true;
            this.m_btnSave.Click += new System.EventHandler(this.m_btnSave_Click);
            // 
            // frmGathering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 273);
            this.Controls.Add(this.controlNavigator2);
            this.Controls.Add(this.m_btnSave);
            this.Controls.Add(this.grid_djksq);
            this.MaximizeBox = false;
            this.Name = "frmGathering";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地价款收取状况";
            this.Load += new System.EventHandler(this.frmGathering_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid_djksq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_djksq)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SkyMap.Net.Gui.Components.SmGridControl grid_djksq;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_djksq;
        private DevExpress.XtraGrid.Columns.GridColumn 支付日期;
        private DevExpress.XtraGrid.Columns.GridColumn 支付比例;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private System.Windows.Forms.Button m_btnSave;
        private DevExpress.XtraEditors.ControlNavigator controlNavigator2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}