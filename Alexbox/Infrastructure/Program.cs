using System;
using System.Diagnostics;
using System.Threading;
using Ninject;
using App = System.Windows.Forms.Application;
using static Alexbox.TelegramBot.TelegramBot;

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
            var telegramBotThread = new Thread(Run);
            telegramBotThread.Start();
            //App.SetHighDpiMode(HighDpiMode.SystemAware);
            //App.EnableVisualStyles();
            //App.SetCompatibleTextRenderingDefault(false);
            //var container = ConfigureContainer();
            //App.Run(new StartForm(container.Get<Quiplash>()));
            while (true)
            {
            }
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