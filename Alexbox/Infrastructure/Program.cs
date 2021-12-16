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

            var quiplash = new CustomGame(2, 8, "Quiplash")
    .AddGamePage(new TextPage("Правила бла бла бла"))
    .AddGamePage(new TextPage("ЗАДАНИЯ"));

            var container = ConfigureContainer();
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
    public sealed class CustomGame
    {
        public List<Player> _players = new();
        public List<Viewer> _viewers = new();
        public readonly GameStatus GameStatus;
        private readonly int _minPlayers;
        public readonly int _maxPlayers;
        private readonly string _name;
        private readonly Queue<Page> _pages;

        public CustomGame(int minPlayers, int maxPlayers, string name)
        {
            GameStatus = GameStatus.WaitingForPlayers;
            _minPlayers = minPlayers;
            _maxPlayers = maxPlayers;
            _name = name;
            _pages = new Queue<Page>();
        }

        public Player GetBestPlayer()
        {
            var bestPlayer = _players.First();
            foreach (var player in _players)
            {
                if (bestPlayer.Score < player.Score)
                {
                    bestPlayer = player;
                }
            }

            return bestPlayer;
        }

        public CustomGame AddGamePage(Page gamePage)
        {
            _pages.Enqueue(gamePage);
            return this;
        }

        public CustomGame AddGamePages(List<Page> gamePages)
        {
            foreach (var gamePage in gamePages)
            {
                _pages.Enqueue(gamePage);
            }
            return this;
        }

        private Panel panel;
        private Page currentPage;

        public void Start(Panel panel)
        {
            this.panel = panel;
            var controls = panel.Controls;
            currentPage = _pages.Dequeue();
            controls.Add(currentPage);
            //await Task.Run(() => currentPage.Timer(6000));
            currentPage.Ended += ChangePage;
        }

        private void ChangePage(TerminationType type)
        {
            panel.Controls.Remove(currentPage);
            currentPage.Ended -= ChangePage;
            if (_pages.Count != 0)
            {
                currentPage = _pages.Dequeue();
                panel.Controls.Add(currentPage);
                //new Thread(() => currentPage.Timer(6000)).Start();
                currentPage.Ended += ChangePage;
            }
        }
    }
    public enum TerminationType
    {
        Timeout,
        EveryoneReplied
    }

    public abstract class Page : UserControl
    {
        //public Page WaitPlayerRepliesOrTimout(60000)

        //public Page WithBackground(Image image);
        public Page WithParagraph(string text)
        {
            paragraph.Text = text;
            return this;
        }

        public Action<TerminationType> Ended;
        protected readonly TableLayoutPanel controlTable;
        private Label paragraph;

        public Page()
        {
            /*            this.components = new System.ComponentModel.Container();
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            */
            this.Text = "ТЕСТ";

            controlTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            paragraph = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            controlTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            controlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            controlTable.Controls.Add(paragraph, 0, 0);
            Dock = DockStyle.Fill;
            Controls.Add(controlTable);

            Load += (sender, e) =>
                       {
                           var timer = new System.Windows.Forms.Timer();
                           timer.Interval = 3000;
                           timer.Start();
                           timer.Tick += (sender, e) =>
                           { Ended(TerminationType.Timeout); timer.Stop(); };
                       };
        }
    }

    public class TextPage : Page
    {
        public TextPage(string text)
        {
            var label = new Label
            {
                Text = text,
                MaximumSize = new Size(Bounds.Width, 0),
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 16),
            };

            controlTable.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            controlTable.Controls.Add(label, 0, 1);
        }
    }

    public class VotingPage : Page
    {

    }
}