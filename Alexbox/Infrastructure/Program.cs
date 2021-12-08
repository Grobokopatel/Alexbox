using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using Alexbox.Domain;
using Alexbox.View;
using Ninject;
using App = System.Windows.Forms.Application;

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
            var telegramBotThread = new Thread(TelegramBot.Run);
            telegramBotThread.Start();
            //App.SetHighDpiMode(HighDpiMode.SystemAware);
            //App.EnableVisualStyles();
            //App.SetCompatibleTextRenderingDefault(false);
            //var container = ConfigureContainer();
            //App.Run(new StartForm(container.Get<Quiplash>()));
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            //container.Bind<MessageSender>().To<TCPMessageSender>();
            //container.Bind<IFormatter>().To<BinaryFormatter>();
            return container;
        }
    }
}
