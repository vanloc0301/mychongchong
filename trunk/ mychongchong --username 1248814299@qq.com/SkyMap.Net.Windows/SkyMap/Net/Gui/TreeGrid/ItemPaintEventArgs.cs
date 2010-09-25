namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ItemPaintEventArgs : PaintEventArgs
    {
        private DynamicListColumn column;
        private Rectangle fillRectangle;
        private bool isMouseEntered;
        private DynamicListItem item;
        private DynamicList list;

        public ItemPaintEventArgs(Graphics graphics, Rectangle rectangle, Rectangle fillRectangle, DynamicList list, DynamicListColumn column, DynamicListItem item, bool isMouseEntered) : base(graphics, rectangle)
        {
            this.fillRectangle = fillRectangle;
            this.list = list;
            this.column = column;
            this.item = item;
            this.isMouseEntered = isMouseEntered;
        }

        public DynamicListColumn Column
        {
            get
            {
                return this.column;
            }
        }

        public Rectangle FillRectangle
        {
            get
            {
                return this.fillRectangle;
            }
        }

        public bool IsMouseEntered
        {
            get
            {
                return this.isMouseEntered;
            }
        }

        public DynamicListItem Item
        {
            get
            {
                return this.item;
            }
        }

        public DynamicList List
        {
            get
            {
                return this.list;
            }
        }

        public DynamicListRow Row
        {
            get
            {
                return this.item.Row;
            }
        }
    }
}

