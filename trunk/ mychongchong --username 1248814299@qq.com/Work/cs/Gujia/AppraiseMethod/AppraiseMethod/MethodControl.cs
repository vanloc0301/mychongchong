using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SkyMap.Net.Workflow.Client.Services;
using SkyMap.Net.Workflow.Client.Box;
using SkyMap.Net.Core;

namespace AppraiseMethod
{
    public partial class MethodControl : UserControl
    {
        private string strprojectid;

        public string StrProjectId
        {
            get
            {
                return this.strprojectid;
            }
        }

        public MethodControl()
        {
            InitializeComponent();
            InitEvent();
        }

        public void InitEvent()
        {
            this.che基准地价修正法.Click += new EventHandler(MethodClick);
            this.check成本法.Click += new EventHandler(MethodClick);
            this.check市场比较法.Click += new EventHandler(MethodClick);
            this.check收益还原法.Click += new EventHandler(MethodClick);
            this.check假设开发法.Click += new EventHandler(MethodClick);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MethodClick(null ,EventArgs.Empty);            
        }

        private void MethodClick(object obj,EventArgs args)
        {
            string project_id = string.Empty;
            if (BoxService.CurrentBox != null && BoxService.CurrentBox is IWfBox)
            {
                IWfBox wfbox = BoxService.CurrentBox as IWfBox;
                try
                {
                    System.Data.DataRow[] rows = wfbox.GetSelectedRows();
                    if (rows.Length == 1)
                    {
                        System.Data.DataRow row = rows[0];
                        if (row.Table.Columns.Contains("project_id"))
                        {
                            project_id = row["project_id"].ToString();
                            if (project_id.Contains("PG"))
                            {
                                groupControlYw.Text = "";
                                this.strprojectid = project_id;
                            }
                            else
                            {
                                groupControlYw.Text = string.Format("请选择评估业务-业务编号以PG开头-当前选择为{0}。", project_id);
                                this.strprojectid = "";
                            }
                            //if (string.IsNullOrEmpty(this.TaxTypeOID))
                            //{
                            //if (string.IsNullOrEmpty(this.ReportOID))
                            //{
                            //    string sql = string.Format(SQL, project_id);
                            //    if (LoggingService.IsDebugEnabled)
                            //        LoggingService.DebugFormatted("Will Execute SQL:\r\n{0}", sql);
                            //    dt = QueryHelper.ExecuteSql("SkyMap.Net.GTOA", string.Empty, sql);
                            //}
                            //else
                            //{

                            //}
                            //}                      
                        }
                    }
                    else
                    {
                        groupControlYw.Text = string.Format("一次只能选择一宗业务！当前选中{0}宗业务。",rows.Length.ToString());
                    }
                }
                catch (SkyMap.Net.Workflow.Client.NotSelectException) 
                {
                }

            }

        }

        private void btnStartAppraise_Click(object sender, EventArgs e)
        {
            this.groupControlYw.Text = "";
            MethodClick(null, EventArgs.Empty);
            if (groupControlYw.Text == "")
            {

                if (!string.IsNullOrEmpty(strprojectid))
                {
                    MethodForm mf = new MethodForm(StrProjectId);
                    mf.Show();
                }
                else
                {
                    groupControlYw.Text = "您未选中任何业务!";
                }
            }

        }



  
    }
}
