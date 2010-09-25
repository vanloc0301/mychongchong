namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GridDialog : Form
    {
        public GridDialog(GridPanel gp)
        {
            try
            {
                base.Icon = ResourceService.GetIcon("Icons.SkyMapSoftIcon");
            }
            catch
            {
                try
                {
                    base.Icon = ResourceService.GetIcon("Dialog.Grid.Icon");
                }
                catch
                {
                }
            }
            gp.Dock = DockStyle.Fill;
            base.Size = new Size((3 * Screen.PrimaryScreen.WorkingArea.Width) / 4, (2 * Screen.PrimaryScreen.WorkingArea.Height) / 3);
            base.Controls.Add(gp);
        }
    }
}

