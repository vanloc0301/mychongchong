namespace SkyMap.Net.Gui
{
    using System;
    using System.Diagnostics;

    public static class WaitDialogHelper
    {
        private static int count = 0;
        private static WaitDialogForm dlg;
        private static Stopwatch stopWatch;

        public static void Close()
        {
            count--;
            if ((count <= 0) && (dlg != null))
            {
                dlg.Close();
                dlg = null;
                stopWatch.Stop();
                StatusBarService.SetMessage("就绪，共用时" + stopWatch.Elapsed.TotalSeconds.ToString("0.0000秒"));
            }
        }

        public static void SetText(string text)
        {
            if (dlg != null)
            {
                dlg.SetCaption(text);
            }
        }

        public static void Show()
        {
            if (dlg == null)
            {
                dlg = new WaitDialogForm("");
            }
            if (count == 0)
            {
                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                }
                StatusBarService.SetMessage("系统运行中，请稍等...");
            }
            count++;
        }
    }
}

