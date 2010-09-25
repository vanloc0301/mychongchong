namespace ZBPMPRINT
{
    partial class Check
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
            this.label2 = new System.Windows.Forms.Label();
            this.m_dtm = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.m_cboGatheringPerson = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.m_cboMoney = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.m_cboInfo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_cboUse = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "时间：";
            // 
            // m_dtm
            // 
            this.m_dtm.Location = new System.Drawing.Point(174, 77);
            this.m_dtm.Name = "m_dtm";
            this.m_dtm.Size = new System.Drawing.Size(245, 21);
            this.m_dtm.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(56, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "中国建设银行（支票）";
            // 
            // m_cboGatheringPerson
            // 
            this.m_cboGatheringPerson.FormattingEnabled = true;
            this.m_cboGatheringPerson.Location = new System.Drawing.Point(174, 113);
            this.m_cboGatheringPerson.Name = "m_cboGatheringPerson";
            this.m_cboGatheringPerson.Size = new System.Drawing.Size(245, 20);
            this.m_cboGatheringPerson.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "收款人全称：";
            // 
            // m_cboMoney
            // 
            this.m_cboMoney.FormattingEnabled = true;
            this.m_cboMoney.Location = new System.Drawing.Point(174, 149);
            this.m_cboMoney.Name = "m_cboMoney";
            this.m_cboMoney.Size = new System.Drawing.Size(245, 20);
            this.m_cboMoney.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(56, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 32;
            this.label9.Text = "金额：";
            // 
            // m_cboInfo
            // 
            this.m_cboInfo.FormattingEnabled = true;
            this.m_cboInfo.Location = new System.Drawing.Point(174, 221);
            this.m_cboInfo.Name = "m_cboInfo";
            this.m_cboInfo.Size = new System.Drawing.Size(245, 20);
            this.m_cboInfo.TabIndex = 35;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(56, 189);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 34;
            this.label10.Text = "用途：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "附加信息：";
            // 
            // m_cboUse
            // 
            this.m_cboUse.FormattingEnabled = true;
            this.m_cboUse.Location = new System.Drawing.Point(174, 185);
            this.m_cboUse.Name = "m_cboUse";
            this.m_cboUse.Size = new System.Drawing.Size(245, 20);
            this.m_cboUse.TabIndex = 37;
            // 
            // Check
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cboUse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_cboInfo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.m_cboMoney);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.m_cboGatheringPerson);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_dtm);
            this.Controls.Add(this.label1);
            this.Name = "Check";
            this.Size = new System.Drawing.Size(800, 400);
            this.Load += new System.EventHandler(this.Check_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker m_dtm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_cboGatheringPerson;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox m_cboMoney;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox m_cboInfo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_cboUse;
    }
}
