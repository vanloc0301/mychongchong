namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class Transition : AbstractWfElement
    {
        private IList<Condition> conditions;
        private Actdef from;
        private Actdef to;

        [DomainObjectList(CacheKey="ALL_Condition", type=typeof(Condition)), DisplayName("转移条件"), Editor("SkyMap.Net.Gui.Components.ListEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public IList<Condition> Conditions
        {
            get
            {
                return this.conditions;
            }
            set
            {
                this.conditions = value;
            }
        }

        [Browsable(false)]
        public Actdef From
        {
            get
            {
                return this.from;
            }
            set
            {
                this.from = value;
            }
        }

        [Browsable(false)]
        public Actdef To
        {
            get
            {
                return this.to;
            }
            set
            {
                this.to = value;
            }
        }
    }
}

