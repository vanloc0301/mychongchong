namespace Crawler
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FileTypeForm : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label6;
        public NumericUpDown numericUpDownMaxSize;
        public NumericUpDown numericUpDownMinSize;
        public TextBox textBoxTypeDescription;

        public FileTypeForm()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FileTypeForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.textBoxTypeDescription = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.numericUpDownMaxSize = new NumericUpDown();
            this.label5 = new Label();
            this.label6 = new Label();
            this.numericUpDownMinSize = new NumericUpDown();
            this.numericUpDownMaxSize.BeginInit();
            this.numericUpDownMinSize.BeginInit();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x146, 9);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(90, 0x18);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "确定";
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x146, 0x22);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(90, 0x19);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.textBoxTypeDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxTypeDescription.BackColor = Color.WhiteSmoke;
            this.textBoxTypeDescription.Location = new Point(0x13, 0x1a);
            this.textBoxTypeDescription.Name = "textBoxTypeDescription";
            this.textBoxTypeDescription.Size = new Size(0x120, 0x15);
            this.textBoxTypeDescription.TabIndex = 8;
            this.label1.Location = new Point(0x13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x87, 0x11);
            this.label1.TabIndex = 6;
            this.label1.Text = "MIME 类型:";
            this.label2.Location = new Point(0x13, 60);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x12);
            this.label2.TabIndex = 5;
            this.label2.Text = "最小值";
            this.label3.Location = new Point(0x13, 0x56);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x60, 0x11);
            this.label3.TabIndex = 5;
            this.label3.Text = "最大值";
            this.numericUpDownMaxSize.BackColor = Color.WhiteSmoke;
            this.numericUpDownMaxSize.Location = new Point(0x73, 0x56);
            int[] bits = new int[4];
            bits[0] = 0x5f5e100;
            this.numericUpDownMaxSize.Maximum = new decimal(bits);
            this.numericUpDownMaxSize.Name = "numericUpDownMaxSize";
            this.numericUpDownMaxSize.Size = new Size(0x4d, 0x15);
            this.numericUpDownMaxSize.TabIndex = 9;
            this.numericUpDownMaxSize.Tag = "";
            this.label5.Location = new Point(0xca, 0x56);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x26, 0x11);
            this.label5.TabIndex = 5;
            this.label5.Text = "(KB)";
            this.label6.Location = new Point(0xca, 60);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x26, 0x12);
            this.label6.TabIndex = 5;
            this.label6.Text = "(KB)";
            this.numericUpDownMinSize.BackColor = Color.WhiteSmoke;
            this.numericUpDownMinSize.Location = new Point(0x73, 60);
            bits = new int[4];
            bits[0] = 0x5f5e100;
            this.numericUpDownMinSize.Maximum = new decimal(bits);
            this.numericUpDownMinSize.Name = "numericUpDownMinSize";
            this.numericUpDownMinSize.Size = new Size(0x4d, 0x15);
            this.numericUpDownMinSize.TabIndex = 9;
            this.numericUpDownMinSize.Tag = "";
            base.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1b2, 0x77);
            base.Controls.Add(this.numericUpDownMaxSize);
            base.Controls.Add(this.textBoxTypeDescription);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.numericUpDownMinSize);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FileTypeForm";
            this.Text = "Edit file type";
            base.Load += new EventHandler(this.FileTypeForm_Load);
            this.numericUpDownMaxSize.EndInit();
            this.numericUpDownMinSize.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

