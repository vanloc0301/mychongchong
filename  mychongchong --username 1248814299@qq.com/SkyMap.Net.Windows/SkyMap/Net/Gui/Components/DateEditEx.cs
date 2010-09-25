namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class DateEditEx : UserControl, SkyMap.Net.Gui.Components.IContainerControl
    {
        private ComboBoxEdit cmbShortcuts;
        private DateEdit dtEt;
        private DateEdit dtSt;
        private Label label1;
        private Label label2;
        private Label label3;

        public DateEditEx()
        {
            this.InitializeComponent();
            this.dtSt.EditValue = DateTime.Now.Date.AddMonths(-1);
            this.dtEt.EditValue = DateTime.Now.Date;
        }

        private void cmbShortcuts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbShortcuts.SelectedIndexChanged -= new EventHandler(this.cmbShortcuts_SelectedIndexChanged);
            try
            {
                DateTime date = DateTime.Now.Date;
                string text = this.cmbShortcuts.Text;
                if (text != null)
                {
                    if (!(text == "今天"))
                    {
                        if (text == "昨天")
                        {
                            goto Label_00C2;
                        }
                        if (text == "本月")
                        {
                            goto Label_0109;
                        }
                        if (text == "上一月")
                        {
                            goto Label_0172;
                        }
                        if (text == "今年")
                        {
                            goto Label_01ED;
                        }
                        if (text == "上一年")
                        {
                            goto Label_0247;
                        }
                    }
                    else
                    {
                        this.dtSt.EditValue = date;
                        this.dtEt.EditValue = date;
                    }
                }
                goto Label_02A5;
            Label_00C2:
                this.dtSt.EditValue = date.AddDays(-1.0);
                this.dtEt.EditValue = date.AddDays(-1.0);
                goto Label_02A5;
            Label_0109:
                this.dtSt.EditValue = new DateTime(date.Year, date.Month, 1);
                DateTime time2 = new DateTime(date.Year, date.Month, 1);
                this.dtEt.EditValue = time2.AddMonths(1).AddDays(-1.0);
                goto Label_02A5;
            Label_0172:
                time2 = new DateTime(date.Year, date.Month, 1);
                this.dtSt.EditValue = time2.AddMonths(-1);
                time2 = new DateTime(date.Year, date.Month, 1);
                this.dtEt.EditValue = time2.AddMonths(-1).AddMonths(1).AddDays(-1.0);
                goto Label_02A5;
            Label_01ED:
                this.dtSt.EditValue = new DateTime(date.Year, 1, 1);
                time2 = new DateTime(date.Year, 1, 1);
                this.dtEt.EditValue = time2.AddYears(1).AddDays(-1.0);
                goto Label_02A5;
            Label_0247:
                this.dtSt.EditValue = new DateTime(date.Year - 1, 1, 1);
                time2 = new DateTime(date.Year - 1, 1, 1);
                this.dtEt.EditValue = time2.AddYears(1).AddDays(-1.0);
            Label_02A5:
                this.cmbShortcuts.SelectedIndex = -1;
            }
            finally
            {
                this.cmbShortcuts.SelectedIndexChanged += new EventHandler(this.cmbShortcuts_SelectedIndexChanged);
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.dtSt = new DateEdit();
            this.dtEt = new DateEdit();
            this.cmbShortcuts = new ComboBoxEdit();
            this.label3 = new Label();
            this.dtSt.Properties.VistaTimeProperties.BeginInit();
            this.dtSt.Properties.BeginInit();
            this.dtEt.Properties.VistaTimeProperties.BeginInit();
            this.dtEt.Properties.BeginInit();
            this.cmbShortcuts.Properties.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始时间";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(8, 0x22);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "结束时间";
            this.dtSt.EditValue = new DateTime(0x7d5, 6, 0x17, 0, 0, 0, 0);
            this.dtSt.Location = new Point(0x43, 4);
            this.dtSt.Name = "dtSt";
            this.dtSt.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.dtSt.Properties.VistaTimeProperties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.dtSt.Size = new Size(0x5d, 0x15);
            this.dtSt.TabIndex = 2;
            this.dtEt.EditValue = new DateTime(0x7d5, 6, 0x17, 0, 0, 0, 0);
            this.dtEt.Location = new Point(0x43, 30);
            this.dtEt.Name = "dtEt";
            this.dtEt.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.dtEt.Properties.VistaTimeProperties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.dtEt.Size = new Size(0x5c, 0x15);
            this.dtEt.TabIndex = 3;
            this.cmbShortcuts.Location = new Point(0x43, 0x39);
            this.cmbShortcuts.Name = "cmbShortcuts";
            this.cmbShortcuts.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cmbShortcuts.Properties.Items.AddRange(new object[] { "今天", "昨天", "本月", "上一月", "今年", "上一年" });
            this.cmbShortcuts.Size = new Size(0x5d, 0x15);
            this.cmbShortcuts.TabIndex = 4;
            this.cmbShortcuts.SelectedIndexChanged += new EventHandler(this.cmbShortcuts_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "快捷选择";
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cmbShortcuts);
            base.Controls.Add(this.dtEt);
            base.Controls.Add(this.dtSt);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "DateEditEx";
            base.Size = new Size(0xa8, 0x54);
            this.dtSt.Properties.VistaTimeProperties.EndInit();
            this.dtSt.Properties.EndInit();
            this.dtEt.Properties.VistaTimeProperties.EndInit();
            this.dtEt.Properties.EndInit();
            this.cmbShortcuts.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public bool QueryPopup(object obj)
        {
            string str = (string) obj;
            if ((str != null) && (str.Length > 0))
            {
                string[] strArray = str.Split(new char[] { '~' });
                this.dtSt.EditValue = DateTime.Parse(strArray[0]);
                this.dtEt.EditValue = DateTime.Parse(strArray[1]);
            }
            else
            {
                this.dtSt.EditValue = null;
                this.dtEt.EditValue = null;
            }
            return true;
        }

        public object QueryValue()
        {
            return (this.StartDate.ToString("yyyy-MM-dd") + "~" + this.EndDate.ToString("yyyy-MM-dd"));
        }

        public DateTime EndDate
        {
            get
            {
                DateTime editValue = (DateTime) this.dtEt.EditValue;
                return editValue.Date;
            }
            set
            {
                this.dtEt.EditValue = value.Date;
            }
        }

        public DateTime StartDate
        {
            get
            {
                DateTime editValue = (DateTime) this.dtSt.EditValue;
                return editValue.Date;
            }
            set
            {
                this.dtSt.EditValue = value.Date;
            }
        }
    }
}

