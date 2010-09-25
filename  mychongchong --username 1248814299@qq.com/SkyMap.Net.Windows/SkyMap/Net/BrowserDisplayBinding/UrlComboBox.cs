namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public class UrlComboBox : AbstractComboBoxCommand
    {
        protected override void OnOwnerChanged(EventArgs e)
        {
            base.OnOwnerChanged(e);
            ToolBarComboBox owner = (ToolBarComboBox) this.Owner;
            ComboBox comboBox = owner.ComboBox;
            comboBox.Width *= 3;
            ((HtmlViewPane) owner.Caller).SetUrlComboBox(owner.ComboBox);
        }
    }
}

