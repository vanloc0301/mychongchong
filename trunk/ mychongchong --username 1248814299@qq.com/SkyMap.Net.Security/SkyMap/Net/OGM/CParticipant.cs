namespace SkyMap.Net.OGM
{
    using SkyMap.Net.DAO;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class CParticipant : CAbstractParticipant, ISaveAs
    {
        private AssignRuleType assignRule;
        private SkyMap.Net.OGM.ParticipantEntity participantEntity;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        [DisplayName("任务分配规则")]
        public AssignRuleType AssignRule
        {
            get
            {
                return this.assignRule;
            }
            set
            {
                this.assignRule = value;
            }
        }

        [DisplayName("实体"), Editor("SkyMap.Net.Tools.Organize.ParticipantEntityEditor,SkyMap.Net.Tools.Organize", typeof(UITypeEditor))]
        public SkyMap.Net.OGM.ParticipantEntity ParticipantEntity
        {
            get
            {
                return this.participantEntity;
            }
            set
            {
                this.participantEntity = value;
            }
        }
    }
}

