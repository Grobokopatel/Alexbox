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
        public event Action AllTaskShown;
        private readonly TableLayoutPanel _controlTable;
        private readonly CustomGame _game;
        private readonly Stage _stage;
        private readonly Label _paragraph;
        private readonly Label _timer;

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
            _controlTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(_controlTable);

            var timerAndParagraph = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            timerAndParagraph.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            timerAndParagraph.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            timerAndParagraph.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _timer = new Label
            {
                Padding = new Padding() { Left = 2, Right = 2, Top = 5, Bottom = 5 },
                Text = (_stage.TimeOutInMs/1000).ToString(),
                Dock = DockStyle.Top,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 30),
            };
            if (_stage.TimeOutInMs != 0)
            {
                var timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += (_, _) =>
                {
                    var next = int.Parse(_timer.Text) - 1;
                    _timer.Text = next.ToString();
                    if (next == 0)
                        timer.Stop();
                };
                timer.Start();
            }
            _paragraph = new Label
            {
                Padding = new Padding() { Left = 5, Right = 5, Top = 2, Bottom = 2 },
                Text = stage.Paragraph,
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 30),
            };
            timerAndParagraph.Controls.Add(_timer, 0, 0);
            timerAndParagraph.Controls.Add(_paragraph, 1, 0);
            _controlTable.Controls.Add(timerAndParagraph /*, 0, 0*/);
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

        private void ShowNextAnswers(IReadOnlyList<Label> labels, int groupSize)
        {
            _game.LastVoteId = new List<long>();
            foreach (var player in _game.Players)
            {
                player.LastRoundVotes = 0;
            }

            var task = _game.PlayersBySentTask.First().Key;
            var group = _game.Players.Where(player => player.Submissions[_game.CurrentRound].ContainsKey(task))
                .Select(player => player.GetSubmission(_game.CurrentRound, task)).ToList();
            for (var i = 0; i < groupSize; ++i)
                labels[i].Text = group[i];
            _paragraph.Text = task.Description;
            _game.PlayersToVote = new Dictionary<Task, List<Player>>
            {
                [task] = _game.PlayersBySentTask[task]
            };
            foreach (var player in _game.Players.Where(player =>
                !player.Submissions[_game.CurrentRound].ContainsKey(task)))
            {
                TelegramBot.SendMessageWithButtonsToUser(player.Id, task.Description,
                    _game.PlayersBySentTask[task].Select(p => p.GetSubmission(_game.CurrentRound, task)));
            }

            foreach (var viewer in _game.Viewers)
            {
                TelegramBot.SendMessageWithButtonsToUser(viewer.Id, task.Description,
                    _game.PlayersBySentTask[task].Select(p => p.GetSubmission(_game.CurrentRound, task)));
            }

            _game.PlayersBySentTask.Remove(task);
        }

        private void HandleRoundSubmits()
        {
            if (!_stage.ShowRoundSubmits)
                return;

            var answersTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            var groupSize = _game.PlayersBySentTask.First().Value.Count;
            var labels = new Label[groupSize];
            answersTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            for (var i = 0; i < groupSize; ++i)
            {
                answersTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
                labels[i] = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 30),
                    BorderStyle = BorderStyle.FixedSingle
                };
                answersTable.Controls.Add(labels[i], i, 0);
            }
            ShowNextAnswers(labels, groupSize);
            _controlTable.Controls.Add(answersTable);
            var timer = new Timer();
            timer.Interval = 15000;
            timer.Start();
            timer.Tick += (_, _) =>
            {
                foreach (var player in _game.Players)
                {
                    try
                    {
                        player.Score +=
                            _game.CurrentStage.ScoreFormula(player.LastRoundVotes, _game.LastVoteId.Count, 100);
                    }
                    catch (DivideByZeroException)
                    {

                    }
                }
                if (_game.PlayersBySentTask.Keys.Count == 0)
                {
                    timer.Stop();
                    AllTaskShown?.Invoke();
                }
                else ShowNextAnswers(labels, groupSize);
            };
        }
    }
}