namespace SkyMap.Net.Gui
{
    using System;
    using System.Windows.Forms;

    public class AutoHideStatusStripContainer : AutoHideContainer
    {
        public AutoHideStatusStripContainer(StatusStrip statusStrip) : base(statusStrip)
        {
            ToolStripItemEventHandler handler = null;
            statusStrip.AutoSize = false;
            statusStrip.MouseMove += new MouseEventHandler(this.StatusStripMouseMove);
            if (handler == null)
            {
                handler = delegate (object sender, ToolStripItemEventArgs e) {
                    e.Item.MouseMove += new MouseEventHandler(this.StatusStripMouseMove);
                };
            }
            statusStrip.ItemAdded += handler;
            foreach (ToolStripItem item in statusStrip.Items)
            {
                item.MouseMove += new MouseEventHandler(this.StatusStripMouseMove);
            }
        }

        private void StatusStripMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < (base.control.Height / 2))
            {
                base.ShowOverlay = false;
            }
        }
    }
}

