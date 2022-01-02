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

        private TableLayoutPanel controlTable;

        public StagePresenter(Stage stage)
        {
            Dock = DockStyle.Fill;
            controlTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            controlTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            controlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            Controls.Add(controlTable);

            var paragraph = new Label
            {
                Text = stage.Paragraph,
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 30),
            };
            controlTable.Controls.Add(paragraph /*, 0, 0*/);

            HandleCaptions(stage);
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
            controlTable.Controls.Add(answersTable);
            Load += (_, _) =>
            {
                foreach (var id in TelegramBot.CurrentGame.Players.Keys)
                {
                    TelegramBot.SendMessageWithButtonsToUser(id, stage.Paragraph, captions);
                }
            };
        }
    }
}