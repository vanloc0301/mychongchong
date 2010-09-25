namespace SkyMap.Net.Gui
{
    using System;

    public sealed class SystemHintHelper
    {
        private SystemHintHelper()
        {
        }

        public static void Show(string caption)
        {
            try
            {
                SystemHintForm form = new SystemHintForm();
                form.HintText = caption;
                form.Animate();
            }
            catch
            {
            }
        }
    }
}

