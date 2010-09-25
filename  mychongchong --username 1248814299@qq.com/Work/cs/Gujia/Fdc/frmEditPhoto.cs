using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data;
using System.Data.SqlClient;
namespace ZBPM
{
    /// <summary>
    /// Summary description for frmEditPhoto.
    /// </summary>
    public class frmEditPhoto : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// µØ¿éµÄid
        /// </summary>
        public string m_strDkid;
        public DataSet m_dstPhoto;
        private ControlNavigator m_nav;
        private SkyMap.Net.Gui.Components.SmGridControl gridtp;
        private SkyMap.Net.Gui.Components.SmCardView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colÍ¼Æ¬Ãû³Æ;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit repositoryItemImageEdit1;
        private Button m_btnSave;
        private DevExpress.XtraGrid.Columns.GridColumn Í¼Æ¬ICON;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmEditPhoto()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_nav = new DevExpress.XtraEditors.ControlNavigator();
            this.gridtp = new SkyMap.Net.Gui.Components.SmGridControl();
            this.gridView1 = new SkyMap.Net.Gui.Components.SmCardView();
            this.colÍ¼Æ¬Ãû³Æ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Í¼Æ¬ICON = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.repositoryItemImageEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.m_btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridtp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_nav
            // 
            this.m_nav.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_nav.Location = new System.Drawing.Point(12, 335);
            this.m_nav.Name = "m_nav";
            this.m_nav.NavigatableControl = this.gridtp;
            this.m_nav.Size = new System.Drawing.Size(224, 24);
            this.m_nav.TabIndex = 6;
            this.m_nav.Text = "controlNavigator2";
            // 
            // gridtp
            // 
            this.gridtp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridtp.EmbeddedNavigator.Name = "";
            this.gridtp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.gridtp.Location = new System.Drawing.Point(12, 12);
            this.gridtp.MainView = this.gridView1;
            this.gridtp.Name = "gridtp";
            this.gridtp.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageEdit1,
            this.repositoryItemPictureEdit1});
            this.gridtp.Size = new System.Drawing.Size(552, 317);
            this.gridtp.TabIndex = 5;
            this.gridtp.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FieldCaption.BackColor = System.Drawing.Color.GhostWhite;
            this.gridView1.Appearance.FieldCaption.Font = new System.Drawing.Font("ËÎÌå", 9F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.FieldCaption.ForeColor = System.Drawing.Color.BlueViolet;
            this.gridView1.Appearance.FieldCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridView1.Appearance.FieldValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridView1.CardCaptionFormat = "Í¼Æ¬Ãû³Æ {1}";
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colÍ¼Æ¬Ãû³Æ,
            this.Í¼Æ¬ICON});
            this.gridView1.FocusedCardTopFieldIndex = 0;
            this.gridView1.GridControl = this.gridtp;
            this.gridView1.MaximumCardColumns = 1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoFocusNewCard = true;
            this.gridView1.OptionsBehavior.AutoHorzWidth = true;
            this.gridView1.OptionsBehavior.FieldAutoHeight = true;
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // colÍ¼Æ¬Ãû³Æ
            // 
            this.colÍ¼Æ¬Ãû³Æ.Caption = "Í¼Æ¬Ãû³Æ";
            this.colÍ¼Æ¬Ãû³Æ.FieldName = "Í¼Æ¬Ãû³Æ";
            this.colÍ¼Æ¬Ãû³Æ.Name = "colÍ¼Æ¬Ãû³Æ";
            this.colÍ¼Æ¬Ãû³Æ.Visible = true;
            this.colÍ¼Æ¬Ãû³Æ.VisibleIndex = 0;
            // 
            // Í¼Æ¬ICON
            // 
            this.Í¼Æ¬ICON.Caption = "Í¼Æ¬ICON";
            this.Í¼Æ¬ICON.ColumnEdit = this.repositoryItemPictureEdit1;
            this.Í¼Æ¬ICON.FieldName = "Í¼Æ¬";
            this.Í¼Æ¬ICON.Name = "Í¼Æ¬ICON";
            this.Í¼Æ¬ICON.Visible = true;
            this.Í¼Æ¬ICON.VisibleIndex = 1;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            // 
            // repositoryItemImageEdit1
            // 
            this.repositoryItemImageEdit1.AutoHeight = false;
            this.repositoryItemImageEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageEdit1.Name = "repositoryItemImageEdit1";
            // 
            // m_btnSave
            // 
            this.m_btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_btnSave.Location = new System.Drawing.Point(264, 335);
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(75, 23);
            this.m_btnSave.TabIndex = 7;
            this.m_btnSave.Text = "±£´æ";
            this.m_btnSave.UseVisualStyleBackColor = true;
            this.m_btnSave.Click += new System.EventHandler(this.m_btnSave_Click);
            // 
            // frmEditPhoto
            // 
            this.ClientSize = new System.Drawing.Size(577, 398);
            this.Controls.Add(this.m_btnSave);
            this.Controls.Add(this.m_nav);
            this.Controls.Add(this.gridtp);
            this.MaximizeBox = false;
            this.Name = "frmEditPhoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "µØ¿éÍ¼Æ¬";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEditPhoto_FormClosing);
            this.Load += new System.EventHandler(this.frmEditPhoto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridtp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmEditPhoto_Load(object sender, EventArgs e)
        {
           
            m_dstPhoto = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("SkyMap.Net.DAO", new string[]{@"SELECT *
FROM YW_tdzbpm_td_tp where dk_id ="+m_strDkid}, new string[] { "" });
            if (m_dstPhoto != null && m_dstPhoto.Tables.Count != 0)
            {
                m_dstPhoto.Tables[0].ExtendedProperties.Add("selectsql", @"SELECT *
FROM YW_tdzbpm_td_tp where dk_id =" + m_strDkid);
                gridtp.DataMember = m_dstPhoto.Tables[0].TableName;
                gridtp.DataSource = m_dstPhoto;
            }
            m_btnSave.Enabled = false;
        }

        private void gridtp_DataSourceChanged(object sender, EventArgs e)
        {
            m_btnSave.Enabled = true;
        }

        private void m_btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            m_btnSave.Enabled = false;
        }
        private void SaveData()
        {
            if (m_dstPhoto.HasChanges() == true)
            {
                foreach (DataRow dr in m_dstPhoto.Tables[0].Rows)
                {
                    dr["dk_id"] = m_strDkid;
                }
                SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
                sqlDataEngine.SaveData(m_dstPhoto);
            }
        }
        private void frmEditPhoto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_dstPhoto.HasChanges())
            {
                if (MessageBox.Show("Êý¾ÝÒÑ¾­ÐÞ¸ÄÊÇ·ñ±£´æ£¿", "ÌáÊ¾£¡", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveData();
                }
                else
                {
                    this.Close();
                }
            }
 
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
           m_btnSave.Enabled = true;
       }
    }
}

