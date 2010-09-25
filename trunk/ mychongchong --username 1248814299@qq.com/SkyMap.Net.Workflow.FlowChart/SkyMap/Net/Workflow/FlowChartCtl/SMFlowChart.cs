namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Tools.DataForms;
    using SkyMap.Net.Tools.Organize;
    using SkyMap.Net.Workflow.XPDL;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SMFlowChart : UserControl
    {
        private IContainer components;
        private ImageList img16;
        private ImageList img32;
        private DrawAction mAction;
        private bool mAllowDrawLine;
        private DrawType mDrawType;
        private GraphDoc mGraphDoc;
        private int mImageIndex;
        private bool mMouseDown;
        private NodeType mNodeType;
        private ProDefDoc mProDefDoc;
        private int mStartNodeIndex;
        private int mStartNodeXPos;
        private int mStartNodeYPos;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel pDomainObject;
        private PictureBox pic;
        private Panel pPic;
        private PropertyGrid propertyGrid;
        private Splitter splitter1;
        private ToolBar toolBar1;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton10;
        private ToolBarButton toolBarButton11;
        private ToolBarButton toolBarButton12;
        private ToolBarButton toolBarButton13;
        private ToolBarButton toolBarButton14;
        private ToolBarButton toolBarButton15;
        private ToolBarButton toolBarButton2;
        private ToolBarButton toolBarButton3;
        private ToolBarButton toolBarButton4;
        private ToolBarButton toolBarButton5;
        private ToolBarButton toolBarButton6;
        private ToolBarButton toolBarButton7;
        private ToolBarButton toolBarButton8;
        private ToolBarButton toolBarButton9;
        private int x1;
        private int x2;
        private int y1;
        private int y2;

        public SMFlowChart()
        {
            this.InitializeComponent();
        }

        private void AddNewActdef(Node nod)
        {
            string newGuid = this.GetNewGuid();
            this.mProDefDoc.AddActdef(newGuid, nod.Name, (ActdefType) nod.ImageIndex, true, nod.XPos, nod.YPos);
            nod.RelationalID = newGuid;
        }

        private void AddNewTransition(ArrowLine aline, string actdefIDFrom, string actdefIDTo)
        {
            string newGuid = this.GetNewGuid();
            this.mProDefDoc.AddTransition(newGuid, aline.Name, actdefIDFrom, actdefIDTo);
            aline.RelationalID = newGuid;
        }

        public void CloseDoc()
        {
            this.mProDefDoc.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawReversibleLine(PictureBox picBox, int x1, int y1, int x2, int y2, Color color)
        {
            ControlPaint.DrawReversibleLine(picBox.PointToScreen(new Point(x1, y1)), picBox.PointToScreen(new Point(x2, y2)), picBox.BackColor);
        }

        private string GetNewGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        private void InitGraphDoc()
        {
            this.pic = new PictureBox();
            this.pPic.Controls.Add(this.pic);
            this.pic.BackColor = Color.White;
            this.pic.Location = new Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new Size(0x4b0, 0x4b0);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.Paint += new PaintEventHandler(this.pic_Paint);
            this.pic.MouseUp += new MouseEventHandler(this.pic_MouseUp);
            this.pic.MouseMove += new MouseEventHandler(this.pic_MouseMove);
            this.pic.MouseDown += new MouseEventHandler(this.pic_MouseDown);
            this.mGraphDoc = new GraphDoc(this.pic);
            this.mGraphDoc.UnitSelect += new UnitSelectEventHandler(this.mGraphDoc_UnitSelect);
            this.mGraphDoc.UnitAdd += new UnitAddEventHandler(this.mGraphDoc_UnitAdd);
            this.mGraphDoc.UnitRemove += new UnitRemoveEventHandler(this.mGraphDoc_UnitRemove);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SMFlowChart));
            this.panel1 = new Panel();
            this.toolBar1 = new ToolBar();
            this.toolBarButton11 = new ToolBarButton();
            this.toolBarButton12 = new ToolBarButton();
            this.toolBarButton14 = new ToolBarButton();
            this.toolBarButton13 = new ToolBarButton();
            this.toolBarButton15 = new ToolBarButton();
            this.toolBarButton1 = new ToolBarButton();
            this.toolBarButton2 = new ToolBarButton();
            this.toolBarButton3 = new ToolBarButton();
            this.toolBarButton4 = new ToolBarButton();
            this.toolBarButton5 = new ToolBarButton();
            this.toolBarButton6 = new ToolBarButton();
            this.toolBarButton7 = new ToolBarButton();
            this.toolBarButton10 = new ToolBarButton();
            this.toolBarButton8 = new ToolBarButton();
            this.toolBarButton9 = new ToolBarButton();
            this.img16 = new ImageList(this.components);
            this.panel2 = new Panel();
            this.panel3 = new Panel();
            this.pPic = new Panel();
            this.splitter1 = new Splitter();
            this.pDomainObject = new Panel();
            this.propertyGrid = new PropertyGrid();
            this.img32 = new ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pDomainObject.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.toolBar1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x247, 30);
            this.panel1.TabIndex = 0;
            this.toolBar1.Appearance = ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new ToolBarButton[] { this.toolBarButton11, this.toolBarButton12, this.toolBarButton14, this.toolBarButton13, this.toolBarButton15, this.toolBarButton1, this.toolBarButton2, this.toolBarButton3, this.toolBarButton4, this.toolBarButton5, this.toolBarButton6, this.toolBarButton7, this.toolBarButton10, this.toolBarButton8, this.toolBarButton9 });
            this.toolBar1.ButtonSize = new Size(20, 20);
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.img16;
            this.toolBar1.Location = new Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new Size(0x247, 0x1c);
            this.toolBar1.TabIndex = 10;
            this.toolBar1.ButtonClick += new ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            this.toolBarButton11.ImageIndex = 10;
            this.toolBarButton11.Name = "toolBarButton11";
            this.toolBarButton11.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton11.Tag = "select";
            this.toolBarButton12.ImageIndex = 11;
            this.toolBarButton12.Name = "toolBarButton12";
            this.toolBarButton12.Tag = "delete";
            this.toolBarButton14.Name = "toolBarButton14";
            this.toolBarButton14.Style = ToolBarButtonStyle.Separator;
            this.toolBarButton13.ImageIndex = 12;
            this.toolBarButton13.Name = "toolBarButton13";
            this.toolBarButton13.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton13.Tag = "arrowline";
            this.toolBarButton15.Name = "toolBarButton15";
            this.toolBarButton15.Style = ToolBarButtonStyle.Separator;
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton1.Tag = "0";
            this.toolBarButton2.ImageIndex = 1;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton2.Tag = "1";
            this.toolBarButton3.ImageIndex = 2;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton3.Tag = "2";
            this.toolBarButton4.ImageIndex = 3;
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton4.Tag = "3";
            this.toolBarButton5.ImageIndex = 4;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton5.Tag = "4";
            this.toolBarButton6.ImageIndex = 5;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton6.Tag = "5";
            this.toolBarButton7.ImageIndex = 6;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton7.Tag = "6";
            this.toolBarButton10.ImageIndex = 9;
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton10.Tag = "9";
            this.toolBarButton8.ImageIndex = 7;
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton8.Tag = "7";
            this.toolBarButton9.ImageIndex = 8;
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Style = ToolBarButtonStyle.ToggleButton;
            this.toolBarButton9.Tag = "8";
            this.img16.ImageStream = (ImageListStreamer) manager.GetObject("img16.ImageStream");
            this.img16.TransparentColor = Color.White;
            this.img16.Images.SetKeyName(0, "");
            this.img16.Images.SetKeyName(1, "");
            this.img16.Images.SetKeyName(2, "");
            this.img16.Images.SetKeyName(3, "");
            this.img16.Images.SetKeyName(4, "");
            this.img16.Images.SetKeyName(5, "");
            this.img16.Images.SetKeyName(6, "");
            this.img16.Images.SetKeyName(7, "");
            this.img16.Images.SetKeyName(8, "");
            this.img16.Images.SetKeyName(9, "");
            this.img16.Images.SetKeyName(10, "");
            this.img16.Images.SetKeyName(11, "");
            this.img16.Images.SetKeyName(12, "");
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.pDomainObject);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Padding(2);
            this.panel2.Size = new Size(0x247, 0x113);
            this.panel2.TabIndex = 1;
            this.panel3.AutoScroll = true;
            this.panel3.BorderStyle = BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.pPic);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x139, 0x10f);
            this.panel3.TabIndex = 7;
            this.pPic.Location = new Point(0, 0);
            this.pPic.Name = "pPic";
            this.pPic.Size = new Size(0x4b0, 0x960);
            this.pPic.TabIndex = 6;
            this.splitter1.Dock = DockStyle.Right;
            this.splitter1.Location = new Point(0x13b, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(4, 0x10f);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            this.pDomainObject.BorderStyle = BorderStyle.Fixed3D;
            this.pDomainObject.Controls.Add(this.propertyGrid);
            this.pDomainObject.Dock = DockStyle.Right;
            this.pDomainObject.Location = new Point(0x13f, 2);
            this.pDomainObject.Name = "pDomainObject";
            this.pDomainObject.Padding = new Padding(2);
            this.pDomainObject.Size = new Size(0x106, 0x10f);
            this.pDomainObject.TabIndex = 0;
            this.propertyGrid.Dock = DockStyle.Fill;
            this.propertyGrid.Location = new Point(2, 2);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new Size(0xfe, 0x107);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.HelpRequested += new HelpEventHandler(this.propertyGrid_HelpRequested);
            this.propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            this.img32.ImageStream = (ImageListStreamer) manager.GetObject("img32.ImageStream");
            this.img32.TransparentColor = Color.White;
            this.img32.Images.SetKeyName(0, "");
            this.img32.Images.SetKeyName(1, "");
            this.img32.Images.SetKeyName(2, "");
            this.img32.Images.SetKeyName(3, "");
            this.img32.Images.SetKeyName(4, "");
            this.img32.Images.SetKeyName(5, "");
            this.img32.Images.SetKeyName(6, "");
            this.img32.Images.SetKeyName(7, "");
            this.img32.Images.SetKeyName(8, "");
            this.img32.Images.SetKeyName(9, "");
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "SMFlowChart";
            base.Size = new Size(0x247, 0x131);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pDomainObject.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void LoadMe(Prodef domainObject, UnitOfWork currentUnitOfWork)
        {
            Prodef prodef = domainObject;
            if (prodef == null)
            {
                throw new ArgumentOutOfRangeException("domainObject");
            }
            if (this.mGraphDoc != null)
            {
                this.mGraphDoc.Reset();
            }
            else
            {
                this.InitGraphDoc();
            }
            if (this.mProDefDoc == null)
            {
                this.mProDefDoc = new ProDefDoc();
            }
            this.mProDefDoc.Load(prodef, currentUnitOfWork);
            this.mProDefDoc.BuildGraphDoc(this.mGraphDoc, this.img32);
            this.mAction = DrawAction.SingleSelect;
        }

        private void mGraphDoc_UnitAdd(object sender, EventArgs e)
        {
        }

        private void mGraphDoc_UnitRemove(object sender, EventArgs e)
        {
            if (this.mGraphDoc.SelectedUnit.GraphType == DrawType.Node)
            {
                this.mProDefDoc.DeleteActdef(this.mGraphDoc.SelectedUnit.RelationalID);
            }
            else if (this.mGraphDoc.SelectedUnit.GraphType == DrawType.ArrowLine)
            {
                this.mProDefDoc.DeleteTransition(this.mGraphDoc.SelectedUnit.RelationalID);
            }
        }

        private void mGraphDoc_UnitSelect(object sender, EventArgs e)
        {
            this.propertyGrid.SelectedObject = null;
            GraphUnit selectedUnit = this.mGraphDoc.SelectedUnit;
            if ((selectedUnit.GraphType == DrawType.Node) && (selectedUnit.RelationalID != null))
            {
                this.propertyGrid.SelectedObject = this.mProDefDoc.GetActdef(this.mGraphDoc.SelectedUnit.RelationalID);
            }
            else if ((selectedUnit.GraphType == DrawType.ArrowLine) && (selectedUnit.RelationalID != null))
            {
                this.propertyGrid.SelectedObject = this.mProDefDoc.GetTransition(this.mGraphDoc.SelectedUnit.RelationalID);
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            this.mMouseDown = true;
            if (this.mAction == DrawAction.Draw)
            {
                switch (this.mDrawType)
                {
                    case DrawType.Node:
                        this.mGraphDoc.AddNode(e.X, e.Y, this.img32, this.mImageIndex, this.mNodeType);
                        this.AddNewActdef((Node) this.mGraphDoc.SelectedUnit);
                        this.mGraphDoc_UnitSelect(null, EventArgs.Empty);
                        return;

                    case DrawType.ArrowLine:
                    {
                        this.mAllowDrawLine = false;
                        int num = this.mGraphDoc.TestCaptureNodeByPos(e.X, e.Y);
                        if (num != -1)
                        {
                            this.mAllowDrawLine = true;
                            this.mStartNodeIndex = num;
                        }
                        this.x1 = e.X;
                        this.y1 = e.Y;
                        this.x2 = this.x1;
                        this.y2 = this.y1;
                        return;
                    }
                }
            }
            else if (this.mAction == DrawAction.SingleSelect)
            {
                this.mGraphDoc.SetSelectedByPos(e.X, e.Y);
                this.mStartNodeIndex = this.mGraphDoc.TestCaptureNodeByPos(e.X, e.Y);
                if (this.mStartNodeIndex != -1)
                {
                    this.x1 = e.X;
                    this.y1 = e.Y;
                    this.mStartNodeXPos = this.mGraphDoc.Nodes[this.mStartNodeIndex].XPos;
                    this.mStartNodeYPos = this.mGraphDoc.Nodes[this.mStartNodeIndex].YPos;
                }
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mMouseDown)
            {
                switch (this.mAction)
                {
                    case DrawAction.Draw:
                        if (this.mDrawType == DrawType.ArrowLine)
                        {
                            if (this.mAllowDrawLine)
                            {
                                this.DrawReversibleLine(this.pic, this.x1, this.y1, this.x2, this.y2, this.pic.BackColor);
                                this.DrawReversibleLine(this.pic, this.x1, this.y1, e.X, e.Y, Color.Black);
                                this.x2 = e.X;
                                this.y2 = e.Y;
                            }
                            break;
                        }
                        break;

                    case DrawAction.SingleSelect:
                        if (this.mStartNodeIndex != -1)
                        {
                            Cursor.Current = Cursors.Hand;
                        }
                        break;
                }
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            this.mMouseDown = false;
            if (this.mAction == DrawAction.Draw)
            {
                switch (this.mDrawType)
                {
                    case DrawType.Node:
                        return;

                    case DrawType.ArrowLine:
                        if (this.mAllowDrawLine)
                        {
                            this.DrawReversibleLine(this.pic, this.x1, this.y1, this.x2, this.y2, this.pic.BackColor);
                            int num = this.mGraphDoc.TestCaptureNodeByPos(e.X, e.Y);
                            if ((num != -1) && (num != this.mStartNodeIndex))
                            {
                                Pen pen = new Pen(Color.Black, 2f);
                                this.mGraphDoc.AddArrowLine(this.mGraphDoc.Nodes[this.mStartNodeIndex], this.mGraphDoc.Nodes[num], pen.Color, (int) pen.Width);
                                this.mGraphDoc.SelectedUnit.Name = this.mGraphDoc.Nodes[this.mStartNodeIndex].Name + "-" + this.mGraphDoc.Nodes[num].Name;
                                this.AddNewTransition((ArrowLine) this.mGraphDoc.SelectedUnit, this.mGraphDoc.Nodes[this.mStartNodeIndex].RelationalID, this.mGraphDoc.Nodes[num].RelationalID);
                                this.mGraphDoc_UnitSelect(null, EventArgs.Empty);
                                this.mStartNodeIndex = -1;
                            }
                        }
                        return;
                }
            }
            else if ((this.mAction == DrawAction.SingleSelect) && (this.mStartNodeIndex != -1))
            {
                Node node = this.mGraphDoc.Nodes[this.mStartNodeIndex];
                node.MoveTo((this.mStartNodeXPos + e.X) - this.x1, (this.mStartNodeYPos + e.Y) - this.y1);
                Cursor.Current = Cursors.Default;
                this.mGraphDoc.Draw();
                Actdef selectedObject = this.propertyGrid.SelectedObject as Actdef;
                if ((selectedObject.XPos != node.XPos) || (selectedObject.YPos != node.YPos))
                {
                    selectedObject.XPos = node.XPos;
                    selectedObject.YPos = node.YPos;
                    this.mProDefDoc.CurrentUnitOfWork.RegisterDirty(selectedObject);
                }
            }
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            this.mGraphDoc.Draw(e.Graphics);
        }

        public void PostEditor()
        {
        }

        private void propertyGrid_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("propertyGrid_DoubleClick-'{0}'", new object[] { this.propertyGrid.SelectedGridItem.PropertyDescriptor.Name });
            }
            Actdef selectedObject = this.propertyGrid.SelectedObject as Actdef;
            string name = this.propertyGrid.SelectedGridItem.PropertyDescriptor.Name;
            if (name != null)
            {
                if (!(name == "SubflowProdefName"))
                {
                    if (name == "ParticipantName")
                    {
                        fSelectParticipant participant = new fSelectParticipant();
                        participant.ParticipantID = selectedObject.ParticipantId;
                        participant.ShowDialog(this);
                        if ((participant.ParticipantID != null) && (participant.ParticipantName != null))
                        {
                            selectedObject.ParticipantId = participant.ParticipantID;
                            selectedObject.ParticipantName = participant.ParticipantName;
                            this.mProDefDoc.CurrentUnitOfWork.RegisterDirty(selectedObject);
                        }
                    }
                    else if (name == "ApplicationName")
                    {
                        fSelectApplication application = new fSelectApplication();
                        application.ApplicationID = selectedObject.ApplicationId;
                        application.ShowDialog(this);
                        if ((application.ApplicationID != null) && (application.ApplicationName != null))
                        {
                            selectedObject.ApplicationId = application.ApplicationID;
                            selectedObject.ApplicationName = application.ApplicationName;
                            this.mProDefDoc.CurrentUnitOfWork.RegisterDirty(selectedObject);
                        }
                    }
                    else if (name == "ActdefFormPermission")
                    {
                        bool flag = false;
                        if (selectedObject.ActdefFormPermission == null)
                        {
                            selectedObject.ActdefFormPermission = new WfActdefFormPermission(selectedObject);
                            flag = true;
                        }
                        if (selectedObject.ActdefFormPermission != null)
                        {
                            DataFormPermissionSettingForm form = new DataFormPermissionSettingForm();
                            form.FormPermission = selectedObject.ActdefFormPermission;
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                if ((form.FormPermission == null) && !flag)
                                {
                                    this.mProDefDoc.CurrentUnitOfWork.RegisterRemoved(selectedObject.ActdefFormPermission);
                                }
                                selectedObject.ActdefFormPermission = form.FormPermission as WfActdefFormPermission;
                                if (flag && (selectedObject.ActdefFormPermission != null))
                                {
                                    this.mProDefDoc.CurrentUnitOfWork.RegisterNew(selectedObject.ActdefFormPermission);
                                }
                                else if (selectedObject.ActdefFormPermission != null)
                                {
                                    this.mProDefDoc.CurrentUnitOfWork.RegisterDirty(selectedObject.ActdefFormPermission);
                                }
                            }
                            else if (flag)
                            {
                                selectedObject.ActdefFormPermission = null;
                            }
                            form.Close();
                        }
                    }
                }
                else
                {
                    fSelectSubflow subflow = new fSelectSubflow();
                    subflow.SubFlowProdefID = selectedObject.SubflowProdefId;
                    subflow.ShowDialog(this);
                    if ((subflow.SubFlowProdefID != null) && (subflow.SubFlowProdefName != null))
                    {
                        selectedObject.SubflowProdefId = subflow.SubFlowProdefID;
                        selectedObject.SubflowProdefName = subflow.SubFlowProdefName;
                        this.mProDefDoc.CurrentUnitOfWork.RegisterDirty(selectedObject);
                    }
                }
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            LoggingService.Debug("数据有改变...");
            if ((this.propertyGrid.SelectedObject != null) && (this.propertyGrid.SelectedObject is DomainObject))
            {
                this.mProDefDoc.CurrentUnitOfWork.RegisterDirty((DomainObject) this.propertyGrid.SelectedObject);
                if (this.propertyGrid.SelectedObject is Actdef)
                {
                    this.RefreshNode();
                }
            }
        }

        private void RefreshNode()
        {
            Node selectedUnit = (Node) this.mGraphDoc.SelectedUnit;
            Actdef actdef = this.mProDefDoc.GetActdef(selectedUnit.RelationalID);
            if (actdef != null)
            {
                selectedUnit.Name = actdef.Name;
                selectedUnit.MoveTo(actdef.XPos, actdef.YPos);
                this.mGraphDoc.Draw();
            }
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            foreach (ToolBarButton button in this.toolBar1.Buttons)
            {
                if (!button.Equals(e.Button))
                {
                    button.Pushed = false;
                }
            }
            switch (e.Button.Tag.ToString())
            {
                case "select":
                    this.mAction = DrawAction.SingleSelect;
                    return;

                case "delete":
                    this.mGraphDoc.RemoveUnit(this.mGraphDoc.SelectedUnit);
                    return;

                case "arrowline":
                    this.mAction = DrawAction.Draw;
                    this.mDrawType = DrawType.ArrowLine;
                    return;
            }
            this.mImageIndex = int.Parse(e.Button.Tag.ToString());
            this.mAction = DrawAction.Draw;
            this.mDrawType = DrawType.Node;
            this.mNodeType = (NodeType) int.Parse(e.Button.Tag.ToString());
        }
    }
}

