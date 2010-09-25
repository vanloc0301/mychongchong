namespace ZBPMPRINT
{
    partial class Income
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.m_cboRemark = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_grvItems = new System.Windows.Forms.DataGridView();
            this.StrName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StrMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_cboPayUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_dtm = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.m_cboType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_cboNo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_cboUser = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_grvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cboRemark
            // 
            this.m_cboRemark.FormattingEnabled = true;
            this.m_cboRemark.Location = new System.Drawing.Point(496, 75);
            this.m_cboRemark.Name = "m_cboRemark";
            this.m_cboRemark.Size = new System.Drawing.Size(245, 20);
            this.m_cboRemark.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(421, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 46;
            this.label4.Text = "备注：";
            // 
            // m_grvItems
            // 
            this.m_grvItems.AllowUserToAddRows = false;
            this.m_grvItems.AllowUserToDeleteRows = false;
            this.m_grvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StrName,
            this.StrMoney});
            this.m_grvItems.Location = new System.Drawing.Point(44, 183);
            this.m_grvItems.Name = "m_grvItems";
            this.m_grvItems.RowTemplate.Height = 23;
            this.m_grvItems.Size = new System.Drawing.Size(723, 108);
            this.m_grvItems.TabIndex = 45;
            this.m_grvItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.m_grvItems_DataError);
            // 
            // StrName
            // 
            this.StrName.DataPropertyName = "StrName";
            this.StrName.HeaderText = "项目";
            this.StrName.Name = "StrName";
            this.StrName.Width = 300;
            // 
            // StrMoney
            // 
            this.StrMoney.DataPropertyName = "Money";
            this.StrMoney.HeaderText = "金额";
            this.StrMoney.Name = "StrMoney";
            this.StrMoney.Width = 200;
            // 
            // m_cboPayUnit
            // 
            this.m_cboPayUnit.FormattingEnabled = true;
            this.m_cboPayUnit.Location = new System.Drawing.Point(158, 112);
            this.m_cboPayUnit.Name = "m_cboPayUnit";
            this.m_cboPayUnit.Size = new System.Drawing.Size(245, 20);
            this.m_cboPayUnit.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "缴款单位：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 42;
            this.label2.Text = "时间：";
            // 
            // m_dtm
            // 
            this.m_dtm.Location = new System.Drawing.Point(158, 75);
            this.m_dtm.Name = "m_dtm";
            this.m_dtm.Size = new System.Drawing.Size(245, 21);
            this.m_dtm.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(40, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 20);
            this.label1.TabIndex = 40;
            this.label1.Text = "广东省政府性基金（资金）通用票据";
            // 
            // m_cboType
            // 
            this.m_cboType.FormattingEnabled = true;
            this.m_cboType.Location = new System.Drawing.Point(496, 115);
            this.m_cboType.Name = "m_cboType";
            this.m_cboType.Size = new System.Drawing.Size(245, 20);
            this.m_cboType.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(421, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 48;
            this.label5.Text = "缴款方式：";
            // 
            // m_cboNo
            // 
            this.m_cboNo.FormattingEnabled = true;
            this.m_cboNo.Location = new System.Drawing.Point(158, 147);
            this.m_cboNo.Name = "m_cboNo";
            this.m_cboNo.Size = new System.Drawing.Size(245, 20);
            this.m_cboNo.TabIndex = 51;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 50;
            this.label6.Text = "缴款通知书编号：";
            // 
            // m_cboUser
            // 
            this.m_cboUser.FormattingEnabled = true;
            this.m_cboUser.Location = new System.Drawing.Point(496, 148);
            this.m_cboUser.Name = "m_cboUser";
            this.m_cboUser.Size = new System.Drawing.Size(245, 20);
            this.m_cboUser.TabIndex = 53;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(421, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 52;
            this.label7.Text = "开票人：";
            // 
            // Income
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cboUser);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.m_cboNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_cboType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_cboRemark);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_grvItems);
            this.Controls.Add(this.m_cboPayUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_dtm);
            this.Controls.Add(this.label1);
            this.Name = "Income";
            this.Size = new System.Drawing.Size(800, 400);
            this.Load += new System.EventHandler(this.Income_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_grvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_cboRemark;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView m_grvItems;
        private System.Windows.Forms.ComboBox m_cboPayUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker m_dtm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_cboType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox m_cboNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn StrName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StrMoney;
        private System.Windows.Forms.ComboBox m_cboUser;
        private System.Windows.Forms.Label label7;
    }
}
