namespace SkyMap.Net.Gui
{
    using System;
    using System.Data;

    public interface IReportVistor
    {
        void Execute(byte[] reportData, DataSet ds);

        object Caller { set; }
    }
}

