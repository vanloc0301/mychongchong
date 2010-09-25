namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class StringListEditor : UserControl
    {
        private Button addButton;
        private Button browseButton;
        private bool browseForDirectory;
        private Button deleteButton;
        private TextBox editTextBox;
        private ListBox listBox;
        private Label listLabel;
        private Button moveDownButton;
        private Button moveUpButton;
        private Button removeButton;
        private Label TitleLabel;
        private Button updateButton;

        public event EventHandler ListChanged;

        public StringListEditor()
        {
            this.InitializeComponent();
            this.ManualOrder = true;
            this.BrowseForDirectory = false;
            this.ListBoxSelectedIndexChanged(null, null);
            this.EditTextBoxTextChanged(null, null);
            this.updateButton.Text = SkyMap.Net.Core.StringParser.Parse(this.updateButton.Text);
            this.removeButton.Text = SkyMap.Net.Core.StringParser.Parse(this.removeButton.Text);
            this.moveUpButton.Image = ResourceService.GetBitmap("Icons.16x16.ArrowUp");
            this.moveDownButton.Image = ResourceService.GetBitmap("Icons.16x16.ArrowDown");
            this.deleteButton.Image = ResourceService.GetBitmap("Icons.16x16.DeleteIcon");
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            this.editTextBox.Text = this.editTextBox.Text.Trim();
            if (this.editTextBox.TextLength > 0)
            {
                int index = this.listBox.Items.IndexOf(this.editTextBox.Text);
                if (index < 0)
                {
                    index = this.listBox.Items.Add(this.editTextBox.Text);
                    this.OnListChanged(EventArgs.Empty);
                }
                this.listBox.SelectedIndex = index;
            }
        }

        private void BrowseButtonClick(object sender, EventArgs e)
        {
            FolderDialog dialog = new FolderDialog();
            if (dialog.DisplayDialog("Select folder") == DialogResult.OK)
            {
                string path = dialog.Path;
                if (!(path.EndsWith(@"\") || path.EndsWith("/")))
                {
                    path = path + @"\";
                }
                this.editTextBox.Text = path;
            }
        }

        private void EditTextBoxTextChanged(object sender, EventArgs e)
        {
            this.addButton.Enabled = this.editTextBox.TextLength > 0;
            this.updateButton.Enabled = (this.listBox.SelectedIndex >= 0) && (this.editTextBox.TextLength > 0);
        }

        public string[] GetList()
        {
            string[] strArray = new string[this.listBox.Items.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = this.listBox.Items[i].ToString();
            }
            return strArray;
        }

        private void InitializeComponent()
        {
            base.Size = new Size(380, 0x110);
            this.removeButton = new Button();
            this.removeButton.Location = new Point(0xa5, 0x35);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new Size(0x4b, 0x17);
            this.removeButton.TabIndex = 5;
            this.removeButton.Text = "${res:Global.DeleteButtonText}";
            this.removeButton.Click += new EventHandler(this.RemoveButtonClick);
            this.deleteButton = new Button();
            this.deleteButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.deleteButton.Location = new Point(0x149, 0xa4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new Size(0x18, 0x18);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Click += new EventHandler(this.RemoveButtonClick);
            this.moveDownButton = new Button();
            this.moveDownButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.moveDownButton.Location = new Point(0x149, 0x86);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new Size(0x18, 0x18);
            this.moveDownButton.TabIndex = 9;
            this.moveDownButton.Click += new EventHandler(this.MoveDownButtonClick);
            this.moveUpButton = new Button();
            this.moveUpButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.moveUpButton.Location = new Point(0x149, 0x68);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new Size(0x18, 0x18);
            this.moveUpButton.TabIndex = 8;
            this.moveUpButton.Click += new EventHandler(this.MoveUpButtonClick);
            this.listBox = new ListBox();
            this.listBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new Point(3, 0x68);
            this.listBox.Name = "listBox";
            this.listBox.Size = new Size(320, 160);
            this.listBox.TabIndex = 7;
            this.listBox.SelectedIndexChanged += new EventHandler(this.ListBoxSelectedIndexChanged);
            this.listLabel = new Label();
            this.listLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listLabel.Location = new Point(3, 0x59);
            this.listLabel.Name = "listLabel";
            this.listLabel.Size = new Size(350, 0x17);
            this.listLabel.TabIndex = 6;
            this.listLabel.Text = "List:";
            this.updateButton = new Button();
            this.updateButton.Location = new Point(0x54, 0x35);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new Size(0x4b, 0x17);
            this.updateButton.TabIndex = 4;
            this.updateButton.Text = "${res:Global.UpdateButtonText}";
            this.updateButton.Click += new EventHandler(this.UpdateButtonClick);
            this.addButton = new Button();
            this.addButton.Location = new Point(3, 0x35);
            this.addButton.Name = "addButton";
            this.addButton.Size = new Size(0x4b, 0x17);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add Item";
            this.addButton.Click += new EventHandler(this.AddButtonClick);
            this.editTextBox = new TextBox();
            this.editTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.editTextBox.Location = new Point(3, 0x1a);
            this.editTextBox.Name = "editTextBox";
            this.editTextBox.Size = new Size(0x13c, 0x15);
            this.editTextBox.TabIndex = 1;
            this.editTextBox.TextChanged += new EventHandler(this.EditTextBoxTextChanged);
            this.browseButton = new Button();
            this.browseButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.browseButton.Location = new Point(0x145, 0x18);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new Size(0x1c, 0x17);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "...";
            this.browseButton.Click += new EventHandler(this.BrowseButtonClick);
            this.TitleLabel = new Label();
            this.TitleLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.TitleLabel.Location = new Point(3, 10);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new Size(350, 0x17);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title:";
            base.Controls.Add(this.removeButton);
            base.Controls.Add(this.deleteButton);
            base.Controls.Add(this.moveDownButton);
            base.Controls.Add(this.moveUpButton);
            base.Controls.Add(this.listBox);
            base.Controls.Add(this.listLabel);
            base.Controls.Add(this.updateButton);
            base.Controls.Add(this.addButton);
            base.Controls.Add(this.editTextBox);
            base.Controls.Add(this.browseButton);
            base.Controls.Add(this.TitleLabel);
            base.Name = "StringListEditor";
        }

        private void ListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox.SelectedIndex >= 0)
            {
                this.editTextBox.Text = this.listBox.Items[this.listBox.SelectedIndex].ToString();
            }
            this.moveUpButton.Enabled = this.listBox.SelectedIndex > 0;
            this.moveDownButton.Enabled = (this.listBox.SelectedIndex >= 0) && (this.listBox.SelectedIndex < (this.listBox.Items.Count - 1));
            this.removeButton.Enabled = this.deleteButton.Enabled = this.listBox.SelectedIndex >= 0;
            this.updateButton.Enabled = (this.listBox.SelectedIndex >= 0) && (this.editTextBox.TextLength > 0);
        }

        public void LoadList(IEnumerable<string> list)
        {
            this.listBox.Items.Clear();
            foreach (string str in list)
            {
                this.listBox.Items.Add(str);
            }
        }

        private void MoveDownButtonClick(object sender, EventArgs e)
        {
            int selectedIndex = this.listBox.SelectedIndex;
            object obj2 = this.listBox.Items[selectedIndex];
            this.listBox.Items[selectedIndex] = this.listBox.Items[selectedIndex + 1];
            this.listBox.Items[selectedIndex + 1] = obj2;
            this.listBox.SelectedIndex = selectedIndex + 1;
            this.OnListChanged(EventArgs.Empty);
        }

        private void MoveUpButtonClick(object sender, EventArgs e)
        {
            int selectedIndex = this.listBox.SelectedIndex;
            object obj2 = this.listBox.Items[selectedIndex];
            this.listBox.Items[selectedIndex] = this.listBox.Items[selectedIndex - 1];
            this.listBox.Items[selectedIndex - 1] = obj2;
            this.listBox.SelectedIndex = selectedIndex - 1;
            this.OnListChanged(EventArgs.Empty);
        }

        protected virtual void OnListChanged(EventArgs e)
        {
            if (this.ListChanged != null)
            {
                this.ListChanged(this, e);
            }
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            this.listBox.Items.RemoveAt(this.listBox.SelectedIndex);
            this.OnListChanged(EventArgs.Empty);
        }

        private void UpdateButtonClick(object sender, EventArgs e)
        {
            this.editTextBox.Text = this.editTextBox.Text.Trim();
            if (this.editTextBox.TextLength > 0)
            {
                this.listBox.Items[this.listBox.SelectedIndex] = this.editTextBox.Text;
                this.OnListChanged(EventArgs.Empty);
            }
        }

        public string AddButtonText
        {
            get
            {
                return this.addButton.Text;
            }
            set
            {
                this.addButton.Text = value;
            }
        }

        public bool BrowseForDirectory
        {
            get
            {
                return this.browseForDirectory;
            }
            set
            {
                this.browseForDirectory = value;
                this.browseButton.Visible = this.browseForDirectory;
            }
        }

        public string ListCaption
        {
            get
            {
                return this.listLabel.Text;
            }
            set
            {
                this.listLabel.Text = value;
            }
        }

        public bool ManualOrder
        {
            get
            {
                return !this.listBox.Sorted;
            }
            set
            {
                this.moveUpButton.Visible = this.moveDownButton.Visible = this.deleteButton.Visible = value;
                this.removeButton.Visible = !value;
                this.listBox.Sorted = !value;
            }
        }

        public string TitleText
        {
            get
            {
                return this.TitleLabel.Text;
            }
            set
            {
                this.TitleLabel.Text = value;
            }
        }
    }
}

