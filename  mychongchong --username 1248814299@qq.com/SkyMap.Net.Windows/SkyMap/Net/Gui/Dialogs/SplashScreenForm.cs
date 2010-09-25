namespace SkyMap.Net.Gui.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class SplashScreenForm : Form
    {
        private Bitmap bitmap;
        private static List<string> parameterList = new List<string>();
        private static List<string> requestedFileList = new List<string>();
        private static SplashScreenForm splashScreen;
        public const string VersionText = "Core 2.50.00";

        public SplashScreenForm()
        {
            Stream stream;
            base.FormBorderStyle = FormBorderStyle.None;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ShowInTaskbar = false;
            string s = "Core 2.50.00";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "SplashScreen.png");
            if (File.Exists(path))
            {
                using (stream = File.OpenRead(path))
                {
                    this.bitmap = new Bitmap(stream);
                }
            }
            else
            {
                using (stream = Assembly.GetEntryAssembly().GetManifestResourceStream("Resources.SplashScreen.png"))
                {
                    this.bitmap = new Bitmap(stream);
                }
            }
            base.ClientSize = this.bitmap.Size;
            using (Font font = new Font("Arial", 9f, FontStyle.Bold))
            {
                using (Graphics graphics = Graphics.FromImage(this.bitmap))
                {
                    graphics.DrawString(s, font, Brushes.Black, (float) 150f, (float) 290f);
                }
            }
            this.BackgroundImage = this.bitmap;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.bitmap != null))
            {
                this.bitmap.Dispose();
                this.bitmap = null;
            }
            base.Dispose(disposing);
        }

        public static string[] GetParameterList()
        {
            return GetStringArray(parameterList);
        }

        public static string[] GetRequestedFileList()
        {
            return GetStringArray(requestedFileList);
        }

        private static string[] GetStringArray(List<string> list)
        {
            return list.ToArray();
        }

        public static void SetCommandLineArgs(string[] args)
        {
            requestedFileList.Clear();
            parameterList.Clear();
            foreach (string str in args)
            {
                if (str.Length != 0)
                {
                    if ((str[0] == '-') || (str[0] == '/'))
                    {
                        int startIndex = 1;
                        if (((str.Length >= 2) && (str[0] == '-')) && (str[1] == '-'))
                        {
                            startIndex = 2;
                        }
                        parameterList.Add(str.Substring(startIndex));
                    }
                    else
                    {
                        requestedFileList.Add(str);
                    }
                }
            }
        }

        public static SplashScreenForm SplashScreen
        {
            get
            {
                if (splashScreen == null)
                {
                    splashScreen = new SplashScreenForm();
                }
                return splashScreen;
            }
        }
    }
}

