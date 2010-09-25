namespace SkyMap.Net.Workflow.FlowChartCtl
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Windows.Forms;

    public class ProDefDoc
    {
        private UnitOfWork currentUnitOfWork;
        private SkyMap.Net.Workflow.XPDL.Prodef mProdef;
        private HybridDictionary mWFTRANSITIONs = new HybridDictionary(2);

        public void AddActdef(string actdefID, string actdefName, ActdefType actdefType, bool actdefEnabled, int xPos, int yPos)
        {
            Actdef actdef = new Actdef();
            actdef.Id = actdefID;
            actdef.Name = actdefName;
            actdef.Type = actdefType;
            actdef.Status = actdefEnabled;
            actdef.Description = string.Empty;
            actdef.Prodef = this.Prodef;
            actdef.IsDefaultInit = actdefType == ActdefType.INITIAL;
            actdef.IsSubflowSync = false;
            actdef.PassNeedInteraction = actdefType == ActdefType.INTERACTION;
            actdef.MNMergeNum = 0;
            actdef.XPos = xPos;
            actdef.YPos = yPos;
            actdef.Froms = new List<Transition>();
            actdef.Tos = new List<Transition>();
            this.CurrentUnitOfWork.RegisterNew(actdef);
            this.Prodef.Actdefs.Add(actdefID, actdef);
        }

        public void AddTransition(string transitionID, string TransitionName, string actdefIDFrom, string actdefIDTo)
        {
            Transition item = new Transition();
            Actdef actdef = this.GetActdef(actdefIDFrom);
            Actdef actdef2 = this.GetActdef(actdefIDTo);
            item.Id = transitionID;
            item.Name = TransitionName;
            item.From = actdef;
            item.To = actdef2;
            item.Description = "";
            actdef.Froms.Add(item);
            actdef2.Tos.Add(item);
            this.CurrentUnitOfWork.RegisterNew(item);
            this.mWFTRANSITIONs.Add(transitionID, item);
        }

        public void BuildGraphDoc(GraphDoc gDoc, ImageList imgList)
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
                gDoc.AddNode(xPos, yPos, imgList, (int) actdef.Type, this.GetNodeType(actdef.Type));
                gDoc.SelectedUnit.RelationalID = actdef.Id;
                gDoc.SelectedUnit.Name = actdef.Name;
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
        }

        public void DeleteActdef(string id)
        {
            if (this.Prodef.Actdefs.ContainsKey(id))
            {
                Actdef actdef = this.Prodef.Actdefs[id];
                List<Transition> list = new List<Transition>(actdef.Froms);
                list.AddRange(actdef.Tos);
                foreach (Transition transition in list)
                {
                    this.DeleteTransition(transition.Id);
                }
                this.CurrentUnitOfWork.RegisterRemoved(actdef);
                this.Prodef.Actdefs.Remove(id);
            }
        }

        public void DeleteTransition(string id)
        {
            if (this.mWFTRANSITIONs.Contains(id))
            {
                Transition item = this.mWFTRANSITIONs[id] as Transition;
                item.From.Froms.Remove(item);
                item.To.Tos.Remove(item);
                this.currentUnitOfWork.RegisterRemoved(item);
                this.mWFTRANSITIONs.Remove(id);
            }
        }

        public Actdef GetActdef(string actdefID)
        {
            if (!this.Prodef.Actdefs.ContainsKey(actdefID))
            {
                throw new NullReferenceException("Cannot find actdef of Id : " + actdefID);
            }
            return this.Prodef.Actdefs[actdefID];
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

        public Transition GetTransition(string transitionID)
        {
            if (!this.mWFTRANSITIONs.Contains(transitionID))
            {
                throw new NullReferenceException("Cannot find the transition of Id : " + transitionID);
            }
            return (this.mWFTRANSITIONs[transitionID] as Transition);
        }

        public void Load(SkyMap.Net.Workflow.XPDL.Prodef prodef, UnitOfWork currentUnitOfWork)
        {
            if (prodef == null)
            {
                throw new NullReferenceException("Prodef cannot be null");
            }
            this.Close();
            this.mProdef = prodef;
            this.currentUnitOfWork = currentUnitOfWork;
            foreach (Actdef actdef in prodef.Actdefs.Values)
            {
                IList<Transition> froms = actdef.Froms;
                if (froms.Count > 0)
                {
                    foreach (Transition transition in froms)
                    {
                        if (!this.mWFTRANSITIONs.Contains(transition.Id))
                        {
                            this.mWFTRANSITIONs.Add(transition.Id, transition);
                        }
                    }
                }
            }
        }

        public UnitOfWork CurrentUnitOfWork
        {
            get
            {
                if (this.currentUnitOfWork == null)
                {
                    throw new NullReferenceException("Current unit of work cannot be null");
                }
                return this.currentUnitOfWork;
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

