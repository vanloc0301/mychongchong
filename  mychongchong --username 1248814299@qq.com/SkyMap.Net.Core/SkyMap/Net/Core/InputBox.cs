namespace SkyMap.Net.Core
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class InputBox : Form
    {
        private Button acceptButton;
        private Button cancelButton;
        private Label label;
        private string result;
        private TextBox textBox;

        public InputBox(string text, string caption, string defaultValue)
        {
            Size size;
            this.InitializeComponent();
            text = SkyMap.Net.Core.StringParser.Parse(text);
            this.Text = SkyMap.Net.Core.StringParser.Parse(caption);
            this.acceptButton.Text = SkyMap.Net.Core.StringParser.Parse("${res:Global.OKButtonText}");
            this.cancelButton.Text = SkyMap.Net.Core.StringParser.Parse("${res:Global.CancelButtonText}");
            using (Graphics graphics = base.CreateGraphics())
            {
                Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                size = graphics.MeasureString(text, this.label.Font, (int) (workingArea.Width - 20)).ToSize();
                size.Width += 4;
            }
            if (size.Width < 200)
            {
                size.Width = 200;
            }
            Size clientSize = base.ClientSize;
            clientSize.Width += size.Width - this.label.Width;
            clientSize.Height += size.Height - this.label.Height;
            base.ClientSize = clientSize;
            this.label.Text = text;
            this.textBox.Text = defaultValue;
            base.DialogResult = DialogResult.Cancel;
            RightToLeftConverter.ConvertRecursive(this);
        }

        private void AcceptButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            this.result = this.textBox.Text;
            base.Close();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.result = null;
            base.Close();
        }

        private void InitializeComponent()
        {
            this.acceptButton = new Button();
            this.textBox = new TextBox();
            this.cancelButton = new Button();
            this.label = new Label();
            base.SuspendLayout();
            this.acceptButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.acceptButton.FlatStyle = FlatStyle.System;
            this.acceptButton.Location = new Point(0xb0, 0x72);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "OK";
            this.acceptButton.Click += new EventHandler(this.AcceptButtonClick);
            this.textBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.textBox.Location = new Point(8, 0x56);
            this.textBox.Name = "textBox";
            this.textBox.Size = new Size(0x13e, 20);
            this.textBox.TabIndex = 1;
            this.textBox.Text = "";
            this.cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.FlatStyle = FlatStyle.System;
            this.cancelButton.Location = new Point(0x100, 0x72);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new EventHandler(this.CancelButtonClick);
            this.label.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label.Location = new Point(8, 8);
            this.label.Name = "label";
            this.label.Size = new Size(0x148, 0x4a);
            this.label.TabIndex = 0;
            this.label.UseMnemonic = false;
            base.AcceptButton = this.acceptButton;
            base.CancelButton = this.cancelButton;
            base.ClientSize = new Size(0x152, 0x90);
            base.Controls.Add(this.textBox);
            base.Controls.Add(this.label);
            base.Controls.Add(this.cancelButton);
            base.Controls.Add(this.acceptButton);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InputBox";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "InputBox";
            base.ResumeLayout(false);
        }

        public string Result
        {
            get
            {
                return this.result;
            }
        }
    }
}

