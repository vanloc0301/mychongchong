namespace SkyMap.Net.Gui
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    internal class FileIcon
    {
        public static Bitmap GetBitmap(string strPath, bool bSmall)
        {
            SHGFI shgfi;
            SHFILEINFO structure = new SHFILEINFO();
            int num = Marshal.SizeOf(structure);
            if (bSmall)
            {
                shgfi = SHGFI.Icon | SHGFI.UseFileAttributes | SHGFI.SmallIcon;
            }
            else
            {
                shgfi = SHGFI.Icon | SHGFI.UseFileAttributes;
            }
            SHGetFileInfo(strPath, 0x100, out structure, (uint) num, shgfi);
            return Bitmap.FromHicon(structure.hIcon);
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szTypeName;
        }

        private enum SHGFI
        {
            DisplayName = 0x200,
            Icon = 0x100,
            LargeIcon = 0,
            SmallIcon = 1,
            SysIconIndex = 0x4000,
            Typename = 0x400,
            UseFileAttributes = 0x10
        }
    }
}

