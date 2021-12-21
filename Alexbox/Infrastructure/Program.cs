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
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

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
            /*            var telegramBotThread = new Thread(Run);
                        telegramBotThread.Start();

                        /*foreach (var gamePage in new List<IGamePage>())
                        {
                            quiplash.AddGamePage(gamePage);
                        }*/
            //quiplash = new CustomGame(2, 8, "Quiplash").AddGamePage(page).Start();
            /*App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);*/
            App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);

            var quiplash = new CustomGame(3, 8, "Quiplash")
                .AddGamePage(new VotingPage(new[] { "Я съел кота", "Бебра понюхана", "Новый автомат"}).WithParagraph("Что бы сказал моргенштерн при встрече с владом а4?"))
                .AddGamePage(new TextPage("Правила бла бла бла").WithParagraph("Paragpah test"))
                .AddGamePage(new TextPage("ЗАДАНИЯ"));

            var form = new Form3();
            quiplash.Start(form.Panel);
            App.Run(form);
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            return container;
        }


    }

    /*    public class Program
        {
            public void Main()
            {
                var rules = "Делайте то-то, то-то, то-то...";
                var textPage = new TextPage(rules).WithBackground(image1);
                var waitingPage = new WaitingForPlayersSubmits(60, SubmitType.Text).WithDistribution(playersNumber, tasksPerPlayer, groupSize);
                var game = new CustomGame(3, 8, "Смехлыст").AddPage(textPage).AddPage(waitingPage).AddPage(...);
            }
        }
    */
}