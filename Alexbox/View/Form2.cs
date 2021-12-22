using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class Form2 : Form
    {
        private readonly TableLayoutPanel table;
        public Form2()
        {
            InitializeComponent();
            ClientSize = new Size(900, 750);
            Text = "Лобби_2";

            table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            var label1 = new Label
            {
                Text = "\nСмехлыст",
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Helvetica", 30),
            };

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            table.Controls.Add(label1);

            var label2 = new Label
            {
                Text = "Правила",
                MaximumSize = new Size(Bounds.Width, 0),
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 16),
            };

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            table.Controls.Add(label2);

            Controls.Add(table);
        }
    }
}
