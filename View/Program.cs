using System;
using System.Windows.Forms;
using Alexbox.Domain;
using Alexbox.View;
using Alexbox.Infrastructure;
using App = System.Windows.Forms.Application;
using Ninject;

namespace Alexbox.Infrastructure
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);

            var container = Services.ConfigureContainer();
            App.Run(new StartForm(container.Get<Quiplash>()));
        }
    }
}
