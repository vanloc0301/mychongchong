namespace SkyMap.Net.Core
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class CustomDialog : Form
    {
        private int acceptButton;
        private int cancelButton;
        private Label label;
        private Panel panel;
        private int result = -1;

        public CustomDialog(string caption, string message, int acceptButton, int cancelButton, string[] buttonLabels)
        {
            base.SuspendLayout();
            this.MyInitializeComponent();
            base.Icon = null;
            this.acceptButton = acceptButton;
            this.cancelButton = cancelButton;
            message = SkyMap.Net.Core.StringParser.Parse(message);
            this.Text = SkyMap.Net.Core.StringParser.Parse(caption);
            using (Graphics graphics = base.CreateGraphics())
            {
                int num2;
                Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                Size size = graphics.MeasureString(message, this.label.Font, (int) (workingArea.Width - 20)).ToSize();
                Button[] controls = new Button[buttonLabels.Length];
                int[] numArray = new int[buttonLabels.Length];
                int num = 0;
                for (num2 = 0; num2 < controls.Length; num2++)
                {
                    Button button = new Button();
                    button.FlatStyle = FlatStyle.System;
                    button.Anchor = AnchorStyles.Right;
                    button.Tag = num2;
                    string text = SkyMap.Net.Core.StringParser.Parse(buttonLabels[num2]);
                    button.Text = text;
                    button.Click += new EventHandler(this.ButtonClick);
                    SizeF ef2 = graphics.MeasureString(text, button.Font);
                    button.Width = Math.Max(button.Width, (((int) Math.Ceiling((double) (((double) ef2.Width) / 8.0))) + 1) * 8);
                    numArray[num2] = num;
                    controls[num2] = button;
                    num += button.Width + 4;
                }
                if (acceptButton >= 0)
                {
                    base.AcceptButton = controls[acceptButton];
                }
                if (cancelButton >= 0)
                {
                    base.CancelButton = controls[cancelButton];
                }
                num -= 4;
                if (num > size.Width)
                {
                    size.Width = num;
                }
                size.Height += this.panel.Height + 6;
                base.ClientSize = size;
                int num3 = (this.panel.ClientSize.Width - num) / 2;
                for (num2 = 0; num2 < controls.Length; num2++)
                {
                    controls[num2].Location = new Point(num3 + numArray[num2], 4);
                }
                this.panel.Controls.AddRange(controls);
            }
            this.label.Text = message;
            RightToLeftConverter.ConvertRecursive(this);
            base.ResumeLayout(false);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            this.result = (int) ((Control) sender).Tag;
            base.Close();
        }

        private void MyInitializeComponent()
        {
            this.panel = new Panel();
            this.label = new Label();
            this.panel.Dock = DockStyle.Bottom;
            this.panel.Location = new Point(4, 80);
            this.panel.Name = "panel";
            this.panel.Size = new Size(0x10a, 0x20);
            this.panel.TabIndex = 0;
            this.label.Dock = DockStyle.Fill;
            this.label.FlatStyle = FlatStyle.System;
            this.label.Location = new Point(4, 4);
            this.label.Name = "label";
            this.label.Size = new Size(0x10a, 0x4c);
            this.label.TabIndex = 1;
            this.label.UseMnemonic = false;
            base.ClientSize = new Size(0x112, 0x70);
            base.Controls.Add(this.label);
            base.Controls.Add(this.panel);
            base.DockPadding.Left = 4;
            base.DockPadding.Right = 4;
            base.DockPadding.Top = 4;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CustomDialog";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "CustomDialog";
        }

        public int Result
        {
            get
            {
                return this.result;
            }
        }
    }
}

