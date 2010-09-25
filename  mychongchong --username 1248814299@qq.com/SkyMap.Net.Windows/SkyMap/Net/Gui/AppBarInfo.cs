namespace SkyMap.Net.Gui
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class AppBarInfo
    {
        private const int ABE_BOTTOM = 3;
        private const int ABE_LEFT = 0;
        private const int ABE_RIGHT = 2;
        private const int ABE_TOP = 1;
        private const int ABM_ACTIVATE = 6;
        private const int ABM_GETAUTOHIDEBAR = 7;
        private const int ABM_GETSTATE = 4;
        private const int ABM_GETTASKBARPOS = 5;
        private const int ABM_NEW = 0;
        private const int ABM_QUERYPOS = 2;
        private const int ABM_REMOVE = 1;
        private const int ABM_SETAUTOHIDEBAR = 8;
        private const int ABM_SETPOS = 3;
        private APPBARDATA m_data;
        private const uint SPI_GETWORKAREA = 0x30;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public void GetPosition(string strClassName, string strWindowName)
        {
            this.m_data = new APPBARDATA();
            this.m_data.cbSize = (uint) Marshal.SizeOf(this.m_data.GetType());
            if (!(FindWindow(strClassName, strWindowName) != IntPtr.Zero))
            {
                throw new Exception("Failed to find an AppBar that matched the given criteria");
            }
            if (SHAppBarMessage(5, ref this.m_data) != 1)
            {
                throw new Exception("Failed to communicate with the given AppBar");
            }
        }

        public void GetSystemTaskBarPosition()
        {
            this.GetPosition("Shell_TrayWnd", null);
        }

        [DllImport("shell32.dll")]
        private static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA data);
        [DllImport("user32.dll")]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        public ScreenEdge Edge
        {
            get
            {
                return (ScreenEdge) this.m_data.uEdge;
            }
        }

        public Rectangle WorkArea
        {
            get
            {
                int num = 0;
                RECT structure = new RECT();
                IntPtr pvParam = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
                num = SystemParametersInfo(0x30, 0, pvParam, 0);
                structure = (RECT) Marshal.PtrToStructure(pvParam, structure.GetType());
                if (num == 1)
                {
                    Marshal.FreeHGlobal(pvParam);
                    return new Rectangle(structure.left, structure.top, structure.right - structure.left, structure.bottom - structure.top);
                }
                return new Rectangle(0, 0, 0, 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public AppBarInfo.RECT rc;
            public int lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public enum ScreenEdge
        {
            Bottom = 3,
            Left = 0,
            Right = 2,
            Top = 1,
            Undefined = -1
        }
    }
}

