using System;
using System.Collections.Generic;
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
            var quiplash = new CustomGame(1, 3, "Quiplash")
                .WithTaskList(new List<Task> {new("TASK1"), new("TASK2")})
                .AddStage(new Stage().WithParagraph("TEST").WaitForTimeOutOrReplies(5000))
                .AddStage(new Stage().WithParagraph("�������� �� �������").WithSendingTasks(2, 2)
                    .WaitForTimeOutOrReplies(200000))
                .AddStage(new Stage()
                    .WithScoreCounting((voteFor, allVotes, coefficient) => voteFor / allVotes * coefficient)
                    .WithRoundSubmits()
                    .WaitForTimeOutOrReplies(30000));
            new Thread(() => Run(quiplash)).Start();
            var form = new MainForm(quiplash);
            form.Start();
            App.Run(form);
        }
    }
}