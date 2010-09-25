namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Drawing;

    public class MeasureWidthEventArgs : EventArgs
    {
        private System.Drawing.Graphics graphics;
        private int itemWidth;

        public MeasureWidthEventArgs(System.Drawing.Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }
            this.graphics = graphics;
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return this.graphics;
            }
        }

        public int ItemWidth
        {
            get
            {
                return this.itemWidth;
            }
            set
            {
                this.itemWidth = value;
            }
        }
    }
}

