namespace SkyMap.Net.Gui.Pads
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class ToolsCodePage
    {
        public string RenderUrl(string section)
        {
            string str = string.Concat(new object[] { Application.StartupPath, Path.DirectorySeparatorChar, "..", Path.DirectorySeparatorChar, "data", Path.DirectorySeparatorChar, "resources", Path.DirectorySeparatorChar, "startpage" });
            return string.Concat(new object[] { str, Path.DirectorySeparatorChar, section, ".htm" });
        }
    }
}

