namespace SkyMap.Net.WebCamLibrary
{
    using SkyMap.Net.Core;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class AutoStartSMTrayCommand : AbstractCommand
    {
        private bool IsRun()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.ProcessName.ToLower() + ".exe") == "SMTray.exe".ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public override void Run()
        {
            if (!this.IsRun())
            {
                string path = FileUtility.Combine(new string[] { FileUtility.ApplicationRootPath, "bin", "SMTray.exe" });
                if (File.Exists(path))
                {
                    Process.Start(path);
                }
            }
        }
    }
}

