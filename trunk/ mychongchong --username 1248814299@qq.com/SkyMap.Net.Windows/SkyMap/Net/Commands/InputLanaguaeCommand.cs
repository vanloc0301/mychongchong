namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public class InputLanaguaeCommand : AbstractComboBoxCommand
    {
        protected virtual InputLanguage GetInputLanguage(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                foreach (InputLanguage language in InputLanguage.InstalledInputLanguages)
                {
                    if (language.LayoutName == lang)
                    {
                        return language;
                    }
                }
            }
            if (InputLanguage.CurrentInputLanguage != null)
            {
                return InputLanguage.CurrentInputLanguage;
            }
            return InputLanguage.DefaultInputLanguage;
        }

        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            ToolBarComboBox owner = (ToolBarComboBox) this.Owner;
            owner.Items.Clear();
            string str = PropertyService.Get<string>("InputLanguage", string.Empty);
            foreach (InputLanguage language in InputLanguage.InstalledInputLanguages)
            {
                owner.Items.Add(language.LayoutName);
                if (language.LayoutName == str)
                {
                    owner.SelectedItem = language;
                    this.Run();
                }
            }
        }

        public override void Run()
        {
            string selectedItem = (string) (this.Owner as ToolBarComboBox).SelectedItem;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("选择的输入法是：{0}", new object[] { selectedItem });
            }
            if (!string.IsNullOrEmpty(selectedItem))
            {
                InputLanguage.CurrentInputLanguage = this.GetInputLanguage(selectedItem);
            }
        }
    }
}

