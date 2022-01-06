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
    public partial class PlayerScore : UserControl
    {
        public PlayerScore(string name, double score) : this(name, score.ToString())
        { }

        public PlayerScore(string name, string score)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightGray;
            var table = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(table);

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,1));
            for (var i = 0; i < 2; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 1));

            var nameLabel = new Label()
            {
                Text = name,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 70),
                Dock = DockStyle.Fill,
                AutoSize = true,
                
            };

            var scoreLabel = new Label()
            {
                Text = score,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 70),
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            table.Controls.Add(nameLabel, 0, 0);
            table.Controls.Add(scoreLabel, 0, 1);
        }
    }
}
