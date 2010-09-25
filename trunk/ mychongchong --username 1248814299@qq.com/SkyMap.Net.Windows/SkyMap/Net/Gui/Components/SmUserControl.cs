namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ComVisible(true)]
    public class SmUserControl : XtraUserControl
    {
        private Container components = null;

        public SmUserControl()
        {
            this.InitializeComponent();
            try
            {
                base.ImeMode = ImeMode.OnHalf;
            }
            catch
            {
            }
            this.DoubleBuffered = true;
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
            base.SuspendLayout();
            base.Name = "SmUserControl";
            base.ResumeLayout(false);
        }
    }
}

