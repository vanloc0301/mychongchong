namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SmBarForm : SmForm
    {
        private ToolStrip[] ToolBars = null;
        protected ToolStripContainer toolStripContainer;
        private MenuStrip TopMenu = null;

        public SmBarForm()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            base.UpdateStyles();
        }

        public void BarStatusUpdate()
        {
            if (this.TopMenu != null)
            {
                this.UpdateMenus();
            }
            if (this.ToolBars != null)
            {
                this.UpdateToolbars();
            }
        }

        protected virtual ContextMenuStrip CreateContextMenu(string contextMenuPath)
        {
            return MenuService.CreateContextMenu(this, contextMenuPath);
        }

        protected virtual void CreateMainMenu()
        {
            if (this.MainMenuPath != null)
            {
                this.TopMenu = new MenuStrip();
                try
                {
                    ToolStripItem[] toolStripItems = (ToolStripItem[]) AddInTree.GetTreeNode(this.MainMenuPath).BuildChildItems(this).ToArray(typeof(ToolStripItem));
                    this.TopMenu.Items.AddRange(toolStripItems);
                    this.UpdateMenus();
                    this.toolStripContainer.TopToolStripPanel.Controls.Add(this.TopMenu);
                }
                catch (TreePathNotFoundException)
                {
                }
            }
        }

        protected virtual void CreateToolbar()
        {
            if ((this.ToolbarPath != null) && (this.ToolBars == null))
            {
                this.ToolBars = ToolbarService.CreateToolbars(this, this.ToolbarPath);
                if (this.ToolBars != null)
                {
                    this.toolStripContainer.TopToolStripPanel.Controls.AddRange(this.ToolBars);
                }
            }
        }

        private void InitializeComponent()
        {
            this.toolStripContainer = new ToolStripContainer();
            this.toolStripContainer.SuspendLayout();
            base.SuspendLayout();
            this.toolStripContainer.ContentPanel.Size = new Size(0x124, 0xf8);
            this.toolStripContainer.Dock = DockStyle.Fill;
            this.toolStripContainer.Location = new Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new Size(0x124, 0x111);
            this.toolStripContainer.TabIndex = 0;
            this.toolStripContainer.Text = "toolStripContainer";
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x124, 0x111);
            base.Controls.Add(this.toolStripContainer);
            base.Name = "SmBarForm";
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            base.ResumeLayout(false);
        }

        protected virtual void UpdateMenus()
        {
            foreach (object obj2 in this.TopMenu.Items)
            {
                if (obj2 is IStatusUpdate)
                {
                    ((IStatusUpdate) obj2).UpdateStatus();
                }
            }
        }

        protected virtual void UpdateToolbars()
        {
            if (this.ToolBars != null)
            {
                foreach (ToolStrip strip in this.ToolBars)
                {
                    ToolbarService.UpdateToolbar(strip);
                }
            }
        }

        protected virtual string MainMenuPath
        {
            get
            {
                return null;
            }
        }

        protected virtual string ToolbarPath
        {
            get
            {
                return null;
            }
        }
    }
}

