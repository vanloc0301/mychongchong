namespace SkyMap.Net.Gui.Dialogs
{
    using DevExpress.LookAndFeel;
    using DevExpress.XtraEditors;
    using Microsoft.Data.ConnectionUI;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;

    public class DBExportDialog : SmForm
    {
        private BackgroundWorker backgroundWorker;
        private SimpleButton btCancel;
        private SimpleButton btChoose;
        private SimpleButton btOk;
        private Container components = null;
        private string connectionString;
        private DataProvider dbProvider;
        private Label label2;
        private TextBox txtSql;

        public DBExportDialog()
        {
            this.InitializeComponent();
            this.InitMe();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("异步操作完成");
            }
            if (e.Error != null)
            {
                LoggingService.Error("发生错误:", e.Error);
                if ((e.Error.InnerException != null) && (e.Error.InnerException is WebException))
                {
                    LoggingService.Error("内部错误是：", e.Error.InnerException);
                    MessageHelper.ShowInfo("网络连接有问题，有可能是：\r\n1.服务端没有运行；\r\n2.你所在的网络连接有问题!");
                }
                else
                {
                    MessageHelper.ShowError("异步载入有错误发生...", e.Error);
                }
            }
        }

        private void btChoose_Click(object sender, EventArgs e)
        {
            DataConnectionDialog dialog = new DataConnectionDialog();
            dialog.DataSources.Add(DataSource.SqlDataSource);
            dialog.DataSources.Add(DataSource.OracleDataSource);
            dialog.DataSources.Add(DataSource.OdbcDataSource);
            dialog.DataSources.Add(DataSource.AccessDataSource);
            dialog.DataSources.Add(DataSource.SqlFileDataSource);
            dialog.SelectedDataSource = DataSource.SqlDataSource;
            dialog.SelectedDataProvider = DataProvider.SqlDataProvider;
            if (DataConnectionDialog.Show(dialog) == DialogResult.OK)
            {
                this.connectionString = dialog.ConnectionString;
                this.dbProvider = dialog.SelectedDataProvider;
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(this.txtSql.Text))
            {
                message = "请输入SQL语句";
            }
            else if (string.IsNullOrEmpty(this.connectionString))
            {
                message = "请选择数据源";
            }
            if (string.IsNullOrEmpty(message))
            {
                try
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.CheckFileExists = false;
                    dialog.CheckPathExists = true;
                    dialog.Title = "导出到";
                    dialog.Filter = "xml 文件 (*.xml)|*.xml";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("数据源提供者是:{0}", new object[] { this.dbProvider.Name });
                        }
                        DbConnection connection = DbProviderFactories.GetFactory(this.dbProvider.Name).CreateConnection();
                        connection.ConnectionString = this.connectionString;
                        connection.Open();
                        DataTable table = null;
                        try
                        {
                            DbCommand command = connection.CreateCommand();
                            command.CommandText = this.txtSql.Text;
                            DbDataReader reader = command.ExecuteReader();
                            table = new DataTable("SMDataTable");
                            try
                            {
                                table.Load(reader);
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                        finally
                        {
                            connection.Close();
                        }
                        if (table != null)
                        {
                            new DataSet("SMDataSet").WriteXml(dialog.FileName);
                        }
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error("导出数据时发生错误!", exception);
                    message = exception.Message;
                }
                if (string.IsNullOrEmpty(message))
                {
                    message = "导出成功!";
                }
                else
                {
                    base.DialogResult = DialogResult.None;
                }
                MessageHelper.ShowInfo(message);
            }
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
            this.btOk = new SimpleButton();
            this.btCancel = new SimpleButton();
            this.label2 = new Label();
            this.backgroundWorker = new BackgroundWorker();
            this.txtSql = new TextBox();
            this.btChoose = new SimpleButton();
            base.SuspendLayout();
            this.btOk.Location = new Point(0x5d, 250);
            this.btOk.Name = "btOk";
            this.btOk.Size = new Size(0x4b, 0x17);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "确定";
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.Location = new Point(0xc5, 250);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x4b, 0x17);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "取消";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x18, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1d, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "SQL";
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.txtSql.Location = new Point(0x3b, 12);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = ScrollBars.Both;
            this.txtSql.Size = new Size(0x10f, 0xce);
            this.txtSql.TabIndex = 8;
            this.btChoose.DialogResult = DialogResult.Cancel;
            this.btChoose.Location = new Point(0xff, 0xe0);
            this.btChoose.Name = "btChoose";
            this.btChoose.Size = new Size(0x4b, 0x17);
            this.btChoose.TabIndex = 9;
            this.btChoose.Text = "数据源...";
            this.btChoose.Click += new EventHandler(this.btChoose_Click);
            base.AcceptButton = this.btOk;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0x156, 0x115);
            base.Controls.Add(this.btChoose);
            base.Controls.Add(this.txtSql);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btCancel);
            base.Controls.Add(this.btOk);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.LookAndFeel.Style = LookAndFeelStyle.Skin;
            base.LookAndFeel.UseDefaultLookAndFeel = false;
            base.LookAndFeel.UseWindowsXPTheme = false;
            base.MaximizeBox = false;
            base.Name = "DBExportDialog";
            this.Text = "导出...";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitMe()
        {
            base.StartPosition = FormStartPosition.CenterScreen;
            this.btOk.Image = ResourceService.GetBitmap("Global.OkButtonImage");
            this.btCancel.Image = ResourceService.GetBitmap("Global.CancelButtonImage");
            this.btOk.DialogResult = DialogResult.OK;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btOk.Enabled = false;
        }
    }
}

