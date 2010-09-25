namespace SkyMap.Net.Gui.Components
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(SmApplicationControl), "SmAppControl.bmp")]
    public class SmApplicationControl : Panel
    {
        private IntPtr appWin;
        private bool created = false;
        private string exeName = "";
        private const int GWL_STYLE = -16;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_FRAMECHANGED = 0x20;
        private const int SWP_NOACTIVATE = 0x10;
        private const int SWP_NOMOVE = 2;
        private const int SWP_NOOWNERZORDER = 0x200;
        private const int SWP_NOREDRAW = 8;
        private const int SWP_NOSIZE = 1;
        private const int SWP_NOZORDER = 4;
        private const int SWP_SHOWWINDOW = 0x40;
        private const int WM_CLOSE = 0x10;
        private const int WS_CHILD = 0x40000000;
        private const int WS_EX_MDICHILD = 0x40;
        private const int WS_VISIBLE = 0x10000000;

        [DllImport("user32.dll", SetLastError=true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint="GetWindowLongA", SetLastError=true)]
        private static extern long GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
        private static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);
        [DllImport("user32.dll", SetLastError=true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (this.appWin != IntPtr.Zero)
            {
                PostMessage(this.appWin, 0x10, 0L, 0L);
                Thread.Sleep(0x3e8);
                this.appWin = IntPtr.Zero;
            }
            base.OnHandleDestroyed(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.appWin != IntPtr.Zero)
            {
                MoveWindow(this.appWin, 0, 0, base.Width, base.Height, true);
            }
            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.Invalidate();
            base.OnSizeChanged(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!this.created)
            {
                this.created = true;
                this.appWin = IntPtr.Zero;
                Process process = null;
                try
                {
                    process = Process.Start(this.exeName);
                    process.WaitForInputIdle();
                    this.appWin = process.MainWindowHandle;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, exception.Message, "Error");
                }
                SetParent(this.appWin, base.Handle);
                SetWindowLong(this.appWin, -16, 0x10000000L);
                MoveWindow(this.appWin, 0, 0, base.Width, base.Height, true);
            }
            base.OnVisibleChanged(e);
        }

        [DllImport("user32.dll", EntryPoint="PostMessageA", SetLastError=true)]
        private static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);
        [DllImport("user32.dll", SetLastError=true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint="SetWindowLongA", SetLastError=true)]
        private static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
        [DllImport("user32.dll", SetLastError=true)]
        private static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Name of the executable to launch")]
        public string ExeName
        {
            get
            {
                return this.exeName;
            }
            set
            {
                this.exeName = value;
            }
        }
    }
}

