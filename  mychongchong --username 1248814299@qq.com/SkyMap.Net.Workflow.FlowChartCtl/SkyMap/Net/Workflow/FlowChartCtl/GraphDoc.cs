namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GraphDoc
    {
        private SkyMap.Net.Workflow.FlowChartCtl.ArrowLines mALines = new SkyMap.Net.Workflow.FlowChartCtl.ArrowLines();
        private bool mBatchUpdate = false;
        private SkyMap.Net.Workflow.FlowChartCtl.Nodes mNodes = new SkyMap.Net.Workflow.FlowChartCtl.Nodes();
        private PictureBox mPic;
        private GraphUnit mSelectedUnit;

        public event UnitAddEventHandler UnitAdd;

        public event UnitRemoveEventHandler UnitRemove;

        public event UnitSelectEventHandler UnitSelect;

        public GraphDoc(PictureBox pic)
        {
            this.mPic = pic;
        }

        public void AddArrowLine(Node StartNode, Node EndNode, Color PenColor, int PenWidth)
        {
            if ((StartNode == null) || (EndNode == null))
            {
                throw new ApplicationException("连线的起始节点或终止节点不能为空");
            }
            ArrowLine o = new ArrowLine(StartNode, EndNode, PenColor, PenWidth);
            this.mALines.Add(o);
            this.SetUnitSelect(o.ID);
            this.Draw();
            this.OnUnitAdd(EventArgs.Empty);
        }

        public void AddArrowLine(int x1, int y1, int x2, int y2, Color PenColor, int PenWidth)
        {
            ArrowLine o = new ArrowLine(x1, y1, x2, y2, PenColor, PenWidth);
            this.mALines.Add(o);
            this.SetUnitSelect(o.ID);
            this.Draw();
            this.OnUnitAdd(EventArgs.Empty);
        }

        public void AddNode(int x, int y, ImageList ImgList, int ImgIndex, NodeType nType)
        {
            Node o = new Node(x, y, ImgList, ImgIndex, nType);
            this.mNodes.Add(o);
            this.SetUnitSelect(o.ID);
            this.Draw();
            this.OnUnitAdd(EventArgs.Empty);
        }

        public void BeginUpdate()
        {
            this.mBatchUpdate = true;
        }

        public void ClearAll()
        {
            this.mNodes.Clear();
            this.mALines.Clear();
            this.Draw();
        }

        public void Draw()
        {
            if (!this.mBatchUpdate)
            {
                int num;
                this.mPic.Invalidate(false);
                Graphics g = this.mPic.CreateGraphics();
                for (num = this.mNodes.Count - 1; num >= 0; num--)
                {
                    this.mNodes[num].Draw(g);
                }
                for (num = this.mALines.Count - 1; num >= 0; num--)
                {
                    this.mALines[num].Draw(g);
                }
            }
        }

        public void Draw(Graphics g)
        {
            int num;
            for (num = this.mNodes.Count - 1; num >= 0; num--)
            {
                this.mNodes[num].Draw(g);
            }
            for (num = this.mALines.Count - 1; num >= 0; num--)
            {
                this.mALines[num].Draw(g);
            }
        }

        public void EndUpdate()
        {
            this.mBatchUpdate = false;
            this.Draw();
        }

        public List<ArrowLine> GetRelationalLine(Node nod)
        {
            List<ArrowLine> list = new List<ArrowLine>();
            for (int i = this.mALines.Count - 1; i >= 0; i--)
            {
                if ((this.mALines[i].StartNode == nod) || (this.mALines[i].EndNode == nod))
                {
                    list.Add(this.mALines[i]);
                }
            }
            return list;
        }

        private void OnUnitAdd(EventArgs e)
        {
            if (this.UnitAdd != null)
            {
                this.UnitAdd(this, e);
            }
        }

        private void OnUnitRemove(EventArgs e)
        {
            if (this.UnitRemove != null)
            {
                this.UnitRemove(this, e);
            }
        }

        private void OnUnitSelect(EventArgs e)
        {
            if (this.UnitSelect != null)
            {
                this.UnitSelect(this, e);
            }
        }

        public void RemoveUnit(GraphUnit u)
        {
            if (u is Node)
            {
                this.OnUnitRemove(EventArgs.Empty);
                for (int i = this.mALines.Count - 1; i >= 0; i--)
                {
                    if ((this.mALines[i].StartNode == u) || (this.mALines[i].EndNode == u))
                    {
                        this.mALines.Remove(this.mALines[i]);
                    }
                }
                this.mNodes.Remove((Node) u);
                this.Draw();
            }
            else if (u is ArrowLine)
            {
                this.OnUnitRemove(EventArgs.Empty);
                this.mALines.Remove((ArrowLine) u);
                this.Draw();
            }
        }

        public void Reset()
        {
            this.mNodes.Reset();
            this.mALines.Reset();
            this.Draw();
        }

        public bool SetSelectedByPos(int x, int y)
        {
            int num;
            bool flag = false;
            if (!this.TestCaptureByPos(x, y))
            {
                return false;
            }
            flag = false;
            for (num = this.mNodes.Count - 1; num >= 0; num--)
            {
                this.mNodes[num].Selected = false;
                if (!flag && this.mNodes[num].TestCapture(x, y))
                {
                    this.mNodes[num].Selected = true;
                    flag = true;
                    this.mSelectedUnit = this.mNodes[num];
                }
            }
            for (num = this.mALines.Count - 1; num >= 0; num--)
            {
                this.mALines[num].Selected = false;
                if (!flag && this.mALines[num].TestCapture(x, y))
                {
                    this.mALines[num].Selected = true;
                    flag = true;
                    this.mSelectedUnit = this.mALines[num];
                }
            }
            this.Draw();
            if (flag)
            {
                this.OnUnitSelect(EventArgs.Empty);
            }
            return flag;
        }

        public void SetUnitSelect(string id)
        {
            int num;
            for (num = this.mNodes.Count - 1; num >= 0; num--)
            {
                if (this.mNodes[num].ID == id)
                {
                    this.SelectedUnit = this.mNodes[num];
                    return;
                }
            }
            for (num = this.mALines.Count - 1; num >= 0; num--)
            {
                if (this.mALines[num].ID == id)
                {
                    this.SelectedUnit = this.mALines[num];
                    break;
                }
            }
        }

        public int TestCaptureArrowLineByPos(int x, int y)
        {
            for (int i = 0; i <= (this.mALines.Count - 1); i++)
            {
                if (this.mALines[i].TestCapture(x, y))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool TestCaptureByPos(int x, int y)
        {
            return ((this.TestCaptureNodeByPos(x, y) != -1) || (this.TestCaptureArrowLineByPos(x, y) != -1));
        }

        public int TestCaptureNodeByPos(int x, int y)
        {
            for (int i = 0; i <= (this.mNodes.Count - 1); i++)
            {
                if (this.mNodes[i].TestCapture(x, y))
                {
                    return i;
                }
            }
            return -1;
        }

        public SkyMap.Net.Workflow.FlowChartCtl.ArrowLines ArrowLines
        {
            get
            {
                return this.mALines;
            }
            set
            {
                this.mALines = value;
            }
        }

        public SkyMap.Net.Workflow.FlowChartCtl.Nodes Nodes
        {
            get
            {
                return this.mNodes;
            }
            set
            {
                this.mNodes = value;
            }
        }

        public GraphUnit SelectedUnit
        {
            get
            {
                int num;
                this.mSelectedUnit = null;
                for (num = this.mNodes.Count - 1; num >= 0; num--)
                {
                    if (this.mNodes[num].Selected)
                    {
                        this.mSelectedUnit = this.mNodes[num];
                    }
                }
                for (num = this.mALines.Count - 1; num >= 0; num--)
                {
                    if (this.mALines[num].Selected)
                    {
                        this.mSelectedUnit = this.mALines[num];
                    }
                }
                return this.mSelectedUnit;
            }
            set
            {
                int num;
                this.mSelectedUnit = value;
                for (num = this.mNodes.Count - 1; num >= 0; num--)
                {
                    this.mNodes[num].Selected = this.mNodes[num] == this.mSelectedUnit;
                }
                for (num = this.mALines.Count - 1; num >= 0; num--)
                {
                    this.mALines[num].Selected = this.mALines[num] == this.mSelectedUnit;
                }
                this.Draw();
                this.OnUnitSelect(EventArgs.Empty);
            }
        }
    }
}

