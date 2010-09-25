namespace SkyMap.Net.Core
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class SaveErrorChooseDialog : Form
    {
        private Button chooseLocationButton;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private string displayMessage;
        private Button exceptionButton;
        private Exception exceptionGot;
        private Button ignoreButton;
        private Button retryButton;

        public SaveErrorChooseDialog(string fileName, string message, string dialogName, Exception exceptionGot, bool chooseLocationEnabled)
        {
            this.Text = SkyMap.Net.Core.StringParser.Parse(dialogName);
            this.InitializeComponents(chooseLocationEnabled);
            RightToLeftConverter.ConvertRecursive(this);
            this.displayMessage = SkyMap.Net.Core.StringParser.Parse(message, new string[,] { { "FileName", fileName }, { "Path", Path.GetDirectoryName(fileName) }, { "FileNameWithoutPath", Path.GetFileName(fileName) }, { "Exception", exceptionGot.GetType().FullName } });
            this.descriptionTextBox.Lines = SkyMap.Net.Core.StringParser.Parse(this.displayMessage).Split(new char[] { '\n' });
            this.exceptionGot = exceptionGot;
        }

        private void InitializeComponents(bool chooseLocationEnabled)
        {
            base.ClientSize = new Size(0x1fc, 320);
            base.SuspendLayout();
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SaveErrorChooseDialog";
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.descriptionLabel = new Label();
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Location = new Point(8, 8);
            this.descriptionLabel.Size = new Size(0x248, 0x18);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.descriptionLabel.TextAlign = ContentAlignment.BottomLeft;
            this.descriptionLabel.Text = SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Core.Services.ErrorDialogs.DescriptionLabel}");
            base.Controls.Add(this.descriptionLabel);
            this.descriptionTextBox = new TextBox();
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Size = new Size(0x248, 0xed);
            this.descriptionTextBox.Location = new Point(8, 40);
            this.descriptionTextBox.TabIndex = 2;
            this.descriptionTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            base.Controls.Add(this.descriptionTextBox);
            this.retryButton = new Button();
            this.retryButton.DialogResult = DialogResult.Retry;
            this.retryButton.Name = "retryButton";
            this.retryButton.TabIndex = 5;
            this.retryButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.retryButton.Text = SkyMap.Net.Core.StringParser.Parse("${res:Global.RetryButtonText}");
            this.retryButton.Size = new Size(110, 0x1b);
            this.retryButton.Location = new Point(0x1c, 0x11d);
            base.Controls.Add(this.retryButton);
            this.ignoreButton = new Button();
            this.ignoreButton.Name = "ignoreButton";
            this.ignoreButton.DialogResult = DialogResult.Ignore;
            this.ignoreButton.TabIndex = 4;
            this.ignoreButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.ignoreButton.Text = SkyMap.Net.Core.StringParser.Parse("${res:Global.IgnoreButtonText}");
            this.ignoreButton.Size = new Size(110, 0x1b);
            this.ignoreButton.Location = new Point(0x92, 0x11d);
            base.Controls.Add(this.ignoreButton);
            this.exceptionButton = new Button();
            this.exceptionButton.TabIndex = 1;
            this.exceptionButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.exceptionButton.Name = "exceptionButton";
            this.exceptionButton.Text = ResourceService.GetString("SkyMap.Net.Core.Services.ErrorDialogs.ShowExceptionButton");
            this.exceptionButton.Size = new Size(110, 0x1b);
            this.exceptionButton.Location = new Point(0x17e, 0x11d);
            this.exceptionButton.Click += new EventHandler(this.ShowException);
            base.Controls.Add(this.exceptionButton);
            if (chooseLocationEnabled)
            {
                this.chooseLocationButton = new Button();
                this.chooseLocationButton.Name = "chooseLocationButton";
                this.chooseLocationButton.DialogResult = DialogResult.OK;
                this.chooseLocationButton.TabIndex = 0;
                this.chooseLocationButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                this.chooseLocationButton.Text = ResourceService.GetString("Global.ChooseLocationButtonText");
                this.chooseLocationButton.Size = new Size(110, 0x1b);
                this.chooseLocationButton.Location = new Point(0x108, 0x11d);
            }
            base.Controls.Add(this.chooseLocationButton);
            base.ResumeLayout(false);
            base.Size = new Size(0x20e, 0x106);
        }

        private void ShowException(object sender, EventArgs e)
        {
            MessageService.ShowMessage(this.exceptionGot.ToString(), SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.Core.Services.ErrorDialogs.ExceptionGotDescription}"));
        }
    }
}

