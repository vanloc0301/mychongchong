namespace SkyMap.Net.Tools.Organize
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ParticipantEntityControl : UserControl
    {
        private LookUpEdit cboParticipantIDValue;
        private LookUpEdit cboPartType;
        private IContainer components = null;
        private Label label4;

        public ParticipantEntityControl(ParticipantEntity pe)
        {
            this.InitializeComponent();
            this.InitMe(pe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cboParticipantIDValue = new LookUpEdit();
            this.cboPartType = new LookUpEdit();
            this.label4 = new Label();
            this.cboParticipantIDValue.Properties.BeginInit();
            this.cboPartType.Properties.BeginInit();
            base.SuspendLayout();
            this.cboParticipantIDValue.Location = new Point(0x36, 0x29);
            this.cboParticipantIDValue.Name = "cboParticipantIDValue";
            this.cboParticipantIDValue.Size = new Size(0xbc, 0x17);
            this.cboParticipantIDValue.TabIndex = 6;
            this.cboPartType.Location = new Point(0x36, 12);
            this.cboPartType.Name = "cboPartType";
            this.cboPartType.Size = new Size(0xbc, 0x17);
            this.cboPartType.TabIndex = 4;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(11, 15);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "类型：";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.cboParticipantIDValue);
            base.Controls.Add(this.cboPartType);
            base.Controls.Add(this.label4);
            base.Name = "ParticipantTypeEdit";
            base.Size = new Size(0xfe, 0x4a);
            this.cboParticipantIDValue.Properties.EndInit();
            this.cboPartType.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitMe(ParticipantEntity pe)
        {
            this.cboPartType.Properties.DisplayMember = "Description";
            this.cboPartType.Properties.ValueMember = "Type";
            LookUpColumnInfo column = new LookUpColumnInfo("Description");
            column.Caption = "参与者类型";
            column.FieldName = "Description";
            this.cboPartType.Properties.Columns.Clear();
            this.cboPartType.Properties.Columns.Add(column);
            this.cboPartType.Properties.DataSource = ParticipantTypeList.Get();
            this.cboParticipantIDValue.EditValueChanged += delegate (object sender, EventArgs e) {
                this.cboParticipantIDValue.Visible = true;
                string editValue = (string) this.cboPartType.EditValue;
                if (editValue != null)
                {
                    if (!(editValue == "DEPT"))
                    {
                        if (editValue == "ROLE")
                        {
                            this.cboParticipantIDValue.Properties.DataSource = OGMService.Roles;
                        }
                        else if (editValue == "STAFF")
                        {
                            this.cboParticipantIDValue.Properties.DataSource = OGMService.Staffs;
                        }
                        else if (editValue == "ALL")
                        {
                            this.cboParticipantIDValue.EditValue = string.Empty;
                            this.cboParticipantIDValue.Properties.DataSource = null;
                            this.cboParticipantIDValue.Visible = false;
                        }
                    }
                    else
                    {
                        this.cboParticipantIDValue.Properties.DataSource = OGMService.Depts;
                    }
                }
            };
            this.cboPartType.EditValue = pe.Type;
            this.cboParticipantIDValue.Properties.DisplayMember = "Name";
            this.cboParticipantIDValue.Properties.ValueMember = "Id";
            column = new LookUpColumnInfo("Description");
            column.Caption = "名称";
            column.FieldName = "Name";
            this.cboParticipantIDValue.Properties.Columns.Clear();
            this.cboParticipantIDValue.Properties.Columns.Add(column);
            this.cboParticipantIDValue.EditValue = pe.IdValue;
        }

        public ParticipantEntity Value
        {
            get
            {
                ParticipantEntity entity = new ParticipantEntity();
                entity.Type = (string) this.cboPartType.EditValue;
                entity.IdValue = (string) this.cboParticipantIDValue.EditValue;
                return entity;
            }
        }
    }
}

