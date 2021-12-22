using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class VotingStage : Stage
    {
        private Label[] labels;
        public VotingStage(string[] captions)
        {
            var table = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Percent,1));

            var count = captions.Length;
            labels = new Label[count];

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
            var label = new Label()
            {
                Dock = DockStyle.Fill,
                Text = captions[0],
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 30),
                BorderStyle = BorderStyle.FixedSingle
            };
            table.Controls.Add(label, 0, 0);

/*            var orLabel = new Label()
            {
                Text = "или",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 30),
                
            };
*/
            for (var i = 0; i < count-1; ++i)
            {
/*                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                table.Controls.Add(orLabel, i * 2 + 1, 0);
*/
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
                label = new Label()
                {
                    Text = captions[i+1],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 30),
                    BorderStyle = BorderStyle.FixedSingle
                };
                table.Controls.Add(label, i+1, 0);
            }

            ControlTable.Controls.Add(table);
        }
    }
}
