using System;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static void Main()
        {
            App.SetHighDpiMode(HighDpiMode.SystemAware);
            App.EnableVisualStyles();
            App.SetCompatibleTextRenderingDefault(false);
            var tasks = System.IO.File.ReadAllText("quiplash.txt").Split("\n").Select(line => new Task(line));
            var quiplash = new CustomGame(1, 8, "Quiplash")
                .WithTaskList(tasks.ToList())
                .AddStage(new Stage().WithParagraph("Раунд 1. Ответьте на вопросы").WithSendingTasks(2, 2)
                    .WaitForTimeOutOrReplies(90000))
                .AddStage(new Stage()
                    .WithScoreCounting((voteFor, allVotes, coefficient) => voteFor / allVotes * coefficient)
                    .WithRoundSubmits())
                .AddStage(new Stage().ShowPlayersScores().WaitForTimeOut(10000))
                .AddStage(new Stage().WithParagraph("Раунд 2. Ответьте на вопросы").WithSendingTasks(2, 2)
                    .WaitForTimeOutOrReplies(90000))
                .AddStage(new Stage()
                    .WithScoreCounting((voteFor, allVotes, coefficient) => voteFor / allVotes * coefficient * 2)
                    .WithRoundSubmits())
                .AddStage(new Stage().ShowPlayersScores().WaitForTimeOut(10000));
            new Thread(() => Run(quiplash)).Start();
            var form = new MainForm(quiplash);
            form.Start();
            App.Run(form);
        }
    }
}