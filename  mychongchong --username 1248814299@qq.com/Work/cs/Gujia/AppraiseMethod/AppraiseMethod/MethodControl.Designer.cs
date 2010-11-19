namespace AppraiseMethod
{
    partial class MethodControl
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControlYw = new DevExpress.XtraEditors.GroupControl();
            this.btnStartAppraise = new DevExpress.XtraEditors.SimpleButton();
            this.GroupControl_评估方法选择 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.报告编号 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.评估类型 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.土地面积 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.建筑面积 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.check收益还原法 = new DevExpress.XtraEditors.CheckEdit();
            this.check市场比较法 = new DevExpress.XtraEditors.CheckEdit();
            this.check假设开发法 = new DevExpress.XtraEditors.CheckEdit();
            this.check成本法 = new DevExpress.XtraEditors.CheckEdit();
            this.che基准地价修正法 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlYw)).BeginInit();
            this.groupControlYw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl_评估方法选择)).BeginInit();
            this.GroupControl_评估方法选择.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check收益还原法.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check市场比较法.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check假设开发法.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.check成本法.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.che基准地价修正法.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControlYw);
            this.panelControl1.Controls.Add(this.GroupControl_评估方法选择);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(238, 293);
            this.panelControl1.TabIndex = 0;
            // 
            // groupControlYw
            // 
            this.groupControlYw.Controls.Add(this.btnStartAppraise);
            this.groupControlYw.Location = new System.Drawing.Point(8, 179);
            this.groupControlYw.Name = "groupControlYw";
            this.groupControlYw.Size = new System.Drawing.Size(223, 108);
            this.groupControlYw.TabIndex = 3;
            // 
            // btnStartAppraise
            // 
            this.btnStartAppraise.Location = new System.Drawing.Point(51, 46);
            this.btnStartAppraise.Name = "btnStartAppraise";
            this.btnStartAppraise.Size = new System.Drawing.Size(118, 23);
            this.btnStartAppraise.TabIndex = 0;
            this.btnStartAppraise.Text = "开始评估";
            this.btnStartAppraise.Click += new System.EventHandler(this.btnStartAppraise_Click);
            // 
            // GroupControl_评估方法选择
            // 
            this.GroupControl_评估方法选择.Controls.Add(this.gridControl1);
            this.GroupControl_评估方法选择.Controls.Add(this.check收益还原法);
            this.GroupControl_评估方法选择.Controls.Add(this.check市场比较法);
            this.GroupControl_评估方法选择.Controls.Add(this.check假设开发法);
            this.GroupControl_评估方法选择.Controls.Add(this.check成本法);
            this.GroupControl_评估方法选择.Controls.Add(this.che基准地价修正法);
            this.GroupControl_评估方法选择.Location = new System.Drawing.Point(3, 5);
            this.GroupControl_评估方法选择.Name = "GroupControl_评估方法选择";
            this.GroupControl_评估方法选择.Size = new System.Drawing.Size(228, 168);
            this.GroupControl_评估方法选择.TabIndex = 1;
            this.GroupControl_评估方法选择.Text = "评估方法选择：";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(160, 86);
            this.gridControl1.MainView = this.cardView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(170, 129);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.cardView1});
            this.gridControl1.Visible = false;
            // 
            // cardView1
            // 
            this.cardView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.报告编号,
            this.评估类型,
            this.土地面积,
            this.建筑面积});
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gridControl1;
            this.cardView1.Name = "cardView1";
            this.cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            // 
            // 报告编号
            // 
            this.报告编号.Caption = "报告编号";
            this.报告编号.Name = "报告编号";
            this.报告编号.Visible = true;
            this.报告编号.VisibleIndex = 0;
            this.报告编号.Width = 70;
            // 
            // 评估类型
            // 
            this.评估类型.Caption = "评估类型";
            this.评估类型.Name = "评估类型";
            this.评估类型.Visible = true;
            this.评估类型.VisibleIndex = 1;
            this.评估类型.Width = 66;
            // 
            // 土地面积
            // 
            this.土地面积.Caption = "土地面积";
            this.土地面积.Name = "土地面积";
            this.土地面积.Visible = true;
            this.土地面积.VisibleIndex = 2;
            this.土地面积.Width = 49;
            // 
            // 建筑面积
            // 
            this.建筑面积.Caption = "建筑面积";
            this.建筑面积.Name = "建筑面积";
            this.建筑面积.Visible = true;
            this.建筑面积.VisibleIndex = 3;
            this.建筑面积.Width = 20;
            // 
            // check收益还原法
            // 
            this.check收益还原法.Location = new System.Drawing.Point(34, 134);
            this.check收益还原法.Name = "check收益还原法";
            this.check收益还原法.Properties.Caption = "收益还原法";
            this.check收益还原法.Size = new System.Drawing.Size(120, 19);
            this.check收益还原法.TabIndex = 4;
            // 
            // check市场比较法
            // 
            this.check市场比较法.Location = new System.Drawing.Point(34, 109);
            this.check市场比较法.Name = "check市场比较法";
            this.check市场比较法.Properties.Caption = "市场比较法";
            this.check市场比较法.Size = new System.Drawing.Size(120, 19);
            this.check市场比较法.TabIndex = 3;
            // 
            // check假设开发法
            // 
            this.check假设开发法.Location = new System.Drawing.Point(34, 84);
            this.check假设开发法.Name = "check假设开发法";
            this.check假设开发法.Properties.Caption = "假设开发法";
            this.check假设开发法.Size = new System.Drawing.Size(120, 19);
            this.check假设开发法.TabIndex = 2;
            // 
            // check成本法
            // 
            this.check成本法.Location = new System.Drawing.Point(34, 59);
            this.check成本法.Name = "check成本法";
            this.check成本法.Properties.Caption = "成本法";
            this.check成本法.Size = new System.Drawing.Size(120, 19);
            this.check成本法.TabIndex = 1;
            // 
            // che基准地价修正法
            // 
            this.che基准地价修正法.Location = new System.Drawing.Point(34, 34);
            this.che基准地价修正法.Name = "che基准地价修正法";
            this.che基准地价修正法.Properties.Caption = "基准地价修正法";
            this.che基准地价修正法.Size = new System.Drawing.Size(120, 19);
            this.che基准地价修正法.TabIndex = 0;
            // 
            // MethodControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelControl1);
            this.Name = "MethodControl";
            this.Size = new System.Drawing.Size(238, 293);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlYw)).EndInit();
            this.groupControlYw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl_评估方法选择)).EndInit();
            this.GroupControl_评估方法选择.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check收益还原法.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check市场比较法.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check假设开发法.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.check成本法.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.che基准地价修正法.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl GroupControl_评估方法选择;
        private DevExpress.XtraEditors.CheckEdit check成本法;
        private DevExpress.XtraEditors.CheckEdit che基准地价修正法;
        private DevExpress.XtraEditors.CheckEdit check收益还原法;
        private DevExpress.XtraEditors.CheckEdit check市场比较法;
        private DevExpress.XtraEditors.CheckEdit check假设开发法;
        private DevExpress.XtraEditors.GroupControl groupControlYw;
        private DevExpress.XtraEditors.SimpleButton btnStartAppraise;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private DevExpress.XtraGrid.Columns.GridColumn 报告编号;
        private DevExpress.XtraGrid.Columns.GridColumn 评估类型;
        private DevExpress.XtraGrid.Columns.GridColumn 土地面积;
        private DevExpress.XtraGrid.Columns.GridColumn 建筑面积;



    }
}
