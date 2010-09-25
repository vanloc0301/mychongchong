namespace RedSpider
{
    using RedSpider.db_class;
    using Spider_Global_variables;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class biaoqian : Form
    {
        private TextBox B_egin;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private CheckBox checkBox1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ComboBox comboBox1;
        private IContainer components = null;
        private TextBox E_nd;
        private Rules_panel fm2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox lable_name;
        private ListView listView1;
        private ListView listView2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private bool badd = false;

        public biaoqian(Rules_panel fm)
        {
            this.fm2 = fm;
            this.InitializeComponent();
        }

        private bool Badd
        {
            get
            {
                return badd;
            }
            set
            {
                badd = value;
            }
        }

        private void biaoqian_Load(object sender, EventArgs e)
        {
            if (Global.LABLE_NAME.ToString() == "")
            {
                Badd = true;
                return;
            }
            Badd = false;
            this.lable();
            this.Exclude_R();
            this.Replacement_R();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.B_egin.Text == "")
            {
                MessageBox.Show("开始符不能为空！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string str = this.B_egin.Text.Replace("'", "''");
                string str2 = this.E_nd.Text.Replace("'", "''");
                Web_Url_Manager manager = new Web_Url_Manager();
                if (this.checkBox1.Checked)
                {
                    if (Badd) 
                    {
                        manager.insert_lable_zz(this.lable_name.Text, str);
                    }
                    else
                    {
                        manager.update_lable_zz(this.lable_name.Text, str);
                    }
                }
                else
                {
                    if (this.E_nd.Text == "")
                    {
                        MessageBox.Show("结束符不能为空！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (Badd)
                    {
                        manager.insert_lable(this.lable_name.Text, str, str2);
                    }
                    else
                    {
                        manager.update_lable(this.lable_name.Text, str, str2);
                    }
                }
                this.fm2.Default_R();
                base.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Global.PAICU = "";
            new Exclude(this).ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要修改的行，然后修改！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                Global.PAICU = item.SubItems[0].Text;
                new Exclude(this).ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要删除的行，然后删除！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                Global.PAICU = item.SubItems[0].Text;
                new Web_Url_Manager().DeL_Exclude();
                this.Exclude_R();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要删除的行，然后删除！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView2.SelectedItems[this.listView2.SelectedItems.Count - 1];
                Global.PAICU = item.SubItems[0].Text;
                new Web_Url_Manager().DeL_Replacement();
                this.Replacement_R();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要修改的行，然后修改！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView2.SelectedItems[this.listView2.SelectedItems.Count - 1];
                Global.PAICU = item.SubItems[0].Text;
                Global.TTH = item.SubItems[1].Text;
                new Replacement(this).ShowDialog();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Global.PAICU = "";
            new Replacement(this).ShowDialog();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.E_nd.Enabled = false;
                this.E_nd.Text = "提示：使用正则中文查找,可以在开始标记中直接输入中文关键字,例：作者　可以找出文章中的作者,多个关鍵字请用空格符分开.例：作者 编辑";
            }
            else
            {
                this.E_nd.Enabled = true;
                this.E_nd.Text = "";
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.lable_name.Text = this.comboBox1.SelectedItem.ToString();
            this.lable_name.Enabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Exclude_R()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command = new OleDbCommand("select * from RedSpider_Exclude_replacement where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='排除'", connection);
                connection.Open();
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                this.listView1.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader.GetString(4) };
                    this.listView1.Items.Add(new ListViewItem(items));
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lable_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.E_nd = new System.Windows.Forms.TextBox();
            this.B_egin = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(7, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 454);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(7, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(398, 428);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.lable_name);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.E_nd);
            this.tabPage1.Controls.Add(this.B_egin);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(390, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "目标约束";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(56, 36);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "正则中文查找";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(63, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(102, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.listView2);
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(7, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(377, 226);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "内容排除替换";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(207, 179);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(23, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "删";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(177, 179);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 23);
            this.button6.TabIndex = 9;
            this.button6.Text = "改";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(148, 179);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(23, 23);
            this.button7.TabIndex = 8;
            this.button7.Text = "添";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(69, 179);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(23, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "删";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(39, 179);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "改";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 179);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(23, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "添";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(138, 50);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(233, 123);
            this.listView2.TabIndex = 4;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "替换前";
            this.columnHeader2.Width = 103;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "替换后";
            this.columnHeader3.Width = 119;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(6, 50);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(121, 123);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "排除";
            this.columnHeader1.Width = 107;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(266, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "保　存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "替换：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "排除：";
            // 
            // lable_name
            // 
            this.lable_name.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lable_name.ForeColor = System.Drawing.Color.Red;
            this.lable_name.Location = new System.Drawing.Point(184, 9);
            this.lable_name.Name = "lable_name";
            this.lable_name.Size = new System.Drawing.Size(99, 23);
            this.lable_name.TabIndex = 5;
            this.lable_name.Click += new System.EventHandler(this.lable_name_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "标签名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "结束";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "开始";
            // 
            // E_nd
            // 
            this.E_nd.Location = new System.Drawing.Point(207, 57);
            this.E_nd.Multiline = true;
            this.E_nd.Name = "E_nd";
            this.E_nd.Size = new System.Drawing.Size(177, 97);
            this.E_nd.TabIndex = 1;
            // 
            // B_egin
            // 
            this.B_egin.Location = new System.Drawing.Point(6, 58);
            this.B_egin.Multiline = true;
            this.B_egin.Name = "B_egin";
            this.B_egin.Size = new System.Drawing.Size(188, 97);
            this.B_egin.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(390, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "固定格式";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // biaoqian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 470);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "biaoqian";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "规则";
            this.Load += new System.EventHandler(this.biaoqian_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        private void lable()
        {            
            this.lable_name.Text = Global.LABLE_NAME.ToString();         
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM RedSpider_Label where Label_name='" + Global.LABLE_NAME.ToString() + "' and url_id='" + Global.URL_BIANHAO.ToString() + "'", connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            dataSet.Clear();
            adapter.Fill(dataSet, "RedSpider_Label");
            this.B_egin.Text = dataSet.Tables["RedSpider_Label"].Rows[0]["B_egin"].ToString();
            if (dataSet.Tables["RedSpider_Label"].Rows[0]["zzyes"].ToString() == "是")
            {
                this.checkBox1.Checked = true;
                this.E_nd.Enabled = false;
                this.E_nd.Text = "提示：使用正则中文查找,可以在开始标记中直接输入中文关键字,例：作者　可以找出文章中的作者,多个关鍵字请用空格符分开.例：作者 编辑";
            }
            else
            {
                this.checkBox1.Checked = false;
                this.E_nd.Text = dataSet.Tables["RedSpider_Label"].Rows[0]["E_nd"].ToString();
            }
        }

        private void lable_name_Click(object sender, EventArgs e)
        {
        }

        public void Replacement_R()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command = new OleDbCommand("select * from RedSpider_Exclude_replacement where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='替换'", connection);
                connection.Open();
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                this.listView2.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader.GetString(4), reader.GetString(5) };
                    this.listView2.Items.Add(new ListViewItem(items));
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

