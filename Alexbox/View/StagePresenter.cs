using System;
using System.Collections.Generic;
using System.Drawing;
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

        private readonly TableLayoutPanel _controlTable;
        private readonly CustomGame _game;

        public StagePresenter(Stage stage, CustomGame game)
        {
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

            HandleCaptions(stage);
            HandleRoundResults(stage);
        }

        private void HandleRoundResults(Stage stage)
        {
            if (!stage.ShowRoundResults)
                return;
            var players = new Queue<Player>(_game.Players);

            var playerCount = players.Count;
            var tableSize = Math.Ceiling(Math.Sqrt(playerCount));
            var table = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill
            };
            for(var i=0; i<tableSize; ++i)
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

        private void HandleCaptions(Stage stage)
        {
            var captions = stage.Captions;
            if (captions is null)
                return;
            var answersTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            answersTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            //answersTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
            for (var i = 0; i < captions.Length; ++i)
            {
                answersTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
                var label = new Label
                {
                    Text = captions[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 30),
                    BorderStyle = BorderStyle.FixedSingle
                };
                answersTable.Controls.Add(label, i, 0);
            }

            //controlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 1));
            _controlTable.Controls.Add(answersTable);
        }
    }
}