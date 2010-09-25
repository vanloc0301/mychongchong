namespace ZBPM
{
    partial class FjBb
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
            this.Btn修正体系 = new DevExpress.XtraEditors.SimpleButton();
            this.Btn单家独户 = new DevExpress.XtraEditors.SimpleButton();
            this.Btn非单家独户 = new DevExpress.XtraEditors.SimpleButton();
            this.Btn车房 = new DevExpress.XtraEditors.SimpleButton();
            this.label12 = new System.Windows.Forms.Label();
            this.cbe修改类型 = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cbe修改类型.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn修正体系
            // 
            this.Btn修正体系.Location = new System.Drawing.Point(334, 18);
            this.Btn修正体系.Name = "Btn修正体系";
            this.Btn修正体系.Size = new System.Drawing.Size(130, 23);
            this.Btn修正体系.TabIndex = 0;
            this.Btn修正体系.Text = "修正体系Excel";
            this.Btn修正体系.Click += new System.EventHandler(this.Btn修正体系_Click);
            // 
            // Btn单家独户
            // 
            this.Btn单家独户.Location = new System.Drawing.Point(331, 60);
            this.Btn单家独户.Name = "Btn单家独户";
            this.Btn单家独户.Size = new System.Drawing.Size(130, 23);
            this.Btn单家独户.TabIndex = 1;
            this.Btn单家独户.Text = "生成单家独户";
            this.Btn单家独户.Visible = false;
            // 
            // Btn非单家独户
            // 
            this.Btn非单家独户.Location = new System.Drawing.Point(331, 104);
            this.Btn非单家独户.Name = "Btn非单家独户";
            this.Btn非单家独户.Size = new System.Drawing.Size(130, 23);
            this.Btn非单家独户.TabIndex = 2;
            this.Btn非单家独户.Text = "非单家独户";
            this.Btn非单家独户.Visible = false;
            // 
            // Btn车房
            // 
            this.Btn车房.Location = new System.Drawing.Point(331, 150);
            this.Btn车房.Name = "Btn车房";
            this.Btn车房.Size = new System.Drawing.Size(130, 23);
            this.Btn车房.TabIndex = 3;
            this.Btn车房.Text = "车房";
            this.Btn车房.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(139, 14);
            this.toolTipController.SetSuperTip(this.label12, null);
            this.label12.TabIndex = 158;
            this.label12.Text = "只显示满足条件的数据：";
            // 
            // cbe修改类型
            // 
            this.cbe修改类型.Location = new System.Drawing.Point(148, 20);
            this.cbe修改类型.Name = "cbe修改类型";
            this.cbe修改类型.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbe修改类型.Properties.Items.AddRange(new object[] {
            "全部",
            "不修改",
            "只修改属性",
            "只修改图形",
            "修改图形后，修改修正方式",
            "同时修改属性及图形",
            "新增区片",
            "其他",
            "修改过的"});
            this.cbe修改类型.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbe修改类型.Size = new System.Drawing.Size(124, 21);
            this.cbe修改类型.TabIndex = 159;
            // 
            // FjBb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbe修改类型);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Btn车房);
            this.Controls.Add(this.Btn非单家独户);
            this.Controls.Add(this.Btn单家独户);
            this.Controls.Add(this.Btn修正体系);
            this.Name = "FjBb";
            this.Size = new System.Drawing.Size(656, 188);
            this.toolTipController.SetSuperTip(this, null);
            ((System.ComponentModel.ISupportInitialize)(this.cbe修改类型.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton Btn修正体系;
        private DevExpress.XtraEditors.SimpleButton Btn单家独户;
        private DevExpress.XtraEditors.SimpleButton Btn非单家独户;
        private DevExpress.XtraEditors.SimpleButton Btn车房;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.ComboBoxEdit cbe修改类型;
    }
}
