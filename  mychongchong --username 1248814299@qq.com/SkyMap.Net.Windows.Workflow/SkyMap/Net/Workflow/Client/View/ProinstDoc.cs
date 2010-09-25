namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.FlowChartCtl;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class ProinstDoc
    {
        private SkyMap.Net.Workflow.XPDL.Prodef mProdef;
        private Proinst mProinst;
        private Dictionary<string, Transition> mWFTRANSITIONs = new Dictionary<string, Transition>(2);

        private void BuildGraphDoc(GraphDoc gDoc, ImageList imgList)
        {
            gDoc.ClearAll();
            gDoc.BeginUpdate();
            GraphUnit selectedUnit = null;
            int num = 0;
            foreach (Actdef actdef in this.Prodef.Actdefs.Values)
            {
                int xPos;
                int yPos;
                try
                {
                    xPos = actdef.XPos;
                    yPos = actdef.YPos;
                }
                catch
                {
                    xPos = 0;
                    yPos = num;
                }
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("添加结点:{0}...", new object[] { actdef.Name });
                }
                gDoc.AddNode(xPos, yPos, imgList, (int) actdef.Type, this.GetNodeType(actdef.Type));
                gDoc.SelectedUnit.RelationalID = actdef.Id;
                if (actdef.Type != ActdefType.INTERACTION)
                {
                    gDoc.SelectedUnit.Name = actdef.Name;
                }
                else
                {
                    gDoc.SelectedUnit.Name = this.GetUnitName(actdef);
                }
                num += 0x20;
                if (actdef.IsDefaultInit)
                {
                    selectedUnit = gDoc.SelectedUnit;
                }
            }
            foreach (Transition transition in this.mWFTRANSITIONs.Values)
            {
                Pen pen = new Pen(Color.Black, 2f);
                try
                {
                    gDoc.AddArrowLine(this.GetNodeByRelationalID(gDoc, transition.From.Id), this.GetNodeByRelationalID(gDoc, transition.To.Id), pen.Color, (int) pen.Width);
                    gDoc.SelectedUnit.RelationalID = transition.Id;
                    gDoc.SelectedUnit.Name = transition.Name;
                }
                catch
                {
                }
            }
            if (selectedUnit != null)
            {
                gDoc.SelectedUnit = selectedUnit;
            }
            gDoc.EndUpdate();
            selectedUnit = null;
        }

        public void Close()
        {
            this.mWFTRANSITIONs.Clear();
            this.mProdef = null;
            this.mProinst = null;
        }

        private Node GetNodeByRelationalID(GraphDoc gDoc, string relID)
        {
            for (int i = 0; i < gDoc.Nodes.Count; i++)
            {
                if (gDoc.Nodes[i].RelationalID == relID)
                {
                    return gDoc.Nodes[i];
                }
            }
            return null;
        }

        private NodeType GetNodeType(ActdefType t)
        {
            switch (t)
            {
                case ActdefType.INITIAL:
                    return NodeType.Start;

                case ActdefType.INTERACTION:
                case ActdefType.DUMMY:
                case ActdefType.SUBFLOW:
                    return NodeType.Normal;

                case ActdefType.COMPLETION:
                    return NodeType.End;

                case ActdefType.AND_BRANCH:
                case ActdefType.OR_BRANCH:
                    return NodeType.Split;

                case ActdefType.AND_MERGE:
                case ActdefType.OR_MERGE:
                case ActdefType.MN_MERGE:
                    return NodeType.Merge;
            }
            return NodeType.Unknown;
        }

        private string GetUnitName(Actdef actdef)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(actdef.Name);
            foreach (Actinst actinst in this.mProinst.Actinsts)
            {
                if (actinst.ActdefId == actdef.Id)
                {
                    foreach (WfAssigninst assigninst in actinst.Assigns)
                    {
                        builder.AppendFormat("\r\n经办人:{0};状态:{1}", string.IsNullOrEmpty(assigninst.StaffName) ? "没有指定经办人" : assigninst.StaffName, WfUtil.GetSMNAssignStatus(assigninst));
                    }
                    if (actdef.Limit > 0.0)
                    {
                        double num = WfUtil.GetCostTime(actinst) - actinst.DueTime;
                        if (num > 0.0)
                        {
                            builder.AppendFormat("\r\n超期{0}天", num);
                        }
                    }
                    break;
                }
            }
            return builder.ToString();
        }

        public void Load(SkyMap.Net.Workflow.XPDL.Prodef prodef, Proinst proinst, GraphDoc gDoc, ImageList imgList)
        {
            if (prodef == null)
            {
                throw new ArgumentNullException("Prodef cannot be null");
            }
            if (proinst == null)
            {
                throw new ArgumentNullException("Proinst cannot be null");
            }
            if ((this.mProdef == null) || (prodef.Id != this.mProdef.Id))
            {
                this.Close();
                this.mProdef = prodef;
                this.mProinst = proinst;
                foreach (Actdef actdef in prodef.Actdefs.Values)
                {
                    IList<Transition> froms = actdef.Froms;
                    if (froms.Count > 0)
                    {
                        foreach (Transition transition in froms)
                        {
                            if (!this.mWFTRANSITIONs.ContainsKey(transition.Id))
                            {
                                this.mWFTRANSITIONs.Add(transition.Id, transition);
                            }
                        }
                    }
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("开始画流程图...");
            }
            this.BuildGraphDoc(gDoc, imgList);
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("结束画流程图...");
            }
        }

        public SkyMap.Net.Workflow.XPDL.Prodef Prodef
        {
            get
            {
                if (this.mProdef == null)
                {
                    throw new NullReferenceException("Prodef cannot be null");
                }
                return this.mProdef;
            }
        }
    }
}

