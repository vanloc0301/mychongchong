namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class StatusBarService
    {
        private static string lastMessage = "";
        private static SmStatusBar statusBar = null;
        private static bool wasError = false;

        static StatusBarService()
        {
            statusBar = new SmStatusBar();
        }

        public static void RedrawStatusbar()
        {
            if (wasError)
            {
                ShowErrorMessage(lastMessage);
            }
            else
            {
                SetMessage(lastMessage);
            }
            Visible = PropertyService.Get<bool>("SkyMap.Net.Gui.StatusBarVisible", true);
        }

        public static void SetCaretPosition(int x, int y, int charOffset)
        {
            statusBar.CursorStatusBarPanel.Text = SkyMap.Net.Core.StringParser.Parse("${res:StatusBarService.CursorStatusBarPanelText}", new string[,] { { "Line", string.Format("{0,-10}", y + 1) }, { "Column", string.Format("{0,-5}", x + 1) }, { "Character", string.Format("{0,-5}", charOffset + 1) } });
        }

        public static void SetInsertMode(bool insertMode)
        {
            statusBar.ModeStatusBarPanel.Text = insertMode ? SkyMap.Net.Core.StringParser.Parse("${res:StatusBarService.CaretModes.Insert}") : SkyMap.Net.Core.StringParser.Parse("${res:StatusBarService.CaretModes.Overwrite}");
        }

        public static void SetLoginInfo(string dept, string name)
        {
            statusBar.SetLoginInfo(dept + ":" + name);
        }

        public static void SetMessage(string message)
        {
            lastMessage = message;
            statusBar.SetMessage(SkyMap.Net.Core.StringParser.Parse(message));
        }

        public static void SetMessage(Image image, string message)
        {
            statusBar.SetMessage(image, SkyMap.Net.Core.StringParser.Parse(message));
        }

        public static void SetOpMsg(string msg)
        {
            statusBar.SetOpMessage(msg);
        }

        public static void ShowErrorMessage(string message)
        {
            statusBar.ShowErrorMessage(SkyMap.Net.Core.StringParser.Parse(message));
        }

        public static void Update()
        {
        }

        public static bool CancelEnabled
        {
            get
            {
                return ((statusBar != null) && statusBar.CancelEnabled);
            }
            set
            {
                statusBar.CancelEnabled = value;
            }
        }

        public static System.Windows.Forms.Control Control
        {
            get
            {
                return statusBar;
            }
        }

        public static IProgressMonitor ProgressMonitor
        {
            get
            {
                return statusBar;
            }
        }

        public static bool Visible
        {
            get
            {
                return statusBar.Visible;
            }
            set
            {
                statusBar.Visible = value;
            }
        }
    }
}

