namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public interface ISubmenuBuilder
    {
        ToolStripItem[] BuildSubmenu(Codon codon, object owner);
    }
}

