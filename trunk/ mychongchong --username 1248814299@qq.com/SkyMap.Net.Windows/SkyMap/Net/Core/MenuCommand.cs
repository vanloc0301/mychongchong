namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class MenuCommand : ToolStripMenuItem, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private string description;
        public static Converter<string, ICommand> LinkCommandCreator;
        private ICommand menuCommand;

        public MenuCommand(string label)
        {
            this.menuCommand = null;
            this.description = "";
            this.RightToLeft = RightToLeft.Inherit;
            this.codon = null;
            this.caller = null;
            this.Text = SkyMap.Net.Core.StringParser.Parse(label);
        }

        public MenuCommand(Codon codon, object caller) : this(codon, caller, false)
        {
        }

        public MenuCommand(string label, EventHandler handler) : this(label)
        {
            base.Click += handler;
        }

        public MenuCommand(Codon codon, object caller, bool createCommand)
        {
            this.menuCommand = null;
            this.description = "";
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            if (createCommand)
            {
                this.CreateCommand();
            }
            this.UpdateText();
            if (codon.Properties.Contains("shortcut"))
            {
                base.ShortcutKeys = ParseShortcut(codon.Properties["shortcut"]);
            }
        }

        private void CreateCommand()
        {
            try
            {
                string str = this.codon.Properties["link"];
                if ((str != null) && (str.Length > 0))
                {
                    if (LinkCommandCreator == null)
                    {
                        throw new NotSupportedException("MenuCommand.LinkCommandCreator is not set, cannot create LinkCommands.");
                    }
                    this.menuCommand = LinkCommandCreator(this.codon.Properties["link"]);
                }
                else
                {
                    this.menuCommand = (ICommand) this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
                }
                if (this.menuCommand != null)
                {
                    this.menuCommand.Owner = this.caller;
                    this.menuCommand.ID = this.codon.Id;
                    this.menuCommand.Codon = this.codon;
                }
                this.UpdateStatus();
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception, "Can't create menu command : " + this.codon.Id);
            }
        }

        private bool GetVisible()
        {
            return ((this.codon == null) || (this.codon.GetFailedAction(this.caller, this.menuCommand) != ConditionFailedAction.Exclude));
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if ((this.codon != null) && (this.GetVisible() && this.Enabled))
            {
                ICommand command = this.Command;
                if (command != null)
                {
                    command.Run();
                }
            }
        }

        public static Keys ParseShortcut(string shortcutString)
        {
            Keys none = Keys.None;
            if (shortcutString.Length > 0)
            {
                try
                {
                    foreach (string str in shortcutString.Split(new char[] { '|' }))
                    {
                        none |= (Keys) Enum.Parse(typeof(Keys), str);
                    }
                }
                catch (Exception exception)
                {
                    MessageService.ShowError(exception);
                    return Keys.None;
                }
            }
            return none;
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                if ((this.Image == null) && this.codon.Properties.Contains("icon"))
                {
                    this.Image = ResourceService.GetBitmap(this.codon.Properties["icon"]);
                }
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                this.Enabled = failedAction != ConditionFailedAction.Disable;
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
                if (this.menuCommand == null)
                {
                    this.CreateCommand();
                }
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
                if ((this.menuCommand != null) && (this.menuCommand is IMenuCommand))
                {
                    flag &= ((IMenuCommand) this.menuCommand).IsEnabled;
                }
                return flag;
            }
        }
    }
}

