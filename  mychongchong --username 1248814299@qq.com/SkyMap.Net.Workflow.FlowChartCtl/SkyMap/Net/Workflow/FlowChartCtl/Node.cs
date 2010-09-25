namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class Node : GraphUnit
    {
        private int mHeight;
        private int mImageIndex;
        private System.Windows.Forms.ImageList mImageList;
        private NodeType mNodType;
        private int mWidth;
        private int mXPos;
        private int mYPos;

        public Node(int x, int y, System.Windows.Forms.ImageList imgList, int imgIndex, NodeType NodType)
        {
            base.mGraphType = DrawType.Node;
            this.mXPos = x;
            this.mYPos = y;
            this.mImageList = imgList;
            this.mImageIndex = imgIndex;
            this.mWidth = this.mImageList.ImageSize.Width;
            this.mHeight = this.mImageList.ImageSize.Height;
            this.mNodType = NodType;
        }

        public override void Draw(Graphics g)
        {
            Font font = new Font("宋体", 9f);
            g.DrawImage(this.mImageList.Images[this.mImageIndex], this.mXPos, this.mYPos, this.mImageList.ImageSize.Width, this.mImageList.ImageSize.Height);
            if (base.Selected)
            {
                Pen pen = new Pen(Color.Blue, 1f);
                g.DrawLine(pen, this.mXPos, this.mYPos, this.mXPos + this.mImageList.ImageSize.Width, this.mYPos);
                g.DrawLine(pen, this.mXPos, this.mYPos, this.mXPos, this.mYPos + this.mImageList.ImageSize.Height);
                g.DrawLine(pen, this.mXPos, this.mYPos + this.mImageList.ImageSize.Height, this.mXPos + this.mImageList.ImageSize.Width, this.mYPos + this.mImageList.ImageSize.Height);
                g.DrawLine(pen, this.mXPos + this.mImageList.ImageSize.Width, this.mYPos, this.mXPos + this.mImageList.ImageSize.Width, this.mYPos + this.mImageList.ImageSize.Height);
                g.DrawString(base.Name, font, Brushes.Blue, (float) ((this.mXPos + this.mWidth) + 10), (float) (this.mYPos + 10));
            }
            else
            {
                g.DrawString(base.Name, font, Brushes.Black, (float) ((this.mXPos + this.mWidth) + 10), (float) (this.mYPos + 10));
            }
        }

        public void MoveTo(int x, int y)
        {
            this.mXPos = x;
            this.mYPos = y;
        }

        public override bool TestCapture(int x, int y)
        {
            return (((x >= this.mXPos) && (x <= (this.mXPos + this.mImageList.ImageSize.Width))) && ((y >= this.mYPos) && (y <= (this.mYPos + this.mImageList.ImageSize.Height))));
        }

        public int Height
        {
            get
            {
                return this.mHeight;
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.mImageIndex;
            }
            set
            {
                this.mImageIndex = value;
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.mImageList;
            }
            set
            {
                this.mImageList = value;
            }
        }

        public NodeType NodType
        {
            get
            {
                return this.mNodType;
            }
            set
            {
                this.mNodType = value;
            }
        }

        public int Width
        {
            get
            {
                return this.mWidth;
            }
        }

        public int XPos
        {
            get
            {
                return this.mXPos;
            }
            set
            {
                this.mXPos = value;
            }
        }

        public int YPos
        {
            get
            {
                return this.mYPos;
            }
            set
            {
                this.mYPos = value;
            }
        }
    }
}

