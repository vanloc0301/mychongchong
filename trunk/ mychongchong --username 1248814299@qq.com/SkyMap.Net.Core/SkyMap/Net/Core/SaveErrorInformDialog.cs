namespace SkyMap.Net.Core
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class SaveErrorInformDialog : Form
    {
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private string displayMessage;
        private Button exceptionButton;
        private Exception exceptionGot;
        private Button okButton;

        public SaveErrorInformDialog(string fileName, string message, string dialogName, Exception exceptionGot)
        {
            this.Text = SkyMap.Net.Core.StringParser.Parse(dialogName);
            this.InitializeComponent2();
            RightToLeftConverter.ConvertRecursive(this);
            this.displayMessage = SkyMap.Net.Core.StringParser.Parse(message, new string[,] { { "FileName", fileName }, { "Path", Path.GetDirectoryName(fileName) }, { "FileNameWithoutPath", Path.GetFileName(fileName) }, { "Exception", exceptionGot.GetType().FullName } });
            this.descriptionTextBox.Lines = this.displayMessage.Split(new char[] { '\n' });
            this.exceptionGot = exceptionGot;
        }

        private void InitializeComponent2()
        {
            base.ClientSize = new Size(0x1fc, 320);
            base.SuspendLayout();
            this.descriptionLabel = new Label();
            this.descriptionLabel.Location = new Point(8, 8);
            this.descriptionLabel.Size = new Size(0x248, 0x18);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.descriptionLabel.TextAlign = ContentAlignment.BottomLeft;
            this.descriptionLabel.Text = SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Core.Services.ErrorDialogs.DescriptionLabel}");
            this.descriptionLabel.Name = "descriptionLabel";
            base.Controls.Add(this.descriptionLabel);
            this.descriptionTextBox = new TextBox();
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Size = new Size(0x248, 0xed);
            this.descriptionTextBox.Location = new Point(8, 40);
            this.descriptionTextBox.TabIndex = 2;
            this.descriptionTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.descriptionTextBox.ReadOnly = true;
            base.Controls.Add(this.descriptionTextBox);
            this.exceptionButton = new Button();
            this.exceptionButton.TabIndex = 1;
            this.exceptionButton.Name = "exceptionButton";
            this.exceptionButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.exceptionButton.Text = SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Core.Services.ErrorDialogs.ShowExceptionButton}");
            this.exceptionButton.Size = new Size(120, 0x1b);
            this.exceptionButton.Location = new Point(0x174, 0x11d);
            this.exceptionButton.Click += new EventHandler(this.ShowException);
            base.Controls.Add(this.exceptionButton);
            this.okButton = new Button();
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 0;
            this.okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.okButton.Text = SkyMap.Net.Core.StringParser.Parse("${res:Global.OKButtonText}");
            this.okButton.Size = new Size(120, 0x1b);
            this.okButton.Location = new Point(0xf4, 0x11d);
            this.okButton.DialogResult = DialogResult.OK;
            base.Controls.Add(this.okButton);
            base.MaximizeBox = false;
            base.Name = "SaveErrorInformDialog";
            base.MinimizeBox = false;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ResumeLayout(false);
            base.Size = new Size(0x20e, 0x106);
        }

        private void ShowException(object sender, EventArgs e)
        {
            MessageService.ShowMessage(this.exceptionGot.ToString(), "Exception got");
        }
    }
}

