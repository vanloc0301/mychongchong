namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class SmBarUserControl : SmUserControl
    {
        protected ToolStrip[] ToolBars = null;

        public SmBarUserControl()
        {
            this.InitializeComponent();
        }

        public void BarStatusUpdate()
        {
            if (this.ToolBars != null)
            {
                this.UpdateToolbars();
            }
        }

        protected virtual ContextMenuStrip CreateContextMenu(string contextMenuPath)
        {
            return MenuService.CreateContextMenu(this, contextMenuPath);
        }

        protected virtual void CreateToolbar()
        {
            if ((this.ToolbarPath != null) && (this.ToolBars == null))
            {
                this.ToolBars = ToolbarService.CreateToolbars(this, this.ToolbarPath);
                if (this.ToolBars != null)
                {
                    for (int i = this.ToolBars.Length - 1; i > -1; i--)
                    {
                        base.Controls.Add(this.ToolBars[i]);
                    }
                }
            }
        }

        public IStatusUpdate GetToolStripItem(System.Type commandType)
        {
            foreach (ToolStrip strip in this.ToolBars)
            {
                foreach (ToolStripItem item in strip.Items)
                {
                    IStatusUpdate update = item as IStatusUpdate;
                    if (((update != null) && (update.Command != null)) && commandType.IsInstanceOfType(update.Command))
                    {
                        return update;
                    }
                }
            }
            return null;
        }

        public List<IStatusUpdate> GetToolStripItems(System.Type commandType)
        {
            List<IStatusUpdate> list = new List<IStatusUpdate>();
            foreach (ToolStrip strip in this.ToolBars)
            {
                foreach (ToolStripItem item in strip.Items)
                {
                    IStatusUpdate update = item as IStatusUpdate;
                    if (((update != null) && (update.Command != null)) && commandType.IsInstanceOfType(update.Command))
                    {
                        list.Add(update);
                    }
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.Name = "SmBarUserControl";
            base.Size = new Size(0x128, 0xdb);
            base.ResumeLayout(false);
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

        protected virtual string ToolbarPath
        {
            get
            {
                return null;
            }
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {
                List<ToolStripItem> list = new List<ToolStripItem>();
                foreach (ToolStrip strip in this.ToolBars)
                {
                    foreach (ToolStripItem item in strip.Items)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }
    }
}

