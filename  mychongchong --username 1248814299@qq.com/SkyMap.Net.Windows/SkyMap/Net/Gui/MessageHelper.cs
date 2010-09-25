namespace SkyMap.Net.Gui
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public static class MessageHelper
    {
        public static string GetCaption(string defaultCaption)
        {
            string applicationTitle = PropertyService.ApplicationTitle;
            if (StringHelper.IsNull(applicationTitle))
            {
                applicationTitle = defaultCaption;
            }
            if (string.IsNullOrEmpty(applicationTitle))
            {
                applicationTitle = ResourceService.GetString("MainWindow.DialogName");
            }
            return applicationTitle;
        }

        public static Icon GetSystemIcon()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "app.ico");
            if (File.Exists(path))
            {
                using (Stream stream = File.OpenRead(path))
                {
                    return new Icon(stream);
                }
            }
            return ResourceService.GetIcon("Icons.SkyMapSoftIcon");
        }

        private static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return XtraMessageBox.Show(LookAndFeelHelper.DefaultUserLookAndFeel, text, caption, buttons, icon);
        }

        public static void ShowBaseExceptionInfo(CoreException e)
        {
            string text = "发生非致命性错误：";
            text = text + "\n" + e.Message;
            LoggingService.Warn(text + "\n");
            if (e.InnerException != null)
            {
                LoggingService.Warn(e.InnerException.Message + "\n" + e.StackTrace);
            }
            ShowInfo(text);
        }

        public static void ShowError(string text, Exception e)
        {
            string caption = GetCaption("错误");
            Show(text + ":\n" + e.Message + "\n" + e.StackTrace, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            LoggingService.Error("未预料到错误", e);
            if (e.InnerException != null)
            {
                LoggingService.Error("内部错误是：", e.InnerException);
            }
        }

        public static void ShowInfo(string text)
        {
            string caption = GetCaption("信息");
            Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowInfo(string text, string[] args)
        {
            ShowInfo(string.Format(text, (object[]) args));
        }

        public static void ShowInfo(string text, string arg0)
        {
            ShowInfo(string.Format(text, arg0));
        }

        public static DialogResult ShowOkCancelInfo(string text)
        {
            string caption = GetCaption("信息");
            return Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        }

        public static DialogResult ShowOkCancelInfo(string text, string[] args)
        {
            return ShowOkCancelInfo(string.Format(text, (object[]) args));
        }

        public static DialogResult ShowOkCancelInfo(string text, string arg0)
        {
            return ShowOkCancelInfo(string.Format(text, arg0));
        }

        public static DialogResult ShowYesNoCancelInfo(string text)
        {
            string caption = GetCaption("信息");
            return Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNoCancelInfo(string text, string[] args)
        {
            return ShowYesNoCancelInfo(string.Format(text, (object[]) args));
        }

        public static DialogResult ShowYesNoCancelInfo(string text, string arg0)
        {
            return ShowYesNoCancelInfo(string.Format(text, arg0));
        }

        public static DialogResult ShowYesNoInfo(string text)
        {
            string caption = GetCaption("信息");
            return Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNoInfo(string text, string arg0)
        {
            return ShowYesNoInfo(string.Format(text, arg0));
        }

        public static DialogResult ShowYesNoInfo(string text, string[] args)
        {
            return ShowYesNoInfo(string.Format(text, (object[]) args));
        }
    }
}

