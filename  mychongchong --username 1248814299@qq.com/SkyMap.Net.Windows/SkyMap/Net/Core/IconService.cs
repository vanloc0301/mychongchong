namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;

    public static class IconService
    {
        private static Dictionary<string, string> extensionHashtable = new Dictionary<string, string>();
        private static Dictionary<string, string> projectFileHashtable = new Dictionary<string, string>();
        private static readonly char[] separators = new char[] { Path.DirectorySeparatorChar, Path.VolumeSeparatorChar };

        static IconService()
        {
            Thread thread = new Thread(new ThreadStart(IconService.LoadThread));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Normal;
            thread.Start();
        }

        public static Bitmap GetBitmap(string name)
        {
            Bitmap bitmap = ResourceService.GetBitmap(name);
            if (bitmap != null)
            {
                return bitmap;
            }
            return ResourceService.GetBitmap("Icons.16x16.MiscFiles");
        }

        public static Bitmap GetGhostBitmap(string name)
        {
            float[][] newColorMatrix = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            newColorMatrix[0] = numArray2;
            numArray2 = new float[5];
            numArray2[1] = 1f;
            newColorMatrix[1] = numArray2;
            numArray2 = new float[5];
            numArray2[2] = 1f;
            newColorMatrix[2] = numArray2;
            numArray2 = new float[5];
            numArray2[3] = 0.5f;
            newColorMatrix[3] = numArray2;
            numArray2 = new float[5];
            numArray2[4] = 1f;
            newColorMatrix[4] = numArray2;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap image = GetBitmap(name);
            Bitmap bitmap2 = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bitmap2))
            {
                graphics.FillRectangle(SystemBrushes.Window, new Rectangle(0, 0, image.Width, image.Height));
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            }
            return bitmap2;
        }

        public static Icon GetIcon(string name)
        {
            Icon icon = ResourceService.GetIcon(name);
            if (icon != null)
            {
                return icon;
            }
            return ResourceService.GetIcon("Icons.16x16.MiscFiles");
        }

        public static string GetImageForFile(string fileName)
        {
            string key = Path.GetExtension(fileName).ToUpperInvariant();
            if (key.Length == 0)
            {
                key = ".TXT";
            }
            if (extensionHashtable.ContainsKey(key))
            {
                return extensionHashtable[key];
            }
            return "Icons.16x16.MiscFiles";
        }

        public static string GetImageForProjectType(string projectType)
        {
            if (projectFileHashtable.ContainsKey(projectType))
            {
                return projectFileHashtable[projectType];
            }
            return "Icons.16x16.SolutionIcon";
        }

        private static void InitializeIcons(AddInTreeNode treeNode)
        {
            foreach (IconDescriptor descriptor in (IconDescriptor[]) treeNode.BuildChildItems(null).ToArray(typeof(IconDescriptor)))
            {
                string str = (descriptor.Resource != null) ? descriptor.Resource : descriptor.Id;
                if (descriptor.Extensions != null)
                {
                    foreach (string str2 in descriptor.Extensions)
                    {
                        extensionHashtable[str2.ToUpperInvariant()] = str;
                    }
                }
                if (descriptor.Language != null)
                {
                    projectFileHashtable[descriptor.Language] = str;
                }
            }
        }

        private static void LoadThread()
        {
            try
            {
                InitializeIcons(AddInTree.GetTreeNode("/Workspace/Icons"));
            }
            catch (TreePathNotFoundException)
            {
            }
        }
    }
}

