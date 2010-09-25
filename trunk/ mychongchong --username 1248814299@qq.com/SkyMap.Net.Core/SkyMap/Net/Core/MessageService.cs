namespace SkyMap.Net.Core
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public static class MessageService
    {
        private static ShowErrorDelegate customErrorReporter;
        private static string defaultMessageBoxTitle = "MessageBox";
        private static Form mainForm;
        private static string productName = "Application Name";

        public static bool AskQuestion(string question)
        {
            return AskQuestion(SkyMap.Net.Core.StringParser.Parse(question), SkyMap.Net.Core.StringParser.Parse("${res:Global.QuestionText}"));
        }

        public static bool AskQuestion(string question, string caption)
        {
            return (MessageBox.Show(MainForm, SkyMap.Net.Core.StringParser.Parse(question), SkyMap.Net.Core.StringParser.Parse(caption), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, GetOptions(question, caption)) == DialogResult.Yes);
        }

        public static bool AskQuestionFormatted(string formatstring, params string[] formatitems)
        {
            return AskQuestion(Format(formatstring, formatitems));
        }

        public static bool AskQuestionFormatted(string caption, string formatstring, params string[] formatitems)
        {
            return AskQuestion(Format(formatstring, formatitems), caption);
        }

        private static string Format(string formatstring, string[] formatitems)
        {
            try
            {
                return string.Format(SkyMap.Net.Core.StringParser.Parse(formatstring), (object[]) formatitems);
            }
            catch (FormatException)
            {
                StringBuilder builder = new StringBuilder(SkyMap.Net.Core.StringParser.Parse(formatstring));
                foreach (string str in formatitems)
                {
                    builder.Append("\nItem: ");
                    builder.Append(str);
                }
                return builder.ToString();
            }
        }

        private static MessageBoxOptions GetOptions(string text, string caption)
        {
            return (IsRtlText(text) ? (MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) : ((MessageBoxOptions) 0));
        }

        private static bool IsRtlText(string text)
        {
            if (RightToLeftConverter.IsRightToLeft)
            {
                foreach (char ch in SkyMap.Net.Core.StringParser.Parse(text))
                {
                    if (char.GetUnicodeCategory(ch) == UnicodeCategory.OtherLetter)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int ShowCustomDialog(string caption, string dialogText, params string[] buttontexts)
        {
            return ShowCustomDialog(caption, dialogText, -1, -1, buttontexts);
        }

        public static int ShowCustomDialog(string caption, string dialogText, int acceptButtonIndex, int cancelButtonIndex, params string[] buttontexts)
        {
            using (CustomDialog dialog = new CustomDialog(caption, dialogText, acceptButtonIndex, cancelButtonIndex, buttontexts))
            {
                dialog.ShowDialog(MainForm);
                return dialog.Result;
            }
        }

        public static void ShowError(Exception ex)
        {
            ShowError(ex, null);
        }

        public static void ShowError(string message)
        {
            ShowError(null, message);
        }

        public static void ShowError(Exception ex, string message)
        {
            if (ex != null)
            {
                LoggingService.Error(message, ex);
                if (customErrorReporter != null)
                {
                    customErrorReporter(ex, message);
                    return;
                }
            }
            else
            {
                LoggingService.Error(message);
            }
            string input = string.Empty;
            if (message != null)
            {
                input = input + message;
            }
            if ((message != null) && (ex != null))
            {
                input = input + "\n\n";
            }
            if (ex != null)
            {
                input = input + "Exception occurred: " + ex.ToString();
            }
            MessageBox.Show(MainForm, SkyMap.Net.Core.StringParser.Parse(input), SkyMap.Net.Core.StringParser.Parse("${res:Global.ErrorText}"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        public static void ShowErrorFormatted(string formatstring, params string[] formatitems)
        {
            ShowError(null, Format(formatstring, formatitems));
        }

        public static string ShowInputBox(string caption, string dialogText, string defaultValue)
        {
            using (InputBox box = new InputBox(dialogText, caption, defaultValue))
            {
                box.ShowDialog(MainForm);
                return box.Result;
            }
        }

        public static void ShowMessage(string message)
        {
            ShowMessage(message, DefaultMessageBoxTitle);
        }

        public static void ShowMessage(string message, string caption)
        {
            message = SkyMap.Net.Core.StringParser.Parse(message);
            LoggingService.Info(message);
            MessageBox.Show(mainForm, message, SkyMap.Net.Core.StringParser.Parse(caption), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, GetOptions(message, caption));
        }

        public static void ShowMessageFormatted(string formatstring, params string[] formatitems)
        {
            ShowMessage(Format(formatstring, formatitems));
        }

        public static void ShowMessageFormatted(string caption, string formatstring, params string[] formatitems)
        {
            ShowMessage(Format(formatstring, formatitems), caption);
        }

        public static void ShowWarning(string message)
        {
            message = SkyMap.Net.Core.StringParser.Parse(message);
            LoggingService.Warn(message);
            MessageBox.Show(MainForm, message, SkyMap.Net.Core.StringParser.Parse("${res:Global.WarningText}"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, GetOptions(message, message));
        }

        public static void ShowWarningFormatted(string formatstring, params string[] formatitems)
        {
            ShowWarning(Format(formatstring, formatitems));
        }

        public static ShowErrorDelegate CustomErrorReporter
        {
            get
            {
                return customErrorReporter;
            }
            set
            {
                customErrorReporter = value;
            }
        }

        public static string DefaultMessageBoxTitle
        {
            get
            {
                return defaultMessageBoxTitle;
            }
            set
            {
                defaultMessageBoxTitle = value;
            }
        }

        public static Form MainForm
        {
            get
            {
                return mainForm;
            }
            set
            {
                mainForm = value;
            }
        }

        public static string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                productName = value;
            }
        }

        public delegate void ShowErrorDelegate(Exception ex, string message);
    }
}

