/*
using Alexbox.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class LobbyForm : Form
    {
        public Label[] playerLabels = new Label[8];
        private readonly Label label;

        public LobbyForm(LocalNetworkGame game)
        {
            InitializeComponent();
            ClientSize = new Size(900, 550);
            Text = "Лобби";
            this.game = game;
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            for (var i = 0; i < 9; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            label = new Label
            {
                Padding = new Padding() { Left = 5, Right = 5, Top = 2, Bottom = 2},
                Text = $"Заходите\nПорт: {game.MessageSender.Port}\nIP: в коммандной строке наберите ipconfig, найдите блок " +
                "\"Адаптер беспроводной локальной сети Беспроводная сеть\" и скопируйте IP из строки \"IPv4-адрес\"",
                MaximumSize = new Size(Bounds.Width, 0),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18),
            };

            Resize += LobbyForm_Resize;
            table.Controls.Add(label, 0, 0);

            for (var i = 0; i < 8; ++i)
            {
                var playerLabel = new Label
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "Место свободно",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("ComicSans", 18),
                };
                playerLabels[i] = playerLabel;
                table.Controls.Add(playerLabel, 0, i + 1);
            }

            game.PlayerLogin += ChangeLabel;

            var button = new Button
            {
                Text = "Начать",
                Dock = DockStyle.Fill
            };

            button.Click += Button_Click;
            table.Controls.Add(button, 0, 9);

            Controls.Add(table);
        }

        private void LobbyForm_Resize(object sender, EventArgs e)
        {
            label.MaximumSize = new Size(Bounds.Width,0);
        }

        private void Button_Click(object sender, EventArgs e) //убрать
        {
            ChangeLabel(this, new PlayerLoginEventArgs(new Player("Вы нажали на кнопку")));
        }

        private void ChangeLabel(object sender, PlayerLoginEventArgs e)
        {
            BeginInvoke(new Action(() => playerLabels
                .First(label => label.Text == "Место свободно")
                .Text = e.Player.Name));
        }
    }
}
*/
