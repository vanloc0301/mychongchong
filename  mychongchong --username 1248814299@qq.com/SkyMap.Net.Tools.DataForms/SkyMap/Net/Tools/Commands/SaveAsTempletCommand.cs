namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Tools.DataForms;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class SaveAsTempletCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            TempletPrintNode owner = (TempletPrintNode) this.Owner;
            if (owner != null)
            {
                TempletPrint domainObject = owner.DomainObject;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = domainObject.TempletFile;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(dialog.FileName, domainObject.Data);
                }
                dialog.Dispose();
            }
        }
    }
}

