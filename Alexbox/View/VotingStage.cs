using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Alexbox.Application.TelegramBot;

namespace Alexbox.View
{
    public class VotingStage : Stage
    {
        public VotingStage(IReadOnlyList<string> captions)
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            var count = captions.Count;

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
            var label = new Label
            {
                Dock = DockStyle.Fill,
                Text = captions[0],
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 30),
                BorderStyle = BorderStyle.FixedSingle
            };
            table.Controls.Add(label, 0, 0);

            for (var i = 0; i < count - 1; ++i)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
                label = new Label
                {
                    Text = captions[i + 1],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 30),
                    BorderStyle = BorderStyle.FixedSingle
                };
                table.Controls.Add(label, i + 1, 0);
            }

            ControlTable.Controls.Add(table);
            Load += (_, _) =>
            {
                foreach (var id in TelegramBot.CurrentGame.Players.Keys)
                {
                    TelegramBot.SendMessageWithButtonsToUser(id, Paragraph.Text, captions);
                }
            };
        }
    }
}