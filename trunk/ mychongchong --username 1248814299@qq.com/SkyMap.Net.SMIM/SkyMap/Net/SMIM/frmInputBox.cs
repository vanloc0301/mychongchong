namespace SkyMap.Net.SMIM
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmInputBox : Form
    {
        private Button cmdCancel;
        private Button cmdOK;
        private IContainer components;
        private Label lblMessage;
        private TextBox txtInput;

        public frmInputBox()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public frmInputBox(string Prompt, string Title, string Default) : this()
        {
            this.lblMessage.Text = Prompt;
            this.Text = Title;
            this.txtInput.Text = Default;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
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
            this.txtInput = new TextBox();
            this.cmdOK = new Button();
            this.cmdCancel = new Button();
            this.lblMessage = new Label();
            base.SuspendLayout();
            this.txtInput.Location = new Point(12, 0x4a);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new Size(0x14f, 20);
            this.txtInput.TabIndex = 0;
            this.cmdOK.Location = new Point(0x111, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new Size(0x4a, 0x19);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
            this.cmdCancel.Location = new Point(0x111, 0x2b);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(0x4a, 0x19);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
            this.lblMessage.Location = new Point(12, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(0xff, 0x39);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Mesage";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x167, 0x66);
            base.Controls.Add(this.lblMessage);
            base.Controls.Add(this.cmdCancel);
            base.Controls.Add(this.cmdOK);
            base.Controls.Add(this.txtInput);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmInputBox";
            this.Text = "InputBox";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string Result
        {
            get
            {
                return this.txtInput.Text;
            }
        }
    }
}

