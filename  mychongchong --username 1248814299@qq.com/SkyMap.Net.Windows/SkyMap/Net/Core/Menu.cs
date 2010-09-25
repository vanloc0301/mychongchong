namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public class Menu : ToolStripMenuItem, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private bool isInitialized;
        private ArrayList subItems;

        public Menu(string text, params ToolStripItem[] subItems)
        {
            this.Text = SkyMap.Net.Core.StringParser.Parse(text);
            base.DropDownItems.AddRange(subItems);
        }

        public Menu(Codon codon, object caller, ArrayList subItems)
        {
            if (subItems == null)
            {
                subItems = new ArrayList();
            }
            this.codon = codon;
            this.caller = caller;
            this.subItems = subItems;
            this.RightToLeft = RightToLeft.Inherit;
            this.UpdateText();
        }

        private void CreateDropDownItems()
        {
            base.DropDownItems.Clear();
            foreach (object obj2 in this.subItems)
            {
                if (obj2 is ToolStripItem)
                {
                    base.DropDownItems.Add((ToolStripItem) obj2);
                    if (obj2 is IStatusUpdate)
                    {
                        ((IStatusUpdate) obj2).UpdateStatus();
                        ((IStatusUpdate) obj2).UpdateText();
                    }
                }
                else
                {
                    ISubmenuBuilder builder = (ISubmenuBuilder) obj2;
                    base.DropDownItems.AddRange(builder.BuildSubmenu(this.codon, this.caller));
                }
            }
        }

        protected override void OnDropDownShow(EventArgs e)
        {
            if (!((this.codon == null) || base.DropDown.Visible))
            {
                this.CreateDropDownItems();
            }
            base.OnDropDownShow(e);
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                if (!this.isInitialized && (failedAction != ConditionFailedAction.Exclude))
                {
                    this.isInitialized = true;
                    this.CreateDropDownItems();
                    if ((base.DropDownItems.Count == 0) && (this.subItems.Count > 0))
                    {
                        base.DropDownItems.Add(new ToolStripMenuItem());
                    }
                }
            }
        }

        public virtual void UpdateText()
        {
            if (this.codon != null)
            {
                this.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
            }
        }

        public ICommand Command
        {
            get
            {
                return null;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (this.codon == null)
                {
                    return base.Enabled;
                }
                return (this.codon.GetFailedAction(this.caller) != ConditionFailedAction.Disable);
            }
        }
    }
}

