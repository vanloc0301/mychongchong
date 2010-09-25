namespace SkyMap.Net.Tools.Holidays
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Holidays;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HolidaySetting : SmUserControl
    {
        private Button btAddFix;
        private Button btAddHolidays;
        private Button btTemp;
        private Button button2;
        private ComboBox cmbFixName;
        private ContextMenu cmLvFix;
        private ContextMenu cmTvHolidays;
        private ColumnHeader colFixDate;
        private ColumnHeader colFixDesc;
        private Container components = null;
        private UnitOfWork currentUnitOfWork = new UnitOfWork(typeof(Holiday));
        private DateTimePicker dtEnd;
        private DateTimePicker dtFix;
        private DateTimePicker dtSelect;
        private DateTimePicker dtStart;
        private DateTimePicker dtTemp;
        private HybridDictionary fixHolidayItems = new HybridDictionary();
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private HybridDictionary holidayNodes = new HybridDictionary();
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListView lvFix;
        private MenuItem miDelFix;
        private MenuItem miDelHoliday;
        private Panel panel1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Splitter splitter1;
        private TextBox textBox1;
        private TreeView tvHolidays;
        private TextBox txtTempDesc;

        public HolidaySetting()
        {
            this.InitializeComponent();
            SkyMap.Net.Tools.Holidays.TreeViewHelper.SetImageList(this.tvHolidays);
            this.InitHolidayTree();
            this.InitLVFixHolidays();
        }

        private Fixholiday AddFixHoliday(string date, string desc)
        {
            Fixholiday fixholiday = new Fixholiday();
            fixholiday.HoliDate = date;
            fixholiday.Description = desc;
            this.currentUnitOfWork.RegisterNew(fixholiday);
            this.currentUnitOfWork.Commit();
            return fixholiday;
        }

        private void AddFixHolidayItem(Fixholiday fh)
        {
            string holiDate = fh.HoliDate;
            if (!this.fixHolidayItems.Contains(holiDate))
            {
                ListViewItem item = new ListViewItem(new string[] { fh.Description, holiDate });
                item.Tag = fh;
                this.lvFix.Items.Add(item);
                this.fixHolidayItems.Add(fh.HoliDate, item);
            }
        }

        private void AddFixHolidayItems(IList<Fixholiday> fixHolidays)
        {
            foreach (Fixholiday fixholiday in fixHolidays)
            {
                this.AddFixHolidayItem(fixholiday);
            }
        }

        private void AddHolidayNodes(IList<Holiday> holidays)
        {
            foreach (Holiday holiday in holidays)
            {
                DateTime holiDate = holiday.HoliDate;
                if (!this.holidayNodes.Contains(holiDate.Date))
                {
                    TreeNode node2;
                    string monthNodeText = this.GetMonthNodeText(holiDate);
                    if (!this.holidayNodes.Contains(monthNodeText))
                    {
                        TreeNode node;
                        string yearNodeText = this.GetYearNodeText(holiDate);
                        if (!this.holidayNodes.Contains(yearNodeText))
                        {
                            node = SkyMap.Net.Tools.Holidays.TreeViewHelper.AddTreeNode(this.tvHolidays.Nodes, yearNodeText);
                            this.holidayNodes.Add(yearNodeText, node);
                        }
                        else
                        {
                            node = this.holidayNodes[yearNodeText] as TreeNode;
                        }
                        node2 = SkyMap.Net.Tools.Holidays.TreeViewHelper.AddTreeNode(node.Nodes, monthNodeText);
                        this.holidayNodes.Add(monthNodeText, node2);
                    }
                    else
                    {
                        node2 = this.holidayNodes[monthNodeText] as TreeNode;
                    }
                    string dayNodeText = this.GetDayNodeText(holiday);
                    TreeNode node3 = SkyMap.Net.Tools.Holidays.TreeViewHelper.AddTreeNode(node2.Nodes, dayNodeText, holiday);
                    this.holidayNodes.Add(holiDate.Date, node3);
                }
            }
        }

        private void AddHolidays(DateTime[] ts, string[] descs)
        {
            List<Holiday> holidays = new List<Holiday>();
            Holiday holiday = null;
            for (int i = 0; i < ts.Length; i++)
            {
                DateTime key = ts[i];
                if (!this.holidayNodes.Contains(key))
                {
                    holiday = new Holiday();
                    holiday.HoliDate = key;
                    holiday.Description = DateTimeHelper.GetCnDayOfWeek(key);
                    if (!(((descs == null) || (descs.Length <= i)) || StringHelper.IsNull(descs[i])))
                    {
                        holiday.Description = holiday.Description + "," + descs[i];
                    }
                    this.currentUnitOfWork.RegisterNew(holiday);
                    holidays.Add(holiday);
                }
            }
            if (holidays.Count > 0)
            {
                this.currentUnitOfWork.Commit();
                this.AddHolidayNodes(holidays);
            }
        }

        private void btAddFix_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            string desc = this.cmbFixName.Text.Trim();
            if (desc.Length == 0)
            {
                text = "请输入假日名称！";
            }
            string key = this.dtFix.Text;
            if (this.fixHolidayItems.Contains(key))
            {
                text = text + "固定假日已包括：" + key + "！";
            }
            if (text != string.Empty)
            {
                MessageHelper.ShowInfo(text);
            }
            else
            {
                Fixholiday fh = this.AddFixHoliday(key, desc);
                this.AddFixHolidayItem(fh);
            }
        }

        private void btAddHolidaysClick(object sender, EventArgs e)
        {
            string text = string.Empty;
            if (this.dtStart.Value > this.dtEnd.Value)
            {
                text = "开始日期大于起始日期";
                MessageHelper.ShowInfo(text);
            }
            else
            {
                string[] strArray;
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                int year = this.dtStart.Value.Year;
                int num2 = this.dtEnd.Value.Year;
                if (year != num2)
                {
                    strArray = new string[] { year.ToString(), num2.ToString() };
                }
                else
                {
                    strArray = new string[] { year.ToString() };
                }
                foreach (string str2 in this.fixHolidayItems.Keys)
                {
                    foreach (string str3 in strArray)
                    {
                        try
                        {
                            DateTime date = DateTime.Parse(str3 + "-" + str2).Date;
                            if ((date >= this.dtStart.Value.Date) && (date <= this.dtEnd.Value.Date))
                            {
                                list.Add(date);
                                list2.Add(((ListViewItem) this.fixHolidayItems[str2]).SubItems[0].Text);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                DateTime[] weekends = DateTimeHelper.GetWeekends(this.dtStart.Value, this.dtEnd.Value);
                foreach (DateTime time2 in weekends)
                {
                    if (!list.Contains(time2))
                    {
                        list.Add(time2);
                        list2.Add(string.Empty);
                    }
                }
                this.AddHolidays((DateTime[]) list.ToArray(typeof(DateTime)), (string[]) list2.ToArray(typeof(string)));
            }
        }

        private void btTemp_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            string str2 = string.Empty;
            if (this.holidayNodes.Contains(this.dtTemp.Value.Date))
            {
                text = "假日已经包括：" + this.dtTemp.Value.Date.ToShortDateString();
            }
            str2 = this.txtTempDesc.Text.Trim();
            if (str2.Length == 0)
            {
                text = text + "请输入假日描述";
            }
            if (text != string.Empty)
            {
                MessageHelper.ShowInfo(text);
            }
            else
            {
                DateTime[] ts = new DateTime[] { this.dtTemp.Value.Date };
                this.AddHolidays(ts, new string[] { str2 });
            }
        }

        private void cmLvFix_Popup(object sender, EventArgs e)
        {
            this.miDelFix.Enabled = this.lvFix.SelectedItems.Count > 0;
        }

        private void cmTvHolidays_Popup(object sender, EventArgs e)
        {
            this.miDelHoliday.Enabled = ((this.tvHolidays.SelectedNode != null) && (this.tvHolidays.SelectedNode.Tag != null)) && (this.tvHolidays.SelectedNode.Tag is Holiday);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetDayNodeText(Holiday h)
        {
            return (h.HoliDate.ToString("yyyy年M月d日") + "," + h.Description);
        }

        private string GetMonthNodeText(DateTime d)
        {
            return d.ToString("yyyy年M月");
        }

        private string GetYearNodeText(DateTime d)
        {
            return d.ToString("yyyy年");
        }

        private void InitHolidayTree()
        {
            IList<Holiday> holidays = QueryHelper.List<Holiday>("ALL_HOLIDAY_DAO");
            this.AddHolidayNodes(holidays);
        }

        private void InitializeComponent()
        {
            this.tvHolidays = new TreeView();
            this.splitter1 = new Splitter();
            this.panel1 = new Panel();
            this.groupBox4 = new GroupBox();
            this.txtTempDesc = new TextBox();
            this.btTemp = new Button();
            this.dtTemp = new DateTimePicker();
            this.label6 = new Label();
            this.label9 = new Label();
            this.groupBox3 = new GroupBox();
            this.cmbFixName = new ComboBox();
            this.label8 = new Label();
            this.lvFix = new ListView();
            this.colFixDesc = new ColumnHeader();
            this.colFixDate = new ColumnHeader();
            this.btAddFix = new Button();
            this.dtFix = new DateTimePicker();
            this.label7 = new Label();
            this.groupBox1 = new GroupBox();
            this.btAddHolidays = new Button();
            this.dtEnd = new DateTimePicker();
            this.dtStart = new DateTimePicker();
            this.label2 = new Label();
            this.label1 = new Label();
            this.groupBox2 = new GroupBox();
            this.label5 = new Label();
            this.textBox1 = new TextBox();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.button2 = new Button();
            this.dtSelect = new DateTimePicker();
            this.label3 = new Label();
            this.label4 = new Label();
            this.cmTvHolidays = new ContextMenu();
            this.cmLvFix = new ContextMenu();
            this.miDelFix = new MenuItem();
            this.miDelHoliday = new MenuItem();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.tvHolidays.ContextMenu = this.cmTvHolidays;
            this.tvHolidays.Dock = DockStyle.Left;
            this.tvHolidays.ImageIndex = -1;
            this.tvHolidays.Location = new Point(3, 3);
            this.tvHolidays.Name = "tvHolidays";
            this.tvHolidays.SelectedImageIndex = -1;
            this.tvHolidays.Size = new Size(0xa8, 0x1aa);
            this.tvHolidays.TabIndex = 0;
            this.splitter1.Location = new Point(0xab, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0x1aa);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0xae, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x1f7, 0x1aa);
            this.panel1.TabIndex = 2;
            this.groupBox4.Controls.Add(this.txtTempDesc);
            this.groupBox4.Controls.Add(this.btTemp);
            this.groupBox4.Controls.Add(this.dtTemp);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new Point(8, 0x7c);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0xd8, 0x68);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "添加临时假日";
            this.txtTempDesc.Location = new Point(0x58, 0x2d);
            this.txtTempDesc.Name = "txtTempDesc";
            this.txtTempDesc.Size = new Size(0x68, 0x15);
            this.txtTempDesc.TabIndex = 9;
            this.txtTempDesc.Text = "";
            this.btTemp.Location = new Point(0x75, 0x48);
            this.btTemp.Name = "btTemp";
            this.btTemp.TabIndex = 5;
            this.btTemp.Text = "确定";
            this.btTemp.Click += new EventHandler(this.btTemp_Click);
            this.dtTemp.Location = new Point(0x58, 0x13);
            this.dtTemp.Name = "dtTemp";
            this.dtTemp.Size = new Size(0x68, 0x15);
            this.dtTemp.TabIndex = 3;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x30, 0x30);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x1d, 0x11);
            this.label6.TabIndex = 1;
            this.label6.Text = "描述";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x30, 0x18);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x1d, 0x11);
            this.label9.TabIndex = 0;
            this.label9.Text = "日期";
            this.groupBox3.Controls.Add(this.cmbFixName);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lvFix);
            this.groupBox3.Controls.Add(this.btAddFix);
            this.groupBox3.Controls.Add(this.dtFix);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new Point(0xe8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x100, 0x198);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "设定固定节假日";
            this.cmbFixName.Items.AddRange(new object[] { "5.1劳动节", "10.1国庆节", "1.1元旦" });
            this.cmbFixName.Location = new Point(0x88, 0x159);
            this.cmbFixName.Name = "cmbFixName";
            this.cmbFixName.Size = new Size(0x68, 20);
            this.cmbFixName.TabIndex = 10;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x3f, 0x15b);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x36, 0x11);
            this.label8.TabIndex = 9;
            this.label8.Text = "假日名称";
            this.lvFix.Columns.AddRange(new ColumnHeader[] { this.colFixDesc, this.colFixDate });
            this.lvFix.ContextMenu = this.cmLvFix;
            this.lvFix.FullRowSelect = true;
            this.lvFix.GridLines = true;
            this.lvFix.HideSelection = false;
            this.lvFix.Location = new Point(0x10, 20);
            this.lvFix.Name = "lvFix";
            this.lvFix.Size = new Size(0xe0, 0x124);
            this.lvFix.TabIndex = 6;
            this.lvFix.View = View.Details;
            this.colFixDesc.Text = "假日名称";
            this.colFixDesc.Width = 90;
            this.colFixDate.Text = "日期";
            this.colFixDate.Width = 90;
            this.btAddFix.Location = new Point(0xa8, 0x174);
            this.btAddFix.Name = "btAddFix";
            this.btAddFix.TabIndex = 5;
            this.btAddFix.Text = "确定";
            this.btAddFix.Click += new EventHandler(this.btAddFix_Click);
            this.dtFix.CustomFormat = "MM-dd";
            this.dtFix.Format = DateTimePickerFormat.Custom;
            this.dtFix.Location = new Point(0x88, 0x13c);
            this.dtFix.Name = "dtFix";
            this.dtFix.ShowUpDown = true;
            this.dtFix.Size = new Size(0x68, 0x15);
            this.dtFix.TabIndex = 3;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x58, 320);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x1d, 0x11);
            this.label7.TabIndex = 0;
            this.label7.Text = "日期";
            this.groupBox1.Controls.Add(this.btAddHolidays);
            this.groupBox1.Controls.Add(this.dtEnd);
            this.groupBox1.Controls.Add(this.dtStart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd8, 0x70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "成批添加节假日";
            this.btAddHolidays.Location = new Point(0x75, 80);
            this.btAddHolidays.Name = "btAddHolidays";
            this.btAddHolidays.TabIndex = 5;
            this.btAddHolidays.Text = "确定";
            this.btAddHolidays.Click += new EventHandler(this.btAddHolidaysClick);
            this.dtEnd.Location = new Point(0x58, 0x31);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new Size(0x68, 0x15);
            this.dtEnd.TabIndex = 4;
            this.dtStart.Location = new Point(0x58, 0x13);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new Size(0x68, 0x15);
            this.dtStart.TabIndex = 3;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x18, 0x36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x36, 0x11);
            this.label2.TabIndex = 1;
            this.label2.Text = "结束日期";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x18, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x36, 0x11);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始日期";
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.dtSelect);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new Point(8, 0xe8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xd8, 0xb8);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "假日计算";
            this.groupBox2.Visible = false;
            this.label5.ForeColor = Color.Blue;
            this.label5.Location = new Point(0x60, 0x90);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x70, 0x20);
            this.label5.TabIndex = 9;
            this.label5.Text = "用来计算指定工作日数后或前的日期";
            this.textBox1.Location = new Point(0x58, 0x31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x68, 0x15);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "";
            this.radioButton2.Location = new Point(0x68, 80);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabIndex = 7;
            this.radioButton2.Text = "以前工作日";
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new Point(0x10, 80);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "以后工作日";
            this.button2.Location = new Point(120, 0x70);
            this.button2.Name = "button2";
            this.button2.TabIndex = 5;
            this.button2.Text = "确定";
            this.dtSelect.Location = new Point(0x58, 0x13);
            this.dtSelect.Name = "dtSelect";
            this.dtSelect.Size = new Size(0x68, 0x15);
            this.dtSelect.TabIndex = 3;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x18, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x36, 0x11);
            this.label3.TabIndex = 1;
            this.label3.Text = "工作日数";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x18, 0x18);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x36, 0x11);
            this.label4.TabIndex = 0;
            this.label4.Text = "选择日期";
            this.cmTvHolidays.MenuItems.AddRange(new MenuItem[] { this.miDelHoliday });
            this.cmTvHolidays.Popup += new EventHandler(this.cmTvHolidays_Popup);
            this.cmLvFix.MenuItems.AddRange(new MenuItem[] { this.miDelFix });
            this.cmLvFix.Popup += new EventHandler(this.cmLvFix_Popup);
            this.miDelFix.Index = 0;
            this.miDelFix.Text = "删除";
            this.miDelFix.Click += new EventHandler(this.miDelFix_Click);
            this.miDelHoliday.Index = 0;
            this.miDelHoliday.Text = "删除";
            this.miDelHoliday.Click += new EventHandler(this.miDelHoliday_Click);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.tvHolidays);
            base.DockPadding.All = 3;
            base.Name = "HolidaySetting";
            base.Size = new Size(680, 0x1b0);
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitLVFixHolidays()
        {
            IList<Fixholiday> fixHolidays = QueryHelper.List<Fixholiday>("ALL_Fixholiday_DAO");
            this.AddFixHolidayItems(fixHolidays);
        }

        private void miDelFix_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowOkCancelInfo("你确定要删除固定假日吗?") == DialogResult.OK)
            {
                ArrayList list = new ArrayList();
                foreach (ListViewItem item in this.lvFix.SelectedItems)
                {
                    list.Add(item.Tag);
                    this.currentUnitOfWork.RegisterRemoved(item.Tag as Fixholiday);
                }
                this.currentUnitOfWork.Commit();
                foreach (Fixholiday fixholiday in list)
                {
                    this.fixHolidayItems.Remove(fixholiday.HoliDate);
                }
                foreach (ListViewItem item in this.lvFix.SelectedItems)
                {
                    item.Remove();
                }
            }
        }

        private void miDelHoliday_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowOkCancelInfo("你确定要删除假日吗?") == DialogResult.OK)
            {
                Holiday tag = this.tvHolidays.SelectedNode.Tag as Holiday;
                this.currentUnitOfWork.RegisterRemoved(tag);
                this.currentUnitOfWork.Commit();
                this.holidayNodes.Remove(tag.HoliDate);
                this.tvHolidays.SelectedNode.Remove();
            }
        }
    }
}

