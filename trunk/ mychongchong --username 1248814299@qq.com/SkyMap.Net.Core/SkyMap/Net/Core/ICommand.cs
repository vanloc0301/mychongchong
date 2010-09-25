namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ICommand
    {
        event EventHandler OwnerChanged;

        void Run();

        SkyMap.Net.Core.Codon Codon { get; set; }

        string ID { get; set; }

        object Owner { get; set; }
    }
}

