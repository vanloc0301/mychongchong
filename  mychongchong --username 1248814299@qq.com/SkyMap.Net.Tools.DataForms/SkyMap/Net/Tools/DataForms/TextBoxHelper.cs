namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class TextBoxHelper
    {
        private TextBoxHelper()
        {
        }

        public static void SetValidatorNullEvent(TextBox txt)
        {
            txt.CausesValidation = true;
            txt.Validated += new EventHandler(TextBoxHelper.ValidatorNull);
        }

        public static void SetValidatorNullEvent(TextBox[] txts)
        {
            foreach (TextBox box in txts)
            {
                SetValidatorNullEvent(box);
            }
        }

        private static void ValidatorNull(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = sender as TextBox;
                if (box.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowInfo("不能为空");
                    box.Undo();
                    box.Focus();
                }
            }
        }
    }
}

