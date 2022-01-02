using System;
using System.Drawing;
using System.Linq;
using static Alexbox.Application.TelegramBot.TelegramBot;
using System.Windows.Forms;

namespace Alexbox.View
{
    public sealed class LobbyControl : UserControl
    {
        private static readonly Label[] PlayerLabels = new Label[CurrentGame.MaxPlayers];
        private readonly Label _viewersLabel;
        public readonly Button Button;

        public LobbyControl()
        {
            Dock = DockStyle.Fill;
            var controlTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            controlTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var gameNameLabel = new Label
            {
                Text = $"\n{CurrentGame.Name}\n",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 22),
            };
            
            var waitingLabel = new Label
            {
                Text = "\nОжидание игроков\n\n",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18),
            };

            controlTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            for (var i = 0; i < CurrentGame.MaxPlayers; ++i)
                controlTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            controlTable.Controls.Add(waitingLabel, 0, 1);
            controlTable.Controls.Add(gameNameLabel, 0, 0);

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
                controlTable.Controls.Add(playerLabel, 0, i + 2);
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
            controlTable.Controls.Add(_viewersLabel, 0, 10);
            controlTable.Controls.Add(Button, 0, 11);

            Controls.Add(controlTable);

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