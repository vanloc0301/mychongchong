namespace SkyMap.Net.Core
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolBarComboBox : ToolStripComboBox, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private string description = string.Empty;
        private IComboBoxCommand menuCommand = null;

        public ToolBarComboBox(Codon codon, object caller)
        {
            this.RightToLeft = RightToLeft.Inherit;
            base.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            base.ComboBox.SelectedIndexChanged += new EventHandler(this.selectionChanged);
            base.ComboBox.KeyDown += new KeyEventHandler(this.ComboBoxKeyDown);
            this.caller = caller;
            this.codon = codon;
            if (codon.Properties.Contains("width"))
            {
                this.Size = new Size(Convert.ToInt32(codon.Properties["width"]), base.Height);
            }
            if (codon.Properties.Contains("dropdownwidth"))
            {
                base.DropDownWidth = Convert.ToInt32(codon.Properties["dropdownwidth"]);
            }
            if (codon.Properties.Contains("font"))
            {
                string str = codon.Properties["font"];
                FontConverter converter = new FontConverter();
                try
                {
                    this.Font = (Font) converter.ConvertFrom(str);
                }
                catch (Exception exception)
                {
                    LoggingService.Warn(exception);
                }
            }
            this.CreateCommand(codon);
            if (base.ComboBox.Items.Count > 0)
            {
                if (codon.Properties.Contains("selectindex"))
                {
                    try
                    {
                        base.ComboBox.SelectedIndex = Convert.ToInt32(codon.Properties["selectindex"]);
                    }
                    catch (Exception exception2)
                    {
                        LoggingService.Warn(exception2);
                    }
                }
                else
                {
                    base.ComboBox.SelectedIndex = 0;
                }
            }
            if (this.menuCommand == null)
            {
                throw new NullReferenceException("Can't create combobox menu command");
            }
            this.UpdateText();
            this.UpdateStatus();
        }

        private void ComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
            {
                this.MenuCommand.Run();
            }
        }

        private void CreateCommand(Codon codon)
        {
            this.menuCommand = (IComboBoxCommand) codon.AddIn.CreateObject(codon.Properties["class"]);
            this.menuCommand.ID = codon.Id;
            this.menuCommand.Owner = this;
            this.menuCommand.Codon = codon;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            this.MenuCommand.Run();
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                this.Enabled = failedAction != ConditionFailedAction.Disable;
            }
        }

        public virtual void UpdateText()
        {
            if (this.codon.Properties.Contains("label"))
            {
                this.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
            }
            if (this.codon.Properties.Contains("tooltip"))
            {
                base.ToolTipText = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["tooltip"]);
            }
        }

        public object Caller
        {
            get
            {
                return this.caller;
            }
        }

        public ICommand Command
        {
            get
            {
                return this.menuCommand;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
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
                bool flag = this.codon.GetFailedAction(this.caller) != ConditionFailedAction.Disable;
                if (this.menuCommand != null)
                {
                    flag &= this.menuCommand.IsEnabled;
                }
                return flag;
            }
        }

        public IComboBoxCommand MenuCommand
        {
            get
            {
                return this.menuCommand;
            }
        }
    }
}

