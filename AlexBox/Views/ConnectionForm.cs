﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexBox.Views
{
    public partial class ConnectionForm : Form
    {
        private MaskedTextBox nameTextBox;
        private MaskedTextBox ipTextBox;
        private MaskedTextBox portTextBox;
        private Label connectionResultLabel;
        private LocalNetworkGame game;
        public ConnectionForm(LocalNetworkGame game)
        {
            InitializeComponent();
            this.game = game;
            Text = "Подключение";
            var connectionInfoTable = new TableLayoutPanel()
            {
                Dock = DockStyle.Bottom

            };
            connectionInfoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            connectionInfoTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            for (var i = 0; i < 3; ++i)
                connectionInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));

            var ipLable = new Label()
            {
                Text = "IP",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                TextAlign = ContentAlignment.BottomLeft
            };
            var portLable = new Label()
            {
                Text = "Порт",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                TextAlign = ContentAlignment.BottomLeft
            };
            var nameLable = new Label()
            {
                Text = "Имя",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                TextAlign = ContentAlignment.BottomLeft
            };

            ipTextBox = new MaskedTextBox()
            {
                Dock = DockStyle.Fill,
            };
            portTextBox = new MaskedTextBox()
            {
                Dock = DockStyle.Fill,
            };
            nameTextBox = new MaskedTextBox()
            {
                Dock = DockStyle.Fill
            };
            connectionInfoTable.Controls.Add(nameLable, 0, 0);
            connectionInfoTable.Controls.Add(ipLable, 1, 0);
            connectionInfoTable.Controls.Add(portLable, 2, 0);
            connectionInfoTable.Controls.Add(nameTextBox, 0, 1);
            connectionInfoTable.Controls.Add(ipTextBox, 1, 1);
            connectionInfoTable.Controls.Add(portTextBox, 2, 1);

            var connectButton = new Button()
            {
                Font = new Font("Arial", 20),
                Text = "Подключиться",
                Dock = DockStyle.Top,
                Height = 100
            };
            connectButton.Click += ConnectButtonAsync_Click;
            AcceptButton = connectButton;

            connectionResultLabel = new Label()
            {
                Font = new Font("Arial", 15),
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red,
            };
            var mainTable = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
            };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            mainTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));

            mainTable.Controls.Add(connectionInfoTable, 0, 0);
            mainTable.Controls.Add(connectionResultLabel, 0, 1);
            mainTable.Controls.Add(connectButton, 0, 2);

            Controls.Add(mainTable);
        }

        private async void ConnectButtonAsync_Click(object sender, EventArgs e)
        {
            var formatter = game.Formatter;
            try
            {
                var ip = ipTextBox.Text;
                var port = int.Parse(portTextBox.Text);
                var result = await game.MessageSender.SendAsync(ip, port, formatter.Serialize(nameTextBox.Text));
                connectionResultLabel.Text = formatter.Deserialize<string>(result);
            }
            catch (SystemException exception)
            {
                connectionResultLabel.Text = exception.Message;
            }
        }
    }
}
