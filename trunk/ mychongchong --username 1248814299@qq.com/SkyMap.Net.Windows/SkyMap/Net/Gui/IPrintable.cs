namespace SkyMap.Net.Gui
{
    using System.Drawing.Printing;

    public interface IPrintable
    {
        System.Drawing.Printing.PrintDocument PrintDocument { get; }
    }
}

