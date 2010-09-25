namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Commands;
    using SkyMap.Net.Workflow.Client.Config;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Static : AbstractBox
    {
        private IContainer components = null;
        private IList<ProdefRow> myStaticProdefs;
        private OpenStaticFormCommand openStaticFormCommand;
        internal TreeView tvStaticList;

        public Static()
        {
            this.InitializeComponent();
        }

        private void DblNodeClick(object sender, EventArgs e)
        {
            if (this.openStaticFormCommand == null)
            {
                this.openStaticFormCommand = new OpenStaticFormCommand();
                this.openStaticFormCommand.Owner = this;
            }
            if (this.openStaticFormCommand != null)
            {
                this.openStaticFormCommand.Run();
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

        public override void Init(CBoxConfig boxConfig)
        {
            base.CreateToolbar();
            this.tvStaticList.LabelEdit = false;
            this.tvStaticList.DoubleClick += new EventHandler(this.DblNodeClick);
        }

        private void InitializeComponent()
        {
            this.tvStaticList = new TreeView();
            base.SuspendLayout();
            this.tvStaticList.Dock = DockStyle.Fill;
            this.tvStaticList.ImageIndex = -1;
            this.tvStaticList.Location = new Point(0, 0);
            this.tvStaticList.Name = "tvStaticList";
            this.tvStaticList.SelectedImageIndex = -1;
            this.tvStaticList.Size = new Size(0x170, 0x158);
            this.tvStaticList.TabIndex = 0;
            base.Controls.Add(this.tvStaticList);
            base.Name = "Static";
            base.Size = new Size(0x170, 0x158);
            base.Controls.SetChildIndex(this.tvStaticList, 0);
            base.ResumeLayout(false);
        }

        public override void RefreshData()
        {
            IList<ProdefRow> myProdefs = WorkflowService.GetMyProdefs("STA");
            if (!myProdefs.Equals(this.myStaticProdefs))
            {
                this.myStaticProdefs = myProdefs;
                this.tvStaticList.Nodes.Clear();
                if (this.myStaticProdefs.Count > 0)
                {
                    Dictionary<Package, TreeNode> dictionary = new Dictionary<Package, TreeNode>(5);
                    Package key = null;
                    TreeNode node = null;
                    Prodef prodef = null;
                    foreach (ProdefRow row in this.myStaticProdefs)
                    {
                        prodef = WorkflowService.Prodefs[row.Id];
                        key = prodef.Package;
                        if (!dictionary.ContainsKey(key))
                        {
                            node = this.tvStaticList.Nodes.Add(key.Name);
                            node.Expand();
                            dictionary.Add(key, node);
                        }
                        else
                        {
                            node = dictionary[key];
                        }
                        node.Nodes.Add(prodef.Name).Tag = prodef;
                    }
                }
                else
                {
                    this.tvStaticList.Nodes.Add(ResourceService.GetString("Workflow.Box.NoStaticPermission"));
                }
            }
        }
    }
}

