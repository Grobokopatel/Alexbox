using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Alexbox.Domain;
using Ninject;
using Alexbox.View;
using App = System.Windows.Forms.Application;
using static Alexbox.Application.TelegramBot.TelegramBot;

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
            var quiplash = new CustomGame(2, 8, "Quiplash");
            foreach (var gamePage in new List<IGamePage>())
            {
                quiplash.AddGamePage(gamePage);
            }
            //quiplash = new CustomGame(2, 8, "Quiplash").AddGamePage(page).Start();
            /*App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);*/
            var container = ConfigureContainer();
            App.Run(new Form1());
            while (true)
            {
            }
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            return container;
        }
    }
}