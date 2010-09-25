namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Config;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    public class EMail : AbstractBox
    {
        private IContainer components = null;
        private WebBrowser webbs;

        public EMail()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetNavigationUrl()
        {
            NameValueCollection section = (NameValueCollection) ConfigurationManager.GetSection("mailSettings");
            if (section == null)
            {
                throw new ConfigurationErrorsException("没有找到配置Mail配置节");
            }
            string str = section["WebMail"];
            if (str == null)
            {
                throw new WfClientException("没有正确配置WebMail服务器!");
            }
            CStaff staff = OGMService.GetStaff(SecurityUtil.GetSmIdentity().UserId);
            if (((staff != null) && (staff.EMail != null)) && (staff.EMailPassword != null))
            {
                str = str + string.Format("?username={0}&password={1}", staff.EMail.Substring(0, staff.EMail.IndexOf("@")), staff.EMailPassword);
            }
            return str;
        }

        public override void Init(CBoxConfig boxConfig)
        {
            base.CreateToolbar();
            this.RefreshData();
        }

        private void InitializeComponent()
        {
            this.webbs = new WebBrowser();
            base.SuspendLayout();
            this.webbs.Dock = DockStyle.Fill;
            this.webbs.Location = new Point(0, 0);
            this.webbs.MinimumSize = new Size(20, 20);
            this.webbs.Name = "webbs";
            this.webbs.Size = new Size(0x15a, 0x107);
            this.webbs.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.webbs);
            base.Name = "EMail";
            base.Size = new Size(0x15a, 0x107);
            base.ResumeLayout(false);
        }

        public override void RefreshData()
        {
            this.webbs.Navigate(this.GetNavigationUrl());
        }
    }
}

