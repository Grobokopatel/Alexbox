using System;
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
            var quiplash = new CustomGame(1, 1, "Quiplash")
                .AddStage(new VotingStage(new[] {"� ���� ����", "����� ��������", "����� �������"}).WithParagraph(
                    "��� �� ������ ����������� ��� ������� � ������ �4?"))
                .AddStage(new TextStage("������� ��� ��� ���").WithParagraph("Paragpah test"))
                .AddStage(new TextStage("�������"));
            var telegramBotThread = new Thread(() => Run(quiplash));
            telegramBotThread.Start();
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
}