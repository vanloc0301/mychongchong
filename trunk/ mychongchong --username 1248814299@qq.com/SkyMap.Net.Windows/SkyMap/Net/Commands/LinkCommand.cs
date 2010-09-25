namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class LinkCommand : AbstractMenuCommand
    {
        private string site;

        public LinkCommand(string site)
        {
            this.site = site;
        }

        public override void Run()
        {
            if (this.site.StartsWith("home://"))
            {
                string fileName = Path.Combine(FileUtility.ApplicationRootPath, this.site.Substring(7).Replace('/', Path.DirectorySeparatorChar));
                try
                {
                    Process.Start(fileName);
                }
                catch (Exception)
                {
                    MessageService.ShowError("Can't execute/view " + fileName + "\n Please check that the file exists and that you can open this file.");
                }
            }
            else
            {
                FileService.OpenFile(this.site);
            }
        }
    }
}

