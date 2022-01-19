using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Alexbox.Domain;
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
            var tasks = System.IO.File.ReadAllText("quiplash.txt").Split("\n").Select(line => new Task(line));
            var quiplash = new CustomGame(1, 3, "Quiplash")
                .WithTaskList(tasks.ToList())
                .AddStage(new Stage().WithParagraph("TEST").WaitForTimeOutOrReplies(1000))
                .AddStage(new Stage().WithParagraph("Ответьте на вопросы").WithSendingTasks(2, 1)
                    .WaitForTimeOutOrReplies(2000000))
                .AddStage(new Stage()
                    .WithScoreCounting((voteFor, allVotes, coefficient) => voteFor / allVotes * coefficient)
                    .WithRoundSubmits()
                    .WaitForTimeOutOrReplies(300000));
            new Thread(() => Run(quiplash)).Start();
            var form = new MainForm(quiplash);
            form.Start();
            App.Run(form);
        }
    }
}