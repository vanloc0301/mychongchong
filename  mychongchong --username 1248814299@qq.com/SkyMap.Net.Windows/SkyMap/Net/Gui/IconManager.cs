namespace SkyMap.Net.Gui
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    internal class IconManager
    {
        private static Dictionary<string, int> iconIndecies = new Dictionary<string, int>();
        private static ImageList icons = new ImageList();

        static IconManager()
        {
            icons.ColorDepth = ColorDepth.Depth32Bit;
        }

        public static int GetIndexForFile(string file)
        {
            string str;
            if (Path.GetExtension(file).Equals(".ico", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(file).Equals(".exe", StringComparison.OrdinalIgnoreCase))
            {
                str = file;
            }
            else
            {
                str = Path.GetExtension(file).ToLower();
            }
            if (icons.Images.Count > 100)
            {
                icons.Images.Clear();
                iconIndecies.Clear();
            }
            if (iconIndecies.ContainsKey(str))
            {
                return iconIndecies[str];
            }
            icons.Images.Add(DriveObject.GetImageForFile(file));
            int num = icons.Images.Count - 1;
            iconIndecies.Add(str, num);
            return num;
        }

        public static ImageList List
        {
            get
            {
                return icons;
            }
        }
    }
}

