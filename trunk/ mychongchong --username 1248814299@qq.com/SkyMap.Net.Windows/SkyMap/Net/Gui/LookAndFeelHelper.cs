namespace SkyMap.Net.Gui
{
    using DevExpress.LookAndFeel;
    using System;

    public class LookAndFeelHelper
    {
        private static UserLookAndFeel userLookAndFeel = new UserLookAndFeel(null);

        static LookAndFeelHelper()
        {
            userLookAndFeel.UseDefaultLookAndFeel = false;
            userLookAndFeel.SetSkinStyle(SkinStyle);
        }

        public static UserLookAndFeel DefaultUserLookAndFeel
        {
            get
            {
                return userLookAndFeel;
            }
        }

        public static string SkinStyle
        {
            get
            {
                return "Caramel";
            }
        }
    }
}

