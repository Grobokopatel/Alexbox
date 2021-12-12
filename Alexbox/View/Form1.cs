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

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            for (var i = 0; i < 9; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            var label1 = new Label
            {
                Text = "Смехлыст\n",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 20),
            };

            Console.WriteLine("l");

            var label2 = new Label
            {
                Text = "\nОжидание игроков...\n\n",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18),
            };

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

            var label3 = new Label
            {
                Dock = DockStyle.Fill,
                Size = new Size(0, 50),
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

            var timer = new Timer()
            {
                Interval = 100
            };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        public static void UpdatePlayers(List<Player> players)
        {
            for (var i = 0; i < 8; ++i)
            {
                playerLabels[i].Text = players[i].Name;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            for (var i = 0; i < 8; ++i)
            {
                try
                {
                    playerLabels[i].Text = Alexbox.Application.TelegramBot.TelegramBot.CurrentGame._players[i].Name;
                }
                catch
                {

                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            // запуск игры
        }
    }
}
