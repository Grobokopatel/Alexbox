using System;
using System.Drawing;
using System.Linq;
using static Alexbox.Application.TelegramBot.TelegramBot;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class LobbyControl : UserControl
    {
        private static readonly Label[] PlayerLabels = new Label[CurrentGame.MaxPlayers];
        private readonly Label _viewersLabel;
        public readonly Button Button;

        public LobbyControl()
        {
            Dock = DockStyle.Fill;
            Text = "Лобби";

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            var gameNameLabel = new Label
            {
                Text = $"\n{CurrentGame.Name}\n",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 22),
            };

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var waitingLabel = new Label
            {
                Text = "\nОжидание игроков\n\n",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18),
            };

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            for (var i = 0; i < CurrentGame.MaxPlayers; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            table.Controls.Add(waitingLabel, 0, 1);
            table.Controls.Add(gameNameLabel, 0, 0);

            for (var i = 0; i < CurrentGame.MaxPlayers; ++i)
            {
                var playerLabel = new Label
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "Место свободно",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("ComicSans", 16),
                };
                PlayerLabels[i] = playerLabel;
                table.Controls.Add(playerLabel, 0, i + 2);
            }

            _viewersLabel = new Label
            {
                Text = $"Зрителей: {CurrentGame.Viewers.Count}",
                Dock = DockStyle.Fill,
                Size = new Size(0, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("ComicSans", 16),
            };

            Button = new Button
            {
                Text = "Начать",
                Dock = DockStyle.Fill,
                Font = new Font("ComicSans", 16),
                Size = new Size(0, 50)
            };
            table.Controls.Add(_viewersLabel, 0, 10);
            table.Controls.Add(Button, 0, 11);

            Controls.Add(table);

            var timer = new Timer
            {
                Interval = 100
            };
            timer.Tick += TimerTickHandle;
            timer.Start();
        }

        private void TimerTickHandle(object sender, EventArgs e)
        {
            for (var i = 0; i < CurrentGame.MaxPlayers; ++i)
            {
                if (i < CurrentGame.Players.Count)
                {
                    PlayerLabels[i].Text =
                        $"{i + 1} - {CurrentGame.Players.Values.Select(player => player.Name).ToList()[i]}";
                }
                else
                {
                    PlayerLabels[i].Text = $"{i + 1} - Место свободно";
                }

                _viewersLabel.Text = $"Зрителей: {CurrentGame.Viewers.Count}";
            }
        }
    }
}