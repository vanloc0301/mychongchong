namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class TipOfTheDayDialog : Form
    {
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button nextTipButton;
        private System.Windows.Forms.Panel panel = new Panel();
        private SkyMap.Net.Gui.TipOfTheDayView tipview;
        private System.Windows.Forms.CheckBox viewTipsAtStartCheckBox;

        public TipOfTheDayDialog()
        {
            this.InitializeComponent();
            base.StartPosition = FormStartPosition.CenterScreen;
            base.Icon = null;
            XmlDocument document = new XmlDocument();
            document.Load(string.Concat(new object[] { PropertyService.DataDirectory, Path.DirectorySeparatorChar, "options", Path.DirectorySeparatorChar, "TipsOfTheDay.xml" }));
            this.tipview = new TipOfTheDayView(document.DocumentElement);
            this.panel.Controls.Add(this.tipview);
            base.Controls.Add(this.panel);
            this.panel.Width = this.tipview.Width = base.Width - 0x18;
            this.panel.Height = this.tipview.Height = this.nextTipButton.Top - 15;
            this.panel.Location = new Point(8, 5);
            this.nextTipButton.Click += new EventHandler(this.NextTip);
            this.viewTipsAtStartCheckBox.CheckedChanged += new EventHandler(this.CheckChange);
            this.viewTipsAtStartCheckBox.Checked = PropertyService.Get<bool>("ShowTipsAtStartup", true);
            base.MaximizeBox = base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            RightToLeftConverter.ConvertRecursive(this);
        }

        private void CheckChange(object sender, EventArgs e)
        {
            PropertyService.Set<bool>("ShowTipsAtStartup", this.viewTipsAtStartCheckBox.Checked);
        }

        private void ExitDialog(object sender, EventArgs e)
        {
            base.Close();
            base.Dispose();
        }

        private void InitializeComponent()
        {
            this.closeButton = new Button();
            this.viewTipsAtStartCheckBox = new CheckBox();
            this.nextTipButton = new Button();
            this.closeButton.Location = new Point(0x148, 0xe8);
            this.closeButton.Click += new EventHandler(this.ExitDialog);
            this.closeButton.Size = new Size(80, 0x18);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = ResourceService.GetString("Global.CloseButtonText");
            this.closeButton.FlatStyle = FlatStyle.System;
            this.viewTipsAtStartCheckBox.Location = new Point(8, 0xe8);
            this.viewTipsAtStartCheckBox.Text = ResourceService.GetString("Dialog.TipOfTheDay.checkBox1Text");
            this.viewTipsAtStartCheckBox.Size = new Size(210, 0x18);
            this.viewTipsAtStartCheckBox.TabIndex = 2;
            this.viewTipsAtStartCheckBox.TextAlign = ContentAlignment.MiddleLeft;
            this.viewTipsAtStartCheckBox.FlatStyle = FlatStyle.System;
            this.Text = ResourceService.GetString("Dialog.TipOfTheDay.DialogName");
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.ClientSize = new Size(0x1a2, 0x107);
            this.nextTipButton.Location = new Point(0xe0, 0xe8);
            this.nextTipButton.Size = new Size(0x60, 0x18);
            this.nextTipButton.TabIndex = 0;
            this.nextTipButton.Text = ResourceService.GetString("Dialog.TipOfTheDay.button1Text");
            this.nextTipButton.FlatStyle = FlatStyle.System;
            base.Controls.Add(this.viewTipsAtStartCheckBox);
            base.Controls.Add(this.closeButton);
            base.Controls.Add(this.nextTipButton);
        }

        private void NextTip(object sender, EventArgs e)
        {
            this.tipview.NextTip();
        }
    }
}

