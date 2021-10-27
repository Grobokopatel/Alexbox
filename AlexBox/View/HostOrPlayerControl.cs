﻿using AlexBox.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlexBox.View
{
    public partial class HostOrPlayerControl : UserControl
    {
        private LocalNetworkGame game;
        public HostOrPlayerControl(LocalNetworkGame game)
        {
            InitializeComponent();
            this.game = game;
            var table = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
            };
            for (var i = 0; i < 4; ++i)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            for (var i = 0; i < 3; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            for (var i = 0; i < 4; ++i)
                for (var j = 0; j < 3; ++j)
                    if (!(i == 1 && j == 1 || i == 2 && j == 1 || i == 1 && j == 0))
                        table.Controls.Add(new Panel(), i, j);

            var label = new Label()
            {
                Text = "Кем ты хочешь быть?",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomCenter
            };

            var hostButton = new Button()
            {
                Text = "Хостом",
                Dock = DockStyle.Fill,
                BackColor = Color.AliceBlue
            };
            hostButton.Click += HostButton_Click;

            var playerButton = new Button()
            {
                Text = "Игроком",
                Dock = DockStyle.Fill,
                BackColor = Color.AliceBlue
            };
            playerButton.Click += PlayerButton_Click;

            table.Controls.Add(hostButton, 1, 1);
            table.Controls.Add(playerButton, 2, 1);
            table.Controls.Add(label, 1, 0);

            Dock = DockStyle.Fill;
            Controls.Add(table);
        }

        private void PlayerButton_Click(object sender, EventArgs e)
        {
            var connectionForm = new ConnectionForm(game);

            connectionForm.Show();
        }

        private void HostButton_Click(object sender, EventArgs e)
        {
            game.MessageSender.StartRecievingMessagesAsync();
            var hostForm = new LobbyForm(game);

            hostForm.Show();
        }
    }
}