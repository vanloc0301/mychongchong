namespace RedSpider.db_class
{
    using System;
    using System.Windows.Forms;

    internal class CONN_ACCESS
    {
        public static readonly string ConnString = ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\SystemData\RedSpider.mdb;Persist Security Info=False;Jet OLEDB:Database Password=bigsea520;");
    }
}

