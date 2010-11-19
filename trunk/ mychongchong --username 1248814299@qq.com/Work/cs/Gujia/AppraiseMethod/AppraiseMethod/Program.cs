
using System.Windows.Forms;
using System;
using AppraiseMethod;
namespace Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MethodForm("PG-2010-JskffSpf"));
        }
    }
}
