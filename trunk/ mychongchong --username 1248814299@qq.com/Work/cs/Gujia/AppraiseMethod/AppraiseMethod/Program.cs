
using System.Windows.Forms;
using System;
using AppraiseMethod;
using System.Threading;
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
            //// Setup unhandled exception handlers
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);


            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(
             OnGuiUnhandedException);



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MethodForm("PG-2010-JskffSpf"));
        }

        //// CLR unhandled exception
        private static void OnUnhandledException(Object sender,
           UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException(e.ExceptionObject);
        }


        // Windows Forms unhandled exception
        private static void OnGuiUnhandedException(Object sender,
           ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }


        static void HandleUnhandledException(Object o)
        {
            Exception e = o as Exception;

            if (e != null)
            { // Report System.Exception info
                MessageBox.Show("Exception = " + e.GetType());
                MessageBox.Show("Message = " + e.Message);
                MessageBox.Show("FullText = " + e.ToString());
            }
            else
            { // Report exception Object info
                MessageBox.Show("Exception = " + o.GetType());
                MessageBox.Show("FullText = " + o.ToString());
            }

            MessageBox.Show("An unhandled exception occurred " +
               "and the application is shutting down.");
            Environment.Exit(1); // Shutting down
        }


    }
}
