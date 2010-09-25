namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using System;
    using System.Drawing;

    public class ArrowLine : GraphUnit
    {
        private int mCaptureDistance;
        private Node mEndNode;
        private int mEndX;
        private int mEndY;
        private Color mHighLightColor;
        private Color mPenColor;
        private int mPenWidth;
        private Node mStartNode;
        private int mStartX;
        private int mStartY;

        public ArrowLine(Node StartNode, Node EndNode, Color PenColor, int PenWidth)
        {
            this.mCaptureDistance = 8;
            this.mHighLightColor = Color.Blue;
            base.mGraphType = DrawType.ArrowLine;
            this.mStartNode = StartNode;
            this.mEndNode = EndNode;
            this.mPenColor = PenColor;
            this.mPenWidth = PenWidth;
            this.ReCalcCoodinate();
        }

        public ArrowLine(int x1, int y1, int x2, int y2, Color PenColor, int PenWidth)
        {
            this.mCaptureDistance = 8;
            this.mHighLightColor = Color.Blue;
            base.mGraphType = DrawType.ArrowLine;
            this.mStartX = x1;
            this.mStartY = y1;
            this.mEndX = x2;
            this.mEndY = y2;
            this.mPenColor = PenColor;
            this.mPenWidth = PenWidth;
        }

        public override void Draw(Graphics g)
        {
            Pen pen;
            Brush blue;
            if ((this.mStartNode != null) && (this.mEndNode != null))
            {
                this.ReCalcCoodinate();
            }
            if (base.Selected)
            {
                pen = new Pen(this.mHighLightColor, (float) this.mPenWidth);
                blue = Brushes.Blue;
            }
            else
            {
                pen = new Pen(this.mPenColor, (float) this.mPenWidth);
                blue = Brushes.Black;
            }
            g.DrawLine(pen, this.mStartX, this.mStartY, this.mEndX, this.mEndY);
            this.DrawArraw(g, pen, blue);
        }

        private void DrawArraw(Graphics g, Pen pen, Brush brush)
        {
            double num;
            double num8;
            double num9;
            double num10;
            double num11;
            if (this.mStartX != this.mEndX)
            {
                double num12 = (this.mStartY - this.mEndY) / (this.mStartX - this.mEndX);
                num = Math.Atan(num12);
            }
            else
            {
                num = 1.5707963267948966;
            }
            double d = num + 0.43633231299858238;
            double num3 = num - 0.43633231299858238;
            double num4 = 12.0 * Math.Cos(d);
            double num5 = 12.0 * Math.Sin(d);
            double num6 = 12.0 * Math.Cos(num3);
            double num7 = 12.0 * Math.Sin(num3);
            if (this.mStartX < this.mEndX)
            {
                num8 = this.mEndX - num4;
                num9 = this.mEndY - num5;
                num10 = this.mEndX - num6;
                num11 = this.mEndY - num7;
            }
            else if (this.mStartX > this.mEndX)
            {
                num8 = this.mEndX + num4;
                num9 = this.mEndY + num5;
                num10 = this.mEndX + num6;
                num11 = this.mEndY + num7;
            }
            else if (this.mEndY < this.mStartY)
            {
                num8 = this.mEndX + num4;
                num9 = this.mEndY + num5;
                num10 = this.mEndX + num6;
                num11 = this.mEndY + num7;
            }
            else
            {
                num8 = this.mEndX - num4;
                num9 = this.mEndY - num5;
                num10 = this.mEndX - num6;
                num11 = this.mEndY - num7;
            }
            Point[] points = new Point[] { new Point((int) num8, (int) num9), new Point((int) num10, (int) num11), new Point(this.mEndX, this.mEndY) };
            g.FillPolygon(brush, points);
        }

        public double PointToLineDis(double xPoint, double yPoint, double x1Line, double y1Line, double x2Line, double y2Line)
        {
            double num;
            double num2;
            double num3;
            if ((y1Line == y2Line) && (x1Line == x2Line))
            {
                return -1.0;
            }
            if (y1Line == y2Line)
            {
                num = 0.0;
                num2 = -1.0;
                num3 = y1Line;
            }
            else if (x1Line == x2Line)
            {
                num = -1.0;
                num2 = 0.0;
                num3 = x1Line;
            }
            else
            {
                num = (y1Line - y2Line) / (x1Line - x2Line);
                num2 = -1.0;
                num3 = 0.0 - ((num * x1Line) + (num2 * y1Line));
            }
            return Math.Abs((double) ((((num * xPoint) + (num2 * yPoint)) + num3) / Math.Sqrt((num * num) + (num2 * num2))));
        }

        private double PointToPointDis(double x1, double y1, double x2, double y2)
        {
            double num = x1 - x2;
            double num2 = y1 - y2;
            return Math.Sqrt((num * num) + (num2 * num2));
        }

        public void ReCalcCoodinate()
        {
            int num = this.mStartNode.ImageList.ImageSize.Width / 2;
            int num2 = this.mStartNode.XPos - this.mEndNode.XPos;
            int num3 = this.mStartNode.YPos - this.mEndNode.YPos;
            double num4 = Math.Sqrt((double) ((num2 * num2) + (num3 * num3)));
            double num5 = Math.Sqrt(2.0) * num;
            this.mStartX = (this.mStartNode.XPos - ((int) ((num2 * num5) / num4))) + num;
            this.mStartY = (this.mStartNode.YPos - ((int) ((num3 * num5) / num4))) + num;
            this.mEndX = (this.mEndNode.XPos + ((int) ((num2 * num5) / num4))) + num;
            this.mEndY = (this.mEndNode.YPos + ((int) ((num3 * num5) / num4))) + num;
        }

        public override bool TestCapture(int x, int y)
        {
            if ((x < Math.Min(this.mStartX, this.mEndX)) && (Math.Abs((int) (Math.Min(this.mStartX, this.mEndX) - x)) > this.mCaptureDistance))
            {
                return false;
            }
            if ((x > Math.Max(this.mStartX, this.mEndX)) && (Math.Abs((int) (Math.Max(this.mStartX, this.mEndX) - x)) > this.mCaptureDistance))
            {
                return false;
            }
            if ((y < Math.Min(this.mStartY, this.mEndY)) && (Math.Abs((int) (Math.Min(this.mStartY, this.mEndY) - y)) > this.mCaptureDistance))
            {
                return false;
            }
            if ((y > Math.Max(this.mStartY, this.mEndY)) && (Math.Abs((int) (Math.Max(this.mStartY, this.mEndY) - y)) > this.mCaptureDistance))
            {
                return false;
            }
            return (this.PointToLineDis((double) x, (double) y, (double) this.mStartX, (double) this.mStartY, (double) this.mEndX, (double) this.mEndY) < this.mCaptureDistance);
        }

        public int CaptureDistance
        {
            get
            {
                return this.mCaptureDistance;
            }
            set
            {
                this.mCaptureDistance = value;
            }
        }

        public Node EndNode
        {
            get
            {
                return this.mEndNode;
            }
            set
            {
                this.mEndNode = value;
            }
        }

        public Color HighLightColor
        {
            get
            {
                return this.mHighLightColor;
            }
            set
            {
                this.mHighLightColor = value;
            }
        }

        public Color PenColor
        {
            get
            {
                return this.mPenColor;
            }
            set
            {
                this.mPenColor = value;
            }
        }

        public int PenWidth
        {
            get
            {
                return this.mPenWidth;
            }
            set
            {
                this.mPenWidth = value;
            }
        }

        public Node StartNode
        {
            get
            {
                return this.mStartNode;
            }
            set
            {
                this.mStartNode = value;
            }
        }
    }
}

