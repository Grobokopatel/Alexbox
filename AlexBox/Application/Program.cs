using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alexbox.Domain;
using Alexbox.View;
using App = System.Windows.Forms.Application;

namespace Alexbox.Application
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);
            App.Run(new StartForm(new Quiplash()));
        }
    }
}
