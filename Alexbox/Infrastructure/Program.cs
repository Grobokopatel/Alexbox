using System;
using System.Collections.Generic;
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
            App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);
            var tasks = new List<Task> {new("Что вы сегодня ели?")};
            var distribution = new Distribution(2, 1, 2);
            var quiplash = new CustomGame(1, 1, "Quiplash")
                .AddStage(new VotingStage(new []{"KEK","KEK"}).WithParagraph(
                    "Что бы сказал моргенштерн при встрече с владом а4?").WaitForTimout(10000));
            var telegramBotThread = new Thread(() => Run(quiplash));
            telegramBotThread.Start();
            var form = new StartPanel();
            quiplash.Start(form.Panel);
            App.Run(form);
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            return container;
        }
    }
}