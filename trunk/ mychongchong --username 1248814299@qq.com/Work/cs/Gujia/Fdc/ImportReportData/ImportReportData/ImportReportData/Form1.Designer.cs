namespace ImportReportData
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_btnImport = new System.Windows.Forms.Button();
            this.m_btnUpdate = new System.Windows.Forms.Button();
            this.bShow = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtReturn1 = new System.Windows.Forms.TextBox();
            this.txtReturn0 = new System.Windows.Forms.TextBox();
            this.txtReturn2 = new System.Windows.Forms.TextBox();
            this.btnBak = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_btnImport
            // 
            this.m_btnImport.Location = new System.Drawing.Point(270, 43);
            this.m_btnImport.Name = "m_btnImport";
            this.m_btnImport.Size = new System.Drawing.Size(75, 23);
            this.m_btnImport.TabIndex = 0;
            this.m_btnImport.Text = "导入";
            this.m_btnImport.UseVisualStyleBackColor = true;
            this.m_btnImport.Click += new System.EventHandler(this.m_btnImport_Click);
            // 
            // m_btnUpdate
            // 
            this.m_btnUpdate.Location = new System.Drawing.Point(184, 12);
            this.m_btnUpdate.Name = "m_btnUpdate";
            this.m_btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.m_btnUpdate.TabIndex = 1;
            this.m_btnUpdate.Text = "更新";
            this.m_btnUpdate.UseVisualStyleBackColor = true;
            this.m_btnUpdate.Click += new System.EventHandler(this.m_btnUpdate_Click);
            // 
            // bShow
            // 
            this.bShow.Location = new System.Drawing.Point(12, 12);
            this.bShow.Name = "bShow";
            this.bShow.Size = new System.Drawing.Size(75, 23);
            this.bShow.TabIndex = 2;
            this.bShow.Text = "显示";
            this.bShow.UseVisualStyleBackColor = true;
            this.bShow.Click += new System.EventHandler(this.bShow_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(13, 43);
            this.txtFileName.Multiline = true;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(223, 33);
            this.txtFileName.TabIndex = 3;
            // 
            // txtReturn1
            // 
            this.txtReturn1.Location = new System.Drawing.Point(13, 129);
            this.txtReturn1.Name = "txtReturn1";
            this.txtReturn1.Size = new System.Drawing.Size(223, 21);
            this.txtReturn1.TabIndex = 4;
            // 
            // txtReturn0
            // 
            this.txtReturn0.Location = new System.Drawing.Point(13, 92);
            this.txtReturn0.Name = "txtReturn0";
            this.txtReturn0.Size = new System.Drawing.Size(225, 21);
            this.txtReturn0.TabIndex = 5;
            // 
            // txtReturn2
            // 
            this.txtReturn2.Location = new System.Drawing.Point(13, 166);
            this.txtReturn2.Name = "txtReturn2";
            this.txtReturn2.Size = new System.Drawing.Size(223, 21);
            this.txtReturn2.TabIndex = 6;
            // 
            // btnBak
            // 
            this.btnBak.Location = new System.Drawing.Point(98, 12);
            this.btnBak.Name = "btnBak";
            this.btnBak.Size = new System.Drawing.Size(75, 23);
            this.btnBak.TabIndex = 7;
            this.btnBak.Text = "备份";
            this.btnBak.UseVisualStyleBackColor = true;
            this.btnBak.Click += new System.EventHandler(this.btnBak_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(270, 12);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 8;
            this.btnRestore.Text = "还原";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 297);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnBak);
            this.Controls.Add(this.txtReturn2);
            this.Controls.Add(this.txtReturn0);
            this.Controls.Add(this.txtReturn1);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.bShow);
            this.Controls.Add(this.m_btnUpdate);
            this.Controls.Add(this.m_btnImport);
            this.Name = "Form1";
            this.Text = "招标办业务报表批量导入";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnImport;
        private System.Windows.Forms.Button m_btnUpdate;
        private System.Windows.Forms.Button bShow;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtReturn1;
        private System.Windows.Forms.TextBox txtReturn0;
        private System.Windows.Forms.TextBox txtReturn2;
        private System.Windows.Forms.Button btnBak;
        private System.Windows.Forms.Button btnRestore;
    }
}

