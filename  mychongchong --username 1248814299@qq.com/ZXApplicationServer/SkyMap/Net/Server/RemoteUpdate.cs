namespace SkyMap.Net.Server
{
    using SkyMap.Net.Core;
    using System;
    using System.IO;
    using System.Text;

    public class RemoteUpdate : MarshalByRefObject, IRemoteUpdate
    {
        public void Execute()
        {
            AutoUpdateHepler.TryUpdate();
        }

        public string GetRemoteLog()
        {
            string path = Path.Combine(FileUtility.ApplicationRootPath, @"bin\log.txt");
            if (File.Exists(path))
            {
                File.Copy(path, path + ".temp", true);
                using (StreamReader reader = new StreamReader(path + ".temp", Encoding.Default, true))
                {
                    return reader.ReadToEnd();
                }
            }
            return ("没有找到日志文件" + path);
        }
    }
}

