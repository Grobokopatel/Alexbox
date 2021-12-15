using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Alexbox.Application.TelegramBot.TelegramBot;
using System.Windows.Forms;
using Alexbox.Domain;

namespace Alexbox.View
{
    public partial class Form1 : Form
    {
        public static readonly Label[] playerLabels = new Label[8];
        private readonly TableLayoutPanel table;
        public readonly Timer timer_viewers; 
        private readonly Timer timer_players;
        private readonly Label label3;

        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(900, 750);
            Text = "Лобби";

            table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            var label1 = new Label
            {
                Text = "\nСмехлыст\n",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 22),
            };

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var label2 = new Label
            {
                Text = "\nОжидание игроков\n\n",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18),
            };

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            for (var i = 0; i < 8; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            table.Controls.Add(label2, 0, 1);
            table.Controls.Add(label1, 0, 0);

            for (var i = 0; i < 8; ++i)
            {
                var playerLabel = new Label
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "Место свободно",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("ComicSans", 16),
                };
                playerLabels[i] = playerLabel;
                table.Controls.Add(playerLabel, 0, i + 2);
            }

            label3 = new Label
            {
                Text = $"Зрителей: {CurrentGame._viewers.Count}",
                Dock = DockStyle.Fill,
                Size = new Size(0, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("ComicSans", 16),
            };

            var button = new Button
            {
                Text = "Начать",
                Dock = DockStyle.Fill,
                Size = new Size(0, 50),
                Font = new Font("ComicSans", 16),
            };
            table.Controls.Add(label3, 0, 10);
            table.Controls.Add(button, 0, 11);

            Controls.Add(table);

            button.Click += Button_Click;

            timer_viewers = new Timer()
            {
                Interval = 100
            };
            timer_viewers.Tick += new EventHandler(Timer_Tick_Players);

            timer_players = new Timer()
            {
                Interval = 100
            };
            timer_players.Tick += new EventHandler(Timer_Tick_Viewers);

            timer_players.Start();
            timer_viewers.Start();

        }
        void Timer_Tick_Players(object sender, EventArgs e)
        {
            for (var i = 0; i < 8; ++i)
            {
                try
                {
                    playerLabels[i].Text = $"{i + 1} - {CurrentGame._players[i].Name}";
                }
                catch
                {
                    playerLabels[i].Text = $"{i + 1} - Место свободно";
                }
            }
        }

        void Timer_Tick_Viewers(object sender, EventArgs e)
        {
            label3.Text = $"Зрителей: {CurrentGame._viewers.Count}";
        }

        private void Button_Click(object sender, EventArgs e)
        {
            timer_players.Stop();

            ActiveForm.Hide();
            Form2 Form2 = new();
            Form2.ShowDialog();
            Close();

            // здесь переключение на вторую форму и надо расписать начало игры
        }
    }
}
