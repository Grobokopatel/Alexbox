using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexBox
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
            ClientSize = new Size(800, 450);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill
            };


            for (var i = 0; i < 10; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            var label = new Label
            {
                Text = "Заходите. Порт: 2313.3213.12312. IP: 34.2342.23423.42",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18),
            };
            table.Controls.Add(label, 0, 0);


            for(var i=0; i<8; ++i)
            {
                var label1 = new Label
                {
                    Text = "Место свободно",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("ComicSans", 18),
                };
                table.Controls.Add(label1, 0, i+1);
            }

            var button = new Button
            {
                Text = "Начать",
                Dock = DockStyle.Fill
            };

            table.Controls.Add(button, 0, 9);

            Controls.Add(table);
        }
    }
}
