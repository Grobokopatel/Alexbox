using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Alexbox.Application.TelegramBot;
using Alexbox.Domain;

namespace Alexbox.View
{
    public sealed class StagePresenter : UserControl
    {

        public StagePresenter WithBackground(Image image)
        {
            BackgroundImage = image;
            return this;
        }

        public event Action AllTaskShown; 
        private readonly TableLayoutPanel _controlTable;
        private readonly CustomGame _game;
        private readonly Stage _stage;

        public StagePresenter(Stage stage, CustomGame game)
        {
            _stage = stage;
            _game = game;
            Dock = DockStyle.Fill;
            _controlTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            _controlTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            _controlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            Controls.Add(_controlTable);

            var paragraph = new Label
            {
                Text = stage.Paragraph,
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 30),
            };
            _controlTable.Controls.Add(paragraph /*, 0, 0*/);
            HandleRoundSubmits();
            HandleScores();
        }

        private void HandleScores()
        {
            if (!_stage.ShowScores)
                return;
            var players = new Queue<Player>(_game.Players.OrderByDescending(player => player.Score));
            var playerCount = players.Count;
            var tableSize = Math.Ceiling(Math.Sqrt(playerCount));
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill
            };
            for (var i = 0; i < tableSize; ++i)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            for (var i = 0; i < tableSize; ++i)
            {
                for (var j = 0; j < tableSize; ++j)
                {
                    var player = players.Dequeue();
                    table.Controls.Add(new PlayerScore(player.Name, player.Score), i, j);
                    if (players.Count == 0)
                        break;
                }

                if (players.Count == 0)
                    break;
            }

            _controlTable.Controls.Add(table);
        }

        private void HandleRoundSubmits()
        {
            if (_stage.ShowRoundSubmits)
                return;

            /*            var showRoundSubmits = _stage.ShowRoundSubmits.Value;
                        Func<Player, IReadOnlyList<string>> selector = showRoundSubmits == -1
                            ? player => player.GetSubmission(_game.CurrentRound).Values.ToList()
                            : player => player.GetSubmission(showRoundSubmits).Values.ToList();
            */
            var submits = new Queue<string[]>(TelegramBot.PlayersBySentTask
                .Select(kv => kv.Value.Select(player => player.Submissions
                                      .Last()[kv.Key])
                                      .ToArray()));

            var answersTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            var groupSize = submits.Peek().Length;
            var labels = new Label[groupSize];
            answersTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            for (var i = 0; i < groupSize; ++i)
            {
                answersTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
                labels[i] = new Label
                {
                    Text = submits.Dequeue()[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 30),
                    BorderStyle = BorderStyle.FixedSingle
                };
                answersTable.Controls.Add(labels[i], i, 0);
            }
            _controlTable.Controls.Add(answersTable);

            var timer = new Timer();
            timer.Interval = 6000;
            timer.Start();
            timer.Tick += (_, _) =>
            {
                if (submits.Count == 0)
                {
                    timer.Stop();
                    AllTaskShown?.Invoke();
                    
                }

                var group = submits.Dequeue();
                for (var i = 0; i < groupSize; ++i)
                    labels[i].Text = group[i];
            };
        }
    }
}