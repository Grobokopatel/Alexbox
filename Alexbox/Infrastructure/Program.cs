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
                .AddStage(new Stage().WithScores().WaitForTimeout(30000))
                .WithTaskList(new List<Task> { new("TASK1"), new("TASK2") })
                .AddStage(new Stage().WithParagraph("Ответьте на вопросы").WithSendingTasks().WaitForTimeout(20000));
            Run(quiplash);
            var form = new MainForm(quiplash);
            form.Start();
            App.Run(form);
        }
    }
}