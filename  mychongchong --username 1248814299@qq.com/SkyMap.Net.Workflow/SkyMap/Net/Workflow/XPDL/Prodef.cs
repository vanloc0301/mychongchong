namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.Components;
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class Prodef : AbstractCommonProdef, ISaveAs
    {
        private IDictionary<string, Actdef> actdefs;
        private string author;
        private DateTime? creationDate;
        private double duration;
        private double limit;
        private SkyMap.Net.Workflow.XPDL.Package package;
        private string responsibleId;
        private string responsibleName;
        private bool status;
        private DateTime? validFrom;
        private DateTime? validTo;
        private string version;

        public Prodef()
        {
            this.responsibleId = string.Empty;
            this.responsibleName = string.Empty;
            this.author = string.Empty;
            this.creationDate = new DateTime?(DateTime.Now);
            this.validFrom = new DateTime?(DateTime.Now);
            this.validTo = new DateTime?(DateTime.Now.AddYears(1));
            this.version = "1.00";
        }

        public Prodef(SkyMap.Net.Workflow.XPDL.Package pkg)
        {
            this.responsibleId = string.Empty;
            this.responsibleName = string.Empty;
            this.author = string.Empty;
            this.creationDate = new DateTime?(DateTime.Now);
            this.validFrom = new DateTime?(DateTime.Now);
            this.validTo = new DateTime?(DateTime.Now.AddYears(1));
            this.version = "1.00";
            this.package = pkg;
            this.Name = pkg.Name;
            base.DaoDataFormId = pkg.DaoDataFormId;
            base.DaoDataFormName = pkg.DaoDataFormName;
            base.DaoDataSetId = pkg.DaoDataSetId;
            base.DaoDataFormName = pkg.DaoDataSetName;
            base.Style = pkg.Style;
            base.Type = pkg.Type;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (Actdef actdef in this.Actdefs.Values)
            {
                unitOfWork.RegisterNew(actdef);
                foreach (Transition transition in actdef.Tos)
                {
                    unitOfWork.RegisterNew(transition);
                    unitOfWork.RegisterNew<Condition>(transition.Conditions);
                }
                if (actdef.ActdefFormPermission != null)
                {
                    actdef.ActdefFormPermission.SaveAs(unitOfWork);
                }
            }
        }

        [Browsable(false)]
        public IDictionary<string, Actdef> Actdefs
        {
            get
            {
                return this.actdefs;
            }
            set
            {
                this.actdefs = value;
            }
        }

        [DisplayName("作者")]
        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }

        [DisplayName("创建日期")]
        public DateTime? CreationDate
        {
            get
            {
                return this.creationDate;
            }
            set
            {
                this.creationDate = value;
            }
        }

        [DisplayName("预计工作期限")]
        public double Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        [DisplayName("最长工作期限")]
        public double Limit
        {
            get
            {
                return this.limit;
            }
            set
            {
                this.limit = value;
            }
        }

        [DisplayName("标识字符串"), ReadOnly(true)]
        public override string Logo
        {
            get
            {
                if (this.package != null)
                {
                    return this.package.Logo;
                }
                return null;
            }
            set
            {
            }
        }

        [Browsable(false)]
        public SkyMap.Net.Workflow.XPDL.Package Package
        {
            get
            {
                return this.package;
            }
            set
            {
                this.package = value;
            }
        }

        [Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DomainObjectDropDown(type=typeof(CParticipant), CacheKey="ALL_CParticipant_DAO"), DisplayName("责任参与者")]
        public CParticipant Responsible
        {
            get
            {
                if (!string.IsNullOrEmpty(this.responsibleId))
                {
                    return QueryHelper.Get<CParticipant>("CParticipant_" + this.responsibleId, this.responsibleId);
                }
                return null;
            }
            set
            {
                this.responsibleId = value.Id;
                this.responsibleName = value.Name;
            }
        }

        [Browsable(false)]
        public string ResponsibleId
        {
            get
            {
                return this.responsibleId;
            }
            set
            {
                this.responsibleId = value;
            }
        }

        [Browsable(false)]
        public string ResponsibleName
        {
            get
            {
                return this.responsibleName;
            }
            set
            {
                this.responsibleName = value;
            }
        }

        [DisplayName("是否有效")]
        public bool Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        [DisplayName("起始日期")]
        public DateTime? ValidFrom
        {
            get
            {
                return this.validFrom;
            }
            set
            {
                this.validFrom = value;
            }
        }

        [DisplayName("结束日期")]
        public DateTime? ValidTo
        {
            get
            {
                return this.validTo;
            }
            set
            {
                this.validTo = value;
            }
        }

        [DisplayName("版本号")]
        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
    }
}

