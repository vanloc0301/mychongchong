namespace SkyMap.Net.Workflow.FlowChart
{
    using SkyMap.Net.Commands;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.FlowChartCtl;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Windows.Forms;

    public class ActdefViewContent : AbstractEditViewContent
    {
        private SMFlowChart pkgs = new SMFlowChart();

        public ActdefViewContent()
        {
            this.TitleName = "流程环节定义";
        }

        public override void Dispose()
        {
            if (PropertyGridEditPanel.UnitOfWork != null)
            {
                PropertyGridEditPanel.UnitOfWork.Commit();
            }
        }

        public override void Load(string fileName)
        {
        }

        public override void Load(DomainObject domainObject, UnitOfWork unitOfWork)
        {
            this.pkgs.LoadMe((Prodef) domainObject, unitOfWork);
        }

        public override void RedrawContent()
        {
        }

        public override void Save()
        {
        }

        public override void Save(string fileName)
        {
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.pkgs;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}

