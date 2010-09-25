namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class ToolBarLabel : ToolStripMenuItem, IStatusUpdate
    {
        private object caller;
        private Codon codon;

        public ToolBarLabel(Codon codon, object caller)
        {
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            if ((this.Image == null) && codon.Properties.Contains("icon"))
            {
                this.Image = ResourceService.GetBitmap(codon.Properties["icon"]);
            }
            this.UpdateStatus();
            this.UpdateText();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        public virtual void UpdateStatus()
        {
        }

        public virtual void UpdateText()
        {
            if (this.codon != null)
            {
                this.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
                base.ToolTipText = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["tooltip"]);
            }
        }

        public ICommand Command
        {
            get
            {
                return null;
            }
        }
    }
}

