namespace Crawler
{
    using LiteLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SettingsForm : Form
    {
        private Button buttonAddExt;
        private Button buttonDeleteExt;
        private Button buttonDownloadFolderBrowse;
        private Button buttonEditExt;
        private Button buttonSettingsCancel;
        private Button buttonSettingsOK;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBoxKeepURLServer;
        private CheckBox checkBoxSettingsUseWindowsDefaultCodePage;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeaderMatchName;
        private ColumnHeader columnHeaderMatchText;
        private ComboBox comboBoxSettingsCodePage;
        private Container components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListView listViewFileMatches;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDownRequests;
        private NumericUpDown numericUpDownRunThreadsCount;
        private NumericUpDown numericUpDownSleepTime;
        private NumericUpDown numericUpDownWebDepth;
        public int SelectedIndex = -1;
        private TabControl tabControlSettings;
        private TabPage tabPageAdvanced;
        private TabPage tabPageConnections;
        private TabPage tabPageFileMatches;
        private TabPage tabPageOutput;
        private TextBox textBoxDownloadFolder;
        private TextBox textBoxExcludeFiles;
        private TextBox textBoxExcludePages;
        private TextBox textBoxExcludeURLs;

        public SettingsForm()
        {
            this.InitializeComponent();
        }

        private void buttonAddExt_Click(object sender, EventArgs e)
        {
            FileTypeForm form = new FileTypeForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                ListViewItem item = this.listViewFileMatches.Items.Add(form.textBoxTypeDescription.Text);
                item.SubItems.Add(form.numericUpDownMinSize.Value.ToString());
                item.SubItems.Add(form.numericUpDownMaxSize.Value.ToString());
            }
        }

        private void buttonDeleteExt_Click(object sender, EventArgs e)
        {
            if (this.listViewFileMatches.SelectedItems.Count != 0)
            {
                this.listViewFileMatches.SelectedItems[0].Remove();
            }
        }

        private void buttonDownloadFolderBrowse_Click(object sender, EventArgs e)
        {
            BrowseForFolderClass class2 = new BrowseForFolderClass {
                Title = "请选择文件存放目录－－[军长搜索]"
            };
            if (class2.ShowDialog() == DialogResult.OK)
            {
                this.textBoxDownloadFolder.Text = class2.DirectoryPath;
            }
        }

        private void buttonEditExt_Click(object sender, EventArgs e)
        {
            if (this.listViewFileMatches.SelectedItems.Count != 0)
            {
                ListViewItem item = this.listViewFileMatches.SelectedItems[0];
                FileTypeForm form = new FileTypeForm {
                    textBoxTypeDescription = { Text = item.Text }
                };
                if (item.SubItems.Count <= 1)
                {
                    item.SubItems.Add("0");
                }
                form.numericUpDownMinSize.Value = int.Parse(item.SubItems[1].Text);
                if (item.SubItems.Count <= 2)
                {
                    item.SubItems.Add("0");
                }
                form.numericUpDownMaxSize.Value = int.Parse(item.SubItems[2].Text);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = form.textBoxTypeDescription.Text;
                    item.SubItems[1].Text = form.numericUpDownMinSize.Value.ToString();
                    item.SubItems[2].Text = form.numericUpDownMaxSize.Value.ToString();
                }
            }
        }

        private void buttonSettingsCancel_Click(object sender, EventArgs e)
        {
        }

        private void buttonSettingsOK_Click(object sender, EventArgs e)
        {
            Settings.SetValue((Form) this);
        }

        private void checkBoxSettingsUseWindowsDefaultCodePage_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBoxSettingsCodePage.Enabled = !this.checkBoxSettingsUseWindowsDefaultCodePage.Checked;
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
            ListViewItem item = new ListViewItem(new string[] { "text/richtext", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item2 = new ListViewItem(new string[] { "text/html", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item3 = new ListViewItem(new string[] { "audio/x-aiff", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item4 = new ListViewItem(new string[] { "audio/basic", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item5 = new ListViewItem(new string[] { "audio/wav", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item6 = new ListViewItem(new string[] { "image/gif", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item7 = new ListViewItem(new string[] { "image/jpeg", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item8 = new ListViewItem(new string[] { "image/pjpeg", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item9 = new ListViewItem(new string[] { "image/tiff", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item10 = new ListViewItem(new string[] { "image/x-png", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item11 = new ListViewItem(new string[] { "image/x-xbitmap", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item12 = new ListViewItem(new string[] { "image/bmp", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item13 = new ListViewItem(new string[] { "image/x-jg", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item14 = new ListViewItem(new string[] { "image/x-emf", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item15 = new ListViewItem(new string[] { "image/x-wmf", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item16 = new ListViewItem(new string[] { "video/avi", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item17 = new ListViewItem(new string[] { "video/mpeg", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item18 = new ListViewItem(new string[] { "application/postscript", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item19 = new ListViewItem(new string[] { "application/base64", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item20 = new ListViewItem(new string[] { "application/macbinhex40", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item21 = new ListViewItem(new string[] { "application/pdf", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item22 = new ListViewItem(new string[] { "application/x-compressed", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item23 = new ListViewItem(new string[] { "application/x-zip-compressed", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item24 = new ListViewItem(new string[] { "application/x-gzip-compressed", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item25 = new ListViewItem(new string[] { "application/java", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            ListViewItem item26 = new ListViewItem(new string[] { "application/x-msdownload", "0", "0" }, -1, SystemColors.WindowText, Color.WhiteSmoke, new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0));
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tabControlSettings = new TabControl();
            this.tabPageFileMatches = new TabPage();
            this.checkBox1 = new CheckBox();
            this.buttonAddExt = new Button();
            this.label9 = new Label();
            this.listViewFileMatches = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.buttonEditExt = new Button();
            this.buttonDeleteExt = new Button();
            this.tabPageOutput = new TabPage();
            this.groupBox3 = new GroupBox();
            this.buttonDownloadFolderBrowse = new Button();
            this.label10 = new Label();
            this.textBoxDownloadFolder = new TextBox();
            this.numericUpDownRequests = new NumericUpDown();
            this.tabPageConnections = new TabPage();
            this.label16 = new Label();
            this.label15 = new Label();
            this.label8 = new Label();
            this.label7 = new Label();
            this.label1 = new Label();
            this.checkBox2 = new CheckBox();
            this.numericUpDownRunThreadsCount = new NumericUpDown();
            this.numericUpDownSleepTime = new NumericUpDown();
            this.numericUpDown1 = new NumericUpDown();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label11 = new Label();
            this.numericUpDownWebDepth = new NumericUpDown();
            this.checkBoxKeepURLServer = new CheckBox();
            this.label12 = new Label();
            this.label13 = new Label();
            this.numericUpDown2 = new NumericUpDown();
            this.tabPageAdvanced = new TabPage();
            this.groupBox4 = new GroupBox();
            this.label19 = new Label();
            this.textBoxExcludeURLs = new TextBox();
            this.groupBox2 = new GroupBox();
            this.textBoxExcludePages = new TextBox();
            this.label17 = new Label();
            this.textBoxExcludeFiles = new TextBox();
            this.label18 = new Label();
            this.groupBox1 = new GroupBox();
            this.checkBoxSettingsUseWindowsDefaultCodePage = new CheckBox();
            this.comboBoxSettingsCodePage = new ComboBox();
            this.buttonSettingsOK = new Button();
            this.buttonSettingsCancel = new Button();
            this.columnHeaderMatchText = new ColumnHeader();
            this.columnHeaderMatchName = new ColumnHeader();
            this.tabControlSettings.SuspendLayout();
            this.tabPageFileMatches.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.numericUpDownRequests.BeginInit();
            this.tabPageConnections.SuspendLayout();
            this.numericUpDownRunThreadsCount.BeginInit();
            this.numericUpDownSleepTime.BeginInit();
            this.numericUpDown1.BeginInit();
            this.numericUpDownWebDepth.BeginInit();
            this.numericUpDown2.BeginInit();
            this.tabPageAdvanced.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.tabControlSettings.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControlSettings.Controls.Add(this.tabPageFileMatches);
            this.tabControlSettings.Controls.Add(this.tabPageOutput);
            this.tabControlSettings.Controls.Add(this.tabPageConnections);
            this.tabControlSettings.Controls.Add(this.tabPageAdvanced);
            this.tabControlSettings.Location = new Point(0, 3);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new Size(0x252, 0x164);
            this.tabControlSettings.TabIndex = 1;
            this.tabControlSettings.Tag = "Settings tab";
            this.tabPageFileMatches.Controls.Add(this.checkBox1);
            this.tabPageFileMatches.Controls.Add(this.buttonAddExt);
            this.tabPageFileMatches.Controls.Add(this.label9);
            this.tabPageFileMatches.Controls.Add(this.listViewFileMatches);
            this.tabPageFileMatches.Controls.Add(this.buttonEditExt);
            this.tabPageFileMatches.Controls.Add(this.buttonDeleteExt);
            this.tabPageFileMatches.Location = new Point(4, 0x15);
            this.tabPageFileMatches.Name = "tabPageFileMatches";
            this.tabPageFileMatches.Size = new Size(0x24a, 0x14b);
            this.tabPageFileMatches.TabIndex = 4;
            this.tabPageFileMatches.Text = "数据类型";
            this.tabPageFileMatches.UseVisualStyleBackColor = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.FlatStyle = FlatStyle.Popup;
            this.checkBox1.Location = new Point(0x13, 9);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0xca, 0x19);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Tag = "Allow all MIME types";
            this.checkBox1.Text = "加载所有的MIME类型";
            this.buttonAddExt.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonAddExt.Location = new Point(0x1d, 0x127);
            this.buttonAddExt.Name = "buttonAddExt";
            this.buttonAddExt.Size = new Size(90, 0x17);
            this.buttonAddExt.TabIndex = 1;
            this.buttonAddExt.Text = "&添加...";
            this.buttonAddExt.Click += new EventHandler(this.buttonAddExt_Click);
            this.label9.Location = new Point(0x13, 0x22);
            this.label9.Name = "label9";
            this.label9.Size = new Size(120, 0x12);
            this.label9.TabIndex = 7;
            this.label9.Text = "MIME 类型: ";
            this.listViewFileMatches.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listViewFileMatches.BackColor = Color.WhiteSmoke;
            this.listViewFileMatches.CheckBoxes = true;
            this.listViewFileMatches.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.listViewFileMatches.FullRowSelect = true;
            this.listViewFileMatches.GridLines = true;
            item.Checked = true;
            item.StateImageIndex = 1;
            item.Tag = "";
            item2.Checked = true;
            item2.StateImageIndex = 1;
            item2.Tag = "";
            item3.Checked = true;
            item3.StateImageIndex = 1;
            item3.Tag = "";
            item4.Checked = true;
            item4.StateImageIndex = 1;
            item4.Tag = "";
            item5.Checked = true;
            item5.StateImageIndex = 1;
            item5.Tag = "";
            item6.Checked = true;
            item6.StateImageIndex = 1;
            item6.Tag = "";
            item7.Checked = true;
            item7.StateImageIndex = 1;
            item7.Tag = "";
            item8.Checked = true;
            item8.StateImageIndex = 1;
            item8.Tag = "";
            item9.Checked = true;
            item9.StateImageIndex = 1;
            item9.Tag = "";
            item10.Checked = true;
            item10.StateImageIndex = 1;
            item10.Tag = "";
            item11.Checked = true;
            item11.StateImageIndex = 1;
            item11.Tag = "";
            item12.Checked = true;
            item12.StateImageIndex = 1;
            item12.Tag = "";
            item13.Checked = true;
            item13.StateImageIndex = 1;
            item13.Tag = "";
            item14.Checked = true;
            item14.StateImageIndex = 1;
            item14.Tag = "";
            item15.Checked = true;
            item15.StateImageIndex = 1;
            item15.Tag = "";
            item16.Checked = true;
            item16.StateImageIndex = 1;
            item16.Tag = "";
            item17.Checked = true;
            item17.StateImageIndex = 1;
            item17.Tag = "";
            item18.Checked = true;
            item18.StateImageIndex = 1;
            item18.Tag = "";
            item19.Checked = true;
            item19.StateImageIndex = 1;
            item19.Tag = "";
            item20.Checked = true;
            item20.StateImageIndex = 1;
            item20.Tag = "";
            item21.Checked = true;
            item21.StateImageIndex = 1;
            item21.Tag = "";
            item22.Checked = true;
            item22.StateImageIndex = 1;
            item22.Tag = "";
            item23.Checked = true;
            item23.StateImageIndex = 1;
            item23.Tag = "";
            item24.Checked = true;
            item24.StateImageIndex = 1;
            item24.Tag = "";
            item25.Checked = true;
            item25.StateImageIndex = 1;
            item25.Tag = "";
            item26.Checked = true;
            item26.StateImageIndex = 1;
            item26.Tag = "";
            this.listViewFileMatches.Items.AddRange(new ListViewItem[] { 
                item, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, 
                item17, item18, item19, item20, item21, item22, item23, item24, item25, item26
             });
            this.listViewFileMatches.Location = new Point(0x13, 0x34);
            this.listViewFileMatches.MultiSelect = false;
            this.listViewFileMatches.Name = "listViewFileMatches";
            this.listViewFileMatches.Size = new Size(0x222, 0xea);
            this.listViewFileMatches.TabIndex = 0;
            this.listViewFileMatches.Tag = "Settings";
            this.listViewFileMatches.UseCompatibleStateImageBehavior = false;
            this.listViewFileMatches.View = View.Details;
            this.columnHeader1.Text = "Category";
            this.columnHeader1.Width = 0xc9;
            this.columnHeader2.Text = "Min";
            this.columnHeader2.Width = 50;
            this.columnHeader3.Text = "Max";
            this.columnHeader3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader3.Width = 50;
            this.buttonEditExt.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonEditExt.Location = new Point(0x7d, 0x127);
            this.buttonEditExt.Name = "buttonEditExt";
            this.buttonEditExt.Size = new Size(90, 0x17);
            this.buttonEditExt.TabIndex = 2;
            this.buttonEditExt.Text = "&修改...";
            this.buttonEditExt.Click += new EventHandler(this.buttonEditExt_Click);
            this.buttonDeleteExt.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonDeleteExt.Location = new Point(0xdd, 0x127);
            this.buttonDeleteExt.Name = "buttonDeleteExt";
            this.buttonDeleteExt.Size = new Size(90, 0x17);
            this.buttonDeleteExt.TabIndex = 3;
            this.buttonDeleteExt.Text = "&删除";
            this.buttonDeleteExt.Click += new EventHandler(this.buttonDeleteExt_Click);
            this.tabPageOutput.Controls.Add(this.groupBox3);
            this.tabPageOutput.Location = new Point(4, 0x15);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Size = new Size(0x24a, 0x14b);
            this.tabPageOutput.TabIndex = 2;
            this.tabPageOutput.Text = "输出目录";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            this.groupBox3.Controls.Add(this.buttonDownloadFolderBrowse);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBoxDownloadFolder);
            this.groupBox3.Controls.Add(this.numericUpDownRequests);
            this.groupBox3.Location = new Point(0x13, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x222, 0x133);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "配置";
            this.buttonDownloadFolderBrowse.Location = new Point(0x1d8, 0x22);
            this.buttonDownloadFolderBrowse.Name = "buttonDownloadFolderBrowse";
            this.buttonDownloadFolderBrowse.Size = new Size(0x1d, 0x16);
            this.buttonDownloadFolderBrowse.TabIndex = 5;
            this.buttonDownloadFolderBrowse.Text = "...";
            this.buttonDownloadFolderBrowse.Click += new EventHandler(this.buttonDownloadFolderBrowse_Click);
            this.label10.Location = new Point(0x13, 0x22);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x73, 0x12);
            this.label10.TabIndex = 4;
            this.label10.Text = "文件下载存放目录";
            this.textBoxDownloadFolder.BackColor = Color.WhiteSmoke;
            this.textBoxDownloadFolder.Location = new Point(0xa3, 0x22);
            this.textBoxDownloadFolder.Name = "textBoxDownloadFolder";
            this.textBoxDownloadFolder.Size = new Size(0xc0, 0x15);
            this.textBoxDownloadFolder.TabIndex = 4;
            this.textBoxDownloadFolder.Tag = "Download folder";
            this.numericUpDownRequests.BackColor = Color.WhiteSmoke;
            this.numericUpDownRequests.Location = new Point(0x175, 0x22);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.numericUpDownRequests.Maximum = new decimal(bits);
            this.numericUpDownRequests.Name = "numericUpDownRequests";
            this.numericUpDownRequests.Size = new Size(0x4d, 0x15);
            this.numericUpDownRequests.TabIndex = 10;
            this.numericUpDownRequests.Tag = "View last requests count";
            bits = new int[4];
            bits[0] = 20;
            this.numericUpDownRequests.Value = new decimal(bits);
            this.tabPageConnections.Controls.Add(this.label16);
            this.tabPageConnections.Controls.Add(this.label15);
            this.tabPageConnections.Controls.Add(this.label8);
            this.tabPageConnections.Controls.Add(this.label7);
            this.tabPageConnections.Controls.Add(this.label1);
            this.tabPageConnections.Controls.Add(this.checkBox2);
            this.tabPageConnections.Controls.Add(this.numericUpDownRunThreadsCount);
            this.tabPageConnections.Controls.Add(this.numericUpDownSleepTime);
            this.tabPageConnections.Controls.Add(this.numericUpDown1);
            this.tabPageConnections.Controls.Add(this.label5);
            this.tabPageConnections.Controls.Add(this.label6);
            this.tabPageConnections.Controls.Add(this.label11);
            this.tabPageConnections.Controls.Add(this.numericUpDownWebDepth);
            this.tabPageConnections.Controls.Add(this.checkBoxKeepURLServer);
            this.tabPageConnections.Controls.Add(this.label12);
            this.tabPageConnections.Controls.Add(this.label13);
            this.tabPageConnections.Controls.Add(this.numericUpDown2);
            this.tabPageConnections.Location = new Point(4, 0x15);
            this.tabPageConnections.Name = "tabPageConnections";
            this.tabPageConnections.Size = new Size(0x24a, 0x14b);
            this.tabPageConnections.TabIndex = 3;
            this.tabPageConnections.Text = "连接设置";
            this.tabPageConnections.UseVisualStyleBackColor = true;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x13, 0x97);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x5f, 12);
            this.label16.TabIndex = 13;
            this.label16.Text = "浏览网页的深度:";
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x13, 0x72);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x3b, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "超时链接:";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x13, 0x4f);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x5f, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "每／秒工作一次:";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x13, 0x2f);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x9b, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "线程停止的时候，队列清空:";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "线程数量";
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = CheckState.Checked;
            this.checkBox2.FlatStyle = FlatStyle.Popup;
            this.checkBox2.Location = new Point(0x15, 0xfd);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0x101, 0x1a);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Tag = "Keep connection alive";
            this.checkBox2.Text = "保持联系有用";
            this.numericUpDownRunThreadsCount.BackColor = Color.WhiteSmoke;
            this.numericUpDownRunThreadsCount.Location = new Point(0x12a, 9);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownRunThreadsCount.Minimum = new decimal(bits);
            this.numericUpDownRunThreadsCount.Name = "numericUpDownRunThreadsCount";
            this.numericUpDownRunThreadsCount.Size = new Size(0x4c, 0x15);
            this.numericUpDownRunThreadsCount.TabIndex = 0;
            this.numericUpDownRunThreadsCount.Tag = "Threads count";
            bits = new int[4];
            bits[0] = 10;
            this.numericUpDownRunThreadsCount.Value = new decimal(bits);
            this.numericUpDownSleepTime.BackColor = Color.WhiteSmoke;
            this.numericUpDownSleepTime.Location = new Point(0x12a, 0x2b);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDownSleepTime.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownSleepTime.Minimum = new decimal(bits);
            this.numericUpDownSleepTime.Name = "numericUpDownSleepTime";
            this.numericUpDownSleepTime.Size = new Size(0x4c, 0x15);
            this.numericUpDownSleepTime.TabIndex = 1;
            this.numericUpDownSleepTime.Tag = "Sleep fetch time";
            bits = new int[4];
            bits[0] = 2;
            this.numericUpDownSleepTime.Value = new decimal(bits);
            this.numericUpDown1.BackColor = Color.WhiteSmoke;
            this.numericUpDown1.Location = new Point(0x12a, 0x4f);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown1.Maximum = new decimal(bits);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x4c, 0x15);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Tag = "Sleep connect time";
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown1.Value = new decimal(bits);
            this.label5.Location = new Point(0x180, 0x2f);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4d, 0x12);
            this.label5.TabIndex = 0;
            this.label5.Text = "秒";
            this.label6.Location = new Point(0x180, 0x4f);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x4d, 0x12);
            this.label6.TabIndex = 0;
            this.label6.Text = "秒";
            this.label11.Location = new Point(0x17e, 13);
            this.label11.Name = "label11";
            this.label11.Size = new Size(90, 0x11);
            this.label11.TabIndex = 0;
            this.label11.Tag = "";
            this.label11.Text = "线程";
            this.numericUpDownWebDepth.BackColor = Color.WhiteSmoke;
            this.numericUpDownWebDepth.Location = new Point(0x12a, 0x95);
            bits = new int[4];
            bits[0] = 20;
            this.numericUpDownWebDepth.Maximum = new decimal(bits);
            this.numericUpDownWebDepth.Name = "numericUpDownWebDepth";
            this.numericUpDownWebDepth.Size = new Size(0x4c, 0x15);
            this.numericUpDownWebDepth.TabIndex = 5;
            this.numericUpDownWebDepth.Tag = "Web depth";
            bits = new int[4];
            bits[0] = 3;
            this.numericUpDownWebDepth.Value = new decimal(bits);
            this.checkBoxKeepURLServer.Checked = true;
            this.checkBoxKeepURLServer.CheckState = CheckState.Checked;
            this.checkBoxKeepURLServer.FlatStyle = FlatStyle.Popup;
            this.checkBoxKeepURLServer.Location = new Point(0x15, 230);
            this.checkBoxKeepURLServer.Name = "checkBoxKeepURLServer";
            this.checkBoxKeepURLServer.Size = new Size(0x161, 0x11);
            this.checkBoxKeepURLServer.TabIndex = 7;
            this.checkBoxKeepURLServer.Tag = "Keep same URL server";
            this.checkBoxKeepURLServer.Text = "保持同一网站[目前支持2-3级子域名](全网搜索请不要勾选)";
            this.label12.Location = new Point(0x180, 0x99);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x4d, 0x11);
            this.label12.TabIndex = 0;
            this.label12.Text = "页";
            this.label13.Location = new Point(0x180, 0x74);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x4d, 0x11);
            this.label13.TabIndex = 0;
            this.label13.Text = "秒";
            this.numericUpDown2.BackColor = Color.WhiteSmoke;
            this.numericUpDown2.Location = new Point(0x12a, 0x72);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown2.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown2.Minimum = new decimal(bits);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x4c, 0x15);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.Tag = "Request timeout";
            bits = new int[4];
            bits[0] = 20;
            this.numericUpDown2.Value = new decimal(bits);
            this.tabPageAdvanced.Controls.Add(this.groupBox4);
            this.tabPageAdvanced.Controls.Add(this.groupBox2);
            this.tabPageAdvanced.Controls.Add(this.groupBox1);
            this.tabPageAdvanced.Location = new Point(4, 0x15);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Size = new Size(0x24a, 0x14b);
            this.tabPageAdvanced.TabIndex = 1;
            this.tabPageAdvanced.Text = "排除相关";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.textBoxExcludeURLs);
            this.groupBox4.Location = new Point(0x13, 0x4a);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x222, 0x61);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "域名排除";
            this.label19.Location = new Point(6, 0x21);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x1e9, 0x11);
            this.label19.TabIndex = 5;
            this.label19.Text = "排除域名后缀(如:.org:.gov,多个域名后缀之间请用\":\"号隔开)";
            this.textBoxExcludeURLs.BackColor = Color.WhiteSmoke;
            this.textBoxExcludeURLs.Location = new Point(6, 50);
            this.textBoxExcludeURLs.Name = "textBoxExcludeURLs";
            this.textBoxExcludeURLs.Size = new Size(0x1e9, 0x15);
            this.textBoxExcludeURLs.TabIndex = 1;
            this.textBoxExcludeURLs.Tag = "Exclude Hosts";
            this.groupBox2.Controls.Add(this.textBoxExcludePages);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.textBoxExcludeFiles);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Location = new Point(0x13, 0x1f);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x222, 0x18);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.textBoxExcludePages.BackColor = Color.WhiteSmoke;
            this.textBoxExcludePages.Location = new Point(10, 0x2b);
            this.textBoxExcludePages.Name = "textBoxExcludePages";
            this.textBoxExcludePages.Size = new Size(0x1e9, 0x15);
            this.textBoxExcludePages.TabIndex = 0;
            this.textBoxExcludePages.Tag = "Exclude words";
            this.label17.Location = new Point(10, 0x1a);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x1e9, 0x11);
            this.label17.TabIndex = 5;
            this.label17.Text = "Exclude pages contain these words: (use semicolon separator)";
            this.textBoxExcludeFiles.BackColor = Color.WhiteSmoke;
            this.textBoxExcludeFiles.Location = new Point(10, 0x81);
            this.textBoxExcludeFiles.Name = "textBoxExcludeFiles";
            this.textBoxExcludeFiles.Size = new Size(0x1e9, 0x15);
            this.textBoxExcludeFiles.TabIndex = 2;
            this.textBoxExcludeFiles.Tag = "Exclude files";
            this.label18.Location = new Point(10, 0x70);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x1e9, 0x11);
            this.label18.TabIndex = 5;
            this.label18.Text = "Exclude files with these extensions from parsing: (use semicolon separator)";
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.checkBoxSettingsUseWindowsDefaultCodePage);
            this.groupBox1.Controls.Add(this.comboBoxSettingsCodePage);
            this.groupBox1.Location = new Point(0x13, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x222, 0x11);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.checkBoxSettingsUseWindowsDefaultCodePage.Checked = true;
            this.checkBoxSettingsUseWindowsDefaultCodePage.CheckState = CheckState.Checked;
            this.checkBoxSettingsUseWindowsDefaultCodePage.FlatStyle = FlatStyle.Popup;
            this.checkBoxSettingsUseWindowsDefaultCodePage.Location = new Point(0x13, 0x34);
            this.checkBoxSettingsUseWindowsDefaultCodePage.Name = "checkBoxSettingsUseWindowsDefaultCodePage";
            this.checkBoxSettingsUseWindowsDefaultCodePage.Size = new Size(0xad, 0x11);
            this.checkBoxSettingsUseWindowsDefaultCodePage.TabIndex = 3;
            this.checkBoxSettingsUseWindowsDefaultCodePage.Tag = "Use windows default code page";
            this.checkBoxSettingsUseWindowsDefaultCodePage.Text = "Use windows default";
            this.checkBoxSettingsUseWindowsDefaultCodePage.CheckedChanged += new EventHandler(this.checkBoxSettingsUseWindowsDefaultCodePage_CheckedChanged);
            this.comboBoxSettingsCodePage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.comboBoxSettingsCodePage.BackColor = Color.WhiteSmoke;
            this.comboBoxSettingsCodePage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSettingsCodePage.Items.AddRange(new object[] { "Arabic (1256)", "Baltic (1257)", "Chinese (Taiwan, Hong Kong) (950)", "Cyrillic (1251)", "Greek (1253)", "Hebrew (1255)", "Japanese (932)", "Korean Extended Wansung (949)", "Latin 1 (1252)", "Latin 2 (1250)", "Latin 5 (1254)", "PRC GBK (XGB) (936)", "Thai (874)", "Viet Nam (1258)" });
            this.comboBoxSettingsCodePage.Location = new Point(0x13, 0x1a);
            this.comboBoxSettingsCodePage.MaxDropDownItems = 20;
            this.comboBoxSettingsCodePage.Name = "comboBoxSettingsCodePage";
            this.comboBoxSettingsCodePage.Size = new Size(0x1fc, 20);
            this.comboBoxSettingsCodePage.Sorted = true;
            this.comboBoxSettingsCodePage.TabIndex = 0;
            this.comboBoxSettingsCodePage.Tag = "Settings";
            this.buttonSettingsOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonSettingsOK.DialogResult = DialogResult.OK;
            this.buttonSettingsOK.Location = new Point(0x17f, 0x16d);
            this.buttonSettingsOK.Name = "buttonSettingsOK";
            this.buttonSettingsOK.Size = new Size(90, 0x18);
            this.buttonSettingsOK.TabIndex = 1;
            this.buttonSettingsOK.Text = "确定";
            this.buttonSettingsOK.Click += new EventHandler(this.buttonSettingsOK_Click);
            this.buttonSettingsCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonSettingsCancel.DialogResult = DialogResult.Cancel;
            this.buttonSettingsCancel.Location = new Point(0x1df, 0x16d);
            this.buttonSettingsCancel.Name = "buttonSettingsCancel";
            this.buttonSettingsCancel.Size = new Size(90, 0x18);
            this.buttonSettingsCancel.TabIndex = 1;
            this.buttonSettingsCancel.Text = "取消";
            this.buttonSettingsCancel.Click += new EventHandler(this.buttonSettingsCancel_Click);
            this.columnHeaderMatchText.Text = "Description";
            this.columnHeaderMatchText.Width = 300;
            this.columnHeaderMatchName.Text = "Match";
            this.columnHeaderMatchName.Width = 90;
            base.AcceptButton = this.buttonSettingsOK;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.CancelButton = this.buttonSettingsCancel;
            base.ClientSize = new Size(0x252, 0x18c);
            base.Controls.Add(this.buttonSettingsOK);
            base.Controls.Add(this.tabControlSettings);
            base.Controls.Add(this.buttonSettingsCancel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SettingsForm";
            base.ShowInTaskbar = false;
            this.Text = "系统设定";
            base.Load += new EventHandler(this.SettingsForm_Load);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageFileMatches.ResumeLayout(false);
            this.tabPageOutput.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.numericUpDownRequests.EndInit();
            this.tabPageConnections.ResumeLayout(false);
            this.tabPageConnections.PerformLayout();
            this.numericUpDownRunThreadsCount.EndInit();
            this.numericUpDownSleepTime.EndInit();
            this.numericUpDown1.EndInit();
            this.numericUpDownWebDepth.EndInit();
            this.numericUpDown2.EndInit();
            this.tabPageAdvanced.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Settings.GetValue((Form) this);
            this.comboBoxSettingsCodePage.Enabled = !this.checkBoxSettingsUseWindowsDefaultCodePage.Checked;
            if (this.textBoxDownloadFolder.Text == "")
            {
                this.textBoxDownloadFolder.Text = Environment.CurrentDirectory;
            }
            if (this.SelectedIndex != -1)
            {
                this.tabControlSettings.SelectedIndex = this.SelectedIndex;
            }
        }
    }
}

