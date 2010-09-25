namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;

    public class DriveObject
    {
        private string drive = null;
        private string text = null;

        public DriveObject(string drive)
        {
            this.drive = drive;
            this.text = drive.Substring(0, 2);
            switch (GetDriveType(drive))
            {
                case DriveType.Removeable:
                    this.text = this.text + " (${res:MainWindow.Windows.FileScout.DriveType.Removeable})";
                    break;

                case DriveType.Fixed:
                    this.text = this.text + " (${res:MainWindow.Windows.FileScout.DriveType.Fixed})";
                    break;

                case DriveType.Remote:
                    this.text = this.text + " (${res:MainWindow.Windows.FileScout.DriveType.Remote})";
                    break;

                case DriveType.Cdrom:
                    this.text = this.text + " (${res:MainWindow.Windows.FileScout.DriveType.CD})";
                    break;
            }
            this.text = SkyMap.Net.Core.StringParser.Parse(this.text);
        }

        public static DriveType GetDriveType(string driveName)
        {
            return NativeMethods.GetDriveType(driveName);
        }

        public static Image GetImageForFile(string fileName)
        {
            return IconService.GetBitmap(IconService.GetImageForFile(fileName));
        }

        public override string ToString()
        {
            return this.text;
        }

        public static string VolumeLabel(string volumePath)
        {
            try
            {
                StringBuilder volumeNameBuffer = new StringBuilder(0x80);
                int volumeSerNr = 0;
                NativeMethods.GetVolumeInformation(volumePath, volumeNameBuffer, 0x80, ref volumeSerNr, ref volumeSerNr, ref volumeSerNr, null, 0);
                return volumeNameBuffer.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string Drive
        {
            get
            {
                return this.drive;
            }
        }

        private class NativeMethods
        {
            [DllImport("kernel32.dll")]
            public static extern DriveType GetDriveType(string driveName);
            [DllImport("kernel32.dll", SetLastError=true)]
            public static extern int GetVolumeInformation(string volumePath, StringBuilder volumeNameBuffer, int volNameBuffSize, ref int volumeSerNr, ref int maxComponentLength, ref int fileSystemFlags, StringBuilder fileSystemNameBuffer, int fileSysBuffSize);
        }
    }
}

