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
            var distribution = new Distribution(2, 1, 2);
            var quiplash = new CustomGame(1, 8, "Quiplash").WithDistribution(distribution)
                .WithTaskList(new List<Task> {new("TASK1"), new("TASK2")})
                .AddStage(new Stage().WithParagraph("Wait for answers").WaitForTimeout(1000));
                //.AddStage(new Stage().WithSubmition());
            new Thread(() => Run(quiplash)).Start();
            var form = new MainForm(quiplash);
            form.Start();
            App.Run(form);
        }
    }
}