using Hbm.Automation.Api.Weighing.Examples.GUIsimple.NETStandard;
using System;
using System.Windows.Forms;

namespace GUIsimple.NETStandard
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUIsimpleForm(args));
        }
    }
}
